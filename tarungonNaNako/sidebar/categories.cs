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
    public partial class categories : Form
    {
        public categories()
        {
            InitializeComponent();
        }

        private void LoadCategories()
        {
            string connectionString = "server=localhost; user=root; Database=docsmanagement; password=";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT categoryId, categoryName FROM category";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Clear existing controls and reset row styles
                            tableLayoutPanel1.Controls.Clear();
                            tableLayoutPanel1.RowCount = 0;
                            tableLayoutPanel1.RowStyles.Clear();

                            // Fixed height for rows
                            int fixedRowHeight = 60; // Adjust this value as needed
                            int rowIndex = 0;

                            while (reader.Read())
                            {
                                int categoryId = reader.GetInt32("categoryId");
                                string categoryName = reader.GetString("categoryName");

                                // Add a new row and set its height
                                tableLayoutPanel1.RowCount = rowIndex + 1;
                                //tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, fixedRowHeight));

                                // Create a Panel for the row to handle layout and events
                                Panel rowPanel = new Panel
                                {
                                    Dock = DockStyle.Fill,
                                    BackColor = Color.White,
                                    Height = fixedRowHeight,
                                    Tag = categoryId // Store categoryId for later use
                                };

                                // Add hover effects
                                rowPanel.MouseEnter += (s, e) => rowPanel.BackColor = Color.LightGray;
                                rowPanel.MouseLeave += (s, e) => rowPanel.BackColor = Color.White;

                                // Add click event to open subcategories
                                rowPanel.Click += (s, e) => ViewSubcategories((int)rowPanel.Tag);

                                // Create Label for category name
                                Label categoryLabel = new Label
                                {
                                    Text = categoryName,
                                    Dock = DockStyle.Left,
                                    AutoSize = false,
                                    Width = 200, // Set a width for the label
                                    TextAlign = ContentAlignment.MiddleLeft,
                                    Padding = new Padding(10, 0, 0, 0)
                                };


                                // Create Edit button
                                Button editButton = new Button
                                {
                                    Text = "Edit",
                                    Dock = DockStyle.Right,
                                    Width = 90, // Set a fixed width
                                    Height = 50
                                };

                                editButton.Click += (sender, e) => EditCategory(categoryId);

                                // Create Archive button
                                Button removeButton = new Button
                                {
                                    Text = "Remove",
                                    Dock = DockStyle.Right,
                                    Width = 90, // Set a fixed width
                                    Height= 50
                                };
                                removeButton.Click += (sender, e) => RemoveCategory(categoryId);

                                // Add controls to the rowPanel
                                rowPanel.Controls.Add(categoryLabel);
                                rowPanel.Controls.Add(editButton);
                                rowPanel.Controls.Add(removeButton);
                                

                                // Add the rowPanel to the TableLayoutPanel
                                tableLayoutPanel1.Controls.Add(rowPanel, 0, rowIndex);
                                tableLayoutPanel1.SetColumnSpan(rowPanel, 3); // Span all columns

                                rowIndex++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading categories: " + ex.Message);
                }
            }
        }



        // Event handler for Edit button
        private void EditCategory(int categoryId)
        {
            adminDashboard parentForm = this.ParentForm as adminDashboard;

            if (parentForm != null)
            {
                // Open addCategory form with the categoryId
                parentForm.LoadFormInPanel(new addCategory(categoryId));
            }
            else
            {
                MessageBox.Show("Parent form not found. Please ensure the form is opened from adminDashboard.");
            }
        }

        // Event handler for Archive button
        // Event handler for Remove button
        private void RemoveCategory(int categoryId)
        {
            // Display a confirmation dialog
            var result = MessageBox.Show(
                "Are you sure you want to remove this category? This action cannot be undone.",
                "Confirm Removal",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            // Check the user's choice
            if (result == DialogResult.Yes)
            {
                string connectionString = "server=localhost; user=root; Database=docsmanagement; password=";

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        // Begin a transaction to ensure both deletions succeed
                        using (MySqlTransaction transaction = conn.BeginTransaction())
                        {
                            try
                            {
                                // Delete associated permissions
                                string deletePermissionsQuery = "DELETE FROM permissions WHERE referenceId = @categoryId AND type = 'category'";
                                using (MySqlCommand cmd = new MySqlCommand(deletePermissionsQuery, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@categoryId", categoryId);
                                    cmd.ExecuteNonQuery();
                                }

                                // Delete the category
                                string deleteCategoryQuery = "DELETE FROM category WHERE categoryId = @categoryId";
                                using (MySqlCommand cmd = new MySqlCommand(deleteCategoryQuery, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@categoryId", categoryId);
                                    int rowsAffected = cmd.ExecuteNonQuery();

                                    if (rowsAffected > 0)
                                    {
                                        // Commit the transaction
                                        transaction.Commit();

                                        MessageBox.Show("Category and its permissions removed successfully.",
                                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        LoadCategories(); // Refresh the category list
                                    }
                                    else
                                    {
                                        throw new Exception("Category not found or could not be removed.");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                // Roll back the transaction if any operation fails
                                transaction.Rollback();
                                throw new Exception("Error removing category and permissions: " + ex.Message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error removing category: " + ex.Message,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        private void ViewSubcategories(int categoryId)
        {
            string connectionString = "server=localhost; user=root; Database=docsmanagement; password=";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT categoryName FROM category WHERE categoryId = @categoryId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@categoryId", categoryId);

                        // Fetch categoryName from the database
                        string categoryName = cmd.ExecuteScalar()?.ToString();

                        if (!string.IsNullOrEmpty(categoryName))
                        {
                            adminDashboard parentForm = this.ParentForm as adminDashboard;

                            if (parentForm != null)
                            {
                                // Load the subCategories form in the parent panel
                                parentForm.LoadFormInPanel(new subCategories(categoryId, categoryName));
                            }
                            else
                            {
                                MessageBox.Show("Parent form not found. Please ensure the form is opened from adminDashboard.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Category not found or has been deleted.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving category details: " + ex.Message);
                }
            }
        }



        private void categories_Load(object sender, EventArgs e)
        {
            LoadCategories();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            adminDashboard parentForm = this.ParentForm as adminDashboard;

            if (parentForm != null)
            {
                // Use the LoadFormInPanel method from adminDashboard to load formRole
                parentForm.LoadFormInPanel(new addCategory());
                //this.Close(); // Close the current form (optional, depending on behavior)
            }
            else
            {
                MessageBox.Show("Parent form not found. Please ensure addRole is opened from adminDashboard.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            addSubCategory addSubCategoryForm = new addSubCategory();
            addSubCategoryForm.Show();
        }


        private void tableLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {
            //tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            //tableLayoutPanel1.Size = new Size(tableLayoutPanel1.Height, 490);

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }   

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
    }
}
