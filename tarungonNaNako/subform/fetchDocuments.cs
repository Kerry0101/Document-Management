using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Compression;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;

namespace tarungonNaNako.subform
{
    public partial class fetchDocuments : Form
    {
        private string selectedType = ""; // "file" or "category"
        private string selectedName = ""; // Holds fileName or categoryName
        private string selectedCategoryName;
        private string connectionString = "server=localhost;database=docsmanagement;uid=root;pwd=;";
        string Document = Path.Combine(Application.StartupPath, "Assets (images)", "document.png");
        string ThreeDotMenu = Path.Combine(Application.StartupPath, "Assets (images)", "menu-dots-vertical.png");
        string FolderIcon = Path.Combine(Application.StartupPath, "Assets (images)", "folder.png");
        string DocumentIcon = Path.Combine(Application.StartupPath, "Assets (images)", "document.png");
        public fetchDocuments(string categoryName)
        {
            selectedCategoryName = categoryName;
            InitializeComponent();
            LoadFilesAndFoldersIntoTablePanel();
        }

        public fetchDocuments()
        {
        }

        //private void LoadFilesAndFoldersIntoTablePanel()
        //{
        //    int buttonWidth = 177; // Adjust as needed
        //    int buttonHeight = 60; // Adjust as needed
        //    int spacing = 10;
        //    int xPosition = 0;
        //    try
        //    {
        //        using (MySqlConnection conn = new MySqlConnection(connectionString))
        //        {
        //            conn.Open();
        //            string query = @"SELECT f.fileName, f.updated_at, c.categoryName
        //                         FROM files f
        //                         JOIN category c ON f.categoryId = c.categoryId
        //                         WHERE c.categoryName = @categoryName AND f.isArchived = 0";

        //            using (MySqlCommand cmd = new MySqlCommand(query, conn))
        //            {
        //                cmd.Parameters.AddWithValue("@categoryName", selectedCategoryName);
        //                using (MySqlDataReader reader = cmd.ExecuteReader())
        //                {
        //                    // Clear existing rows
        //                    tableLayoutPanel1.Controls.Clear();
        //                    tableLayoutPanel1.RowCount = 0;
        //                    tableLayoutPanel1.Padding = new Padding(0, 0, 0, 10);
        //                    tableLayoutPanel1.RowStyles.Clear();

        //                    int rowIndex = 0;

        //                    while (reader.Read())
        //                    {
        //                        string fileName = reader["fileName"].ToString();
        //                        string modificationTime = Convert.ToDateTime(reader["updated_at"]).ToString("yyyy-MM-dd hh:mm tt");
        //                        string categoryName = reader["categoryName"].ToString();

        //                        // ✅ Create TableLayoutPanel for Row
        //                        TableLayoutPanel rowTable = new TableLayoutPanel
        //                        {
        //                            ColumnCount = 5,
        //                            Dock = DockStyle.Fill,
        //                            Height = 50, // Adjust row height
        //                            BackColor = ColorTranslator.FromHtml("#ffe261")
        //                        };

        //                        // Set column sizes
        //                        rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40)); // Image Icon
        //                        rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200)); // File Name
        //                        rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180)); // Modification Time
        //                        rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180)); // Category
        //                        rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 10)); // Action

        //                        Image fileIcon = Image.FromFile(Document); // Load file icon

        //                        // 🔴 Create Labels (aligned properly)
        //                        Label fileLabel = new Label
        //                        {
        //                            Text = fileName,
        //                            Dock = DockStyle.Fill,
        //                            AutoSize = false,
        //                            TextAlign = ContentAlignment.MiddleLeft,
        //                            Padding = new Padding(5, 0, 0, 0),
        //                            Font = new Font("Microsoft Sans Serif", 10),
        //                            BackColor = Color.Transparent // Ensure background remains transparent
        //                        };

        //                        Label dateLabel = new Label
        //                        {
        //                            Text = modificationTime,
        //                            Dock = DockStyle.Fill,
        //                            AutoSize = false,
        //                            TextAlign = ContentAlignment.MiddleLeft,
        //                            Padding = new Padding(10, 0, 0, 0),
        //                            Font = new Font("Microsoft Sans Serif", 10),
        //                            BackColor = Color.Transparent
        //                        };

        //                        Label categoryLabel = new Label
        //                        {
        //                            Text = categoryName,
        //                            Dock = DockStyle.Fill,
        //                            AutoSize = false,
        //                            TextAlign = ContentAlignment.MiddleLeft,
        //                            Padding = new Padding(10, 0, 0, 0),
        //                            Font = new Font("Microsoft Sans Serif", 10),
        //                            BackColor = Color.Transparent
        //                        };

        //                        Guna.UI2.WinForms.Guna2CircleButton actionButton = new Guna.UI2.WinForms.Guna2CircleButton
        //                        {
        //                            Image = Image.FromFile(ThreeDotMenu),
        //                            ImageSize = new Size(15, 15),
        //                            ImageAlign = HorizontalAlignment.Center,
        //                            ImageOffset = new Point(0, 12),
        //                            BackColor = Color.FromArgb(255, 226, 97),
        //                            FillColor = Color.Transparent,
        //                            Size = new Size(30, 26),
        //                            Text = "⋮",
        //                            Anchor = AnchorStyles.Right, // This will align the button to the right
        //                            Margin = new Padding(0, 5, 30, 0), // Adjust the right margin if needed
        //                            PressedDepth = 10
        //                        };

        //                        actionButton.Click += (s, e) =>
        //                        {
        //                            //ShowContextMenu(fileName, actionButton);
        //                            ShowPanel("file", fileName, categoryName, actionButton); // Show panel with file-related options
        //                        };

        //                        // ✅ Add hover effect to row and its labels 255, 255, 192
        //                        void RowHover(object sender, EventArgs e)
        //                        {
        //                            rowTable.BackColor = Color.FromArgb(219, 195, 0);
        //                            actionButton.BackColor = Color.FromArgb(219, 195, 0);
        //                        }
        //                        void RowLeave(object sender, EventArgs e)
        //                        {
        //                            rowTable.BackColor = ColorTranslator.FromHtml("#ffe261");
        //                            actionButton.BackColor = ColorTranslator.FromHtml("#ffe261");
        //                        }

        //                        rowTable.MouseEnter += RowHover;
        //                        rowTable.MouseLeave += RowLeave;

        //                        fileLabel.MouseEnter += RowHover;
        //                        fileLabel.MouseLeave += RowLeave;

        //                        dateLabel.MouseEnter += RowHover;
        //                        dateLabel.MouseLeave += RowLeave;

        //                        categoryLabel.MouseEnter += RowHover;
        //                        categoryLabel.MouseLeave += RowLeave;

        //                        // ✅ Add Labels and Button to rowTable
        //                        PictureBox fileIconPictureBox = new PictureBox
        //                        {
        //                            Image = fileIcon,
        //                            SizeMode = PictureBoxSizeMode.Zoom, // Change to Zoom to maintain aspect ratio
        //                            Margin = new Padding(20, 18, 0, 5),
        //                            Width = 15, // Set desired width
        //                            Height = 15 // Set desired height
        //                        };

        //                        rowTable.Controls.Add(fileIconPictureBox, 0, 0);
        //                        rowTable.Controls.Add(fileLabel, 1, 0);
        //                        rowTable.Controls.Add(dateLabel, 2, 0);
        //                        rowTable.Controls.Add(categoryLabel, 3, 0);
        //                        rowTable.Controls.Add(actionButton, 4, 0);

        //                        // 🔴 Add rowTable to TableLayoutPanel
        //                        tableLayoutPanel1.RowCount = rowIndex + 1;
        //                        tableLayoutPanel1.Controls.Add(rowTable, 0, rowIndex);
        //                        tableLayoutPanel1.SetColumnSpan(rowTable, 3); // Span all columns

        //                        rowIndex++;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error loading files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        private void LoadFilesAndFoldersIntoTablePanel()
        {
            int buttonWidth = 177; // Adjust as needed
            int buttonHeight = 60; // Adjust as needed
            int spacing = 10;
            int xPosition = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                SELECT 'file' AS type, f.fileName AS name, f.updated_at AS updated_at, c.categoryName AS categoryName
                FROM files f
                JOIN category c ON f.categoryId = c.categoryId
                WHERE c.categoryName = @categoryName AND f.isArchived = 0
                UNION
                SELECT 'folder' AS type, s.subcategoryName AS name, s.updated_at AS updated_at, c.categoryName AS categoryName
                FROM subcategory s
                JOIN category c ON s.categoryId = c.categoryId
                WHERE c.categoryName = @categoryName AND s.is_archived = 0";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@categoryName", selectedCategoryName);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Clear existing rows
                            tableLayoutPanel1.Controls.Clear();
                            tableLayoutPanel1.RowCount = 0;
                            tableLayoutPanel1.Padding = new Padding(0, 0, 0, 10);
                            tableLayoutPanel1.RowStyles.Clear();

                            int rowIndex = 0;

                            while (reader.Read())
                            {
                                string type = reader["type"].ToString();
                                string name = reader["name"].ToString();
                                string modificationTime = Convert.ToDateTime(reader["updated_at"]).ToString("yyyy-MM-dd hh:mm tt");
                                string categoryName = reader["categoryName"].ToString();

                                // Create TableLayoutPanel for Row
                                TableLayoutPanel rowTable = new TableLayoutPanel
                                {
                                    ColumnCount = 5,
                                    Dock = DockStyle.Fill,
                                    Height = 50, // Adjust row height
                                    BackColor = ColorTranslator.FromHtml("#ffe261")
                                };

                                // Set column sizes
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40)); // Image Icon
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200)); // Name
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180)); // Modification Time
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180)); // Category
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 10)); // Action

                                Image icon = type == "file" ? Image.FromFile(Document) : Image.FromFile(FolderIcon); // Load appropriate icon

                                // Create Labels (aligned properly)
                                Label nameLabel = new Label
                                {
                                    Text = name,
                                    Dock = DockStyle.Fill,
                                    AutoSize = false,
                                    TextAlign = ContentAlignment.MiddleLeft,
                                    Padding = new Padding(5, 0, 0, 0),
                                    Font = new Font("Microsoft Sans Serif", 10),
                                    BackColor = Color.Transparent // Ensure background remains transparent
                                };

                                Label dateLabel = new Label
                                {
                                    Text = modificationTime,
                                    Dock = DockStyle.Fill,
                                    AutoSize = false,
                                    TextAlign = ContentAlignment.MiddleLeft,
                                    Padding = new Padding(10, 0, 0, 0),
                                    Font = new Font("Microsoft Sans Serif", 10),
                                    BackColor = Color.Transparent
                                };

                                Label categoryLabel = new Label
                                {
                                    Text = categoryName,
                                    Dock = DockStyle.Fill,
                                    AutoSize = false,
                                    TextAlign = ContentAlignment.MiddleLeft,
                                    Padding = new Padding(10, 0, 0, 0),
                                    Font = new Font("Microsoft Sans Serif", 10),
                                    BackColor = Color.Transparent
                                };

                                Guna.UI2.WinForms.Guna2CircleButton actionButton = new Guna.UI2.WinForms.Guna2CircleButton
                                {
                                    Image = Image.FromFile(ThreeDotMenu),
                                    ImageSize = new Size(15, 15),
                                    ImageAlign = HorizontalAlignment.Center,
                                    ImageOffset = new Point(0, 12),
                                    BackColor = Color.FromArgb(255, 226, 97),
                                    FillColor = Color.Transparent,
                                    Size = new Size(30, 26),
                                    Text = "⋮",
                                    Anchor = AnchorStyles.Right, // This will align the button to the right
                                    Margin = new Padding(0, 5, 30, 0), // Adjust the right margin if needed
                                    PressedDepth = 10
                                };

                                actionButton.Click += (s, e) =>
                                {
                                    ShowPanel(type, name, categoryName, actionButton); // Show panel with file or folder-related options
                                };

                                // Add hover effect to row and its labels
                                void RowHover(object sender, EventArgs e)
                                {
                                    rowTable.BackColor = Color.FromArgb(219, 195, 0);
                                    actionButton.BackColor = Color.FromArgb(219, 195, 0);
                                }
                                void RowLeave(object sender, EventArgs e)
                                {
                                    rowTable.BackColor = ColorTranslator.FromHtml("#ffe261");
                                    actionButton.BackColor = ColorTranslator.FromHtml("#ffe261");
                                }

                                rowTable.MouseEnter += RowHover;
                                rowTable.MouseLeave += RowLeave;

                                nameLabel.MouseEnter += RowHover;
                                nameLabel.MouseLeave += RowLeave;

                                dateLabel.MouseEnter += RowHover;
                                dateLabel.MouseLeave += RowLeave;

                                categoryLabel.MouseEnter += RowHover;
                                categoryLabel.MouseLeave += RowLeave;

                                // Add Labels and Button to rowTable
                                PictureBox iconPictureBox = new PictureBox
                                {
                                    Image = icon,
                                    SizeMode = PictureBoxSizeMode.Zoom, // Change to Zoom to maintain aspect ratio
                                    Margin = new Padding(20, 18, 0, 5),
                                    Width = 15, // Set desired width
                                    Height = 15 // Set desired height
                                };

                                rowTable.Controls.Add(iconPictureBox, 0, 0);
                                rowTable.Controls.Add(nameLabel, 1, 0);
                                rowTable.Controls.Add(dateLabel, 2, 0);
                                rowTable.Controls.Add(categoryLabel, 3, 0);
                                rowTable.Controls.Add(actionButton, 4, 0);

                                // Add rowTable to TableLayoutPanel
                                tableLayoutPanel1.RowCount = rowIndex + 1;
                                tableLayoutPanel1.Controls.Add(rowTable, 0, rowIndex);
                                tableLayoutPanel1.SetColumnSpan(rowTable, 3); // Span all columns

                                rowIndex++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading files and folders: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void FetchSubcategoryDocuments(string subcategoryName)
        {
            selectedCategoryName = subcategoryName;
            LoadFilesAndFoldersIntoTablePanel(); // Reload files for this subcategory
        }


        private void ShowPanel(string type, string name, string categoryName, Control btn)
        {
            selectedType = type; // Store what was clicked
            selectedName = name; // Store file or category name
                                 // Toggle visibility of guna2Panel2
            if (guna2Panel2.Visible)
            {
                guna2Panel2.Visible = false;
                return;
            }

            string download = Path.Combine(Application.StartupPath, "Assets (images)", "down-to-line.png");
            string rename = Path.Combine(Application.StartupPath, "Assets (images)", "pencil.png");
            string remove = Path.Combine(Application.StartupPath, "Assets (images)", "trash.png");

            guna2Panel2.Controls.Clear(); // Clear previous content
            guna2Panel2.Visible = true;   // Show the panel

            // Set panel properties
            guna2Panel2.Size = new Size(181, 132); // Adjust size     181, 132
            guna2Panel2.BorderRadius = 5;
            guna2Panel2.BackColor = Color.FromArgb(255, 255, 192);
            guna2Panel2.BringToFront();
            guna2Panel2.Font = new Font("Segoe UI", 9);
            guna2Panel2.ForeColor = Color.Black;

            Label titleLabel = new Label
            {
                Text = (type == "file") ? "File Options" : "Category Options",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Create buttons
            Guna.UI2.WinForms.Guna2Button btnDownload = new Guna.UI2.WinForms.Guna2Button();
            btnDownload.Size = new Size(181, 42);
            btnDownload.Text = "Download";
            btnDownload.TextAlign = HorizontalAlignment.Center;
            btnDownload.TextOffset = new Point(-18, 0);
            btnDownload.BackColor = Color.FromArgb(255, 255, 192);
            btnDownload.FillColor = Color.FromArgb(255, 236, 130);
            btnDownload.Font = new Font("Microsoft Sans Serif", 10);
            btnDownload.ForeColor = Color.Black;
            btnDownload.Image = Image.FromFile(download);
            btnDownload.ImageAlign = HorizontalAlignment.Left;
            btnDownload.ImageSize = new Size(15, 15);
            btnDownload.Location = new Point(0, 1);
            btnDownload.PressedColor = Color.Black;
            btnDownload.PressedDepth = 10;

            // ✅ Attach the event that handles downloading differently
            btnDownload.Click += DownloadButton_Click;

            Guna.UI2.WinForms.Guna2Button btnRename = new Guna.UI2.WinForms.Guna2Button();
            btnRename.Size = new Size(181, 42);
            btnRename.Text = "Rename";
            btnRename.TextAlign = HorizontalAlignment.Center;
            btnRename.TextOffset = new Point(-18, 0);
            btnRename.BackColor = Color.FromArgb(255, 255, 192);
            btnRename.FillColor = Color.FromArgb(255, 236, 130);
            btnRename.Font = new Font("Microsoft Sans Serif", 10);
            btnRename.ForeColor = Color.Black;
            btnRename.Image = Image.FromFile(rename);
            btnRename.ImageAlign = HorizontalAlignment.Left;
            btnRename.ImageSize = new Size(15, 15);
            btnRename.Location = new Point(0, 44);
            btnRename.PressedColor = Color.Black;
            btnRename.PressedDepth = 10;

            // ✅ Attach the event that handles renaming differently
            btnRename.Click += (s, e) => RenameButton_Click();

            Guna.UI2.WinForms.Guna2Button btnDelete = new Guna.UI2.WinForms.Guna2Button();
            btnDelete.Size = new Size(181, 42);
            btnDelete.Text = "Move to trash";
            btnDelete.TextAlign = HorizontalAlignment.Right;
            btnDelete.TextOffset = new Point(-12, 0);
            btnDelete.BackColor = Color.FromArgb(255, 255, 192);
            btnDelete.FillColor = Color.FromArgb(255, 236, 130);
            btnDelete.Font = new Font("Microsoft Sans Serif", 10);
            btnDelete.ForeColor = Color.Black;
            btnDelete.Image = Image.FromFile(remove);
            btnDelete.ImageAlign = HorizontalAlignment.Left;
            btnDelete.ImageSize = new Size(15, 15);
            btnDelete.Location = new Point(0, 87);
            btnDelete.PressedColor = Color.Black;
            btnDelete.PressedDepth = 10;
            btnDelete.Click += (s, e) => RemoveButton_Click();

            // Add buttons to panel
            guna2Panel2.Controls.Add(btnDownload);
            guna2Panel2.Controls.Add(btnRename);
            guna2Panel2.Controls.Add(btnDelete);

            // ===== Adjust Panel Position to Keep it Inside the Form =====
            Point btnScreenLocation = btn.Parent.PointToScreen(btn.Location);
            Point panelLocation = this.PointToClient(new Point(btnScreenLocation.X, btnScreenLocation.Y + btn.Height + 5));

            int panelX = panelLocation.X;
            int panelY = panelLocation.Y;
            int panelWidth = guna2Panel2.Width;
            int panelHeight = guna2Panel2.Height;

            // Ensure panel doesn't go beyond the right boundary
            if (panelX + panelWidth > this.ClientSize.Width)
            {
                panelX = this.ClientSize.Width - panelWidth - 10;
            }

            // Ensure panel doesn't go beyond the bottom boundary
            if (panelY + panelHeight > this.ClientSize.Height)
            {
                panelY = this.ClientSize.Height - panelHeight - 10;
            }

            // Apply final position
            guna2Panel2.Location = new Point(panelX, panelY);
            guna2Panel2.Visible = true;
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }


        private void DownloadButton_Click(object sender, EventArgs e)
        {
            if (selectedType == "file")
            {
                DownloadFile(selectedName);
            }
            else if (selectedType == "category")
            {
                DownloadCategory(selectedName);
            }
        }
        private int GetCategoryIdByName(string categoryName)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT categoryId FROM category WHERE categoryName = @categoryName";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@categoryName", categoryName);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        private bool IsFileNameExists(string fileName)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM files WHERE fileName = @fileName";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@fileName", fileName);
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        private bool IsCategoryNameExists(string categoryName)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM category WHERE categoryName = @categoryName";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@categoryName", categoryName);
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        private void RenameButton_Click()
        {
            if (selectedType == "file")
            {
                EditFile(GetCategoryIdByName(selectedName), selectedName);
            }
            else if (selectedType == "category")
            {
                EditCategory(GetCategoryIdByName(selectedName), selectedName);
            }
        }

        private void RemoveButton_Click()
        {
            if (selectedType == "file")
            {
                RemoveFile(selectedName);
            }
            else if (selectedType == "category")
            {
                RemoveCategory(selectedName);
            }
        }

        private void DownloadFile(string fileName)
        {
            string storagePath = @"C:\\DocsManagement\\"; // Ensure this path is correct
            string sourceFilePath = Path.Combine(storagePath, fileName);

            // Debugging: Show the path to confirm it's correct
            MessageBox.Show($"Checking file path: {sourceFilePath}", "Debug Info");

            if (!File.Exists(sourceFilePath))
            {
                MessageBox.Show("File not found!\nPath: " + sourceFilePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.FileName = fileName;
                saveFileDialog.Filter = "All Files|*.*";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string destinationPath = saveFileDialog.FileName;
                    File.Copy(sourceFilePath, destinationPath, true);
                    MessageBox.Show("File downloaded successfully!", "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }



        private void EditFile(int categoryId, string selectedName)
        {             // Prompt user for new file name
            string newFileName = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter new file name:",
                "Edit File",
                selectedName
            ).Trim();
            if (string.IsNullOrWhiteSpace(newFileName))
            {
                MessageBox.Show("File name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Prevent duplicate file names
            if (IsFileNameExists(newFileName))
            {
                MessageBox.Show("A file with this name already exists. Please choose a different name.",
                                "Duplicate File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE files SET fileName = @newFileName WHERE fileName = @fileName";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@newFileName", newFileName);
                        cmd.Parameters.AddWithValue("@fileName", selectedName);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            guna2Panel2.Visible = false;
                            MessageBox.Show($"File renamed to '{newFileName}' successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadFilesAndFoldersIntoTablePanel(); // Refresh the panel after renaming the file
                        }
                        else
                        {
                            MessageBox.Show("File not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error renaming file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void RemoveFile(string fileName)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE files SET isArchived = 1 WHERE fileName = @fileName";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@fileName", fileName);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            guna2Panel2.Visible = false;
                            MessageBox.Show($"File '{fileName}' removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadFilesAndFoldersIntoTablePanel(); // Refresh the panel after removing the file
                        }
                        else
                        {
                            MessageBox.Show("File not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void DownloadCategory(string categoryName)
        {
            string storagePath = @"C:\DocSpace\UploadedFiles\"; // Ensure this is correct
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
        }


        private void EditCategory(int categoryId, string categoryName)
        {
            // Prompt user for new category name
            string newCategoryName = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter new category name:",
                "Edit Category",
                categoryName
            ).Trim();

            if (string.IsNullOrWhiteSpace(newCategoryName))
            {
                MessageBox.Show("Category name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Prevent duplicate category names
            if (IsCategoryNameExists(newCategoryName))
            {
                MessageBox.Show("A category with this name already exists. Please choose a different name.",
                                "Duplicate Category", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE category SET categoryName = @newCategoryName WHERE categoryid = @categoryId";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@newCategoryName", newCategoryName);
                        cmd.Parameters.AddWithValue("@categoryId", categoryId);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            guna2Panel2.Visible = false;
                            MessageBox.Show($"Category renamed to '{newCategoryName}' successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            RefreshPanel5();
                            LoadFilesAndFoldersIntoTablePanel();
                        }
                        else
                        {
                            MessageBox.Show("Category not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error renaming category: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RemoveCategory(string categoryName)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE category SET is_archived = 1 WHERE categoryName = @categoryName";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@categoryName", categoryName);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            guna2Panel2.Visible = false;
                            MessageBox.Show($"Category '{categoryName}' removed successfully.");
                            RefreshPanel5(); // Refresh panel5 after removing the category
                        }
                        else
                        {
                            MessageBox.Show($"Category '{categoryName}' not found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error archiving category: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void RefreshPanel5()
        {
            // Clear existing controls
            panel5.Controls.Clear();
             
            // Reload files into the panel
            LoadFilesAndFoldersIntoTablePanel();
        }

    }
}
