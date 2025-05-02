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

            // Input validation: More robust checks
            if (string.IsNullOrEmpty(folderName))
            {
                MessageBox.Show("Folder name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Sanitize the folder name to prevent directory traversal vulnerabilities.
            folderName = Path.GetInvalidFileNameChars().Aggregate(folderName, (current, c) => current.Replace(c, '_'));

            // Check for invalid characters in folder name. This prevents security vulnerabilities.
            if (System.IO.Path.GetInvalidPathChars().Any(folderName.Contains))
            {
                MessageBox.Show("Folder name contains invalid characters.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // User authentication: Consider a more secure approach than Session variable.
            int userId = Session.CurrentUserId;
            if (userId == 0)
            {
                MessageBox.Show("User is not logged in. Please log in again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Database connection: Use parameterized queries to prevent SQL injection.
            string connectionString = "server=localhost; user=root; Database=docsmanagement; password="; // **Never hardcode passwords in production! Use configuration files.**
            string folderQuery = "INSERT INTO category (categoryName, parentCategoryId, folderPath, userId, uploadedBy) VALUES (@categoryName, @parentCategoryId, @folderPath, @userId, @roleId)";
            string roleQuery = "SELECT roleId FROM users WHERE userId = @userId";
            string parentPathQuery = "SELECT folderPath FROM category WHERE categoryId = @parentCategoryId";
            string checkFolderQuery = "SELECT COUNT(*) FROM category WHERE folderPath = @folderPath";

            int parentCategoryId = -1; // Default to root-level folder
            string parentFolderPath = @"C:\DocsManagement"; // Base path for root-level folders
            string physicalPath = "";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Step 1: Fetch the roleId of the current user
                    int roleId = -1; // Default value if no role is found
                    using (MySqlCommand roleCmd = new MySqlCommand(roleQuery, connection))
                    {
                        roleCmd.Parameters.AddWithValue("@userId", userId);
                        using (MySqlDataReader reader = roleCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                roleId = reader.GetInt32("roleId"); // Get the roleId for the current user
                            }
                        }
                    }

                    if (roleId == -1)
                    {
                        MessageBox.Show("Error: User role not found.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Step 2: Determine if the folder is root-level or child
                    if (parentCategoryId != -1) // If parentCategoryId is set, fetch the parent's folder path
                    {
                        using (MySqlCommand parentPathCmd = new MySqlCommand(parentPathQuery, connection))
                        {
                            parentPathCmd.Parameters.AddWithValue("@parentCategoryId", parentCategoryId);
                            object result = parentPathCmd.ExecuteScalar();
                            if (result != null)
                            {
                                parentFolderPath = result.ToString();
                            }
                        }
                    }

                    // Step 3: Construct the physical path for the new folder
                    physicalPath = Path.Combine(parentFolderPath, folderName);

                    // Step 4: Check if the folder already exists and modify the name if necessary
                    int suffix = 1;
                    string originalPhysicalPath = physicalPath;
                    while (true)
                    {
                        using (MySqlCommand checkFolderCmd = new MySqlCommand(checkFolderQuery, connection))
                        {
                            checkFolderCmd.Parameters.AddWithValue("@folderPath", physicalPath);
                            int count = Convert.ToInt32(checkFolderCmd.ExecuteScalar());
                            if (count == 0 && !Directory.Exists(physicalPath))
                            {
                                break; // Folder name is unique
                            }
                        }

                        // Append a suffix to the folder name
                        physicalPath = $"{originalPhysicalPath} ({suffix++})";
                    }

                    // Step 5: Insert the folder into the database
                    using (MySqlCommand folderCmd = new MySqlCommand(folderQuery, connection))
                    {
                        folderCmd.Parameters.AddWithValue("@categoryName", Path.GetFileName(physicalPath));
                        folderCmd.Parameters.AddWithValue("@parentCategoryId", parentCategoryId == -1 ? (object)DBNull.Value : parentCategoryId);
                        folderCmd.Parameters.AddWithValue("@folderPath", physicalPath);
                        folderCmd.Parameters.AddWithValue("@userId", userId);
                        folderCmd.Parameters.AddWithValue("@roleId", roleId);

                        folderCmd.ExecuteNonQuery();
                    }

                    // Step 6: Create the physical folder after successful database insertion
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
