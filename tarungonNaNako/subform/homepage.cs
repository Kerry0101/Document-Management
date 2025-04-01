using MySql.Data.MySqlClient;
using System.IO;
using System.IO.Compression;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using tarungonNaNako.sidebar;
using System.Xml.Linq;

namespace tarungonNaNako.subform
{
    public partial class homepage : Form
    {
        private string storagePath = @"C:\\DocSpace\\DocsManagement\\";
        private string selectedType = ""; // "file" or "category"
        private string selectedName = ""; // Holds fileName or categoryName
        private string connectionString = "server=localhost;database=docsmanagement;uid=root;pwd=;";
        private Image originalImage;
        string Folder = Path.Combine(Application.StartupPath, "Assets (images)", "folder.png");
        string Document = Path.Combine(Application.StartupPath, "Assets (images)", "document.png");
        string ThreeDotMenu = Path.Combine(Application.StartupPath, "Assets (images)", "menu-dots-vertical.png");


        public homepage()

        {
            InitializeComponent();
            LoadPngImage();
            LoadFilesIntoTablePanel();
        }


        public void LoadFormInPanel(Form form)
        {
            // Clear previous controls in the panel (replace "panel5" with your content panel name)
            panel3.Controls.Clear();

            // Set properties for the form to display it in the panel
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            panel3.Controls.Add(form);
            form.Show();
        }

        private void homepage_Load(object sender, EventArgs e)
        {
            Guna2Panel1.Visible = Properties.Settings.Default.isPanel1Visible;
            panel5.Visible = Properties.Settings.Default.isPanel5Visible;

            float rotationAngle2 = Properties.Settings.Default.isArrowDown2 ? 0 : -90;
            pictureBox2.Image = RotateImage(originalImage, rotationAngle2);

            float rotationAngle3 = Properties.Settings.Default.isArrowDown3 ? 0 : -90;
            pictureBox3.Image = RotateImage(originalImage, rotationAngle3);

            LoadCategoriesIntoButtons();  // Load categories dynamically
        }

        private void LoadFilesIntoTablePanel()
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
                    string query = @"SELECT f.fileName, f.updated_at, c.categoryName
                                     FROM files f
                                     JOIN category c ON f.categoryId = c.categoryId
                                     WHERE f.isArchived = 0";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
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
                                string fileName = reader["fileName"].ToString();
                                string modificationTime = Convert.ToDateTime(reader["updated_at"]).ToString("yyyy-MM-dd hh:mm tt");
                                string categoryName = reader["categoryName"].ToString();

                                // ✅ Create TableLayoutPanel for Row
                                TableLayoutPanel rowTable = new TableLayoutPanel
                                {
                                    ColumnCount = 5,
                                    Dock = DockStyle.Fill,
                                    Height = 50, // Adjust row height
                                    BackColor = ColorTranslator.FromHtml("#ffe261")
                                };

                                // Set column sizes
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40)); // Image Icon
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200)); // File Name
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180)); // Modification Time
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180)); // Category
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 10)); // Action

                                Image fileIcon = Image.FromFile(Document); // Load file icon

                                // 🔴 Create Labels (aligned properly)
                                Label fileLabel = new Label
                                {
                                    Text = fileName,
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
                                    //ShowContextMenu(fileName, actionButton);
                                    ShowPanel("file", fileName, categoryName, actionButton); // Show panel with file-related options
                                };

                                // ✅ Add hover effect to row and its labels 255, 255, 192
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

                                fileLabel.MouseEnter += RowHover;
                                fileLabel.MouseLeave += RowLeave;

                                dateLabel.MouseEnter += RowHover;
                                dateLabel.MouseLeave += RowLeave;

                                categoryLabel.MouseEnter += RowHover;
                                categoryLabel.MouseLeave += RowLeave;

                                // ✅ Add Labels and Button to rowTable
                                PictureBox fileIconPictureBox = new PictureBox
                                {
                                    Image = fileIcon,
                                    SizeMode = PictureBoxSizeMode.Zoom, // Change to Zoom to maintain aspect ratio
                                    Margin = new Padding(20, 18, 0, 5),
                                    Width = 15, // Set desired width
                                    Height = 15 // Set desired height
                                };

                                rowTable.Controls.Add(fileIconPictureBox, 0, 0);
                                rowTable.Controls.Add(fileLabel, 1, 0);
                                rowTable.Controls.Add(dateLabel, 2, 0);
                                rowTable.Controls.Add(categoryLabel, 3, 0);
                                rowTable.Controls.Add(actionButton, 4, 0);

                                // 🔴 Add rowTable to TableLayoutPanel
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
                MessageBox.Show($"Error loading files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void LoadCategoriesIntoButtons()
        {
            Guna2Panel1.Controls.Clear(); // Clear previous buttons
            int buttonWidth = 177; // Adjust as needed
            int buttonHeight = 60; // Adjust as needed
            int spacing = 10;
            int xPosition = 0;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Ensure the user is logged in
                    if (Session.CurrentUserId == 0)
                    {
                        MessageBox.Show("User is not logged in.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string query = @"
                SELECT categoryName 
                FROM category 
                WHERE is_archived = 0 
                AND userId = @userId 
                ORDER BY created_at DESC 
                LIMIT 5"; // Fetch only the 5 most recent categories

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", Session.CurrentUserId); // Filter by logged-in user

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string categoryName = reader["categoryName"].ToString();

                                // ✅ Create Category Button
                                Guna.UI2.WinForms.Guna2Button categoryButton = new Guna.UI2.WinForms.Guna2Button
                                {
                                    Text = categoryName,
                                    Width = buttonWidth,
                                    Height = buttonHeight,
                                    BorderRadius = 10,
                                    PressedDepth = 0,
                                    FillColor = Color.FromArgb(255, 226, 97),
                                    ForeColor = Color.Black,
                                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                                    Image = Image.FromFile(Folder),
                                    ImageSize = new Size(15, 15),
                                    ImageAlign = HorizontalAlignment.Left,
                                    TextAlign = HorizontalAlignment.Left,
                                    Location = new Point(xPosition, 0),
                                    Cursor = Cursors.Hand
                                };

                                categoryButton.DoubleClick += (s, e) =>
                                {
                                    LoadFormInPanel(new fetchDocuments(categoryName));
                                };

                                // ✅ Create Three-Dot Menu Button
                                Guna.UI2.WinForms.Guna2CircleButton menuButton = new Guna.UI2.WinForms.Guna2CircleButton
                                {
                                    Image = Image.FromFile(ThreeDotMenu),
                                    ImageSize = new Size(15, 15),
                                    ImageAlign = HorizontalAlignment.Center,
                                    ImageOffset = new Point(0, 12),
                                    BackColor = Color.FromArgb(255, 226, 97),
                                    FillColor = Color.Transparent,
                                    Size = new Size(28, 26),
                                    Location = new Point(xPosition + buttonWidth - 35, 15),
                                    Text = "⋮",
                                    PressedDepth = 10
                                };

                                menuButton.Click += (s, e) =>
                                {
                                    ShowPanel("category", categoryName, null, menuButton);
                                    popupPanel.Hide();
                                };

                                // ✅ Attach Hover Effects
                                categoryButton.MouseEnter += (s, e) => menuButton.BackColor = Color.FromArgb(219, 195, 0);
                                categoryButton.MouseLeave += (s, e) => menuButton.BackColor = ColorTranslator.FromHtml("#ffe261");

                                // ✅ Add Controls to Panel
                                Guna2Panel1.Controls.Add(menuButton);
                                Guna2Panel1.Controls.Add(categoryButton);

                                xPosition += buttonWidth + spacing;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        //private void ShowPanel(string type, string name, string categoryName, Control btn)
        //{
        //    selectedType = type; // Store what was clicked
        //    selectedName = name; // Store file or category name
        //                         // Toggle visibility of guna2Panel2
        //    if (guna2Panel2.Visible)
        //    {
        //        guna2Panel2.Visible = false;
        //        return;
        //    }

        //    string download = Path.Combine(Application.StartupPath, "Assets (images)", "down-to-line.png");
        //    string rename = Path.Combine(Application.StartupPath, "Assets (images)", "pencil.png");
        //    string remove = Path.Combine(Application.StartupPath, "Assets (images)", "trash.png");

        //    guna2Panel2.Controls.Clear(); // Clear previous content
        //    guna2Panel2.Visible = true;   // Show the panel

        //    // Set panel properties
        //    guna2Panel2.Size = new Size(181, 132); // Adjust size     181, 132
        //    guna2Panel2.BorderRadius = 5;
        //    guna2Panel2.BackColor = Color.FromArgb(255, 255, 192);
        //    guna2Panel2.BringToFront();
        //    guna2Panel2.Font = new Font("Segoe UI", 9);
        //    guna2Panel2.ForeColor = Color.Black;



        //    Label titleLabel = new Label
        //    {
        //        Text = (type == "file") ? "File Options" : "Category Options",
        //        Font = new Font("Segoe UI", 12, FontStyle.Bold),
        //        Dock = DockStyle.Top,
        //        TextAlign = ContentAlignment.MiddleCenter
        //    };

        //    // Create buttons
        //    Guna.UI2.WinForms.Guna2Button btnDownload = new Guna.UI2.WinForms.Guna2Button();
        //    btnDownload.Size = new Size(181, 42);
        //    btnDownload.Text = "Download";
        //    btnDownload.TextAlign = HorizontalAlignment.Center;
        //    btnDownload.TextOffset = new Point(-18, 0);
        //    btnDownload.BackColor = Color.FromArgb(255, 255, 192);
        //    btnDownload.FillColor = Color.FromArgb(255, 236, 130);
        //    btnDownload.Font = new Font("Microsoft Sans Serif", 10);
        //    btnDownload.ForeColor = Color.Black;
        //    btnDownload.Image = Image.FromFile(download);
        //    btnDownload.ImageAlign = HorizontalAlignment.Left;
        //    btnDownload.ImageSize = new Size(15, 15);
        //    btnDownload.Location = new Point(0, 1);
        //    btnDownload.PressedColor = Color.Black;
        //    btnDownload.PressedDepth = 10;

        //    // ✅ Attach the event that handles downloading differently
        //    btnDownload.Click += DownloadButton_Click;

        //    Guna.UI2.WinForms.Guna2Button btnRename = new Guna.UI2.WinForms.Guna2Button();
        //    btnRename.Size = new Size(181, 42);
        //    btnRename.Text = "Rename";
        //    btnRename.TextAlign = HorizontalAlignment.Center;
        //    btnRename.TextOffset = new Point(-18, 0);
        //    btnRename.BackColor = Color.FromArgb(255, 255, 192);
        //    btnRename.FillColor = Color.FromArgb(255, 236, 130);
        //    btnRename.Font = new Font("Microsoft Sans Serif", 10);
        //    btnRename.ForeColor = Color.Black;
        //    btnRename.Image = Image.FromFile(rename);
        //    btnRename.ImageAlign = HorizontalAlignment.Left;
        //    btnRename.ImageSize = new Size(15, 15);
        //    btnRename.Location = new Point(0, 44);
        //    btnRename.PressedColor = Color.Black;
        //    btnRename.PressedDepth = 10;
        //    btnRename.Click += (s, e) => EditCategory(GetCategoryIdByName(categoryName), categoryName);

        //    Guna.UI2.WinForms.Guna2Button btnDelete = new Guna.UI2.WinForms.Guna2Button();
        //    btnDelete.Size = new Size(181, 42);
        //    btnDelete.Text = "Move to trash";
        //    btnDelete.TextAlign = HorizontalAlignment.Right;
        //    btnDelete.TextOffset = new Point(-12, 0);
        //    btnDelete.BackColor = Color.FromArgb(255, 255, 192);
        //    btnDelete.FillColor = Color.FromArgb(255, 236, 130);
        //    btnDelete.Font = new Font("Microsoft Sans Serif", 10);
        //    btnDelete.ForeColor = Color.Black;
        //    btnDelete.Image = Image.FromFile(remove);
        //    btnDelete.ImageAlign = HorizontalAlignment.Left;
        //    btnDelete.ImageSize = new Size(15, 15);
        //    btnDelete.Location = new Point(0, 87);
        //    btnDelete.PressedColor = Color.Black;
        //    btnDelete.PressedDepth = 10;
        //    btnDelete.Click += (s, e) => RemoveCategory(categoryName);

        //    // Add buttons to panel
        //    guna2Panel2.Controls.Add(btnDownload);
        //    guna2Panel2.Controls.Add(btnRename);
        //    guna2Panel2.Controls.Add(btnDelete);

        //    // ===== Adjust Panel Position to Keep it Inside the Form =====
        //    Point btnScreenLocation = btn.Parent.PointToScreen(btn.Location);
        //    Point panelLocation = this.PointToClient(new Point(btnScreenLocation.X, btnScreenLocation.Y + btn.Height + 5));

        //    int panelX = panelLocation.X;
        //    int panelY = panelLocation.Y;
        //    int panelWidth = guna2Panel2.Width;
        //    int panelHeight = guna2Panel2.Height;

        //    // Ensure panel doesn't go beyond the right boundary
        //    if (panelX + panelWidth > this.ClientSize.Width)
        //    {
        //        panelX = this.ClientSize.Width - panelWidth - 10;
        //    }

        //    // Ensure panel doesn't go beyond the bottom boundary
        //    if (panelY + panelHeight > this.ClientSize.Height)
        //    {
        //        panelY = this.ClientSize.Height - panelHeight - 10;
        //    }

        //    // Apply final position
        //    guna2Panel2.Location = new Point(panelX, panelY);
        //    guna2Panel2.Visible = true;

        //}

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
                            LoadFilesIntoTablePanel(); // Refresh the panel after removing the file
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







        private void LoadPngImage()
        {
            originalImage = Image.FromFile("C:\\Users\\teche\\source\\repos\\tarungonNaNako\\tarungonNaNako\\Assets (images)\\dropdown-select.png");
            pictureBox2.Image = originalImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.Image = originalImage;
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Guna2Panel1.Visible = !Guna2Panel1.Visible;
            Properties.Settings.Default.isPanel1Visible = Guna2Panel1.Visible;

            Properties.Settings.Default.isArrowDown2 = !Properties.Settings.Default.isArrowDown2;
            float rotationAngle = Properties.Settings.Default.isArrowDown2 ? 0 : -90;
            pictureBox2.Image = RotateImage(originalImage, rotationAngle);

            Properties.Settings.Default.Save(); // Save changes
            guna2Panel2.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

            panel5.Visible = !panel5.Visible;
            Properties.Settings.Default.isPanel5Visible = panel5.Visible;

            Properties.Settings.Default.isArrowDown3 = !Properties.Settings.Default.isArrowDown3;
            float rotationAngle = Properties.Settings.Default.isArrowDown3 ? 0 : -90;
            pictureBox3.Image = RotateImage(originalImage, rotationAngle);

            Properties.Settings.Default.Save(); // Save changes
            guna2Panel2.Hide();
        }


        private Image RotateImage(Image img, float rotationAngle)
        {
            // Create a new empty bitmap to hold rotated image
            Bitmap bmp = new Bitmap(img.Width, img.Height);
            bmp.SetResolution(img.HorizontalResolution, img.VerticalResolution);

            // Make a graphics object from the empty bitmap
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent); // Clear the background to avoid overlapping
                // Move rotation point to center of image
                g.TranslateTransform((float)img.Width / 2, (float)img.Height / 2);
                // Rotate
                g.RotateTransform(rotationAngle);
                // Move image back
                g.TranslateTransform(-(float)img.Width / 2, -(float)img.Height / 2);
                // Draw passed in image onto graphics object
                g.DrawImage(img, new Point(0, 0));
            }

            return bmp;
        }


        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

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
                            LoadFilesIntoTablePanel(); // Refresh the panel after renaming the file
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
                            RefreshCategoriesPanel();
                            LoadFilesIntoTablePanel();
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

        // Helper method to check for duplicate category names
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


        //private void EditCategory(string categoryName)
        //{
        //    // Prompt user for new category name
        //    string newCategoryName = Microsoft.VisualBasic.Interaction.InputBox(
        //        "Enter new category name:",
        //        "Edit Category",
        //        categoryName
        //    );

        //    if (string.IsNullOrWhiteSpace(newCategoryName))
        //    {
        //        MessageBox.Show("Category name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }

        //    try
        //    {
        //        using (MySqlConnection conn = new MySqlConnection(connectionString))
        //        {
        //            conn.Open();
        //            string query = "UPDATE category SET categoryName = @newCategoryName WHERE categoryName = @categoryName";
        //            using (MySqlCommand cmd = new MySqlCommand(query, conn))
        //            {
        //                cmd.Parameters.AddWithValue("@newCategoryName", newCategoryName);
        //                cmd.Parameters.AddWithValue("@categoryName", categoryName);
        //                int rowsAffected = cmd.ExecuteNonQuery();
        //                if (rowsAffected > 0)
        //                {
        //                    MessageBox.Show($"Category '{categoryName}' renamed to '{newCategoryName}' successfully.");
        //                }
        //                else
        //                {
        //                    MessageBox.Show($"Category '{categoryName}' not found.");
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error renaming category: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

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
                            RefreshCategoriesPanel(); // Refresh the panel after removing the category
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



        private void RefreshCategoriesPanel()
        {
            LoadCategoriesIntoButtons();
        }



        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Guna2Panel1_Scroll(object sender, ScrollEventArgs e)
        {
            guna2Panel2.Visible = false;
        }

        private void homepage_Click(object sender, EventArgs e)
        {
            guna2Panel2.Hide();
        }

        private void panel3_Click(object sender, EventArgs e)
        {
            guna2Panel2.Hide();
            popupPanel.Hide();
        }

        private void panel5_Scroll(object sender, ScrollEventArgs e)
        {
            guna2Panel2.Hide();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }

        private void panel5_Click(object sender, EventArgs e)
        {
            guna2Panel2.Hide();
        }

        private void Guna2Panel1_Click(object sender, EventArgs e)
        {
            guna2Panel2.Hide();
        }

        private void Newbtn_Click(object sender, EventArgs e)
        {
            guna2Panel2.Hide();
            CREATE_FOLDER_AND_FILE_PANEL();
        }

        private void CREATE_FOLDER_AND_FILE_PANEL()
        {

            if (popupPanel.Visible)
            {
                popupPanel.Visible = false;
                return;
            }

            string newFolder = Path.Combine(Application.StartupPath, "Assets (images)", "newfolder.png");
            string fileUpload = Path.Combine(Application.StartupPath, "Assets (images)", "fileupload.png");
            string folderUpload = Path.Combine(Application.StartupPath, "Assets (images)", "folderupload.png");

            popupPanel.Controls.Clear(); // Clear previous content
            popupPanel.Visible = true;   // Show the panel


            // Set panel properties
            popupPanel.Size = new Size(200, 150); // Adjust size     181, 132
            popupPanel.BorderRadius = 5;
            popupPanel.Padding = new Padding(0,0,0,0);
            popupPanel.BackColor = Color.FromArgb(255, 255, 192);
            popupPanel.BringToFront();
            popupPanel.Font = new Font("Segoe UI", 9);
            popupPanel.ForeColor = Color.Black;

            // Create buttons
            Guna.UI2.WinForms.Guna2Button btnNewFolder = new Guna.UI2.WinForms.Guna2Button
            {
                Size = new Size(181, 42),
                Text = "New Folder",
                TextAlign = HorizontalAlignment.Center,
                TextOffset = new Point(-12, 0),
                BackColor = Color.FromArgb(255, 255, 192),
                FillColor = Color.FromArgb(255, 236, 130),
                Font = new Font("Microsoft Sans Serif", 10),
                ForeColor = Color.Black,
                Image = Image.FromFile(newFolder),
                ImageAlign = HorizontalAlignment.Left,
                ImageSize = new Size(15, 15),
                Location = new Point(10, 10),
                PressedColor = Color.Black,
                PressedDepth = 10,
            };
            btnNewFolder.Click += (s, e) => { MessageBox.Show("New Folder Clicked"); };

            Guna.UI2.WinForms.Guna2Button btnFileUpload = new Guna.UI2.WinForms.Guna2Button
            {
                Size = new Size(181, 42),
                Text = "File Upload",
                TextAlign = HorizontalAlignment.Center,
                TextOffset = new Point(-12, 0),
                BackColor = Color.FromArgb(255, 255, 192),
                FillColor = Color.FromArgb(255, 236, 130),
                Font = new Font("Microsoft Sans Serif", 10),
                ForeColor = Color.Black,
                Image = Image.FromFile(fileUpload),
                ImageAlign = HorizontalAlignment.Left,
                ImageSize = new Size(15, 15),
                Location = new Point(10, 55),
                PressedColor = Color.Black,
                PressedDepth = 10,
            };
            btnFileUpload.Click += (s, e) => {
                LoadFormInPanel(new addDocs());
            };

            Guna.UI2.WinForms.Guna2Button btnFolderUpload = new Guna.UI2.WinForms.Guna2Button
            {
                Size = new Size(181, 42),
                Text = "Folder Upload",
                TextAlign = HorizontalAlignment.Center,
                TextOffset = new Point(-1, 0),
                BackColor = Color.FromArgb(255, 255, 192),
                FillColor = Color.FromArgb(255, 236, 130),
                Font = new Font("Microsoft Sans Serif", 10),
                ForeColor = Color.Black,
                Image = Image.FromFile(folderUpload),
                ImageAlign = HorizontalAlignment.Left,
                ImageSize = new Size(15, 15),
                Location = new Point(10, 100),
                PressedColor = Color.Black,
                PressedDepth = 10,
            };
            btnFolderUpload.Click += (s, e) => { MessageBox.Show("Folder Upload Clicked"); };

            // Add buttons to the panel
            popupPanel.Controls.Add(btnNewFolder);
            popupPanel.Controls.Add(btnFileUpload);
            popupPanel.Controls.Add(btnFolderUpload);

            // Add panel to the form
            this.Controls.Add(popupPanel);
            popupPanel.BringToFront();
        }

        private void Guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
