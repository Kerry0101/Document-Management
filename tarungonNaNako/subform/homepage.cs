using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace tarungonNaNako.subform
{
    public partial class homepage : Form
    {
        private string connectionString = "server=localhost;database=docsmanagement;uid=root;pwd=;";
        private Image originalImage;

        public homepage()

        {
            InitializeComponent();
            LoadPngImage();
            LoadFilesIntoTablePanel();
        }

        private void homepage_Load(object sender, EventArgs e)
        {
            flowLayoutPanel1.Visible = Properties.Settings.Default.isPanel1Visible;
            panel5.Visible = Properties.Settings.Default.isPanel5Visible;

            float rotationAngle2 = Properties.Settings.Default.isArrowDown2 ? 0 : -90;
            pictureBox2.Image = RotateImage(originalImage, rotationAngle2);

            float rotationAngle3 = Properties.Settings.Default.isArrowDown3 ? 0 : -90;
            pictureBox3.Image = RotateImage(originalImage, rotationAngle3);

            LoadCategoriesIntoButtons();  // Load categories dynamically
            //LoadRecentFiles();
        }

        //private void LoadRecentFiles()
        //{
        //    string connectionString = "server=localhost;database=docsmanagement;uid=root;pwd=;";
        //    using (MySqlConnection conn = new MySqlConnection(connectionString))
        //    {
        //        try
        //        {
        //            conn.Open();
        //            string query = @"
        //        SELECT f.fileName, f.updated_at, c.categoryName
        //        FROM files f
        //        LEFT JOIN category c ON f.categoryId = c.categoryId
        //        ORDER BY f.updated_at DESC
        //        LIMIT 5";  // Adjust the limit as needed

        //            MySqlCommand cmd = new MySqlCommand(query, conn);
        //            MySqlDataReader reader = cmd.ExecuteReader();

        //            // Clear the table before adding new rows
        //            tableLayoutPanel1.Controls.Clear();
        //            tableLayoutPanel1.RowCount = 0; // Reset row count
        //            tableLayoutPanel1.ColumnCount = 3; // Ensure 3 columns exist

        //            while (reader.Read())
        //            {
        //                string fileName = reader["fileName"].ToString();
        //                string modificationTime = Convert.ToDateTime(reader["updated_at"]).ToString("yyyy-MM-dd HH:mm:ss");
        //                string categoryName = reader["categoryName"].ToString();

        //                int newRowIndex = tableLayoutPanel1.RowCount;
        //                tableLayoutPanel1.RowCount++; // Add a new row

        //                // Create labels for each column
        //                Label fileLabel = new Label
        //                {
        //                    Text = fileName,
        //                    AutoSize = true,
        //                    Font = new Font("Arial", 10, FontStyle.Regular),
        //                    TextAlign = ContentAlignment.MiddleLeft,
        //                    Dock = DockStyle.Fill
        //                };

        //                Label dateLabel = new Label
        //                {
        //                    Text = modificationTime,
        //                    AutoSize = true,
        //                    Font = new Font("Arial", 10, FontStyle.Regular),
        //                    TextAlign = ContentAlignment.MiddleLeft,
        //                    Dock = DockStyle.Fill
        //                };

        //                Label categoryLabel = new Label
        //                {
        //                    Text = categoryName,
        //                    AutoSize = true,
        //                    Font = new Font("Arial", 10, FontStyle.Regular),
        //                    TextAlign = ContentAlignment.MiddleLeft,
        //                    Dock = DockStyle.Fill
        //                };

        //                // Add controls to the TableLayoutPanel
        //                tableLayoutPanel1.Controls.Add(fileLabel, 0, newRowIndex);
        //                tableLayoutPanel1.Controls.Add(dateLabel, 1, newRowIndex);
        //                tableLayoutPanel1.Controls.Add(categoryLabel, 2, newRowIndex);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Error loading files: " + ex.Message);
        //        }
        //        finally
        //        {
        //            conn.Close();
        //        }
        //    }
        //}


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

                            int fixedRowHeight = 60; // Adjust row height as needed
                            int rowIndex = 0;

                            while (reader.Read())
                            {
                                string fileName = reader["fileName"].ToString();
                                string modificationTime = Convert.ToDateTime(reader["updated_at"]).ToString("yyyy-MM-dd HH:mm:ss");
                                string categoryName = reader["categoryName"].ToString();

                                // 🔵 Create a Panel for each row
                                Panel rowPanel = new Panel
                                {
                                    Dock = DockStyle.Fill,
                                    BackColor = Color.White,
                                    Height = fixedRowHeight
                                };

                                // Add hover effect
                                rowPanel.MouseEnter += (s, e) => rowPanel.BackColor = Color.LightGray;
                                rowPanel.MouseLeave += (s, e) => rowPanel.BackColor = Color.White;

                                // 🔴 Create labels for File Name, Date, and Category
                                Label fileLabel = new Label
                                {
                                    Text = fileName,
                                    Dock = DockStyle.Left,
                                    AutoSize = false,
                                    Width = 200,
                                    TextAlign = ContentAlignment.MiddleLeft,
                                    Padding = new Padding(10, 0, 0, 0)
                                };

                                Label dateLabel = new Label
                                {
                                    Text = modificationTime,
                                    Dock = DockStyle.Left,
                                    AutoSize = false,
                                    Width = 180,
                                    TextAlign = ContentAlignment.MiddleLeft,
                                    Padding = new Padding(10, 0, 0, 0)
                                };

                                Label categoryLabel = new Label
                                {
                                    Text = categoryName,
                                    Dock = DockStyle.Left,
                                    AutoSize = false,
                                    Width = 150,
                                    TextAlign = ContentAlignment.MiddleLeft,
                                    Padding = new Padding(10, 0, 0, 0)
                                };

                                // 🔵 Add labels to the row panel
                                rowPanel.Controls.Add(fileLabel);
                                rowPanel.Controls.Add(dateLabel);
                                rowPanel.Controls.Add(categoryLabel);

                                // 🔴 Add rowPanel to TableLayoutPanel
                                tableLayoutPanel1.RowCount = rowIndex + 1;
                                tableLayoutPanel1.Controls.Add(rowPanel, 0, rowIndex);
                                tableLayoutPanel1.SetColumnSpan(rowPanel, 3); // Span all columns

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
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT categoryName FROM category LIMIT 4";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    // Assuming you have 4 Guna2Buttons named guna2Button1 to guna2Button4
                    Guna.UI2.WinForms.Guna2Button[] buttons = { guna2Button1, guna2Button2, guna2Button3, guna2Button4 };

                    int i = 0;
                    while (reader.Read() && i < buttons.Length)
                    {
                        buttons[i].Text = reader["categoryName"].ToString();
                        i++;
                    }

                    // Hide extra buttons if less than 4 categories
                    for (; i < buttons.Length; i++)
                    {
                        buttons[i].Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading categories: " + ex.Message);
                }
                finally
                {
                    conn.Close();
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
            flowLayoutPanel1.Visible = !flowLayoutPanel1.Visible;
            Properties.Settings.Default.isPanel1Visible = flowLayoutPanel1.Visible;

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
    }
}
