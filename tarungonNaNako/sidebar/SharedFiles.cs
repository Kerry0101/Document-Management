using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using tarungonNaNako.subform;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace tarungonNaNako.sidebar
{
    public partial class SharedFiles : Form
    {

        string ThreeDotMenu = Path.Combine(Application.StartupPath, "Assets (images)", "menu-dots-vertical.png");
        private readonly string connectionString = "server=localhost;user=root;database=docsmanagement;password=";
        public SharedFiles()
        {
            InitializeComponent();
        }        
        
        private void SharedFiles_Load(object sender, EventArgs e)
        {
            LoadApprovedFiles();
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

        private void LoadApprovedFiles()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("server=localhost;database=docsmanagement;uid=root;pwd=;"))
                {
                    conn.Open();

                    string query = @"
            SELECT
                sf.id,              -- The ID from shared_files
                sf.file_name,       -- The name stored in shared_files
                u.username AS shared_by,
                r.roleName AS user_role,
                sf.created_at AS dateShared,
                sf.status,
                sf.is_folder,
                sf.categoryId,      -- The linked categoryId
                c.categoryName      -- The actual categoryName from the 'category' table
            FROM
                shared_files sf
            JOIN
                users u ON sf.userId = u.userId
            JOIN
                roles r ON u.roleId = r.roleId
            LEFT JOIN
                category c ON sf.categoryId = c.categoryId -- Join with category table
            WHERE
                sf.status = 'Approved' -- Only load approved files
                AND sf.is_folder = 1   -- Only display folders
                AND sf.parentId IS NULL -- Only root-level folders (no parent folder)
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
                                string sharedBy = reader.GetString("shared_by");
                                string userRole = reader.GetString("user_role");
                                DateTime dateShared = reader.GetDateTime("dateShared");
                                bool isFolder = reader.GetBoolean("is_folder");
                                string status = reader.GetString("status");

                                int? categoryId = reader.IsDBNull(reader.GetOrdinal("categoryId")) ? (int?)null : reader.GetInt32("categoryId");
                                string categoryName = reader.IsDBNull(reader.GetOrdinal("categoryName")) ? "N/A" : reader.GetString("categoryName");

                                // Create a Panel for the row
                                Panel rowPanel = new Panel
                                {
                                    Dock = DockStyle.Fill,
                                    BackColor = ColorTranslator.FromHtml("#ffe261"),
                                    Height = fixedRowHeight,
                                    Padding = new Padding(0, 0, 10, 0),
                                    Margin = new Padding(0, 0, 9, 5),
                                    Tag = new Tuple<int?, string>(categoryId, categoryName)
                                };

                                // Double-click to navigate to the folder or file
                                rowPanel.DoubleClick += (s, e) =>
                                {
                                    Panel clickedPanel = s as Panel;
                                    if (clickedPanel?.Tag is Tuple<int?, string> categoryInfo)
                                    {
                                        int? cId = categoryInfo.Item1;
                                        string cName = categoryInfo.Item2;

                                        if (cId.HasValue)
                                        {
                                            // Navigate to the category or load its contents
                                            LoadFormInPanel(new fetchDocuments(cId.Value, cName, ""));
                                        }
                                        else
                                        {
                                            MessageBox.Show("This file is not linked to a category.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    }
                                };

                                // Create Label for file name
                                Label fileNameLabel = new Label
                                {
                                    Text = sharedFileName,
                                    Dock = DockStyle.Left,
                                    AutoSize = false,
                                    Font = new Font("Microsoft Sans Serif", 10),
                                    Width = 280,
                                    TextAlign = ContentAlignment.MiddleLeft,
                                    Padding = new Padding(10, 0, 0, 0)
                                };

                                // Create Label for shared by
                                Label sharedByLabel = new Label
                                {
                                    Text = $"{sharedBy} ({userRole})",
                                    Dock = DockStyle.Left,
                                    AutoSize = false,
                                    Font = new Font("Microsoft Sans Serif", 10),
                                    Width = 200,
                                    TextAlign = ContentAlignment.MiddleLeft,
                                    Padding = new Padding(10, 0, 0, 0)
                                };

                                // Create Label for date shared
                                Label dateSharedLabel = new Label
                                {
                                    Text = dateShared.ToString("yyyy-MM-dd hh:mm tt"),
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

                                Guna2CircleButton actionButton = new Guna2CircleButton
                                {
                                    Image = Image.FromFile(ThreeDotMenu),
                                    ImageSize = new Size(15, 15),
                                    ImageAlign = HorizontalAlignment.Center,
                                    ImageOffset = new Point(0, 12),
                                    BackColor = Color.FromArgb(255, 226, 97),
                                    FillColor = Color.Transparent,
                                    Size = new Size(30, 26),
                                    Text = "⋮",
                                    Anchor = AnchorStyles.Right,
                                    Margin = new Padding(0, 5, 0, 0),
                                    PressedDepth = 10,
                                    Location = new Point(rowPanel.Width - 65, 17),
                                };

                                actionButton.Click += (s, e) =>
                                {
                                    ShowPanel("category", categoryName, actionButton, categoryId, categoryName);
                                };

                                // Add hover effects to the rowPanel and all child controls
                                void ApplyHoverEffect(Control control)
                                {
                                    control.MouseEnter += (s, e) =>
                                    {
                                        rowPanel.BackColor = Color.FromArgb(219, 195, 0);
                                        actionButton.BackColor = Color.FromArgb(219, 195, 0);
                                    };
                                    control.MouseLeave += (s, e) =>
                                    {
                                        rowPanel.BackColor = ColorTranslator.FromHtml("#ffe261");
                                        actionButton.BackColor = ColorTranslator.FromHtml("#ffe261");
                                    };
                                }

                                // Apply hover effect to all child controls
                                ApplyHoverEffect(rowPanel);
                                ApplyHoverEffect(fileNameLabel);
                                ApplyHoverEffect(sharedByLabel);
                                ApplyHoverEffect(dateSharedLabel);
                                ApplyHoverEffect(statusLabel);
                                ApplyHoverEffect(actionButton);

                                // Add controls to the rowPanel
                                rowPanel.Controls.Add(statusLabel);
                                rowPanel.Controls.Add(actionButton);
                                rowPanel.Controls.Add(dateSharedLabel);
                                rowPanel.Controls.Add(sharedByLabel);
                                rowPanel.Controls.Add(fileNameLabel);

                                actionButton.BringToFront();

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
                MessageBox.Show($"Error loading approved files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ShowPanel(string type, string name, Control btn, int? categoryId, string categoryName)
        {
            // Store the selected type and name
            string selectedType = type; // "file" or "category"
            string selectedName = name;

            // Toggle visibility of guna2Panel2
            if (guna2Panel2.Visible)
            {
                guna2Panel2.Visible = false;
                return;
            }

            // Paths for button icons
            string downloadIcon = Path.Combine(Application.StartupPath, "Assets (images)", "down-to-line.png");
            string viewIcon = Path.Combine(Application.StartupPath, "Assets (images)", "folder-open.png");

            // Clear previous content and show the panel
            guna2Panel2.Controls.Clear();
            guna2Panel2.Visible = true;

            // Set panel properties
            guna2Panel2.Size = new Size(181, 89); // Adjust size for two buttons
            guna2Panel2.BorderRadius = 5;
            guna2Panel2.BorderThickness = 1;
            guna2Panel2.BorderColor = Color.Black;
            guna2Panel2.BackColor = ColorTranslator.FromHtml("#ffe261");
            guna2Panel2.BringToFront();
            guna2Panel2.Font = new Font("Segoe UI", 9);
            guna2Panel2.ForeColor = Color.Black;

            // Create "Download" button
            Guna2Button btnDownload = new Guna2Button
            {
                Size = new Size(177, 42),
                Text = "Download",
                TextAlign = HorizontalAlignment.Left,
                TextOffset = new Point(10, 0),
                BackColor = Color.FromArgb(255, 236, 130),
                FillColor = Color.FromArgb(255, 236, 130),
                Font = new Font("Microsoft Sans Serif", 10),
                ForeColor = Color.Black,
                Image = Image.FromFile(downloadIcon),
                ImageAlign = HorizontalAlignment.Left,
                ImageSize = new Size(15, 15),
                Location = new Point(2, 3),
                PressedColor = Color.Black,
                PressedDepth = 10,
                BorderRadius = 5
            };

            // Attach event handler for "Download" button
            btnDownload.Click += (s, e) =>
            {
                if (selectedType == "file")
                {
                    DownloadFile(selectedName); // Call method to download file
                }
                else if (selectedType == "category")
                {
                    DownloadCategory(selectedName); // Call method to download category
                }
                guna2Panel2.Hide(); // Hide the panel after action
            };

            // Create "View" button
            Guna2Button btnView = new Guna2Button
            {
                Size = new Size(177, 42),
                Text = "View",
                TextAlign = HorizontalAlignment.Left,
                TextOffset = new Point(6, 0),
                BackColor = Color.FromArgb(255, 236, 130),
                FillColor = Color.FromArgb(255, 236, 130),
                Font = new Font("Microsoft Sans Serif", 10),
                ForeColor = Color.Black,
                Image = Image.FromFile(viewIcon),
                ImageAlign = HorizontalAlignment.Left,
                ImageSize = new Size(20, 20),
                Location = new Point(2, 45),
                PressedColor = Color.Black,
                PressedDepth = 10,
                BorderRadius = 5,
                Tag = new Tuple<int?, string>(categoryId, categoryName)
            };

            // Attach event handler for "View" button
            btnView.Click += (s, e) =>
            {
                // Retrieve the parent panel from the button's Tag
                if (btnView.Tag is Tuple<int?, string> categoryInfo)
                {
                    int? cId = categoryInfo.Item1;
                    string cName = categoryInfo.Item2;

                    if (cId.HasValue)
                    {
                        // Navigate to the category or load its contents
                        LoadFormInPanel(new fetchDocuments(cId.Value, cName, ""));
                    }
                    else
                    {
                        MessageBox.Show("This file is not linked to a category.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            };

            // Add buttons to the panel
            guna2Panel2.Controls.Add(btnDownload);
            guna2Panel2.Controls.Add(btnView);

            // Adjust panel position to keep it inside the form
            Point btnScreenLocation = btn.Parent.PointToScreen(btn.Location);
            Point panelLocation = this.PointToClient(new Point(btnScreenLocation.X, btnScreenLocation.Y + btn.Height + -60));

            int panelX = panelLocation.X;
            int panelY = panelLocation.Y;
            int panelWidth = guna2Panel2.Width;
            int panelHeight = guna2Panel2.Height;

            // Ensure panel doesn't go beyond the right boundary
            if (panelX + panelWidth > this.ClientSize.Width)
            {
                panelX = this.ClientSize.Width - panelWidth - 60;
            }

            // Ensure panel doesn't go beyond the bottom boundary
            if (panelY + panelHeight > this.ClientSize.Height)
            {
                panelY = this.ClientSize.Height - panelHeight - 60;
            }

            // Apply final position
            guna2Panel2.Location = new Point(panelX, panelY);
            guna2Panel2.Visible = true;
        }

        private int GetCategoryIdByName(string categoryName)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT categoryId FROM category WHERE categoryName = @categoryName AND userId = @userId AND is_archived = 0 AND is_hidden = 0";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@categoryName", categoryName);
                        cmd.Parameters.AddWithValue("@userId", Session.CurrentUserId);

                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            return Convert.ToInt32(result);
                        }
                        else
                        {
                            MessageBox.Show($"Category '{categoryName}' not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return -1; // Return -1 if the category is not found
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving category ID: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1; // Return -1 in case of an error
            }
        }

        private void DownloadFile(string fileName)
        {
            try
            {
                // Step 1: Retrieve the file path from the database
                string sourceFilePath = GetFilePathByFileName(fileName);

                if (string.IsNullOrEmpty(sourceFilePath) || !File.Exists(sourceFilePath))
                {
                    MessageBox.Show("File not found!\nPath: " + sourceFilePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Step 2: Prompt the user to select a save location
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.FileName = fileName;
                    saveFileDialog.Filter = "All Files|*.*";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string destinationPath = saveFileDialog.FileName;

                        // Step 3: Copy the file to the selected location
                        File.Copy(sourceFilePath, destinationPath, true);

                        MessageBox.Show("File downloaded successfully!", "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while downloading the file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            guna2Panel2.Hide();
        }

        private string GetFilePathByFileName(string fileName)
        {
            string filePath = string.Empty;

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT filePath FROM files WHERE fileName = @fileName";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@fileName", fileName);
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            filePath = result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving file path: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return filePath;
        }

        private void DownloadCategory(string categoryName)
        {
            string storagePath = @"C:\DocsManagement"; // Ensure this is correct
            string categoryPath = Path.Combine(storagePath, categoryName);

            // Debugging: Show the path to confirm it's correct
            MessageBox.Show($"Checking category path: {categoryPath}", "Debug Info");

            if (!Directory.Exists(categoryPath))
            {
                MessageBox.Show("Category folder not found!\nPath: " + categoryPath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.FileName = $"{categoryName}.zip";
                saveFileDialog.Filter = "ZIP files|*.zip";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string zipPath = saveFileDialog.FileName;

                    if (File.Exists(zipPath))
                        File.Delete(zipPath); // Ensure no conflict

                    ZipFile.CreateFromDirectory(categoryPath, zipPath);
                    MessageBox.Show("Category downloaded successfully as ZIP!", "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            guna2Panel2.Hide();
        }



    }
}
