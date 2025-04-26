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
    public partial class UPLOAD_FILE_INSIDE_CHILD_FOLDER : Form
    {
        // Initialize OpenFileDialog
        OpenFileDialog openFileDialog = new OpenFileDialog();

        private string selectedFilePath = ""; // To store the selected file path
        private readonly string connectionString = "server=localhost;user=root;database=docsmanagement;password="; // DB connection
        private System.Windows.Forms.Timer fadeTimer;
        private string currentFolderPath; // To store the current folder path

        public int ParentCategoryId { get; internal set; }

        //private int currentCategoryId; // To store the category ID

        public UPLOAD_FILE_INSIDE_CHILD_FOLDER(string folderPath)
        {
            InitializeComponent();
            currentFolderPath = folderPath; // Set the current folder path
            this.Opacity = 10; // Start invisible
            fadeTimer = new System.Windows.Forms.Timer();
            fadeTimer.Interval = 50; // Speed of transition
            fadeTimer.Tick += FadeIn;
        }

        private void FadeIn(object sender, EventArgs e)
        {
            if (this.Opacity < 1)
                this.Opacity += 0.05; // Increase opacity
            else
                fadeTimer.Stop(); // Stop when fully visible
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Files (*.*)|*.*|Text Files (*.txt)|*.txt|Image Files (*.jpg;*.png)|*.jpg;*.png|PDF Files (*.pdf)|*.pdf|Word Documents (*.doc;*.docx)|*.doc;*.docx";
            openFileDialog.Title = "Select a File";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(openFileDialog.FileName) && File.Exists(openFileDialog.FileName))
                {
                    selectedFilePath = openFileDialog.FileName; // Assign to the class variable
                    textBox1.Text = Path.GetFileName(selectedFilePath);
                }
                else
                {
                    MessageBox.Show("The selected file does not exist. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("File selection canceled.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }



        private void btnSave_Click(object sender, EventArgs e)
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
            string filePath = Path.Combine(currentFolderPath, fileName);
            int userId = Session.CurrentUserId; // Use the logged-in user's ID
            int roleId = GetRoleIdFromUserId(userId); // Fetch the roleId based on userId
            string fileType = fileExtension;
            long fileSize = new FileInfo(selectedFilePath).Length;

            try
            {
                // Ensure the destination directory exists
                if (!Directory.Exists(currentFolderPath))
                {
                    Directory.CreateDirectory(currentFolderPath);
                }

                // Log the current folder path for debugging
                Console.WriteLine($"Current Folder Path: {currentFolderPath}");

                // Copy the file to the destination with the new name
                if (!File.Exists(filePath)) // Avoid overwriting existing files
                {
                    File.Copy(selectedFilePath, filePath, true);
                    Console.WriteLine($"File copied to: {filePath}");
                }
                else
                {
                    MessageBox.Show("A file with the same name already exists in the destination folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Insert file metadata into the database
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO files (fileName, filePath, uploadedBy, userId, uploadDate, fileType, fileSize, categoryId) " +
                                   "VALUES (@fileName, @filePath, @uploadedBy, @userId, CURRENT_TIMESTAMP, @fileType, @fileSize, @categoryId)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@fileName", fileName);
                        cmd.Parameters.AddWithValue("@filePath", filePath);
                        cmd.Parameters.AddWithValue("@categoryId", ParentCategoryId); // Use ParentCategoryId here
                        cmd.Parameters.AddWithValue("@uploadedBy", roleId); // Use roleId here
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@fileType", fileType);
                        cmd.Parameters.AddWithValue("@fileSize", fileSize);
                        cmd.ExecuteNonQuery();
                    }
                }

                // Insert file version into the database
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
                        cmd.Parameters.AddWithValue("@uploadedBy", roleId); // Use roleId here
                        cmd.Parameters.AddWithValue("@fileName", fileName);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show($"File uploaded successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during file upload or database insertion: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }




        private int GetRoleIdFromUserId(int userId)
        {
            // Implement this method to fetch the roleId based on userId
            // This is a placeholder implementation
            int roleId = 0;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT roleId FROM users WHERE userId = @userId";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    roleId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            return roleId;
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Hide the current form
            this.Hide();
        }

        private void UPLOAD_FILE_INSIDE_CHILD_FOLDER_Load(object sender, EventArgs e)
        {
            // Usage example:
            guna2PictureBox1.Image = SetImageOpacity(guna2PictureBox1.Image, 0.9f); // 50% opacity
        }
    }
}

