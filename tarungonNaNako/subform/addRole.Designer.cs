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
            button2 = new Button();
            dataGridView1 = new DataGridView();
            role = new DataGridViewTextBoxColumn();
            editRole = new DataGridViewButtonColumn();
            archiveRole = new DataGridViewButtonColumn();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button2.Location = new Point(780, 37);
            button2.Margin = new Padding(2, 2, 2, 2);
            button2.Name = "button2";
            button2.Size = new Size(150, 27);
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
            dataGridView1.Location = new Point(16, 71);
            dataGridView1.Margin = new Padding(2, 2, 2, 2);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.Size = new Size(917, 548);
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
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Georgia", 12F);
            label1.Location = new Point(19, 37);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(60, 24);
            label1.TabIndex = 5;
            label1.Text = "Roles";
            // 
            // addRole
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(255, 255, 192);
            ClientSize = new Size(961, 646);
            Controls.Add(label1);
            Controls.Add(dataGridView1);
            Controls.Add(button2);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(2, 2, 2, 2);
            Name = "addRole";
            Text = "addRole";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button button2;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn role;
        private DataGridViewButtonColumn editRole;
        private DataGridViewButtonColumn archiveRole;
        private Label label1;
    }
}