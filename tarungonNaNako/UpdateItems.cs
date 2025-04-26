using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;

namespace WandererCup
{
    public partial class UpdateItems : Form
    {
        private DataTable itemsTable;
        private DataTable categoriesTable;
        private AutoCompleteStringCollection categoryAutoComplete;


        public UpdateItems()
        {
            InitializeComponent();
            guna2Panel1.Visible = false;
            guna2Panel16.Visible = false; // Hide guna2Panel16 by default
            guna2CustomGradientPanel1.Visible = false; // Hide guna2CustomGradientPanel1 by default
            guna2CustomGradientPanel2.Visible = false;
            guna2Panel21.Visible = false;
            LoadItems();
            LoadCategories();
            CustomizeDataGridView();
            guna2TextBox1.TextChanged += Guna2TextBox1_TextChanged;
            guna2TextBox2.TextChanged += guna2TextBox2_TextChanged;
            guna2Button2.Click += Guna2Button2_Click;
            guna2Button1.Click += guna2Button1_Click;
            guna2Button4.Click += guna2Button4_Click;
            guna2DataGridView1.EditingControlShowing += guna2DataGridView1_EditingControlShowing;
            guna2DataGridView1.SelectionChanged += guna2DataGridView1_SelectionChanged;
            CategoryTextBox.Validating += CategoryTextBox_Validating; // Add Validating event handler
            guna2Button6.Click += guna2Button6_Click;

            CategoryTextBox.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Bold);
            CategoryTextBox.ForeColor = Color.Black; // Set font color to black

            // Allow typing in the TextBox
            CategoryTextBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            CategoryTextBox.AutoCompleteSource = AutoCompleteSource.CustomSource;

            // Enhance the design of the CategoryTextBox
            CategoryTextBox.BorderRadius = 10;
            CategoryTextBox.BorderThickness = 2;
            CategoryTextBox.BorderColor = Color.Gray;
            CategoryTextBox.FillColor = Color.White;
            CategoryTextBox.ForeColor = Color.Black;
            CategoryTextBox.PlaceholderText = "Enter Category...";
            CategoryTextBox.PlaceholderForeColor = Color.Gray;
            CategoryTextBox.ShadowDecoration.Enabled = true;
            CategoryTextBox.ShadowDecoration.BorderRadius = 10;
            CategoryTextBox.ShadowDecoration.Color = Color.Gray;
            CategoryTextBox.ShadowDecoration.Shadow = new Padding(5);

            // Enhance the design of the ProductTextBox
            ItemNameTextbox.BorderRadius = 10;
            ItemNameTextbox.BorderThickness = 2;
            ItemNameTextbox.BorderColor = Color.Gray;
            ItemNameTextbox.FillColor = Color.White;
            ItemNameTextbox.ForeColor = Color.Black;
            ItemNameTextbox.PlaceholderText = "Enter new product name...";
            ItemNameTextbox.PlaceholderForeColor = Color.Gray;
            ItemNameTextbox.ShadowDecoration.Enabled = true;
            ItemNameTextbox.ShadowDecoration.BorderRadius = 10;
            ItemNameTextbox.ShadowDecoration.Color = Color.Gray;
            ItemNameTextbox.ShadowDecoration.Shadow = new Padding(5);

            // Enhance the design of the PriceTextBox
            PriceTextbox.BorderRadius = 10;
            PriceTextbox.BorderThickness = 2;
            PriceTextbox.BorderColor = Color.Gray;
            PriceTextbox.FillColor = Color.White;
            PriceTextbox.ForeColor = Color.Black;
            PriceTextbox.PlaceholderText = "Enter new price...";
            PriceTextbox.PlaceholderForeColor = Color.Gray;
            PriceTextbox.ShadowDecoration.Enabled = true;
            PriceTextbox.ShadowDecoration.BorderRadius = 10;
            PriceTextbox.ShadowDecoration.Color = Color.Gray;
            PriceTextbox.ShadowDecoration.Shadow = new Padding(5);
        }

        private void CategoryTextBox_Validating(object sender, CancelEventArgs e)
        {
            string enteredCategory = CategoryTextBox.Text.ToLower();
            bool categoryExists = categoryAutoComplete.Cast<string>().Any(c => c.ToLower() == enteredCategory);
            if (!categoryExists)
            {
                guna2Panel16.Visible = true; // Show guna2Panel16 if the category is not in the list
                guna2CustomGradientPanel1.Visible = false; // Hide guna2CustomGradientPanel1
            }
        }








        private void guna2DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = guna2DataGridView1.SelectedRows[0];
                string category = selectedRow.Cells["Category"].Value.ToString();
                string productName = selectedRow.Cells["Product Name"].Value.ToString();
                string price = selectedRow.Cells["Price"].Value.ToString();

                ItemNameTextbox.Text = productName;
                PriceTextbox.Text = price;

                // Populate the TextBox for Category
                CategoryTextBox.Text = category;

                // Store the original values in the Tag property
                CategoryTextBox.Tag = category;
                ItemNameTextbox.Tag = productName;
                PriceTextbox.Tag = price;
            }
        }


        private void CustomizeDataGridView()
        {
            guna2DataGridView1.RowHeadersVisible = false;
            guna2DataGridView1.BorderStyle = BorderStyle.Fixed3D;
            guna2DataGridView1.GridColor = Color.Black;
            guna2DataGridView1.ReadOnly = true; // Set to readonly
            guna2DataGridView1.AllowUserToAddRows = false;
            guna2DataGridView1.AllowUserToDeleteRows = false;
            guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None; // Disable auto size columns mode
            guna2DataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None; // Disable auto size rows mode
            guna2DataGridView1.ScrollBars = ScrollBars.Both; // Enable both horizontal and vertical scrollbars
            guna2DataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            guna2DataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            guna2DataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            guna2DataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font(guna2DataGridView1.Font.FontFamily, 12, FontStyle.Bold); // Change font size
            guna2DataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.Navy;
            guna2DataGridView1.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            guna2DataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            guna2DataGridView1.DefaultCellStyle.BackColor = Color.Beige;
            guna2DataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            guna2DataGridView1.DefaultCellStyle.SelectionBackColor = Color.DarkSlateBlue;
            guna2DataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;
            guna2DataGridView1.RowTemplate.Height = 30;
            guna2DataGridView1.AllowUserToResizeColumns = true; // Allow user to resize columns
            guna2DataGridView1.AllowUserToResizeRows = true; // Allow user to resize rows
            guna2DataGridView1.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Regular); // Change font size
            guna2DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Set default width size of each cell to 227
            foreach (DataGridViewColumn column in guna2DataGridView1.Columns)
            {
                column.Width = 198;
            }

            // Make all columns editable except the 'Category' column
            foreach (DataGridViewColumn column in guna2DataGridView1.Columns)
            {
                if (column.HeaderText == "Category")
                {
                    column.ReadOnly = true;
                }
                else
                {
                    column.ReadOnly = false;
                }
            }

            // Hide the ProductID column
            if (guna2DataGridView1.Columns["ProductID"] != null)
            {
                guna2DataGridView1.Columns["ProductID"].Visible = false;
            }



            guna2DataGridView2.RowHeadersVisible = false;
            guna2DataGridView2.BorderStyle = BorderStyle.Fixed3D;
            guna2DataGridView2.GridColor = Color.Black;
            guna2DataGridView2.ReadOnly = false;
            guna2DataGridView2.AllowUserToAddRows = false;
            guna2DataGridView2.AllowUserToDeleteRows = false;
            guna2DataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None; // Disable auto size columns mode
            guna2DataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None; // Disable auto size rows mode
            guna2DataGridView2.ScrollBars = ScrollBars.Both; // Enable both horizontal and vertical scrollbars

            guna2DataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            guna2DataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            guna2DataGridView2.ColumnHeadersDefaultCellStyle.Font = new Font(guna2DataGridView2.Font.FontFamily, 12, FontStyle.Bold); // Change font size
            guna2DataGridView2.RowHeadersDefaultCellStyle.BackColor = Color.Navy;
            guna2DataGridView2.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            guna2DataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            guna2DataGridView2.DefaultCellStyle.BackColor = Color.Beige;
            guna2DataGridView2.DefaultCellStyle.ForeColor = Color.Black;
            guna2DataGridView2.DefaultCellStyle.SelectionBackColor = Color.DarkSlateBlue;
            guna2DataGridView2.DefaultCellStyle.SelectionForeColor = Color.White;
            guna2DataGridView2.RowTemplate.Height = 30;
            guna2DataGridView2.AllowUserToResizeColumns = true; // Allow user to resize columns
            guna2DataGridView2.AllowUserToResizeRows = true; // Allow user to resize rows
            guna2DataGridView2.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular); // Change font size

            // Set default width size of each cell to 210
            foreach (DataGridViewColumn column in guna2DataGridView2.Columns)
            {
                column.Width = 227;
            }
        }


        private void LoadItems()
        {
            string connectionString = GetConnectionString();
            string query = @"
    SELECT p.ProductID, c.CategoryName AS 'Category', p.ProductName AS 'Product Name', ps.SellingPrice AS 'Price'
    FROM product p
    JOIN category c ON p.CategoryID = c.CategoryID
    JOIN product_sales ps ON p.ProductID = ps.ProductID
    WHERE p.is_archived = 0";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                connection.Open();

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                itemsTable = new DataTable();
                adapter.Fill(itemsTable);

                // Bind the DataTable to the DataGridView
                guna2DataGridView1.DataSource = itemsTable;
            }
        }




        private void LoadCategories()
        {
            string connectionString = GetConnectionString();
            string query = "SELECT CategoryName FROM category WHERE is_archived = 0";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                connection.Open();

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                categoriesTable = new DataTable();
                adapter.Fill(categoriesTable);

                // Remove the CategoryID column if it exists
                if (categoriesTable.Columns.Contains("CategoryID"))
                {
                    categoriesTable.Columns.Remove("CategoryID");
                }

                // Bind the DataTable to the DataGridView
                guna2DataGridView2.DataSource = categoriesTable;

                // Prepare auto-complete collection
                categoryAutoComplete = new AutoCompleteStringCollection();
                foreach (DataRow row in categoriesTable.Rows)
                {
                    categoryAutoComplete.Add(row["CategoryName"].ToString());
                }

                // Populate the TextBox with category names
                CategoryTextBox.AutoCompleteCustomSource = categoryAutoComplete;
            }
        }

        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        }

        private void Guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            string searchText = guna2TextBox1.Text.ToLower();
            DataView dv = itemsTable.DefaultView;
            dv.RowFilter = string.Format("[Product Name] LIKE '%{0}%'", searchText);
            guna2DataGridView1.DataSource = dv.ToTable();
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            string searchText = guna2TextBox2.Text.ToLower();
            DataView dv = categoriesTable.DefaultView;
            dv.RowFilter = string.Format("[CategoryName] LIKE '%{0}%'", searchText);
            guna2DataGridView2.DataSource = dv.ToTable();
        }

        private async void guna2Button7_Click(object sender, EventArgs e)
        {
            string connectionString = GetConnectionString();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                foreach (DataRow row in itemsTable.Rows)
                {
                    if (row.RowState == DataRowState.Modified)
                    {
                        // Get the CategoryID for the selected CategoryName
                        string categoryQuery = "SELECT CategoryID FROM category WHERE CategoryName = @CategoryName";
                        MySqlCommand categoryCommand = new MySqlCommand(categoryQuery, connection);
                        categoryCommand.Parameters.AddWithValue("@CategoryName", row["Category"]);
                        int categoryId = Convert.ToInt32(categoryCommand.ExecuteScalar());

                        string updateQuery = @"
                                                UPDATE product
                                                SET ProductName = @ProductName, Price = @Price, CategoryID = @CategoryID
                                                WHERE ProductName = @OldProductName";

                        MySqlCommand command = new MySqlCommand(updateQuery, connection);
                        command.Parameters.AddWithValue("@ProductName", row["Product Name"]);
                        command.Parameters.AddWithValue("@Price", row["Price"]);
                        command.Parameters.AddWithValue("@CategoryID", categoryId);
                        command.Parameters.AddWithValue("@OldProductName", row["Product Name", DataRowVersion.Original]);

                        command.ExecuteNonQuery();
                    }
                }

                foreach (DataRow row in categoriesTable.Rows)
                {
                    if (row.RowState == DataRowState.Modified)
                    {
                        string updateQuery = "UPDATE category SET CategoryName = @CategoryName WHERE CategoryName = @OldCategoryName";

                        MySqlCommand command = new MySqlCommand(updateQuery, connection);
                        command.Parameters.AddWithValue("@CategoryName", row["CategoryName"]);
                        command.Parameters.AddWithValue("@OldCategoryName", row["CategoryName", DataRowVersion.Original]);

                        command.ExecuteNonQuery();
                    }
                }
            }

            // Refresh the data
            LoadItems();
            LoadCategories();
            guna2Panel21.Visible = false;
            guna2CustomGradientPanel2.Visible = true;
            await Task.Delay(3000);
            guna2CustomGradientPanel2.Visible = false;
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void UpdateItems_Load(object sender, EventArgs e)
        {

        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2DataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            guna2Panel1.Visible = true;
        }

        private void guna2DataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox autoText = e.Control as TextBox;
            if (autoText != null)
            {
                autoText.AutoCompleteMode = AutoCompleteMode.None;
            }
        }

        private void guna2TextBox2_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void guna2DataGridView2_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private async void guna2Button3_Click(object sender, EventArgs e)
        {
            // Get the values from the controls in guna2Panel1
            string category = string.IsNullOrWhiteSpace(CategoryTextBox.Text) ? CategoryTextBox.Tag.ToString() : CategoryTextBox.Text;
            string productName = string.IsNullOrWhiteSpace(ItemNameTextbox.Text) ? ItemNameTextbox.Tag.ToString() : ItemNameTextbox.Text;
            string price = string.IsNullOrWhiteSpace(PriceTextbox.Text) ? PriceTextbox.Tag.ToString() : PriceTextbox.Text;

            // Find the selected row in guna2DataGridView1
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = guna2DataGridView1.SelectedRows[0];
                int productId = Convert.ToInt32(selectedRow.Cells["ProductID"].Value);

                // Update the values in the DataGridView
                selectedRow.Cells["Category"].Value = category;
                selectedRow.Cells["Product Name"].Value = productName;
                selectedRow.Cells["Price"].Value = price;

                // Find the corresponding DataRow in the itemsTable
                DataRow[] rows = itemsTable.Select($"ProductID = {productId}");
                if (rows.Length > 0)
                {
                    DataRow row = rows[0];
                    row["Category"] = category;
                    row["Product Name"] = productName;
                    row["Price"] = price;
                    row.AcceptChanges(); // Accept the changes to reset the row state
                    row.SetModified(); // Mark the row as modified
                }

                // Save the index of the selected row
                int selectedIndex = selectedRow.Index;

                // Hide the panel after updating
                guna2Panel1.Visible = false;

                // Save changes to the database
                SaveChangesToDatabase();

                // Re-select the previously selected row
                guna2DataGridView1.ClearSelection();
                guna2DataGridView1.Rows[selectedIndex].Selected = true;

                // Check if guna2Panel16 is visible before showing guna2CustomGradientPanel1
                if (!guna2Panel16.Visible)
                {
                    // Show guna2CustomGradientPanel1 if update is successful
                    guna2CustomGradientPanel1.Visible = true;
                    // Wait for 3 seconds
                    await Task.Delay(3000);

                    // Hide guna2CustomGradientPanel1 after 3 seconds
                    guna2CustomGradientPanel1.Visible = false;
                }
            }
        }






        private void SaveChangesToDatabase()
        {
            string connectionString = GetConnectionString();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                foreach (DataRow row in itemsTable.Rows)
                {
                    if (row.RowState == DataRowState.Modified)
                    {
                        try
                        {
                            // Get the CategoryID for the selected CategoryName
                            string categoryQuery = "SELECT CategoryID FROM category WHERE CategoryName = @CategoryName";
                            MySqlCommand categoryCommand = new MySqlCommand(categoryQuery, connection);
                            categoryCommand.Parameters.AddWithValue("@CategoryName", row["Category"]);
                            int categoryId = Convert.ToInt32(categoryCommand.ExecuteScalar());

                            string updateQuery = @"
                            UPDATE product
                            SET ProductName = @ProductName, Price = @Price, CategoryID = @CategoryID
                            WHERE ProductID = @ProductID";

                            MySqlCommand command = new MySqlCommand(updateQuery, connection);
                            command.Parameters.AddWithValue("@ProductName", row["Product Name"]);
                            command.Parameters.AddWithValue("@Price", row["Price"]);
                            command.Parameters.AddWithValue("@CategoryID", categoryId);
                            command.Parameters.AddWithValue("@ProductID", row["ProductID"]);

                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            // Show guna2Panel16 if an error occurs
                            guna2Panel16.Visible = true;
                        }
                    }
                }
            }

            // Refresh the data
            LoadItems();
        }






        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void PriceTextbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void ItemNameTextbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            guna2Panel1.Visible = false;
            guna2Panel16.Visible = false;
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            //var addroducts = new AddProducts();
            //addroducts.FormClosed += (s, args) => Application.Exit();
            this.Hide();
            //addroducts.Show();
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            guna2Panel16.Visible = false; // Hide guna2Panel16
            guna2Panel1.Visible = true; // Show guna2Panel1
        }

        private void guna2HtmlLabel3_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel16_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Guna2Button2_Click(object sender, EventArgs e)
        {
            guna2Panel21.Visible = true;
        }

        private void guna2HtmlLabel5_Click(object sender, EventArgs e)
        {

        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            guna2Panel21.Visible = false;
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            guna2Panel21.Visible = false;
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
