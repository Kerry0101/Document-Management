namespace tarungonNaNako.subform
{
    partial class addCategory
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
            label1 = new Label();
            panel2 = new Panel();
            panel1 = new Panel();
            checkBox3 = new CheckBox();
            checkBox2 = new CheckBox();
            label3 = new Label();
            checkBox1 = new CheckBox();
            button2 = new Button();
            button1 = new Button();
            label2 = new Label();
            textBox1 = new TextBox();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            label1.Location = new Point(37, 29);
            label1.Name = "label1";
            label1.Size = new Size(249, 32);
            label1.TabIndex = 0;
            label1.Text = "ADD NEW CATEGORY";
            // 
            // panel2
            // 
            panel2.BorderStyle = BorderStyle.Fixed3D;
            panel2.Controls.Add(label1);
            panel2.Controls.Add(panel1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(954, 650);
            panel2.TabIndex = 2;
            panel2.Paint += panel2_Paint;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(checkBox3);
            panel1.Controls.Add(checkBox2);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(checkBox1);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(textBox1);
            panel1.Location = new Point(37, 99);
            panel1.Name = "panel1";
            panel1.Size = new Size(871, 521);
            panel1.TabIndex = 1;
            panel1.Paint += panel1_Paint;
            // 
            // checkBox3
            // 
            checkBox3.AutoSize = true;
            checkBox3.Location = new Point(292, 111);
            checkBox3.Name = "checkBox3";
            checkBox3.Size = new Size(299, 29);
            checkBox3.TabIndex = 15;
            checkBox3.Text = "Needs Approval to upload a file?";
            checkBox3.UseVisualStyleBackColor = true;
            checkBox3.CheckedChanged += checkBox3_CheckedChanged_1;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Location = new Point(41, 176);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(96, 29);
            checkBox2.TabIndex = 14;
            checkBox2.Text = "Teacher";
            checkBox2.UseVisualStyleBackColor = true;
            checkBox2.CheckedChanged += checkBox2_CheckedChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(41, 111);
            label3.Name = "label3";
            label3.Size = new Size(153, 25);
            label3.TabIndex = 9;
            label3.Text = "Who can Upload?";
            label3.Click += label3_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(41, 141);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(104, 29);
            checkBox1.TabIndex = 8;
            checkBox1.Text = "Principal";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // button2
            // 
            button2.Location = new Point(236, 322);
            button2.Name = "button2";
            button2.Size = new Size(150, 34);
            button2.TabIndex = 7;
            button2.Text = "Cancel";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Location = new Point(41, 322);
            button1.Name = "button1";
            button1.Size = new Size(136, 34);
            button1.TabIndex = 6;
            button1.Text = "Save";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(41, 28);
            label2.Name = "label2";
            label2.Size = new Size(136, 25);
            label2.TabIndex = 3;
            label2.Text = "Category Name";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(41, 56);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(356, 31);
            textBox1.TabIndex = 0;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // addCategory
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(224, 224, 224);
            ClientSize = new Size(954, 650);
            Controls.Add(panel2);
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "addCategory";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "addCategory";
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
        private Panel panel2;
        private Panel panel1;
        private CheckBox checkBox2;
        private Label label3;
        private CheckBox checkBox1;
        private Button button2;
        private Button button1;
        private Label label2;
        private TextBox textBox1;
        private CheckBox checkBox3;
    }
}