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
            this.P_Group_Friend = new System.Windows.Forms.Panel();
            this.cp_MenuFriend = new QLUSER.Models.CircularPicture();
            this.flp_Group = new System.Windows.Forms.FlowLayoutPanel();
            this.P_Kenh_DanhMuc_User = new System.Windows.Forms.Panel();
            this.P_TTinUser = new System.Windows.Forms.Panel();
            this.cp_ProfilePic = new QLUSER.Models.CircularPicture();
            this.btn_CDuser = new System.Windows.Forms.Button();
            this.TC_ServerOrFriend = new System.Windows.Forms.TabControl();
            this.TP_FriendsTab = new System.Windows.Forms.TabPage();
            this.flp_TinNhanTT = new System.Windows.Forms.FlowLayoutPanel();
            this.lb_TinNhanTT = new System.Windows.Forms.Label();
            this.btn_AddGroupPrivate = new System.Windows.Forms.Button();
            this.btn_Banbe = new System.Windows.Forms.Button();
            this.TP_ServerTab = new System.Windows.Forms.TabPage();
            this.P_Kenh_DanhMuc = new System.Windows.Forms.Panel();
            this.tv_Kenh_DanhMuc = new System.Windows.Forms.TreeView();
            this.P_TenGroup = new System.Windows.Forms.Panel();
            this.lb_TenGroup = new System.Windows.Forms.Label();
            this.btn_MenuGroup = new System.Windows.Forms.Button();
            this.TP_ServerTab1 = new System.Windows.Forms.TabPage();
            this.P_Kenh_DanhMuc1 = new System.Windows.Forms.Panel();
            this.P_TenGroup1 = new System.Windows.Forms.Panel();
            this.lb_TenGroup1 = new System.Windows.Forms.Label();
            this.btn_MenuGroup1 = new System.Windows.Forms.Button();
            this.P_GuiMessage = new System.Windows.Forms.Panel();
            this.btn_Emoji = new System.Windows.Forms.Button();
            this.P_Gui = new System.Windows.Forms.Panel();
            this.btn_Gui = new System.Windows.Forms.Button();
            this.btn_ChonAnh = new System.Windows.Forms.Button();
            this.P_Message = new System.Windows.Forms.Panel();
            this.tb_Message = new System.Windows.Forms.TextBox();
            this.btn_ChonFile = new System.Windows.Forms.Button();
            this.lb_File_Anh = new System.Windows.Forms.Label();
            this.flp_Message = new System.Windows.Forms.FlowLayoutPanel();
            this.lb_TenKenh = new System.Windows.Forms.Label();
            this.P_TenKenh = new System.Windows.Forms.Panel();
            this.btn_SearchMessage = new System.Windows.Forms.Button();
            this.P_Thoat = new System.Windows.Forms.Panel();
            this.btn_Thoat = new System.Windows.Forms.Button();
            this.TC_Chat = new System.Windows.Forms.TabControl();
            this.TP_ChattingView = new System.Windows.Forms.TabPage();
            this.TP_FriendRequestView = new System.Windows.Forms.TabPage();
            this.btn_XemListFriend = new System.Windows.Forms.Button();
            this.P_Friend = new System.Windows.Forms.Panel();
            this.flp_Friends = new System.Windows.Forms.FlowLayoutPanel();
            this.btn_XemLoiMoi = new System.Windows.Forms.Button();
            this.btn_Ketban = new System.Windows.Forms.Button();
            this.P_Group_Friend.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cp_MenuFriend)).BeginInit();
            this.P_Kenh_DanhMuc_User.SuspendLayout();
            this.P_TTinUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cp_ProfilePic)).BeginInit();
            this.TC_ServerOrFriend.SuspendLayout();
            this.TP_FriendsTab.SuspendLayout();
            this.TP_ServerTab.SuspendLayout();
            this.P_Kenh_DanhMuc.SuspendLayout();
            this.P_TenGroup.SuspendLayout();
            this.TP_ServerTab1.SuspendLayout();
            this.P_TenGroup1.SuspendLayout();
            this.P_GuiMessage.SuspendLayout();
            this.P_Gui.SuspendLayout();
            this.P_Message.SuspendLayout();
            this.P_TenKenh.SuspendLayout();
            this.P_Thoat.SuspendLayout();
            this.TC_Chat.SuspendLayout();
            this.TP_ChattingView.SuspendLayout();
            this.TP_FriendRequestView.SuspendLayout();
            this.P_Friend.SuspendLayout();
            this.SuspendLayout();
            // 
            // P_Group_Friend
            // 
            this.P_Group_Friend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(36)))));
            this.P_Group_Friend.Controls.Add(this.cp_MenuFriend);
            this.P_Group_Friend.Controls.Add(this.flp_Group);
            this.P_Group_Friend.Dock = System.Windows.Forms.DockStyle.Left;
            this.P_Group_Friend.Location = new System.Drawing.Point(0, 0);
            this.P_Group_Friend.Name = "P_Group_Friend";
            this.P_Group_Friend.Size = new System.Drawing.Size(93, 474);
            this.P_Group_Friend.TabIndex = 0;
            // 
            // cp_MenuFriend
            // 
            this.cp_MenuFriend.filepath = "";
            this.cp_MenuFriend.Image = global::QLUSER.Properties.Resources._379512_chat_icon;
            this.cp_MenuFriend.Location = new System.Drawing.Point(23, 5);
            this.cp_MenuFriend.Margin = new System.Windows.Forms.Padding(2);
            this.cp_MenuFriend.Name = "cp_MenuFriend";
            this.cp_MenuFriend.Size = new System.Drawing.Size(51, 53);
            this.cp_MenuFriend.TabIndex = 0;
            this.cp_MenuFriend.TabStop = false;
            this.cp_MenuFriend.TextColor = System.Drawing.Color.Black;
            this.cp_MenuFriend.TextFont = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.cp_MenuFriend.Click += new System.EventHandler(this.cp_Menu_Click);
            // 
            // flp_Group
            // 
            this.flp_Group.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.flp_Group.AutoScroll = true;
            this.flp_Group.AutoScrollMinSize = new System.Drawing.Size(1, 1);
            this.flp_Group.Location = new System.Drawing.Point(0, 63);
            this.flp_Group.Name = "flp_Group";
            this.flp_Group.Size = new System.Drawing.Size(93, 410);
            this.flp_Group.TabIndex = 7;
            // 
            // P_Kenh_DanhMuc_User
            // 
            this.P_Kenh_DanhMuc_User.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.P_Kenh_DanhMuc_User.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(57)))), ((int)(((byte)(62)))));
            this.P_Kenh_DanhMuc_User.Controls.Add(this.P_TTinUser);
            this.P_Kenh_DanhMuc_User.Controls.Add(this.TC_ServerOrFriend);
            this.P_Kenh_DanhMuc_User.Location = new System.Drawing.Point(93, 0);
            this.P_Kenh_DanhMuc_User.Name = "P_Kenh_DanhMuc_User";
            this.P_Kenh_DanhMuc_User.Size = new System.Drawing.Size(190, 474);
            this.P_Kenh_DanhMuc_User.TabIndex = 6;
            // 
            // P_TTinUser
            // 
            this.P_TTinUser.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.P_TTinUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(43)))), ((int)(((byte)(48)))));
            this.P_TTinUser.Controls.Add(this.cp_ProfilePic);
            this.P_TTinUser.Controls.Add(this.btn_CDuser);
            this.P_TTinUser.Location = new System.Drawing.Point(0, 409);
            this.P_TTinUser.Margin = new System.Windows.Forms.Padding(2);
            this.P_TTinUser.Name = "P_TTinUser";
            this.P_TTinUser.Size = new System.Drawing.Size(190, 65);
            this.P_TTinUser.TabIndex = 9;
            // 
            // cp_ProfilePic
            // 
            this.cp_ProfilePic.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(43)))), ((int)(((byte)(48)))));
            this.cp_ProfilePic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cp_ProfilePic.filepath = "";
            this.cp_ProfilePic.ImageLocation = "";
            this.cp_ProfilePic.Location = new System.Drawing.Point(2, 5);
            this.cp_ProfilePic.Margin = new System.Windows.Forms.Padding(2);
            this.cp_ProfilePic.Name = "cp_ProfilePic";
            this.cp_ProfilePic.Size = new System.Drawing.Size(59, 56);
            this.cp_ProfilePic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.cp_ProfilePic.TabIndex = 5;
            this.cp_ProfilePic.TabStop = false;
            this.cp_ProfilePic.TextColor = System.Drawing.Color.Black;
            this.cp_ProfilePic.TextFont = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            // 
            // btn_CDuser
            // 
            this.btn_CDuser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(43)))), ((int)(((byte)(48)))));
            this.btn_CDuser.BackgroundImage = global::QLUSER.Properties.Resources.settings_24dp_5F6368_FILL0_wght400_GRAD0_opsz24;
            this.btn_CDuser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_CDuser.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_CDuser.FlatAppearance.BorderColor = System.Drawing.Color.IndianRed;
            this.btn_CDuser.FlatAppearance.BorderSize = 0;
            this.btn_CDuser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_CDuser.Location = new System.Drawing.Point(148, 18);
            this.btn_CDuser.Name = "btn_CDuser";
            this.btn_CDuser.Size = new System.Drawing.Size(35, 37);
            this.btn_CDuser.TabIndex = 5;
            this.btn_CDuser.UseVisualStyleBackColor = true;
            this.btn_CDuser.Click += new System.EventHandler(this.button4_Click);
            // 
            // TC_ServerOrFriend
            // 
            this.TC_ServerOrFriend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TC_ServerOrFriend.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.TC_ServerOrFriend.Controls.Add(this.TP_FriendsTab);
            this.TC_ServerOrFriend.Controls.Add(this.TP_ServerTab);
            this.TC_ServerOrFriend.Controls.Add(this.TP_ServerTab1);
            this.TC_ServerOrFriend.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.TC_ServerOrFriend.ItemSize = new System.Drawing.Size(0, 1);
            this.TC_ServerOrFriend.Location = new System.Drawing.Point(-7, -7);
            this.TC_ServerOrFriend.Margin = new System.Windows.Forms.Padding(2);
            this.TC_ServerOrFriend.Name = "TC_ServerOrFriend";
            this.TC_ServerOrFriend.SelectedIndex = 0;
            this.TC_ServerOrFriend.Size = new System.Drawing.Size(202, 428);
            this.TC_ServerOrFriend.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.TC_ServerOrFriend.TabIndex = 0;
            // 
            // TP_FriendsTab
            // 
            this.TP_FriendsTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(57)))), ((int)(((byte)(62)))));
            this.TP_FriendsTab.Controls.Add(this.flp_TinNhanTT);
            this.TP_FriendsTab.Controls.Add(this.lb_TinNhanTT);
            this.TP_FriendsTab.Controls.Add(this.btn_AddGroupPrivate);
            this.TP_FriendsTab.Controls.Add(this.btn_Banbe);
            this.TP_FriendsTab.Location = new System.Drawing.Point(4, 5);
            this.TP_FriendsTab.Margin = new System.Windows.Forms.Padding(2);
            this.TP_FriendsTab.Name = "TP_FriendsTab";
            this.TP_FriendsTab.Padding = new System.Windows.Forms.Padding(2);
            this.TP_FriendsTab.Size = new System.Drawing.Size(194, 419);
            this.TP_FriendsTab.TabIndex = 1;
            this.TP_FriendsTab.Text = "tabPage4";
            // 
            // flp_TinNhanTT
            // 
            this.flp_TinNhanTT.AutoScroll = true;
            this.flp_TinNhanTT.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flp_TinNhanTT.Location = new System.Drawing.Point(9, 113);
            this.flp_TinNhanTT.Name = "flp_TinNhanTT";
            this.flp_TinNhanTT.Padding = new System.Windows.Forms.Padding(10, 0, 0, 10);
            this.flp_TinNhanTT.Size = new System.Drawing.Size(174, 285);
            this.flp_TinNhanTT.TabIndex = 2;
            this.flp_TinNhanTT.WrapContents = false;
            // 
            // lb_TinNhanTT
            // 
            this.lb_TinNhanTT.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_TinNhanTT.ForeColor = System.Drawing.Color.Transparent;
            this.lb_TinNhanTT.Location = new System.Drawing.Point(17, 82);
            this.lb_TinNhanTT.Name = "lb_TinNhanTT";
            this.lb_TinNhanTT.Size = new System.Drawing.Size(135, 25);
            this.lb_TinNhanTT.TabIndex = 1;
            this.lb_TinNhanTT.Text = "Tin nhắn trực tiếp";
            this.lb_TinNhanTT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_AddGroupPrivate
            // 
            this.btn_AddGroupPrivate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_AddGroupPrivate.Location = new System.Drawing.Point(158, 82);
            this.btn_AddGroupPrivate.Name = "btn_AddGroupPrivate";
            this.btn_AddGroupPrivate.Size = new System.Drawing.Size(25, 25);
            this.btn_AddGroupPrivate.TabIndex = 0;
            this.btn_AddGroupPrivate.Text = "+";
            this.btn_AddGroupPrivate.UseVisualStyleBackColor = true;
            this.btn_AddGroupPrivate.Click += new System.EventHandler(this.button10_Click_1);
            // 
            // btn_Banbe
            // 
            this.btn_Banbe.Location = new System.Drawing.Point(37, 26);
            this.btn_Banbe.Name = "btn_Banbe";
            this.btn_Banbe.Size = new System.Drawing.Size(120, 25);
            this.btn_Banbe.TabIndex = 0;
            this.btn_Banbe.Text = "Bạn bè";
            this.btn_Banbe.UseVisualStyleBackColor = true;
            this.btn_Banbe.Click += new System.EventHandler(this.btn_Banbe_Click);
            // 
            // TP_ServerTab
            // 
            this.TP_ServerTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(57)))), ((int)(((byte)(62)))));
            this.TP_ServerTab.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.TP_ServerTab.Controls.Add(this.P_Kenh_DanhMuc);
            this.TP_ServerTab.Controls.Add(this.P_TenGroup);
            this.TP_ServerTab.Location = new System.Drawing.Point(4, 5);
            this.TP_ServerTab.Margin = new System.Windows.Forms.Padding(2);
            this.TP_ServerTab.Name = "TP_ServerTab";
            this.TP_ServerTab.Padding = new System.Windows.Forms.Padding(2);
            this.TP_ServerTab.Size = new System.Drawing.Size(194, 419);
            this.TP_ServerTab.TabIndex = 0;
            // 
            // P_Kenh_DanhMuc
            // 
            this.P_Kenh_DanhMuc.Controls.Add(this.tv_Kenh_DanhMuc);
            this.P_Kenh_DanhMuc.Location = new System.Drawing.Point(8, 53);
            this.P_Kenh_DanhMuc.Margin = new System.Windows.Forms.Padding(2);
            this.P_Kenh_DanhMuc.Name = "P_Kenh_DanhMuc";
            this.P_Kenh_DanhMuc.Size = new System.Drawing.Size(179, 355);
            this.P_Kenh_DanhMuc.TabIndex = 11;
            // 
            // tv_Kenh_DanhMuc
            // 
            this.tv_Kenh_DanhMuc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            this.tv_Kenh_DanhMuc.ForeColor = System.Drawing.Color.White;
            this.tv_Kenh_DanhMuc.Location = new System.Drawing.Point(2, 3);
            this.tv_Kenh_DanhMuc.Name = "tv_Kenh_DanhMuc";
            this.tv_Kenh_DanhMuc.Size = new System.Drawing.Size(168, 324);
            this.tv_Kenh_DanhMuc.TabIndex = 0;
            this.tv_Kenh_DanhMuc.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // P_TenGroup
            // 
            this.P_TenGroup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(43)))), ((int)(((byte)(48)))));
            this.P_TenGroup.Controls.Add(this.lb_TenGroup);
            this.P_TenGroup.Controls.Add(this.btn_MenuGroup);
            this.P_TenGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.P_TenGroup.Location = new System.Drawing.Point(2, 2);
            this.P_TenGroup.Margin = new System.Windows.Forms.Padding(2);
            this.P_TenGroup.Name = "P_TenGroup";
            this.P_TenGroup.Size = new System.Drawing.Size(190, 45);
            this.P_TenGroup.TabIndex = 10;
            // 
            // lb_TenGroup
            // 
            this.lb_TenGroup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            this.lb_TenGroup.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lb_TenGroup.ForeColor = System.Drawing.Color.White;
            this.lb_TenGroup.Location = new System.Drawing.Point(10, 9);
            this.lb_TenGroup.Name = "lb_TenGroup";
            this.lb_TenGroup.Size = new System.Drawing.Size(147, 23);
            this.lb_TenGroup.TabIndex = 4;
            // 
            // btn_MenuGroup
            // 
            this.btn_MenuGroup.Location = new System.Drawing.Point(164, 9);
            this.btn_MenuGroup.Name = "btn_MenuGroup";
            this.btn_MenuGroup.Size = new System.Drawing.Size(25, 23);
            this.btn_MenuGroup.TabIndex = 11;
            this.btn_MenuGroup.Text = "v";
            this.btn_MenuGroup.UseVisualStyleBackColor = true;
            this.btn_MenuGroup.Click += new System.EventHandler(this.button5_Click_1);
            // 
            // TP_ServerTab1
            // 
            this.TP_ServerTab1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(57)))), ((int)(((byte)(62)))));
            this.TP_ServerTab1.Controls.Add(this.P_Kenh_DanhMuc1);
            this.TP_ServerTab1.Controls.Add(this.P_TenGroup1);
            this.TP_ServerTab1.Location = new System.Drawing.Point(4, 5);
            this.TP_ServerTab1.Margin = new System.Windows.Forms.Padding(2);
            this.TP_ServerTab1.Name = "TP_ServerTab1";
            this.TP_ServerTab1.Padding = new System.Windows.Forms.Padding(2);
            this.TP_ServerTab1.Size = new System.Drawing.Size(194, 419);
            this.TP_ServerTab1.TabIndex = 2;
            this.TP_ServerTab1.Text = "tabPage3";
            // 
            // P_Kenh_DanhMuc1
            // 
            this.P_Kenh_DanhMuc1.Location = new System.Drawing.Point(6, 51);
            this.P_Kenh_DanhMuc1.Margin = new System.Windows.Forms.Padding(2);
            this.P_Kenh_DanhMuc1.Name = "P_Kenh_DanhMuc1";
            this.P_Kenh_DanhMuc1.Size = new System.Drawing.Size(184, 357);
            this.P_Kenh_DanhMuc1.TabIndex = 12;
            // 
            // P_TenGroup1
            // 
            this.P_TenGroup1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(43)))), ((int)(((byte)(48)))));
            this.P_TenGroup1.Controls.Add(this.lb_TenGroup1);
            this.P_TenGroup1.Controls.Add(this.btn_MenuGroup1);
            this.P_TenGroup1.Dock = System.Windows.Forms.DockStyle.Top;
            this.P_TenGroup1.Location = new System.Drawing.Point(2, 2);
            this.P_TenGroup1.Margin = new System.Windows.Forms.Padding(2);
            this.P_TenGroup1.Name = "P_TenGroup1";
            this.P_TenGroup1.Size = new System.Drawing.Size(190, 45);
            this.P_TenGroup1.TabIndex = 11;
            // 
            // lb_TenGroup1
            // 
            this.lb_TenGroup1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            this.lb_TenGroup1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lb_TenGroup1.ForeColor = System.Drawing.Color.White;
            this.lb_TenGroup1.Location = new System.Drawing.Point(10, 9);
            this.lb_TenGroup1.Name = "lb_TenGroup1";
            this.lb_TenGroup1.Size = new System.Drawing.Size(147, 23);
            this.lb_TenGroup1.TabIndex = 4;
            // 
            // btn_MenuGroup1
            // 
            this.btn_MenuGroup1.Location = new System.Drawing.Point(164, 9);
            this.btn_MenuGroup1.Name = "btn_MenuGroup1";
            this.btn_MenuGroup1.Size = new System.Drawing.Size(25, 23);
            this.btn_MenuGroup1.TabIndex = 11;
            this.btn_MenuGroup1.Text = "v";
            this.btn_MenuGroup1.UseVisualStyleBackColor = true;
            this.btn_MenuGroup1.Click += new System.EventHandler(this.button1_Click);
            // 
            // P_GuiMessage
            // 
            this.P_GuiMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(69)))), ((int)(((byte)(73)))));
            this.P_GuiMessage.Controls.Add(this.btn_Emoji);
            this.P_GuiMessage.Controls.Add(this.P_Gui);
            this.P_GuiMessage.Controls.Add(this.btn_ChonAnh);
            this.P_GuiMessage.Controls.Add(this.P_Message);
            this.P_GuiMessage.Controls.Add(this.btn_ChonFile);
            this.P_GuiMessage.Controls.Add(this.lb_File_Anh);
            this.P_GuiMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.P_GuiMessage.Location = new System.Drawing.Point(2, 407);
            this.P_GuiMessage.Name = "P_GuiMessage";
            this.P_GuiMessage.Size = new System.Drawing.Size(637, 75);
            this.P_GuiMessage.TabIndex = 8;
            // 
            // btn_Emoji
            // 
            this.btn_Emoji.BackColor = System.Drawing.Color.Transparent;
            this.btn_Emoji.BackgroundImage = global::QLUSER.Properties.Resources._1F60A_color;
            this.btn_Emoji.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Emoji.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Emoji.FlatAppearance.BorderSize = 0;
            this.btn_Emoji.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Emoji.Location = new System.Drawing.Point(496, 37);
            this.btn_Emoji.Name = "btn_Emoji";
            this.btn_Emoji.Size = new System.Drawing.Size(32, 30);
            this.btn_Emoji.TabIndex = 7;
            this.btn_Emoji.UseVisualStyleBackColor = false;
            this.btn_Emoji.Click += new System.EventHandler(this.button7_Click);
            // 
            // P_Gui
            // 
            this.P_Gui.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.P_Gui.Controls.Add(this.btn_Gui);
            this.P_Gui.Location = new System.Drawing.Point(531, 37);
            this.P_Gui.Margin = new System.Windows.Forms.Padding(2);
            this.P_Gui.Name = "P_Gui";
            this.P_Gui.Size = new System.Drawing.Size(104, 34);
            this.P_Gui.TabIndex = 5;
            // 
            // btn_Gui
            // 
            this.btn_Gui.BackColor = System.Drawing.Color.Gray;
            this.btn_Gui.Location = new System.Drawing.Point(3, 3);
            this.btn_Gui.Name = "btn_Gui";
            this.btn_Gui.Size = new System.Drawing.Size(97, 30);
            this.btn_Gui.TabIndex = 1;
            this.btn_Gui.Text = "Gửi";
            this.btn_Gui.UseVisualStyleBackColor = false;
            this.btn_Gui.Click += new System.EventHandler(this.bt_guitinnhan_Click);
            // 
            // btn_ChonAnh
            // 
            this.btn_ChonAnh.BackColor = System.Drawing.Color.Transparent;
            this.btn_ChonAnh.BackgroundImage = global::QLUSER.Properties.Resources.photo_library_24dp_FFFFFF_FILL0_wght400_GRAD0_opsz24;
            this.btn_ChonAnh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_ChonAnh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_ChonAnh.FlatAppearance.BorderSize = 0;
            this.btn_ChonAnh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ChonAnh.Location = new System.Drawing.Point(18, 6);
            this.btn_ChonAnh.Name = "btn_ChonAnh";
            this.btn_ChonAnh.Size = new System.Drawing.Size(32, 30);
            this.btn_ChonAnh.TabIndex = 4;
            this.btn_ChonAnh.UseVisualStyleBackColor = false;
            this.btn_ChonAnh.Click += new System.EventHandler(this.buttonchonanh_Click);
            // 
            // P_Message
            // 
            this.P_Message.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.P_Message.Controls.Add(this.tb_Message);
            this.P_Message.Location = new System.Drawing.Point(58, 36);
            this.P_Message.Margin = new System.Windows.Forms.Padding(2);
            this.P_Message.Name = "P_Message";
            this.P_Message.Size = new System.Drawing.Size(436, 32);
            this.P_Message.TabIndex = 6;
            // 
            // tb_Message
            // 
            this.tb_Message.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            this.tb_Message.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tb_Message.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_Message.Font = new System.Drawing.Font("Segoe UI Emoji", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Message.ForeColor = System.Drawing.Color.White;
            this.tb_Message.Location = new System.Drawing.Point(0, 0);
            this.tb_Message.Multiline = true;
            this.tb_Message.Name = "tb_Message";
            this.tb_Message.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.tb_Message.Size = new System.Drawing.Size(436, 32);
            this.tb_Message.TabIndex = 0;
            this.tb_Message.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // btn_ChonFile
            // 
            this.btn_ChonFile.BackColor = System.Drawing.Color.Transparent;
            this.btn_ChonFile.BackgroundImage = global::QLUSER.Properties.Resources.attach_file_24dp_FFFFFF_FILL0_wght400_GRAD0_opsz24;
            this.btn_ChonFile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_ChonFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_ChonFile.FlatAppearance.BorderSize = 0;
            this.btn_ChonFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ChonFile.Location = new System.Drawing.Point(18, 41);
            this.btn_ChonFile.Name = "btn_ChonFile";
            this.btn_ChonFile.Size = new System.Drawing.Size(32, 30);
            this.btn_ChonFile.TabIndex = 3;
            this.btn_ChonFile.UseVisualStyleBackColor = false;
            this.btn_ChonFile.Click += new System.EventHandler(this.buttonchonfile_Click);
            // 
            // lb_File_Anh
            // 
            this.lb_File_Anh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(69)))), ((int)(((byte)(73)))));
            this.lb_File_Anh.Location = new System.Drawing.Point(56, 6);
            this.lb_File_Anh.Name = "lb_File_Anh";
            this.lb_File_Anh.Size = new System.Drawing.Size(408, 29);
            this.lb_File_Anh.TabIndex = 2;
            // 
            // flp_Message
            // 
            this.flp_Message.AutoScroll = true;
            this.flp_Message.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(69)))), ((int)(((byte)(73)))));
            this.flp_Message.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flp_Message.Location = new System.Drawing.Point(2, 53);
            this.flp_Message.Name = "flp_Message";
            this.flp_Message.Padding = new System.Windows.Forms.Padding(10, 0, 10, 10);
            this.flp_Message.Size = new System.Drawing.Size(637, 357);
            this.flp_Message.TabIndex = 7;
            this.flp_Message.WrapContents = false;
            // 
            // lb_TenKenh
            // 
            this.lb_TenKenh.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_TenKenh.ForeColor = System.Drawing.Color.White;
            this.lb_TenKenh.Location = new System.Drawing.Point(82, 10);
            this.lb_TenKenh.Name = "lb_TenKenh";
            this.lb_TenKenh.Size = new System.Drawing.Size(400, 30);
            this.lb_TenKenh.TabIndex = 3;
            // 
            // P_TenKenh
            // 
            this.P_TenKenh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(69)))), ((int)(((byte)(73)))));
            this.P_TenKenh.Controls.Add(this.btn_SearchMessage);
            this.P_TenKenh.Controls.Add(this.P_Thoat);
            this.P_TenKenh.Controls.Add(this.lb_TenKenh);
            this.P_TenKenh.Dock = System.Windows.Forms.DockStyle.Top;
            this.P_TenKenh.Location = new System.Drawing.Point(2, 2);
            this.P_TenKenh.Name = "P_TenKenh";
            this.P_TenKenh.Size = new System.Drawing.Size(637, 51);
            this.P_TenKenh.TabIndex = 0;
            // 
            // btn_SearchMessage
            // 
            this.btn_SearchMessage.BackColor = System.Drawing.Color.Transparent;
            this.btn_SearchMessage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_SearchMessage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_SearchMessage.FlatAppearance.BorderSize = 0;
            this.btn_SearchMessage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_SearchMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_SearchMessage.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.btn_SearchMessage.Location = new System.Drawing.Point(483, 10);
            this.btn_SearchMessage.Name = "btn_SearchMessage";
            this.btn_SearchMessage.Size = new System.Drawing.Size(32, 30);
            this.btn_SearchMessage.TabIndex = 8;
            this.btn_SearchMessage.Text = "🔎";
            this.btn_SearchMessage.UseVisualStyleBackColor = false;
            this.btn_SearchMessage.Click += new System.EventHandler(this.button8_Click);
            // 
            // P_Thoat
            // 
            this.P_Thoat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.P_Thoat.Controls.Add(this.btn_Thoat);
            this.P_Thoat.Location = new System.Drawing.Point(520, 10);
            this.P_Thoat.Margin = new System.Windows.Forms.Padding(2);
            this.P_Thoat.Name = "P_Thoat";
            this.P_Thoat.Size = new System.Drawing.Size(107, 30);
            this.P_Thoat.TabIndex = 11;
            // 
            // btn_Thoat
            // 
            this.btn_Thoat.BackColor = System.Drawing.Color.Gray;
            this.btn_Thoat.Location = new System.Drawing.Point(3, 0);
            this.btn_Thoat.Name = "btn_Thoat";
            this.btn_Thoat.Size = new System.Drawing.Size(97, 30);
            this.btn_Thoat.TabIndex = 4;
            this.btn_Thoat.Text = "Thoát";
            this.btn_Thoat.UseVisualStyleBackColor = false;
            this.btn_Thoat.Click += new System.EventHandler(this.bt_thoat_Click);
            // 
            // TC_Chat
            // 
            this.TC_Chat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TC_Chat.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.TC_Chat.Controls.Add(this.TP_ChattingView);
            this.TC_Chat.Controls.Add(this.TP_FriendRequestView);
            this.TC_Chat.ItemSize = new System.Drawing.Size(0, 1);
            this.TC_Chat.Location = new System.Drawing.Point(278, -11);
            this.TC_Chat.Margin = new System.Windows.Forms.Padding(2);
            this.TC_Chat.Name = "TC_Chat";
            this.TC_Chat.SelectedIndex = 0;
            this.TC_Chat.Size = new System.Drawing.Size(649, 493);
            this.TC_Chat.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.TC_Chat.TabIndex = 0;
            // 
            // TP_ChattingView
            // 
            this.TP_ChattingView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(69)))), ((int)(((byte)(73)))));
            this.TP_ChattingView.Controls.Add(this.P_GuiMessage);
            this.TP_ChattingView.Controls.Add(this.P_TenKenh);
            this.TP_ChattingView.Controls.Add(this.flp_Message);
            this.TP_ChattingView.Location = new System.Drawing.Point(4, 5);
            this.TP_ChattingView.Margin = new System.Windows.Forms.Padding(2);
            this.TP_ChattingView.Name = "TP_ChattingView";
            this.TP_ChattingView.Padding = new System.Windows.Forms.Padding(2);
            this.TP_ChattingView.Size = new System.Drawing.Size(641, 484);
            this.TP_ChattingView.TabIndex = 0;
            this.TP_ChattingView.Text = "tabPage1";
            // 
            // TP_FriendRequestView
            // 
            this.TP_FriendRequestView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(69)))), ((int)(((byte)(73)))));
            this.TP_FriendRequestView.Controls.Add(this.btn_XemListFriend);
            this.TP_FriendRequestView.Controls.Add(this.P_Friend);
            this.TP_FriendRequestView.Controls.Add(this.btn_XemLoiMoi);
            this.TP_FriendRequestView.Controls.Add(this.btn_Ketban);
            this.TP_FriendRequestView.Location = new System.Drawing.Point(4, 5);
            this.TP_FriendRequestView.Margin = new System.Windows.Forms.Padding(2);
            this.TP_FriendRequestView.Name = "TP_FriendRequestView";
            this.TP_FriendRequestView.Padding = new System.Windows.Forms.Padding(2);
            this.TP_FriendRequestView.Size = new System.Drawing.Size(641, 484);
            this.TP_FriendRequestView.TabIndex = 1;
            this.TP_FriendRequestView.Text = "tabPage2";
            // 
            // btn_XemListFriend
            // 
            this.btn_XemListFriend.Location = new System.Drawing.Point(281, 17);
            this.btn_XemListFriend.Margin = new System.Windows.Forms.Padding(2);
            this.btn_XemListFriend.Name = "btn_XemListFriend";
            this.btn_XemListFriend.Size = new System.Drawing.Size(146, 19);
            this.btn_XemListFriend.TabIndex = 5;
            this.btn_XemListFriend.Text = "Xem danh sách bạn bè";
            this.btn_XemListFriend.UseVisualStyleBackColor = true;
            this.btn_XemListFriend.Click += new System.EventHandler(this.btn_XemListFriend_Click);
            // 
            // P_Friend
            // 
            this.P_Friend.Controls.Add(this.flp_Friends);
            this.P_Friend.Location = new System.Drawing.Point(6, 62);
            this.P_Friend.Margin = new System.Windows.Forms.Padding(2);
            this.P_Friend.Name = "P_Friend";
            this.P_Friend.Size = new System.Drawing.Size(610, 393);
            this.P_Friend.TabIndex = 3;
            // 
            // flp_Friends
            // 
            this.flp_Friends.AutoScroll = true;
            this.flp_Friends.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flp_Friends.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flp_Friends.Location = new System.Drawing.Point(0, 0);
            this.flp_Friends.Margin = new System.Windows.Forms.Padding(2);
            this.flp_Friends.Name = "flp_Friends";
            this.flp_Friends.Size = new System.Drawing.Size(610, 393);
            this.flp_Friends.TabIndex = 0;
            // 
            // btn_XemLoiMoi
            // 
            this.btn_XemLoiMoi.Location = new System.Drawing.Point(135, 17);
            this.btn_XemLoiMoi.Margin = new System.Windows.Forms.Padding(2);
            this.btn_XemLoiMoi.Name = "btn_XemLoiMoi";
            this.btn_XemLoiMoi.Size = new System.Drawing.Size(128, 19);
            this.btn_XemLoiMoi.TabIndex = 1;
            this.btn_XemLoiMoi.Text = "Xem lời mời kết bạn";
            this.btn_XemLoiMoi.UseVisualStyleBackColor = true;
            this.btn_XemLoiMoi.Click += new System.EventHandler(this.button2_Click);
            // 
            // btn_Ketban
            // 
            this.btn_Ketban.Location = new System.Drawing.Point(12, 17);
            this.btn_Ketban.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Ketban.Name = "btn_Ketban";
            this.btn_Ketban.Size = new System.Drawing.Size(102, 19);
            this.btn_Ketban.TabIndex = 0;
            this.btn_Ketban.Text = "Kết bạn";
            this.btn_Ketban.UseVisualStyleBackColor = true;
            this.btn_Ketban.Click += new System.EventHandler(this.btn_Ketban_Click);
            // 
            // GiaoDien
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 474);
            this.Controls.Add(this.P_Kenh_DanhMuc_User);
            this.Controls.Add(this.P_Group_Friend);
            this.Controls.Add(this.TC_Chat);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "GiaoDien";
            this.Text = "GiaoDien";
            this.Load += new System.EventHandler(this.GiaoDien_Load);
            this.P_Group_Friend.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cp_MenuFriend)).EndInit();
            this.P_Kenh_DanhMuc_User.ResumeLayout(false);
            this.P_TTinUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cp_ProfilePic)).EndInit();
            this.TC_ServerOrFriend.ResumeLayout(false);
            this.TP_FriendsTab.ResumeLayout(false);
            this.TP_ServerTab.ResumeLayout(false);
            this.P_Kenh_DanhMuc.ResumeLayout(false);
            this.P_TenGroup.ResumeLayout(false);
            this.TP_ServerTab1.ResumeLayout(false);
            this.P_TenGroup1.ResumeLayout(false);
            this.P_GuiMessage.ResumeLayout(false);
            this.P_Gui.ResumeLayout(false);
            this.P_Message.ResumeLayout(false);
            this.P_Message.PerformLayout();
            this.P_TenKenh.ResumeLayout(false);
            this.P_Thoat.ResumeLayout(false);
            this.TC_Chat.ResumeLayout(false);
            this.TP_ChattingView.ResumeLayout(false);
            this.TP_FriendRequestView.ResumeLayout(false);
            this.P_Friend.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel P_Group_Friend;
        private System.Windows.Forms.Panel P_Kenh_DanhMuc_User;
        private Label lb_TenGroup;
        private Panel P_GuiMessage;
        private Button btn_Gui;
        private TextBox tb_Message;
        private FlowLayoutPanel flp_Message;
        private Label lb_TenKenh;
        private Panel P_TenKenh;
        private Button btn_Thoat;
        private Label lb_File_Anh;
        private Button btn_ChonFile;
        private Button btn_ChonAnh;
        private Button btn_CDuser;
        private Models.CircularPicture cp_ProfilePic;
        private Panel P_TTinUser;
        private TreeView tv_Kenh_DanhMuc;
        private Button btn_MenuGroup;
        private Panel P_TenGroup;
        private FlowLayoutPanel flp_Group;
        private Models.CircularPicture cp_MenuFriend;
        private Panel P_Thoat;
        private Panel P_Gui;
        private Panel P_Message;
        private TabControl TC_Chat;
        private TabPage TP_ChattingView;
        private TabPage TP_FriendRequestView;
        private TabControl TC_ServerOrFriend;
        private TabPage TP_ServerTab;
        private TabPage TP_FriendsTab;
        private TabPage TP_ServerTab1;
        private Panel P_TenGroup1;
        private Label lb_TenGroup1;
        private Button btn_MenuGroup1;
        private Panel P_Kenh_DanhMuc1;
        private Panel P_Kenh_DanhMuc;
        private Button btn_Ketban;
        private Button btn_XemLoiMoi;
        private Button btn_Emoji;
        private Button btn_SearchMessage;
        private FlowLayoutPanel flp_TinNhanTT;
        private Label lb_TinNhanTT;
        private Button btn_AddGroupPrivate;
        private Button btn_Banbe;
        private Button btn_XemListFriend;
        private Panel P_Friend;
        private FlowLayoutPanel flp_Friends;
    }
}