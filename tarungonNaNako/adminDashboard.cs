using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using tarungonNaNako.sidebar;
using tarungonNaNako.subform;

namespace tarungonNaNako
{
    public partial class adminDashboard : Form
    {
        private int loggedInUserId; // Store logged-in user ID

        public adminDashboard()
        {
            InitializeComponent();
        }
        public adminDashboard(int userId)
        {
            InitializeComponent();
            loggedInUserId = userId; // Save logged-in admin's user ID
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

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadFormInPanel(new manageDocs());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadFormInPanel(new formHome());
        }

        private void adminDashboard_Load_1(object sender, EventArgs e)
        {
            LoadFormInPanel(new formHome());  // Load formDashboard on load
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadFormInPanel(new docsApproval());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            LoadFormInPanel(new addRole());
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            manageUser manageUserForm = new manageUser(loggedInUserId);
            LoadFormInPanel(manageUserForm);

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            LoadFormInPanel(new categories());
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            LoadFormInPanel(new archived());
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            // Show a confirmation dialog
            DialogResult result = MessageBox.Show(
                "Are you sure you want to log out?",
                "Logout Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                // Close the current dashboard
                this.Close();

                // Show the login page
                loginPage login = new loginPage();
                login.Show();
            }
        }
    }
}
