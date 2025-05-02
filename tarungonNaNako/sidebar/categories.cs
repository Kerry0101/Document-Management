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
using System.Timers;
using Guna.UI2.WinForms;
using System.IO.Compression;

namespace tarungonNaNako.sidebar
{
    public partial class categories : Form
    {
        private BackgroundWorker backgroundWorker;
        private System.Timers.Timer debounceTimer;
        private bool isSortByNameAscending = false;
        string ThreeDotMenu = Path.Combine(Application.StartupPath, "Assets (images)", "menu-dots-vertical.png");
        private readonly string connectionString = "server=localhost;user=root;database=docsmanagement;password=";
        string FolderIcon = Path.Combine(Application.StartupPath, "Assets (images)", "folder.png");
        string ZippedFolderIcon = Path.Combine(Application.StartupPath, "Assets (images)", "zip-file-format.png");
        string download = Path.Combine(Application.StartupPath, "Assets (images)", "down-to-line.png");
        private string selectedName = "";

        public categories()
        {
            InitializeComponent();
            InitializeBackgroundWorker();
            loadingPictureBox.Visible = false;
            searchBar.TextChanged += SearchBar_TextChanged;

            debounceTimer = new System.Timers.Timer();
            debounceTimer.Interval = 500; // Set the debounce interval (in milliseconds)
            debounceTimer.AutoReset = false;
            debounceTimer.Elapsed += DebounceTimer_Elapsed;
            popupPanel.Visible = false;
        }

        private void SearchBar_TextChanged(object sender, EventArgs e)
        {
            // Reset the debounce timer
            debounceTimer.Stop();
            debounceTimer.Start();
        }

        private void DebounceTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Execute the search on the UI thread
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    if (!backgroundWorker.IsBusy)
                    {
                        ShowLoadingAnimation();
                        backgroundWorker.RunWorkerAsync(searchBar.Text);
                    }
                }));
            }
            else
            {
                if (!backgroundWorker.IsBusy)
                {
                    ShowLoadingAnimation();
                    backgroundWorker.RunWorkerAsync(searchBar.Text);
                }
            }
        }

        private void InitializeBackgroundWorker()
        {
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string searchTerm = e.Argument as string;
            LoadCategoriesInternal(currentCategoryId, searchTerm);
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
            loadingPictureBox.Visible = true;
        }

        private void HideLoadingAnimation()
        {
            // Hide the loading animation
            loadingPictureBox.Visible = false;
        }

        private int? currentCategoryId = null; // NULL means Root Folder
        private Stack<int?> navigationHistory = new Stack<int?>(); // Stack to track navigation

        public void NavigateToCategory(int categoryId)
        {
            navigationHistory.Push(currentCategoryId); // Store current category before navigating
            currentCategoryId = categoryId; // Update current category
            LoadCategoriesInternal(currentCategoryId); // Reload with new category
        }

        private void LoadCategories(string searchTerm = "")
        {
            LoadCategoriesInternal(currentCategoryId);
        }

        private void LoadCategoriesInternal(int? currentCategoryId = null, string searchTerm = "")
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => LoadCategoriesInternal(currentCategoryId, searchTerm)));
                return;
            }

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

                    // Update the query to include sorting and search logic
                    string query = @"
            SELECT categoryId, categoryName, created_at
            FROM category
            WHERE userId = @userId
            AND is_archived = 0
            AND is_hidden = 0
            AND (@currentCategoryId IS NULL OR parentCategoryId = @currentCategoryId)
            AND categoryName LIKE @searchTerm
            ORDER BY " + (isSortByNameAscending ? "categoryName ASC" : "created_at DESC");

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", Session.CurrentUserId);
                        cmd.Parameters.AddWithValue("@currentCategoryId", currentCategoryId.HasValue ? (object)currentCategoryId.Value : DBNull.Value);
                        cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Clear existing rows
                            tableLayoutPanel1.Controls.Clear();
                            tableLayoutPanel1.RowCount = 0;
                            tableLayoutPanel1.RowStyles.Clear();

                            int rowIndex = 0;

                            while (reader.Read())
                            {
                                int categoryId = reader.GetInt32("categoryId");
                                string categoryName = reader.GetString("categoryName");
                                string createdAt = Convert.ToDateTime(reader["created_at"]).ToString("yyyy-MM-dd hh:mm tt");

                                // Create TableLayoutPanel for Row
                                TableLayoutPanel rowTable = new TableLayoutPanel
                                {
                                    ColumnCount = 4,
                                    Dock = DockStyle.Fill,
                                    Height = 60,
                                    BackColor = ColorTranslator.FromHtml("#ffe261")
                                };

                                // Set column sizes
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40));
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200));
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170));
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 10));

                                // Determine the icon based on file type
                                Image icon;
                                string categoryExtension = Path.GetExtension(categoryName).ToLower();
                                if (categoryExtension == ".zip")
                                {
                                    icon = Image.FromFile(ZippedFolderIcon);
                                }
                                else
                                {
                                    icon = Image.FromFile(FolderIcon);
                                }

                                // Create Labels
                                Label categoryLabel = new Label
                                {
                                    Text = categoryName,
                                    Dock = DockStyle.Fill,
                                    AutoSize = false,
                                    TextAlign = ContentAlignment.MiddleLeft,
                                    Padding = new Padding(5, 0, 0, 0),
                                    Font = new Font("Microsoft Sans Serif", 10),
                                    BackColor = Color.Transparent
                                };

                                Label dateLabel = new Label
                                {
                                    Text = createdAt,
                                    Dock = DockStyle.Fill,
                                    AutoSize = false,
                                    TextAlign = ContentAlignment.MiddleLeft,
                                    Padding = new Padding(10, 0, 0, 0),
                                    Margin = new Padding(13, 0, 0, 0),
                                    Font = new Font("Microsoft Sans Serif", 10),
                                    BackColor = Color.Transparent
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
                                    Margin = new Padding(0, 5, 50, 0),
                                    PressedDepth = 10
                                };

                                actionButton.Click += (s, e) =>
                                {
                                    ShowPanel("category", categoryName, actionButton);
                                    popupPanel.Hide();
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

                                categoryLabel.MouseEnter += RowHover;
                                categoryLabel.MouseLeave += RowLeave;

                                dateLabel.MouseEnter += RowHover;
                                dateLabel.MouseLeave += RowLeave;

                                // Add Labels and Button to rowTable
                                PictureBox fileIconPictureBox = new PictureBox
                                {
                                    Image = icon,
                                    SizeMode = PictureBoxSizeMode.Zoom,
                                    Margin = new Padding(13, 17, 5, 5),
                                    Width = 25,
                                    Height = 25
                                };

                                rowTable.Controls.Add(fileIconPictureBox, 0, 0);
                                rowTable.Controls.Add(categoryLabel, 1, 0);
                                rowTable.Controls.Add(dateLabel, 2, 0);
                                rowTable.Controls.Add(actionButton, 4, 0);

                                // Add rowTable to TableLayoutPanel
                                tableLayoutPanel1.RowCount = rowIndex + 1;
                                tableLayoutPanel1.Controls.Add(rowTable, 0, rowIndex);
                                tableLayoutPanel1.SetColumnSpan(rowTable, 3);

                                rowIndex++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void ShowPanel(string type, string name, Control btn)
        {
            selectedName = name; // Store the category name

            // Toggle visibility of guna2Panel2
            if (guna2Panel2.Visible)
            {
                guna2Panel2.Visible = false;
                return;
            }

            string openFolder = Path.Combine(Application.StartupPath, "Assets (images)", "folder-open.png");
            string rename = Path.Combine(Application.StartupPath, "Assets (images)", "pencil.png");
            string remove = Path.Combine(Application.StartupPath, "Assets (images)", "trash.png");
            string archive = Path.Combine(Application.StartupPath, "Assets (images)", "archive.png");

            guna2Panel2.Controls.Clear(); // Clear previous content
            guna2Panel2.Visible = true;   // Show the panel

            // Set panel properties
            guna2Panel2.Size = new Size(181, 218); // Adjust size
            guna2Panel2.BorderRadius = 5;
            guna2Panel2.BorderThickness = 1;
            guna2Panel2.BorderColor = Color.Black;
            guna2Panel2.BackColor = ColorTranslator.FromHtml("#ffe261");
            guna2Panel2.BringToFront();
            guna2Panel2.Font = new Font("Segoe UI", 9);
            guna2Panel2.ForeColor = Color.Black;

            // Create buttons
            Guna2Button btnOpen = new Guna2Button
            {
                Size = new Size(177, 42),
                Text = "View",
                TextAlign = HorizontalAlignment.Left,
                TextOffset = new Point(5, 0),
                BackColor = Color.FromArgb(255, 236, 130),
                FillColor = Color.FromArgb(255, 236, 130),
                Font = new Font("Microsoft Sans Serif", 10),
                ForeColor = Color.Black,
                Image = Image.FromFile(openFolder),
                ImageAlign = HorizontalAlignment.Left,
                ImageSize = new Size(20, 20),
                Location = new Point(2, 3),
                PressedColor = Color.Black,
                PressedDepth = 10,
                BorderRadius = 5
            };
            btnOpen.Click += (s, e) =>
            {
                ShowLoadingAnimation(); // Show loading animation
                Task.Run(() =>
                {
                    try
                    {
                        // Simulate some loading process if needed
                        System.Threading.Thread.Sleep(1000); // Simulate delay

                        // Navigate to the category and load the form
                        Invoke(new Action(() =>
                        {
                            int categoryId = GetCategoryIdByName(selectedName); // Get the category ID by name
                            if (categoryId != -1) // Ensure the category exists
                            {
                                NavigateToCategory(categoryId); // Navigate to the selected category
                                LoadFormInPanel(new fetchDocuments(categoryId, selectedName, "")); // Load the fetchDocuments form
                            }
                            else
                            {
                                MessageBox.Show("Unable to open the folder. Category not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            HideLoadingAnimation(); // Hide loading animation after loading the form
                        }));
                    }
                    catch (Exception ex)
                    {
                        Invoke(new Action(() =>
                        {
                            MessageBox.Show($"An error occurred while opening the folder: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            HideLoadingAnimation(); // Ensure the loading animation is hidden in case of an error
                        }));
                    }
                });
            };


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
                Image = Image.FromFile(download),
                ImageAlign = HorizontalAlignment.Left,
                ImageSize = new Size(15, 15),
                Location = new Point(2, 45),
                PressedColor = Color.Black,
                PressedDepth = 10,
                BorderRadius = 5
            };
            btnDownload.Click += (s, e) =>
            {
                try
                {
                    // Step 1: Retrieve the folder path for the selected category
                    int categoryId = GetCategoryIdByName(selectedName);
                    if (categoryId == -1)
                    {
                        MessageBox.Show("Category not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string folderPath = GetFolderPathByCategoryId(categoryId);
                    if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
                    {
                        MessageBox.Show("Category folder not found in the file system.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Step 2: Prompt the user to select a save location
                    using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                    {
                        saveFileDialog.FileName = $"{selectedName}.zip";
                        saveFileDialog.Filter = "ZIP files|*.zip";

                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            string zipPath = saveFileDialog.FileName;

                            // Step 3: Create the ZIP file
                            if (File.Exists(zipPath))
                                File.Delete(zipPath); // Ensure no conflict

                            ZipFile.CreateFromDirectory(folderPath, zipPath);

                            MessageBox.Show("Category downloaded successfully as ZIP!", "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while downloading the category: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                guna2Panel2.Hide(); // Hide the options panel after the action
            };


            Guna2Button btnRename = new Guna2Button
            {
                Size = new Size(177, 42),
                Text = "Rename",
                TextAlign = HorizontalAlignment.Left,
                TextOffset = new Point(10, 0),
                BackColor = Color.FromArgb(255, 236, 130),
                FillColor = Color.FromArgb(255, 236, 130),
                Font = new Font("Microsoft Sans Serif", 10),
                ForeColor = Color.Black,
                Image = Image.FromFile(rename),
                ImageAlign = HorizontalAlignment.Left,
                ImageSize = new Size(15, 15),
                Location = new Point(2, 87),
                PressedColor = Color.Black,
                PressedDepth = 10,
                BorderRadius = 5
            };

            // Attach the event that handles renaming
            btnRename.Click += (s, e) => RenameCategory(selectedName);

            Guna2Button btnDelete = new Guna2Button
            {
                Size = new Size(177, 44),
                Text = "Move to trash",
                TextAlign = HorizontalAlignment.Left,
                TextOffset = new Point(10, 0),
                BackColor = Color.FromArgb(255, 236, 130),
                FillColor = Color.FromArgb(255, 236, 130),
                Font = new Font("Microsoft Sans Serif", 10),
                ForeColor = Color.Black,
                Image = Image.FromFile(remove),
                ImageAlign = HorizontalAlignment.Left,
                ImageSize = new Size(15, 15),
                Location = new Point(2, 129),
                PressedColor = Color.Black,
                PressedDepth = 10,
                BorderRadius = 5
            };

            // Attach the event that handles deleting
            btnDelete.Click += (s, e) => RemoveCategory(GetCategoryIdByName(selectedName));

            Guna2Button btnArchive = new Guna2Button
            {
                Size = new Size(177, 44),
                Text = "Archive",
                TextAlign = HorizontalAlignment.Left,
                TextOffset = new Point(10, 0),
                BackColor = Color.FromArgb(255, 236, 130),
                FillColor = Color.FromArgb(255, 236, 130),
                Font = new Font("Microsoft Sans Serif", 10),
                ForeColor = Color.Black,
                Image = Image.FromFile(archive),
                ImageAlign = HorizontalAlignment.Left,
                ImageSize = new Size(15, 15),
                Location = new Point(2, 172),
                PressedColor = Color.Black,
                PressedDepth = 10,
                BorderRadius = 5
            };

            // Attach the event that handles archiving
            btnArchive.Click += (s, e) => ArchiveCategory(GetCategoryIdByName(selectedName));

            // Add buttons to panel
            guna2Panel2.Controls.Add(btnOpen);
            guna2Panel2.Controls.Add(btnDownload);
            guna2Panel2.Controls.Add(btnRename);
            guna2Panel2.Controls.Add(btnDelete);
            guna2Panel2.Controls.Add(btnArchive);

            // Adjust Panel Position to Keep it Inside the Form
            Point btnScreenLocation = btn.Parent.PointToScreen(btn.Location);
            Point panelLocation = this.PointToClient(new Point(btnScreenLocation.X, btnScreenLocation.Y + btn.Height + -100));

            int panelX = panelLocation.X;
            int panelY = panelLocation.Y;
            int panelWidth = guna2Panel2.Width;
            int panelHeight = guna2Panel2.Height;

            // Ensure panel doesn't go beyond the right boundary
            if (panelX + panelWidth > this.ClientSize.Width)
            {
                panelX = this.ClientSize.Width - panelWidth - 100;
            }

            // Ensure panel doesn't go beyond the bottom boundary
            if (panelY + panelHeight > this.ClientSize.Height)
            {
                panelY = this.ClientSize.Height - panelHeight - 190;
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

        private bool IsCategoryNameExists(string categoryName)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                // Only check against ACTIVE categories for the current user
                string query = "SELECT COUNT(*) FROM category WHERE categoryName = @categoryName AND is_archived = 0 AND userId = @userId";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@categoryName", categoryName);
                    cmd.Parameters.AddWithValue("@userId", Session.CurrentUserId); // Ensure check is per user
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }


        private string GetFolderPathByCategoryId(int categoryId)
        {
            string folderPath = string.Empty;

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT folderPath FROM category WHERE categoryId = @categoryId";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@categoryId", categoryId);
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            folderPath = result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving folder path: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return folderPath;
        }


        private void RenameCategory(string categoryName)
        {
            try
            {
                // Step 1: Retrieve the category ID and current folder path
                int categoryId = GetCategoryIdByName(categoryName);
                if (categoryId == -1)
                {
                    MessageBox.Show("Category not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string currentFolderPath = GetFolderPathByCategoryId(categoryId);
                if (string.IsNullOrEmpty(currentFolderPath) || !Directory.Exists(currentFolderPath))
                {
                    MessageBox.Show("Category folder not found in the file system.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Step 2: Prompt user for a new category name
                string newCategoryName = Microsoft.VisualBasic.Interaction.InputBox(
                    "Enter new category name:",
                    "Rename Category",
                    categoryName
                ).Trim();

                if (string.IsNullOrWhiteSpace(newCategoryName))
                {
                    MessageBox.Show("Category name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Step 3: Prevent duplicate category names
                if (IsCategoryNameExists(newCategoryName))
                {
                    MessageBox.Show("A category with this name already exists. Please choose a different name.",
                                    "Duplicate Category", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Step 4: Construct the new folder path
                string parentFolderPath = Path.GetDirectoryName(currentFolderPath);
                string newFolderPath = Path.Combine(parentFolderPath, newCategoryName);

                // Step 5: Rename the folder in the file system
                Directory.Move(currentFolderPath, newFolderPath);

                // Step 6: Update the category name and folder path in the database
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
                            MessageBox.Show($"Category renamed to '{newCategoryName}' successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadCategories(); // Refresh the categories list
                        }
                        else
                        {
                            MessageBox.Show("Category not found in the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error renaming category: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        // Event handler for Edit button
        private void ArchiveCategory(int categoryId)
        {
            try
            {
                string connectionString = "server=localhost; user=root; Database=docsmanagement; password=";
                string categoryName;

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Step 1: Retrieve the category name
                    string selectQuery = "SELECT categoryName FROM category WHERE categoryId = @categoryId";
                    using (MySqlCommand selectCmd = new MySqlCommand(selectQuery, conn))
                    {
                        selectCmd.Parameters.AddWithValue("@categoryId", categoryId);
                        categoryName = selectCmd.ExecuteScalar()?.ToString();
                    }

                    if (string.IsNullOrEmpty(categoryName))
                    {
                        MessageBox.Show("Folder not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Step 2: Confirm the archiving action
                    var confirmationResult = MessageBox.Show(
                        $"Do you want to proceed with archiving the folder '{categoryName}' and all its files inside?",
                        "Confirmation",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (confirmationResult != DialogResult.Yes)
                        return;

                    // Step 3: Fetch all subcategories recursively
                    string fetchSubcategoriesQuery = @"
                WITH RECURSIVE Subcategories AS (
                    SELECT categoryId
                    FROM category
                    WHERE categoryId = @rootCategoryId
                    UNION ALL
                    SELECT c.categoryId
                    FROM category c
                    INNER JOIN Subcategories sc ON c.parentCategoryId = sc.categoryId
                )
                SELECT categoryId FROM Subcategories";

                    List<int> categoryIds = new List<int>();
                    using (MySqlCommand fetchSubcategoriesCmd = new MySqlCommand(fetchSubcategoriesQuery, conn))
                    {
                        fetchSubcategoriesCmd.Parameters.AddWithValue("@rootCategoryId", categoryId);
                        using (MySqlDataReader reader = fetchSubcategoriesCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                categoryIds.Add(reader.GetInt32(0));
                            }
                        }
                    }

                    if (categoryIds.Count == 0)
                    {
                        MessageBox.Show("No subcategories found to archive.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Step 4: Update the `is_hidden` column for all categories
                    string updateCategoriesQuery = "UPDATE category SET is_hidden = 1 WHERE categoryId IN (" + string.Join(",", categoryIds) + ")";
                    using (MySqlCommand updateCategoriesCmd = new MySqlCommand(updateCategoriesQuery, conn))
                    {
                        updateCategoriesCmd.ExecuteNonQuery();
                    }

                    // Step 5: Update the `is_hidden` column for all files in these categories
                    string updateFilesQuery = @"
                UPDATE files
                SET is_hidden = 1
                WHERE categoryId IN (" + string.Join(",", categoryIds) + ")";
                    using (MySqlCommand updateFilesCmd = new MySqlCommand(updateFilesQuery, conn))
                    {
                        updateFilesCmd.ExecuteNonQuery();
                    }

                    // Step 6: Notify the user and refresh the UI
                    MessageBox.Show($"Category '{categoryName}' and all its subfolders and files have been archived successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCategories();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error archiving category: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        // Event handler for Archive button
        // Event handler for Remove button
        private void RemoveCategory(int categoryId)
        {
            try
            {
                string connectionString = "server=localhost; user=root; Database=docsmanagement; password=";
                string categoryName = null;

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Step 1: Retrieve the category name and folder path
                    string selectQuery = "SELECT categoryName, folderPath FROM category WHERE categoryId = @categoryId";
                    string folderPath = null;

                    using (MySqlCommand selectCmd = new MySqlCommand(selectQuery, conn))
                    {
                        selectCmd.Parameters.AddWithValue("@categoryId", categoryId);
                        using (MySqlDataReader reader = selectCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                categoryName = reader["categoryName"].ToString();
                                folderPath = reader["folderPath"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("Category not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }

                    // Step 2: Confirm the removal action
                    var confirmationResult = MessageBox.Show(
                        $"Are you sure you want to remove the category '{categoryName}' and all its subcategories and files? This action cannot be undone.",
                        "Confirm Removal",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (confirmationResult != DialogResult.Yes)
                        return;

                    // Step 3: Perform soft delete recursively
                    string archiveSuffix = $"_archived_{DateTime.Now:yyyyMMddHHmmss}";
                    string archivedFolderPath = folderPath + archiveSuffix;

                    if (Directory.Exists(folderPath))
                    {
                        Directory.Move(folderPath, archivedFolderPath); // Rename the folder
                    }

                    // Step 4: Update the database for the category and its contents
                    ArchiveCategoryAndContents(conn, categoryId, archivedFolderPath, archiveSuffix);

                    // Step 5: Notify the user and refresh the UI
                    MessageBox.Show($"Category '{categoryName}' and its contents have been archived successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCategories(); // Refresh the category list
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing category: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ArchiveCategoryAndContents(MySqlConnection conn, int categoryId, string archivedFolderPath, string archiveSuffix)
        {
            try
            {
                // Step 1: Archive all subcategories recursively
                string subcategoriesQuery = "SELECT categoryId, folderPath, categoryName FROM category WHERE parentCategoryId = @ParentCategoryId";
                List<(int SubcategoryId, string SubcategoryPath, string SubcategoryName)> subcategories = new List<(int, string, string)>();

                using (MySqlCommand subcategoriesCmd = new MySqlCommand(subcategoriesQuery, conn))
                {
                    subcategoriesCmd.Parameters.AddWithValue("@ParentCategoryId", categoryId);
                    using (MySqlDataReader reader = subcategoriesCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int subcategoryId = reader.GetInt32(0);
                            string subcategoryPath = reader.GetString(1);
                            string subcategoryName = reader.GetString(2);
                            subcategories.Add((subcategoryId, subcategoryPath, subcategoryName));
                        }
                    }
                }

                foreach (var (subcategoryId, subcategoryPath, subcategoryName) in subcategories)
                {
                    // Adjust the subcategory path to reflect the new parent folder path
                    string archivedSubcategoryPath = Path.Combine(archivedFolderPath, subcategoryName + archiveSuffix);
                    string archivedSubcategoryName = subcategoryName + archiveSuffix;

                    if (Directory.Exists(subcategoryPath))
                    {
                        Directory.Move(subcategoryPath, archivedSubcategoryPath); // Rename the subfolder
                    }

                    // Recursively archive subcategories
                    ArchiveCategoryAndContents(conn, subcategoryId, archivedSubcategoryPath, archiveSuffix);

                    // Update the subcategory name and path in the database
                    string updateSubcategoryQuery = "UPDATE category SET folderPath = @ArchivedFolderPath, categoryName = @ArchivedCategoryName, is_archived = 1 WHERE categoryId = @CategoryId";
                    using (MySqlCommand updateSubcategoryCmd = new MySqlCommand(updateSubcategoryQuery, conn))
                    {
                        updateSubcategoryCmd.Parameters.AddWithValue("@ArchivedFolderPath", archivedSubcategoryPath);
                        updateSubcategoryCmd.Parameters.AddWithValue("@ArchivedCategoryName", archivedSubcategoryName);
                        updateSubcategoryCmd.Parameters.AddWithValue("@CategoryId", subcategoryId);
                        updateSubcategoryCmd.ExecuteNonQuery();
                    }
                }

                // Step 2: Archive all files within this category
                string filesQuery = "SELECT fileId, filePath, fileName FROM files WHERE categoryId = @CategoryId";
                List<(int FileId, string FilePath, string FileName)> files = new List<(int, string, string)>();

                using (MySqlCommand filesCmd = new MySqlCommand(filesQuery, conn))
                {
                    filesCmd.Parameters.AddWithValue("@CategoryId", categoryId);
                    using (MySqlDataReader reader = filesCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int fileId = reader.GetInt32(0);
                            string filePath = reader.GetString(1);
                            string fileName = reader.GetString(2);
                            files.Add((fileId, filePath, fileName));
                        }
                    }
                }

                foreach (var (fileId, filePath, fileName) in files)
                {
                    string archivedFilePath = Path.Combine(archivedFolderPath, Path.GetFileNameWithoutExtension(fileName) + archiveSuffix + Path.GetExtension(fileName));
                    string archivedFileName = Path.GetFileNameWithoutExtension(fileName) + archiveSuffix + Path.GetExtension(fileName);

                    if (File.Exists(filePath))
                    {
                        File.Move(filePath, archivedFilePath); // Rename the file
                    }

                    // Update the file path and file name in the database, and mark as archived
                    string updateFileQuery = "UPDATE files SET filePath = @ArchivedFilePath, fileName = @ArchivedFileName, isArchived = 1 WHERE fileId = @FileId";
                    using (MySqlCommand updateFileCmd = new MySqlCommand(updateFileQuery, conn))
                    {
                        updateFileCmd.Parameters.AddWithValue("@ArchivedFilePath", archivedFilePath);
                        updateFileCmd.Parameters.AddWithValue("@ArchivedFileName", archivedFileName);
                        updateFileCmd.Parameters.AddWithValue("@FileId", fileId);
                        updateFileCmd.ExecuteNonQuery();
                    }
                }

                // Step 3: Update the category name, path, and mark as archived in the database
                string updateCategoryQuery = "UPDATE category SET folderPath = @ArchivedFolderPath, categoryName = @ArchivedCategoryName, is_archived = 1 WHERE categoryId = @CategoryId";
                using (MySqlCommand updateCategoryCmd = new MySqlCommand(updateCategoryQuery, conn))
                {
                    string archivedCategoryName = Path.GetFileName(archivedFolderPath); // Extract the new folder name
                    updateCategoryCmd.Parameters.AddWithValue("@ArchivedFolderPath", archivedFolderPath);
                    updateCategoryCmd.Parameters.AddWithValue("@ArchivedCategoryName", archivedCategoryName);
                    updateCategoryCmd.Parameters.AddWithValue("@CategoryId", categoryId);
                    updateCategoryCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error archiving category contents: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private async void categories_Load(object sender, EventArgs e)
        {
            int folderCount = GetFolderCount();
            if (folderCount > 10)
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        ShowLoadingAnimation();
                    }));
                }
                else
                {
                    ShowLoadingAnimation();
                }
                await Task.Delay(3000);
            }
            if (!backgroundWorker.IsBusy)
            {
                backgroundWorker.RunWorkerAsync();
            }
        }


        private int GetFolderCount()
        {
            string connectionString = "server=localhost; user=root; Database=docsmanagement; password=";
            int count = 0;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM category WHERE userId = @userId AND is_archived = 0 AND is_hidden = 0";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", Session.CurrentUserId);
                        count = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            MessageBox.Show("Error retrieving folder count: " + ex.Message);
                        }));
                    }
                    else
                    {
                        MessageBox.Show("Error retrieving folder count: " + ex.Message);
                    }
                }
            }

            return count;
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Newbtn_Click(object sender, EventArgs e)
        {
            popupPanel.Visible = !popupPanel.Visible;
        }

        private void loadFromA_Z_Click(object sender, EventArgs e)
        {
            // Toggle the state
            isSortByNameAscending = !isSortByNameAscending;

            // Optionally, update the button's appearance to reflect the toggle state
            loadFromA_Z.FillColor = isSortByNameAscending ? Color.FromArgb(219, 195, 0) : Color.FromArgb(255, 236, 130);

            // Reload the documents with the updated sorting
            LoadCategories();
        }

        private void btnFileUpload_Click(object sender, EventArgs e)
        {
            createNewFolderForm NewFolder = new createNewFolderForm(); // Pass 'this'
            NewFolder.StartPosition = FormStartPosition.CenterScreen;
            NewFolder.TopMost = true;
            NewFolder.FormBorderStyle = FormBorderStyle.FixedDialog;
            NewFolder.MinimizeBox = false;
            NewFolder.MaximizeBox = false;
            NewFolder.ShowDialog(this);
            LoadCategories();
        }
    }
}
