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
    public partial class createChildFolder : Form
    {
        public int? ParentCategoryId { get; set; }
        private readonly string connectionString = "server=localhost;user=root;database=docsmanagement;password=";
        private readonly string baseStoragePath = @"C:\DocsManagement"; // Or your actual root storage
        public createChildFolder()
        {
            InitializeComponent();
        }

        private void Cancelbtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private bool IsCategoryNameExists(string categoryName, int? parentCategoryId)
        {
            if (string.IsNullOrWhiteSpace(categoryName)) return false;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT COUNT(*) FROM category
                                 WHERE categoryName = @categoryName
                                 AND (parentCategoryId = @parentCategoryId OR (@parentCategoryId IS NULL AND parentCategoryId IS NULL))
                                 AND is_archived = 0 AND userId = @userId";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@categoryName", categoryName);
                    cmd.Parameters.AddWithValue("@parentCategoryId", (object)parentCategoryId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@userId", Session.CurrentUserId);
                    try { return Convert.ToInt32(cmd.ExecuteScalar()) > 0; }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error checking for duplicate category name: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return true; // Assume duplicate on error
                    }
                }
            }
        }

        private void Createbtn_Click(object sender, EventArgs e)
        {
            string folderName = CreatefolderTextBox.Text.Trim();

            // --- Step 1: Validate Input ---
            if (string.IsNullOrEmpty(folderName))
            {
                MessageBox.Show("Please enter a folder name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CreatefolderTextBox.Focus();
                return;
            }

            // Validate for invalid characters
            if (folderName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 || folderName.Contains(".."))
            {
                MessageBox.Show("Invalid characters detected in the folder name. Please avoid characters like \\ / : * ? \" < > | and '..'.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CreatefolderTextBox.Focus();
                return;
            }

            // --- Step 2: Basic Setup ---
            if (!ParentCategoryId.HasValue)
            {
                MessageBox.Show("Error: Parent category information is missing.", "Internal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string parentFolderPath = "";
            int roleId = -1;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // --- Step 3: Get User Role ID ---
                    string roleQuery = "SELECT roleId FROM users WHERE userId = @userId";
                    using (MySqlCommand roleCmd = new MySqlCommand(roleQuery, conn))
                    {
                        roleCmd.Parameters.AddWithValue("@userId", Session.CurrentUserId);
                        object roleResult = roleCmd.ExecuteScalar();
                        if (roleResult != null && roleResult != DBNull.Value)
                        {
                            roleId = Convert.ToInt32(roleResult);
                        }
                    }
                    if (roleId == -1)
                    {
                        MessageBox.Show("Error: User role not found.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // --- Step 4: Determine Parent Folder Path ---
                    string parentPathQuery = "SELECT folderPath FROM category WHERE categoryId = @parentCategoryId AND userId = @userId";
                    using (MySqlCommand parentPathCmd = new MySqlCommand(parentPathQuery, conn))
                    {
                        parentPathCmd.Parameters.AddWithValue("@parentCategoryId", ParentCategoryId.Value);
                        parentPathCmd.Parameters.AddWithValue("@userId", Session.CurrentUserId);
                        object result = parentPathCmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            parentFolderPath = result.ToString();
                        }
                        else
                        {
                            parentFolderPath = baseStoragePath;
                        }
                    }

                    if (!Directory.Exists(parentFolderPath))
                    {
                        try { Directory.CreateDirectory(parentFolderPath); }
                        catch (Exception dirEx)
                        {
                            MessageBox.Show($"Could not create or access parent directory '{parentFolderPath}': {dirEx.Message}", "File System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // --- Step 5: Check for Existing Folder and Adjust Name ---
                    string newFolderPath = Path.Combine(parentFolderPath, folderName);
                    string originalFolderName = folderName;
                    int suffix = 1;

                    while (IsCategoryNameExists(folderName, ParentCategoryId) || Directory.Exists(newFolderPath))
                    {
                        folderName = $"{originalFolderName} ({suffix++})";
                        newFolderPath = Path.Combine(parentFolderPath, folderName);
                    }

                    // --- Step 6: Create Physical Folder ---
                    Directory.CreateDirectory(newFolderPath);

                    // --- Step 7: Insert into Database ---
                    string insertQuery = @"
            INSERT INTO category
                (categoryName, parentCategoryId, folderPath, is_archived, created_at, updated_at, userId, uploadedBy)
            VALUES
                (@categoryName, @parentCategoryId, @folderPath, 0, NOW(), NOW(), @userId, @uploadedBy);";

                    using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@categoryName", folderName);
                        insertCmd.Parameters.AddWithValue("@parentCategoryId", ParentCategoryId.Value);
                        insertCmd.Parameters.AddWithValue("@folderPath", newFolderPath);
                        insertCmd.Parameters.AddWithValue("@userId", Session.CurrentUserId);
                        insertCmd.Parameters.AddWithValue("@uploadedBy", roleId);

                        insertCmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Folder created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error creating folder: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }






        private void CreatefolderTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
