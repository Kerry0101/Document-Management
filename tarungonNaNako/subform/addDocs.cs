using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using tarungonNaNako.sidebar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace tarungonNaNako.subform
{
    public partial class addDocs : Form
    {
        // Initialize OpenFileDialog
        OpenFileDialog openFileDialog = new OpenFileDialog();

        private string selectedFilePath = ""; // To store the selected file path
        private readonly string connectionString = "server=localhost;user=root;database=docsmanagement;password="; // DB connection

        public addDocs()
        {
            InitializeComponent();
            LoadCategories(); // Load categories into comboBox1
            searchBar.TextChanged += searchBar_TextChanged; // Add event handler for searchBar
        }
        private void addDocs_Load(object sender, EventArgs e)
        {
            // Usage example:
            guna2PictureBox1.Image = SetImageOpacity(guna2PictureBox1.Image, 0.5f); // 50% opacity
        }

        private void searchBar_TextChanged(object sender, EventArgs e)
        {
            string searchText = searchBar.Text.Trim();
            if (!string.IsNullOrEmpty(searchText))
            {
                FetchSearchSuggestions(searchText);
            }
            else
            {
                suggestionListBox.Visible = false;
            }
        }

        private void FetchSearchSuggestions(string searchText)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT categoryName AS name FROM category WHERE categoryName LIKE @searchText AND userId = @userId
                        UNION
                        SELECT subcategoryName AS name FROM subcategory WHERE subcategoryName LIKE @searchText AND userId = @userId";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@searchText", "%" + searchText + "%");
                        cmd.Parameters.AddWithValue("@userId", Session.CurrentUserId); // Assuming Session.CurrentUserId holds the logged-in user's ID
                        using (var reader = cmd.ExecuteReader())
                        {
                            List<string> suggestions = new List<string>();
                            while (reader.Read())
                            {
                                suggestions.Add(reader["name"].ToString());
                            }
                            DisplaySuggestions(suggestions);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching search suggestions: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplaySuggestions(List<string> suggestions)
        {
            // Clear previous suggestions
            suggestionListBox.Items.Clear();
            if (suggestions.Count > 0)
            {
                suggestionListBox.Items.AddRange(suggestions.ToArray());
                suggestionListBox.Visible = true;
            }
            else
            {
                suggestionListBox.Visible = false;
            }
        }

        private void LoadCategories()
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Files (*.*)|*.*|Text Files (*.txt)|*.txt|Image Files (*.jpg;*.png)|*.jpg;*.png";
            openFileDialog.Title = "Select a File";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(openFileDialog.FileName))
                {
                    selectedFilePath = openFileDialog.FileName; // Assign to the class variable
                    textBox1.Text = Path.GetFileName(selectedFilePath);
                    MessageBox.Show($"Selected file path: {selectedFilePath}", "Debug Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No file was selected. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("File selection canceled.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }





        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedFilePath))
            {
                MessageBox.Show("File path is empty. Please select a file first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Use the new file name from textBox1, but make sure it's not empty
            string newFileName = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(newFileName))
            {
                MessageBox.Show("Please enter a new file name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Ensure the file extension is retained
            string fileExtension = Path.GetExtension(selectedFilePath);
            string fileName = newFileName + fileExtension; // Create the new file name with the original extension
            string filePath = Path.Combine("C:\\DocsManagement", fileName);
            int userId = Session.CurrentUserId; // Use the logged-in user's ID
            string fileType = fileExtension;
            long fileSize = new FileInfo(selectedFilePath).Length;

            // Extract category and subcategory from searchBar
            string selectedCategory = searchBar.Text.Trim();
            int categoryId = 0;
            int subcategoryId = 0;
            int roleId = 0;

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    // Fetch categoryId and subcategoryId based on the selectedCategory
                    string query = @"
                SELECT categoryId FROM category WHERE categoryName = @selectedCategory AND userId = @userId
                UNION
                SELECT subcategoryId FROM subcategory WHERE subcategoryName = @selectedCategory AND userId = @userId";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@selectedCategory", selectedCategory);
                        cmd.Parameters.AddWithValue("@userId", userId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                categoryId = reader.GetInt32(0);
                                if (reader.NextResult() && reader.Read())
                                {
                                    subcategoryId = reader.GetInt32(0);
                                }
                            }
                        }
                    }

                    // Fetch roleId based on the userId
                    string roleQuery = "SELECT roleId FROM users WHERE userId = @userId";
                    using (var roleCmd = new MySqlCommand(roleQuery, conn))
                    {
                        roleCmd.Parameters.AddWithValue("@userId", userId);
                        roleId = Convert.ToInt32(roleCmd.ExecuteScalar());
                    }
                }

                // Ensure the destination directory exists
                if (!Directory.Exists("C:\\DocsManagement"))
                {
                    Directory.CreateDirectory("C:\\DocsManagement");
                }

                // Copy the file to the destination with the new name
                File.Copy(selectedFilePath, filePath, true);
                MessageBox.Show($"File uploaded successfully to: {filePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Step 1: Insert the file data into the `files` table
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO files (fileName, filePath, categoryId, subcategoryId, uploadedBy, userId, uploadDate, fileType, fileSize) " +
                                   "VALUES (@fileName, @filePath, @categoryId, @subcategoryId, @uploadedBy, @userId, CURRENT_TIMESTAMP, @fileType, @fileSize)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@fileName", fileName);
                        cmd.Parameters.AddWithValue("@filePath", filePath);
                        cmd.Parameters.AddWithValue("@categoryId", categoryId);
                        cmd.Parameters.AddWithValue("@subcategoryId", subcategoryId == 0 ? DBNull.Value : subcategoryId);
                        cmd.Parameters.AddWithValue("@uploadedBy", roleId);
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@fileType", fileType);
                        cmd.Parameters.AddWithValue("@fileSize", fileSize);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("File uploaded and data saved to database successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("File upload succeeded, but data saving to database failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

                // Step 2: Insert the initial version into the `file_versions` table
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string insertVersionQuery = @"
                INSERT INTO file_versions (fileId, version, versionFilePath, uploadedBy, uploadDate)
                SELECT fileId, 1, @filePath, @uploadedBy, CURRENT_TIMESTAMP
                FROM files
                WHERE fileName = @fileName";

                    using (MySqlCommand cmd = new MySqlCommand(insertVersionQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@filePath", filePath);
                        cmd.Parameters.AddWithValue("@uploadedBy", roleId);
                        cmd.Parameters.AddWithValue("@fileName", fileName);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during file upload or database insertion: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        private Image SetImageOpacity(Image image, float opacity)
        {
            Bitmap bmp = new Bitmap(image.Width, image.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                ColorMatrix matrix = new ColorMatrix();
                matrix.Matrix33 = opacity; // Set opacity level (0.0 = fully transparent, 1.0 = fully opaque)

                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                g.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
            }
            return bmp;
        }



        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void suggestionListBox_Click(object sender, EventArgs e)
        {
            if (suggestionListBox.SelectedItem != null)
            {
                searchBar.Text = suggestionListBox.SelectedItem.ToString();
                suggestionListBox.Visible = false;
            }
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}
