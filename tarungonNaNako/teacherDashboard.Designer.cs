namespace tarungonNaNako
{
    partial class teacherDashboard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(teacherDashboard));
            panel3 = new Panel();
            pictureBox3 = new PictureBox();
            label3 = new Label();
            pictureBox2 = new PictureBox();
            label1 = new Label();
            panel2 = new Panel();
            panel4 = new Panel();
            button7 = new Button();
            button9 = new Button();
            button8 = new Button();
            button3 = new Button();
            button2 = new Button();
            button1 = new Button();
            panel1 = new Panel();
            label2 = new Label();
            pictureBox1 = new PictureBox();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panel2.SuspendLayout();
            panel4.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(224, 224, 224);
            panel3.Controls.Add(pictureBox3);
            panel3.Controls.Add(label3);
            panel3.Controls.Add(pictureBox2);
            panel3.Controls.Add(label1);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(194, 50);
            panel3.Margin = new Padding(2);
            panel3.Name = "panel3";
            panel3.Size = new Size(763, 520);
            panel3.TabIndex = 5;
            panel3.Click += pictureBox2_Click;
            panel3.Paint += panel3_Paint;
            // 
            // pictureBox3
            // 
            pictureBox3.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox3.Location = new Point(282, 182);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(25, 22);
            pictureBox3.TabIndex = 3;
            pictureBox3.TabStop = false;
            pictureBox3.Click += pictureBox3_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(21, 182);
            label3.Name = "label3";
            label3.Size = new Size(260, 20);
            label3.TabIndex = 2;
            label3.Text = "Recently modified Documents";
            // 
            // pictureBox2
            // 
            pictureBox2.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox2.Location = new Point(158, 18);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(25, 22);
            pictureBox2.TabIndex = 1;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(21, 17);
            label1.Name = "label1";
            label1.Size = new Size(137, 20);
            label1.TabIndex = 0;
            label1.Text = "Recent Folders";
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(224, 224, 224);
            panel2.Controls.Add(panel4);
            panel2.Dock = DockStyle.Left;
            panel2.Location = new Point(0, 50);
            panel2.Margin = new Padding(2);
            panel2.Name = "panel2";
            panel2.Size = new Size(194, 520);
            panel2.TabIndex = 4;
            // 
            // panel4
            // 
            panel4.BackColor = Color.WhiteSmoke;
            panel4.Controls.Add(button7);
            panel4.Controls.Add(button9);
            panel4.Controls.Add(button8);
            panel4.Controls.Add(button3);
            panel4.Controls.Add(button2);
            panel4.Controls.Add(button1);
            panel4.Dock = DockStyle.Bottom;
            panel4.Location = new Point(0, 5);
            panel4.Margin = new Padding(2);
            panel4.Name = "panel4";
            panel4.Size = new Size(194, 515);
            panel4.TabIndex = 0;
            // 
            // button7
            // 
            button7.BackColor = Color.White;
            button7.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Bold);
            button7.Location = new Point(5, 359);
            button7.Margin = new Padding(2);
            button7.Name = "button7";
            button7.Size = new Size(184, 42);
            button7.TabIndex = 10;
            button7.Text = "PROFILE";
            button7.UseVisualStyleBackColor = false;
            // 
            // button9
            // 
            button9.BackColor = Color.White;
            button9.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Bold);
            button9.Location = new Point(5, 155);
            button9.Margin = new Padding(2);
            button9.Name = "button9";
            button9.Size = new Size(184, 42);
            button9.TabIndex = 9;
            button9.Text = "ARCHIVED";
            button9.UseVisualStyleBackColor = false;
            // 
            // button8
            // 
            button8.BackColor = Color.White;
            button8.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Bold);
            button8.Location = new Point(5, 406);
            button8.Margin = new Padding(2);
            button8.Name = "button8";
            button8.Size = new Size(184, 42);
            button8.TabIndex = 8;
            button8.Text = "LOGOUT";
            button8.UseVisualStyleBackColor = false;
            // 
            // button3
            // 
            button3.BackColor = Color.White;
            button3.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Bold);
            button3.Location = new Point(5, 108);
            button3.Margin = new Padding(2);
            button3.Name = "button3";
            button3.Size = new Size(184, 42);
            button3.TabIndex = 6;
            button3.Text = "CATEGORIES";
            button3.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            button2.BackColor = Color.White;
            button2.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Bold);
            button2.Location = new Point(5, 61);
            button2.Margin = new Padding(2);
            button2.Name = "button2";
            button2.Size = new Size(184, 42);
            button2.TabIndex = 1;
            button2.Text = "MANAGE DOCUMENT";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.White;
            button1.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Bold);
            button1.Location = new Point(5, 14);
            button1.Margin = new Padding(2);
            button1.Name = "button1";
            button1.Size = new Size(184, 42);
            button1.TabIndex = 0;
            button1.Text = "HOME";
            button1.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            panel1.BackColor = Color.WhiteSmoke;
            panel1.Controls.Add(label2);
            panel1.Controls.Add(pictureBox1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(2);
            panel1.Name = "panel1";
            panel1.Size = new Size(957, 50);
            panel1.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(76, 16);
            label2.Name = "label2";
            label2.Size = new Size(200, 20);
            label2.TabIndex = 1;
            label2.Text = "Welcome to DocSpace";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(22, 0);
            pictureBox1.Margin = new Padding(2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(48, 50);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // teacherDashboard
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(957, 570);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Margin = new Padding(2);
            MaximizeBox = false;
            Name = "teacherDashboard";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "teacherDashboard";
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            panel2.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel3;
        private Panel panel2;
        private Panel panel4;
        private Button button7;
        private Button button9;
        private Button button8;
        private Button button3;
        private Button button2;
        private Button button1;
        private Panel panel1;
        private PictureBox pictureBox1;
        private Label label1;
        private Label label2;
        private PictureBox pictureBox2;
        private PictureBox pictureBox3;
        private Label label3;
    }
}