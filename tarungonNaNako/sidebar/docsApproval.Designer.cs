namespace tarungonNaNako.sidebar
{
    partial class docsApproval
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
            panel1 = new Panel();
            dataGridView1 = new DataGridView();
            actionPendingApproval = new DataGridViewComboBoxColumn();
            fileName = new DataGridViewTextBoxColumn();
            submittedBy = new DataGridViewTextBoxColumn();
            dateUploaded = new DataGridViewTextBoxColumn();
            category = new DataGridViewTextBoxColumn();
            subCategory = new DataGridViewTextBoxColumn();
            status = new DataGridViewTextBoxColumn();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            label1.Location = new Point(31, 34);
            label1.Name = "label1";
            label1.Size = new Size(270, 32);
            label1.TabIndex = 2;
            label1.Text = "APPROVE DOCUMENTS";
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BackColor = Color.White;
            panel1.Controls.Add(dataGridView1);
            panel1.Location = new Point(44, 69);
            panel1.Name = "panel1";
            panel1.Size = new Size(865, 534);
            panel1.TabIndex = 3;
            // 
            // dataGridView1
            // 
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { actionPendingApproval, fileName, submittedBy, dateUploaded, category, subCategory, status });
            dataGridView1.Location = new Point(41, 54);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.Size = new Size(785, 449);
            dataGridView1.TabIndex = 0;
            // 
            // actionPendingApproval
            // 
            actionPendingApproval.HeaderText = "Action";
            actionPendingApproval.MinimumWidth = 8;
            actionPendingApproval.Name = "actionPendingApproval";
            actionPendingApproval.Width = 150;
            // 
            // fileName
            // 
            fileName.HeaderText = "File Name";
            fileName.MinimumWidth = 8;
            fileName.Name = "fileName";
            fileName.Width = 150;
            // 
            // submittedBy
            // 
            submittedBy.HeaderText = "Submitted By";
            submittedBy.MinimumWidth = 8;
            submittedBy.Name = "submittedBy";
            submittedBy.Width = 150;
            // 
            // dateUploaded
            // 
            dateUploaded.HeaderText = "Date";
            dateUploaded.MinimumWidth = 7;
            dateUploaded.Name = "dateUploaded";
            dateUploaded.Width = 150;
            // 
            // category
            // 
            category.HeaderText = "Category";
            category.MinimumWidth = 8;
            category.Name = "category";
            category.Width = 150;
            // 
            // subCategory
            // 
            subCategory.HeaderText = "SubCategory";
            subCategory.MinimumWidth = 8;
            subCategory.Name = "subCategory";
            subCategory.Width = 150;
            // 
            // status
            // 
            status.HeaderText = "Status";
            status.MinimumWidth = 8;
            status.Name = "status";
            status.Width = 150;
            // 
            // docsApproval
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(224, 224, 224);
            ClientSize = new Size(954, 650);
            Controls.Add(panel1);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "docsApproval";
            Text = "docsApproval";
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Panel panel1;
        private DataGridView dataGridView1;
        private DataGridViewComboBoxColumn actionPendingApproval;
        private DataGridViewTextBoxColumn fileName;
        private DataGridViewTextBoxColumn submittedBy;
        private DataGridViewTextBoxColumn dateUploaded;
        private DataGridViewTextBoxColumn category;
        private DataGridViewTextBoxColumn subCategory;
        private DataGridViewTextBoxColumn status;
    }
}