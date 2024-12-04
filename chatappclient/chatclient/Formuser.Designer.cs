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
            this.lb_username = new System.Windows.Forms.Label();
            this.lb_user = new System.Windows.Forms.Label();
            this.bt_thoat = new System.Windows.Forms.Button();
            this.lb_email = new System.Windows.Forms.Label();
            this.lb_Ten = new System.Windows.Forms.Label();
            this.lb_NgSinh = new System.Windows.Forms.Label();
            this.bt_dangxuat = new System.Windows.Forms.Button();
            this.lb_uname = new System.Windows.Forms.Label();
            this.lb_mail = new System.Windows.Forms.Label();
            this.lb_name = new System.Windows.Forms.Label();
            this.lb_bd = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.circularPicture1 = new QLUSER.Models.CircularPicture();
            this.btn_ChangeAva = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.circularPicture1)).BeginInit();
            this.SuspendLayout();
            // 
            // lb_username
            // 
            this.lb_username.AutoSize = true;
            this.lb_username.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lb_username.ForeColor = System.Drawing.Color.Silver;
            this.lb_username.Location = new System.Drawing.Point(140, 358);
            this.lb_username.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lb_username.Name = "lb_username";
            this.lb_username.Size = new System.Drawing.Size(73, 17);
            this.lb_username.TabIndex = 1;
            this.lb_username.Text = "Username";
            this.lb_username.Click += new System.EventHandler(this.lb_username_Click);
            // 
            // lb_user
            // 
            this.lb_user.AutoSize = true;
            this.lb_user.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(36)))));
            this.lb_user.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_user.ForeColor = System.Drawing.Color.White;
            this.lb_user.Location = new System.Drawing.Point(77, 77);
            this.lb_user.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lb_user.Name = "lb_user";
            this.lb_user.Size = new System.Drawing.Size(267, 31);
            this.lb_user.TabIndex = 2;
            this.lb_user.Text = "Thông tin người dùng";
            // 
            // bt_thoat
            // 
            this.bt_thoat.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.bt_thoat.Location = new System.Drawing.Point(121, 600);
            this.bt_thoat.Margin = new System.Windows.Forms.Padding(6);
            this.bt_thoat.Name = "bt_thoat";
            this.bt_thoat.Size = new System.Drawing.Size(190, 72);
            this.bt_thoat.TabIndex = 9;
            this.bt_thoat.Text = "Tắt";
            this.bt_thoat.UseVisualStyleBackColor = true;
            this.bt_thoat.Click += new System.EventHandler(this.bt_thoat_Click);
            // 
            // lb_email
            // 
            this.lb_email.AutoSize = true;
            this.lb_email.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lb_email.ForeColor = System.Drawing.Color.Silver;
            this.lb_email.Location = new System.Drawing.Point(140, 417);
            this.lb_email.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lb_email.Name = "lb_email";
            this.lb_email.Size = new System.Drawing.Size(42, 17);
            this.lb_email.TabIndex = 1;
            this.lb_email.Text = "Email";
            // 
            // lb_Ten
            // 
            this.lb_Ten.AutoSize = true;
            this.lb_Ten.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lb_Ten.ForeColor = System.Drawing.Color.Silver;
            this.lb_Ten.Location = new System.Drawing.Point(140, 477);
            this.lb_Ten.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lb_Ten.Name = "lb_Ten";
            this.lb_Ten.Size = new System.Drawing.Size(33, 17);
            this.lb_Ten.TabIndex = 1;
            this.lb_Ten.Text = "Tên";
            // 
            // lb_NgSinh
            // 
            this.lb_NgSinh.AutoSize = true;
            this.lb_NgSinh.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lb_NgSinh.ForeColor = System.Drawing.Color.Silver;
            this.lb_NgSinh.Location = new System.Drawing.Point(140, 530);
            this.lb_NgSinh.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lb_NgSinh.Name = "lb_NgSinh";
            this.lb_NgSinh.Size = new System.Drawing.Size(71, 17);
            this.lb_NgSinh.TabIndex = 1;
            this.lb_NgSinh.Text = "Ngày sinh";
            // 
            // bt_dangxuat
            // 
            this.bt_dangxuat.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.bt_dangxuat.Location = new System.Drawing.Point(974, 600);
            this.bt_dangxuat.Margin = new System.Windows.Forms.Padding(6);
            this.bt_dangxuat.Name = "bt_dangxuat";
            this.bt_dangxuat.Size = new System.Drawing.Size(190, 72);
            this.bt_dangxuat.TabIndex = 10;
            this.bt_dangxuat.Text = "Đăng xuất";
            this.bt_dangxuat.UseVisualStyleBackColor = true;
            this.bt_dangxuat.Click += new System.EventHandler(this.bt_dangxuat_Click);
            // 
            // lb_uname
            // 
            this.lb_uname.AutoSize = true;
            this.lb_uname.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_uname.ForeColor = System.Drawing.Color.White;
            this.lb_uname.Location = new System.Drawing.Point(142, 378);
            this.lb_uname.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_uname.Name = "lb_uname";
            this.lb_uname.Size = new System.Drawing.Size(134, 20);
            this.lb_uname.TabIndex = 11;
            this.lb_uname.Text = "<username here>";
            // 
            // lb_mail
            // 
            this.lb_mail.AutoSize = true;
            this.lb_mail.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_mail.ForeColor = System.Drawing.Color.White;
            this.lb_mail.Location = new System.Drawing.Point(142, 437);
            this.lb_mail.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_mail.Name = "lb_mail";
            this.lb_mail.Size = new System.Drawing.Size(100, 20);
            this.lb_mail.TabIndex = 12;
            this.lb_mail.Text = "<email here>";
            // 
            // lb_name
            // 
            this.lb_name.AutoSize = true;
            this.lb_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_name.ForeColor = System.Drawing.Color.White;
            this.lb_name.Location = new System.Drawing.Point(144, 497);
            this.lb_name.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_name.Name = "lb_name";
            this.lb_name.Size = new System.Drawing.Size(103, 20);
            this.lb_name.TabIndex = 13;
            this.lb_name.Text = "<name here>";
            // 
            // lb_bd
            // 
            this.lb_bd.AutoSize = true;
            this.lb_bd.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_bd.ForeColor = System.Drawing.Color.White;
            this.lb_bd.Location = new System.Drawing.Point(142, 550);
            this.lb_bd.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_bd.Name = "lb_bd";
            this.lb_bd.Size = new System.Drawing.Size(110, 20);
            this.lb_bd.TabIndex = 14;
            this.lb_bd.Text = "<ngsinh here>";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(36)))));
            this.label1.Location = new System.Drawing.Point(66, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1152, 656);
            this.label1.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(69)))), ((int)(((byte)(73)))));
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label2.Location = new System.Drawing.Point(105, 318);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(1073, 374);
            this.label2.TabIndex = 16;
            // 
            // circularPicture1
            // 
            this.circularPicture1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.circularPicture1.Location = new System.Drawing.Point(144, 143);
            this.circularPicture1.Name = "circularPicture1";
            this.circularPicture1.Size = new System.Drawing.Size(124, 120);
            this.circularPicture1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.circularPicture1.TabIndex = 17;
            this.circularPicture1.TabStop = false;
            this.circularPicture1.TextColor = System.Drawing.Color.Black;
            this.circularPicture1.TextFont = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            // 
            // btn_ChangeAva
            // 
            this.btn_ChangeAva.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(113)))), ((int)(((byte)(255)))));
            this.btn_ChangeAva.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_ChangeAva.ForeColor = System.Drawing.Color.White;
            this.btn_ChangeAva.Location = new System.Drawing.Point(971, 179);
            this.btn_ChangeAva.Name = "btn_ChangeAva";
            this.btn_ChangeAva.Size = new System.Drawing.Size(193, 54);
            this.btn_ChangeAva.TabIndex = 18;
            this.btn_ChangeAva.Text = "Đổi ảnh đại diện";
            this.btn_ChangeAva.UseVisualStyleBackColor = false;
            this.btn_ChangeAva.Click += new System.EventHandler(this.btn_ChangeAva_Click);
            // 
            // Formuser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(69)))), ((int)(((byte)(73)))));
            this.ClientSize = new System.Drawing.Size(1275, 755);
            this.Controls.Add(this.btn_ChangeAva);
            this.Controls.Add(this.circularPicture1);
            this.Controls.Add(this.lb_bd);
            this.Controls.Add(this.lb_name);
            this.Controls.Add(this.lb_mail);
            this.Controls.Add(this.lb_uname);
            this.Controls.Add(this.bt_dangxuat);
            this.Controls.Add(this.bt_thoat);
            this.Controls.Add(this.lb_user);
            this.Controls.Add(this.lb_NgSinh);
            this.Controls.Add(this.lb_Ten);
            this.Controls.Add(this.lb_email);
            this.Controls.Add(this.lb_username);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "Formuser";
            this.Text = "Formuser";
            this.Load += new System.EventHandler(this.Formuser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.circularPicture1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lb_username;
        private System.Windows.Forms.Label lb_user;
        private System.Windows.Forms.Button bt_thoat;
        private System.Windows.Forms.Label lb_email;
        private System.Windows.Forms.Label lb_Ten;
        private System.Windows.Forms.Label lb_NgSinh;
        private System.Windows.Forms.Button bt_dangxuat;
        private System.Windows.Forms.Label lb_uname;
        private System.Windows.Forms.Label lb_mail;
        private System.Windows.Forms.Label lb_name;
        private System.Windows.Forms.Label lb_bd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Models.CircularPicture circularPicture1;
        private System.Windows.Forms.Button btn_ChangeAva;
    }
}