using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using tarungonNaNako.sidebar;

namespace tarungonNaNako.subform
{
    public partial class addUser : Form
    {
        private int userId; // Store the userId for editing
        public addUser()
        {
            InitializeComponent();
        }
        // Constructor for editing an existing user
        public addUser(int userId)
        {
            InitializeComponent();
            this.userId = userId; // Store the userId
        }
        // Load user details for editing
        private void LoadUserDetails()
        {
            string connectionString = "server=localhost; user=root; Database=docsmanagement; password=";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"
                    SELECT firstName, lastName, mobileNumber, username, roleId
                    FROM users
                    WHERE userId = @userId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                textBox1.Text = reader.GetString("firstName");
                                textBox2.Text = reader.GetString("lastName");
                                textBox3.Text = reader.GetString("mobileNumber");
                                textBox4.Text = reader.GetString("username");
                                int roleId = reader.GetInt32("roleId");

                                // Set the ComboBox selected value
                                comboBox1.SelectedValue = roleId;
                            }
                            /* if (reader.Read())
                             {
                                 textBox1.Text = reader.GetString("firstName");
                                 textBox2.Text = reader.GetString("lastName");
                                 textBox3.Text = reader.GetString("mobileNumber");
                                 textBox4.Text = reader.GetString("username");
                                 int roleId = reader.GetInt32("roleId");

                                 // Set the role in the ComboBox (assuming it's populated with roles)
                                 comboBox1.SelectedValue = roleId;
                             }*/
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading user details: " + ex.Message);
                }
            }
        }

        //why when i edit the user details, the current role of the user in the combobox is not displayed?


        // This method will load the roles into the ComboBox
        private void LoadRoles()
        {
            string connectionString = "server=localhost; user=root; Database=docsmanagement; password=";
            comboBox1.Items.Clear();  // Clear any existing items in the ComboBox

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT roleId, roleName FROM roles";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Create a DataTable to bind the ComboBox
                            DataTable rolesTable = new DataTable();
                            rolesTable.Load(reader);

                            // Bind ComboBox with DataTable
                            comboBox1.DataSource = rolesTable;
                            comboBox1.DisplayMember = "roleName"; // Column to display
                            comboBox1.ValueMember = "roleId";   // Column to use as Value
                        }
                        /*{
                            while (reader.Read())
                            {
                                var roleItem = new ComboBoxItem
                                {
                                    Value = reader.GetInt32("roleId"),
                                    Text = reader.GetString("roleName")
                                };
                                comboBox1.Items.Add(roleItem);
                            }
                        }*/
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading roles: " + ex.Message);
                }
            }
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void addUser_Load(object sender, EventArgs e)
        {
            LoadRoles();  // Load roles into ComboBox when the form is loaded

            // If userId is provided (for editing), load the existing user data
            if (userId > 0)
            {
                LoadUserDetails();  // Load user details for editing
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            adminDashboard parentForm = this.ParentForm as adminDashboard;
            string firstName = textBox1.Text.Trim();
            string lastName = textBox2.Text.Trim();
            string mobileNumber = textBox3.Text.Trim();
            string username = textBox4.Text.Trim();
            string password = textBox5.Text.Trim();

            /*var selectedRole = (ComboBoxItem)comboBox1.SelectedItem;
            if (selectedRole == null)*/
            if (comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Please select a role.");
                return;
            }

            //int roleId = selectedRole.Value;
            int roleId = Convert.ToInt32(comboBox1.SelectedValue); // Use SelectedValue to get the roleId

            string connectionString = "server=localhost; user=root; Database=docsmanagement; password=";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    if (userId > 0) // Editing an existing user
                    {
                        string query;

                        if (string.IsNullOrWhiteSpace(password)) // If password is not changed
                        {
                            query = @"
                    UPDATE users
                    SET firstName = @firstName, lastName = @lastName, mobileNumber = @mobileNumber, 
                        username = @username, roleId = @roleId
                    WHERE userId = @userId";
                        }
                        else // If a new password is provided
                        {
                            query = @"
                    UPDATE users
                    SET firstName = @firstName, lastName = @lastName, mobileNumber = @mobileNumber, 
                        username = @username, roleId = @roleId, password = @password
                    WHERE userId = @userId";
                        }

                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@firstName", firstName);
                            cmd.Parameters.AddWithValue("@lastName", lastName);
                            cmd.Parameters.AddWithValue("@mobileNumber", mobileNumber);
                            cmd.Parameters.AddWithValue("@username", username);
                            cmd.Parameters.AddWithValue("@roleId", roleId);
                            cmd.Parameters.AddWithValue("@userId", userId);

                            if (!string.IsNullOrWhiteSpace(password))
                            {
                                cmd.Parameters.AddWithValue("@password", password);
                            }

                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("User details updated successfully!");
                    }
                    else // Adding a new user
                    {
                        string query = @"
                INSERT INTO users (username, password, roleId, firstName, lastName, mobileNumber)
                VALUES (@username, @password, @roleId, @firstName, @lastName, @mobileNumber)";

                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@username", username);
                            cmd.Parameters.AddWithValue("@password", password);
                            cmd.Parameters.AddWithValue("@roleId", roleId);
                            cmd.Parameters.AddWithValue("@firstName", firstName);
                            cmd.Parameters.AddWithValue("@lastName", lastName);
                            cmd.Parameters.AddWithValue("@mobileNumber", mobileNumber);

                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("User created successfully!");
                    }

                    if (parentForm != null)
                    {
                        parentForm.LoadFormInPanel(new manageUser()); // Reload the manageUser form
                    }
                    this.Close(); // Close the form after saving
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving user details: " + ex.Message);
                }
            }
        }


        /*private void button2_Click(object sender, EventArgs e)
        {
            adminDashboard parentForm = this.ParentForm as adminDashboard;
            string firstName = textBox1.Text.Trim();
            string lastName = textBox2.Text.Trim();
            string mobileNumber = textBox3.Text.Trim();
            string username = textBox4.Text.Trim();
            string password = textBox5.Text.Trim();

            var selectedRole = (ComboBoxItem)comboBox1.SelectedItem;
            if (selectedRole == null)
            {
                MessageBox.Show("Please select a role.");
                return;
            }

            int roleId = selectedRole.Value;

            string connectionString = "server=localhost; user=root; Database=docsmanagement; password=";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // If userId is provided, we are updating an existing user
                    if (userId > 0)
                    {
                        // Update the user in the database
                        string query = @"
                        UPDATE users
                        SET firstName = @firstName, lastName = @lastName, mobileNumber = @mobileNumber, 
                            username = @username, roleId = @roleId, password = @password
                        WHERE userId = @userId";

                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@firstName", firstName);
                            cmd.Parameters.AddWithValue("@lastName", lastName);
                            cmd.Parameters.AddWithValue("@mobileNumber", mobileNumber);
                            cmd.Parameters.AddWithValue("@password", password);
                            cmd.Parameters.AddWithValue("@username", username);
                            cmd.Parameters.AddWithValue("@roleId", roleId);
                            cmd.Parameters.AddWithValue("@userId", userId);

                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("User details updated successfully!");

                    }
                    else
                    {
                        // Insert a new user
                        string query = @"
                        INSERT INTO users (username, password, roleId, firstName, lastName, mobileNumber)
                        VALUES (@username, @password, @roleId, @firstName, @lastName, @mobileNumber)";

                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@username", username);
                            cmd.Parameters.AddWithValue("@password", password);  // Ensure password is hashed before inserting
                            cmd.Parameters.AddWithValue("@roleId", roleId);
                            cmd.Parameters.AddWithValue("@firstName", firstName);
                            cmd.Parameters.AddWithValue("@lastName", lastName);
                            cmd.Parameters.AddWithValue("@mobileNumber", mobileNumber);

                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("User created successfully!");
                    }

                    if (parentForm != null)
                    {
                        parentForm.LoadFormInPanel(new manageUser()); // Load the manageUser form in the panel
                    }
                    this.Close(); // Close the form after saving

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving user details: " + ex.Message);
                }
            }
        }
*/
        // Clear the form fields after saving
        private void ClearForm()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            comboBox1.SelectedIndex = -1;  // Reset ComboBox selection

        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
        public class ComboBoxItem
        {
            public int Value { get; set; }
            public string Text { get; set; }

            public override string ToString()
            {
                return Text;  // Display RoleName in ComboBox
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            adminDashboard parentForm = this.ParentForm as adminDashboard;

            if (parentForm != null)
            {
                // Use the LoadFormInPanel method from adminDashboard to load formRole
                parentForm.LoadFormInPanel(new manageUser());
                //this.Close(); // Close the current form (optional, depending on behavior)
            }
            else
            {
                MessageBox.Show("Parent form not found. Please ensure addRole is opened from adminDashboard.");
            }

        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }
    }
}
