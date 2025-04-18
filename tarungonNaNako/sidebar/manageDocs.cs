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

namespace tarungonNaNako.sidebar
{
    public partial class manageDocs : Form
    {
        private readonly string connectionString = "server=localhost;user=root;database=docsmanagement;password="; // DB connection
        string ThreeDotMenu = Path.Combine(Application.StartupPath, "Assets (images)", "menu-dots-vertical.png");
        string DocumentIcon = Path.Combine(Application.StartupPath, "Assets (images)", "document.png");
        string ZippedFolderIcon = Path.Combine(Application.StartupPath, "Assets (images)", "zip-file-format.png");
        private BackgroundWorker backgroundWorker;
        private System.Timers.Timer debounceTimer;

        public manageDocs()
        {
            InitializeComponent();
            InitializeBackgroundWorker();
            popupPanel.Visible = false;
            LoadDocumentsIntoTablePanel();
            searchBar.TextChanged += SearchBar_TextChanged;
            loadingPictureBox.Visible = false;

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
            LoadDocumentsIntoTablePanel(searchTerm);
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
                Invoke(new Action(() => LoadDocumentsIntoTablePanel(searchTerm)));
                return;
            }

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
                   SELECT f.fileName, f.uploadDate, c.categoryName  
                   FROM files f  
                   JOIN category c ON f.categoryId = c.categoryId  
                   WHERE f.isArchived = 0
                   AND f.userId = @userId  
                   ORDER BY f.uploadDate DESC"; // Filter by logged-in user and order by uploadDate 

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", Session.CurrentUserId); // Bind the logged-in user ID  
                        cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Clear existing rows  
                            tableLayoutPanel1.Controls.Clear();
                            tableLayoutPanel1.RowCount = 5;
                            tableLayoutPanel1.Padding = new Padding(0, 0, 0, 10);
                            tableLayoutPanel1.RowStyles.Clear();

                            int rowIndex = 0;

                            while (reader.Read())
                            {
                                string fileName = reader["fileName"].ToString();
                                string modificationTime = Convert.ToDateTime(reader["uploadDate"]).ToString("yyyy-MM-dd hh:mm tt");
                                string categoryName = reader["categoryName"].ToString();

                                // ✅ Create TableLayoutPanel for Row  
                                TableLayoutPanel rowTable = new TableLayoutPanel
                                {
                                    ColumnCount = 5,
                                    Dock = DockStyle.Fill,
                                    Height = 60, // Adjust row height  
                                    BackColor = ColorTranslator.FromHtml("#ffe261")
                                };

                                // Set column sizes  
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40)); // Image Icon  
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200)); // File Name  
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180)); // Modification Time  
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180)); // Category  
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 10)); // Action  

                                // Determine the icon based on file type  
                                Image icon;
                                string fileExtension = Path.GetExtension(fileName).ToLower();
                                if (fileExtension == ".zip")
                                {
                                    icon = Image.FromFile(ZippedFolderIcon); // Use zipped folder icon  
                                }
                                else
                                {
                                    icon = Image.FromFile(DocumentIcon); // Use document icon  
                                }

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
                                    //ShowPanel("file", fileName, categoryName, actionButton); // Show panel with file-related options  
                                    popupPanel.Hide(); // Hide the category panel if it's open  
                                };

                                // ✅ Add hover effect to row and its labels  
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
                                    Image = icon,
                                    SizeMode = PictureBoxSizeMode.Zoom, // Change to Zoom to maintain aspect ratio  
                                    Margin = new Padding(13, 17, 5, 5),
                                    Text = ":",
                                    Width = 25,
                                    Height = 25
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

        private void manageDocs_Load(object sender, EventArgs e)
        {

        }


        private void Newbtn_Click(object sender, EventArgs e)
        {
            popupPanel.Visible = !popupPanel.Visible;
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
    }
}






