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
    public partial class addSubCategory : Form
    {
        private string connectionString = "server=localhost;user=root;database=docsmanagement;password=";
        private int _categoryId;
        private string _categoryName;

        public addSubCategory()
        {
            InitializeComponent();
            LoadCategories(); // Populate categories in the ComboBox
        }
        public addSubCategory(int categoryId, string categoryName)
        {
            InitializeComponent();

            _categoryId = categoryId;
            _categoryName = categoryName;

            LoadCategories(); // Populate categories in the ComboBox
            PreSelectCategory(); // Pre-select the passed category
        }
        private void PreSelectCategory()
        {
            // Find the index of the category in the ComboBox and select it
            for (int i = 0; i < comboBox1.Items.Count; i++)
            {
                DataRowView row = comboBox1.Items[i] as DataRowView;
                if (row != null && Convert.ToInt32(row["categoryId"]) == _categoryId)
                {
                    comboBox1.SelectedIndex = i;
                    break;
                }
            }
        }
        private void LoadCategories()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT categoryId, categoryName FROM category";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        comboBox1.DataSource = dt;
                        comboBox1.DisplayMember = "categoryName"; // Display category name
                        comboBox1.ValueMember = "categoryId"; // Use category ID as value
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            adminDashboard parentForm = this.ParentForm as adminDashboard;

            if (parentForm != null)
            {
                // Use the LoadFormInPanel method from adminDashboard to load categories form
                parentForm.LoadFormInPanel(new categories());
            }
            else
            {
                MessageBox.Show("Parent form not found. Please ensure addRole is opened from adminDashboard.");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Validate user input
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please enter a subcategory name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a category.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string subcategoryName = textBox1.Text.Trim();
            int categoryId = Convert.ToInt32(comboBox1.SelectedValue);

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO subcategory (categoryId, SubcategoryName) VALUES (@categoryId, @subcategoryName)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@categoryId", categoryId);
                        cmd.Parameters.AddWithValue("@subcategoryName", subcategoryName);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Subcategory added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            textBox1.Clear(); // Clear the input field
                            comboBox1.SelectedIndex = -1; // Reset the ComboBox selection
                        }
                        else
                        {
                            MessageBox.Show("Failed to add subcategory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding subcategory: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
