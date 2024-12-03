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
            this.panel12 = new System.Windows.Forms.Panel();
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
            this.button2 = new System.Windows.Forms.Button();
            this.btn_Ketban = new System.Windows.Forms.Button();
            this.cp_ProfilePic = new QLUSER.Models.CircularPicture();
            this.cp_Menu = new QLUSER.Models.CircularPicture();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.ServerOrFriend.SuspendLayout();
            this.ServerTab.SuspendLayout();
            this.panel12.SuspendLayout();
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
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(36)))));
            this.panel1.Controls.Add(this.cp_Menu);
            this.panel1.Controls.Add(this.flowLayoutPanel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(93, 474);
            this.panel1.TabIndex = 0;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.flowLayoutPanel2.AutoScroll = true;
            this.flowLayoutPanel2.AutoScrollMinSize = new System.Drawing.Size(1, 1);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 63);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(93, 410);
            this.flowLayoutPanel2.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(57)))), ((int)(((byte)(62)))));
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.ServerOrFriend);
            this.panel2.Location = new System.Drawing.Point(93, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(190, 474);
            this.panel2.TabIndex = 6;
            // 
            // panel5
            // 
            this.panel5.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(43)))), ((int)(((byte)(48)))));
            this.panel5.Controls.Add(this.cp_ProfilePic);
            this.panel5.Controls.Add(this.button4);
            this.panel5.Location = new System.Drawing.Point(0, 409);
            this.panel5.Margin = new System.Windows.Forms.Padding(2);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(190, 65);
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
            this.button4.Location = new System.Drawing.Point(148, 18);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(35, 37);
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
            this.ServerOrFriend.Location = new System.Drawing.Point(-7, -7);
            this.ServerOrFriend.Margin = new System.Windows.Forms.Padding(2);
            this.ServerOrFriend.Name = "ServerOrFriend";
            this.ServerOrFriend.SelectedIndex = 0;
            this.ServerOrFriend.Size = new System.Drawing.Size(202, 428);
            this.ServerOrFriend.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.ServerOrFriend.TabIndex = 0;
            // 
            // FriendsTab
            // 
            this.FriendsTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(57)))), ((int)(((byte)(62)))));
            this.FriendsTab.Location = new System.Drawing.Point(4, 5);
            this.FriendsTab.Margin = new System.Windows.Forms.Padding(2);
            this.FriendsTab.Name = "FriendsTab";
            this.FriendsTab.Padding = new System.Windows.Forms.Padding(2);
            this.FriendsTab.Size = new System.Drawing.Size(194, 419);
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
            this.ServerTab.Margin = new System.Windows.Forms.Padding(2);
            this.ServerTab.Name = "ServerTab";
            this.ServerTab.Padding = new System.Windows.Forms.Padding(2);
            this.ServerTab.Size = new System.Drawing.Size(194, 419);
            this.ServerTab.TabIndex = 0;
            this.ServerTab.Click += new System.EventHandler(this.ServerTab_Click);
            // 
            // panel12
            // 
            this.panel12.Controls.Add(this.treeView1);
            this.panel12.Location = new System.Drawing.Point(8, 53);
            this.panel12.Margin = new System.Windows.Forms.Padding(2);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(179, 355);
            this.panel12.TabIndex = 11;
            // 
            // treeView1
            // 
            this.treeView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            this.treeView1.ForeColor = System.Drawing.Color.White;
            this.treeView1.Location = new System.Drawing.Point(2, 3);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(168, 324);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(164, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(13, 13);
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
            this.panel6.Location = new System.Drawing.Point(2, 2);
            this.panel6.Margin = new System.Windows.Forms.Padding(2);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(190, 45);
            this.panel6.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(10, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 23);
            this.label1.TabIndex = 4;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(164, 9);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(25, 23);
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
            this.label5.Location = new System.Drawing.Point(164, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(13, 13);
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
            this.tabPage3.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage3.Size = new System.Drawing.Size(194, 419);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            // 
            // panel11
            // 
            this.panel11.Location = new System.Drawing.Point(6, 51);
            this.panel11.Margin = new System.Windows.Forms.Padding(2);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(184, 357);
            this.panel11.TabIndex = 12;
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(43)))), ((int)(((byte)(48)))));
            this.panel10.Controls.Add(this.label6);
            this.panel10.Controls.Add(this.button1);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel10.Location = new System.Drawing.Point(2, 2);
            this.panel10.Margin = new System.Windows.Forms.Padding(2);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(190, 45);
            this.panel10.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            this.label6.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(10, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(147, 23);
            this.label6.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(164, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(25, 23);
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
            this.panel3.Location = new System.Drawing.Point(2, 407);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(817, 75);
            this.panel3.TabIndex = 8;
            // 
            // panel8
            // 
            this.panel8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel8.Controls.Add(this.button3);
            this.panel8.Location = new System.Drawing.Point(711, 37);
            this.panel8.Margin = new System.Windows.Forms.Padding(2);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(104, 34);
            this.panel8.TabIndex = 5;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Gray;
            this.button3.Location = new System.Drawing.Point(3, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(97, 30);
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
            this.chonanh.Location = new System.Drawing.Point(18, 6);
            this.chonanh.Name = "chonanh";
            this.chonanh.Size = new System.Drawing.Size(32, 30);
            this.chonanh.TabIndex = 4;
            this.chonanh.UseVisualStyleBackColor = false;
            this.chonanh.Click += new System.EventHandler(this.buttonchonanh_Click);
            // 
            // panel9
            // 
            this.panel9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel9.Controls.Add(this.textBox1);
            this.panel9.Location = new System.Drawing.Point(58, 36);
            this.panel9.Margin = new System.Windows.Forms.Padding(2);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(616, 32);
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
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textBox1.Size = new System.Drawing.Size(616, 32);
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
            this.chonfile.Location = new System.Drawing.Point(18, 41);
            this.chonfile.Name = "chonfile";
            this.chonfile.Size = new System.Drawing.Size(32, 30);
            this.chonfile.TabIndex = 3;
            this.chonfile.UseVisualStyleBackColor = false;
            this.chonfile.Click += new System.EventHandler(this.buttonchonfile_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(69)))), ((int)(((byte)(73)))));
            this.label3.Location = new System.Drawing.Point(56, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(626, 29);
            this.label3.TabIndex = 2;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(69)))), ((int)(((byte)(73)))));
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(2, 53);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(10);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(820, 357);
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
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(69)))), ((int)(((byte)(73)))));
            this.panel4.Controls.Add(this.panel7);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(2, 2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(817, 51);
            this.panel4.TabIndex = 0;
            // 
            // panel7
            // 
            this.panel7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel7.Controls.Add(this.button6);
            this.panel7.Location = new System.Drawing.Point(700, 10);
            this.panel7.Margin = new System.Windows.Forms.Padding(2);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(107, 30);
            this.panel7.TabIndex = 11;
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.Gray;
            this.button6.Location = new System.Drawing.Point(3, 0);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(97, 30);
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
            this.tabControl1.Location = new System.Drawing.Point(278, -11);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(829, 493);
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
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(821, 484);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(69)))), ((int)(((byte)(73)))));
            this.tabPage2.Controls.Add(this.button2);
            this.tabPage2.Controls.Add(this.btn_Ketban);
            this.tabPage2.Location = new System.Drawing.Point(4, 5);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(821, 484);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(135, 17);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(56, 19);
            this.button2.TabIndex = 1;
            this.button2.Text = "Xem lời mời kết bạn";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btn_Ketban
            // 
            this.btn_Ketban.Location = new System.Drawing.Point(12, 17);
            this.btn_Ketban.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Ketban.Name = "btn_Ketban";
            this.btn_Ketban.Size = new System.Drawing.Size(56, 19);
            this.btn_Ketban.TabIndex = 0;
            this.btn_Ketban.Text = "Kết bạn";
            this.btn_Ketban.UseVisualStyleBackColor = true;
            this.btn_Ketban.Click += new System.EventHandler(this.btn_Ketban_Click);
            // 
            // cp_ProfilePic
            // 
            this.cp_ProfilePic.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(43)))), ((int)(((byte)(48)))));
            this.cp_ProfilePic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cp_ProfilePic.Location = new System.Drawing.Point(2, 5);
            this.cp_ProfilePic.Margin = new System.Windows.Forms.Padding(2);
            this.cp_ProfilePic.Name = "cp_ProfilePic";
            this.cp_ProfilePic.Size = new System.Drawing.Size(59, 56);
            this.cp_ProfilePic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.cp_ProfilePic.TabIndex = 5;
            this.cp_ProfilePic.TabStop = false;
            // 
            // cp_Menu
            // 
            this.cp_Menu.Image = global::QLUSER.Properties.Resources._379512_chat_icon;
            this.cp_Menu.Location = new System.Drawing.Point(23, 5);
            this.cp_Menu.Margin = new System.Windows.Forms.Padding(2);
            this.cp_Menu.Name = "cp_Menu";
            this.cp_Menu.Size = new System.Drawing.Size(51, 53);
            this.cp_Menu.TabIndex = 0;
            this.cp_Menu.TabStop = false;
            this.cp_Menu.Click += new System.EventHandler(this.cp_Menu_Click);
            // 
            // GiaoDien
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 474);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "GiaoDien";
            this.Text = "GiaoDien";
            this.Load += new System.EventHandler(this.GiaoDien_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.ServerOrFriend.ResumeLayout(false);
            this.ServerTab.ResumeLayout(false);
            this.ServerTab.PerformLayout();
            this.panel12.ResumeLayout(false);
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