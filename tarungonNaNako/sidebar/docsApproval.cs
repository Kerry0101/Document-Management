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
using tarungonNaNako.subform;

namespace tarungonNaNako.sidebar
{
    public partial class docsApproval : Form
    {
        string folder = Path.Combine(Application.StartupPath, "Assets (images)", "shared_folders.png");

        public docsApproval()
        {
            InitializeComponent();

            // Enable horizontal scrolling for panel1
            panel1.AutoScroll = true;
            panel1.HorizontalScroll.Enabled = true;
            panel1.HorizontalScroll.Visible = true;
            panel1.Padding = new Padding(0, 0, 0, 100);
        }

        public void LoadFormInPanel(Form form)
        {
            // Clear previous controls in the panel (replace "panel5" with your content panel name)
            panel3.Controls.Clear();

            // Dispose of the previous form if it exists
            foreach (Control control in panel3.Controls)
            {
                if (control is Form previousForm)
                {
                    previousForm.Dispose();
                }
            }

            // Set properties for the form to display it in the panel
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            panel3.Controls.Add(form);
            form.Show();
        }

        private int? currentCategoryId = null; // NULL means Root Folder
        private Stack<int?> navigationHistory = new Stack<int?>(); // Stack to track navigation

        public void NavigateToCategory(int categoryId)
        {
            navigationHistory.Push(currentCategoryId); // Store current category before navigating
            currentCategoryId = categoryId; // Update current category
            LoadPendingApprovals(currentCategoryId); // Reload with new category
        }

        private void LoadPendingApprovals()
        {
            LoadPendingApprovals(currentCategoryId);
        }

        private void LoadPendingApprovals(int? currentCategoryId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("server=localhost;database=docsmanagement;uid=root;pwd=;"))
                {
                    conn.Open();

                    string query = @"
                    SELECT
                        sf.id,              -- The ID from shared_files
                        sf.file_name,       -- The name stored in shared_files (might be same as categoryName)
                        u.username AS submitted_by,
                        r.roleName AS user_role,
                        sf.created_at AS dateSubmitted,
                        sf.status,
                        sf.is_folder,
                        sf.categoryId,      -- The crucial categoryId linking to the 'category' table
                        c.categoryName      -- The actual categoryName from the 'category' table
                    FROM
                        shared_files sf
                    JOIN
                        users u ON sf.userId = u.userId
                    JOIN
                        roles r ON u.roleId = r.roleId
                    JOIN
                        category c ON sf.categoryId = c.categoryId -- Join with category table
                    WHERE
                        sf.status = 'Pending'
                        AND sf.parentId IS NULL -- Fetch only root-level shared items for approval list
                        AND sf.is_folder = 1    -- Ensure it's a folder
                    ORDER BY
                        sf.created_at DESC";



                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Clear existing rows in the TableLayoutPanel
                            tableLayoutPanel1.Controls.Clear();
                            tableLayoutPanel1.RowCount = 0;
                            tableLayoutPanel1.RowStyles.Clear();

                            // Set the column count to 1 (Single column for the row panel)
                            tableLayoutPanel1.ColumnCount = 1;
                            tableLayoutPanel1.ColumnStyles.Clear();
                            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100)); // Full width for the row panel

                            // Fixed height for rows
                            int fixedRowHeight = 60; // Adjust this value as needed
                            int rowIndex = 0;

                            while (reader.Read())
                            {
                                int sharedFileId = reader.GetInt32("id");
                                string sharedFileName = reader.GetString("file_name"); 
                                string submittedBy = reader.GetString("submitted_by");
                                string userRole = reader.GetString("user_role");
                                DateTime dateSubmitted = reader.GetDateTime("dateSubmitted");
                                string status = reader.GetString("status");
                                bool isFolder = reader.GetBoolean("is_folder");

                                int categoryId = reader.GetInt32("categoryId"); // Get the linked category ID
                                string categoryName = reader.GetString("categoryName");

                                // Create a Panel for the row
                                Panel rowPanel = new Panel
                                {
                                    Dock = DockStyle.Fill,
                                    BackColor = ColorTranslator.FromHtml("#ffe261"),
                                    Height = fixedRowHeight,
                                    Padding = new Padding(0, 0, 10, 0),
                                    Margin = new Padding(0, 0, 9, 5),
                                    Tag = new Tuple<int, string>(categoryId, categoryName)
                                };
                                rowPanel.DoubleClick += (s, e) =>
                                {
                                    Panel clickedPanel = s as Panel;
                                    if (clickedPanel?.Tag is Tuple<int, string> categoryInfo)
                                    {
                                        int cId = categoryInfo.Item1;
                                        string cName = categoryInfo.Item2;

                                        // Navigate using the actual categoryId from the 'category' table
                                        // NavigateToCategory(cId); // Keep or remove this based on desired behavior
                                        LoadFormInPanel(new fetchDocuments(cId, cName, "")); // Pass correct categoryId and Name
                                    }
                                    else
                                    {
                                        MessageBox.Show("Could not retrieve category details for this item.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                };

                                PictureBox fileIconPictureBox = new PictureBox
                                {
                                    Image = Image.FromFile(folder),
                                    SizeMode = PictureBoxSizeMode.Zoom,
                                    Width = 100,
                                    Height = 25
                                };

                                Label fileNameLabel = new Label
                                {
                                    Text = sharedFileName, // Display the name from shared_files table
                                    Dock = DockStyle.Left,
                                    AutoSize = false,
                                    Font = new Font("Microsoft Sans Serif", 10),
                                    Width = 280,
                                    TextAlign = ContentAlignment.MiddleLeft,
                                    Padding = new Padding(10, 0, 0, 0)
                                };

                                // Create Label for submitted by
                                Label submittedByLabel = new Label
                                {
                                    Text = $"{submittedBy} ({userRole})",
                                    Dock = DockStyle.Left,
                                    AutoSize = false,
                                    Font = new Font("Microsoft Sans Serif", 10),
                                    Width = 200,
                                    TextAlign = ContentAlignment.MiddleLeft,
                                    Padding = new Padding(10, 0, 0, 0)
                                };

                                // Create Label for date submitted
                                Label dateSubmittedLabel = new Label
                                {
                                    Text = dateSubmitted.ToString("yyyy-MM-dd hh:mm tt"),
                                    Dock = DockStyle.Left,
                                    AutoSize = false,
                                    Font = new Font("Microsoft Sans Serif", 10),
                                    Width = 200,
                                    TextAlign = ContentAlignment.MiddleLeft,
                                    Padding = new Padding(10, 0, 0, 0)
                                };

                                // Create Label for status
                                Label statusLabel = new Label
                                {
                                    Text = status,
                                    Dock = DockStyle.Left,
                                    AutoSize = false,
                                    Font = new Font("Microsoft Sans Serif", 10),
                                    Width = 120,
                                    TextAlign = ContentAlignment.MiddleLeft,
                                    Padding = new Padding(10, 0, 0, 0)
                                };

                                // Create Approve button
                                Guna.UI2.WinForms.Guna2Button approveButton = new Guna.UI2.WinForms.Guna2Button
                                {
                                    Text = "Approve",
                                    BorderRadius = 10,
                                    PressedDepth = 10,
                                    FillColor = Color.FromArgb(255, 226, 97),
                                    ForeColor = Color.Black,
                                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                                    Dock = DockStyle.Right,
                                    Width = 100,
                                    Height = 50
                                };
                                approveButton.Click += (sender, e) => UpdateApprovalStatus(sharedFileId, "Approved");

                                // Create Reject button
                                Guna.UI2.WinForms.Guna2Button rejectButton = new Guna.UI2.WinForms.Guna2Button
                                {
                                    Text = "Reject",
                                    BorderRadius = 10,
                                    PressedDepth = 10,
                                    FillColor = Color.FromArgb(255, 226, 97),
                                    ForeColor = Color.Black,
                                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                                    Dock = DockStyle.Right,
                                    Width = 100,
                                    Height = 50
                                };
                                rejectButton.Click += (sender, e) => UpdateApprovalStatus(sharedFileId, "Rejected");

                                // Add hover effects to the rowPanel and all child controls
                                void ApplyHoverEffect(Control control)
                                {
                                    control.MouseEnter += (s, e) =>
                                    {
                                        rowPanel.BackColor = Color.FromArgb(219, 195, 0);
                                        approveButton.FillColor = Color.FromArgb(219, 195, 0);
                                        rejectButton.FillColor = Color.FromArgb(219, 195, 0);
                                    };
                                    control.MouseLeave += (s, e) =>
                                    {
                                        rowPanel.BackColor = ColorTranslator.FromHtml("#ffe261");
                                        approveButton.FillColor = Color.FromArgb(255, 226, 97);
                                        rejectButton.FillColor = Color.FromArgb(255, 226, 97);
                                    };
                                }
                                
                                // Apply hover effect to all child controls
                                ApplyHoverEffect(rowPanel);
                                ApplyHoverEffect(fileIconPictureBox);
                                ApplyHoverEffect(fileNameLabel);
                                ApplyHoverEffect(submittedByLabel);
                                ApplyHoverEffect(dateSubmittedLabel);
                                ApplyHoverEffect(statusLabel);
                                ApplyHoverEffect(approveButton);
                                ApplyHoverEffect(rejectButton);

                                // Add controls to the rowPanel (Important: Right-docked buttons first)
                                rowPanel.Controls.Add(rejectButton);
                                rowPanel.Controls.Add(approveButton);
                                rowPanel.Controls.Add(statusLabel);
                                rowPanel.Controls.Add(dateSubmittedLabel);
                                rowPanel.Controls.Add(submittedByLabel);
                                rowPanel.Controls.Add(fileNameLabel);
                                rowPanel.Controls.Add(fileIconPictureBox);

                                // Add the rowPanel to the TableLayoutPanel
                                tableLayoutPanel1.Controls.Add(rowPanel, 0, rowIndex);
                                tableLayoutPanel1.RowCount = rowIndex + 1;
                                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, fixedRowHeight));

                                rowIndex++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading pending approvals: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void UpdateApprovalStatus(int sharedFileId, string status)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("server=localhost;database=docsmanagement;uid=root;pwd=;"))
                {
                    conn.Open();

                    // Step 1: Fetch all IDs using WITH RECURSIVE based on parentId
                    string recursiveQuery = @"
            WITH RECURSIVE Subfolders AS (
                SELECT id
                FROM shared_files
                WHERE id = @ParentId
                UNION ALL
                SELECT sf.id
                FROM shared_files sf
                INNER JOIN Subfolders s ON sf.parentId = s.id
            )
            SELECT id FROM Subfolders";

                    List<int> idsToUpdate = new List<int>();
                    using (MySqlCommand cmd = new MySqlCommand(recursiveQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@ParentId", sharedFileId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                idsToUpdate.Add(reader.GetInt32("id"));
                            }
                        }
                    }

                    // Debug: Log the IDs fetched by the recursive query
                    Console.WriteLine("IDs to update: " + string.Join(", ", idsToUpdate));

                    // Step 2: Update the status of all fetched IDs
                    if (idsToUpdate.Count > 0)
                    {
                        string updateQuery = "UPDATE shared_files SET status = @Status WHERE id IN (" + string.Join(",", idsToUpdate) + ")";
                        using (MySqlCommand cmd = new MySqlCommand(updateQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@Status", status);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show($"The folder and all its contents have been {status.ToLower()} successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Refresh the TableLayoutPanel
                    LoadPendingApprovals();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating the folder and its contents: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }






        private void docsApproval_Load(object sender, EventArgs e)
        {
            LoadPendingApprovals();
        }
    }
}
