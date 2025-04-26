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

namespace tarungonNaNako.subform
{
    public partial class addRole : Form
    {
        public addRole()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            adminDashboard parentForm = this.ParentForm as adminDashboard;

            if (parentForm != null)
            {
                formRole manageRole = new formRole();
                manageRole.StartPosition = FormStartPosition.Manual;
                manageRole.Location = new Point(663, 270);
                manageRole.FormBorderStyle = FormBorderStyle.FixedDialog;
                manageRole.MinimizeBox = false;
                manageRole.MaximizeBox = false;
                manageRole.ShowDialog(this);

            }
            else
            {
                MessageBox.Show("Parent form not found. Please ensure addRole is opened from adminDashboard.");
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
