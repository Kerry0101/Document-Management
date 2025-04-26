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
using System.Timers;
using Guna.UI2.WinForms;

namespace tarungonNaNako.sidebar
{
    public partial class manageDocs : Form
    {
        private bool isSortByNameAscending = false;
        private readonly string connectionString = "server=localhost;user=root;database=docsmanagement;password="; // DB connection
        string ThreeDotMenu = Path.Combine(Application.StartupPath, "Assets (images)", "menu-dots-vertical.png");
        string DocumentIcon = Path.Combine(Application.StartupPath, "Assets (images)", "document.png");
        string ZippedFolderIcon = Path.Combine(Application.StartupPath, "Assets (images)", "zip-file-format.png");
        private BackgroundWorker backgroundWorker;
        private System.Timers.Timer debounceTimer;
        private string selectedName = ""; 

        public manageDocs()
        {
            InitializeComponent();
            InitializeBackgroundWorker();
            popupPanel.Visible = false;
            loadingPictureBox.Visible = false;
            searchBar.TextChanged += SearchBar_TextChanged;

            debounceTimer = new System.Timers.Timer();
            debounceTimer.Interval = 500; // Set the debounce interval (in milliseconds)
            debounceTimer.AutoReset = false;
            debounceTimer.Elapsed += DebounceTimer_Elapsed;
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

        private void SearchBar_TextChanged(object sender, EventArgs e)
        {
            // Reset the debounce timer
            debounceTimer.Stop();
            debounceTimer.Start();
        }

        private void DebounceTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    if (!backgroundWorker.IsBusy)
                    {
                        ShowLoadingAnimation();
                        backgroundWorker.RunWorkerAsync(searchBar.Text); // Pass the search term
                    }
                }));
            }
            else
            {
                if (!backgroundWorker.IsBusy)
                {
                    ShowLoadingAnimation();
                    backgroundWorker.RunWorkerAsync(searchBar.Text); // Pass the search term
                }
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

        private void InitializeBackgroundWorker()
        {
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string searchTerm = e.Argument as string;
            LoadDocumentsIntoTablePanelInternal(searchTerm);
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

        public void LoadDocumentsIntoTablePanel(string searchTerm = "")
        {
            LoadDocumentsIntoTablePanelInternal();
        }

        public void LoadDocumentsIntoTablePanelInternal(string searchTerm = "")
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => LoadDocumentsIntoTablePanelInternal(searchTerm)));
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
                        if (InvokeRequired)
                        {
                            Invoke(new Action(() =>
                            {
                                MessageBox.Show("User is not logged in.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }));
                        }
                        else
                        {
                            MessageBox.Show("User is not logged in.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        return;
                    }

                    // Update the query to include sorting logic
                    string query = @"
                    SELECT f.fileName, f.uploadDate, c.categoryName
                    FROM files f
                    JOIN category c ON f.categoryId = c.categoryId
                    WHERE f.isArchived = 0
                    AND f.is_hidden = 0
                    AND c.is_hidden = 0
                    AND f.userId = @userId
                    AND f.fileName LIKE @searchTerm
                    ORDER BY " + (isSortByNameAscending ? "f.fileName ASC" : "f.uploadDate DESC");

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", Session.CurrentUserId);
                        cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Clear existing rows
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

                            int rowIndex = 0;

                            while (reader.Read())
                            {
                                string fileName = reader["fileName"].ToString();
                                string modificationTime = Convert.ToDateTime(reader["uploadDate"]).ToString("yyyy-MM-dd hh:mm tt");
                                string categoryName = reader["categoryName"].ToString();

                                // Create TableLayoutPanel for Row
                                TableLayoutPanel rowTable = new TableLayoutPanel
                                {
                                    ColumnCount = 5,
                                    Dock = DockStyle.Fill,
                                    Height = 60,
                                    BackColor = ColorTranslator.FromHtml("#ffe261")
                                };

                                // Set column sizes
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40));
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200));
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180));
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180));
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 10));

                                // Determine the icon based on file type
                                Image icon;
                                string fileExtension = Path.GetExtension(fileName).ToLower();
                                if (fileExtension == ".zip")
                                {
                                    icon = Image.FromFile(ZippedFolderIcon);
                                }
                                else
                                {
                                    icon = Image.FromFile(DocumentIcon);
                                }

                                // Create Labels
                                Label fileLabel = new Label
                                {
                                    Text = fileName,
                                    Dock = DockStyle.Fill,
                                    AutoSize = false,
                                    TextAlign = ContentAlignment.MiddleLeft,
                                    Padding = new Padding(5, 0, 0, 0),
                                    Font = new Font("Microsoft Sans Serif", 10),
                                    BackColor = Color.Transparent
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
                                    Anchor = AnchorStyles.Right,
                                    Margin = new Padding(0, 5, 30, 0),
                                    PressedDepth = 10
                                };

                                actionButton.Click += (s, e) =>
                                {
                                    if (InvokeRequired)
                                    {
                                        Invoke(new Action(() =>
                                        {
                                            ShowPanel("file", fileName, actionButton);
                                            popupPanel.Hide();

                                        }));
                                    }
                                    else
                                    {
                                        ShowPanel("file", fileName, actionButton);
                                        popupPanel.Hide();
                                    }
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

                                fileLabel.MouseEnter += RowHover;
                                fileLabel.MouseLeave += RowLeave;

                                dateLabel.MouseEnter += RowHover;
                                dateLabel.MouseLeave += RowLeave;

                                categoryLabel.MouseEnter += RowHover;
                                categoryLabel.MouseLeave += RowLeave;

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
                                rowTable.Controls.Add(fileLabel, 1, 0);
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
                                        tableLayoutPanel1.SetColumnSpan(rowTable, 3);
                                    }));
                                }
                                else
                                {
                                    tableLayoutPanel1.RowCount = rowIndex + 1;
                                    tableLayoutPanel1.Controls.Add(rowTable, 0, rowIndex);
                                    tableLayoutPanel1.SetColumnSpan(rowTable, 3);
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
                        MessageBox.Show($"Error loading files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
                else
                {
                    MessageBox.Show($"Error loading files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ShowPanel(string type, string name, Control btn)
        {
            selectedName = name; // Store file name  

            // Toggle visibility of guna2Panel2  
            if (guna2Panel2.Visible)
            {
                guna2Panel2.Visible = false;
                return;
            }

            string download = Path.Combine(Application.StartupPath, "Assets (images)", "down-to-line.png");
            string rename = Path.Combine(Application.StartupPath, "Assets (images)", "pencil.png");
            string remove = Path.Combine(Application.StartupPath, "Assets (images)", "trash.png");
            string archive = Path.Combine(Application.StartupPath, "Assets (images)", "archive.png");

            guna2Panel2.Controls.Clear(); // Clear previous content  
            guna2Panel2.Visible = true;   // Show the panel  

            // Set panel properties  
            guna2Panel2.Size = new Size(181, 176); // Adjust size  
            guna2Panel2.BorderRadius = 5;
            guna2Panel2.BorderThickness = 1;
            guna2Panel2.BorderColor = Color.Black;
            guna2Panel2.BackColor = ColorTranslator.FromHtml("#ffe261");
            guna2Panel2.BringToFront();
            guna2Panel2.Font = new Font("Segoe UI", 9);
            guna2Panel2.ForeColor = Color.Black;

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

            // Attach the event that handles downloading  
            btnDownload.Click += DownloadButton_Click;

            Guna.UI2.WinForms.Guna2Button btnRename = new Guna.UI2.WinForms.Guna2Button();
            btnRename.Size = new Size(177, 42);
            btnRename.Text = "Rename";
            btnRename.TextAlign = HorizontalAlignment.Left;
            btnRename.TextOffset = new Point(1, 0);
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

            // Attach the event that handles renaming  
            btnRename.Click += (s, e) => RenameButton_Click();

            Guna.UI2.WinForms.Guna2Button btnDelete = new Guna.UI2.WinForms.Guna2Button();
            btnDelete.Size = new Size(177, 44);
            btnDelete.Text = "Move to trash";
            btnDelete.TextAlign = HorizontalAlignment.Right;
            btnDelete.TextOffset = new Point(-13, 0);
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

            Guna.UI2.WinForms.Guna2Button btnArchive = new Guna.UI2.WinForms.Guna2Button();
            btnArchive.Size = new Size(177, 44);
            btnArchive.Text = "Archive";
            btnArchive.TextAlign = HorizontalAlignment.Left;
            btnArchive.TextOffset = new Point(2, 0);
            btnArchive.BackColor = Color.FromArgb(255, 236, 130);
            btnArchive.FillColor = Color.FromArgb(255, 236, 130);
            btnArchive.Font = new Font("Microsoft Sans Serif", 10);
            btnArchive.ForeColor = Color.Black;
            btnArchive.Image = Image.FromFile(archive);
            btnArchive.ImageAlign = HorizontalAlignment.Left;
            btnArchive.ImageSize = new Size(15, 15);
            btnArchive.Location = new Point(2, 130);
            btnArchive.PressedColor = Color.Black;
            btnArchive.PressedDepth = 10;
            btnArchive.BorderRadius = 5;
            btnArchive.Click += (s, e) => ArchiveButton_Click();

            // Add buttons to panel  
            guna2Panel2.Controls.Add(btnDownload);
            guna2Panel2.Controls.Add(btnRename);
            guna2Panel2.Controls.Add(btnDelete);
            guna2Panel2.Controls.Add(btnArchive);

            // Adjust Panel Position to Keep it Inside the Form  
            Point btnScreenLocation = btn.Parent.PointToScreen(btn.Location);
            Point panelLocation = this.PointToClient(new Point(btnScreenLocation.X, btnScreenLocation.Y + btn.Height + -60));

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
                panelY = this.ClientSize.Height - panelHeight - 100;
            }

            // Apply final position  
            guna2Panel2.Location = new Point(panelX, panelY);
            guna2Panel2.Visible = true;
        }

        private void DownloadButton_Click(object sender, EventArgs e)
        {
                DownloadFile(selectedName);
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
                    guna2Panel2.Hide();
                }
            }
        }

        private void RenameButton_Click()
        {

            EditFile(GetCategoryIdByName(selectedName), selectedName);
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
                MessageBox.Show("File name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("A file with this name already exists. Please choose a different name.",
                                "Duplicate File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Step 1: Retrieve the current file path from the database
                string currentFilePath = GetFilePathByFileName(selectedName);
                if (string.IsNullOrEmpty(currentFilePath) || !File.Exists(currentFilePath))
                {
                    MessageBox.Show("File not found in the file system.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                            guna2Panel2.Visible = false;
                            MessageBox.Show($"File renamed to '{newFileName}' successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDocumentsIntoTablePanelInternal();
                        }
                        else
                        {
                            MessageBox.Show("File not found in the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error renaming file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ArchiveButton_Click()
        {
            ArchiveFile(selectedName);
        }
        private void ArchiveFile(string name)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE files SET is_hidden = 1 WHERE fileName = @fileName";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@fileName", name);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            guna2Panel2.Visible = false;
                            MessageBox.Show($"File '{name}' archived successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDocumentsIntoTablePanelInternal();
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
                MessageBox.Show($"Error archiving file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void RemoveButton_Click()
        {
                RemoveFile(selectedName);
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
                            LoadDocumentsIntoTablePanelInternal();
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private async void manageDocs_Load(object sender, EventArgs e)
        {
            int fileCount = GetfileCount();
            if (fileCount > 10)
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


        private int GetfileCount()
        {
            string connectionString = "server=localhost; user=root; Database=docsmanagement; password=";
            int count = 0;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM files WHERE userId = @userId AND isArchived = 0 AND is_hidden = 0";
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
                            MessageBox.Show("Error retrieving file count: " + ex.Message);
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


        private void Newbtn_Click(object sender, EventArgs e)
        {
            popupPanel.Visible = !popupPanel.Visible;
            guna2Panel2.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnFileUpload_Click(object sender, EventArgs e)
        {
            var addDocsForm = new addDocs(this);
            addDocsForm.StartPosition = FormStartPosition.CenterScreen;
            addDocsForm.TopMost = true; // Ensure the form appears on top
            addDocsForm.FormBorderStyle = FormBorderStyle.FixedDialog; // Set the form border style
            addDocsForm.MinimizeBox = false; // Remove minimize button
            addDocsForm.MaximizeBox = false; // Remove maximize button
            DialogResult result = addDocsForm.ShowDialog(this); // Show the form as a dialog
            popupPanel.Hide();
            if (result == DialogResult.OK)
            {
                LoadDocumentsIntoTablePanel();
            }
        }

        private void loadFromA_Z_Click(object sender, EventArgs e)
        {
            // Toggle the state
            isSortByNameAscending = !isSortByNameAscending;

            // Optionally, update the button's appearance to reflect the toggle state
            loadFromA_Z.FillColor = isSortByNameAscending ? Color.FromArgb(219, 195, 0) : Color.FromArgb(255, 236, 130);

            // Reload the documents with the updated sorting
            LoadDocumentsIntoTablePanel();
        }
    }
}






