namespace QLUSER
{
    partial class Dangnhap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Dangnhap));
            this.lb_username = new System.Windows.Forms.Label();
            this.lb_pwd = new System.Windows.Forms.Label();
            this.tb_username = new System.Windows.Forms.TextBox();
            this.tb_pwd = new System.Windows.Forms.TextBox();
            this.bt_DN = new System.Windows.Forms.Button();
            this.bt_DK = new System.Windows.Forms.Button();
            this.lb_DN = new System.Windows.Forms.Label();
            this.lb_text = new System.Windows.Forms.Label();
            this.bt_thoat = new System.Windows.Forms.Button();
            this.btn_ForgotPass = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lb_username
            // 
            this.lb_username.AutoSize = true;
            this.lb_username.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lb_username.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lb_username.Location = new System.Drawing.Point(65, 146);
            this.lb_username.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_username.Name = "lb_username";
            this.lb_username.Size = new System.Drawing.Size(119, 20);
            this.lb_username.TabIndex = 0;
            this.lb_username.Text = "Tên đăng nhập";
            // 
            // lb_pwd
            // 
            this.lb_pwd.AutoSize = true;
            this.lb_pwd.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lb_pwd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lb_pwd.Location = new System.Drawing.Point(65, 228);
            this.lb_pwd.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_pwd.Name = "lb_pwd";
            this.lb_pwd.Size = new System.Drawing.Size(77, 20);
            this.lb_pwd.TabIndex = 1;
            this.lb_pwd.Text = "Mật khẩu";
            // 
            // tb_username
            // 
            this.tb_username.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.tb_username.Location = new System.Drawing.Point(69, 170);
            this.tb_username.Margin = new System.Windows.Forms.Padding(4);
            this.tb_username.Name = "tb_username";
            this.tb_username.Size = new System.Drawing.Size(335, 26);
            this.tb_username.TabIndex = 2;
            // 
            // tb_pwd
            // 
            this.tb_pwd.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.tb_pwd.Location = new System.Drawing.Point(69, 252);
            this.tb_pwd.Margin = new System.Windows.Forms.Padding(4);
            this.tb_pwd.Name = "tb_pwd";
            this.tb_pwd.PasswordChar = '*';
            this.tb_pwd.Size = new System.Drawing.Size(335, 26);
            this.tb_pwd.TabIndex = 3;
            // 
            // bt_DN
            // 
            this.bt_DN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(137)))), ((int)(((byte)(218)))));
            this.bt_DN.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bt_DN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_DN.ForeColor = System.Drawing.Color.White;
            this.bt_DN.Location = new System.Drawing.Point(69, 354);
            this.bt_DN.Margin = new System.Windows.Forms.Padding(4);
            this.bt_DN.Name = "bt_DN";
            this.bt_DN.Size = new System.Drawing.Size(136, 46);
            this.bt_DN.TabIndex = 4;
            this.bt_DN.Text = "Đăng nhập";
            this.bt_DN.UseVisualStyleBackColor = false;
            this.bt_DN.Click += new System.EventHandler(this.bt_DN_Click);
            // 
            // bt_DK
            // 
            this.bt_DK.FlatAppearance.BorderSize = 0;
            this.bt_DK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_DK.Font = new System.Drawing.Font("MS Reference Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_DK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(137)))), ((int)(((byte)(218)))));
            this.bt_DK.Location = new System.Drawing.Point(277, 438);
            this.bt_DK.Margin = new System.Windows.Forms.Padding(4);
            this.bt_DK.Name = "bt_DK";
            this.bt_DK.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.bt_DK.Size = new System.Drawing.Size(120, 34);
            this.bt_DK.TabIndex = 5;
            this.bt_DK.Text = "Đăng ký ngay";
            this.bt_DK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bt_DK.UseVisualStyleBackColor = true;
            this.bt_DK.Click += new System.EventHandler(this.bt_DK_Click);
            // 
            // lb_DN
            // 
            this.lb_DN.AutoSize = true;
            this.lb_DN.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F);
            this.lb_DN.ForeColor = System.Drawing.Color.White;
            this.lb_DN.Location = new System.Drawing.Point(89, 43);
            this.lb_DN.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_DN.Name = "lb_DN";
            this.lb_DN.Size = new System.Drawing.Size(270, 58);
            this.lb_DN.TabIndex = 6;
            this.lb_DN.Text = "Đăng nhập";
            // 
            // lb_text
            // 
            this.lb_text.AutoSize = true;
            this.lb_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lb_text.ForeColor = System.Drawing.Color.White;
            this.lb_text.Location = new System.Drawing.Point(65, 444);
            this.lb_text.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_text.Name = "lb_text";
            this.lb_text.Size = new System.Drawing.Size(184, 20);
            this.lb_text.TabIndex = 7;
            this.lb_text.Text = "Bạn chưa có tài khoản?";
            // 
            // bt_thoat
            // 
            this.bt_thoat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(137)))), ((int)(((byte)(218)))));
            this.bt_thoat.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bt_thoat.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_thoat.ForeColor = System.Drawing.Color.White;
            this.bt_thoat.Location = new System.Drawing.Point(277, 354);
            this.bt_thoat.Margin = new System.Windows.Forms.Padding(4);
            this.bt_thoat.Name = "bt_thoat";
            this.bt_thoat.Size = new System.Drawing.Size(127, 46);
            this.bt_thoat.TabIndex = 8;
            this.bt_thoat.Text = "Thoát";
            this.bt_thoat.UseVisualStyleBackColor = false;
            this.bt_thoat.Click += new System.EventHandler(this.bt_thoat_Click);
            // 
            // btn_ForgotPass
            // 
            this.btn_ForgotPass.FlatAppearance.BorderSize = 0;
            this.btn_ForgotPass.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ForgotPass.Font = new System.Drawing.Font("MS Reference Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_ForgotPass.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(137)))), ((int)(((byte)(218)))));
            this.btn_ForgotPass.Location = new System.Drawing.Point(69, 285);
            this.btn_ForgotPass.Name = "btn_ForgotPass";
            this.btn_ForgotPass.Size = new System.Drawing.Size(180, 33);
            this.btn_ForgotPass.TabIndex = 9;
            this.btn_ForgotPass.Text = "Quên mật khẩu?";
            this.btn_ForgotPass.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_ForgotPass.UseVisualStyleBackColor = true;
            this.btn_ForgotPass.Click += new System.EventHandler(this.btn_ForgotPass_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::QLUSER.Properties.Resources._3_removebg_preview;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(462, 115);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(308, 349);
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // Dangnhap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(57)))), ((int)(((byte)(62)))));
            this.ClientSize = new System.Drawing.Size(784, 554);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btn_ForgotPass);
            this.Controls.Add(this.bt_thoat);
            this.Controls.Add(this.lb_text);
            this.Controls.Add(this.lb_DN);
            this.Controls.Add(this.bt_DK);
            this.Controls.Add(this.bt_DN);
            this.Controls.Add(this.tb_pwd);
            this.Controls.Add(this.tb_username);
            this.Controls.Add(this.lb_pwd);
            this.Controls.Add(this.lb_username);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Dangnhap";
            this.Text = "Đăng nhập";
            this.Load += new System.EventHandler(this.Dangnhap_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lb_username;
        private System.Windows.Forms.Label lb_pwd;
        private System.Windows.Forms.TextBox tb_username;
        private System.Windows.Forms.TextBox tb_pwd;
        private System.Windows.Forms.Button bt_DN;
        private System.Windows.Forms.Button bt_DK;
        private System.Windows.Forms.Label lb_DN;
        private System.Windows.Forms.Label lb_text;
        private System.Windows.Forms.Button bt_thoat;
        private System.Windows.Forms.Button btn_ForgotPass;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

