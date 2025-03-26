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
    public partial class teacherDashboard : Form
    {

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
    }
}
