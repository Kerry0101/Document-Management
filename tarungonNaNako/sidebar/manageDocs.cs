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
using MySql.Data.MySqlClient;

namespace tarungonNaNako.sidebar
{
    public partial class manageDocs : Form
    {
        private readonly string connectionString = "server=localhost;user=root;database=docsmanagement;password="; // DB connection

        public manageDocs()
        {
            InitializeComponent();

            LoadFiles();
        }
        private void LoadFiles(bool includeArchived = false)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                SELECT 
                    f.fileId,
                    f.fileName AS docuName,
                    c.categoryName AS category,
                    s.subcategoryName AS subCategory,
                    f.uploadDate AS dateCreated,
                    u.username AS createdBy
                FROM 
                    files f
                LEFT JOIN 
                    category c ON f.categoryId = c.categoryId
                LEFT JOIN 
                    subcategory s ON f.subcategoryId = s.subcategoryId
                LEFT JOIN 
                    users u ON f.uploadedBy = u.userId
                WHERE 
                    f.isArchived = @includeArchived OR @includeArchived = 1";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@includeArchived", includeArchived ? 1 : 0);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            dataGridView1.Rows.Clear();

                            foreach (DataRow row in dt.Rows)
                            {
                                int rowIndex = dataGridView1.Rows.Add();
                                DataGridViewRow dgvRow = dataGridView1.Rows[rowIndex];

                                dgvRow.Cells["fileId"].Value = row["fileId"];
                                dgvRow.Cells["docuName"].Value = row["docuName"];
                                dgvRow.Cells["category"].Value = row["category"];
                                dgvRow.Cells["subCategory"].Value = row["subCategory"];
                                dgvRow.Cells["dateCreated"].Value = Convert.ToDateTime(row["dateCreated"]).ToString("yyyy-MM-dd HH:mm");
                                dgvRow.Cells["createdBy"].Value = row["createdBy"];

                                // Add Action options to the combobox
                                DataGridViewComboBoxCell actionCell = dgvRow.Cells["action"] as DataGridViewComboBoxCell;
                                if (actionCell != null)
                                {
                                    actionCell.Items.Clear();
                                    actionCell.Items.AddRange("Versions", "Archive", "Download", "Rename");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading files: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        private void button1_Click(object sender, EventArgs e)
        {
            adminDashboard parentForm = this.ParentForm as adminDashboard;

            if (parentForm != null)
            {
                // Use the LoadFormInPanel method from adminDashboard to load formRole
                parentForm.LoadFormInPanel(new addDocs());
                this.Close(); // Close the current form (optional, depending on behavior)
            }
            else
            {
                MessageBox.Show("Parent form not found. Please ensure addRole is opened from adminDashboard.");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }



        private void ArchiveFile(int fileId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE files SET isArchived = TRUE WHERE fileId = @fileId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@fileId", fileId);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("File archived successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadFiles(); // Refresh the file list
                        }
                        else
                        {
                            MessageBox.Show("Failed to archive the file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error archiving file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void manageDocs_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellValueChanged_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "action")
            {
                string action = dataGridView1.Rows[e.RowIndex].Cells["action"].Value?.ToString();
                int fileId;
                if (int.TryParse(dataGridView1.Rows[e.RowIndex].Cells["fileId"].Value?.ToString(), out fileId))
                {
                    switch (action)
                    {
                        case "Versions":
                            // Handle Versions (to be implemented)
                            MessageBox.Show($"Viewing versions for File ID {fileId}", "Info");
                            break;

                        case "Archive":
                            ArchiveFile(fileId);
                            break;

                        case "Download":
                            // Handle Download (to be implemented)
                            MessageBox.Show($"Downloading File ID {fileId}", "Info");
                            break;

                        case "Rename":
                            // Handle Rename (to be implemented)
                            MessageBox.Show($"Renaming File ID {fileId}", "Info");
                            break;

                        default:
                            MessageBox.Show("Invalid action selected!", "Error");
                            break;
                    }

                    // Reset combobox value after handling
                    dataGridView1.Rows[e.RowIndex].Cells["action"].Value = 0;
                }
                else
                {
                    MessageBox.Show("Invalid File ID", "Error");
                }
            }
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            /*if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["action"].Index)
            {
                // Check if the clicked cell is in the 'action' column
                DataGridViewComboBoxCell actionCell = dataGridView1.Rows[e.RowIndex].Cells["action"] as DataGridViewComboBoxCell;

                if (actionCell != null)
                {
                    // Retrieve the selected action
                    string action = actionCell.Value?.ToString();
                    int fileId;
                    if (int.TryParse(dataGridView1.Rows[e.RowIndex].Cells["fileId"].Value?.ToString(), out fileId))
                    {
                        switch (action)
                        {
                            case "Versions":
                                // Handle Versions (to be implemented)
                                MessageBox.Show($"Viewing versions for File ID {fileId}", "Info");
                                break;

                            case "Archive":
                                ArchiveFile(fileId);
                                break;

                            case "Download":
                                // Handle Download (to be implemented)
                                MessageBox.Show($"Downloading File ID {fileId}", "Info");
                                break;

                            case "Rename":
                                // Handle Rename (to be implemented)
                                MessageBox.Show($"Renaming File ID {fileId}", "Info");
                                break;

                            default:
                                MessageBox.Show("Invalid action selected!", "Error");
                                break;
                        }

                        // Reset combobox value after handling
                        dataGridView1.Rows[e.RowIndex].Cells["action"].Value = null;
                    }
                    else
                    {
                        MessageBox.Show("Invalid File ID", "Error");
                    }
                }
            }*/
        }
    }
}
        

    
