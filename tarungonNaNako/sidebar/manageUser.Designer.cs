namespace tarungonNaNako.sidebar
{
    partial class manageUser
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
            dataGridView1 = new DataGridView();
            userId = new DataGridViewTextBoxColumn();
            name = new DataGridViewTextBoxColumn();
            role = new DataGridViewTextBoxColumn();
            archiveUser = new DataGridViewButtonColumn();
            editUser = new DataGridViewButtonColumn();
            button1 = new Button();
            panel1 = new Panel();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            label1.Location = new Point(39, 35);
            label1.Name = "label1";
            label1.Size = new Size(84, 32);
            label1.TabIndex = 0;
            label1.Text = "USERS";
            label1.Click += label1_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { userId, name, role, archiveUser, editUser });
            dataGridView1.Location = new Point(44, 40);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.Size = new Size(800, 481);
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
            role.Width = 240;
            // 
            // archiveUser
            // 
            archiveUser.FillWeight = 45.4545441F;
            archiveUser.HeaderText = "Archive";
            archiveUser.MinimumWidth = 8;
            archiveUser.Name = "archiveUser";
            archiveUser.Text = "Archive";
            archiveUser.Width = 91;
            // 
            // editUser
            // 
            editUser.FillWeight = 54.84052F;
            editUser.HeaderText = "Edit";
            editUser.MinimumWidth = 8;
            editUser.Name = "editUser";
            editUser.Text = "EDIT";
            editUser.Width = 110;
            // 
            // button1
            // 
            button1.Location = new Point(740, 33);
            button1.Name = "button1";
            button1.Size = new Size(187, 34);
            button1.TabIndex = 2;
            button1.Text = "Add User";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(dataGridView1);
            panel1.Location = new Point(39, 73);
            panel1.Name = "panel1";
            panel1.Size = new Size(888, 551);
            panel1.TabIndex = 3;
            // 
            // manageUser
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(224, 224, 224);
            ClientSize = new Size(954, 650);
            Controls.Add(panel1);
            Controls.Add(button1);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "manageUser";
            Text = "manageUser";
            Load += manageUser_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private DataGridView dataGridView1;
        private Button button1;
        private Panel panel1;
        private DataGridViewTextBoxColumn userId;
        private DataGridViewTextBoxColumn name;
        private DataGridViewTextBoxColumn role;
        private DataGridViewButtonColumn archiveUser;
        private DataGridViewButtonColumn editUser;
    }
}