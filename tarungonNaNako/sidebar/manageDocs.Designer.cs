namespace tarungonNaNako.sidebar
{
    partial class manageDocs
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
            label1 = new Label();
            panel1 = new Panel();
            comboBox1 = new ComboBox();
            label3 = new Label();
            textBox1 = new TextBox();
            label2 = new Label();
            dataGridView1 = new DataGridView();
            fileId = new DataGridViewTextBoxColumn();
            action = new DataGridViewComboBoxColumn();
            docuName = new DataGridViewLinkColumn();
            category = new DataGridViewTextBoxColumn();
            subCategory = new DataGridViewTextBoxColumn();
            dateCreated = new DataGridViewTextBoxColumn();
            createdBy = new DataGridViewTextBoxColumn();
            button1 = new Button();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            label1.Location = new Point(22, 37);
            label1.Name = "label1";
            label1.Size = new Size(267, 32);
            label1.TabIndex = 0;
            label1.Text = "MANAGE DOCUMENTS";
            label1.Click += label1_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(comboBox1);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(textBox1);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(dataGridView1);
            panel1.Location = new Point(22, 93);
            panel1.Margin = new Padding(2);
            panel1.Name = "panel1";
            panel1.Size = new Size(899, 527);
            panel1.TabIndex = 1;
            panel1.Paint += panel1_Paint;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(690, 44);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(182, 33);
            comboBox1.TabIndex = 5;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(690, 16);
            label3.Name = "label3";
            label3.Size = new Size(135, 25);
            label3.TabIndex = 3;
            label3.Text = "Select Category";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(26, 44);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(549, 31);
            textBox1.TabIndex = 2;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(26, 16);
            label2.Name = "label2";
            label2.Size = new Size(138, 25);
            label2.TabIndex = 1;
            label2.Text = "Search by name";
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { fileId, action, docuName, category, subCategory, dateCreated, createdBy });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridView1.Location = new Point(26, 79);
            dataGridView1.Margin = new Padding(2);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.Size = new Size(845, 424);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged_1;
            // 
            // fileId
            // 
            fileId.HeaderText = "";
            fileId.MinimumWidth = 8;
            fileId.Name = "fileId";
            fileId.Visible = false;
            fileId.Width = 150;
            // 
            // action
            // 
            action.FillWeight = 106.508865F;
            action.HeaderText = "Action";
            action.MinimumWidth = 8;
            action.Name = "action";
            action.Width = 150;
            // 
            // docuName
            // 
            docuName.FillWeight = 97.1848F;
            docuName.HeaderText = "File Name";
            docuName.MinimumWidth = 8;
            docuName.Name = "docuName";
            docuName.Width = 137;
            // 
            // category
            // 
            category.FillWeight = 91.3956F;
            category.HeaderText = "Category";
            category.MinimumWidth = 8;
            category.Name = "category";
            category.Width = 129;
            // 
            // subCategory
            // 
            subCategory.FillWeight = 92.89742F;
            subCategory.HeaderText = "SubCategory";
            subCategory.MinimumWidth = 8;
            subCategory.Name = "subCategory";
            subCategory.Width = 130;
            // 
            // dateCreated
            // 
            dateCreated.FillWeight = 99.87594F;
            dateCreated.HeaderText = "Date Created";
            dateCreated.MinimumWidth = 8;
            dateCreated.Name = "dateCreated";
            dateCreated.Width = 141;
            // 
            // createdBy
            // 
            createdBy.FillWeight = 112.137321F;
            createdBy.HeaderText = "Created By";
            createdBy.MinimumWidth = 8;
            createdBy.Name = "createdBy";
            createdBy.Width = 158;
            // 
            // button1
            // 
            button1.Location = new Point(740, 37);
            button1.Name = "button1";
            button1.Size = new Size(181, 34);
            button1.TabIndex = 2;
            button1.Text = "Add Document";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // manageDocs
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(224, 224, 224);
            ClientSize = new Size(954, 650);
            Controls.Add(button1);
            Controls.Add(panel1);
            Controls.Add(label1);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.None;
            Name = "manageDocs";
            Text = "manageDocs";
            Load += manageDocs_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Panel panel1;
        private DataGridView dataGridView1;
        private Button button1;
        private ComboBox comboBox1;
        private Label label3;
        private TextBox textBox1;
        private Label label2;
        private DataGridViewTextBoxColumn fileId;
        private DataGridViewComboBoxColumn action;
        private DataGridViewLinkColumn docuName;
        private DataGridViewTextBoxColumn category;
        private DataGridViewTextBoxColumn subCategory;
        private DataGridViewTextBoxColumn dateCreated;
        private DataGridViewTextBoxColumn createdBy;
    }
}