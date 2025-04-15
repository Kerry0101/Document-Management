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

namespace tarungonNaNako.subform
{

    public partial class subCategories : Form
    {
        private int _categoryId;
        private string _categoryName;
        public subCategories(int categoryId, string categoryName)
        {
            InitializeComponent();
            _categoryId = categoryId;
            _categoryName = categoryName;
        }



        private void subCategories_Load(object sender, EventArgs e)
        {
            label1.Text = _categoryName; // Display the selected category name in label1
            LoadSubcategories();        // Load the subcategories into the TableLayoutPanel
        }

        private void LoadSubcategories()
        {
            string connectionString = "server=localhost; user=root; Database=docsmanagement; password=";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT subcategoryId, subcategoryName FROM subcategory WHERE categoryId = @categoryId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@categoryId", _categoryId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Clear existing controls and reset row styles
                            tableLayoutPanel1.Controls.Clear();
                            tableLayoutPanel1.RowCount = 0;
                            tableLayoutPanel1.RowStyles.Clear();

                            int fixedRowHeight = 60; // Adjust as needed
                            int rowIndex = 0;

                            while (reader.Read())
                            {
                                int subcategoryId = reader.GetInt32("subcategoryId");
                                string subcategoryName = reader.GetString("subcategoryName");

                                tableLayoutPanel1.RowCount = rowIndex + 1;

                                // Create a Panel for the subcategory row
                                Panel rowPanel = new Panel
                                {
                                    Dock = DockStyle.Fill,
                                    BackColor = ColorTranslator.FromHtml("#ffe261"),
                                    Height = fixedRowHeight,
                                    Tag = subcategoryId // Store subcategoryId for events
                                };

                                // Add hover effects
                                rowPanel.MouseEnter += (s, e) => rowPanel.BackColor = Color.LightGray;
                                rowPanel.MouseLeave += (s, e) => rowPanel.BackColor = Color.White;

                                // Create Label for subcategory name
                                Label subcategoryLabel = new Label
                                {
                                    Text = subcategoryName,
                                    Dock = DockStyle.Left,
                                    AutoSize = false,
                                    Width = 200,
                                    TextAlign = ContentAlignment.MiddleLeft,
                                    Padding = new Padding(10, 0, 0, 0)
                                };

                                // Create Edit button
                                Button editButton = new Button
                                {
                                    Text = "Edit",
                                    Dock = DockStyle.Right,
                                    Width = 90
                                };

                                editButton.Click += (sender, e) => EditSubcategory(subcategoryId);

                                // Create Remove button
                                Button removeButton = new Button
                                {
                                    Text = "Remove",
                                    Dock = DockStyle.Right,
                                    Width = 90
                                };

                                removeButton.Click += (sender, e) => RemoveSubcategory(subcategoryId);

                                // Add controls to rowPanel
                                rowPanel.Controls.Add(subcategoryLabel);
                                rowPanel.Controls.Add(editButton);
                                rowPanel.Controls.Add(removeButton);

                                // Add rowPanel to TableLayoutPanel
                                tableLayoutPanel1.Controls.Add(rowPanel, 0, rowIndex);
                                tableLayoutPanel1.SetColumnSpan(rowPanel, 3);

                                rowIndex++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading subcategories: " + ex.Message);
                }
            }
        }
        private void EditSubcategory(int subcategoryId)
        {
            // Handle editing functionality here
            MessageBox.Show($"Edit subcategory ID: {subcategoryId}");
        }

        private void RemoveSubcategory(int subcategoryId)
        {
            // Display confirmation dialog
            var result = MessageBox.Show(
                "Are you sure you want to remove this subcategory? This action cannot be undone.",
                "Confirm Removal",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                string connectionString = "server=localhost; user=root; Database=docsmanagement; password=";

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        string query = "DELETE FROM subcategory WHERE subcategoryId = @subcategoryId";

                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@subcategoryId", subcategoryId);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Subcategory removed successfully.");
                                LoadSubcategories(); // Refresh the list
                            }
                            else
                            {
                                MessageBox.Show("Subcategory not found or could not be removed.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error removing subcategory: " + ex.Message);
                    }
                }
            }
        }
           
        private void label1_Click(object sender, EventArgs e)
        {

        }


        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
