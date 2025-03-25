namespace tarungonNaNako
{
    partial class principalDashboard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(principalDashboard));
            panel3 = new Panel();
            panel2 = new Panel();
            panel4 = new Panel();
            button8 = new Button();
            button4 = new Button();
            button3 = new Button();
            button6 = new Button();
            button2 = new Button();
            button1 = new Button();
            panel1 = new Panel();
            pictureBox1 = new PictureBox();
            panel2.SuspendLayout();
            panel4.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(224, 224, 224);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(242, 62);
            panel3.Name = "panel3";
            panel3.Size = new Size(954, 650);
            panel3.TabIndex = 5;
            panel3.Paint += panel3_Paint;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(224, 224, 224);
            panel2.Controls.Add(panel4);
            panel2.Dock = DockStyle.Left;
            panel2.Location = new Point(0, 62);
            panel2.Name = "panel2";
            panel2.Size = new Size(242, 650);
            panel2.TabIndex = 4;
            // 
            // panel4
            // 
            panel4.BackColor = Color.WhiteSmoke;
            panel4.Controls.Add(button8);
            panel4.Controls.Add(button4);
            panel4.Controls.Add(button3);
            panel4.Controls.Add(button6);
            panel4.Controls.Add(button2);
            panel4.Controls.Add(button1);
            panel4.Dock = DockStyle.Bottom;
            panel4.Location = new Point(0, 6);
            panel4.Name = "panel4";
            panel4.Size = new Size(242, 644);
            panel4.TabIndex = 0;
            // 
            // button8
            // 
            button8.BackColor = Color.White;
            button8.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Bold);
            button8.Location = new Point(6, 508);
            button8.Name = "button8";
            button8.Size = new Size(230, 53);
            button8.TabIndex = 8;
            button8.Text = "LOGOUT";
            button8.UseVisualStyleBackColor = false;
            // 
            // button4
            // 
            button4.BackColor = Color.White;
            button4.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Bold);
            button4.Location = new Point(6, 76);
            button4.Name = "button4";
            button4.Size = new Size(230, 53);
            button4.TabIndex = 3;
            button4.Text = "DOCUMENT APPROVAL";
            button4.UseVisualStyleBackColor = false;
            button4.Click += button4_Click;
            // 
            // button3
            // 
            button3.BackColor = Color.White;
            button3.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Bold);
            button3.Location = new Point(6, 194);
            button3.Name = "button3";
            button3.Size = new Size(230, 53);
            button3.TabIndex = 6;
            button3.Text = "test cat";
            button3.UseVisualStyleBackColor = false;
            // 
            // button6
            // 
            button6.BackColor = Color.White;
            button6.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Bold);
            button6.Location = new Point(6, 253);
            button6.Name = "button6";
            button6.Size = new Size(230, 53);
            button6.TabIndex = 5;
            button6.Text = "test cat";
            button6.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            button2.BackColor = Color.White;
            button2.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Bold);
            button2.Location = new Point(6, 135);
            button2.Name = "button2";
            button2.Size = new Size(230, 53);
            button2.TabIndex = 1;
            button2.Text = "MANAGE DOCUMENT";
            button2.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            button1.BackColor = Color.White;
            button1.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Bold);
            button1.Location = new Point(6, 17);
            button1.Name = "button1";
            button1.Size = new Size(230, 53);
            button1.TabIndex = 0;
            button1.Text = "HOME";
            button1.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            panel1.BackColor = Color.WhiteSmoke;
            panel1.Controls.Add(pictureBox1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1196, 62);
            panel1.TabIndex = 3;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(27, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(60, 62);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // principalDashboard
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1196, 712);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "principalDashboard";
            Text = "principalDashboard";
            panel2.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel3;
        private Panel panel2;
        private Panel panel4;
        private Button button8;
        private Button button4;
        private Button button3;
        private Button button6;
        private Button button2;
        private Button button1;
        private Panel panel1;
        private PictureBox pictureBox1;
    }
}