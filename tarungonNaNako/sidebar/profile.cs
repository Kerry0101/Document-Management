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
using System.IO;
using tarungonNaNako.subform;

namespace tarungonNaNako
{
    public partial class profile : Form
    {

        private string connectionString = "server=localhost;database=docsmanagement;uid=root;pwd=;";

        public profile()
        {
            InitializeComponent();
            AccessGrant.Visible = false;
            AccessDenied.Visible = false;
            profileUpdate.Visible = false;
            LoadUserDetails();
        }

        private void LoadUserDetails()
        {
            try
            {

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT firstName, lastName, username, password, mobileNumber FROM users WHERE userId = @userId";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", Session.CurrentUserId);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Assuming you have TextBox controls with these names
                                FirstName.Text = reader["firstName"].ToString();
                                LastName.Text = reader["lastName"].ToString();
                                Username.Text = reader["username"].ToString();
                                Password.Text = reader["password"].ToString();
                                MobileNumber.Text = reader["mobileNumber"].ToString();

                                // Make the password hidden by default
                                Password.PasswordChar = '●';
                                label6.Text = $"{reader["firstName"]}, {reader["lastName"]}";
                                CenterLabel(label6);
                            }
                            else
                            {
                                MessageBox.Show("User not found.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
        private void CenterLabel(Label label)
        {
            if (label.Parent != null)
            {
                // Center the label horizontally within its parent container
                label.Left = (label.Parent.Width - label.Width) / 2;
            }
        }



        private void guna2CustomGradientPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private async void ChangeProfileBtn_Click(object sender, EventArgs e)
        {
            // Create and configure the OpenFileDialog
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select Profile Picture";
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif"; // Allow only image files
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                // Show the dialog and check if the user selected a file
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Image originalImage = Image.FromFile(openFileDialog.FileName);
                        guna2CirclePictureBox1.Image = new Bitmap(originalImage, guna2CirclePictureBox1.Size);
                        profileUpdate.Visible = true;
                        await Task.Delay(3000);
                        profileUpdate.Visible = false;


                        // Optionally, save the selected image path or data for future use
                        // For example, you can save the path to the database or application settings
                    }
                    catch (Exception ex)
                    {
                        // Handle any errors that occur while loading the image
                        MessageBox.Show($"An error occurred while loading the image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }

        private async void Update_Click(object sender, EventArgs e)
        {
            // Show the password prompt dialog
            using (PasswordPrompt prompt = new PasswordPrompt())
            {
                prompt.StartPosition = FormStartPosition.Manual;
                prompt.Location = new Point(835, 430); 

                if (prompt.ShowDialog() == DialogResult.OK)
                {
                    // Validate the entered password
                    if (prompt.EnteredPassword == Password.Text) // Compare with the current password
                    {
                        // Modify the starting position of the EditUserAccount form
                        EditUserAccount editUserAccountForm = new EditUserAccount();
                        editUserAccountForm.StartPosition = FormStartPosition.Manual; // Set to Manual for custom positioning
                        editUserAccountForm.Location = new Point(835, 330); // Set the desired X and Y coordinates
                        editUserAccountForm.TopMost = true;
                        editUserAccountForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                        editUserAccountForm.MinimizeBox = false;
                        editUserAccountForm.MaximizeBox = false;
                        editUserAccountForm.ShowDialog(this);
                        LoadUserDetails();

                    }
                    else
                    {
                        // Show access denied feedback
                        AccessDenied.Visible = true;
                        await Task.Delay(2000);
                        AccessDenied.Visible = false;
                    }
                }
            }
        }


    }
}
