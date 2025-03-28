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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace tarungonNaNako
{
    public partial class teacherDashboard : Form
    {
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;
        public teacherDashboard()
        {
            InitializeComponent();
            LoadFormInPanel(new homepage());
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

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadFormInPanel(new categories());

        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadFormInPanel(new homepage());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadFormInPanel(new manageDocs());
        }

        private void button9_Click(object sender, EventArgs e)
        {
            LoadFormInPanel(new archived());
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) // Detect left mouse button press
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void MinimizeBtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

    }
}
