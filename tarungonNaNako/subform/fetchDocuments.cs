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
using System.Timers;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using Org.BouncyCastle.Asn1.X509;

namespace tarungonNaNako.subform
{
    public partial class fetchDocuments : Form
    {
        private System.Windows.Forms.Timer fadeTimer;
        private TextBox txtFolderName;
        public string? FolderName { get; private set; }
        private fetchDocuments parentForm;

        //private FlowLayoutPanel breadcrumbPanel;
        public int currentCategoryId { get; private set; }
        private string selectedType = ""; // "file" or "category"
        private string selectedName = ""; // Holds fileName or categoryName
        private string selectedCategoryName;
        private string connectionString = "server=localhost;database=docsmanagement;uid=root;pwd=;";
        string ThreeDotMenu = Path.Combine(Application.StartupPath, "Assets (images)", "menu-dots-vertical.png");
        string FolderIcon = Path.Combine(Application.StartupPath, "Assets (images)", "folder.png");
        string DocumentIcon = Path.Combine(Application.StartupPath, "Assets (images)", "document.png");
        string ZippedFolderIcon = Path.Combine(Application.StartupPath, "Assets (images)", "zip-file-format.png");
        string HorizontalThreeDotMenu = Path.Combine(Application.StartupPath, "Assets (images)", "menu.png");
        private int selectedCategoryId; // Add this line to declare the variable
        private int parentCategoryId; // Add this line to declare the variable
        private string parentCategoryName; // Add this line to declare the variable
        private int breadcrumbItemsCount = 0; // Add this line to declare the variable
        string folderPath = @"C:\DocsManagement\";
        private BackgroundWorker backgroundWorker;
        private System.Timers.Timer debounceTimer;

        public fetchDocuments(int categoryId, string categoryName, string parentCategoryName)
        {
            int categoryId1 = categoryId;
            selectedCategoryId = categoryId1; // Store the categoryId to use later for loading files
            selectedCategoryName = categoryName; // Store the categoryName to display in the UI
            this.parentCategoryName = parentCategoryName; // Store the parentCategoryName to use later
            InitializeComponent();
            InitializeBackgroundWorker();
            InitializeDebounceTimer();
            searchBar.TextChanged += searchBar_TextChanged;
            this.Controls.Add(breadcrumbPanel);
            LoadFilesAndFoldersIntoTablePanel(); // Load files and folders specific to the selected category
            UpdateBreadcrumbs();
            searchBarPanel.Visible = false;
        }

        private void searchBarBtn_Click(object sender, EventArgs e)
        {
            searchBarPanel.Visible = !searchBarPanel.Visible;
        }

        private void searchBar_TextChanged(object sender, EventArgs e)
        {
            // Reset the debounce timer
            debounceTimer.Stop();
            debounceTimer.Start();
        }
        private void tableLayoutPanel1_Click(object sender, EventArgs e)
        {
            searchBarPanel.Hide();
            popupPanel.Hide();
            guna2Panel2.Hide();
        }

        private void InitializeBackgroundWorker()
        {
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
        }

        private void InitializeDebounceTimer()
        {
            debounceTimer = new System.Timers.Timer();
            debounceTimer.Interval = 500; // Set the debounce interval (in milliseconds)
            debounceTimer.AutoReset = false;
            debounceTimer.Elapsed += DebounceTimer_Elapsed;
        }

        private void DebounceTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Execute the operation on the UI thread
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    if (!backgroundWorker.IsBusy)
                    {
                        ShowLoadingAnimation();
                        backgroundWorker.RunWorkerAsync(searchBar.Text.Trim()); // Pass search term
                    }
                }));
            }
            else
            {
                if (!backgroundWorker.IsBusy)
                {
                    ShowLoadingAnimation();
                    backgroundWorker.RunWorkerAsync(searchBar.Text.Trim()); // Pass search term
                }
            }
        }


        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string searchTerm = e.Argument as string; // Retrieve search term
            LoadFilesAndFoldersIntoTablePanelInternal(searchTerm);
        }


        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    HideLoadingAnimation();
                }));
            }
            else
            {
                HideLoadingAnimation();
            }
        }

        private void ShowLoadingAnimation()
        {
            // Show a loading animation (e.g., a ProgressBar or a PictureBox with a GIF)
            // Implement this method based on your UI design
        }

        private void HideLoadingAnimation()
        {
            // Hide the loading animation
            // Implement this method based on your UI design
        }


        public int GetCurrentCategoryId()
        {
            // Logic to get the current category ID
            return currentCategoryId; // Make sure this variable exists in fetchDocuments
        }

        public void UpdateBreadcrumbs()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    breadcrumbPanel.Controls.Clear();
                    breadcrumbPanel.AutoSize = true;
                    breadcrumbPanel.Padding = new Padding(10);
                    breadcrumbPanel.BackColor = Color.FromArgb(255, 255, 192);

                    if (selectedCategoryId == -1) return;

                    BuildBreadcrumbTrail(selectedCategoryId);

                    Panel breadcrumbContainer = new Panel();
                    breadcrumbContainer.AutoSize = true;
                    breadcrumbContainer.Dock = DockStyle.Top;
                    breadcrumbContainer.Margin = new Padding(0, 50, 0, 0);
                    breadcrumbContainer.Padding = new Padding(0);

                    breadcrumbContainer.Controls.Add(breadcrumbPanel);
                    this.Controls.Add(breadcrumbContainer);
                }));
            }
            else
            {
                breadcrumbPanel.Controls.Clear();
                breadcrumbPanel.AutoSize = true;
                breadcrumbPanel.Padding = new Padding(10);
                breadcrumbPanel.BackColor = Color.FromArgb(255, 255, 192);

                if (selectedCategoryId == -1) return;

                BuildBreadcrumbTrail(selectedCategoryId);

                Panel breadcrumbContainer = new Panel();
                breadcrumbContainer.AutoSize = true;
                breadcrumbContainer.Dock = DockStyle.Top;
                breadcrumbContainer.Margin = new Padding(0, 50, 0, 0);
                breadcrumbContainer.Padding = new Padding(0);

                breadcrumbContainer.Controls.Add(breadcrumbPanel);
                this.Controls.Add(breadcrumbContainer);
            }
        }



        private void BuildBreadcrumbTrail(int categoryId)
        {
            List<Tuple<string, int>> breadcrumbItems = new List<Tuple<string, int>>();
            GetBreadcrumbItemsRecursive(categoryId, ref breadcrumbItems);

            breadcrumbPanel.Controls.Clear();

            breadcrumbItems.Reverse();
            if (breadcrumbItems.Count > 8)
            {
                Guna2CircleButton moreButton = CreateMoreButton(breadcrumbItems.Take(breadcrumbItems.Count - 4).ToList());
                breadcrumbPanel.Controls.Add(moreButton);
                breadcrumbItems = breadcrumbItems.Skip(breadcrumbItems.Count - 4).ToList();
            }

            for (int i = 0; i < breadcrumbItems.Count; i++)
            {
                string categoryName = breadcrumbItems[i].Item1;
                int currentCategoryId = breadcrumbItems[i].Item2;

                Guna2Button breadcrumbItem = CreateBreadcrumbItem(categoryName, currentCategoryId);
                breadcrumbPanel.Controls.Add(breadcrumbItem);

                if (i < breadcrumbItems.Count - 1)
                {
                    breadcrumbPanel.Controls.Add(new Label { Padding = new Padding(0, 8, 0, 0), Text = " > ", AutoSize = true });
                }
            }
        }

        //Modified CreateMoreButton to accept a list of hidden items
        private Guna2CircleButton CreateMoreButton(List<Tuple<string, int>> hiddenItems)
        {
            Guna2CircleButton moreButton = new Guna2CircleButton
            {
                Image = Image.FromFile(HorizontalThreeDotMenu),
                Margin = new Padding(0, 4, 5, 0),
                ImageOffset = new Point(1, 0),
                ImageAlign = HorizontalAlignment.Center,
                ImageSize = new Size(25, 25),
                BackgroundImageLayout = ImageLayout.Zoom,
                Size = new Size(30, 30),
                FillColor = Color.FromArgb(255, 226, 97),
                Tag = hiddenItems
            };
            moreButton.Click += (sender, e) => MoreButtonClick(sender, e, (List<Tuple<string, int>>)((Guna2CircleButton)sender).Tag);
            return moreButton;
        }


        private Guna2Button CreateBreadcrumbItem(string categoryName, int categoryId)
        {
            Guna2Button breadcrumbItem = new Guna2Button
            {
                Text = categoryName,
                AutoSize = true,
                Tag = categoryId,
                BorderThickness = 0,
                BorderRadius = 10,
                PressedDepth = 0,
                Margin = new Padding(-20, 4, -20, 0),
                FillColor = Color.FromArgb(255, 226, 97),
                BorderColor = Color.Transparent,
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            breadcrumbItem.Click += BreadcrumbItem_Click;
            return breadcrumbItem;
        }

        private void MoreButtonClick(object sender, EventArgs e, List<Tuple<string, int>> hiddenItems)
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip
            {
                BackColor = Color.LightYellow
            };

            foreach (var item in hiddenItems)
            {
                ToolStripMenuItem menuItem = new ToolStripMenuItem(item.Item1)
                {
                    Tag = item.Item2,
                    BackColor = Color.LightYellow // Set the background color of each item to a light color
                };
                menuItem.Click += (s, ev) =>
                {
                    int id = (int)((ToolStripMenuItem)s).Tag;
                    selectedCategoryId = id;
                    LoadFilesAndFoldersIntoTablePanel();
                    UpdateBreadcrumbs();
                };
                contextMenu.Items.Add(menuItem);
            }
            contextMenu.Show(Cursor.Position);
        }


        private void GetBreadcrumbItemsRecursive(int categoryId, ref List<Tuple<string, int>> breadcrumbItems)
        {
            string categoryName = GetCategoryName(categoryId);
            if (string.IsNullOrEmpty(categoryName)) return;

            breadcrumbItems.Add(new Tuple<string, int>(categoryName, categoryId));

            int parentCategoryId = GetParentCategoryId(categoryId);
            if (parentCategoryId != -1 && parentCategoryId != categoryId)
            {
                // Check for circular reference
                if (breadcrumbItems.Any(item => item.Item2 == parentCategoryId))
                {
                    MessageBox.Show("Circular reference detected in category hierarchy.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                GetBreadcrumbItemsRecursive(parentCategoryId, ref breadcrumbItems);
            }
        }


        private void BreadcrumbItem_Click(object sender, EventArgs e)
        {
            Guna2Button clickedBreadcrumb = sender as Guna2Button;
            if (clickedBreadcrumb != null)
            {
                if (clickedBreadcrumb.Tag != null && int.TryParse(clickedBreadcrumb.Tag.ToString(), out int categoryId))
                {
                    selectedCategoryId = categoryId;
                    LoadFilesAndFoldersIntoTablePanel();
                    UpdateBreadcrumbs();
                }
                else
                {
                    MessageBox.Show("Error: Invalid category ID in breadcrumb.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private string GetCategoryName(int categoryId)
        {
            string categoryName = "";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT categoryName FROM category WHERE categoryId = @categoryId";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@categoryId", categoryId);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            categoryName = reader.GetString("categoryName");
                        }
                    }
                }
            }
            return categoryName;
        }

        private int GetParentCategoryId(int categoryId)
        {
            int parentCategoryId = -1;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT parentCategoryId FROM category WHERE categoryId = @categoryId";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@categoryId", categoryId);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("parentCategoryId")))
                            {
                                parentCategoryId = reader.GetInt32("parentCategoryId");
                            }
                        }
                    }
                }
            }
            return parentCategoryId;
        }




        private int GetCategoryIdFromName(string categoryName)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT categoryId FROM category WHERE categoryName = @categoryName LIMIT 1";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@categoryName", categoryName);
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1; // Return -1 if not found
                }
            }
        }

        public void LoadFilesAndFoldersIntoTablePanel()
        {
            LoadFilesAndFoldersIntoTablePanelInternal();
        }

        private void LoadFilesAndFoldersIntoTablePanelInternal(string searchTerm = "")
        {
            int buttonWidth = 177; // Adjust as needed
            int buttonHeight = 60; // Adjust as needed
            int spacing = 10;
            //int xPosition = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                    SELECT 'file' AS type, f.fileName AS name, f.updated_at, c.categoryName
                    FROM files f
                    JOIN category c ON f.categoryId = c.categoryId
                    WHERE f.isArchived = 0 AND f.is_hidden = 0 AND f.categoryId = @categoryId
                        AND (@searchTerm IS NULL OR f.fileName LIKE @searchTerm)
                    UNION
                    SELECT 'category' AS type, c.categoryName AS name, c.updated_at, pc.categoryName AS parentCategoryName
                    FROM category c
                    LEFT JOIN category pc ON c.parentCategoryId = pc.categoryId
                    WHERE c.is_archived = 0 AND c.is_hidden = 0 AND c.parentCategoryId = @categoryId
                        AND (@searchTerm IS NULL OR c.categoryName LIKE @searchTerm)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@categoryId", selectedCategoryId);
                        cmd.Parameters.AddWithValue("@searchTerm", string.IsNullOrEmpty(searchTerm) ? null : "%" + searchTerm + "%");
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Clear existing rows
                            if (InvokeRequired)
                            {
                                Invoke(new Action(() =>
                                {
                                    tableLayoutPanel1.Controls.Clear();
                                    tableLayoutPanel1.RowCount = 0;
                                    tableLayoutPanel1.Padding = new Padding(0, 0, 0, 20);
                                    tableLayoutPanel1.RowStyles.Clear();
                                }));
                            }
                            else
                            {
                                tableLayoutPanel1.Controls.Clear();
                                tableLayoutPanel1.RowCount = 0;
                                tableLayoutPanel1.Padding = new Padding(0, 0, 0, 20);
                                tableLayoutPanel1.RowStyles.Clear();
                            }

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
                                    Height = 60, // Adjust row height
                                    BackColor = ColorTranslator.FromHtml("#ffe261")
                                };

                                // Set column sizes
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40)); // Image Icon
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200)); // Name
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180)); // Modification Time
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180)); // Category
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 10)); // Action

                                Image icon;
                                if (type == "file")
                                {
                                    string fileExtension = Path.GetExtension(name).ToLower();
                                    if (fileExtension == ".zip")
                                    {
                                        icon = Image.FromFile(ZippedFolderIcon); // Use zipped folder icon
                                    }
                                    else
                                    {
                                        icon = Image.FromFile(DocumentIcon); // Use document icon
                                    }
                                }
                                else
                                {
                                    icon = Image.FromFile(FolderIcon); // Use folder icon
                                }

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
                                    popupPanel.Hide();
                                    ShowPanel(type, name, categoryName, actionButton); // Show panel with file or folder-related options
                                };

                                if (type == "category")
                                {
                                    nameLabel.DoubleClick += (s, e) =>
                                    {
                                        int categoryId = GetCategoryIdByName(name);
                                        if (categoryId != -1)
                                        {
                                            selectedCategoryId = categoryId;
                                            guna2Panel2.Hide();
                                            LoadFilesAndFoldersIntoTablePanel();
                                            UpdateBreadcrumbs();
                                        }
                                        else
                                        {
                                            MessageBox.Show("Error: Category not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    };
                                    dateLabel.DoubleClick += (s, e) =>
                                    {
                                        int categoryId = GetCategoryIdByName(name);
                                        if (categoryId != -1)
                                        {
                                            selectedCategoryId = categoryId;
                                            guna2Panel2.Hide();
                                            LoadFilesAndFoldersIntoTablePanel();
                                            UpdateBreadcrumbs();
                                        }
                                        else
                                        {
                                            MessageBox.Show("Error: Category not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    };
                                    categoryLabel.DoubleClick += (s, e) =>
                                    {
                                        int categoryId = GetCategoryIdByName(name);
                                        if (categoryId != -1)
                                        {
                                            selectedCategoryId = categoryId;
                                            guna2Panel2.Hide();
                                            LoadFilesAndFoldersIntoTablePanel();
                                            UpdateBreadcrumbs();
                                        }
                                        else
                                        {
                                            MessageBox.Show("Error: Category not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    };
                                }

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
                                    Margin = new Padding(13, 17, 5, 5),
                                    Text = ":",
                                    Width = 25,
                                    Height = 25
                                };

                                rowTable.Controls.Add(iconPictureBox, 0, 0);
                                rowTable.Controls.Add(nameLabel, 1, 0);
                                rowTable.Controls.Add(dateLabel, 2, 0);
                                rowTable.Controls.Add(categoryLabel, 3, 0);
                                rowTable.Controls.Add(actionButton, 4, 0);

                                // Add rowTable to TableLayoutPanel
                                if (InvokeRequired)
                                {
                                    Invoke(new Action(() =>
                                    {
                                        tableLayoutPanel1.RowCount = rowIndex + 1;
                                        tableLayoutPanel1.Controls.Add(rowTable, 0, rowIndex);
                                        tableLayoutPanel1.SetColumnSpan(rowTable, 3); // Span all columns
                                    }));
                                }
                                else
                                {
                                    tableLayoutPanel1.RowCount = rowIndex + 1;
                                    tableLayoutPanel1.Controls.Add(rowTable, 0, rowIndex);
                                    tableLayoutPanel1.SetColumnSpan(rowTable, 3); // Span all columns
                                }

                                rowIndex++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show($"Error loading files and folders: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
                else
                {
                    MessageBox.Show($"Error loading files and folders: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }




        private void ShowPanel(string type, string name, string categoryName, Control btn)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    ShowPanelInternal(type, name, categoryName, btn);
                }));
            }
            else
            {
                ShowPanelInternal(type, name, categoryName, btn);
            }
        }

        private void ShowPanelInternal(string type, string name, string categoryName, Control btn)
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
            guna2Panel2.Size = new Size(181, 132); // Adjust size  
            guna2Panel2.BorderRadius = 5;
            guna2Panel2.BorderThickness = 1;
            guna2Panel2.BorderColor = Color.Black;
            guna2Panel2.BackColor = ColorTranslator.FromHtml("#ffe261");
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
            btnDownload.Size = new Size(177, 42);
            btnDownload.Text = "Download";
            btnDownload.TextAlign = HorizontalAlignment.Center;
            btnDownload.TextOffset = new Point(-18, 0);
            btnDownload.BackColor = Color.FromArgb(255, 236, 130);
            btnDownload.FillColor = Color.FromArgb(255, 236, 130);
            btnDownload.Font = new Font("Microsoft Sans Serif", 10);
            btnDownload.ForeColor = Color.Black;
            btnDownload.Image = Image.FromFile(download);
            btnDownload.ImageAlign = HorizontalAlignment.Left;
            btnDownload.ImageSize = new Size(15, 15);
            btnDownload.Location = new Point(2, 2);
            btnDownload.PressedColor = Color.Black;
            btnDownload.PressedDepth = 10;
            btnDownload.BorderRadius = 5;

            // Attach the event that handles downloading differently
            btnDownload.Click += DownloadButton_Click;

            Guna.UI2.WinForms.Guna2Button btnRename = new Guna.UI2.WinForms.Guna2Button();
            btnRename.Size = new Size(177, 42);
            btnRename.Text = "Rename";
            btnRename.TextAlign = HorizontalAlignment.Center;
            btnRename.TextOffset = new Point(-18, 0);
            btnRename.BackColor = Color.FromArgb(255, 236, 130);
            btnRename.FillColor = Color.FromArgb(255, 236, 130);
            btnRename.Font = new Font("Microsoft Sans Serif", 10);
            btnRename.ForeColor = Color.Black;
            btnRename.Image = Image.FromFile(rename);
            btnRename.ImageAlign = HorizontalAlignment.Left;
            btnRename.ImageSize = new Size(15, 15);
            btnRename.Location = new Point(2, 44);
            btnRename.PressedColor = Color.Black;
            btnRename.PressedDepth = 10;
            btnRename.BorderRadius = 5;

            // Attach the event that handles renaming differently
            btnRename.Click += (s, e) => RenameButton_Click();

            Guna.UI2.WinForms.Guna2Button btnDelete = new Guna.UI2.WinForms.Guna2Button();
            btnDelete.Size = new Size(177, 44);
            btnDelete.Text = "Move to trash";
            btnDelete.TextAlign = HorizontalAlignment.Right;
            btnDelete.TextOffset = new Point(-12, 0);
            btnDelete.BackColor = Color.FromArgb(255, 236, 130);
            btnDelete.FillColor = Color.FromArgb(255, 236, 130);
            btnDelete.Font = new Font("Microsoft Sans Serif", 10);
            btnDelete.ForeColor = Color.Black;
            btnDelete.Image = Image.FromFile(remove);
            btnDelete.ImageAlign = HorizontalAlignment.Left;
            btnDelete.ImageSize = new Size(15, 15);
            btnDelete.Location = new Point(2, 86);
            btnDelete.PressedColor = Color.Black;
            btnDelete.PressedDepth = 10;
            btnDelete.BorderRadius = 5;
            btnDelete.Click += (s, e) => RemoveButton_Click();

            // Add buttons to panel
            guna2Panel2.Controls.Add(btnDownload);
            guna2Panel2.Controls.Add(btnRename);
            guna2Panel2.Controls.Add(btnDelete);

            // Adjust Panel Position to Keep it Inside the Form
            Point btnScreenLocation = btn.Parent.PointToScreen(btn.Location);
            Point panelLocation = panel5.PointToClient(new Point(btnScreenLocation.X, btnScreenLocation.Y + btn.Height + 5));

            int panelX = panelLocation.X;
            int panelY = panelLocation.Y;
            int panelWidth = guna2Panel2.Width;
            int panelHeight = guna2Panel2.Height;

            // Ensure panel doesn't go beyond the right boundary
            if (panelX + panelWidth > panel5.ClientSize.Width)
            {
                panelX = panel5.ClientSize.Width - panelWidth - 40;
            }

            // Ensure panel doesn't go beyond the bottom boundary
            if (panelY + panelHeight > panel5.ClientSize.Height)
            {
                panelY = panel5.ClientSize.Height - panelHeight - 50;
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
            int categoryId = -1;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT categoryId FROM category WHERE categoryName = @categoryName";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@categoryName", categoryName);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                categoryId = reader.GetInt32("categoryId");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting category ID: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return categoryId;
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
            try
            {
                // Step 1: Retrieve the file path from the database
                string sourceFilePath = GetFilePathByFileName(fileName);

                if (string.IsNullOrEmpty(sourceFilePath) || !File.Exists(sourceFilePath))
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            MessageBox.Show("File not found!\nPath: " + sourceFilePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }));
                    }
                    else
                    {
                        MessageBox.Show("File not found!\nPath: " + sourceFilePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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

                        if (InvokeRequired)
                        {
                            Invoke(new Action(() =>
                            {
                                MessageBox.Show("File downloaded successfully!", "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }));
                        }
                        else
                        {
                            MessageBox.Show("File downloaded successfully!", "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                guna2Panel2.Hide();
            }
            catch (Exception ex)
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show($"An error occurred while downloading the file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
                else
                {
                    MessageBox.Show($"An error occurred while downloading the file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
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
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show($"Error retrieving file path: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
                else
                {
                    MessageBox.Show($"Error retrieving file path: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return filePath;
        }




        private void EditFile(int categoryId, string selectedName)
        {
            // Prompt user for new file name
            string newFileName = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter new file name:",
                "Edit File",
                selectedName
            ).Trim();

            if (string.IsNullOrWhiteSpace(newFileName))
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show("File name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
                else
                {
                    MessageBox.Show("File name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }

            // Ensure the new file name includes the correct extension
            string fileExtension = Path.GetExtension(selectedName);
            if (!newFileName.EndsWith(fileExtension, StringComparison.OrdinalIgnoreCase))
            {
                newFileName += fileExtension;
            }

            // Prevent duplicate file names
            if (IsFileNameExists(newFileName))
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show("A file with this name already exists. Please choose a different name.",
                                        "Duplicate File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }));
                }
                else
                {
                    MessageBox.Show("A file with this name already exists. Please choose a different name.",
                                    "Duplicate File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return;
            }

            try
            {
                // Step 1: Retrieve the current file path from the database
                string currentFilePath = GetFilePathByFileName(selectedName);
                if (string.IsNullOrEmpty(currentFilePath) || !File.Exists(currentFilePath))
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            MessageBox.Show("File not found in the file system.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }));
                    }
                    else
                    {
                        MessageBox.Show("File not found in the file system.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return;
                }

                // Step 2: Construct the new file path
                string newFilePath = Path.Combine(Path.GetDirectoryName(currentFilePath), newFileName);

                // Step 3: Rename the file in the file system
                File.Move(currentFilePath, newFilePath);

                // Step 4: Update the file name and path in the database
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE files SET fileName = @newFileName, filePath = @newFilePath WHERE fileName = @fileName";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@newFileName", newFileName);
                        cmd.Parameters.AddWithValue("@newFilePath", newFilePath);
                        cmd.Parameters.AddWithValue("@fileName", selectedName);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            if (InvokeRequired)
                            {
                                Invoke(new Action(() =>
                                {
                                    guna2Panel2.Visible = false;
                                    MessageBox.Show($"File renamed to '{newFileName}' successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    RefreshPanel5();
                                }));
                            }
                            else
                            {
                                guna2Panel2.Visible = false;
                                MessageBox.Show($"File renamed to '{newFileName}' successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                RefreshPanel5();
                            }
                        }
                        else
                        {
                            if (InvokeRequired)
                            {
                                Invoke(new Action(() =>
                                {
                                    MessageBox.Show("File not found in the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }));
                            }
                            else
                            {
                                MessageBox.Show("File not found in the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show($"Error renaming file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
                else
                {
                    MessageBox.Show($"Error renaming file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
                            if (InvokeRequired)
                            {
                                Invoke(new Action(() =>
                                {
                                    guna2Panel2.Visible = false;
                                    MessageBox.Show($"File '{fileName}' removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    LoadFilesAndFoldersIntoTablePanel(); // Refresh the panel after removing the file
                                }));
                            }
                            else
                            {
                                guna2Panel2.Visible = false;
                                MessageBox.Show($"File '{fileName}' removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadFilesAndFoldersIntoTablePanel(); // Refresh the panel after removing the file
                            }
                        }
                        else
                        {
                            if (InvokeRequired)
                            {
                                Invoke(new Action(() =>
                                {
                                    MessageBox.Show("File not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }));
                            }
                            else
                            {
                                MessageBox.Show("File not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show($"Error removing file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
                else
                {
                    MessageBox.Show($"Error removing file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void DownloadCategory(string categoryName)
        {
            try
            {
                // Step 1: Retrieve the folder path from the database
                string categoryPath = "";
                int categoryId = -1;
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT folderPath, categoryId FROM category WHERE categoryName = @categoryName";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@categoryName", categoryName);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                categoryPath = reader["folderPath"].ToString();
                                categoryId = Convert.ToInt32(reader["categoryId"]);
                            }
                        }
                    }
                }

                // Step 2: Validate the folder path
                if (string.IsNullOrEmpty(categoryPath) || !Directory.Exists(categoryPath))
                {
                    MessageBox.Show("Category folder not found on the file system.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Step 3: Retrieve and save documents from the database
                SaveDocumentsToFolder(categoryId, categoryPath);

                // Step 4: Use a temporary directory to prepare the contents for zipping
                string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                Directory.CreateDirectory(tempDirectory);

                // Copy the contents of the root folder (excluding the root folder itself) to the temp directory
                foreach (string dirPath in Directory.GetDirectories(categoryPath, "*", SearchOption.TopDirectoryOnly))
                {
                    string destDir = Path.Combine(tempDirectory, Path.GetFileName(dirPath));
                    DirectoryCopy(dirPath, destDir, true);
                }

                foreach (string filePath in Directory.GetFiles(categoryPath, "*", SearchOption.TopDirectoryOnly))
                {
                    string destFile = Path.Combine(tempDirectory, Path.GetFileName(filePath));
                    File.Copy(filePath, destFile, true);
                }

                // Step 5: Use SaveFileDialog to let the user choose where to save the ZIP file
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.FileName = $"{categoryName}.zip";
                    saveFileDialog.Filter = "ZIP files|*.zip";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string zipPath = saveFileDialog.FileName;

                        // Ensure no conflict by deleting any existing ZIP file at the destination
                        if (File.Exists(zipPath))
                        {
                            File.Delete(zipPath);
                        }

                        // Step 6: Compress the contents of the temp directory into a ZIP file
                        ZipFile.CreateFromDirectory(tempDirectory, zipPath);

                        MessageBox.Show("Category downloaded successfully as ZIP!", "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                // Step 7: Clean up the temporary directory
                if (Directory.Exists(tempDirectory))
                {
                    Directory.Delete(tempDirectory, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error downloading category: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveDocumentsToFolder(int categoryId, string folderPath)
        {
            try
            {

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Step 1: Retrieve all folders and their paths, excluding the root folder itself
                    Dictionary<int, string> folderPaths = RetrieveFolderPaths(conn, categoryId, folderPath);

                    // Step 2: Retrieve all files and save them in their respective folders
                    SaveFilesToFolders(conn, categoryId, folderPaths);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving documents to folder: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Dictionary<int, string> RetrieveFolderPaths(MySqlConnection conn, int categoryId, string rootFolderPath)
        {
            string folderQuery = @"
        WITH RECURSIVE FolderHierarchy AS (
            SELECT categoryId, parentCategoryId, categoryName
            FROM category
            WHERE categoryId = @categoryId
            UNION ALL
            SELECT c.categoryId, c.parentCategoryId, c.categoryName
            FROM category c
            INNER JOIN FolderHierarchy fh ON c.parentCategoryId = fh.categoryId
        )
        SELECT * FROM FolderHierarchy WHERE categoryId != @categoryId";

            Dictionary<int, string> folderPaths = new Dictionary<int, string>();

            using (MySqlCommand folderCmd = new MySqlCommand(folderQuery, conn))
            {
                folderCmd.Parameters.AddWithValue("@categoryId", categoryId);

                using (MySqlDataReader folderReader = folderCmd.ExecuteReader())
                {
                    while (folderReader.Read())
                    {
                        int id = folderReader.GetInt32("categoryId");
                        string categoryName = folderReader.GetString("categoryName");
                        int parentCategoryId = folderReader.IsDBNull(folderReader.GetOrdinal("parentCategoryId"))
                            ? -1
                            : folderReader.GetInt32("parentCategoryId");

                        // Determine the folder path
                        string parentPath = parentCategoryId == -1 || !folderPaths.ContainsKey(parentCategoryId)
                            ? rootFolderPath
                            : folderPaths[parentCategoryId];

                        string subFolderPath = Path.Combine(parentPath, categoryName);

                        folderPaths[id] = subFolderPath;

                        // Ensure the folder exists on the file system
                        if (!Directory.Exists(subFolderPath))
                        {
                            Directory.CreateDirectory(subFolderPath);
                        }
                    }
                }
            }

            return folderPaths;
        }

        private void SaveFilesToFolders(MySqlConnection conn, int categoryId, Dictionary<int, string> folderPaths)
        {
            string fileQuery = @"
        SELECT f.fileName, f.filePath, f.categoryId
        FROM files f
        WHERE f.categoryId IN (
            SELECT categoryId FROM category 
            WHERE categoryId = @categoryId OR parentCategoryId = @categoryId)";

            using (MySqlCommand fileCmd = new MySqlCommand(fileQuery, conn))
            {
                fileCmd.Parameters.AddWithValue("@categoryId", categoryId);

                using (MySqlDataReader fileReader = fileCmd.ExecuteReader())
                {
                    while (fileReader.Read())
                    {
                        string fileName = fileReader.GetString("fileName");
                        string filePath = fileReader.GetString("filePath");
                        int fileCategoryId = fileReader.GetInt32("categoryId");

                        // Ensure the file exists in the database path
                        if (File.Exists(filePath) && folderPaths.ContainsKey(fileCategoryId))
                        {
                            string destinationPath = Path.Combine(folderPaths[fileCategoryId], fileName);

                            // Avoid overwriting existing files
                            if (!File.Exists(destinationPath))
                            {
                                File.Copy(filePath, destinationPath);
                            }
                        }
                    }
                }
            }
        }




        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException($"Source directory does not exist or could not be found: {sourceDirName}");
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            Directory.CreateDirectory(destDirName);

            // Copy all the files
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }

            // If copying subdirectories, copy them and their contents
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, true);
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
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show("Category name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
                else
                {
                    MessageBox.Show("Category name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }

            // Prevent duplicate category names
            if (IsCategoryNameExists(newCategoryName))
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show("A category with this name already exists. Please choose a different name.",
                                        "Duplicate Category", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }));
                }
                else
                {
                    MessageBox.Show("A category with this name already exists. Please choose a different name.",
                                    "Duplicate Category", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return;
            }

            try
            {
                // Step 1: Retrieve the current folder path from the database
                string currentFolderPath = GetFolderPathByCategoryId(categoryId);
                if (string.IsNullOrEmpty(currentFolderPath) || !Directory.Exists(currentFolderPath))
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            MessageBox.Show("Category folder not found in the file system.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }));
                    }
                    else
                    {
                        MessageBox.Show("Category folder not found in the file system.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return;
                }

                // Step 2: Construct the new folder path
                string parentFolderPath = Path.GetDirectoryName(currentFolderPath);
                string newFolderPath = Path.Combine(parentFolderPath, newCategoryName);

                // Step 3: Rename the folder in the file system
                Directory.Move(currentFolderPath, newFolderPath);

                // Step 4: Update the category name and folder path in the database
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE category SET categoryName = @newCategoryName, folderPath = @newFolderPath WHERE categoryId = @categoryId";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@newCategoryName", newCategoryName);
                        cmd.Parameters.AddWithValue("@newFolderPath", newFolderPath);
                        cmd.Parameters.AddWithValue("@categoryId", categoryId);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            if (InvokeRequired)
                            {
                                Invoke(new Action(() =>
                                {
                                    guna2Panel2.Visible = false;
                                    MessageBox.Show($"Category renamed to '{newCategoryName}' successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    RefreshPanel5();
                                    LoadFilesAndFoldersIntoTablePanel();
                                }));
                            }
                            else
                            {
                                guna2Panel2.Visible = false;
                                MessageBox.Show($"Category renamed to '{newCategoryName}' successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                RefreshPanel5();
                                LoadFilesAndFoldersIntoTablePanel();
                            }
                        }
                        else
                        {
                            if (InvokeRequired)
                            {
                                Invoke(new Action(() =>
                                {
                                    MessageBox.Show("Category not found in the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }));
                            }
                            else
                            {
                                MessageBox.Show("Category not found in the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show("Access to the folder is denied. Please check folder permissions.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
                else
                {
                    MessageBox.Show("Access to the folder is denied. Please check folder permissions.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (IOException ioEx)
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show($"An I/O error occurred: {ioEx.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
                else
                {
                    MessageBox.Show($"An I/O error occurred: {ioEx.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show($"Error renaming category: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
                else
                {
                    MessageBox.Show($"Error renaming category: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void RemoveCategory(string categoryName)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Step 1: Get the categoryId and folderPath of the parent category
                    string getCategoryQuery = "SELECT categoryId, folderPath FROM category WHERE categoryName = @categoryName";
                    int? parentCategoryId = null;
                    string parentFolderPath = null;

                    using (MySqlCommand getCategoryCmd = new MySqlCommand(getCategoryQuery, conn))
                    {
                        getCategoryCmd.Parameters.AddWithValue("@categoryName", categoryName);
                        using (MySqlDataReader reader = getCategoryCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                parentCategoryId = reader.GetInt32(0);
                                parentFolderPath = reader.GetString(1);
                            }
                            else
                            {
                                MessageBox.Show($"Category '{categoryName}' not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }

                    // Step 2: Archive all subcategories recursively
                    string archiveSubcategoriesQuery = @"
                WITH RECURSIVE Subcategories AS (
                    SELECT categoryId, folderPath
                    FROM category
                    WHERE categoryId = @parentCategoryId
                    UNION ALL
                    SELECT c.categoryId, c.folderPath
                    FROM category c
                    INNER JOIN Subcategories sc ON c.parentCategoryId = sc.categoryId
                )
                SELECT categoryId, folderPath FROM Subcategories";

                    List<int> categoryIds = new List<int>();
                    List<string> folderPaths = new List<string>();

                    using (MySqlCommand archiveSubcategoriesCmd = new MySqlCommand(archiveSubcategoriesQuery, conn))
                    {
                        archiveSubcategoriesCmd.Parameters.AddWithValue("@parentCategoryId", parentCategoryId);

                        using (MySqlDataReader reader = archiveSubcategoriesCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                categoryIds.Add(reader.GetInt32(0));
                                folderPaths.Add(reader.GetString(1));
                            }
                        }
                    }

                    // Step 3: Update the categories using the fetched IDs
                    if (categoryIds.Count > 0)
                    {
                        string updateCategoriesQuery = "UPDATE category SET is_archived = 1 WHERE categoryId IN (" + string.Join(",", categoryIds) + ")";
                        using (MySqlCommand updateCategoriesCmd = new MySqlCommand(updateCategoriesQuery, conn))
                        {
                            updateCategoriesCmd.ExecuteNonQuery();
                        }
                    }

                    // Step 4: Archive all files in the parent category and its subcategories
                    string archiveFilesQuery = @"
                UPDATE files
                SET isArchived = 1
                WHERE categoryId IN (" + string.Join(",", categoryIds) + ")";

                    using (MySqlCommand archiveFilesCmd = new MySqlCommand(archiveFilesQuery, conn))
                    {
                        archiveFilesCmd.ExecuteNonQuery();
                    }

                    // Step 5: Delete the parent folder and its subfolders from the file system
                    foreach (string folderPath in folderPaths)
                    {
                        if (Directory.Exists(folderPath))
                        {
                            try
                            {
                                // Debugging: Log the folder path being deleted
                                Console.WriteLine($"Attempting to delete folder: {folderPath}");

                                Directory.Delete(folderPath, true); // Recursively delete the folder
                            }
                            catch (UnauthorizedAccessException ex)
                            {
                                MessageBox.Show($"Access denied to folder '{folderPath}'. Please check permissions.\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            catch (IOException ex)
                            {
                                MessageBox.Show($"Folder '{folderPath}' is in use or cannot be deleted.\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error deleting folder '{folderPath}': {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            // Debugging: Log if the folder does not exist
                            Console.WriteLine($"Folder does not exist: {folderPath}");
                        }
                    }

                    // Step 6: Notify the user and refresh the UI
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            guna2Panel2.Visible = false;
                            MessageBox.Show($"Category '{categoryName}' and its subfolders and files have been removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            RefreshPanel5(); // Refresh the panel after removing the category
                        }));
                    }
                    else
                    {
                        guna2Panel2.Visible = false;
                        MessageBox.Show($"Category '{categoryName}' and its subfolders and files have been removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshPanel5(); // Refresh the panel after removing the category
                    }
                }
            }
            catch (Exception ex)
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show($"Error removing category: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
                else
                {
                    MessageBox.Show($"Error removing category: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        public void RefreshPanel5()
        {
            // Reload files into the panel
            LoadFilesAndFoldersIntoTablePanel();
        }

        private void Newbtn_Click(object sender, EventArgs e)
        {
            guna2Panel2.Hide();
            CREATE_FOLDER_AND_UPLOAD_FILE_PANEL();
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

        private void CREATE_FOLDER_AND_UPLOAD_FILE_PANEL()
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
            popupPanel.Size = new Size(200, 150); // Adjust size
            popupPanel.BorderRadius = 5;
            popupPanel.Padding = new Padding(0, 0, 0, 0);
            popupPanel.BackColor = Color.FromArgb(255, 255, 192);
            popupPanel.BringToFront();
            popupPanel.Font = new Font("Segoe UI", 9);
            popupPanel.ForeColor = Color.Black;
            popupPanel.Location = new Point(277, 110);

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
            btnNewFolder.Click += (s, e) =>
            {
                int parentCategoryId = GetCategoryIdFromName(parentCategoryName);
                createChildFolder NewFolder = new createChildFolder
                {
                    ParentCategoryId = selectedCategoryId // Pass the parent category ID
                };
                NewFolder.StartPosition = FormStartPosition.CenterScreen;
                NewFolder.TopMost = true;
                NewFolder.FormBorderStyle = FormBorderStyle.FixedDialog;
                NewFolder.MinimizeBox = false;
                NewFolder.MaximizeBox = false;
                DialogResult result = NewFolder.ShowDialog(this); // Show the form as a dialog

                if (result == DialogResult.OK)
                {
                    // Optionally reload the folders here
                    LoadFilesAndFoldersIntoTablePanel(); // Your own method
                }

                popupPanel.Hide();
            };

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
            btnFileUpload.Click += (s, e) =>
            {
                // Step 1: Retrieve the folder path for the selected category
                string selectedFolderPath = GetFolderPathByCategoryId(selectedCategoryId);

                if (string.IsNullOrEmpty(selectedFolderPath) || !Directory.Exists(selectedFolderPath))
                {
                    MessageBox.Show("The selected folder does not exist on the file system.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Step 2: Pass the correct folder path to the UPLOAD_FILE_INSIDE_CHILD_FOLDER form
                UPLOAD_FILE_INSIDE_CHILD_FOLDER btnFileUpload = new UPLOAD_FILE_INSIDE_CHILD_FOLDER(selectedFolderPath)
                {
                    ParentCategoryId = selectedCategoryId // Pass the current category ID
                };
                btnFileUpload.StartPosition = FormStartPosition.CenterScreen;
                btnFileUpload.TopMost = true; // Ensure the form appears on top
                btnFileUpload.FormBorderStyle = FormBorderStyle.FixedDialog; // Set the form border style
                btnFileUpload.MinimizeBox = false; // Remove minimize button
                btnFileUpload.MaximizeBox = false; // Remove maximize button
                DialogResult result = btnFileUpload.ShowDialog(this); // Show the form as a dialog
                popupPanel.Hide();
                if (result == DialogResult.OK)
                {
                    LoadFilesAndFoldersIntoTablePanel(); // Call the refresh function after the dialog is closed
                }
            };


            Guna.UI2.WinForms.Guna2Button btnFolderUpload = new Guna.UI2.WinForms.Guna2Button
            {
                Size = new Size(181, 42),
                Text = "Zip Upload",
                TextAlign = HorizontalAlignment.Center,
                TextOffset = new Point(-13, 0),
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
            btnFolderUpload.Click += (s, e) =>
            {
                UPLOAD_FILE_INSIDE_CHILD_FOLDER btnFolderUpload = new UPLOAD_FILE_INSIDE_CHILD_FOLDER(folderPath)
                {
                    ParentCategoryId = selectedCategoryId // Pass the current category ID
                };
                btnFolderUpload.StartPosition = FormStartPosition.CenterScreen;
                btnFolderUpload.TopMost = true; // Ensure the form appears on top
                btnFolderUpload.FormBorderStyle = FormBorderStyle.FixedDialog; // Set the form border style
                btnFolderUpload.MinimizeBox = false; // Remove minimize button
                btnFolderUpload.MaximizeBox = false; // Remove maximize button
                DialogResult result = btnFolderUpload.ShowDialog(this);
                popupPanel.Hide();
                if (result == DialogResult.OK)
                {
                    LoadFilesAndFoldersIntoTablePanel(); // Call the refresh function after the dialog is closed
                }
            };

            // Add buttons to the panel
            popupPanel.Controls.Add(btnNewFolder);
            popupPanel.Controls.Add(btnFileUpload);
            popupPanel.Controls.Add(btnFolderUpload);

            // Add panel to the form
            this.Controls.Add(popupPanel);
            popupPanel.BringToFront();
        }

        private string GetFolderPathByCategoryId(int categoryId)
        {
            string folderPath = string.Empty;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT folderPath FROM category WHERE categoryId = @categoryId";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@categoryId", categoryId);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        folderPath = result.ToString();
                    }
                }
            }

            return folderPath;
        }



        private void panel5_Scroll(object sender, ScrollEventArgs e)
        {
            guna2Panel2.Hide();
            popupPanel.Hide();
        }

        private void panel3_Click(object sender, EventArgs e)
        {
            popupPanel.Hide();
            guna2Panel2.Hide();
        }

        private void fetchDocumentPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fetchDocumentPanel_Click(object sender, EventArgs e)
        {
            popupPanel.Hide();
            guna2Panel2.Hide();
        }

        private void breadcrumbPanel_Click(object sender, EventArgs e)
        {
            popupPanel.Hide();
            guna2Panel2.Hide();
        }

        private void guna2Panel2_Click(object sender, EventArgs e)
        {
            popupPanel.Hide();
        }

        private void panel5_Click(object sender, EventArgs e)
        {
            popupPanel.Hide();
            guna2Panel2.Hide();
        }
    }
}       
