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

namespace tarungonNaNako.subform
{
    public partial class createNewFolderForm : Form
    {
        private System.Windows.Forms.Timer fadeTimer;
        private TextBox txtFolderName;

        public string? FolderName { get; private set; }

        public createNewFolderForm()
        {
            InitializeComponent();
            this.Opacity = 10; // Start invisible
            fadeTimer = new System.Windows.Forms.Timer();
            fadeTimer.Interval = 50; // Speed of transition
            fadeTimer.Tick += FadeIn;
        }
        private void NewFolderForm_Load(object sender, EventArgs e)
        {
            fadeTimer.Start(); // Start fade-in animation on load
        }

        private void FadeIn(object sender, EventArgs e)
        {
            if (this.Opacity < 1)
                this.Opacity += 0.05; // Increase opacity
            else
                fadeTimer.Stop(); // Stop when fully visible
        }

        private void Cancelbtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void Createbtn_Click(object sender, EventArgs e)
        {
            string folderName = CreatefolderTextBox.Text.Trim();

            // Input validation:  More robust checks
            if (string.IsNullOrEmpty(folderName))
            {
                MessageBox.Show("Folder name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Sanitize the folder name to prevent directory traversal vulnerabilities.
            folderName = Path.GetInvalidFileNameChars().Aggregate(folderName, (current, c) => current.Replace(c, '_'));


            //Check for invalid characters in folder name.  This prevents security vulnerabilities.

            if (System.IO.Path.GetInvalidPathChars().Any(folderName.Contains))
            {
                MessageBox.Show("Folder name contains invalid characters.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            // User authentication:  Consider a more secure approach than Session variable.
            int userId = Session.CurrentUserId;
            if (userId == 0)
            {
                MessageBox.Show("User is not logged in. Please log in again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Database connection: Use parameterized queries to prevent SQL injection.
            string connectionString = "server=localhost; user=root; Database=docsmanagement; password="; // **Never hardcode passwords in production! Use configuration files.**
            string query = "INSERT INTO category (categoryName, parentCategoryId, userId) VALUES (@categoryName, NULL, @userId)";

            string physicalPath = Path.Combine(@"C:\Your\Base\Folder\Path", folderName); //Specify your base path here.

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@categoryName", folderName);
                command.Parameters.AddWithValue("@userId", userId);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();

                    // Create the physical folder after successful database insertion.
                    Directory.CreateDirectory(physicalPath);

                    MessageBox.Show("Folder created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (MySqlException ex)
                {
                    // More specific error handling
                    if (ex.Number == 1062) // Duplicate entry error
                    {
                        MessageBox.Show("A folder with that name already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (ex.Number == 1451) // Foreign key constraint error
                    {
                        MessageBox.Show("Error creating folder. Please check database constraints.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("An error occurred while creating the folder in the database: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while creating the folder on the file system: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }




    }

}
