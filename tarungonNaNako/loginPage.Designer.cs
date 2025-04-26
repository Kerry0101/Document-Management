
namespace tarungonNaNako
{
    partial class loginPage
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(loginPage));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            button1 = new Button();
            pictureBox1 = new PictureBox();
            panel1 = new Panel();
            HideButton = new Guna.UI2.WinForms.Guna2CircleButton();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            loginPanel = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel1.SuspendLayout();
            loginPanel.SuspendLayout();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox1.Location = new Point(141, 160);
            textBox1.Margin = new Padding(2);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(151, 32);
            textBox1.TabIndex = 0;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // textBox2
            // 
            textBox2.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox2.Location = new Point(141, 214);
            textBox2.Margin = new Padding(2);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(151, 32);
            textBox2.TabIndex = 1;
            textBox2.TextChanged += textBox2_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 11F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Black;
            label1.Location = new Point(38, 164);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(99, 25);
            label1.TabIndex = 2;
            label1.Text = "Username:";
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 11F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label2.Location = new Point(43, 218);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(94, 25);
            label2.TabIndex = 3;
            label2.Text = "Password:";
            label2.Click += label2_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 14F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label3.Location = new Point(126, 77);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(105, 32);
            label3.TabIndex = 4;
            label3.Text = "SIGN IN";
            label3.Click += label3_Click;
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            button1.Location = new Point(110, 286);
            button1.Margin = new Padding(2);
            button1.Name = "button1";
            button1.Size = new Size(129, 33);
            button1.TabIndex = 5;
            button1.Text = "LOGIN";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(434, 67);
            pictureBox1.Margin = new Padding(2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(191, 194);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 6;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Transparent;
            panel1.BorderStyle = BorderStyle.Fixed3D;
            panel1.Controls.Add(HideButton);
            panel1.Controls.Add(textBox1);
            panel1.Controls.Add(textBox2);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Location = new Point(8, 6);
            panel1.Margin = new Padding(2);
            panel1.Name = "panel1";
            panel1.Size = new Size(358, 416);
            panel1.TabIndex = 7;
            panel1.Paint += panel1_Paint;
            // 
            // HideButton
            // 
            HideButton.BackColor = SystemColors.Window;
            HideButton.DisabledState.BorderColor = Color.DarkGray;
            HideButton.DisabledState.CustomBorderColor = Color.DarkGray;
            HideButton.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            HideButton.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            HideButton.FillColor = SystemColors.Window;
            HideButton.Font = new Font("Segoe UI", 9F);
            HideButton.ForeColor = Color.Black;
            HideButton.Image = (Image)resources.GetObject("HideButton.Image");
            HideButton.ImageAlign = HorizontalAlignment.Left;
            HideButton.ImageOffset = new Point(-6, 0);
            HideButton.Location = new Point(256, 218);
            HideButton.Name = "HideButton";
            HideButton.ShadowDecoration.CustomizableEdges = customizableEdges1;
            HideButton.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            HideButton.Size = new Size(29, 22);
            HideButton.TabIndex = 26;
            HideButton.Click += HideButton_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label4.Location = new Point(434, 237);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(120, 25);
            label4.TabIndex = 8;
            label4.Text = "Welcome to ";
            label4.Click += label4_Click_1;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label5.Location = new Point(434, 261);
            label5.Margin = new Padding(2, 0, 2, 0);
            label5.Name = "label5";
            label5.Size = new Size(219, 25);
            label5.TabIndex = 9;
            label5.Text = "Mange Your Docoments";
            label5.Click += label5_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.Transparent;
            label6.Font = new Font("Script MT Bold", 15.2F, FontStyle.Bold);
            label6.Location = new Point(548, 231);
            label6.Name = "label6";
            label6.Size = new Size(116, 32);
            label6.TabIndex = 10;
            label6.Text = "DocSpace";
            // 
            // loginPanel
            // 
            loginPanel.Controls.Add(label6);
            loginPanel.Controls.Add(label5);
            loginPanel.Controls.Add(label4);
            loginPanel.Controls.Add(panel1);
            loginPanel.Controls.Add(pictureBox1);
            loginPanel.CustomizableEdges = customizableEdges2;
            loginPanel.Dock = DockStyle.Fill;
            loginPanel.FillColor = Color.FromArgb(255, 255, 192);
            loginPanel.FillColor2 = Color.FromArgb(255, 255, 192);
            loginPanel.FillColor3 = Color.FromArgb(255, 207, 64);
            loginPanel.FillColor4 = Color.FromArgb(255, 207, 64);
            loginPanel.Location = new Point(0, 0);
            loginPanel.Name = "loginPanel";
            loginPanel.ShadowDecoration.CustomizableEdges = customizableEdges3;
            loginPanel.Size = new Size(678, 435);
            loginPanel.TabIndex = 11;
            // 
            // loginPage
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(255, 255, 192);
            ClientSize = new Size(678, 435);
            Controls.Add(loginPanel);
            Margin = new Padding(2);
            MaximizeBox = false;
            Name = "loginPage";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            Load += loginPage_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            loginPanel.ResumeLayout(false);
            loginPanel.PerformLayout();
            ResumeLayout(false);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            
        }

        #endregion

        private TextBox textBox1;
        private TextBox textBox2;
        private Label label1;
        private Label label2;
        private Label label3;
        private Button button1;
        private PictureBox pictureBox1;
        private Panel panel1;
        private Label label5;
        private Label label4;
        private Guna.UI2.WinForms.Guna2CircleButton HideButton;
        private Label label6;
        private Guna.UI2.WinForms.Guna2CustomGradientPanel loginPanel;
    }
}
