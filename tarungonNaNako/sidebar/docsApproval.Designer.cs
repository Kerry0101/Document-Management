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
            panel1 = new Panel();
            dataGridView1 = new DataGridView();
            label8 = new Label();
            actionPendingApproval = new DataGridViewComboBoxColumn();
            fileName = new DataGridViewTextBoxColumn();
            submittedBy = new DataGridViewTextBoxColumn();
            dateUploaded = new DataGridViewTextBoxColumn();
            status = new DataGridViewTextBoxColumn();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BackColor = Color.White;
            panel1.Controls.Add(dataGridView1);
            panel1.Location = new Point(35, 55);
            panel1.Margin = new Padding(2);
            panel1.Name = "panel1";
            panel1.Size = new Size(890, 553);
            panel1.TabIndex = 3;
            // 
            // dataGridView1
            // 
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { actionPendingApproval, fileName, submittedBy, dateUploaded, status });
            dataGridView1.Location = new Point(33, 43);
            dataGridView1.Margin = new Padding(2);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.Size = new Size(826, 485);
            dataGridView1.TabIndex = 0;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Georgia", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label8.Location = new Point(35, 25);
            label8.Margin = new Padding(2, 0, 2, 0);
            label8.Name = "label8";
            label8.Size = new Size(195, 24);
            label8.TabIndex = 7;
            label8.Text = "Documents approval";
            // 
            // actionPendingApproval
            // 
            actionPendingApproval.HeaderText = "Action";
            actionPendingApproval.MinimumWidth = 8;
            actionPendingApproval.Name = "actionPendingApproval";
            actionPendingApproval.Width = 180;
            // 
            // fileName
            // 
            fileName.HeaderText = "File Name";
            fileName.MinimumWidth = 8;
            fileName.Name = "fileName";
            fileName.ReadOnly = true;
            fileName.Width = 180;
            // 
            // submittedBy
            // 
            submittedBy.HeaderText = "Submitted By";
            submittedBy.MinimumWidth = 8;
            submittedBy.Name = "submittedBy";
            submittedBy.ReadOnly = true;
            submittedBy.Width = 180;
            // 
            // dateUploaded
            // 
            dateUploaded.HeaderText = "Date";
            dateUploaded.MinimumWidth = 7;
            dateUploaded.Name = "dateUploaded";
            dateUploaded.ReadOnly = true;
            dateUploaded.Width = 180;
            // 
            // status
            // 
            status.HeaderText = "Status";
            status.MinimumWidth = 8;
            status.Name = "status";
            status.ReadOnly = true;
            status.Width = 180;
            // 
            // docsApproval
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(255, 255, 192);
            ClientSize = new Size(961, 646);
            Controls.Add(label8);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(2);
            Name = "docsApproval";
            Text = "docsApproval";
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Panel panel1;
        private DataGridView dataGridView1;
        private Label label8;
        private DataGridViewComboBoxColumn actionPendingApproval;
        private DataGridViewTextBoxColumn fileName;
        private DataGridViewTextBoxColumn submittedBy;
        private DataGridViewTextBoxColumn dateUploaded;
        private DataGridViewTextBoxColumn status;
    }
}