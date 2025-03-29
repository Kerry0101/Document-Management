using MySql.Data.MySqlClient;

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
using Guna.UI2.WinForms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using tarungonNaNako.sidebar;

namespace tarungonNaNako.subform
{
    public partial class homepage : Form
    {
        private string connectionString = "server=localhost;database=docsmanagement;uid=root;pwd=;";
        private Image originalImage;
        string Folder = Path.Combine(Application.StartupPath, "Assets (images)", "folder.png");
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
                                     JOIN category c ON f.categoryId = c.categoryId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Clear existing rows
                            tableLayoutPanel1.Controls.Clear();
                            tableLayoutPanel1.RowCount = 0;
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
                                    ColumnCount = 4,
                                    Dock = DockStyle.Fill,
                                    Height = 50, // Adjust row height
                                    BackColor = ColorTranslator.FromHtml("#ffe261")
                                };

                                // Set column sizes
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200)); // File Name
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180)); // Modification Time
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150)); // Category
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150)); // Action

                                // 🔴 Create Labels (aligned properly)
                                Label fileLabel = new Label
                                {
                                    Text = fileName,
                                    Dock = DockStyle.Fill,
                                    AutoSize = false,
                                    TextAlign = ContentAlignment.MiddleLeft,
                                    Padding = new Padding(10, 0, 0, 0),
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
                                rowTable.Controls.Add(fileLabel, 0, 0);
                                rowTable.Controls.Add(dateLabel, 1, 0);
                                rowTable.Controls.Add(categoryLabel, 2, 0);
                                rowTable.Controls.Add(actionButton, 3, 0);

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

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT categoryName FROM category WHERE is_archived = 0 LIMIT 5";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string categoryName = reader["categoryName"].ToString();

                        // Create Category Button
                        Guna.UI2.WinForms.Guna2Button categoryButton = new Guna.UI2.WinForms.Guna2Button();
                        categoryButton.Text = categoryName;
                        categoryButton.Width = buttonWidth;
                        categoryButton.Height = buttonHeight;
                        categoryButton.BorderRadius = 10;
                        categoryButton.PressedDepth = 0;
                        categoryButton.FillColor = Color.FromArgb(255, 226, 97);
                        categoryButton.ForeColor = Color.Black;
                        categoryButton.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                        categoryButton.Image = Image.FromFile(Folder);
                        categoryButton.ImageSize = new Size(15, 15);
                        categoryButton.ImageAlign = HorizontalAlignment.Left;
                        categoryButton.TextAlign = HorizontalAlignment.Left;
                        categoryButton.Location = new Point(xPosition, 0);
                        categoryButton.Cursor = Cursors.Hand;
                        categoryButton.Click += (s, e) =>
                        {
                            LoadFormInPanel(new fetchDocuments());
                        };

                        // Create Three-Dot Menu Button
                        Guna.UI2.WinForms.Guna2CircleButton menuButton = new Guna.UI2.WinForms.Guna2CircleButton();
                        menuButton.Image = Image.FromFile(ThreeDotMenu);
                        menuButton.ImageSize = new Size(15, 15);
                        menuButton.ImageAlign = HorizontalAlignment.Center;
                        menuButton.ImageOffset = new Point(0, 12);
                        menuButton.BackColor = Color.FromArgb(255, 226, 97);
                        menuButton.FillColor = Color.Transparent;
                        menuButton.Size = new Size(28, 26);
                        menuButton.Text = "⋮";
                        menuButton.Location = new Point(xPosition + buttonWidth - 35, 15);
                        menuButton.Click += (s, e) => ShowContextMenu(categoryName, menuButton);
                        menuButton.PressedDepth = 10;

                        // Attach MouseEnter and MouseLeave event handlers
                        categoryButton.MouseEnter += (s, e) => menuButton.BackColor = Color.FromArgb(219, 195, 0);
                        categoryButton.MouseLeave += (s, e) => menuButton.BackColor = ColorTranslator.FromHtml("#ffe261");

                        // Add controls to Panel
                        Guna2Panel1.Controls.Add(menuButton);
                        Guna2Panel1.Controls.Add(categoryButton);

                        xPosition += buttonWidth + spacing;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading categories: " + ex.Message);
                }
            }
        }




        // Example function for showing the menu when clicking the three-dot button
        private void ShowContextMenu(string categoryName, Control btn)
        {
            // Toggle visibility of guna2Panel2
            if (guna2Panel2.Visible)
            {
                guna2Panel2.Visible = false;
                return;
            }

            string download = Path.Combine(Application.StartupPath, "Assets (images)", "down-to-line.png");
            string rename = Path.Combine(Application.StartupPath, "Assets (images)", "pencil.png");
            string remove = Path.Combine(Application.StartupPath, "Assets (images)", "trash.png");

            // Clear previous controls in guna2Panel2 (if any)
            guna2Panel2.Controls.Clear();

            // Set panel properties
            guna2Panel2.Size = new Size(181, 132); // Adjust size
            guna2Panel2.BorderRadius = 5;
            guna2Panel2.BackColor = Color.FromArgb(255, 255, 192);
            guna2Panel2.BringToFront();
            guna2Panel2.Font = new Font("Segoe UI", 9);
            guna2Panel2.ForeColor = Color.Black;

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
            btnRename.Click += (s, e) => EditCategory(GetCategoryIdByName(categoryName), categoryName);

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
            btnDelete.Click += (s, e) => RemoveCategory(categoryName);

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
                panelY = btnScreenLocation.Y - panelHeight - 5;
            }

            // Apply final position
            guna2Panel2.Location = new Point(panelX, panelY);
            guna2Panel2.Visible = true;
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
        }
    }
}
