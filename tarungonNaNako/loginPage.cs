using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Generators;
using BCrypt.Net;
using System.Security.Cryptography;
using System.Text;
using tarungonNaNako; // Make sure this is the namespace where the Session class is defined



namespace tarungonNaNako
{
    public partial class loginPage : Form
    {
        private bool isPasswordHidden = true; // Track password visibility
        private string EyeCrossed = Path.Combine(Application.StartupPath, "Assets (images)", "eye-crossed.png");
        private string EyeOpened = Path.Combine(Application.StartupPath, "Assets (images)", "eye.png");

        public loginPage()
        {
            InitializeComponent();
            string mysqlCon = "server=localhost; user=root; Database=docsmanagement; password=";
            MySqlConnection mySqlConnection = new MySqlConnection(mysqlCon);
            textBox2.PasswordChar = '●';
            isPasswordHidden = true; 

            HideButton.Image = Image.FromFile(EyeCrossed);
        }

        private void HideButton_Click(object sender, EventArgs e)
        {
            if (isPasswordHidden)
            {
                // Show the password
                textBox2.PasswordChar = '\0'; // '\0' means no masking
                isPasswordHidden = false;
                HideButton.Image = Image.FromFile(EyeOpened);
            }
            else
            {
                // Hide the password
                textBox2.PasswordChar = '●'; // Mask the password
                isPasswordHidden = true;
                HideButton.Image = Image.FromFile(EyeCrossed);
            }
        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim(); // Replace with your username textbox
            string password = textBox2.Text.Trim(); // Replace with your password textbox

            string mysqlCon = "server=localhost; user=root; Database=docsmanagement; password=";

            using (MySqlConnection mySqlConnection = new MySqlConnection(mysqlCon))
            {
                try
                {
                    mySqlConnection.Open();
                    string query = @"
                SELECT u.userId, r.roleName 
                FROM users u
                JOIN roles r ON u.roleId = r.roleId
                WHERE u.username = @username 
                AND u.password = @password 
                AND u.isArchived = 0";

                    using (MySqlCommand cmd = new MySqlCommand(query, mySqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password); // Ensure password is hashed if needed

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int loggedInUserId = reader.GetInt32("userId");
                                string roleName = reader.GetString("roleName");

                                // Set the session variables after successful login
                                Session.CurrentUserId = loggedInUserId;
                                Session.CurrentUserName = username;

                                MessageBox.Show($"Login successful! Role: {roleName}");

                                // Navigate to the appropriate dashboard based on the role
                                if (roleName == "Admin")
                                {
                                    adminDashboard adminDashboard = new adminDashboard(loggedInUserId);
                                    adminDashboard.Show();
                                    this.Hide();
                                }
                                else if (roleName == "Principal")
                                {
                                    principalDashboard principalDashboard = new principalDashboard();
                                    principalDashboard.Show();
                                    this.Hide();
                                }
                                else if (roleName == "Teacher")
                                {
                                    teacherDashboard teacherDashboard = new teacherDashboard();
                                    teacherDashboard.Show();
                                    this.Hide();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid username or password.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }



        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click_1(object sender, EventArgs e)
        {

        }

        private void loginPage_Load(object sender, EventArgs e)
        {

        }
    }
}
