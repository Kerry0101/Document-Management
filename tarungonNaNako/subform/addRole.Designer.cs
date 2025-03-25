namespace tarungonNaNako.subform
{
    partial class addRole
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
            button2 = new Button();
            dataGridView1 = new DataGridView();
            role = new DataGridViewTextBoxColumn();
            editRole = new DataGridViewButtonColumn();
            archiveRole = new DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            label1.Location = new Point(27, 23);
            label1.Name = "label1";
            label1.Size = new Size(71, 32);
            label1.TabIndex = 0;
            label1.Text = "ROLE";
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button2.Location = new Point(709, 23);
            button2.Name = "button2";
            button2.Size = new Size(217, 32);
            button2.TabIndex = 1;
            button2.Text = "Add Role";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { role, editRole, archiveRole });
            dataGridView1.Location = new Point(27, 76);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.Size = new Size(899, 546);
            dataGridView1.TabIndex = 2;
            // 
            // role
            // 
            role.FillWeight = 210.905853F;
            role.HeaderText = "Role";
            role.MinimumWidth = 8;
            role.Name = "role";
            // 
            // editRole
            // 
            editRole.FillWeight = 46.4805069F;
            editRole.HeaderText = "Edit";
            editRole.MinimumWidth = 8;
            editRole.Name = "editRole";
            editRole.Text = "EDIT";
            // 
            // archiveRole
            // 
            archiveRole.FillWeight = 42.6136322F;
            archiveRole.HeaderText = "Archive";
            archiveRole.MinimumWidth = 8;
            archiveRole.Name = "archiveRole";
            archiveRole.Text = "ARCHIVE";
            // 
            // addRole
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(224, 224, 224);
            ClientSize = new Size(954, 650);
            Controls.Add(dataGridView1);
            Controls.Add(button2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "addRole";
            Text = "addRole";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        private Button button2;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn role;
        private DataGridViewButtonColumn editRole;
        private DataGridViewButtonColumn archiveRole;
    }
}