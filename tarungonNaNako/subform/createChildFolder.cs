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
        public int ParentCategoryId { get; set; }
        public createChildFolder()
        {
            InitializeComponent();
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
                MessageBox.Show("Please enter a folder name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = "server=localhost; user=root; Database=docsmanagement; password=";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Step 1: Fetch the roleId of the current user
                    string roleQuery = "SELECT roleId FROM users WHERE userId = @userId";
                    int roleId = -1;  // Default value if no role is found
                    using (MySqlCommand roleCmd = new MySqlCommand(roleQuery, conn))
                    {
                        roleCmd.Parameters.AddWithValue("@userId", Session.CurrentUserId);
                        using (MySqlDataReader reader = roleCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                roleId = reader.GetInt32("roleId");  // Get the roleId for the current user
                            }
                        }
                    }

                    if (roleId == -1)
                    {
                        MessageBox.Show("Error: User role not found.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string query = @"
                INSERT INTO category 
                    (categoryName, parentCategoryId, is_archived, created_at, updated_at, userId, uploadedBy)
                VALUES 
                    (@categoryName, @parentCategoryId, 0, NOW(), NOW(), @userId, @uploadedBy);";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@categoryName", folderName);
                        cmd.Parameters.AddWithValue("@parentCategoryId", ParentCategoryId);
                        cmd.Parameters.AddWithValue("@userId", Session.CurrentUserId);
                        cmd.Parameters.AddWithValue("@uploadedBy", roleId);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Folder created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error creating folder: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void CreatefolderTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
