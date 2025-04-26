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

namespace tarungonNaNako.sidebar
{
    public partial class categories : Form
    {
        private BackgroundWorker backgroundWorker;
        private System.Timers.Timer debounceTimer;
        private bool isSortByNameAscending = false;

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
            LoadCategories(currentCategoryId, searchTerm);
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
            LoadCategories(currentCategoryId); // Reload with new category
        }

        private void LoadCategories(string searchTerm = "")
        {
            LoadCategories(currentCategoryId);
        }

        private void LoadCategories(int? currentCategoryId, string searchTerm = "")
        {
            string connectionString = "server=localhost; user=root; Database=docsmanagement; password=";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Ensure the user is logged in
                    if (Session.CurrentUserId == 0)
                    {
                        MessageBox.Show("User is not logged in.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string query = "SELECT categoryId, categoryName, created_at FROM category WHERE userId = @userId AND is_archived = 0 AND is_hidden = 0";
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        query += " AND categoryName LIKE @searchTerm";
                    }
                    query += isSortByNameAscending ? " ORDER BY categoryName ASC" : " ORDER BY categoryName DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", Session.CurrentUserId);
                        if (!string.IsNullOrEmpty(searchTerm))
                        {
                            cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                        }

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Clear existing controls and reset row styles
                            if (InvokeRequired)
                            {
                                Invoke(new Action(() =>
                                {
                                    tableLayoutPanel1.Controls.Clear();
                                    tableLayoutPanel1.RowCount = 0;
                                    tableLayoutPanel1.RowStyles.Clear();
                                }));
                            }
                            else
                            {
                                tableLayoutPanel1.Controls.Clear();
                                tableLayoutPanel1.RowCount = 0;
                                tableLayoutPanel1.RowStyles.Clear();
                            }

                            // Fixed height for rows
                            int fixedRowHeight = 60; // Adjust this value as needed
                            int rowIndex = 0;

                            while (reader.Read())
                            {
                                int categoryId = reader.GetInt32("categoryId");
                                string categoryName = reader.GetString("categoryName");
                                DateTime dateCreated = reader.GetDateTime("created_at");

                                // Add a new row and set its height
                                if (InvokeRequired)
                                {
                                    Invoke(new Action(() =>
                                    {
                                        tableLayoutPanel1.RowCount = rowIndex + 1;
                                    }));
                                }
                                else
                                {
                                    tableLayoutPanel1.RowCount = rowIndex + 1;
                                }

                                // Create Label for category name
                                Label categoryLabel = new Label
                                {
                                    Text = categoryName,
                                    Dock = DockStyle.Left,
                                    AutoSize = false,
                                    Font = new Font("Microsoft Sans Serif", 10),
                                    Width = 200, // Set a width for the label
                                    TextAlign = ContentAlignment.MiddleLeft,
                                    Padding = new Padding(10, 0, 0, 0)
                                };

                                // Create Label for date created  
                                Label dateCreatedLabel = new Label
                                {
                                    Text = dateCreated.ToString("yyyy-MM-dd hh:mm tt"),
                                    AutoSize = false,
                                    Font = new Font("Microsoft Sans Serif", 10),
                                    Width = 200, // Set a width for the label  
                                    TextAlign = ContentAlignment.MiddleLeft, // Align text to the right 
                                    Dock = DockStyle.Right, // Dock it to the right of the rowPanel  
                                    Padding = new Padding(0, 0, 10, 0) // Add padding to the right for spacing  
                                };
                                // Create Archive button
                                Guna.UI2.WinForms.Guna2Button archiveButton = new Guna.UI2.WinForms.Guna2Button
                                {
                                    Text = "Archive",
                                    BorderRadius = 10,
                                    PressedDepth = 10,
                                    FillColor = Color.FromArgb(255, 226, 97),
                                    ForeColor = Color.Black,
                                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                                    Dock = DockStyle.Right,
                                    Width = 90, // Set a fixed width
                                    Height = 50
                                };

                                archiveButton.Click += (sender, e) => ArchiveCategory(categoryId);

                                // Create Remove button
                                Guna.UI2.WinForms.Guna2Button removeButton = new Guna.UI2.WinForms.Guna2Button
                                {
                                    Text = "Remove",
                                    BorderRadius = 10,
                                    PressedDepth = 10,
                                    FillColor = Color.FromArgb(255, 226, 97),
                                    ForeColor = Color.Black,
                                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                                    Dock = DockStyle.Right,
                                    Width = 95, // Set a fixed width
                                    Height = 50,
                                };
                                removeButton.Click += (sender, e) => RemoveCategory(categoryId);

                                // Create a Panel for the row to handle layout and events
                                Panel rowPanel = new Panel
                                {
                                    Dock = DockStyle.Fill,
                                    BackColor = ColorTranslator.FromHtml("#ffe261"),
                                    Height = fixedRowHeight,
                                    Tag = categoryId, // Store categoryId for later use
                                    Padding = new Padding(0, 0, 20, 0)
                                };
                                rowPanel.DoubleClick += (s, e) =>
                                {
                                    ShowLoadingAnimation(); // Show loading animation
                                    Task.Run(() =>
                                    {
                                        // Simulate some loading process if needed
                                        System.Threading.Thread.Sleep(1000); // Simulate delay

                                        // Navigate to the category and load the form
                                        Invoke(new Action(() =>
                                        {
                                            NavigateToCategory(categoryId);
                                            LoadFormInPanel(new fetchDocuments(categoryId, categoryName, ""));
                                            HideLoadingAnimation(); // Hide loading animation after loading the form
                                        }));
                                    });
                                };
                                // Add hover effects
                                rowPanel.MouseEnter += (s, e) =>
                                {
                                    rowPanel.BackColor = Color.FromArgb(219, 195, 0);
                                    archiveButton.FillColor = Color.FromArgb(219, 195, 0);
                                    removeButton.FillColor = Color.FromArgb(219, 195, 0);
                                };
                                rowPanel.MouseLeave += (s, e) =>
                                {
                                    rowPanel.BackColor = ColorTranslator.FromHtml("#ffe261");
                                    archiveButton.FillColor = ColorTranslator.FromHtml("#ffe261");
                                    removeButton.FillColor = ColorTranslator.FromHtml("#ffe261");
                                };

                                // Add controls to the rowPanel
                                rowPanel.Controls.Add(categoryLabel);
                                rowPanel.Controls.Add(dateCreatedLabel); // Add the date created label
                                rowPanel.Controls.Add(archiveButton);
                                rowPanel.Controls.Add(removeButton);

                                // Add the rowPanel to the TableLayoutPanel
                                if (InvokeRequired)
                                {
                                    Invoke(new Action(() =>
                                    {
                                        this.tableLayoutPanel1.Controls.Add(rowPanel, 0, rowIndex);
                                        this.tableLayoutPanel1.SetColumnSpan(rowPanel, 3); // Span all columns
                                    }));
                                }
                                else
                                {
                                    this.tableLayoutPanel1.Controls.Add(rowPanel, 0, rowIndex);
                                    this.tableLayoutPanel1.SetColumnSpan(rowPanel, 3); // Span all columns
                                }

                                rowIndex++;
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
                            MessageBox.Show("Error loading categories: " + ex.Message);
                        }));
                    }
                    else
                    {
                        MessageBox.Show("Error loading categories: " + ex.Message);
                    }
                }
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

                    // Retrieve the category name
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

                    // Display a confirmation dialog
                    var result = MessageBox.Show(
                        $"Are you sure you want to remove the folder '{categoryName}' and all its files inside? This action cannot be undone.",
                        "Confirm Removal",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        // Step 1: Get the categoryId and folderPath of the root category
                        string getCategoryQuery = "SELECT categoryId, folderPath FROM category WHERE categoryId = @categoryId";
                        int? rootCategoryId = null;
                        string rootFolderPath = null;

                        using (MySqlCommand getCategoryCmd = new MySqlCommand(getCategoryQuery, conn))
                        {
                            getCategoryCmd.Parameters.AddWithValue("@categoryId", categoryId);
                            using (MySqlDataReader reader = getCategoryCmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    rootCategoryId = reader.GetInt32(0);
                                    rootFolderPath = reader.GetString(1);
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
                    WHERE categoryId = @rootCategoryId
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
                            archiveSubcategoriesCmd.Parameters.AddWithValue("@rootCategoryId", rootCategoryId);

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

                        // Step 4: Archive all files in the root category and its subcategories
                        string archiveFilesQuery = @"
                UPDATE files
                SET isArchived = 1
                WHERE categoryId IN (" + string.Join(",", categoryIds) + ")";

                        using (MySqlCommand archiveFilesCmd = new MySqlCommand(archiveFilesQuery, conn))
                        {
                            archiveFilesCmd.ExecuteNonQuery();
                        }

                        // Step 5: Delete the root folder and its subfolders from the file system
                        foreach (string folderPath in folderPaths)
                        {
                            if (Directory.Exists(folderPath))
                            {
                                try
                                {
                                    Directory.Delete(folderPath, true); // Recursively delete the folder
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Error deleting folder '{folderPath}': {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }

                        // Step 6: Notify the user and refresh the UI
                        MessageBox.Show($"Folder '{categoryName}' and all its files inside have been removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCategories();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                MessageBox.Show($"Error removing category: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
