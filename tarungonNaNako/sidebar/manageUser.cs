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
using tarungonNaNako.subform;

namespace tarungonNaNako.sidebar
{
    public partial class manageUser : Form
    {
        public manageUser()
        {
            InitializeComponent();
        }
        private int loggedInUserId; // Store logged-in admin's user ID
        public manageUser(int userId)
        {
            InitializeComponent();
            loggedInUserId = userId;
        }

        // Method to load the user data into the DataGridView
        private void LoadUserData()
        {
            string connectionString = "server=localhost; user=root; Database=docsmanagement; password=";

            // Clear existing data in the DataGridView
            dataGridView1.Rows.Clear();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = @"
                        SELECT u.userId, CONCAT(u.firstName, ' ', u.lastName) AS fullName, r.roleName
                        FROM users u
                        INNER JOIN roles r ON u.roleId = r.roleId
                        WHERE u.isArchived = 0 AND is_hidden = 0";  // Assuming "IsArchived" is a field indicating if the user is archived

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int userId = reader.GetInt32("userId");
                                string fullName = reader.GetString("fullName");
                                string roleName = reader.GetString("roleName");

                                // Add row to the DataGridView
                                dataGridView1.Rows.Add(userId, fullName, roleName, "Archive", "Edit");
                            }
                        }
                    }
                
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading users: " + ex.Message);
                }
            }
        
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the click is within a valid row and column
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Check if the "Archive" button column was clicked
                if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "Archive")
                {
                    try
                    {
                        // Get the User ID of the selected row
                        int userId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["userId"].Value);

                        // Check if the selected user is the currently logged-in admin
                        if (userId == loggedInUserId)
                        {
                            MessageBox.Show("You cannot archive the account that is currently using.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        // Confirm archive action
                        DialogResult dialogResult = MessageBox.Show(
                            "Are you sure you want to archive this user?",
                            "Confirm Archive",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question
                        );

                        if (dialogResult == DialogResult.Yes)
                        {
                            // Update the isArchived field in the database
                            string connectionString = "server=localhost; user=root; Database=docsmanagement; password=";

                            using (MySqlConnection conn = new MySqlConnection(connectionString))
                            {
                                conn.Open();

                                string query = "UPDATE users SET is_hidden = 1 WHERE userId = @userId";

                                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                                {
                                    cmd.Parameters.AddWithValue("@userId", userId);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            MessageBox.Show("User archived successfully!");

                            // Reload the data to reflect the changes
                            LoadUserData();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error archiving user: " + ex.Message);
                    }
                }
                else if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "Edit")
                {
                    // Handle the "Edit" button click here
                    try
                    {
                        int userId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["userId"].Value);
                        adminDashboard parentForm = this.ParentForm as adminDashboard;

                        if (parentForm != null)
                        {
                            addUser editForm = new addUser(userId);
                            editForm.StartPosition = FormStartPosition.Manual;
                            editForm.Location = new Point(663, 270);
                            editForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                            editForm.MinimizeBox = false;
                            editForm.MaximizeBox = false;
                            editForm.ShowDialog(this);
                            LoadUserData();
                        }
                        else
                        {
                            MessageBox.Show("Parent form not found.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error opening edit form: " + ex.Message);
                    }
                }
            }


            /*try
            {
                // Get the User ID of the selected row
                int userId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["userId"].Value);

                // Reference the parent form
                adminDashboard parentForm = this.ParentForm as adminDashboard;

                if (parentForm != null)
                {
                    // Open the AddUser form in Edit mode and pass the userId
                    addUser editForm = new addUser(userId);
                    parentForm.LoadFormInPanel(editForm); // Use the panel-loading method
                    //this.Close(); // Optionally close the current form
                }
                else
                {
                    MessageBox.Show("Parent form not found. Please ensure this form is opened from adminDashboard.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening edit form: " + ex.Message);
            }*/


        }

        private void button1_Click(object sender, EventArgs e)
        {
            adminDashboard parentForm = this.ParentForm as adminDashboard;

            if (parentForm != null)
            {
                // Use the LoadFormInPanel method from adminDashboard to load formRole
                addUser addUser = new addUser(); // Pass 'this'
                addUser.StartPosition = FormStartPosition.Manual;
                addUser.Location = new Point(663, 270);
                addUser.FormBorderStyle = FormBorderStyle.FixedDialog;
                addUser.MinimizeBox = false;
                addUser.MaximizeBox = false;
                addUser.ShowDialog(this);
                LoadUserData();

                //this.Close(); // Close the current form (optional, depending on behavior)
            }
            else
            {
                MessageBox.Show("Parent form not found. Please ensure addRole is opened from adminDashboard.");
            }
        }

        private void manageUser_Load(object sender, EventArgs e)
        {
            LoadUserData();
        }
    }
}
