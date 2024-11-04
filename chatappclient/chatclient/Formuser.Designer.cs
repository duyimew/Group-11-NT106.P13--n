namespace QLUSER
{
    partial class Formuser
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
            this.tb_username = new System.Windows.Forms.TextBox();
            this.lb_username = new System.Windows.Forms.Label();
            this.lb_user = new System.Windows.Forms.Label();
            this.bt_thoat = new System.Windows.Forms.Button();
            this.tb_email = new System.Windows.Forms.TextBox();
            this.lb_email = new System.Windows.Forms.Label();
            this.tb_HienTen = new System.Windows.Forms.TextBox();
            this.lb_Ten = new System.Windows.Forms.Label();
            this.tb_HIenNgSinh = new System.Windows.Forms.TextBox();
            this.lb_NgSinh = new System.Windows.Forms.Label();
            this.bt_dangxuat = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tb_username
            // 
            this.tb_username.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.tb_username.Location = new System.Drawing.Point(180, 129);
            this.tb_username.Name = "tb_username";
            this.tb_username.ReadOnly = true;
            this.tb_username.Size = new System.Drawing.Size(184, 23);
            this.tb_username.TabIndex = 0;
            // 
            // lb_username
            // 
            this.lb_username.AutoSize = true;
            this.lb_username.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lb_username.Location = new System.Drawing.Point(65, 136);
            this.lb_username.Name = "lb_username";
            this.lb_username.Size = new System.Drawing.Size(73, 17);
            this.lb_username.TabIndex = 1;
            this.lb_username.Text = "Username";
            // 
            // lb_user
            // 
            this.lb_user.AutoSize = true;
            this.lb_user.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F);
            this.lb_user.Location = new System.Drawing.Point(66, 36);
            this.lb_user.Name = "lb_user";
            this.lb_user.Size = new System.Drawing.Size(394, 46);
            this.lb_user.TabIndex = 2;
            this.lb_user.Text = "Thông tin người dùng";
            // 
            // bt_thoat
            // 
            this.bt_thoat.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.bt_thoat.Location = new System.Drawing.Point(74, 319);
            this.bt_thoat.Name = "bt_thoat";
            this.bt_thoat.Size = new System.Drawing.Size(95, 37);
            this.bt_thoat.TabIndex = 9;
            this.bt_thoat.Text = "Tắt";
            this.bt_thoat.UseVisualStyleBackColor = true;
            this.bt_thoat.Click += new System.EventHandler(this.bt_thoat_Click);
            // 
            // tb_email
            // 
            this.tb_email.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.tb_email.Location = new System.Drawing.Point(180, 168);
            this.tb_email.Name = "tb_email";
            this.tb_email.ReadOnly = true;
            this.tb_email.Size = new System.Drawing.Size(184, 23);
            this.tb_email.TabIndex = 0;
            // 
            // lb_email
            // 
            this.lb_email.AutoSize = true;
            this.lb_email.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lb_email.Location = new System.Drawing.Point(65, 175);
            this.lb_email.Name = "lb_email";
            this.lb_email.Size = new System.Drawing.Size(42, 17);
            this.lb_email.TabIndex = 1;
            this.lb_email.Text = "Email";
            // 
            // tb_HienTen
            // 
            this.tb_HienTen.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.tb_HienTen.Location = new System.Drawing.Point(180, 208);
            this.tb_HienTen.Name = "tb_HienTen";
            this.tb_HienTen.ReadOnly = true;
            this.tb_HienTen.Size = new System.Drawing.Size(184, 23);
            this.tb_HienTen.TabIndex = 0;
            // 
            // lb_Ten
            // 
            this.lb_Ten.AutoSize = true;
            this.lb_Ten.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lb_Ten.Location = new System.Drawing.Point(65, 215);
            this.lb_Ten.Name = "lb_Ten";
            this.lb_Ten.Size = new System.Drawing.Size(33, 17);
            this.lb_Ten.TabIndex = 1;
            this.lb_Ten.Text = "Tên";
            // 
            // tb_HIenNgSinh
            // 
            this.tb_HIenNgSinh.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.tb_HIenNgSinh.Location = new System.Drawing.Point(180, 249);
            this.tb_HIenNgSinh.Name = "tb_HIenNgSinh";
            this.tb_HIenNgSinh.ReadOnly = true;
            this.tb_HIenNgSinh.Size = new System.Drawing.Size(184, 23);
            this.tb_HIenNgSinh.TabIndex = 0;
            // 
            // lb_NgSinh
            // 
            this.lb_NgSinh.AutoSize = true;
            this.lb_NgSinh.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lb_NgSinh.Location = new System.Drawing.Point(65, 256);
            this.lb_NgSinh.Name = "lb_NgSinh";
            this.lb_NgSinh.Size = new System.Drawing.Size(71, 17);
            this.lb_NgSinh.TabIndex = 1;
            this.lb_NgSinh.Text = "Ngày sinh";
            // 
            // bt_dangxuat
            // 
            this.bt_dangxuat.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.bt_dangxuat.Location = new System.Drawing.Point(269, 319);
            this.bt_dangxuat.Name = "bt_dangxuat";
            this.bt_dangxuat.Size = new System.Drawing.Size(95, 37);
            this.bt_dangxuat.TabIndex = 10;
            this.bt_dangxuat.Text = "Đăng xuất";
            this.bt_dangxuat.UseVisualStyleBackColor = true;
            this.bt_dangxuat.Click += new System.EventHandler(this.bt_dangxuat_Click);
            // 
            // Formuser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 450);
            this.Controls.Add(this.bt_dangxuat);
            this.Controls.Add(this.bt_thoat);
            this.Controls.Add(this.lb_user);
            this.Controls.Add(this.lb_NgSinh);
            this.Controls.Add(this.tb_HIenNgSinh);
            this.Controls.Add(this.lb_Ten);
            this.Controls.Add(this.tb_HienTen);
            this.Controls.Add(this.lb_email);
            this.Controls.Add(this.tb_email);
            this.Controls.Add(this.lb_username);
            this.Controls.Add(this.tb_username);
            this.Name = "Formuser";
            this.Text = "Formuser";
            this.Load += new System.EventHandler(this.Formuser_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_username;
        private System.Windows.Forms.Label lb_username;
        private System.Windows.Forms.Label lb_user;
        private System.Windows.Forms.Button bt_thoat;
        private System.Windows.Forms.TextBox tb_email;
        private System.Windows.Forms.Label lb_email;
        private System.Windows.Forms.TextBox tb_HienTen;
        private System.Windows.Forms.Label lb_Ten;
        private System.Windows.Forms.TextBox tb_HIenNgSinh;
        private System.Windows.Forms.Label lb_NgSinh;
        private System.Windows.Forms.Button bt_dangxuat;
    }
}