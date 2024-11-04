namespace QLUSER
{
    partial class Dangky
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
            this.lb_username = new System.Windows.Forms.Label();
            this.lb_pwd = new System.Windows.Forms.Label();
            this.lb_cfpwd = new System.Windows.Forms.Label();
            this.lb_email = new System.Windows.Forms.Label();
            this.tb_username = new System.Windows.Forms.TextBox();
            this.tb_pwd = new System.Windows.Forms.TextBox();
            this.tb_cfpwd = new System.Windows.Forms.TextBox();
            this.tb_email = new System.Windows.Forms.TextBox();
            this.bt_Dk = new System.Windows.Forms.Button();
            this.lb_DK = new System.Windows.Forms.Label();
            this.bt_thoat = new System.Windows.Forms.Button();
            this.tb_hoten = new System.Windows.Forms.TextBox();
            this.tb_ngsinh = new System.Windows.Forms.TextBox();
            this.lb_ten = new System.Windows.Forms.Label();
            this.lb_ngaysinh = new System.Windows.Forms.Label();
            this.lb_chuthich = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lb_username
            // 
            this.lb_username.AutoSize = true;
            this.lb_username.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lb_username.Location = new System.Drawing.Point(61, 95);
            this.lb_username.Name = "lb_username";
            this.lb_username.Size = new System.Drawing.Size(105, 17);
            this.lb_username.TabIndex = 0;
            this.lb_username.Text = "Tên đăng nhập";
            // 
            // lb_pwd
            // 
            this.lb_pwd.AutoSize = true;
            this.lb_pwd.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lb_pwd.Location = new System.Drawing.Point(61, 137);
            this.lb_pwd.Name = "lb_pwd";
            this.lb_pwd.Size = new System.Drawing.Size(66, 17);
            this.lb_pwd.TabIndex = 1;
            this.lb_pwd.Text = "Mật khẩu";
            // 
            // lb_cfpwd
            // 
            this.lb_cfpwd.AutoSize = true;
            this.lb_cfpwd.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lb_cfpwd.Location = new System.Drawing.Point(61, 177);
            this.lb_cfpwd.Name = "lb_cfpwd";
            this.lb_cfpwd.Size = new System.Drawing.Size(130, 17);
            this.lb_cfpwd.TabIndex = 2;
            this.lb_cfpwd.Text = "Xác nhận mật khẩu";
            // 
            // lb_email
            // 
            this.lb_email.AutoSize = true;
            this.lb_email.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lb_email.Location = new System.Drawing.Point(61, 216);
            this.lb_email.Name = "lb_email";
            this.lb_email.Size = new System.Drawing.Size(42, 17);
            this.lb_email.TabIndex = 3;
            this.lb_email.Text = "Email";
            // 
            // tb_username
            // 
            this.tb_username.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.tb_username.Location = new System.Drawing.Point(208, 89);
            this.tb_username.Name = "tb_username";
            this.tb_username.Size = new System.Drawing.Size(159, 23);
            this.tb_username.TabIndex = 4;
            // 
            // tb_pwd
            // 
            this.tb_pwd.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.tb_pwd.Location = new System.Drawing.Point(208, 131);
            this.tb_pwd.Name = "tb_pwd";
            this.tb_pwd.PasswordChar = '*';
            this.tb_pwd.Size = new System.Drawing.Size(159, 23);
            this.tb_pwd.TabIndex = 5;
            // 
            // tb_cfpwd
            // 
            this.tb_cfpwd.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.tb_cfpwd.Location = new System.Drawing.Point(208, 171);
            this.tb_cfpwd.Name = "tb_cfpwd";
            this.tb_cfpwd.PasswordChar = '*';
            this.tb_cfpwd.Size = new System.Drawing.Size(159, 23);
            this.tb_cfpwd.TabIndex = 6;
            // 
            // tb_email
            // 
            this.tb_email.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.tb_email.Location = new System.Drawing.Point(208, 210);
            this.tb_email.Name = "tb_email";
            this.tb_email.Size = new System.Drawing.Size(159, 23);
            this.tb_email.TabIndex = 7;
            // 
            // bt_Dk
            // 
            this.bt_Dk.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.bt_Dk.Location = new System.Drawing.Point(115, 388);
            this.bt_Dk.Name = "bt_Dk";
            this.bt_Dk.Size = new System.Drawing.Size(86, 46);
            this.bt_Dk.TabIndex = 8;
            this.bt_Dk.Text = "Đăng ký";
            this.bt_Dk.UseVisualStyleBackColor = true;
            this.bt_Dk.Click += new System.EventHandler(this.bt_DK_Click);
            // 
            // lb_DK
            // 
            this.lb_DK.AutoSize = true;
            this.lb_DK.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F);
            this.lb_DK.Location = new System.Drawing.Point(156, 21);
            this.lb_DK.Name = "lb_DK";
            this.lb_DK.Size = new System.Drawing.Size(166, 46);
            this.lb_DK.TabIndex = 9;
            this.lb_DK.Text = "Đăng ký";
            // 
            // bt_thoat
            // 
            this.bt_thoat.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.bt_thoat.Location = new System.Drawing.Point(249, 388);
            this.bt_thoat.Name = "bt_thoat";
            this.bt_thoat.Size = new System.Drawing.Size(95, 46);
            this.bt_thoat.TabIndex = 10;
            this.bt_thoat.Text = "Thoát";
            this.bt_thoat.UseVisualStyleBackColor = true;
            this.bt_thoat.Click += new System.EventHandler(this.bt_thoat_Click);
            // 
            // tb_hoten
            // 
            this.tb_hoten.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.tb_hoten.Location = new System.Drawing.Point(208, 252);
            this.tb_hoten.Name = "tb_hoten";
            this.tb_hoten.Size = new System.Drawing.Size(159, 23);
            this.tb_hoten.TabIndex = 6;
            // 
            // tb_ngsinh
            // 
            this.tb_ngsinh.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.tb_ngsinh.Location = new System.Drawing.Point(208, 291);
            this.tb_ngsinh.Name = "tb_ngsinh";
            this.tb_ngsinh.Size = new System.Drawing.Size(159, 23);
            this.tb_ngsinh.TabIndex = 7;
            // 
            // lb_ten
            // 
            this.lb_ten.AutoSize = true;
            this.lb_ten.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lb_ten.Location = new System.Drawing.Point(61, 258);
            this.lb_ten.Name = "lb_ten";
            this.lb_ten.Size = new System.Drawing.Size(55, 17);
            this.lb_ten.TabIndex = 3;
            this.lb_ten.Text = "Họ Tên";
            // 
            // lb_ngaysinh
            // 
            this.lb_ngaysinh.AutoSize = true;
            this.lb_ngaysinh.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lb_ngaysinh.Location = new System.Drawing.Point(61, 297);
            this.lb_ngaysinh.Name = "lb_ngaysinh";
            this.lb_ngaysinh.Size = new System.Drawing.Size(71, 17);
            this.lb_ngaysinh.TabIndex = 3;
            this.lb_ngaysinh.Text = "Ngày sinh";
            // 
            // lb_chuthich
            // 
            this.lb_chuthich.AutoSize = true;
            this.lb_chuthich.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_chuthich.Location = new System.Drawing.Point(61, 340);
            this.lb_chuthich.Name = "lb_chuthich";
            this.lb_chuthich.Size = new System.Drawing.Size(312, 32);
            this.lb_chuthich.TabIndex = 11;
            this.lb_chuthich.Text = "Lưu ý ngày sinh ghi theo định dạng sau dd/mm/yyyy\r\n\r\n";
            // 
            // Dangky
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 480);
            this.Controls.Add(this.lb_chuthich);
            this.Controls.Add(this.bt_thoat);
            this.Controls.Add(this.lb_DK);
            this.Controls.Add(this.bt_Dk);
            this.Controls.Add(this.tb_ngsinh);
            this.Controls.Add(this.tb_hoten);
            this.Controls.Add(this.tb_email);
            this.Controls.Add(this.tb_cfpwd);
            this.Controls.Add(this.tb_pwd);
            this.Controls.Add(this.tb_username);
            this.Controls.Add(this.lb_ngaysinh);
            this.Controls.Add(this.lb_ten);
            this.Controls.Add(this.lb_email);
            this.Controls.Add(this.lb_cfpwd);
            this.Controls.Add(this.lb_pwd);
            this.Controls.Add(this.lb_username);
            this.Name = "Dangky";
            this.Text = "Dangky";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lb_username;
        private System.Windows.Forms.Label lb_pwd;
        private System.Windows.Forms.Label lb_cfpwd;
        private System.Windows.Forms.Label lb_email;
        private System.Windows.Forms.TextBox tb_username;
        private System.Windows.Forms.TextBox tb_pwd;
        private System.Windows.Forms.TextBox tb_cfpwd;
        private System.Windows.Forms.TextBox tb_email;
        private System.Windows.Forms.Button bt_Dk;
        private System.Windows.Forms.Label lb_DK;
        private System.Windows.Forms.Button bt_thoat;
        private System.Windows.Forms.TextBox tb_hoten;
        private System.Windows.Forms.TextBox tb_ngsinh;
        private System.Windows.Forms.Label lb_ten;
        private System.Windows.Forms.Label lb_ngaysinh;
        private System.Windows.Forms.Label lb_chuthich;
    }
}