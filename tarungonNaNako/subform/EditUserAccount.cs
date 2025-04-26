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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace tarungonNaNako.subform
{
    public partial class EditUserAccount : Form
    {
        private string connectionString = "server=localhost;database=docsmanagement;uid=root;pwd=;";
        public EditUserAccount()
        {
            InitializeComponent();
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

        private void EditUserAccount_Load(object sender, EventArgs e)
        {

        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Validate that no text fields are empty
            if (!ValidateFields())
            {
                return; // Stop execution if validation fails
            }

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "UPDATE users SET firstName = @firstName, lastName = @lastName, username = @username, password = @password, mobileNumber = @mobileNumber WHERE userId = @userId";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@firstName", FirstName.Text);
                        command.Parameters.AddWithValue("@lastName", LastName.Text);
                        command.Parameters.AddWithValue("@username", Username.Text);
                        command.Parameters.AddWithValue("@password", Password.Text);
                        command.Parameters.AddWithValue("@mobileNumber", MobileNumber.Text);
                        command.Parameters.AddWithValue("@userId", Session.CurrentUserId);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("User details updated successfully.");
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("No changes were made.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }


        private bool ValidateFields()
        {
            bool isValid = true;

            // Reset background colors to default  
            ResetFieldBorders();

            // Check each field and set background color to red if empty  
            if (string.IsNullOrWhiteSpace(FirstName.Text))
            {
                FirstName.BorderColor = Color.Red; // Highlight the field  
                FirstName.BackColor = Color.FromArgb(255, 226, 97);
                FirstName.PlaceholderText = "This field is required";
                FirstName.PlaceholderForeColor = Color.Red;
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(LastName.Text))
            {
                LastName.BorderColor = Color.Red; // Highlight the field  
                LastName.BackColor = Color.FromArgb(255, 226, 97);
                LastName.PlaceholderText = "This field is required";
                LastName.PlaceholderForeColor = Color.Red;
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(Username.Text))
            {
                Username.BorderColor = Color.Red; // Highlight the field  
                Username.BackColor = Color.FromArgb(255, 226, 97);
                Username.PlaceholderText = "This field is required";
                Username.PlaceholderForeColor = Color.Red;
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(Password.Text))
            {
                Password.BorderColor = Color.Red; // Highlight the field  
                Password.BackColor = Color.FromArgb(255, 226, 97);
                Password.PlaceholderText = "This field is required";
                Password.PlaceholderForeColor = Color.Red;
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(MobileNumber.Text))
            {
                MobileNumber.BorderColor = Color.Red; // Highlight the field  
                MobileNumber.BackColor = Color.FromArgb(255, 226, 97);
                MobileNumber.PlaceholderText = "This field is required";
                MobileNumber.PlaceholderForeColor = Color.Red;
                isValid = false;
            }
            else if (!MobileNumber.Text.All(char.IsDigit))
            {
                MobileNumber.BorderColor = Color.Red; // Highlight the field  
                MobileNumber.BackColor = Color.FromArgb(255, 226, 97);
                MobileNumber.PlaceholderText = "Only numbers are allowed";
                MobileNumber.PlaceholderForeColor = Color.Red;
                isValid = false;
            }

            return isValid;
        }

        private void ResetFieldBorders()
        {
            FirstName.BackColor = SystemColors.Window;
            LastName.BackColor = SystemColors.Window;
            Username.BackColor = SystemColors.Window;
            Password.BackColor = SystemColors.Window;
            MobileNumber.BackColor = SystemColors.Window;
        }

        private void FirstName_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(FirstName.Text))
            {
                FirstName.BorderColor = Color.FromArgb(213, 218, 223);
                FirstName.BackColor = Color.FromArgb(255, 226, 97);
                FirstName.PlaceholderText = string.Empty; // Clear placeholder text
            }
        }

        private void LastName_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(LastName.Text))
            {
                LastName.BorderColor = Color.FromArgb(213, 218, 223);
                LastName.BackColor = Color.FromArgb(255, 226, 97);
                LastName.PlaceholderText = string.Empty; // Clear placeholder text
            }
        }

        private void Username_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Username.Text))
            {
                Username.BorderColor = Color.FromArgb(213, 218, 223);
                Username.BackColor = Color.FromArgb(255, 226, 97);
                Username.PlaceholderText = string.Empty; // Clear placeholder text
            }
        }

        private void Password_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Password.Text))
            {
                Password.BorderColor = Color.FromArgb(213, 218, 223);
                Password.BackColor = Color.FromArgb(255, 226, 97);
                Password.PlaceholderText = string.Empty; // Clear placeholder text
            }
        }

        private void MobileNumber_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(MobileNumber.Text))
            {
                MobileNumber.BorderColor = Color.FromArgb(213, 218, 223);
                MobileNumber.BackColor = Color.FromArgb(255, 226, 97);
                MobileNumber.PlaceholderText = string.Empty; // Clear placeholder text
            }
        }
    }
}
