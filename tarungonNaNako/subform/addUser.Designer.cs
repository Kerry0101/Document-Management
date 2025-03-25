namespace tarungonNaNako.subform
{
    partial class addUser
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
            button1 = new Button();
            panel1 = new Panel();
            label7 = new Label();
            textBox5 = new TextBox();
            button2 = new Button();
            comboBox1 = new ComboBox();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            textBox4 = new TextBox();
            textBox3 = new TextBox();
            textBox2 = new TextBox();
            textBox1 = new TextBox();
            label3 = new Label();
            label2 = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(31, 33);
            label1.Name = "label1";
            label1.Size = new Size(192, 32);
            label1.TabIndex = 2;
            label1.Text = "MANAGE USERS";
            label1.Click += label1_Click;
            // 
            // button1
            // 
            button1.Location = new Point(168, 425);
            button1.Name = "button1";
            button1.Size = new Size(145, 34);
            button1.TabIndex = 4;
            button1.Text = "Cancel";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // panel1
            // 
            panel1.BackColor = Color.WhiteSmoke;
            panel1.Controls.Add(label7);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(textBox5);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(comboBox1);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(textBox4);
            panel1.Controls.Add(textBox3);
            panel1.Controls.Add(textBox2);
            panel1.Controls.Add(textBox1);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Location = new Point(31, 80);
            panel1.Name = "panel1";
            panel1.Size = new Size(893, 547);
            panel1.TabIndex = 5;
            panel1.Paint += panel1_Paint_1;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(466, 257);
            label7.Name = "label7";
            label7.Size = new Size(87, 25);
            label7.TabIndex = 13;
            label7.Text = "Password";
            // 
            // textBox5
            // 
            textBox5.Location = new Point(466, 285);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(350, 31);
            textBox5.TabIndex = 12;
            // 
            // button2
            // 
            button2.Location = new Point(25, 425);
            button2.Name = "button2";
            button2.Size = new Size(137, 34);
            button2.TabIndex = 11;
            button2.Text = "Save";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(25, 285);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(182, 33);
            comboBox1.TabIndex = 10;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged_1;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(25, 257);
            label6.Name = "label6";
            label6.Size = new Size(46, 25);
            label6.TabIndex = 9;
            label6.Text = "Role";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(466, 138);
            label5.Name = "label5";
            label5.Size = new Size(91, 25);
            label5.TabIndex = 7;
            label5.Text = "Username";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(25, 138);
            label4.Name = "label4";
            label4.Size = new Size(137, 25);
            label4.TabIndex = 6;
            label4.Text = "Mobile Number";
            // 
            // textBox4
            // 
            textBox4.Location = new Point(466, 166);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(350, 31);
            textBox4.TabIndex = 5;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(25, 166);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(306, 31);
            textBox3.TabIndex = 4;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(466, 54);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(350, 31);
            textBox2.TabIndex = 3;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(25, 54);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(306, 31);
            textBox1.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(466, 26);
            label3.Name = "label3";
            label3.Size = new Size(95, 25);
            label3.TabIndex = 1;
            label3.Text = "Last Name";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(25, 26);
            label2.Name = "label2";
            label2.Size = new Size(97, 25);
            label2.TabIndex = 0;
            label2.Text = "First Name";
            // 
            // addUser
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(224, 224, 224);
            ClientSize = new Size(954, 650);
            Controls.Add(panel1);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "addUser";
            Text = " ";
            Load += addUser_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        private Button button1;
        private Panel panel1;
        private TextBox textBox4;
        private TextBox textBox3;
        private TextBox textBox2;
        private TextBox textBox1;
        private Label label3;
        private Label label2;
        private Button button2;
        private ComboBox comboBox1;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label7;
        private TextBox textBox5;
    }
}