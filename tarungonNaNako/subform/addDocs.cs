using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            //try
            //{
            //    using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            //    {
            //        conn.Open();
            //        string query = "SELECT categoryId, categoryName FROM category";
            //        using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(query, conn))
            //        using (var reader = cmd.ExecuteReader())
            //        {
            //            DataTable dt = new DataTable();
            //            dt.Load(reader);

            //            comboBox1q.DataSource = dt;
            //            comboBox1q.DisplayMember = "categoryName";
            //            comboBox1q.ValueMember = "categoryId";
            //            comboBox1q.SelectedIndex = -1; // Default no selection
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error loading categories: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
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
            int uploadedBy = Session.CurrentUserId; // Use the logged-in user's ID
            string fileType = fileExtension;
            long fileSize = new FileInfo(selectedFilePath).Length;

            try
            {
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
                    string query = "INSERT INTO files (fileName, filePath, uploadedBy, uploadDate, fileType, fileSize) " +
                                   "VALUES (@fileName, @filePath, @uploadedBy, CURRENT_TIMESTAMP, @fileType, @fileSize)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@fileName", fileName);
                        cmd.Parameters.AddWithValue("@filePath", filePath);
                        cmd.Parameters.AddWithValue("@uploadedBy", uploadedBy);
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
                        cmd.Parameters.AddWithValue("@uploadedBy", uploadedBy);
                        cmd.Parameters.AddWithValue("@fileName", fileName);
                        cmd.ExecuteNonQuery();
                    }
                }

                // Optionally, navigate to another form or action after successful upload
                adminDashboard parentForm = this.ParentForm as adminDashboard;
                if (parentForm != null)
                {
                    parentForm.LoadFormInPanel(new manageDocs());
                }
                else
                {
                    MessageBox.Show("Parent form not found. Please ensure addRole is opened from adminDashboard.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during file upload or database insertion: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        //private void button1_Click(object sender, EventArgs e)
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog();
        //    openFileDialog.Filter = "All Files (*.*)|*.*|Text Files (*.txt)|*.txt|Image Files (*.jpg;*.png)|*.jpg;*.png";
        //    openFileDialog.Title = "Select a File";

        //    if (openFileDialog.ShowDialog() == DialogResult.OK)
        //    {
        //        if (!string.IsNullOrEmpty(openFileDialog.FileName))
        //        {
        //            selectedFilePath = openFileDialog.FileName; // Assign to the class variable
        //            textBox1.Text = Path.GetFileName(selectedFilePath);
        //            MessageBox.Show($"Selected file path: {selectedFilePath}", "Debug Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //        else
        //        {
        //            MessageBox.Show("No file was selected. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("File selection canceled.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //}


        //private void button2_Click(object sender, EventArgs e)
        //{
        //    //if (string.IsNullOrEmpty(selectedFilePath))
        //    //{
        //    //    MessageBox.Show("File path is empty. Please select a file first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //    //    return;
        //    //}

        //    //// Use the new file name from textBox1, but make sure it's not empty
        //    //string newFileName = textBox1.Text.Trim();
        //    //if (string.IsNullOrEmpty(newFileName))
        //    //{
        //    //    MessageBox.Show("Please enter a new file name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //    //    return;
        //    //}

        //    //// Ensure the file extension is retained
        //    //string fileExtension = Path.GetExtension(selectedFilePath);
        //    //string fileName = newFileName + fileExtension; // Create the new file name with the original extension
        //    //string filePath = Path.Combine("C:\\DocsManagement", fileName);
        //    //int categoryId = comboBox1q.SelectedValue != null ? Convert.ToInt32(comboBox1q.SelectedValue) : 0;
        //    //int subcategoryId = comboBox2.SelectedValue != null ? Convert.ToInt32(comboBox2.SelectedValue) : 0;
        //    //int uploadedBy = Session.CurrentUserId; // Use the logged-in user's ID
        //    //string fileType = fileExtension;
        //    //long fileSize = new FileInfo(selectedFilePath).Length;

        //    //try
        //    //{
        //    //    // Ensure the destination directory exists
        //    //    if (!Directory.Exists("C:\\DocsManagement"))
        //    //    {
        //    //        Directory.CreateDirectory("C:\\DocsManagement");
        //    //    }

        //    //    // Copy the file to the destination with the new name
        //    //    File.Copy(selectedFilePath, filePath, true);
        //    //    MessageBox.Show($"File uploaded successfully to: {filePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

        //    //    // Step 1: Insert the file data into the `files` table
        //    //    using (MySqlConnection conn = new MySqlConnection(connectionString))
        //    //    {
        //    //        conn.Open();
        //    //        string query = "INSERT INTO files (fileName, filePath, categoryId, subcategoryId, uploadedBy, uploadDate, fileType, fileSize) " +
        //    //                       "VALUES (@fileName, @filePath, @categoryId, @subcategoryId, @uploadedBy, CURRENT_TIMESTAMP, @fileType, @fileSize)";

        //    //        using (MySqlCommand cmd = new MySqlCommand(query, conn))
        //    //        {
        //    //            cmd.Parameters.AddWithValue("@fileName", fileName);
        //    //            cmd.Parameters.AddWithValue("@filePath", filePath);
        //    //            cmd.Parameters.AddWithValue("@categoryId", categoryId);
        //    //            cmd.Parameters.AddWithValue("@subcategoryId", subcategoryId == 0 ? DBNull.Value : subcategoryId);
        //    //            cmd.Parameters.AddWithValue("@uploadedBy", uploadedBy);
        //    //            cmd.Parameters.AddWithValue("@fileType", fileType);
        //    //            cmd.Parameters.AddWithValue("@fileSize", fileSize);

        //    //            int rowsAffected = cmd.ExecuteNonQuery();
        //    //            if (rowsAffected > 0)
        //    //            {
        //    //                MessageBox.Show("File uploaded and data saved to database successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    //            }
        //    //            else
        //    //            {
        //    //                MessageBox.Show("File upload succeeded, but data saving to database failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    //            }
        //    //        }
        //    //    }

        //    //    // Step 2: Insert the initial version into the `file_versions` table
        //    //    using (MySqlConnection conn = new MySqlConnection(connectionString))
        //    //    {
        //    //        conn.Open();
        //    //        string insertVersionQuery = @"
        //    //    INSERT INTO file_versions (fileId, version, versionFilePath, uploadedBy, uploadDate)
        //    //    SELECT fileId, 1, @filePath, @uploadedBy, CURRENT_TIMESTAMP
        //    //    FROM files
        //    //    WHERE fileName = @fileName";

        //    //        using (MySqlCommand cmd = new MySqlCommand(insertVersionQuery, conn))
        //    //        {
        //    //            cmd.Parameters.AddWithValue("@filePath", filePath);
        //    //            cmd.Parameters.AddWithValue("@uploadedBy", uploadedBy);
        //    //            cmd.Parameters.AddWithValue("@fileName", fileName);
        //    //            cmd.ExecuteNonQuery();
        //    //        }
        //    //    }

        //    //    // Optionally, navigate to another form or action after successful upload
        //    //    adminDashboard parentForm = this.ParentForm as adminDashboard;
        //    //    if (parentForm != null)
        //    //    {
        //    //        parentForm.LoadFormInPanel(new manageDocs());
        //    //    }
        //    //    else
        //    //    {
        //    //        MessageBox.Show("Parent form not found. Please ensure addRole is opened from adminDashboard.");
        //    //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    MessageBox.Show($"Error during file upload or database insertion: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    //}
        //}








        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        //private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (comboBox1q.SelectedIndex != -1)
        //    {
        //        DataRowView selectedRow = comboBox1q.SelectedItem as DataRowView;
        //        if (selectedRow != null)
        //        {
        //            int categoryId = Convert.ToInt32(selectedRow["categoryId"]); // Extract the categoryId
        //            LoadSubcategories(categoryId);
        //            comboBox2.Enabled = true;
        //        }
        //    }
        //    else
        //    {
        //        comboBox2.DataSource = null;
        //        comboBox2.Enabled = false;
        //    }
        //}
        //private void LoadSubcategories(int categoryId)
        //{
        //    try
        //    {
        //        using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
        //        {
        //            conn.Open();
        //            string query = "SELECT subcategoryId, subcategoryName FROM subcategory WHERE categoryId = @categoryId";
        //            using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(query, conn))
        //            {
        //                cmd.Parameters.AddWithValue("@categoryId", categoryId);
        //                using (var reader = cmd.ExecuteReader())
        //                {
        //                    DataTable dt = new DataTable();
        //                    dt.Load(reader);

        //                    comboBox2.DataSource = dt;
        //                    comboBox2.DisplayMember = "subcategoryName";
        //                    comboBox2.ValueMember = "subcategoryId";
        //                    comboBox2.SelectedIndex = -1; // Default no selection
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error loading subcategories: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            adminDashboard parentForm = this.ParentForm as adminDashboard;

            if (parentForm != null)
            {
                // Use the LoadFormInPanel method from adminDashboard to load formRole
                parentForm.LoadFormInPanel(new manageDocs());
                //this.Close(); // Close the current form (optional, depending on behavior)
            }
            else
            {
                MessageBox.Show("Parent form not found. Please ensure addRole is opened from adminDashboard.");
            }
        }

        private void suggestionListBox_Click(object sender, EventArgs e)
        {
            if (suggestionListBox.SelectedItem != null)
            {
                searchBar.Text = suggestionListBox.SelectedItem.ToString();
                suggestionListBox.Visible = false;
            }
        }
    }
}
