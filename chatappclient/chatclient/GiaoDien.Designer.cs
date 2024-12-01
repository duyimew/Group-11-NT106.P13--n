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
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.ServerOrFriend = new System.Windows.Forms.TabControl();
            this.FriendsTab = new System.Windows.Forms.TabPage();
            this.ServerTab = new System.Windows.Forms.TabPage();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.label4 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.panel11 = new System.Windows.Forms.Panel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.chonanh = new System.Windows.Forms.Button();
            this.panel9 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.chonfile = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.button6 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.cp_ProfilePic = new QLUSER.Models.CircularPicture();
            this.cp_Menu = new QLUSER.Models.CircularPicture();
            this.panel12 = new System.Windows.Forms.Panel();
            this.btn_Ketban = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.ServerOrFriend.SuspendLayout();
            this.ServerTab.SuspendLayout();
            this.panel6.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel7.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cp_ProfilePic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cp_Menu)).BeginInit();
            this.panel12.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(36)))));
            this.panel1.Controls.Add(this.cp_Menu);
            this.panel1.Controls.Add(this.flowLayoutPanel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(124, 583);
            this.panel1.TabIndex = 0;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 78);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(124, 505);
            this.flowLayoutPanel2.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(57)))), ((int)(((byte)(62)))));
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.ServerOrFriend);
            this.panel2.Location = new System.Drawing.Point(124, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(253, 583);
            this.panel2.TabIndex = 6;
            // 
            // panel5
            // 
            this.panel5.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(43)))), ((int)(((byte)(48)))));
            this.panel5.Controls.Add(this.cp_ProfilePic);
            this.panel5.Controls.Add(this.button4);
            this.panel5.Location = new System.Drawing.Point(0, 503);
            this.panel5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(253, 80);
            this.panel5.TabIndex = 9;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(43)))), ((int)(((byte)(48)))));
            this.button4.BackgroundImage = global::QLUSER.Properties.Resources.settings_24dp_5F6368_FILL0_wght400_GRAD0_opsz24;
            this.button4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button4.FlatAppearance.BorderColor = System.Drawing.Color.IndianRed;
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Location = new System.Drawing.Point(197, 22);
            this.button4.Margin = new System.Windows.Forms.Padding(4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(47, 46);
            this.button4.TabIndex = 5;
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // ServerOrFriend
            // 
            this.ServerOrFriend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ServerOrFriend.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.ServerOrFriend.Controls.Add(this.FriendsTab);
            this.ServerOrFriend.Controls.Add(this.ServerTab);
            this.ServerOrFriend.Controls.Add(this.tabPage3);
            this.ServerOrFriend.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ServerOrFriend.ItemSize = new System.Drawing.Size(0, 1);
            this.ServerOrFriend.Location = new System.Drawing.Point(-9, -9);
            this.ServerOrFriend.Name = "ServerOrFriend";
            this.ServerOrFriend.SelectedIndex = 0;
            this.ServerOrFriend.Size = new System.Drawing.Size(269, 527);
            this.ServerOrFriend.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.ServerOrFriend.TabIndex = 0;
            // 
            // FriendsTab
            // 
            this.FriendsTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(57)))), ((int)(((byte)(62)))));
            this.FriendsTab.Location = new System.Drawing.Point(4, 5);
            this.FriendsTab.Name = "FriendsTab";
            this.FriendsTab.Padding = new System.Windows.Forms.Padding(3);
            this.FriendsTab.Size = new System.Drawing.Size(261, 518);
            this.FriendsTab.TabIndex = 1;
            this.FriendsTab.Text = "tabPage4";
            // 
            // ServerTab
            // 
            this.ServerTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(57)))), ((int)(((byte)(62)))));
            this.ServerTab.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ServerTab.Controls.Add(this.panel12);
            this.ServerTab.Controls.Add(this.label4);
            this.ServerTab.Controls.Add(this.panel6);
            this.ServerTab.Controls.Add(this.label5);
            this.ServerTab.Location = new System.Drawing.Point(4, 5);
            this.ServerTab.Name = "ServerTab";
            this.ServerTab.Padding = new System.Windows.Forms.Padding(3);
            this.ServerTab.Size = new System.Drawing.Size(261, 518);
            this.ServerTab.TabIndex = 0;
            this.ServerTab.Click += new System.EventHandler(this.ServerTab_Click);
            // 
            // treeView1
            // 
            this.treeView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            this.treeView1.ForeColor = System.Drawing.Color.White;
            this.treeView1.Location = new System.Drawing.Point(3, 4);
            this.treeView1.Margin = new System.Windows.Forms.Padding(4);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(222, 398);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(218, 65);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 16);
            this.label4.TabIndex = 0;
            this.label4.Text = "+";
            this.label4.Visible = false;
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(43)))), ((int)(((byte)(48)))));
            this.panel6.Controls.Add(this.label1);
            this.panel6.Controls.Add(this.button5);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(3, 3);
            this.panel6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(255, 55);
            this.panel6.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(14, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(196, 28);
            this.label1.TabIndex = 4;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(218, 11);
            this.button5.Margin = new System.Windows.Forms.Padding(4);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(33, 28);
            this.button5.TabIndex = 11;
            this.button5.Text = "v";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click_1);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(218, 96);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 16);
            this.label5.TabIndex = 10;
            this.label5.Text = "+";
            this.label5.Visible = false;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(57)))), ((int)(((byte)(62)))));
            this.tabPage3.Controls.Add(this.panel11);
            this.tabPage3.Controls.Add(this.panel10);
            this.tabPage3.Location = new System.Drawing.Point(4, 5);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(261, 518);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            // 
            // panel11
            // 
            this.panel11.Location = new System.Drawing.Point(8, 63);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(246, 439);
            this.panel11.TabIndex = 12;
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(43)))), ((int)(((byte)(48)))));
            this.panel10.Controls.Add(this.label6);
            this.panel10.Controls.Add(this.button1);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel10.Location = new System.Drawing.Point(3, 3);
            this.panel10.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(255, 55);
            this.panel10.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            this.label6.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(14, 11);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(196, 28);
            this.label6.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(218, 11);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(33, 28);
            this.button1.TabIndex = 11;
            this.button1.Text = "v";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(69)))), ((int)(((byte)(73)))));
            this.panel3.Controls.Add(this.panel8);
            this.panel3.Controls.Add(this.chonanh);
            this.panel3.Controls.Add(this.panel9);
            this.panel3.Controls.Add(this.chonfile);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(3, 503);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1091, 92);
            this.panel3.TabIndex = 8;
            // 
            // panel8
            // 
            this.panel8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel8.Controls.Add(this.button3);
            this.panel8.Location = new System.Drawing.Point(950, 46);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(138, 42);
            this.panel8.TabIndex = 5;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Gray;
            this.button3.Location = new System.Drawing.Point(4, 4);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(129, 37);
            this.button3.TabIndex = 1;
            this.button3.Text = "Gửi";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.bt_guitinnhan_Click);
            // 
            // chonanh
            // 
            this.chonanh.BackColor = System.Drawing.Color.Transparent;
            this.chonanh.BackgroundImage = global::QLUSER.Properties.Resources.photo_library_24dp_FFFFFF_FILL0_wght400_GRAD0_opsz24;
            this.chonanh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.chonanh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chonanh.FlatAppearance.BorderSize = 0;
            this.chonanh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chonanh.Location = new System.Drawing.Point(24, 7);
            this.chonanh.Margin = new System.Windows.Forms.Padding(4);
            this.chonanh.Name = "chonanh";
            this.chonanh.Size = new System.Drawing.Size(43, 37);
            this.chonanh.TabIndex = 4;
            this.chonanh.UseVisualStyleBackColor = false;
            this.chonanh.Click += new System.EventHandler(this.buttonchonanh_Click);
            // 
            // panel9
            // 
            this.panel9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel9.Controls.Add(this.textBox1);
            this.panel9.Location = new System.Drawing.Point(78, 44);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(823, 40);
            this.panel9.TabIndex = 6;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.Color.White;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textBox1.Size = new System.Drawing.Size(823, 40);
            this.textBox1.TabIndex = 0;
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // chonfile
            // 
            this.chonfile.BackColor = System.Drawing.Color.Transparent;
            this.chonfile.BackgroundImage = global::QLUSER.Properties.Resources.attach_file_24dp_FFFFFF_FILL0_wght400_GRAD0_opsz24;
            this.chonfile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.chonfile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chonfile.FlatAppearance.BorderSize = 0;
            this.chonfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chonfile.Location = new System.Drawing.Point(24, 50);
            this.chonfile.Margin = new System.Windows.Forms.Padding(4);
            this.chonfile.Name = "chonfile";
            this.chonfile.Size = new System.Drawing.Size(43, 37);
            this.chonfile.TabIndex = 3;
            this.chonfile.UseVisualStyleBackColor = false;
            this.chonfile.Click += new System.EventHandler(this.buttonchonfile_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(69)))), ((int)(((byte)(73)))));
            this.label3.Location = new System.Drawing.Point(75, 7);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(835, 36);
            this.label3.TabIndex = 2;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(69)))), ((int)(((byte)(73)))));
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 65);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1094, 439);
            this.flowLayoutPanel1.TabIndex = 7;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(109, 12);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(789, 37);
            this.label2.TabIndex = 3;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(69)))), ((int)(((byte)(73)))));
            this.panel4.Controls.Add(this.panel7);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(3, 3);
            this.panel4.Margin = new System.Windows.Forms.Padding(4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1091, 63);
            this.panel4.TabIndex = 0;
            // 
            // panel7
            // 
            this.panel7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel7.Controls.Add(this.button6);
            this.panel7.Location = new System.Drawing.Point(935, 12);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(143, 37);
            this.panel7.TabIndex = 11;
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.Gray;
            this.button6.Location = new System.Drawing.Point(4, 0);
            this.button6.Margin = new System.Windows.Forms.Padding(4);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(129, 37);
            this.button6.TabIndex = 4;
            this.button6.Text = "Thoát";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.bt_thoat_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.ItemSize = new System.Drawing.Size(0, 1);
            this.tabControl1.Location = new System.Drawing.Point(370, -14);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1105, 607);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(69)))), ((int)(((byte)(73)))));
            this.tabPage1.Controls.Add(this.panel3);
            this.tabPage1.Controls.Add(this.panel4);
            this.tabPage1.Controls.Add(this.flowLayoutPanel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 5);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1097, 598);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(69)))), ((int)(((byte)(73)))));
            this.tabPage2.Controls.Add(this.button2);
            this.tabPage2.Controls.Add(this.btn_Ketban);
            this.tabPage2.Location = new System.Drawing.Point(4, 5);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1097, 598);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            // 
            // cp_ProfilePic
            // 
            this.cp_ProfilePic.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(43)))), ((int)(((byte)(48)))));
            this.cp_ProfilePic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cp_ProfilePic.Location = new System.Drawing.Point(3, 6);
            this.cp_ProfilePic.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cp_ProfilePic.Name = "cp_ProfilePic";
            this.cp_ProfilePic.Size = new System.Drawing.Size(78, 68);
            this.cp_ProfilePic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.cp_ProfilePic.TabIndex = 5;
            this.cp_ProfilePic.TabStop = false;
            // 
            // cp_Menu
            // 
            this.cp_Menu.Image = global::QLUSER.Properties.Resources._379512_chat_icon;
            this.cp_Menu.Location = new System.Drawing.Point(22, 6);
            this.cp_Menu.Name = "cp_Menu";
            this.cp_Menu.Size = new System.Drawing.Size(77, 65);
            this.cp_Menu.TabIndex = 0;
            this.cp_Menu.TabStop = false;
            this.cp_Menu.Click += new System.EventHandler(this.cp_Menu_Click);
            // 
            // panel12
            // 
            this.panel12.Controls.Add(this.treeView1);
            this.panel12.Location = new System.Drawing.Point(10, 65);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(239, 437);
            this.panel12.TabIndex = 11;
            // 
            // btn_Ketban
            // 
            this.btn_Ketban.Location = new System.Drawing.Point(16, 21);
            this.btn_Ketban.Name = "btn_Ketban";
            this.btn_Ketban.Size = new System.Drawing.Size(75, 23);
            this.btn_Ketban.TabIndex = 0;
            this.btn_Ketban.Text = "Kết bạn";
            this.btn_Ketban.UseVisualStyleBackColor = true;
            this.btn_Ketban.Click += new System.EventHandler(this.btn_Ketban_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(180, 21);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Xem lời mời kết bạn";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // GiaoDien
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1467, 583);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "GiaoDien";
            this.Text = "GiaoDien";
            this.Load += new System.EventHandler(this.GiaoDien_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.ServerOrFriend.ResumeLayout(false);
            this.ServerTab.ResumeLayout(false);
            this.ServerTab.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cp_ProfilePic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cp_Menu)).EndInit();
            this.panel12.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private Label label1;
        private Panel panel3;
        private Button button3;
        private TextBox textBox1;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label label2;
        private Panel panel4;
        private Button button6;
        private Label label3;
        private Button chonfile;
        private Button chonanh;
        private Button button4;
        private Models.CircularPicture cp_ProfilePic;
        private Panel panel5;
        private TreeView treeView1;
        private Label label4;
        private Label label5;
        private Button button5;
        private Panel panel6;
        private FlowLayoutPanel flowLayoutPanel2;
        private Models.CircularPicture cp_Menu;
        private Panel panel7;
        private Panel panel8;
        private Panel panel9;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabControl ServerOrFriend;
        private TabPage ServerTab;
        private TabPage FriendsTab;
        private TabPage tabPage3;
        private Panel panel10;
        private Label label6;
        private Button button1;
        private Panel panel11;
        private Panel panel12;
        private Button btn_Ketban;
        private Button button2;
    }
}