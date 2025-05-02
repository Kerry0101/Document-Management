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

        public adminDashboard(int userId)
        {
            InitializeComponent();
            HighlightButton(button1);
            loggedInUserId = userId; // Save logged-in admin's user ID
        }

        private void HighlightButton(Guna.UI2.WinForms.Guna2Button buttonToHighlight)
        {
            // Reset the highlight for all buttons
            button1.FillColor = Color.Transparent;
            button2.FillColor = Color.Transparent;
            button4.FillColor = Color.Transparent;
            button3.FillColor = Color.Transparent;
            button5.FillColor = Color.Transparent;
            button6.FillColor = Color.Transparent;
            button7.FillColor = Color.Transparent;
            button8.FillColor = Color.Transparent;
            button9.FillColor = Color.Transparent;
            Shared.FillColor = Color.Transparent;

            // Highlight the selected button
            buttonToHighlight.FillColor = Color.FromArgb(219, 195, 0);
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
            HighlightButton(button2);
            LoadFormInPanel(new manageDocs());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HighlightButton(button1);
            LoadFormInPanel(new homepage());
        }

        private void adminDashboard_Load_1(object sender, EventArgs e)
        {
            LoadFormInPanel(new homepage());  // Load formDashboard on load
        }

        private void button4_Click(object sender, EventArgs e)
        {
            HighlightButton(button4);
            LoadFormInPanel(new docsApproval());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            HighlightButton(button5);
            LoadFormInPanel(new addRole());
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            HighlightButton(button6);
            manageUser manageUserForm = new manageUser(loggedInUserId);
            LoadFormInPanel(manageUserForm);

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            HighlightButton(button3);
            LoadFormInPanel(new categories());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            HighlightButton(button7);
            LoadFormInPanel(new profile());
        }

        private void button9_Click(object sender, EventArgs e)
        {
            HighlightButton(button9);
            LoadFormInPanel(new archived());
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            HighlightButton(button8);
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

        private void button7_Click_1(object sender, EventArgs e)
        {

        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MinimizeBtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Shared_Click(object sender, EventArgs e)
        {
            HighlightButton(Shared);
            LoadFormInPanel(new SharedFiles());
        }
    }
}
