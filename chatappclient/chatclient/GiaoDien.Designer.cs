using System.Drawing;
using System.Windows.Forms;

namespace QLUSER
{
    partial class GiaoDien
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.Nhóm = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Tênkênh = new System.Windows.Forms.ListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.chonanh = new System.Windows.Forms.Button();
            this.chonfile = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.button6 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            this.panel1.Controls.Add(this.Nhóm);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(150, 474);
            this.panel1.TabIndex = 0;
            // 
            // Nhóm
            // 
            this.Nhóm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            this.Nhóm.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.Nhóm.ForeColor = System.Drawing.Color.White;
            this.Nhóm.FormattingEnabled = true;
            this.Nhóm.ItemHeight = 21;
            this.Nhóm.Location = new System.Drawing.Point(35, 110);
            this.Nhóm.Name = "Nhóm";
            this.Nhóm.Size = new System.Drawing.Size(75, 298);
            this.Nhóm.TabIndex = 2;
            this.Nhóm.SelectedIndexChanged += new System.EventHandler(this.lb_nhom_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            this.button1.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(35, 38);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 45);
            this.button1.TabIndex = 1;
            this.button1.Text = "Tạo Nhóm";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.bt_taonhom_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            this.panel2.Controls.Add(this.button4);
            this.panel2.Controls.Add(this.button5);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.Tênkênh);
            this.panel2.Controls.Add(this.button2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(150, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(150, 474);
            this.panel2.TabIndex = 6;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.Gray;
            this.button4.Location = new System.Drawing.Point(22, 432);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(111, 30);
            this.button4.TabIndex = 5;
            this.button4.Text = "Thông tin user";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            this.button5.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.button5.ForeColor = System.Drawing.Color.White;
            this.button5.Location = new System.Drawing.Point(22, 93);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(112, 45);
            this.button5.TabIndex = 5;
            this.button5.Text = "Tạo kênh";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.bt_taokenh_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(18, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "groupname";
            // 
            // Tênkênh
            // 
            this.Tênkênh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            this.Tênkênh.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.Tênkênh.ForeColor = System.Drawing.Color.White;
            this.Tênkênh.FormattingEnabled = true;
            this.Tênkênh.ItemHeight = 21;
            this.Tênkênh.Location = new System.Drawing.Point(21, 152);
            this.Tênkênh.Name = "Tênkênh";
            this.Tênkênh.Size = new System.Drawing.Size(112, 256);
            this.Tênkênh.TabIndex = 3;
            this.Tênkênh.SelectedIndexChanged += new System.EventHandler(this.lb_Tenkenh_SelectedIndexChanged);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            this.button2.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(21, 38);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(112, 45);
            this.button2.TabIndex = 3;
            this.button2.Text = "Mời bạn bè";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.bt_moiuser_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            this.panel3.Controls.Add(this.chonanh);
            this.panel3.Controls.Add(this.chonfile);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.button3);
            this.panel3.Controls.Add(this.textBox1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(300, 400);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(800, 74);
            this.panel3.TabIndex = 8;
            // 
            // chonanh
            // 
            this.chonanh.BackColor = System.Drawing.Color.Gray;
            this.chonanh.Location = new System.Drawing.Point(6, 6);
            this.chonanh.Name = "chonanh";
            this.chonanh.Size = new System.Drawing.Size(64, 30);
            this.chonanh.TabIndex = 4;
            this.chonanh.Text = "Chọn ảnh";
            this.chonanh.UseVisualStyleBackColor = false;
            this.chonanh.Click += new System.EventHandler(this.buttonchonanh_Click);
            // 
            // chonfile
            // 
            this.chonfile.BackColor = System.Drawing.Color.Gray;
            this.chonfile.Location = new System.Drawing.Point(6, 38);
            this.chonfile.Name = "chonfile";
            this.chonfile.Size = new System.Drawing.Size(64, 30);
            this.chonfile.TabIndex = 3;
            this.chonfile.Text = "Chọn file";
            this.chonfile.UseVisualStyleBackColor = false;
            this.chonfile.Click += new System.EventHandler(this.buttonchonfile_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(76, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(594, 29);
            this.label3.TabIndex = 2;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Gray;
            this.button3.Location = new System.Drawing.Point(687, 38);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(97, 30);
            this.button3.TabIndex = 1;
            this.button3.Text = "Gửi";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.bt_guitinnhan_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.Color.White;
            this.textBox1.Location = new System.Drawing.Point(76, 38);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(594, 30);
            this.textBox1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(300, 50);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(10);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(800, 350);
            this.flowLayoutPanel1.TabIndex = 7;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(82, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(592, 30);
            this.label2.TabIndex = 3;
            this.label2.Text = "kênh";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            this.panel4.Controls.Add(this.button6);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(300, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(800, 50);
            this.panel4.TabIndex = 0;
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.Gray;
            this.button6.Location = new System.Drawing.Point(691, 9);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(97, 30);
            this.button6.TabIndex = 4;
            this.button6.Text = "Thoát";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.bt_thoat_Click);
            // 
            // GiaoDien
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 474);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "GiaoDien";
            this.Text = "GiaoDien";
            this.Load += new System.EventHandler(this.GiaoDien_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox Nhóm;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel2;
        private Label label1;
        private ListBox Tênkênh;
        private Button button2;
        private Panel panel3;
        private Button button3;
        private TextBox textBox1;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label label2;
        private Panel panel4;
        private Button button5;
        private Button button6;
        private Label label3;
        private Button chonfile;
        private Button chonanh;
        private Button button4;
    }
}