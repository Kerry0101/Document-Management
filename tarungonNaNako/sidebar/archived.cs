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
        }
        // Method to load archived users into the DataGridView
        private void LoadArchivedUsers()
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
                        WHERE u.isArchived = 1"; // Select only archived users

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int userId = reader.GetInt32("userId");
                            string fullName = reader.GetString("fullName");
                            string roleName = reader.GetString("roleName");

                            // Add row to the DataGridView with Unarchive and Delete buttons
                            dataGridView1.Rows.Add(userId, fullName, roleName, "Unarchive", "Delete");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading archived users: " + ex.Message);
                }
            }
        }
        /*private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the clicked cell is the Unarchive button
            if (e.ColumnIndex == dataGridView1.Columns["Unarchive"].Index && e.RowIndex >= 0)
            {
                int userId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["userId"].Value);

                DialogResult result = MessageBox.Show("Are you sure you want to unarchive this user?", "Confirm Unarchive", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    UnarchiveUser(userId);
                }
            }
        }*/
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the clicked cell is the Unarchive or Delete button
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == dataGridView1.Columns["Unarchive"].Index)
                {
                    int userId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["userId"].Value);

                    DialogResult result = MessageBox.Show("Are you sure you want to unarchive this user?", "Confirm Unarchive", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        UnarchiveUser(userId);
                    }
                }
                else if (e.ColumnIndex == dataGridView1.Columns["Delete"].Index)
                {
                    int userId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["userId"].Value);

                    DialogResult result = MessageBox.Show("Are you sure you want to delete this user?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        DeleteUser(userId);
                    }
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

                    string query = "DELETE FROM users WHERE userId = @userId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("User successfully deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

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

                    string query = "UPDATE users SET isArchived = 0 WHERE userId = @userId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("User successfully unarchived.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Reload the DataGridView
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
            LoadArchivedUsers();
            LoadUnarchivedFiles();
        }

        
        private void LoadUnarchivedFiles()
        {
            string connectionString = "server=localhost; user=root; database=docsmanagement; password=";

            // Clear existing data in the DataGridView
            //dataGridView2.Rows.Clear();

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
                            f.isArchived = 0"; // Select only unarchived files

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int fileId = reader.GetInt32("fileId");
                            string fileName = reader.GetString("fileName");
                            string category = reader.GetString("category");

                            // Add row to the DataGridView with Unarchive and Delete buttons
                            dataGridView2.Rows.Add(fileName, category, "Unarchive", "Delete");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading unarchived files: " + ex.Message);
                }
            }
        }

       

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the clicked cell is the Unarchive or Delete button
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == dataGridView2.Columns["unarchive2"].Index)
                {
                    int fileId = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells["fileId"].Value);

                    DialogResult result = MessageBox.Show("Are you sure you want to unarchive this file?", "Confirm Unarchive", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        UnarchiveFile(fileId);
                    }
                }
                else if (e.ColumnIndex == dataGridView2.Columns["delete2"].Index)
                {
                    int fileId = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells["fileId"].Value);

                    DialogResult result = MessageBox.Show("Are you sure you want to delete this file?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        DeleteFile(fileId);
                    }
                }
            }
        }

        private void UnarchiveFile(int fileId)
        {
            string connectionString = "server=localhost; user=root; database=docsmanagement; password=";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = "UPDATE files SET isArchived = 0 WHERE fileId = @fileId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@fileId", fileId);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("File successfully unarchived.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Reload the DataGridView
                        LoadUnarchivedFiles();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error unarchiving file: " + ex.Message);
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
