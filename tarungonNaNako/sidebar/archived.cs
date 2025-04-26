using Guna.UI2.WinForms;
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

namespace tarungonNaNako.sidebar
{
    public partial class archived : Form
    {
        public archived()
        {
            InitializeComponent();
            CustomizeDataGridView1();
            CustomizeDataGridView2();

        }

        private void CustomizeDataGridView2()
        {
            dataGridView2.EnableHeadersVisualStyles = false;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.BorderStyle = BorderStyle.Fixed3D;
            dataGridView2.GridColor = Color.White;
            dataGridView2.ReadOnly = true; // Set to readonly
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.AllowUserToDeleteRows = false;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None; // Disable auto size columns mode
            dataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None; // Disable auto size rows mode
            dataGridView2.ScrollBars = ScrollBars.Both; // Enable both horizontal and vertical scrollbars
            //guna2DataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView2.ColumnHeadersDefaultCellStyle.Font = new Font(dataGridView1.Font.FontFamily, 10, FontStyle.Bold); // Change font size
            dataGridView2.RowHeadersDefaultCellStyle.BackColor = Color.Navy;
            dataGridView2.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            dataGridView2.DefaultCellStyle.BackColor = Color.Beige;
            dataGridView2.DefaultCellStyle.ForeColor = Color.Black;
            dataGridView2.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 195, 0);
            dataGridView2.DefaultCellStyle.SelectionForeColor = Color.Black;
            dataGridView2.RowTemplate.Height = 30;
            dataGridView2.AllowUserToResizeColumns = true; // Allow user to resize columns
            dataGridView2.AllowUserToResizeRows = true; // Allow user to resize rows
            dataGridView2.DefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Regular); // Change font size
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
        private void CustomizeDataGridView1()
        {
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.BorderStyle = BorderStyle.Fixed3D;
            dataGridView1.GridColor = Color.White;
            dataGridView1.ReadOnly = true; // Set to readonly
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None; // Disable auto size columns mode
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None; // Disable auto size rows mode
            dataGridView1.ScrollBars = ScrollBars.Both; // Enable both horizontal and vertical scrollbars
            //guna2DataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font(dataGridView1.Font.FontFamily, 10, FontStyle.Bold); // Change font size
            dataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.Navy;
            dataGridView1.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            dataGridView1.DefaultCellStyle.BackColor = Color.Beige;
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 195, 0);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;
            dataGridView1.RowTemplate.Height = 30;
            dataGridView1.AllowUserToResizeColumns = true; // Allow user to resize columns
            dataGridView1.AllowUserToResizeRows = true; // Allow user to resize rows
            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Regular); // Change font size
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void LoadArchivedUsers()
        {
            string connectionString = "server=localhost; user=root; Database=docsmanagement; password=";

            // Clear existing data in the DataGridView
            dataGridView3.Rows.Clear();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = @"
                        SELECT u.userId, CONCAT(u.firstName, ' ', u.lastName) AS fullName, r.roleName
                        FROM users u
                        INNER JOIN roles r ON u.roleId = r.roleId
                        WHERE u.is_hidden = 1 AND u.isArchived = 0"; // Select only archived users

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int userId = reader.GetInt32("userId");
                            string fullName = reader.GetString("fullName");
                            string roleName = reader.GetString("roleName");

                            // Add row to the DataGridView with Unarchive and Delete buttons
                            dataGridView3.Rows.Add(userId, fullName, roleName, "Unarchive", "Delete");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading archived users: " + ex.Message);
                }
            }
        }
        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the clicked cell is the Unarchive or Delete button
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == dataGridView3.Columns["unarchive1"].Index)
                {
                    int userId = Convert.ToInt32(dataGridView3.Rows[e.RowIndex].Cells["userId"].Value);

                    DialogResult result = MessageBox.Show("Are you sure you want to unarchive this user?", "Confirm Unarchive", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        UnarchiveUser(userId);
                    }
                }
                else if (e.ColumnIndex == dataGridView3.Columns["Delete"].Index)
                {
                    int userId = Convert.ToInt32(dataGridView3.Rows[e.RowIndex].Cells["userId"].Value);

                    DialogResult result = MessageBox.Show("Are you sure you want to delete this user?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        DeleteUser(userId);
                    }
                }
            }
        }


        private void LoadArchivedFolders()
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
                SELECT categoryName
                FROM category
                WHERE is_archived = 0 AND is_hidden = 1";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string categoryName = reader.GetString("categoryName");

                            // Add the folder name to the DataGridView
                            dataGridView1.Rows.Add(categoryName, "Unarchive");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading archived folders: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the clicked cell is the "Unarchive" button
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["Unarchive"].Index)
            {
                // Get the folder name from the clicked row
                string folderName = dataGridView1.Rows[e.RowIndex].Cells["categoryName"].Value.ToString();

                // Confirm the unarchive action
                DialogResult result = MessageBox.Show($"Are you sure you want to unarchive the folder '{folderName}'?",
                                                      "Confirm Unarchive",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    UnarchiveFolder(folderName);
                }
            }
        }

        private void UnarchiveFolder(string folderName)
        {
            string connectionString = "server=localhost; user=root; Database=docsmanagement; password=";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Update the is_hidden column to 0 for the specified folder
                    string query = "UPDATE category SET is_hidden = 0 WHERE categoryName = @categoryName";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@categoryName", folderName);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show($"Folder '{folderName}' successfully unarchived.",
                                        "Success",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);

                        // Reload the DataGridView to reflect changes
                        LoadArchivedFolders();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error unarchiving folder: " + ex.Message,
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
        }

        private void DeleteUser(int userId)
        {
            string connectionString = "server=localhost; user=root; Database=docsmanagement; password=";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = "UPDATE users SET isArchived = 1 WHERE userId = @userId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("User removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Reload the DataGridView
                        LoadArchivedUsers();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting user: " + ex.Message);
                }
            }
        }
        // Method to unarchive a user in the database
        private void UnarchiveUser(int userId)
        {
            string connectionString = "server=localhost; user=root; Database=docsmanagement; password=";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = "UPDATE users SET is_hidden = 0 WHERE userId = @userId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("User successfully unarchived.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadArchivedUsers();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error unarchiving user: " + ex.Message);
                }
            }
        }



        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        // Load archived users when the form loads
        private void archived_Load(object sender, EventArgs e)
        {

            LoadArchivedFolders();
            LoadArchivedUsers();
            LoadUnarchivedFiles();

            // Check the logged-in user's role
            string userRole = GetLoggedInUserRole();

            if (userRole == "Teacher")
            {
                // Hide panel5 if the user is a teacher
                panel5.Visible = false;
            }
            else if (userRole == "Admin")
            {
                // Show panel5 if the user is an admin
                panel5.Visible = true;
            }
        }


        private string GetLoggedInUserRole()
        {
            string connectionString = "server=localhost; user=root; database=docsmanagement; password=";
            string role = string.Empty;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Query to get the role name based on the logged-in user's username
                    string query = @"
                SELECT u.userId, r.roleName 
                FROM users u
                JOIN roles r ON u.roleId = r.roleId
                WHERE u.username = @username";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Replace 'Session.CurrentUsername' with the actual variable holding the logged-in user's username
                        cmd.Parameters.AddWithValue("@username", Session.CurrentUserName);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Retrieve the role name
                                role = reader.GetString("roleName");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving user role: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return role;
        }


        private void LoadUnarchivedFiles()
        {
            string connectionString = "server=localhost; user=root; database=docsmanagement; password=";

            // Clear existing data in the DataGridView
            dataGridView2.Rows.Clear();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = @"
                SELECT 
                    f.fileId,
                    f.fileName AS fileName,
                    c.categoryName AS category
                FROM 
                    files f
                LEFT JOIN 
                    category c ON f.categoryId = c.categoryId
                WHERE 
                    f.isArchived = 0
                    AND f.is_hidden = 1"; // Select only unarchived files

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int fileId = reader.GetInt32("fileId");
                            string fileName = reader.GetString("fileName");
                            string category = reader.GetString("category");

                            // Add row to the DataGridView with Unarchive and Delete buttons
                            dataGridView2.Rows.Add(fileName, category, "Unarchive");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading unarchived files: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }




        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the clicked cell is the "Unarchive" button
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView2.Columns["unarchive2"].Index)
            {
                // Get the file name and category from the clicked row
                string fileName = dataGridView2.Rows[e.RowIndex].Cells["fileName"].Value.ToString();
                string category = dataGridView2.Rows[e.RowIndex].Cells["category"].Value.ToString();

                // Confirm the unarchive action
                DialogResult result = MessageBox.Show($"Are you sure you want to unarchive the file '{fileName}' in category '{category}'?",
                                                      "Confirm Unarchive",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Call the UnarchiveFile method
                    UnarchiveFile(fileName);
                }
            }
        }

        private void UnarchiveFile(string fileName)
        {
            string connectionString = "server=localhost; user=root; database=docsmanagement; password=";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Update the is_hidden column to 0 for the specified file
                    string query = "UPDATE files SET is_hidden = 0 WHERE fileName = @fileName";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@fileName", fileName);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show($"File '{fileName}' successfully unarchived.",
                                        "Success",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);

                        // Reload the DataGridView to reflect changes
                        LoadUnarchivedFiles();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error unarchiving file: " + ex.Message,
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
        }



        private void DeleteFile(int fileId)
        {
            string connectionString = "server=localhost; user=root; database=docsmanagement; password=";
            string filePath = string.Empty;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Retrieve the file path from the database
                    string query = "SELECT filePath FROM files WHERE fileId = @fileId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@fileId", fileId);
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            filePath = result.ToString();
                        }
                        else
                        {
                            MessageBox.Show("File not found in the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Delete the file from the file system if it exists
                    if (!string.IsNullOrEmpty(filePath) && System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    else
                    {
                        MessageBox.Show("File not found in the file system.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Delete the file record from the database
                    string deleteQuery = "DELETE FROM files WHERE fileId = @fileId";

                    using (MySqlCommand cmd = new MySqlCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@fileId", fileId);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("File successfully deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Reload the DataGridView
                    LoadUnarchivedFiles();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting file: " + ex.Message);
                }
            }
        }

    }
}
