using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tarungonNaNako
{
    public partial class PasswordPrompt : Form
    {
        public string EnteredPassword { get; private set; } = string.Empty;
        private bool isPasswordHidden = true; // Track the visibility state of the password
        private string EyeCrossed = Path.Combine(Application.StartupPath, "Assets (images)", "eye-crossed.png");
        private string EyeOpened = Path.Combine(Application.StartupPath, "Assets (images)", "eye.png");

        public PasswordPrompt()
        {
            InitializeComponent();
            HideButton.Image = Image.FromFile(EyeCrossed);
            txtPassword.PasswordChar = '●';
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Store the entered password and close the dialog
            EnteredPassword = txtPassword.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Close the dialog without doing anything
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void HideButton_Click(object sender, EventArgs e)
        {
            if (isPasswordHidden)
            {
                // Show the password
                txtPassword.PasswordChar = '\0'; // '\0' means no masking
                isPasswordHidden = false;
                HideButton.Image = Image.FromFile(EyeOpened); // Change to "eye-opened" icon
            }
            else
            {
                // Hide the password
                txtPassword.PasswordChar = '●'; // Mask the password
                isPasswordHidden = true;
                HideButton.Image = Image.FromFile(EyeCrossed); // Change to "eye-crossed" icon
            }
        }
    }
}
