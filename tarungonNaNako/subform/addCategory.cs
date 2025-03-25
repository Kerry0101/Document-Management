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
    public partial class addCategory : Form
    {
        private int? categoryId; // Nullable to distinguish between Add and Edit mode

        public addCategory(int? categoryId = null)
        {
            InitializeComponent();
            this.categoryId = categoryId;

            if (categoryId.HasValue)
            {
                LoadCategoryDetails(categoryId.Value);
            }
        }

        private void LoadCategoryDetails(int categoryId)
        {
            string connectionString = "server=localhost; user=root; Database=docsmanagement; password=";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = @"
                        SELECT c.categoryName, 
                               p.canUploadByPrincipal, 
                               p.canUploadByTeacher, 
                               p.needsApproval
                        FROM category c
                        LEFT JOIN permissions p 
                        ON c.categoryId = p.referenceId 
                        WHERE c.categoryId = @categoryId AND p.type = 'category'";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@categoryId", categoryId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                textBox1.Text = reader.GetString("categoryName");
                                checkBox1.Checked = reader.GetBoolean("canUploadByPrincipal");
                                checkBox2.Checked = reader.GetBoolean("canUploadByTeacher");
                                checkBox3.Checked = reader.GetBoolean("needsApproval");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading category details: " + ex.Message);
                }
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string categoryName = textBox1.Text.Trim();
            bool canUploadByPrincipal = checkBox1.Checked;
            bool canUploadByTeacher = checkBox2.Checked;
            bool needsApproval = checkBox3.Checked;

            if (string.IsNullOrEmpty(categoryName))
            {
                MessageBox.Show("Category name cannot be empty.");
                return;
            }

            string connectionString = "server=localhost; user=root; Database=docsmanagement; password=";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    if (categoryId.HasValue) // Edit mode
                    {
                        // Update category and permissions
                        string updateCategoryQuery = "UPDATE category SET categoryName = @categoryName WHERE categoryId = @categoryId";
                        using (MySqlCommand cmd = new MySqlCommand(updateCategoryQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@categoryName", categoryName);
                            cmd.Parameters.AddWithValue("@categoryId", categoryId.Value);
                            cmd.ExecuteNonQuery();
                        }

                        string updatePermissionsQuery = @"
                            UPDATE permissions
                            SET canUploadByPrincipal = @canUploadByPrincipal,
                                canUploadByTeacher = @canUploadByTeacher,
                                needsApproval = @needsApproval
                            WHERE referenceId = @referenceId AND type = 'category'";
                        using (MySqlCommand cmd = new MySqlCommand(updatePermissionsQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@canUploadByPrincipal", canUploadByPrincipal);
                            cmd.Parameters.AddWithValue("@canUploadByTeacher", canUploadByTeacher);
                            cmd.Parameters.AddWithValue("@needsApproval", needsApproval);
                            cmd.Parameters.AddWithValue("@referenceId", categoryId.Value);
                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Category updated successfully!");
                    }
                    else // Add mode
                    {
                        // Add new category and permissions
                        string insertCategoryQuery = "INSERT INTO category (categoryName) VALUES (@categoryName)";
                        using (MySqlCommand cmd = new MySqlCommand(insertCategoryQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@categoryName", categoryName);
                            cmd.ExecuteNonQuery();
                        }

                        // Retrieve the last inserted ID
                        string getLastIdQuery = "SELECT LAST_INSERT_ID()";
                        long newCategoryId;
                        using (MySqlCommand cmd = new MySqlCommand(getLastIdQuery, conn))
                        {
                            newCategoryId = Convert.ToInt64(cmd.ExecuteScalar());
                        }

                        string insertPermissionsQuery = @"
                            INSERT INTO permissions (type, referenceId, canUploadByPrincipal, canUploadByTeacher, needsApproval)
                            VALUES ('category', @referenceId, @canUploadByPrincipal, @canUploadByTeacher, @needsApproval)";
                        using (MySqlCommand cmd = new MySqlCommand(insertPermissionsQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@referenceId", newCategoryId);
                            cmd.Parameters.AddWithValue("@canUploadByPrincipal", canUploadByPrincipal);
                            cmd.Parameters.AddWithValue("@canUploadByTeacher", canUploadByTeacher);
                            cmd.Parameters.AddWithValue("@needsApproval", needsApproval);
                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Category added successfully!");
                    }
                    // Redirect to categories page
                    adminDashboard parentForm = this.ParentForm as adminDashboard;
                    if (parentForm != null)
                    {
                        parentForm.LoadFormInPanel(new categories()); // Load the manageUser form in the panel
                    }
                    this.Close(); // Close the form after saving
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving category: " + ex.Message);
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            adminDashboard parentForm = this.ParentForm as adminDashboard;

            if (parentForm != null)
            {
                // Use the LoadFormInPanel method from adminDashboard to load formRole
                parentForm.LoadFormInPanel(new categories());
                //this.Close(); // Close the current form (optional, depending on behavior)
            }
            else
            {
                MessageBox.Show("Parent form not found. Please ensure addRole is opened from adminDashboard.");
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
