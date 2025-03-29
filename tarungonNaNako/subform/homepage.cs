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
                                    ColumnCount = 3,
                                    Dock = DockStyle.Fill,
                                    Height = 50, // Adjust row height
                                    BackColor = ColorTranslator.FromHtml("#ffe261")
                                };

                                // Set column sizes
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200)); // File Name
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180)); // Modification Time
                                rowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150)); // Category

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

                                // ✅ Add hover effect to row and its labels 255, 255, 192
                                void RowHover(object sender, EventArgs e) => rowTable.BackColor = Color.FromArgb(219, 195, 0);
                                void RowLeave(object sender, EventArgs e) => rowTable.BackColor = ColorTranslator.FromHtml("#ffe261");

                                rowTable.MouseEnter += RowHover;
                                rowTable.MouseLeave += RowLeave;

                                fileLabel.MouseEnter += RowHover;
                                fileLabel.MouseLeave += RowLeave;

                                dateLabel.MouseEnter += RowHover;
                                dateLabel.MouseLeave += RowLeave;

                                categoryLabel.MouseEnter += RowHover;
                                categoryLabel.MouseLeave += RowLeave;

                                // ✅ Add Labels to rowTable
                                rowTable.Controls.Add(fileLabel, 0, 0);
                                rowTable.Controls.Add(dateLabel, 1, 0);
                                rowTable.Controls.Add(categoryLabel, 2, 0);

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
                    string query = "SELECT categoryName FROM category LIMIT 5";
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

                        // Create Three-Dot Menu Button
                        Guna.UI2.WinForms.Guna2CircleButton menuButton = new Guna.UI2.WinForms.Guna2CircleButton();
                        menuButton.Image = Image.FromFile(ThreeDotMenu);
                        menuButton.ImageSize = new Size(15, 15);
                        menuButton.ImageAlign = HorizontalAlignment.Center;
                        menuButton.ImageOffset = new Point(0, 12);
                        menuButton.BackColor = Color.FromArgb(255, 226, 97);
                        menuButton.FillColor = Color.Transparent;
                        menuButton.Size = new Size(21, 26);
                        menuButton.Text = "⋮";
                        menuButton.Location = new Point(xPosition + buttonWidth - 25, 15);
                        menuButton.Click += (s, e) => ShowContextMenu(categoryName, menuButton);

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
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add("Edit").Click += (s, e) => EditCategory(categoryName);
            menu.Items.Add("Delete").Click += (s, e) => DeleteCategory(categoryName);
            menu.Show(btn, new Point(0, btn.Height));
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

        private void guna2hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }




        private void EditCategory(string categoryName)
        {
            // Implement the logic to edit the category here
            MessageBox.Show($"Edit category: {categoryName}");
        }

        private void DeleteCategory(string categoryName)
        {
            // Implement the logic to delete the category here
            MessageBox.Show($"Delete category: {categoryName}");
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
