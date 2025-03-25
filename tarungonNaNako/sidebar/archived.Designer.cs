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
            panel1 = new Panel();
            panel3 = new Panel();
            label2 = new Label();
            dataGridView1 = new DataGridView();
            userId = new DataGridViewTextBoxColumn();
            name = new DataGridViewTextBoxColumn();
            role = new DataGridViewTextBoxColumn();
            unarchive = new DataGridViewButtonColumn();
            Delete = new DataGridViewButtonColumn();
            label1 = new Label();
            panel2 = new Panel();
            dataGridView2 = new DataGridView();
            panel4 = new Panel();
            label3 = new Label();
            fileName = new DataGridViewTextBoxColumn();
            category = new DataGridViewTextBoxColumn();
            unarchive2 = new DataGridViewButtonColumn();
            delete2 = new DataGridViewButtonColumn();
            fileId = new DataGridViewTextBoxColumn();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            panel4.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.BackColor = Color.White;
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(dataGridView1);
            panel1.Location = new Point(33, 70);
            panel1.Name = "panel1";
            panel1.Size = new Size(888, 415);
            panel1.TabIndex = 6;
            panel1.Paint += panel1_Paint;
            // 
            // panel3
            // 
            panel3.BackColor = Color.Black;
            panel3.Controls.Add(label2);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(888, 34);
            panel3.TabIndex = 2;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.ForeColor = SystemColors.ButtonHighlight;
            label2.Location = new Point(415, 0);
            label2.Name = "label2";
            label2.Size = new Size(55, 25);
            label2.TabIndex = 2;
            label2.Text = "Users";
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { userId, name, role, unarchive, Delete });
            dataGridView1.Location = new Point(44, 40);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.Size = new Size(800, 344);
            dataGridView1.TabIndex = 1;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // userId
            // 
            userId.HeaderText = "";
            userId.MinimumWidth = 8;
            userId.Name = "userId";
            userId.Visible = false;
            userId.Width = 150;
            // 
            // name
            // 
            name.FillWeight = 179.381561F;
            name.HeaderText = "Name";
            name.MinimumWidth = 20;
            name.Name = "name";
            name.Width = 359;
            // 
            // role
            // 
            role.FillWeight = 120.3234F;
            role.HeaderText = "Role";
            role.MinimumWidth = 8;
            role.Name = "role";
            role.Width = 200;
            // 
            // unarchive
            // 
            unarchive.FillWeight = 54.84052F;
            unarchive.HeaderText = "";
            unarchive.MinimumWidth = 8;
            unarchive.Name = "unarchive";
            unarchive.Text = "UNARCHIVE";
            unarchive.Width = 110;
            // 
            // Delete
            // 
            Delete.HeaderText = "";
            Delete.MinimumWidth = 8;
            Delete.Name = "Delete";
            Delete.Resizable = DataGridViewTriState.True;
            Delete.SortMode = DataGridViewColumnSortMode.Automatic;
            Delete.ToolTipText = "DELETE";
            Delete.Width = 110;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            label1.Location = new Point(33, 32);
            label1.Name = "label1";
            label1.Size = new Size(129, 32);
            label1.TabIndex = 4;
            label1.Text = "ARCHIVED";
            // 
            // panel2
            // 
            panel2.AutoScroll = true;
            panel2.BackColor = Color.White;
            panel2.Controls.Add(dataGridView2);
            panel2.Controls.Add(panel4);
            panel2.Location = new Point(33, 510);
            panel2.Name = "panel2";
            panel2.Size = new Size(888, 415);
            panel2.TabIndex = 7;
            // 
            // dataGridView2
            // 
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.BackgroundColor = Color.White;
            dataGridView2.BorderStyle = BorderStyle.None;
            dataGridView2.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridView2.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Columns.AddRange(new DataGridViewColumn[] { fileName, category, unarchive2, delete2, fileId });
            dataGridView2.Location = new Point(44, 35);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.RowHeadersWidth = 62;
            dataGridView2.Size = new Size(800, 344);
            dataGridView2.TabIndex = 4;
            dataGridView2.CellContentClick += dataGridView2_CellContentClick;
            // 
            // panel4
            // 
            panel4.BackColor = Color.Black;
            panel4.Controls.Add(label3);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(0, 0);
            panel4.Name = "panel4";
            panel4.Size = new Size(888, 34);
            panel4.TabIndex = 3;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.ForeColor = SystemColors.ButtonHighlight;
            label3.Location = new Point(377, 0);
            label3.Name = "label3";
            label3.Size = new Size(103, 25);
            label3.TabIndex = 2;
            label3.Text = "Documents";
            label3.TextAlign = ContentAlignment.BottomRight;
            // 
            // fileName
            // 
            fileName.FillWeight = 179.381561F;
            fileName.HeaderText = "File Name";
            fileName.MinimumWidth = 20;
            fileName.Name = "fileName";
            fileName.Width = 359;
            // 
            // category
            // 
            category.FillWeight = 120.3234F;
            category.HeaderText = "Category";
            category.MinimumWidth = 8;
            category.Name = "category";
            category.Width = 200;
            // 
            // unarchive2
            // 
            unarchive2.FillWeight = 54.84052F;
            unarchive2.HeaderText = "";
            unarchive2.MinimumWidth = 8;
            unarchive2.Name = "unarchive2";
            unarchive2.Text = "UNARCHIVE";
            unarchive2.Width = 110;
            // 
            // delete2
            // 
            delete2.HeaderText = "";
            delete2.MinimumWidth = 8;
            delete2.Name = "delete2";
            delete2.Resizable = DataGridViewTriState.True;
            delete2.SortMode = DataGridViewColumnSortMode.Automatic;
            delete2.ToolTipText = "DELETE";
            delete2.Width = 110;
            // 
            // fileId
            // 
            fileId.HeaderText = "";
            fileId.MinimumWidth = 8;
            fileId.Name = "fileId";
            fileId.Visible = false;
            fileId.Width = 150;
            // 
            // archived
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(954, 650);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "archived";
            Text = "archived";
            Load += archived_Load;
            panel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private DataGridView dataGridView1;
        private Label label1;
        private Panel panel3;
        private Label label2;
        private Panel panel2;
        private Panel panel4;
        private Label label3;
        private DataGridViewTextBoxColumn userId;
        private DataGridViewTextBoxColumn name;
        private DataGridViewTextBoxColumn role;
        private DataGridViewButtonColumn unarchive;
        private DataGridViewButtonColumn Delete;
        private DataGridView dataGridView2;
        private DataGridViewTextBoxColumn fileName;
        private DataGridViewTextBoxColumn category;
        private DataGridViewButtonColumn unarchive2;
        private DataGridViewButtonColumn delete2;
        private DataGridViewTextBoxColumn fileId;
    }
}