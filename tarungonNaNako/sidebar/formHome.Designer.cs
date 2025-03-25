
namespace tarungonNaNako
{
    partial class formHome
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            panel2 = new Panel();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.WhiteSmoke;
            panel1.Location = new Point(37, 37);
            panel1.Name = "panel1";
            panel1.Size = new Size(881, 272);
            panel1.TabIndex = 0;
            panel1.Paint += panel1_Paint;
            // 
            // panel2
            // 
            panel2.BackColor = Color.WhiteSmoke;
            panel2.Location = new Point(37, 338);
            panel2.Name = "panel2";
            panel2.Size = new Size(881, 272);
            panel2.TabIndex = 1;
            panel2.Paint += panel2_Paint;
            // 
            // formHome
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(224, 224, 224);
            ClientSize = new Size(954, 650);
            Controls.Add(panel2);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "formHome";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "Form1";
            Load += formDashboard_Load;
            ResumeLayout(false);
        }

        private void formDashboard_Load(object sender, EventArgs e)
        {

        }

        #endregion

        private Panel panel1;
        private Panel panel2;
    }
}