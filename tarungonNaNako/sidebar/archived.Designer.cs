namespace tarungonNaNako.sidebar
{
    partial class archived
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            panel1 = new Panel();
            dataGridView1 = new DataGridView();
            categoryName = new DataGridViewTextBoxColumn();
            Unarchive = new DataGridViewButtonColumn();
            panel3 = new Panel();
            label2 = new Label();
            dataGridView2 = new DataGridView();
            fileName = new DataGridViewTextBoxColumn();
            category = new DataGridViewTextBoxColumn();
            unarchive2 = new DataGridViewButtonColumn();
            label1 = new Label();
            panel2 = new Panel();
            panel4 = new Panel();
            label3 = new Label();
            panel5 = new Panel();
            dataGridView3 = new DataGridView();
            userId = new DataGridViewTextBoxColumn();
            name = new DataGridViewTextBoxColumn();
            role = new DataGridViewTextBoxColumn();
            unarchive1 = new DataGridViewButtonColumn();
            Delete = new DataGridViewButtonColumn();
            panel6 = new Panel();
            label4 = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            panel2.SuspendLayout();
            panel4.SuspendLayout();
            panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView3).BeginInit();
            panel6.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.BackColor = Color.White;
            panel1.Controls.Add(dataGridView1);
            panel1.Controls.Add(panel3);
            panel1.Location = new Point(107, 50);
            panel1.Margin = new Padding(2);
            panel1.Name = "panel1";
            panel1.Size = new Size(710, 332);
            panel1.TabIndex = 6;
            panel1.Paint += panel1_Paint;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { categoryName, Unarchive });
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 27);
            dataGridView1.Margin = new Padding(4, 5, 4, 5);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(710, 305);
            dataGridView1.TabIndex = 95;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // categoryName
            // 
            categoryName.HeaderText = "Folder name";
            categoryName.MinimumWidth = 20;
            categoryName.Name = "categoryName";
            categoryName.ReadOnly = true;
            categoryName.Width = 590;
            // 
            // Unarchive
            // 
            Unarchive.HeaderText = "Actions";
            Unarchive.MinimumWidth = 6;
            Unarchive.Name = "Unarchive";
            Unarchive.Width = 117;
            // 
            // panel3
            // 
            panel3.BackColor = Color.Black;
            panel3.Controls.Add(label2);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 0);
            panel3.Margin = new Padding(2);
            panel3.Name = "panel3";
            panel3.Size = new Size(710, 27);
            panel3.TabIndex = 2;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.ForeColor = SystemColors.ButtonHighlight;
            label2.Location = new Point(294, 0);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(119, 20);
            label2.TabIndex = 2;
            label2.Text = "Archived Folders";
            // 
            // dataGridView2
            // 
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.BackgroundColor = Color.White;
            dataGridView2.BorderStyle = BorderStyle.None;
            dataGridView2.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridView2.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Columns.AddRange(new DataGridViewColumn[] { fileName, category, unarchive2 });
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dataGridView2.DefaultCellStyle = dataGridViewCellStyle1;
            dataGridView2.Dock = DockStyle.Fill;
            dataGridView2.Location = new Point(0, 27);
            dataGridView2.Margin = new Padding(2);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.RowHeadersWidth = 62;
            dataGridView2.Size = new Size(710, 305);
            dataGridView2.TabIndex = 4;
            dataGridView2.CellContentClick += dataGridView2_CellContentClick;
            // 
            // fileName
            // 
            fileName.FillWeight = 179.381561F;
            fileName.HeaderText = "File Name";
            fileName.MinimumWidth = 20;
            fileName.Name = "fileName";
            fileName.ReadOnly = true;
            fileName.Width = 357;
            // 
            // category
            // 
            category.FillWeight = 120.3234F;
            category.HeaderText = "Location";
            category.MinimumWidth = 8;
            category.Name = "category";
            category.ReadOnly = true;
            category.Width = 234;
            // 
            // unarchive2
            // 
            unarchive2.FillWeight = 54.84052F;
            unarchive2.HeaderText = "Action";
            unarchive2.MinimumWidth = 8;
            unarchive2.Name = "unarchive2";
            unarchive2.Text = "UNARCHIVE";
            unarchive2.Width = 117;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Georgia", 12F);
            label1.Location = new Point(110, 17);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(158, 24);
            label1.TabIndex = 4;
            label1.Text = "Archived section";
            // 
            // panel2
            // 
            panel2.AutoScroll = true;
            panel2.BackColor = Color.White;
            panel2.Controls.Add(dataGridView2);
            panel2.Controls.Add(panel4);
            panel2.Location = new Point(107, 402);
            panel2.Margin = new Padding(2);
            panel2.Name = "panel2";
            panel2.Size = new Size(710, 332);
            panel2.TabIndex = 7;
            // 
            // panel4
            // 
            panel4.BackColor = Color.Black;
            panel4.Controls.Add(label3);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(0, 0);
            panel4.Margin = new Padding(2);
            panel4.Name = "panel4";
            panel4.Size = new Size(710, 27);
            panel4.TabIndex = 3;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.ForeColor = SystemColors.ButtonHighlight;
            label3.Location = new Point(294, 0);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(146, 20);
            label3.TabIndex = 2;
            label3.Text = "Archived Documents";
            label3.TextAlign = ContentAlignment.BottomRight;
            // 
            // panel5
            // 
            panel5.AutoScroll = true;
            panel5.BackColor = Color.White;
            panel5.Controls.Add(dataGridView3);
            panel5.Controls.Add(panel6);
            panel5.Location = new Point(107, 756);
            panel5.Margin = new Padding(2);
            panel5.Name = "panel5";
            panel5.Size = new Size(710, 332);
            panel5.TabIndex = 8;
            // 
            // dataGridView3
            // 
            dataGridView3.AllowUserToAddRows = false;
            dataGridView3.BackgroundColor = Color.White;
            dataGridView3.BorderStyle = BorderStyle.None;
            dataGridView3.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridView3.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView3.Columns.AddRange(new DataGridViewColumn[] { userId, name, role, unarchive1, Delete });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dataGridView3.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridView3.Dock = DockStyle.Fill;
            dataGridView3.Location = new Point(0, 27);
            dataGridView3.Margin = new Padding(2);
            dataGridView3.Name = "dataGridView3";
            dataGridView3.RowHeadersVisible = false;
            dataGridView3.RowHeadersWidth = 62;
            dataGridView3.Size = new Size(710, 305);
            dataGridView3.TabIndex = 4;
            dataGridView3.CellContentClick += dataGridView3_CellContentClick;
            // 
            // userId
            // 
            userId.HeaderText = "";
            userId.MinimumWidth = 8;
            userId.Name = "userId";
            userId.ReadOnly = true;
            userId.Visible = false;
            userId.Width = 150;
            // 
            // name
            // 
            name.FillWeight = 179.381561F;
            name.HeaderText = "Name";
            name.MinimumWidth = 20;
            name.Name = "name";
            name.ReadOnly = true;
            name.Width = 359;
            // 
            // role
            // 
            role.FillWeight = 120.3234F;
            role.HeaderText = "Role";
            role.MinimumWidth = 8;
            role.Name = "role";
            role.ReadOnly = true;
            role.Width = 200;
            // 
            // unarchive1
            // 
            unarchive1.FillWeight = 54.84052F;
            unarchive1.HeaderText = "";
            unarchive1.MinimumWidth = 8;
            unarchive1.Name = "unarchive1";
            unarchive1.Text = "UNARCHIVE";
            unarchive1.Width = 110;
            // 
            // Delete
            // 
            Delete.HeaderText = "";
            Delete.MinimumWidth = 8;
            Delete.Name = "Delete";
            Delete.SortMode = DataGridViewColumnSortMode.Automatic;
            Delete.ToolTipText = "DELETE";
            Delete.Width = 110;
            // 
            // panel6
            // 
            panel6.BackColor = Color.Black;
            panel6.Controls.Add(label4);
            panel6.Dock = DockStyle.Top;
            panel6.Location = new Point(0, 0);
            panel6.Margin = new Padding(2);
            panel6.Name = "panel6";
            panel6.Size = new Size(710, 27);
            panel6.TabIndex = 3;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.ForeColor = SystemColors.ButtonHighlight;
            label4.Location = new Point(294, 0);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(106, 20);
            label4.TabIndex = 2;
            label4.Text = "Archived Users";
            label4.TextAlign = ContentAlignment.BottomRight;
            // 
            // archived
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            AutoScrollMargin = new Size(0, 50);
            BackColor = Color.FromArgb(255, 255, 192);
            ClientSize = new Size(961, 646);
            Controls.Add(panel5);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(2);
            Name = "archived";
            Text = "archived";
            Load += archived_Load;
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            panel2.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView3).EndInit();
            panel6.ResumeLayout(false);
            panel6.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private Panel panel3;
        private Label label2;
        private Panel panel2;
        private Panel panel4;
        private Label label3;
        private DataGridView dataGridView2;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn fileName;
        private DataGridViewTextBoxColumn category;
        private DataGridViewButtonColumn unarchive2;
        private DataGridViewTextBoxColumn categoryName;
        private DataGridViewButtonColumn Unarchive;
        private Panel panel5;
        private DataGridView dataGridView3;
        private Panel panel6;
        private Label label4;
        private DataGridViewTextBoxColumn userId;
        private DataGridViewTextBoxColumn name;
        private DataGridViewTextBoxColumn role;
        private DataGridViewButtonColumn unarchive1;
        private DataGridViewButtonColumn Delete;
    }
}