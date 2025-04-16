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

namespace tarungonNaNako.sidebar
{
    public partial class categories : Form
    {
        private BackgroundWorker backgroundWorker;
        private System.Timers.Timer debounceTimer;

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

                    string query = "SELECT categoryId, categoryName FROM category WHERE userId = @userId AND is_archived = 0";
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        query += " AND categoryName LIKE @searchTerm";
                    }

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

                                // Create Edit button
                                Guna.UI2.WinForms.Guna2Button editButton = new Guna.UI2.WinForms.Guna2Button
                                {
                                    Text = "Edit",
                                    BorderRadius = 10,
                                    PressedDepth = 10,
                                    FillColor = Color.FromArgb(255, 226, 97),
                                    ForeColor = Color.Black,
                                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                                    Dock = DockStyle.Right,
                                    Width = 90, // Set a fixed width
                                    Height = 50
                                };

                                editButton.Click += (sender, e) => EditCategory(categoryId);

                                // Create Archive button
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
                                    editButton.FillColor = Color.FromArgb(219, 195, 0);
                                    removeButton.FillColor = Color.FromArgb(219, 195, 0);
                                };
                                rowPanel.MouseLeave += (s, e) =>
                                {
                                    rowPanel.BackColor = ColorTranslator.FromHtml("#ffe261");
                                    editButton.FillColor = ColorTranslator.FromHtml("#ffe261");
                                    removeButton.FillColor = ColorTranslator.FromHtml("#ffe261");
                                };

                                // Add controls to the rowPanel
                                rowPanel.Controls.Add(categoryLabel);
                                rowPanel.Controls.Add(editButton);
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
        private void EditCategory(int categoryId)
        {
            adminDashboard parentForm = this.ParentForm as adminDashboard;

            if (parentForm != null)
            {
                // Open addCategory form with the categoryId
                parentForm.LoadFormInPanel(new addCategory(categoryId));
            }
            else
            {
                MessageBox.Show("Parent form not found. Please ensure the form is opened from adminDashboard.");
            }
        }

        // Event handler for Archive button
        // Event handler for Remove button
        private void RemoveCategory(int categoryId)
        {
            // Display a confirmation dialog
            var result = MessageBox.Show(
                "Are you sure you want to remove this category? This action cannot be undone.",
                "Confirm Removal",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            // Check the user's choice
            if (result == DialogResult.Yes)
            {
                string connectionString = "server=localhost; user=root; Database=docsmanagement; password=";

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        // Begin a transaction to ensure both updates succeed
                        using (MySqlTransaction transaction = conn.BeginTransaction())
                        {
                            try
                            {
                                // Update associated permissions to set is_archived to 1
                                string updatePermissionsQuery = "UPDATE permissions SET is_archived = 1 WHERE referenceId = @categoryId AND type = 'category'";
                                using (MySqlCommand cmd = new MySqlCommand(updatePermissionsQuery, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@categoryId", categoryId);
                                    cmd.ExecuteNonQuery();
                                }

                                // Update the category to set is_archived to 1
                                string updateCategoryQuery = "UPDATE category SET is_archived = 1 WHERE categoryId = @categoryId";
                                using (MySqlCommand cmd = new MySqlCommand(updateCategoryQuery, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@categoryId", categoryId);
                                    int rowsAffected = cmd.ExecuteNonQuery();

                                    if (rowsAffected > 0)
                                    {
                                        // Commit the transaction
                                        transaction.Commit();

                                        if (InvokeRequired)
                                        {
                                            Invoke(new Action(() =>
                                            {
                                                MessageBox.Show("Category and its permissions archived successfully.",
                                                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                LoadCategories(currentCategoryId);
                                            }));
                                        }
                                        else
                                        {
                                            MessageBox.Show("Category and its permissions archived successfully.",
                                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            LoadCategories(currentCategoryId);
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("Category not found or could not be archived.");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                // Roll back the transaction if any operation fails
                                transaction.Rollback();
                                throw new Exception("Error archiving category and permissions: " + ex.Message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (InvokeRequired)
                        {
                            Invoke(new Action(() =>
                            {
                                MessageBox.Show("Error archiving category: " + ex.Message,
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }));
                        }
                        else
                        {
                            MessageBox.Show("Error archiving category: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
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
                    string query = "SELECT COUNT(*) FROM category WHERE userId = @userId AND is_archived = 0";
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
    }
}
