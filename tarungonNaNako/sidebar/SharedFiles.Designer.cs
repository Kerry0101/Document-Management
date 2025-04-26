namespace tarungonNaNako.sidebar
{
    partial class SharedFiles
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
            panel5 = new Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            label4 = new Label();
            label6 = new Label();
            label5 = new Label();
            label7 = new Label();
            label1 = new Label();
            panel5.SuspendLayout();
            SuspendLayout();
            // 
            // panel5
            // 
            panel5.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel5.AutoScroll = true;
            panel5.BackColor = Color.FromArgb(255, 207, 64);
            panel5.Controls.Add(tableLayoutPanel1);
            panel5.Controls.Add(label4);
            panel5.Controls.Add(label6);
            panel5.Controls.Add(label5);
            panel5.Controls.Add(label7);
            panel5.Location = new Point(16, 71);
            panel5.Name = "panel5";
            panel5.Padding = new Padding(0, 0, 0, 100);
            panel5.Size = new Size(917, 548);
            panel5.TabIndex = 21;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 56.7615662F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Cursor = Cursors.Hand;
            tableLayoutPanel1.Location = new Point(8, 33);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(1161, 0);
            tableLayoutPanel1.TabIndex = 11;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(803, 9);
            label4.Name = "label4";
            label4.Size = new Size(62, 20);
            label4.TabIndex = 10;
            label4.Text = "Action";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(444, 9);
            label6.Name = "label6";
            label6.Size = new Size(81, 20);
            label6.TabIndex = 8;
            label6.Text = "Location";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(265, 9);
            label5.Name = "label5";
            label5.Size = new Size(118, 20);
            label5.TabIndex = 7;
            label5.Text = "Date created";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(13, 9);
            label7.Name = "label7";
            label7.Size = new Size(91, 20);
            label7.TabIndex = 6;
            label7.Text = "File name";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Georgia", 12F);
            label1.Location = new Point(19, 37);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(115, 24);
            label1.TabIndex = 22;
            label1.Text = "Shared files";
            // 
            // SharedFiles
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(255, 255, 192);
            ClientSize = new Size(961, 646);
            Controls.Add(label1);
            Controls.Add(panel5);
            FormBorderStyle = FormBorderStyle.None;
            Name = "SharedFiles";
            Text = "SharedFiles";
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel5;
        private TableLayoutPanel tableLayoutPanel1;
        private Label label4;
        private Label label6;
        private Label label5;
        private Label label7;
        private Label label1;
    }
}