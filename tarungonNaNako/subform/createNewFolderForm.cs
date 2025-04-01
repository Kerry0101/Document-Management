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

            if (string.IsNullOrEmpty(folderName))
            {
                MessageBox.Show("Folder name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Ensure the logged-in user's ID is available
            if (Session.CurrentUserId == 0)  // Assuming 0 means no logged-in user
            {
                MessageBox.Show("User is not logged in. Please log in again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connectionString = "server=localhost; user=root; Database=docsmanagement; password=";
            string query = "INSERT INTO category (categoryName, userId) VALUES (@categoryName, @userId)";  // Using userId

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@categoryName", folderName);
                command.Parameters.AddWithValue("@userId", Session.CurrentUserId);  // Assign logged-in user's ID

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Folder created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while creating the folder: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }




    }

}
