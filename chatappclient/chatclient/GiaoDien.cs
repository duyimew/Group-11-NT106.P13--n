using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Reflection;
using QLUSER.Models;
using chatclient.DTOs.User;
using chatclient.DTOs.Group;
using chatclient.DTOs.Channel;
using chatclient.DTOs.Danhmuc;
using chatclient.DTOs.Message;
using System.Threading;
using System.Net.Http;
using Microsoft.AspNetCore.SignalR.Client;
using System.Configuration;
using AForge.Video;
using NAudio.Wave;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.ApplicationServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Runtime.ConstrainedExecution;
using System.Runtime.CompilerServices;
using chatclient.DTOs.Friends;

namespace QLUSER
{
    public partial class GiaoDien : Form
    {
        Dangnhap DN;
        private FlowLayoutPanel panelMenu;

        Group _group = new Group();
        Channel _channel = new Channel();
        Models.Message _message = new Models.Message();
        Models.User _user = new Models.User();
        file _file = new file();
        Token _token = new Token();
        Find _find = new Find();
        UserAvatar _avatar = new UserAvatar();
        DanhMuc _danhmuc = new DanhMuc();
        GroupMember _groupMember = new GroupMember();

        string _username = "";
        string _dpname = "";
        string _userid = "";
        string _gdpname = "";
        string _userrole = "";
        string _groupid = "";
        string _groupname = "";
        string _channelid = "";
        string _channelname = "";
        string _messageid = "";
        string _danhmucid = "";
        bool grouppublicorprivate = true;
        private bool AddGroupMembers = false;
        private List<string> selectedFilePaths = new List<string>();
        private HashSet<string> displayedMessages = new HashSet<string>();

        private bool isReceivingMessages = true;
        public HubConnection connection = null;
        private bool isstop = false;
        private bool isRecevie = false;
        public GiaoDien(string username, string userid, Dangnhap dn)
        {
            InitializeComponent();
            InitializeSignalR();
            UserSession.InitializeSynchronizationContext();
            _username = username;
            _userid = userid;
            DN = dn;
            UserSession.ActionUpdateusername += UpdateUsername;
            UserSession.ActionAvatarUpdated += UpdateAvatarDisplay;
            UserSession.ActionUpdateGroup += UpdateGroupDislay;
            UserSession.ActionUpdateGroupPrivate += UpdateGroupPrivateDislay;
            UserSession.ActionDeleteuser += UpdateGroupPrivateDislay;
            /*UserSession.ActionAvatarUpdated += () => HandleUpdate("AvatarDisplay");
            UserSession.ActionUpdateGroup += () => HandleUpdate("GroupDislay");
            UserSession.ActionUpdateGroupPrivate += () => HandleUpdate("GroupPrivateDislay");
            UserSession.ActionAvatarGroupUpdated += () => HandleUpdate("AvatarGroupUpdated");
            UserSession.ActionDeleteuser += () => HandleUpdate("Deleteuser");
            UserSession.ActionUpdateFriend += () => HandleUpdate("UpdateFriend");
            UserSession.ActionUpdateFriendRequest += () => HandleUpdate("UpdateFriendRequest");
            UserSession.ActionUpdategdpname += () => HandleUpdate("Updategdpname");
            UserSession.ActionUpdateGroupMember += () => HandleUpdate("UpdateGroupMember");
            UserSession.ActionUpdateChannel += () => HandleUpdate("UpdateChannel");
            UserSession.ActionUpdateChannelname += () => HandleUpdate("UpdateChannelname");
            UserSession.ActionUpdateDanhMuc += () => HandleUpdate("UpdateDanhMuc");
            UserSession.ActionUpdatedanhmucname += () => HandleUpdate("Updatedanhmucname");
            UserSession.ActionUpdateGroupMemberPrivate += () => HandleUpdate("UpdateGroupMemberPrivate");
            UserSession.ActionUpdateGroupname += () => HandleUpdate("UpdateGroupname");
            UserSession.ActionUpdateMessage += () => HandleUpdate("UpdateMessage");
            UserSession.ActionUpdateMessageText += () => HandleUpdate("UpdateMessageText");
            UserSession.ActionUpdateRole += () => HandleUpdate("UpdateRole");
            UserSession.ActionUpdatedpname += () => HandleUpdate("dpname");*/
            TC_Chat.SelectedIndex = 1;
        }
        private async void GiaoDien_Load(object sender, EventArgs e)
        {
            try
            {
                _dpname = await _user.FindDisplayname(_userid);
                UserSession.ActionUpdatedpname += Updatedpname;
                tv_Kenh_DanhMuc.NodeMouseClick += treeView1_NodeMouseClick;
                UpdateGroupPrivateDislay();
                UpdateGroupDislay();
                UpdateAvatarDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in GiaoDien_Load: {ex.Message}");
            }
        }

        public async void SendUpdate(string updateType)
        {
            try
            {
                while (true)
                {
                    if (connection != null && connection.State == HubConnectionState.Connected)
                    {
                        await connection.SendAsync($"Send{updateType}", true);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in SendUpdate: {ex.Message}");
            }
        }

        private async void UpdateUsername()
        {
            try
            {
                _username = await _user.FindUsername(_userid);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in UpdateUsername: {ex.Message}");
            }
        }

        private async void Updatedpname()
        {
            try
            {
                _dpname = await _user.FindDisplayname(_userid);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in Updatedpname: {ex.Message}");
            }
        }

        private async void UpdateAvatarDisplay()
        {
            try
            {
                UserSession.RunOnUIThread(new Action(async () =>
                {
                    try
                    {
                        cp_ProfilePic.Image = await _avatar.LoadAvatarAsync(_userid);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error in UpdateAvatarDisplay (RunOnUIThread): {ex.Message}");
                    }
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in UpdateAvatarDisplay: {ex.Message}");
            }
        }

        private async void UpdateGroupDislay()
        {
            try
            {
                UserSession.RunOnUIThread(new Action(async () =>
                {
                    try
                    {
                        flp_Group.Controls.Clear();
                        flp_Group.AutoScrollPosition = new Point(0, 0);
                        flp_Group.AutoScroll = true;
                        var result = await _group.RequestGroupName(_userid, 0);
                        if (result.issuccess)
                            LoadGroup(result.groupidname);
                        flp_Group.PerformLayout();
                        TC_Chat.SelectedIndex = 1;
                        TC_ServerOrFriend.SelectedIndex = 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error in UpdateGroupDislay (RunOnUIThread): {ex.Message}");
                    }
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in UpdateGroupDislay: {ex.Message}");
            }
        }

        private async void UpdateGroupPrivateDislay()
        {
            try
            {
                UserSession.RunOnUIThread(new Action(async () =>
                {
                    try
                    {
                        flp_TinNhanTT.Controls.Clear();
                        flp_TinNhanTT.AutoScrollPosition = new Point(0, 0);
                        flp_TinNhanTT.AutoScroll = true;
                        var result = await _group.RequestGroupName(_userid, 2);
                        if (result.issuccess)
                            LoadGroupPrivate(result.groupidname, 2);
                        var result1 = await _group.RequestGroupName(_userid, 1);
                        if (result1.issuccess)
                            LoadGroupPrivate(result1.groupidname, 1);
                        flp_TinNhanTT.PerformLayout();
                        TC_Chat.SelectedIndex = 1;
                        TC_ServerOrFriend.SelectedIndex = 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error in UpdateGroupPrivateDislay (RunOnUIThread): {ex.Message}");
                    }
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in UpdateGroupPrivateDislay: {ex.Message}");
            }
        }

        protected override async void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                UserSession.ActionUpdateusername -= UpdateUsername;
                UserSession.ActionAvatarUpdated -= UpdateAvatarDisplay;
                UserSession.ActionUpdateGroup -= UpdateGroupDislay;
                UserSession.ActionUpdateGroupPrivate -= UpdateGroupPrivateDislay;
                UserSession.ActionUpdatedpname -= Updatedpname;
                base.OnFormClosing(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in OnFormClosing: {ex.Message}");
            }
        }

        private async void bt_thoat_Click(object sender, EventArgs e)
        {
            try
            {
                if (connection != null)
                    await StopSignalR();

                await _token.GenerateToken(_username);
                DN.Close();
                Environment.Exit(0);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in bt_thoat_Click: {ex.Message}");
            }
        }

        private async void LoadGroupPrivate(string[] groupname, int privatenumber)
        {
            try
            {
                if (groupname != null)
                {
                    for (int i = 0; i < groupname.Length; i++)
                    {
                        string[] group1 = groupname[i].Split('|');
                        string[] groupmember = await _groupMember.FindUserRoleNameId(group1[0]);
                        foreach (var member in groupmember)
                        {
                            Panel panel = new Panel();
                            panel.Width = flp_TinNhanTT.Width;
                            panel.Height = 50;
                            panel.Margin = new Padding(10, 0, 0, 0);
                            panel.Name = privatenumber.ToString();
                            string[] user = member.Split('|');
                            if (user[0] != _userid)
                            {
                                CircularPicture useravatar = new CircularPicture();
                                if (privatenumber == 2)
                                {
                                    try
                                    {
                                        var loadedImage = await _avatar.LoadAvatarAsync(user[0]);
                                        if (loadedImage != null)
                                        {
                                            useravatar.Image = loadedImage;
                                        }
                                        else
                                        {
                                            MessageBox.Show("Không tải được ảnh, kiểm tra đường dẫn hoặc server.", "Lỗi");
                                        }
                                        useravatar.SizeMode = PictureBoxSizeMode.Zoom;
                                        useravatar.Text = group1[1];
                                        useravatar.Name = $"{group1[0]}";
                                        UserSession.ActionAvatarUpdated += async () =>
                                        {
                                            useravatar.Image = await _avatar.LoadAvatarAsync(user[0]);
                                        };
                                        UserSession.ActionUpdateGroupname += async () =>
                                        {
                                            group1[1] = await _group.Groupname(group1[0]);
                                            useravatar.Text = group1[1];
                                        };
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show($"Không thể tải ảnh: {ex.Message}");
                                        return;
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        var loadedImage = await _avatar.LoadAvatarGroupAsync(group1[0]);
                                        if (loadedImage != null)
                                        {
                                            useravatar.Image = loadedImage;
                                        }
                                        else
                                        {
                                            MessageBox.Show("Không tải được ảnh, kiểm tra đường dẫn hoặc server.", "Lỗi");
                                        }
                                        useravatar.SizeMode = PictureBoxSizeMode.Zoom;
                                        useravatar.Text = group1[1];
                                        useravatar.Name = $"{group1[0]}";
                                        UserSession.ActionAvatarGroupUpdated += async () =>
                                        {
                                            useravatar.Image = await _avatar.LoadAvatarGroupAsync(group1[0]);
                                        };
                                        UserSession.ActionUpdateGroupname += async () =>
                                        {
                                            group1[1] = await _group.Groupname(group1[0]);
                                            useravatar.Text = group1[1];
                                        };
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show($"Không thể tải ảnh: {ex.Message}");
                                        return;
                                    }
                                }
                                useravatar.Location = new Point(0, 10);
                                useravatar.Size = new Size(25, 25);
                                useravatar.Margin = new Padding(5);
                                useravatar.Click += async (s, e) =>
                                {
                                    if (privatenumber == 2) button1.Visible = false;
                                    else button1.Visible = true;
                                    if (AddGroupMembers)
                                    {
                                        button1_Click_1(null, EventArgs.Empty);
                                    }
                                    panel_click(group1, user);
                                };
                                Label label = new Label();
                                label.Text = $"{user[1]}";
                                UserSession.ActionUpdategdpname += async () =>
                                {
                                    user[1] = await _groupMember.FindGroupDisplayname(user[0], group1[0]);
                                    label.Text = user[1];
                                };
                                label.Name = $"{user[0]}";
                                label.Font = new Font("Arial", 12, FontStyle.Bold);
                                label.ForeColor = Color.White;
                                label.Location = new Point(40, 15);
                                label.AutoSize = true;
                                label.TextAlign = ContentAlignment.MiddleCenter;
                                label.Click += async (s, e) =>
                                {
                                    if (privatenumber == 2) button1.Visible = false;
                                    else button1.Visible = true;
                                    if (AddGroupMembers)
                                    {
                                        button1_Click_1(null, EventArgs.Empty);
                                    }
                                    panel_click(group1, user);
                                };
                                Button deletegroup = new Button();
                                deletegroup.Text = "X";
                                deletegroup.Name = $"{user[0]}";
                                deletegroup.Font = new Font("Arial", 10, FontStyle.Bold);
                                deletegroup.ForeColor = Color.White;
                                deletegroup.Size = new Size(25, 25);
                                deletegroup.PerformLayout();
                                deletegroup.Location = new Point(panel.Width - deletegroup.Width - 20, 10);
                                deletegroup.TextAlign = ContentAlignment.MiddleCenter;
                                deletegroup.Click += async (s, e) =>
                                {
                                    await roinhom(useravatar.Name, false, _userid, 1);
                                };
                                Button addmember = new Button();
                                addmember.Text = "+";
                                addmember.Name = $"{user[0]}";
                                addmember.Font = new Font("Arial", 10, FontStyle.Bold);
                                addmember.ForeColor = Color.White;
                                addmember.Size = new Size(25, 25);
                                addmember.PerformLayout();
                                addmember.Location = new Point(panel.Width - deletegroup.Width - addmember.Width - 20, 10);
                                addmember.TextAlign = ContentAlignment.MiddleCenter;
                                addmember.Click += async (s, e) =>
                                {
                                    bool canclose = true;
                                    Form form = new Form();
                                    form.Text = $"Chọn bạn bè";
                                    form.Size = new Size(500, 350);
                                    form.StartPosition = FormStartPosition.CenterParent;

                                    form.Deactivate += (s1, e1) =>
                                    {
                                        if (canclose)
                                            form.Close();
                                    };


                                    Label label1 = new Label();
                                    label1.Text = $"Chọn bạn bè";
                                    label1.Font = new Font("Arial", 12, FontStyle.Bold);
                                    label1.ForeColor = Color.Black;
                                    label1.AutoSize = true;
                                    label1.PerformLayout();
                                    int labelWidth1 = TextRenderer.MeasureText(label1.Text, label1.Font).Width;
                                    label1.Location = new Point(20, 20);
                                    label1.TextAlign = ContentAlignment.MiddleCenter;


                                    FlowLayoutPanel panel1 = new FlowLayoutPanel();
                                    panel1.Location = new Point(20, 100);
                                    panel1.Size = new Size(440, 100);
                                    panel1.FlowDirection = FlowDirection.TopDown;  // Arrange buttons vertically
                                    panel1.WrapContents = false;  // Don't wrap buttons to next row
                                    panel1.AutoScroll = true; // Enable scrolling if buttons overflow

                                    using (HttpClient _httpClient = new HttpClient())
                                    {
                                        try
                                        {

                                            flp_Friends.Controls.Clear();

                                            string requestUrl = $"{ConfigurationManager.AppSettings["ServerUrl"]}Friend/{_userid}";

                                            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

                                            if (response.IsSuccessStatusCode)
                                            {
                                                string responseContent = await response.Content.ReadAsStringAsync();
                                                var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);

                                                if (responseData != null && responseData.message != null)
                                                {
                                                    foreach (string displayName in responseData.message)
                                                    {
                                                        string userid = await _user.finduserid(displayName);
                                                        string isjoined = await _groupMember.FindJointime(userid, useravatar.Name);
                                                        if (isjoined != null) continue;
                                                        Image avatarImage = await _avatar.LoadAvatarAsync(userid);
                                                        Panel friendPanel = new Panel
                                                        {
                                                            Width = panel.Width - 20,
                                                            Height = 30,
                                                            BorderStyle = BorderStyle.FixedSingle,
                                                            AutoSize = true,
                                                        };
                                                        CheckBox usernamebox = new CheckBox
                                                        {
                                                            Checked = false,
                                                            Text = displayName,
                                                            Name = userid,
                                                            AutoSize = true,
                                                            Location = new Point(45, 10),
                                                            ForeColor = Color.Black,
                                                            TextAlign = ContentAlignment.MiddleCenter,
                                                            CheckAlign = ContentAlignment.MiddleRight,
                                                        };
                                                        UserSession.ActionUpdatedpname += async () =>
                                                        {

                                                            usernamebox.Text = await _user.FindDisplayname(userid);
                                                        };
                                                        usernamebox.PerformLayout();
                                                        PictureBox avatarBox = new PictureBox
                                                        {
                                                            Image = avatarImage,
                                                            Width = 25,
                                                            Height = 25,
                                                            SizeMode = PictureBoxSizeMode.Zoom,
                                                            Location = new Point(5, 5)
                                                        };
                                                        UserSession.ActionAvatarUpdated += async () =>
                                                        {
                                                            avatarBox.Image = await _avatar.LoadAvatarAsync(userid);
                                                        };

                                                        friendPanel.Controls.Add(usernamebox);
                                                        friendPanel.Controls.Add(avatarBox);
                                                        // Thêm Panel vào FlowLayoutPanel
                                                        panel1.Controls.Add(friendPanel);
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show("Không có dữ liệu bạn bè trả về.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                }
                                            }
                                            else
                                            {
                                                // Hiển thị lỗi từ server nếu có
                                                string errorContent = await response.Content.ReadAsStringAsync();
                                                MessageBox.Show($"Lỗi từ server: {errorContent}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            // Xử lý lỗi trong quá trình gửi request hoặc hiển thị danh sách
                                            MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }


                                    Button thongtin = new Button();
                                    thongtin.Text = "Thêm người dùng";
                                    thongtin.Location = new Point(20, 250);
                                    thongtin.Size = new Size(440, 30);
                                    thongtin.Click += async (s1, e1) =>
                                    {
                                        canclose = false;
                                        List<CheckBox> list = new List<CheckBox>();
                                        foreach (Control control in panel1.Controls)
                                        {
                                            if (control is Panel friendPanel)
                                            {
                                                foreach (Control subControl in friendPanel.Controls)
                                                {
                                                    if (subControl is CheckBox checkBox && checkBox.Checked)
                                                    {
                                                        list.Add(checkBox);
                                                    }
                                                }
                                            }
                                        }
                                        var result3 = await _groupMember.FindUserRoleNameId(useravatar.Name);
                                        if (list.Count <= 10 - result3.Length && list.Count > 0)
                                        {
                                            foreach (var user1 in list)
                                            {
                                                var result2 = await _groupMember.AddMembersToGroup(user1.Name, useravatar.Name, user1.Text, "Member");
                                            }
                                            UserSession.UpdateGroupMember = (true, false);
                                            SendUpdate("UpdateGroupMemberPrivate");
                                            form.Dispose();
                                            form.Close();
                                        }
                                        else
                                        {
                                            MessageBox.Show($"Nhóm tối đa 10 người. Xin vui lòng chọn dưới {10 - result3.Length} người");
                                        }
                                        canclose = true;
                                    };

                                    form.Controls.Add(label1);

                                    form.Controls.Add(panel1);
                                    form.Controls.Add(thongtin);


                                    form.ShowDialog();
                                };

                                panel.Click += async (s, e) =>
                                {
                                    if (privatenumber == 2) button1.Visible = false;
                                    else button1.Visible = true;
                                    if(AddGroupMembers)
                                    {
                                        button1_Click_1(null, EventArgs.Empty);
                                    }
                                    panel_click(group1, user);
                                    
                                };
                                panel.Controls.Add(useravatar); // Thêm avatar trước
                                panel.Controls.Add(label);      // Thêm label sau
                                panel.Controls.Add(deletegroup);
                                if (privatenumber == 1) panel.Controls.Add(addmember);
                                flp_TinNhanTT.Controls.Add(panel);
                                break;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void panel_click(string[] group1, string[] user)
        {
            TC_Chat.SelectedIndex = 0;
            
            while (true)
            {
                if (connection != null && connection.State == HubConnectionState.Connected)
                {
                    await connection.SendAsync("LeaveGroup", _channelid, _gdpname);
                    break;
                }
            }
            grouppublicorprivate = false;
            displayedMessages.Clear();
            flp_Message.Controls.Clear();
            flp_Message.AutoScrollPosition = new Point(0, 0);
            flp_Message.PerformLayout();
            _groupid = group1[0];
            _groupname = group1[1];
            _userrole = await _groupMember.FindOneUserRole(_groupid, _userid);
            UserSession.ActionUpdateRole += async () =>
            {
                _userrole = await _groupMember.FindOneUserRole(_groupid, _userid);
            };
            _gdpname = await _groupMember.FindGroupDisplayname(_userid, _groupid);
            UserSession.ActionUpdategdpname += async () =>
            {
                _gdpname = await _groupMember.FindGroupDisplayname(_userid, _groupid);
            };
            var channel1 = await _channel.RequestChannelName(_groupid);
            string[] channelid = channel1.channelidname[0].Split('|');
            _channelid = channelid[0];
            _channelname = channelid[1];
            UserSession.ActionUpdateChannelname += async () =>
            {
                var channel = await _channel.RequestoneChannelName(_channelid);
                _channelname = channel.channelname;
            };
            lb_TenGroup.Text = group1[1];
            UserSession.ActionUpdateGroupname += async () =>
            {
                _groupname = await _group.Groupname(_groupid);
                lb_TenGroup.Text = _groupname;
            };
            lb_TenGroup.Name = _groupid;

            lb_TenKenh.Text = user[1];
            UserSession.ActionUpdategdpname += async () =>
            {
                lb_TenKenh.Text = await _groupMember.FindGroupDisplayname(user[0], _groupid);
            };

            lb_TenKenh.Name = _channelid;
            var result = await _message.ReceiveMessage(_channelid);
            if (result.issuccess)
            {
                for (int j = result.messagetext.Length - 1; j > 0; j -= 4)
                {
                    if (j - 3 < result.messagetext.Length)
                    {
                        string messageid = result.messagetext[j - 3];
                        string gdpname = result.messagetext[j - 2];
                        string message = result.messagetext[j - 1];
                        string filename = result.messagetext[j];
                        string combinedKey = messageid;
                        if (!displayedMessages.Contains(combinedKey))
                        {
                            if (displayedMessages.Count >= 100)
                            {
                                displayedMessages.Remove(displayedMessages.First());
                            }
                            displayedMessages.Add(combinedKey);
                            string[] filenames = filename.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries);
                            await AddMessageToChat(messageid, gdpname, message, filenames);
                        }
                    }
                }
            }
            while (true)
            {
                if (connection != null && connection.State == HubConnectionState.Connected)
                {
                    await connection.SendAsync("JoinGroup", _channelid, _gdpname);
                    break;
                }
            }
        }
        

        public async Task roinhom(string groupid,bool ispublic,string userid,int duoiorroi)
        {
            try
            {
                var result = DialogResult.No;
                if (duoiorroi == 1)
                {
                    result = MessageBox.Show(
                "Bạn có chắc chắn muốn rời nhóm này không?",
                "Xác nhận rời nhóm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );
                }
                else
                {
                    result = MessageBox.Show(
                "Bạn có chắc chắn muốn đuổi người dùng này ra khỏi nhóm này không?",
                "Xác nhận đuổi người dùng",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );
                }
            // Kiểm tra kết quả người dùng chọn
            if (result == DialogResult.Yes)
            {
                // Thực hiện xóa nhóm

                var remove =await _group.RemoveMemberFromGroup(groupid, userid);
                if (remove)
                {
                    UserSession.UpdateGroup = (true, ispublic);
                    if (ispublic) SendUpdate("UpdateGroupMember");
                    else SendUpdate("UpdateGroupMemberPrivate");
                    if(duoiorroi ==1) MessageBox.Show("Rời nhóm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else MessageBox.Show("Đuổi khỏi nhóm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
            }
            else
            {
                // Hủy thao tác
                MessageBox.Show("Thao tác rời nhóm đã bị hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void LoadGroup(string[] groupname)
        {
            try { 
            if (groupname != null)
            {
                for (int i = 0; i < groupname.Length; i++)
                {
                    string[] group1 = groupname[i].Split('|');
                    CircularPicture circulargroup = new CircularPicture();
                    try
                    {
                        circulargroup.Image = await _avatar.LoadAvatarGroupAsync(group1[0]);
                        circulargroup.SizeMode = PictureBoxSizeMode.Zoom;
                        circulargroup.Text = group1[1];
                        UserSession.ActionUpdateGroupname += async () =>
                        {
                            group1[1]= await _group.Groupname(group1[0]);
                            circulargroup.Text = group1[1];
                        };
                        circulargroup.Name = group1[0];
                        UserSession.ActionAvatarGroupUpdated += async () =>
                        {
                            circulargroup.Image = await _avatar.LoadAvatarGroupAsync(group1[0]);
                        };
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Không thể tải ảnh: {ex.Message}");
                        return;
                    }
                    circulargroup.Size = new Size(50, 50);

                    circulargroup.Anchor = AnchorStyles.None;
                    circulargroup.Click += async (s, e) =>
                    {
                        button1.Visible = true;
                        if (AddGroupMembers)
                        {
                            button1_Click_1(null, EventArgs.Empty);
                        }
                        TC_Chat.SelectedIndex = 0;
                        TC_ServerOrFriend.SelectedIndex = 1;
                        while (true)
                        {
                            if (connection != null && connection.State == HubConnectionState.Connected)
                            {
                                await connection.SendAsync("LeaveGroup", _channelid, _gdpname);
                                break;
                            }
                        }
                        await CloseLabel();
                        grouppublicorprivate = true;
                        tv_Kenh_DanhMuc.Nodes.Clear();
                        flp_Members.Controls.Clear();
                        flp_Message.Controls.Clear();
                        flp_Message.AutoScrollPosition = new Point(0, 0);
                        flp_Message.PerformLayout();
                        _groupid = group1[0];
                        _groupname = group1[1];
                        _userrole = await _groupMember.FindOneUserRole(_groupid, _userid);
                        _gdpname = await _groupMember.FindGroupDisplayname(_userid, _groupid);
                        lb_TenKenh.Text = "";
                        lb_TenKenh.Name = "";
                        string selectedGroupName = circulargroup.Text;
                        lb_TenGroup.Text = selectedGroupName;
                        lb_TenGroup1.Text = selectedGroupName;
                        lb_TenGroup1.Name = group1[0];
                        lb_TenGroup.Name = group1[0];
                        //AddGroupMembersList();
                        CreateMenu();
                        UserSession.ActionUpdateRole += async () =>
                        {
                            try
                            {
                                UserSession.RunOnUIThread(new Action( () => CreateMenu()));
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                           
                        };
                        UserSession.ActionUpdateGroupname += async () =>
                        {
                            _groupname = await _group.Groupname(group1[0]);
                            lb_TenGroup.Text = _groupname;
                            lb_TenGroup1.Text = _groupname;
                        };

                        UserSession.ActionUpdateRole += async () =>
                        {
                            _userrole = await _groupMember.FindOneUserRole(_groupid, _userid);
                        };
                        UserSession.ActionUpdategdpname += async () =>
                        {
                            _gdpname = await _groupMember.FindGroupDisplayname(_userid, _groupid);
                        };
                        kenh();

                    };
                    circulargroup.MouseUp += (s,e) =>
                    {
                        if (e.Button == MouseButtons.Right) // Kiểm tra chuột phải
                        {
                            formaddremovemember(circulargroup,e);
                        }
                    };
                    flp_Group.Controls.Add(circulargroup);
                        UserSession.ActionUpdateChannel -= kenh;
                        UserSession.ActionUpdateChannel += kenh;
                        UserSession.ActionUpdateDanhMuc -= kenh;
                        UserSession.ActionUpdateDanhMuc += kenh;
                 
                }
            }
            
            CircularPicture circularadd = new CircularPicture();

            try
            {
                circularadd.Image = global::QLUSER.Properties.Resources.add_icon_png_14;
                circularadd.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể tải ảnh: {ex.Message}");
                return;
            }
            circularadd.Size = new Size(50, 50);

            circularadd.Anchor = AnchorStyles.None;
            circularadd.Click += (s, e) =>
            {

                circularadd_Click();
            };
            flp_Group.Controls.Add(circularadd);

            flp_Group.FlowDirection = FlowDirection.LeftToRight;
            flp_Group.WrapContents = true;
            flp_Group.AutoScroll = false;
            flp_Group.MouseWheel += FlowLayoutPanel2_MouseWheel;

            flp_Group.Padding = new Padding((flp_Group.ClientSize.Width - 50) / 2, 0, 0, 10);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void formaddremovemember(CircularPicture circulargroup, MouseEventArgs e)
        {
            TC_ServerOrFriend.SelectedIndex = 2;
            //CreateMenu();
        }
        void FlowLayoutPanel2_MouseWheel(object sender, MouseEventArgs e)
        {
            try
            {
                int delta = e.Delta > 0 ? -20 : 20;
                int currentY = -flp_Group.AutoScrollPosition.Y;
                int newScrollY = currentY + delta;
                flp_Group.AutoScrollPosition = new Point(0, newScrollY);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void kenh()
        {
            try
            {
                await CloseLabel();
                tv_Kenh_DanhMuc.Nodes.Clear();
                flp_Message.Controls.Clear();
                flp_Message.AutoScrollPosition = new Point(0, 0);
                flp_Message.PerformLayout();
                var result = await _danhmuc.RequestDanhMucName(_groupid);
                if (result.issuccess)
                {
                    for (int ib = 0; ib < result.danhmucidname.Length; ib++)
                    {
                        string[] danhmuc = result.danhmucidname[ib].Split('|');
                        TreeNode node = tv_Kenh_DanhMuc.Nodes.Add($"DanhMuc|{danhmuc[0]}", danhmuc[1]);
                        await createlabel(node);
                    }
                }
                var result1 = await _channel.RequestChannelName(_groupid);
                if (result1.issuccess)
                {
                    for (int ia = 0; ia < result1.channelidname.Length; ia++)
                    {

                        string[] channel = result1.channelidname[ia].Split('|');
                        if (channel[3] == "True")
                        {
                            if (!string.IsNullOrEmpty(channel[2]))
                            {
                                TreeNode[] nodes = tv_Kenh_DanhMuc.Nodes.Find($"DanhMuc|{channel[2]}", false);
                                if (nodes != null)
                                {
                                    TreeNode node = nodes[0].Nodes.Add($"VanBanChat|{channel[0]}", channel[1]);
                                }
                            }
                            else
                            {
                                TreeNode node = tv_Kenh_DanhMuc.Nodes.Add($"VanBanChat|{channel[0]}", channel[1]);
                            }
                        }
                        else if (channel[3] == "False")
                        {
                            if (!string.IsNullOrEmpty(channel[2]))
                            {
                                TreeNode[] nodes = tv_Kenh_DanhMuc.Nodes.Find($"DanhMuc|{channel[2]}", false);
                                if (nodes != null)
                                {
                                    TreeNode node = nodes[0].Nodes.Add($"CuocGoiVideo|{channel[0]}", channel[1]);
                                }
                            }
                            else
                            {
                                TreeNode node = tv_Kenh_DanhMuc.Nodes.Add($"CuocGoiVideo|{channel[0]}", channel[1]);
                            }
                        }
                    }
                }
                tv_Kenh_DanhMuc.AfterExpand += TreeView_AfterExpandCollapse;
                tv_Kenh_DanhMuc.AfterCollapse += TreeView_AfterExpandCollapse;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void TreeView_AfterExpandCollapse(object sender, TreeViewEventArgs e)
        {
            await ChangeLabelLocation();
        }
        private async Task ChangeLabelLocation()
        {
            try { 
            var matchedNodes = tv_Kenh_DanhMuc.Nodes
            .Cast<TreeNode>()
            .Where(node => node.Name.Contains("DanhMuc"))
            .ToList();
            foreach (var node in matchedNodes)
            {
                if (node.Tag is Label label)
                {
                    int x = tv_Kenh_DanhMuc.Location.X + tv_Kenh_DanhMuc.Size.Width - label.Size.Width - 1;
                    int nodeY = node.Bounds.Location.Y;
                    int y = tv_Kenh_DanhMuc.Location.Y + 1 + nodeY;
                    label.Location = new Point(x, y);
                }
            }
        }
    catch (Exception ex)
    {
        MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
        private async Task CloseLabel()
        {
            try { 
            var matchedNodes = tv_Kenh_DanhMuc.Nodes
            .Cast<TreeNode>()
            .Where(node => node.Name.Contains("DanhMuc"))
            .ToList();
            foreach (var node in matchedNodes)
            {
                if (node.Tag is Label label)
                {
                    label.Parent?.Controls.Remove(label);
                    label.Dispose();
                    node.Tag = null;
                }
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
            

            

        private async void bt_guitinnhan_Click(object sender, EventArgs e)
        {
            try { 
            if (lb_TenKenh.Text == "") MessageBox.Show("Gửi tin nhắn thất bại");
            else
            {
                string message = tb_Message.Text;
                if (!string.IsNullOrEmpty(lb_File_Anh.Text))
                {
                    tb_Message.Clear();
                    string[] filenames = selectedFilePaths.Select(Path.GetFileName).ToArray();
                    var result = await _file.SendFileToServer(_userid, _channelid, message, selectedFilePaths);
                    if (result.issuccess)
                    {
                        string[] messageid = result.messatid.Split('|');
                        while (true)
                        {
                            if (connection != null && connection.State == HubConnectionState.Connected)
                            {
                                await connection.SendAsync("SendMessage", messageid[0], _gdpname, _channelid, message, filenames);
                                break;
                            }
                        }
                        lb_File_Anh.Text = string.Empty;
                    }
                }
                else if (!string.IsNullOrWhiteSpace(message))
                {
                    tb_Message.Clear();
                    var result = await _message.SendMessage(_channelid, _userid, message);
                    if (result.issuccess)
                    {
                        while (true)
                        {
                            if (connection != null && connection.State == HubConnectionState.Connected)
                            {
                                await connection.SendAsync("SendMessage", result.messageid, _gdpname, _channelid, message, null);
                                break;
                            }
                        }
                    }
                }
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private async Task AddMessageToChat(string messageid, string gdpname, string messageContent, string[] filename)
        {
            try
            {
                string gdpid = await _groupMember.FindGroupDisplayID(_groupid, gdpname);
                Panel panelMessage = new Panel
                {
                    BackColor = Color.FromArgb(66, 69, 73),
                    Padding = new Padding(5),
                    AutoSize = true, // Tự động điều chỉnh kích thước
                    AutoSizeMode = AutoSizeMode.GrowAndShrink, // Chỉ điều chỉnh theo chiều cao
                    Margin = new Padding(10, 0, 0, 0),
                    Name = messageid,
                    Width = flp_Message.Size.Width-40, // Cố định chiều rộng
                    MinimumSize = new Size(flp_Message.Size.Width - 40, 0), // Giữ chiều rộng cố định
                };
                CircularPicture pictureBoxAvatar = new CircularPicture
                {
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Width = 40,
                    Height = 40,
                    Image = await _avatar.LoadAvatarAsync(gdpid),
                    Margin = new Padding(0, 0, 10, 0)
                };

                pictureBoxAvatar.MouseUp += (s, e) =>
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        CaiDatMayChu caiDatMayChu = new CaiDatMayChu(_groupname, _groupid, _userid, this, DN);
                        caiDatMayChu.formusergroup(gdpname, false,grouppublicorprivate);
                    }
                    if (e.Button == MouseButtons.Left)
                    {

                        USERINFOR(gdpname);
                    }
                };
                UserSession.ActionAvatarUpdated += async () =>
                {
                    pictureBoxAvatar.Image = await _avatar.LoadAvatarAsync(gdpid);
                };
                panelMessage.Controls.Add(pictureBoxAvatar);

                Label lblUsername = new Label
                {
                    Text = gdpname,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    ForeColor = Color.FromArgb(88, 101, 242),
                    AutoSize = true,
                    Top = 5,
                    Left = pictureBoxAvatar.Right + 10
                };
                UserSession.ActionUpdategdpname += async () =>
                {
                    lblUsername.Text = await _groupMember.FindGroupDisplayname(gdpid, _groupid);
                };
                Label lblMessage=null;
                panelMessage.Controls.Add(lblUsername);
                if (!string.IsNullOrEmpty(messageContent))
                {
                    lblMessage = new Label
                    {
                        Text = $"{messageContent}",
                        Font = new Font("Segoe UI", 10),
                        ForeColor = Color.White,
                        AutoSize = true,
                        Top = lblUsername.Bottom + 5,
                        Left = pictureBoxAvatar.Right + 10,
                    };
                    UserSession.ActionUpdateMessageText += async () =>
                    {
                        lblMessage.Text = await _message.Onemessage(messageid);
                    };
                    panelMessage.Controls.Add(lblMessage);
                }
                List<string> nonImageVideoFiles = new List<string>();

                if (filename != null && filename.Length > 0)
                {
                    foreach (var Filename in filename)
                    {

                        string fileExtension = System.IO.Path.GetExtension(Filename).ToLower();

                        if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png" ||
                            fileExtension == ".bmp" || fileExtension == ".gif")
                        {
                            string filepath = await _file.GetFileUrlAsync(Filename);
                            PictureBox pictureBox = new PictureBox
                            {
                                SizeMode = PictureBoxSizeMode.Zoom,
                                Width = 200,
                                Height = 150,
                                Top = panelMessage.Controls.Count > 2 ? lblMessage.Bottom + 5 : lblUsername.Bottom + 5,
                                Left = pictureBoxAvatar.Right + 10
                            };
                            using (HttpClient client = new HttpClient())
                            {
                                try
                                {
                                    byte[] imageBytes = await client.GetByteArrayAsync(filepath);
                                    using (MemoryStream ms = new MemoryStream(imageBytes))
                                    {
                                        pictureBox.Image = Image.FromStream(ms);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Lỗi khi tải ảnh: " + ex.Message);
                                }
                            }
                            pictureBox.Click += (sender, e) =>
                            {
                                if (MessageBox.Show("Bạn có muốn tải xuống hình ảnh này không?", "Download Image", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    isReceivingMessages = false;
                                    _file.DownloadFile(Filename);
                                    isReceivingMessages = true;
                                }
                            };

                            panelMessage.Controls.Add(pictureBox);
                            panelMessage.Height += pictureBox.Height + 10;
                        }
                        else
                        {
                            nonImageVideoFiles.Add(Filename);
                        }
                    }
                }

                if (nonImageVideoFiles.Count > 0)
                {
                    ListView listViewFiles = new ListView
                    {
                        View = View.LargeIcon,
                        ForeColor = Color.White,
                        BackColor = Color.FromArgb(47, 49, 54),
                        Width = panelMessage.Width - 60,
                        Height = 40,
                        Scrollable = true,
                        Top = panelMessage.Controls.Count > 2 ? lblMessage.Bottom + 5 : lblUsername.Bottom + 5,
                        Left = pictureBoxAvatar.Right + 10
                    };
                    foreach (var FileName in nonImageVideoFiles)
                    {
                        listViewFiles.Items.Add(new ListViewItem(FileName)
                        {
                            Tag = FileName
                        });
                    }

                    listViewFiles.ItemActivate += (sender, e) =>
                    {
                        var selectedItem = listViewFiles.SelectedItems[0];
                        string selectedFileName = (string)selectedItem.Tag;

                        if (MessageBox.Show("Bạn có muốn tải xuống tệp này không?", "Download File", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        {
                            isReceivingMessages = false;
                            _file.DownloadFile(selectedFileName);
                            isReceivingMessages = true;
                        }
                    };

                    panelMessage.Controls.Add(listViewFiles);
                    panelMessage.Height += listViewFiles.Height + 10;
                }

                panelMessage.MouseUp += (s, e) =>
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        // Tạo menu ngữ cảnh
                        ContextMenuStrip contextMenu = new ContextMenuStrip();

                        if (gdpid == _userid)
                        {
                            contextMenu.Items.Add("Chỉnh sửa tin nhắn", null, (sv, ev) =>
                            {
                                Chinhsuatinnhan(messageid, lblMessage);
                            });
                        }
                        contextMenu.Items.Add("Sao chép văn bản", null, (sv, ev) => {
                            Saochepvanban(lblMessage);
                        });
                        if(_userid == gdpid ||(grouppublicorprivate && _userrole !="Member") )

                        contextMenu.Items.Add("Xóa tin nhắn", null, (sv, ev) => {
                            XoaTinNhan(messageid);
                        });

                        // Chuyển đổi tọa độ từ Panel sang màn hình
                        Point screenPoint = panelMessage.PointToScreen(e.Location);

                        // Chuyển đổi tọa độ từ màn hình sang flp_Message
                        Point flpPoint = flp_Message.PointToClient(screenPoint);

                        // Hiển thị ContextMenuStrip tại vị trí chính xác
                        contextMenu.Show(flp_Message, flpPoint);

                    }

                };
                flp_Message.Controls.Add(panelMessage);
                flp_Message.ScrollControlIntoView(panelMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void Chinhsuatinnhan(string messageid,Label message)
        {
            try
            {
                string messagetext = "";
                if (message != null) messagetext = message.Text; 
                string newmessagetext = Microsoft.VisualBasic.Interaction.InputBox(
               $"Nhập nội dung tin nhắn mới:",
               "Đổi nội dung tin nhắn",
               messagetext
                );
                if (newmessagetext != messagetext && !string.IsNullOrWhiteSpace(newmessagetext))
                {
                    var result = await _message.EditMessage(newmessagetext, messageid);
                    if (result)
                    {

                        if (messagetext != "")
                        {
                            UserSession.RenameMessageText = true;
                            SendUpdate("UpdateMessageText");
                        }
                        else
                        {
                            UserSession.UpdateMessage = true;
                            SendUpdate("UpdateMessage");
                        }
                        MessageBox.Show("Nội dung tin nhắn đã được đổi thành công!", "Thông Báo");
                    }
                    else MessageBox.Show("Nội dung tin nhắn đổi thất bại!", "Thông Báo");
                }
                else
                {
                    MessageBox.Show("Nội dung tin nhắn không thay đổi.", "Thông Báo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void Saochepvanban(Label message)
        {
            try
            {
                if (message == null) return;
                Thread staThread = new Thread(() =>
                {
                    try
                    {
                        Clipboard.SetText(message.Text);
                    }
                    catch (Exception ex)
                    {
                        Saochepvanban(message);
                    }
                });
                staThread.SetApartmentState(ApartmentState.STA);
                staThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void XoaTinNhan(string messageid)
        {
            try
            {
                var result = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa tin nhắn này không?",
                    "Xác nhận xóa tin nhắn",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                // Kiểm tra kết quả người dùng chọn
                if (result == DialogResult.Yes)
                {
                    // Thực hiện xóa nhóm
                    var delete = await _message.DeleteMessage(messageid);
                    if (delete)
                    {
                        UserSession.UpdateMessage = true;
                        SendUpdate("UpdateMessage");
                        MessageBox.Show("Tin nhắn đã được xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Thao tác xóa tin nhắn đã bị hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void buttonchonfile_Click(object sender, EventArgs e)
        {
            try
            {
                Thread thread = new Thread(() =>
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog
                    {
                        Multiselect = true,
                        Filter = "Document Files|*.txt;*.pdf;*.docx;*.xlsx;*.pptx;*.csv;*.xml;*.json;*.html;*.css|All Files (*.*)|*.*"
                    };

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        if (openFileDialog.FileNames.Length > 5)
                        {
                            this.Invoke(new Action(() =>
                            {
                                MessageBox.Show("Bạn chỉ được chọn tối đa 5 file. Vui lòng chọn lại.");
                            }));
                        }
                        else
                        {
                            string[] selectedFiles = openFileDialog.FileNames;
                            this.Invoke(new Action(() =>
                            {
                                selectedFilePaths.Clear();
                                selectedFilePaths.AddRange(selectedFiles);
                                lb_File_Anh.Text = string.Join(", ", openFileDialog.SafeFileNames);
                            }));
                        }
                    }
                });
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonchonanh_Click(object sender, EventArgs e)
        {
            try { 
            Thread thread = new Thread(() =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Multiselect = false,
                    Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All Files (*.*)|*.*",
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFile = openFileDialog.FileName;
                    this.Invoke(new Action(() =>
                    {
                        selectedFilePaths.Clear();
                        selectedFilePaths.Add(selectedFile);
                        lb_File_Anh.Text = openFileDialog.SafeFileName;
                    }));
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CaiDatNguoiDung formuser = new CaiDatNguoiDung(_userid,DN,this,_username,0,null);
            formuser.Show();
        }



        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_Gui.PerformClick();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }

        }
        private async Task InitializeSignalR()
        {
            try { 
            connection = new HubConnectionBuilder()
            .WithUrl(ConfigurationManager.AppSettings["HubUrl"] + "messageHub")
            .ConfigureLogging(logging =>
            {
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Debug);
            })
            .Build();
            await connection.StartAsync();
            connection.On<string, string, string, string[]>("ReceiveMessage", async (messageid, message, groupdisplayname, filenames) =>
            {
                try
                {
                    isRecevie = true;
                    this.Invoke(new Action(async () =>
                    {
                        await AddMessageToChat(messageid, groupdisplayname, message, filenames);
                    }));
                    isRecevie = false;
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }

            });

            connection.On<bool>("ReceiveAvatarDisplay", async (update) =>
            {
                UserSession.AvatarUrl = update;
            });
            connection.On<bool>("ReceiveGroupDislay", async (update) =>
            {
                UserSession.UpdateGroup = (update,true);
            });
            connection.On<bool>("ReceiveGroupPrivateDislay", async (update) =>
            {
                UserSession.UpdateGroup = (update,false);
            });
            connection.On<bool>("Receivedpname", async (update) =>
            {
                UserSession.Renamedpname = update;
            });
            connection.On<bool>("ReceiveAvatarGroupUpdated", async (update) =>
            {
                UserSession.AvatarGroupUrl = update;
            });
            connection.On<bool>("ReceiveDeleteuser", async (update) =>
            {
                UserSession.DeleteUser = update;
            });
            connection.On<bool>("ReceiveUpdateFriend", async (update) =>
            {
                UserSession.UpdateFriend = update;
            });
            connection.On<bool>("ReceiveUpdateFriendRequest", async (update) =>
            {
                UserSession.UpdateFriendRequest = update;
            });
            connection.On<bool>("ReceiveUpdategdpname", async (update) =>
            {
                UserSession.Renamegdpname = (update,update);
            });
            connection.On<bool>("ReceiveUpdateGroupMember", async (update) =>
            {
                UserSession.UpdateGroupMember = (update,true);
            });
            connection.On<bool>("ReceiveUpdateChannel", async (update) =>
            {
                UserSession.UpdateChannel = update;
            });
            connection.On<bool>("ReceiveUpdateChannelname", async (update) =>
            {
                UserSession.Renamechannelname = update;
            });
            connection.On<bool>("ReceiveUpdateDanhMuc", async (update) =>
            {
                UserSession.UpdateDanhMuc = update;
            });
            connection.On<bool>("ReceiveUpdatedanhmucname", async (update) =>
            {
                UserSession.Renamedanhmucname = update;
            });
            connection.On<bool>("ReceiveUpdateGroupMemberPrivate", async (update) =>
            {
                UserSession.UpdateGroupMember = (update,false);
            });
            connection.On<bool>("ReceiveUpdateGroupname", async (update) =>
            {
                UserSession.RenameGroupname = update;
            });
            connection.On<bool>("ReceiveUpdateMessage", async (update) =>
            {
                UserSession.UpdateMessage = update;
            });
            connection.On<bool>("ReceiveUpdateMessageText", async (update) =>
            {
                UserSession.RenameMessageText = update;
            });
            connection.On<bool>("ReceiveUpdateRole", async (update) =>
            {
                UserSession.UpdateUserRole = (update,update);
            });


            connection.Closed += async (exception) =>
            {
                while (true)
                {
                    try
                    {
                        if (!isstop)
                            await ReconnectSignalR();
                        break;
                    }
                    catch (Exception reconnectEx)
                    {
                        MessageBox.Show("Reconnect failed: " + reconnectEx.Message);
                    }
                }
            };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private async Task ReconnectSignalR()
        {
            try
            {
                if (connection.State != HubConnectionState.Connected)
                {
                    await connection.StartAsync();
                    await connection.SendAsync("JoinGroup", _channelid, _gdpname);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Reconnection failed: " + ex.Message);
            }
        }
        public async Task StopSignalR()
        {
            try { 
            if (connection != null)
            {
                try
                {
                    await connection.SendAsync("LeaveGroup", _channelid, _gdpname);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error sending LeaveCall: " + ex.Message);
                }
            }
            await Task.Delay(100);
            while (true)
            {
                if (!isRecevie)
                {

                    if (connection != null)
                    {
                        isstop = true;
                        await connection.StopAsync();
                        await connection.DisposeAsync();
                        connection = null;
                    }
                    break;
                }
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void treeView1_AfterSelect(TreeNodeMouseClickEventArgs e)
        {
            try { 
            TreeNode selectedNode = e.Node;
            if (selectedNode != null)
            {
                if (selectedNode.Name.Contains("VanBanChat"))
                {
                    while (true)
                    {
                        if (connection != null && connection.State == HubConnectionState.Connected)
                        {
                            await connection.SendAsync("LeaveGroup", _channelid, _gdpname);
                            break;
                        }
                    }
                    displayedMessages.Clear();
                    flp_Message.Controls.Clear();
                    flp_Message.AutoScrollPosition = new Point(0, 0);
                    flp_Message.PerformLayout();
                    string selectedKenhName = selectedNode.Text;
                    lb_TenKenh.Text = selectedKenhName;
                    string[] channelid = selectedNode.Name.Split('|');
                    lb_TenKenh.Name = channelid[1];
                    _channelid = channelid[1];
                    _channelname = selectedKenhName;
                        UserSession.ActionUpdateChannelname -= hamupdatechannelname;
                        UserSession.ActionUpdateChannelname += hamupdatechannelname;
                        message();
                    while (true)
                    {
                        if (connection != null && connection.State == HubConnectionState.Connected)
                        {
                            await connection.SendAsync("JoinGroup", _channelid, _gdpname);
                            break;
                        }
                    }
                    
                }
                else if (selectedNode.Name.Contains("CuocGoiVideo"))
                {
                    while (true)
                    {
                        if (connection != null && connection.State == HubConnectionState.Connected)
                        {
                            await connection.SendAsync("LeaveGroup", _channelid, _gdpname);
                            break;
                        }
                    }
                    VideoCall vc = new VideoCall(_gdpname, _channelid,_groupid,this);
                    vc.Show();
                    displayedMessages.Clear();
                    flp_Message.Controls.Clear();
                    flp_Message.AutoScrollPosition = new Point(0, 0);
                    flp_Message.PerformLayout();
                    string selectedKenhName = selectedNode.Text;
                    lb_TenKenh.Text = selectedKenhName;
                    string[] channelid = selectedNode.Name.Split('|');
                    lb_TenKenh.Name = channelid[1];
                    _channelid = channelid[1];
                    _channelname = selectedKenhName;
                        UserSession.ActionUpdateChannelname -= hamupdatechannelname;
                        UserSession.ActionUpdateChannelname += hamupdatechannelname;
                    message();
                    while (true)
                    {
                        if (connection != null && connection.State == HubConnectionState.Connected)
                        {
                            await connection.SendAsync("JoinGroup", _channelid, _gdpname);
                            break;
                        }
                    }
                }
                    UserSession.ActionUpdateMessage -= hamupdatemessage;
                    UserSession.ActionDeleteuser -= hamupdatemessage;
                    UserSession.ActionUpdateMessage += hamupdatemessage;
                    UserSession.ActionDeleteuser += hamupdatemessage;
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode clickedNode = e.Node;

                if (clickedNode != null)
                {
                    if (_userrole == "Member" || _userrole == "Mod") return;
                    TreeNode node = clickedNode.Parent;
                    _danhmucid = null;
                    string[] danhmucid=null;
                    if (node != null)
                    { danhmucid = node.Name.Split('|');
                        _danhmucid = danhmucid[1]; }
                    //MessageBox.Show(node + "|" + node.Text + "|" + node.Name);
                    //tv_Kenh_DanhMuc.SelectedNode = clickedNode; // Đặt nút này làm nút được chọn dòng này cho vảo chỉ để nhấn 1 lần muốn nhấn lại phải nhấn nút khác trước
                    ContextMenuStrip contextMenu = new ContextMenuStrip();

                    if (clickedNode.Name.Contains("VanBanChat")|| clickedNode.Name.Contains("CuocGoiVideo"))
                    {
                        contextMenu.Items.Add("Chỉnh sửa kênh", null, (s, ev) =>
                        {
                            string[] channelid = clickedNode.Name.Split('|');
                            CaiDatKenh caiDatKenh = new CaiDatKenh(channelid[1], clickedNode.Text,_danhmucid,this);
                            caiDatKenh.ShowDialog();
                        });
                        contextMenu.Items.Add("Tạo kênh", null, (s, ev) => TaoKenh(_danhmucid,node));
                        contextMenu.Items.Add("Xóa kênh", null, (s, ev) => XoaKenh(clickedNode));
                    }
                    else if (clickedNode.Name.Contains("DanhMuc"))
                    {
                        contextMenu.Items.Add("Chỉnh sửa danh mục", null, (s, ev) =>
                        {
                            string[] danhmucid1 = clickedNode.Name.Split('|');
                            CaiDatDanhMuc caiDatDanhMuc = new CaiDatDanhMuc(danhmucid1[1], clickedNode.Text,this);
                            caiDatDanhMuc.ShowDialog();
                        });
                        contextMenu.Items.Add("Xóa mục", null, (s, ev) => XoaMuc(clickedNode));
                    }

                    // Hiển thị menu ngữ cảnh tại vị trí nhấp chuột
                    contextMenu.Show(tv_Kenh_DanhMuc, e.Location);
                }
            }
            else if(e.Button == MouseButtons.Left)
            {
                treeView1_AfterSelect(e);
            }
        }
        public async void XoaKenh(TreeNode node)
        {
            try
            {
                var result = DialogResult.No;

                    result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa kênh này không?",
                "Xác nhận xóa kênh",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );
 
                    
                
                // Kiểm tra kết quả người dùng chọn
                if (result == DialogResult.Yes)
                {
                    // Thực hiện xóa nhóm
                    string[] channelid = node.Name.Split('|');
                    var remove = await _channel.DeleteChannel(channelid[1]);
                    if (remove)
                    {
                        UserSession.UpdateChannel = true;
                        SendUpdate("UpdateChannel");
                        MessageBox.Show("Xóa kênh thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    // Hủy thao tác
                    MessageBox.Show("Thao tác xóa kênh đã bị hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public async void XoaMuc(TreeNode node)
        {
            try
            {
                var result = DialogResult.No;

                result = MessageBox.Show(
            "Bạn có chắc chắn muốn xóa danh mục này không?",
            "Xác nhận xóa danh mục",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning
        );



                // Kiểm tra kết quả người dùng chọn
                if (result == DialogResult.Yes)
                {
                    // Thực hiện xóa nhóm
                    string[] danhmucid = node.Name.Split('|');
                    var remove = await _danhmuc.Deletedanhmuc(danhmucid[1]);
                    if (remove)
                    {
                        UserSession.UpdateDanhMuc = true;
                        SendUpdate("UpdateDanhMuc");
                        MessageBox.Show("Xóa danh mục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    // Hủy thao tác
                    MessageBox.Show("Thao tác xóa danh mục đã bị hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private async void hamupdatechannelname()
        {
            var channel = await _channel.RequestoneChannelName(_channelid);
            lb_TenKenh.Text = channel.channelname;
            _channelname = channel.channelname;
        }
        private async void hamupdatemessage()
        {
           UserSession.RunOnUIThread(new Action(()=> message()));
        }

        private async void AddGroupMembersList()
        {
            try
            {

                var users = await _groupMember.FindGroupMembers(_groupid);
                foreach (var elem in users) {
                    var user = elem.ToObject<dynamic>();

                    Panel panelUser = new Panel
                    {
                        Width = flp_Members.Width,
                        MinimumSize = new Size(flp_Members.Width-20,0),
                        BackColor = Color.FromArgb(54, 57, 62),
                        Padding = new Padding(5),
                        AutoSize = true,
                        Margin = new Padding(10, 0, 0, 0),
                        Name = user.groupDisplayname
                    };

                    panelUser.MouseUp += (s, e) =>
                    {
                        if (e.Button == MouseButtons.Right)
                        {
                            CaiDatMayChu caiDatMayChu = new CaiDatMayChu(_groupname, _groupid, _userid, this, DN);
                            caiDatMayChu.formusergroup(user.groupDisplayname.ToString(), false,grouppublicorprivate);
                        }
                        if (e.Button == MouseButtons.Left)
                        {
                            USERINFOR(user.groupDisplayname.ToString());
                        }
                    };
                    CircularPicture pictureBoxAvatar = new CircularPicture
                    {
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Width = 30,
                        Height = 30,
                        Image = await _avatar.LoadAvatarAsync(user.userId.ToString()),
                        Margin = new Padding(0, 0, 10, 0)
                    };

                    pictureBoxAvatar.MouseUp += (s, e) =>
                    {
                        if (e.Button == MouseButtons.Right)
                        {
                            CaiDatMayChu caiDatMayChu = new CaiDatMayChu(_groupname, _groupid, _userid, this, DN);
                            caiDatMayChu.formusergroup(user.groupDisplayname.ToString(), false,grouppublicorprivate);
                        }
                        if (e.Button == MouseButtons.Left)
                        {
                            USERINFOR(user.groupDisplayname.ToString());
                        }
                    };
                    UserSession.ActionAvatarUpdated += async () =>
                    {
                        pictureBoxAvatar.Image = await _avatar.LoadAvatarAsync(user.userId.ToString());
                    };
                    panelUser.Controls.Add(pictureBoxAvatar);

                    Label lblUsername = new Label
                    {
                        Text = user.groupDisplayname.ToString(),
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        ForeColor = Color.FromArgb(88, 101, 242),
                        AutoSize = true,
                        Top = 5,
                        Left = pictureBoxAvatar.Right + 10
                    };
                    lblUsername.MouseUp += (s, e) =>
                    {
                        if (e.Button == MouseButtons.Right)
                        {
                            CaiDatMayChu caiDatMayChu = new CaiDatMayChu(_groupname, _groupid, _userid, this, DN);
                            caiDatMayChu.formusergroup(user.groupDisplayname.ToString(), false,grouppublicorprivate);
                        }
                        if (e.Button == MouseButtons.Left)
                        {
                            USERINFOR(user.groupDisplayname.ToString());
                        }
                    };
                    UserSession.ActionUpdategdpname += async () =>
                    {
                        string groupdisplayname = await _groupMember.FindGroupDisplayname(user.userId.ToString(), _groupid);
                        lblUsername.Text = groupdisplayname;
                        panelUser.Name = groupdisplayname;
                    };

                    UserSession.ActionUpdateGroupMemberPrivate -= AddGroupMembersList;
                    UserSession.ActionUpdateGroupMemberPrivate += AddGroupMembersList;
                    UserSession.ActionDeleteuser -= AddGroupMembersList;
                    UserSession.ActionDeleteuser += AddGroupMembersList;
                    panelUser.Controls.Add(lblUsername);

                    flp_Members.Controls.Add(panelUser);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
            private async void CreateMenu()
        {
            try
            {
                _userrole = await _groupMember.FindOneUserRole(_groupid, _userid);
                P_Kenh_DanhMuc1.Controls.Clear();
                panelMenu = new FlowLayoutPanel();
                panelMenu.Dock = DockStyle.Fill;
                panelMenu.BackColor = Color.FromArgb(47, 49, 54);
                panelMenu.FlowDirection = FlowDirection.TopDown;
                panelMenu.WrapContents = false;
                panelMenu.AutoScroll = true;
                P_Kenh_DanhMuc1.Controls.Add(panelMenu);

                AddMenuItem(panelMenu, "Mời Mọi Người");
                if (_userrole == "Admin" || _userrole == "Owner") AddMenuItem(panelMenu, "Cài đặt máy chủ");
                if (_userrole == "Admin" || _userrole == "Owner") AddMenuItem(panelMenu, "Tạo kênh");
                if (_userrole == "Admin" || _userrole == "Owner") AddMenuItem(panelMenu, "Tạo Danh Mục");
                AddMenuItem(panelMenu, "Chỉnh Sửa Hồ Sơ Máy Chủ");
                if (_userrole != "Owner") AddMenuItem(panelMenu, "Rời khỏi phòng");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            

        }

        private async void message()
        {
            try {
                displayedMessages.Clear();
                flp_Message.Controls.Clear();
            var result = await _message.ReceiveMessage(_channelid);
            if (result.issuccess)
            {
                for (int i = result.messagetext.Length - 1; i > 0; i -= 4)
                {
                    if (i - 3 < result.messagetext.Length)
                    {
                        string messageid = result.messagetext[i - 3];
                        string gdpname = result.messagetext[i - 2];
                        string message = result.messagetext[i - 1];
                        string filename = result.messagetext[i];
                        string combinedKey = messageid;
                        if (!displayedMessages.Contains(combinedKey))
                        {
                            if (displayedMessages.Count >= 100)
                            {
                                displayedMessages.Remove(displayedMessages.First());
                            }
                            displayedMessages.Add(combinedKey);
                            string[] filenames = filename.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries);
                            await AddMessageToChat(messageid, gdpname, message, filenames);
                        }
                    }
                }
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddMenuItem(FlowLayoutPanel parentPanel, string text)
        {
            try { 
            Panel menuItem = new Panel();
            menuItem.Height = 25;
            menuItem.Width = parentPanel.Width;
            menuItem.BackColor = Color.FromArgb(54, 57, 63);
            menuItem.Padding = new Padding(10, 0, 0, 0);

            Label label = new Label();
            label.Text = text;
            label.ForeColor = Color.White;
            label.Font = new Font("Arial", 10, FontStyle.Regular);
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.Click += async (s, e) =>
            {
                switch (text)
                {
                    case "Mời Mọi Người": MoiMoiNguoi(_groupname,_groupid); break;
                    case "Cài đặt máy chủ": Caidatmaychu(_groupname,_groupid); break;
                    case "Tạo kênh": TaoKenh(null, null); break;
                    case "Tạo Danh Mục": TaoDanhMuc(); break;
                    case "Chỉnh Sửa Hồ Sơ Máy Chủ":
                        CaiDatNguoiDung formuser = new CaiDatNguoiDung(_userid, DN, this, _username, 2, _groupid);
                        formuser.ShowDialog(); 
                        break;
                    case "Rời khỏi phòng": await roinhom(_groupid,true,_userid,1); break;
                }
            };
            menuItem.Controls.Add(label);
            parentPanel.Controls.Add(menuItem);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void button5_Click_1(object sender, EventArgs e)
        {
            TC_ServerOrFriend.SelectedIndex = 2;
            //CreateMenu();


        }
        private void Caidatmaychu(string groupname,string groupid)
        {
            CaiDatMayChu maychu = new CaiDatMayChu(groupname, groupid,_userid,this,DN);
            maychu.ShowDialog();
        }
        private async void MoiMoiNguoi(string groupname,string groupid)
        {
            try
            {
                Form form = new Form();
                form.Text = $"Mời bạn bè vào {groupname}";
                form.Size = new Size(500, 370);
                form.AutoSize = true;
                form.StartPosition = FormStartPosition.CenterParent;

                Label label = new Label();
                label.Text = $"Mời bạn bè vào nhóm: {groupname}";
                label.Font = new Font("Arial", 12, FontStyle.Bold);
                label.ForeColor = Color.Black;
                label.AutoSize = true;
                label.Location = new Point(20, 20);
                UserSession.ActionUpdateGroupname += async () =>
                {
                    groupname = await _group.Groupname(_groupid);
                    form.Text = $"Mời bạn bè vào {groupname}";
                    label.Text = $"Mời bạn bè vào nhóm: {groupname}";
                };
                string maloimoi = await _group.FindMaLoiMoi(_groupid);
                FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
                flowLayoutPanel.Location = new Point(20, 50);
                flowLayoutPanel.Size = new Size(440, 150);
                flowLayoutPanel.FlowDirection = FlowDirection.TopDown;
                flowLayoutPanel.WrapContents = false;
                flowLayoutPanel.AutoScroll = true;
                using (HttpClient _httpClient = new HttpClient())
                {
                    try
                    {

                        flowLayoutPanel.Controls.Clear();

                        string requestUrl = $"{ConfigurationManager.AppSettings["ServerUrl"]}Friend/{_userid}";

                        HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            string responseContent = await response.Content.ReadAsStringAsync();
                            var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);

                            if (responseData != null && responseData.message != null)
                            {
                                foreach (string displayName in responseData.message)
                                {
                                    string userid = await _user.finduserid(displayName);
                                    string isjoined = await _groupMember.FindJointime(userid, _groupid);
                                    if (isjoined != null) continue;
                                    Image avatarImage = await _avatar.LoadAvatarAsync(userid);
                                    Panel friendPanel = new Panel
                                    {
                                        Width = flowLayoutPanel.Width - 60,
                                        Height = 30,
                                        BorderStyle = BorderStyle.FixedSingle,
                                        AutoSize = true,
                                    };
                                    Label usernamebox = new Label
                                    {
                                        Text = displayName,
                                        Name = userid,
                                        AutoSize = true,
                                        Location = new Point(45, 10),
                                        ForeColor = Color.Black,
                                        TextAlign = ContentAlignment.MiddleCenter,
                                    };
                                    UserSession.ActionUpdatedpname += async () =>
                                    {
                                        usernamebox.Text = await _user.FindDisplayname(userid);
                                    };
                                    usernamebox.PerformLayout();
                                    PictureBox avatarBox = new PictureBox
                                    {
                                        Image = avatarImage,
                                        Width = 25,
                                        Height = 25,
                                        SizeMode = PictureBoxSizeMode.Zoom,
                                        Location = new Point(5, 5)
                                    };
                                    UserSession.ActionAvatarUpdated += async () =>
                                    {
                                        avatarBox.Image = await _avatar.LoadAvatarAsync(userid);
                                    };
                                    Button Moivaonhom = new Button
                                    {
                                        Text = $"Mời",
                                        AutoSize = true,
                                        ForeColor = Color.Black,
                                        Location = new Point(friendPanel.Width - 20, 10),
                                        TextAlign = ContentAlignment.MiddleCenter,
                                    };
                                    Moivaonhom.Click += async (s, e) =>
                                    {
                                        var result = await _groupMember.AddMembersToGroup(usernamebox.Name, _groupid, usernamebox.Text, "Member");
                                        if (result)
                                        {
                                            UserSession.UpdateGroupMember = (true, true);
                                            SendUpdate("UpdateGroupMember");
                                            form.Close();
                                        }
                                    };
                                    friendPanel.Controls.Add(usernamebox);
                                    friendPanel.Controls.Add(avatarBox);
                                    friendPanel.Controls.Add(Moivaonhom);
                                    // Thêm Panel vào FlowLayoutPanel
                                    flowLayoutPanel.Controls.Add(friendPanel);
                                }
                            }
                            else
                            {

                                MessageBox.Show("Không có dữ liệu bạn bè trả về.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            // Hiển thị lỗi từ server nếu có
                            string errorContent = await response.Content.ReadAsStringAsync();
                            MessageBox.Show($"Lỗi từ server: {errorContent}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Xử lý lỗi trong quá trình gửi request hoặc hiển thị danh sách
                        MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }


                Label label1 = new Label();
                label1.Text = $"Hoặc gửi lời mời cho họ";
                label1.Font = new Font("Arial", 12, FontStyle.Bold);
                label1.ForeColor = Color.Black;
                label1.AutoSize = true;
                label1.Location = new Point(20, flowLayoutPanel.Location.Y + flowLayoutPanel.Size.Height + 20);

                TextBox textbox = new TextBox();
                textbox.Text = $"{maloimoi}";
                textbox.Width = 440;
                textbox.Location = new Point(20, flowLayoutPanel.Location.Y + flowLayoutPanel.Size.Height + 50);
                textbox.ReadOnly = true;


                Button btnCopy = new Button();
                btnCopy.Text = "Sao chép";
                btnCopy.Location = new Point(20, textbox.Location.Y + textbox.Size.Height + 20);
                btnCopy.Click += (s, e) =>
                {
                    Thread staThread = new Thread(() =>
                    {
                        try
                        {
                            Clipboard.SetText(textbox.Text);
                        }
                        catch (Exception ex)
                        {
                            btnCopy.Click += BtnCopy_Click;
                        }
                    });
                    staThread.SetApartmentState(ApartmentState.STA);
                    staThread.Start();
                };
                Button btnClose = new Button();
                btnClose.Text = "Đóng";
                btnClose.Location = new Point(120, textbox.Location.Y + textbox.Size.Height + 20);
                btnClose.Click += (s, e) => form.Close();

                form.Controls.Add(label);
                form.Controls.Add(label1);
                form.Controls.Add(flowLayoutPanel);
                form.Controls.Add(textbox);
                form.Controls.Add(btnCopy);
                form.Controls.Add(btnClose);
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCopy_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void circularadd_Click()
        {
            try { 
            Form form = new Form();
            form.Text = $"Tạo máy chủ của bạn";
            form.Size = new Size(400, 300);
            form.StartPosition = FormStartPosition.CenterParent;
            Label label = new Label();
            label.Text = $"Tạo máy chủ của bạn";
            label.Font = new Font("Arial", 12, FontStyle.Bold);
            label.ForeColor = Color.Black;
            label.AutoSize = true;

            label.PerformLayout();
            int labelWidth = TextRenderer.MeasureText(label.Text, label.Font).Width;
            label.Location = new Point((form.ClientSize.Width - labelWidth) / 2, 50);

            label.TextAlign = ContentAlignment.MiddleCenter;

            Button btncreate = new Button();
            btncreate.Text = "Tạo Group mới";
            btncreate.Location = new Point(20, 100);
            btncreate.Size = new Size(340, 30);
            btncreate.Click += async (s, e) =>
            {
                form.Hide();
                await creategroup(form);
            };
            Button btnClose = new Button();
            btnClose.Text = "Đóng";
            btnClose.Location = new Point(300, 20);
            btnClose.Click += (s, e) => form.Close();

            Label label1 = new Label();
            label1.Text = $"Bạn đã nhận được lời mời";
            label1.Font = new Font("Arial", 12, FontStyle.Bold);
            label1.ForeColor = Color.Black;
            label1.AutoSize = true;

            label1.PerformLayout();
            int labelWidth1 = TextRenderer.MeasureText(label1.Text, label1.Font).Width;
            label1.Location = new Point((form.ClientSize.Width - labelWidth1) / 2, 150);

            label1.TextAlign = ContentAlignment.MiddleCenter;

            Button btnjoin = new Button();
            btnjoin.Text = "Tham gia group";
            btnjoin.Location = new Point(20, 200);
            btnjoin.Size = new Size(340, 30);
            btnjoin.Click += async (s, e) =>
            {
                form.Hide();
                await joingroup(form);
            };
            form.Controls.Add(label);
            form.Controls.Add(btncreate);
            form.Controls.Add(btnClose);
            form.Controls.Add(label1);
            form.Controls.Add(btnjoin);
            form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async Task creategroup(Form form1)
        {
            try { 
            Image image = global::QLUSER.Properties.Resources.group_1824145_1280;
            string imagepath = _find.find("group-1824145_1280.png");
            Form form = new Form();
            form.Text = $"Tùy chỉnh máy chủ của bạn";
            form.Size = new Size(600, 400);
            form.StartPosition = FormStartPosition.CenterParent;
            Label label = new Label();
            label.Text = $"Tùy chỉnh máy chủ của bạn";
            label.Font = new Font("Arial", 12, FontStyle.Bold);
            label.ForeColor = Color.Black;
            label.AutoSize = true;

            label.PerformLayout();
            int labelWidth = TextRenderer.MeasureText(label.Text, label.Font).Width;
            label.Location = new Point((form.ClientSize.Width - labelWidth) / 2, 20);

            label.TextAlign = ContentAlignment.MiddleCenter;

            CircularPicture grouppicture = new CircularPicture();
            try
            {
                grouppicture.Image = global::QLUSER.Properties.Resources.Upload_Icon_Logo_PNG_Clipart_Background;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể tải ảnh: {ex.Message}");
                return;
            }
            grouppicture.SizeMode = PictureBoxSizeMode.Zoom;
            grouppicture.Location = new Point((form.ClientSize.Width - grouppicture.Width) / 2, 50);
            grouppicture.Size = new Size(100, 100);
            grouppicture.Anchor = AnchorStyles.None;
            grouppicture.Click += async (s, e) =>
            {
                Dictionary<Image, string> imagePaths = await doianh();

                if (imagePaths != null && imagePaths.Count > 0)
                {
                    Image selectedImage = imagePaths.Keys.First();
                    grouppicture.Image = selectedImage;

                    image = selectedImage;

                    imagepath = imagePaths[selectedImage];
                }

            };

            Label tenmaychu = new Label();
            tenmaychu.Text = $"Tên máy chủ";
            tenmaychu.Font = new Font("Arial", 12, FontStyle.Bold);
            tenmaychu.ForeColor = Color.Black;
            tenmaychu.AutoSize = true;

            tenmaychu.PerformLayout();
            tenmaychu.Location = new Point(20, 200);
            tenmaychu.TextAlign = ContentAlignment.MiddleCenter;

            TextBox ten = new TextBox();
            ten.Text = $"máy chủ của {_dpname}";
            ten.Font = new Font("Arial", 12, FontStyle.Bold);
            ten.ForeColor = Color.Black;
            ten.Size = new Size(540, 30);
            UserSession.ActionUpdatedpname += () =>
            {
                ten.Text = $"máy chủ của {_dpname}";
            };
            ten.PerformLayout();
            ten.Location = new Point(20, 250);

            Button btncreate = new Button();
            btncreate.Text = "Tạo";
            btncreate.AutoSize = AutoSize;
            btncreate.Location = new Point(560 - btncreate.Width, 320);
            btncreate.Click += async (s, e) =>
            {
                if (!string.IsNullOrEmpty(ten.Text))
                {
                    var result = await _group.SaveGroupToDatabase(ten.Text,0);
                    if (result.issuccess)
                    {
                        var result1 = await _groupMember.AddMembersToGroup(_userid, result.groupid,_dpname,"Owner");
                        if (result1)
                        {
                            var result2 = await _danhmuc.SaveDanhMucToDatabase(result.groupid, "Chat");
                            if (result2.issuccess)
                            {
                                await _channel.SaveKenhToDatabase(result.groupid, "ChatChung", true, result2.danhmucID);
                            }
                            result2 = await _danhmuc.SaveDanhMucToDatabase(result.groupid, "Video");
                            if (result2.issuccess)
                            {
                                await _channel.SaveKenhToDatabase(result.groupid, "Videochung", false, result2.danhmucID);
                            }
                            string avatargroupUrl = await _avatar.UploadAvatarGroupAsync(imagepath, result.groupid);
                            if (avatargroupUrl != null)
                            {
                                UserSession.UpdateGroup = (true, true);
                                SendUpdate("GroupDislay");
                                MessageBox.Show("Avatar group uploaded successfully!");
                            }
                            else
                            {
                                MessageBox.Show("Failed to upload avatar.");
                            }
                        }
                        form.Close();
                        form1.Close();
                    }

                }
            };
            Button btnClose = new Button();
            btnClose.Text = "Trở lại";
            btnClose.Location = new Point(20, 320);
            btnClose.AutoSize = AutoSize;
            btnClose.Click += (s, e) =>
            {
                form1.Show();
                form.Close();
            };
            form.Controls.Add(label);
            form.Controls.Add(btnClose);
            form.Controls.Add(tenmaychu);
            form.Controls.Add(ten);
            form.Controls.Add(grouppicture);
            form.Controls.Add(btnClose);
            form.Controls.Add(btncreate);
            form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private async Task joingroup(Form form1)
        {
            try { 
            Form form = new Form();
            form.Text = $"Tham gia máy chủ";
            form.Size = new Size(600, 200);
            form.StartPosition = FormStartPosition.CenterParent;
            Label label = new Label();
            label.Text = $"Tham gia máy chủ";
            label.Font = new Font("Arial", 12, FontStyle.Bold);
            label.ForeColor = Color.Black;
            label.AutoSize = true;

            label.PerformLayout();
            int labelWidth = TextRenderer.MeasureText(label.Text, label.Font).Width;
            label.Location = new Point((form.ClientSize.Width - labelWidth) / 2, 20);

            label.TextAlign = ContentAlignment.MiddleCenter;



            Label tenmaychu = new Label();
            tenmaychu.Text = $"Liên kết mời";
            tenmaychu.Font = new Font("Arial", 12, FontStyle.Bold);
            tenmaychu.ForeColor = Color.Black;
            tenmaychu.AutoSize = true;

            tenmaychu.PerformLayout();
            tenmaychu.Location = new Point(20, 50);
            tenmaychu.TextAlign = ContentAlignment.MiddleCenter;

            TextBox ten = new TextBox();
            ten.Text = $"";
            ten.Font = new Font("Arial", 12, FontStyle.Bold);
            ten.ForeColor = Color.Black;
            ten.Size = new Size(540, 30);

            ten.PerformLayout();
            ten.Location = new Point(20, 80);

            Button btnjoin = new Button();
            btnjoin.Text = "Tham gia máy chủ";
            btnjoin.AutoSize = AutoSize;
            btnjoin.Location = new Point(560 - btnjoin.Width, 120);
            btnjoin.Click += async (s, e) =>
            {
                if (!string.IsNullOrEmpty(ten.Text))
                {

                    if (!string.IsNullOrEmpty(_dpname))
                    {
                        string groupid = await _group.FindGroupID(ten.Text);
                        if (groupid != null)
                        {
                            bool add = await _groupMember.AddMembersToGroup(_userid, groupid, _dpname, "Member");
                            if (add)
                            {
                                string avatargroupUrl = await _avatar.LoadGroupUrlAsync(groupid);
                                if (avatargroupUrl != null)
                                {
                                    UserSession.UpdateGroupMember = (true, true);
                                    SendUpdate("UpdateGroupMember");
                                }
                                form.Close();
                                form1.Close();
                            }
                        }
                    }
                }
            };
            Button btnClose = new Button();
            btnClose.Text = "Trở lại";
            btnClose.Location = new Point(20, 120);
            btnClose.AutoSize = AutoSize;
            btnClose.Click += (s, e) =>
            {
                form1.Show();
                form.Close();
            };
            form.Controls.Add(label);
            form.Controls.Add(btnClose);
            form.Controls.Add(tenmaychu);
            form.Controls.Add(ten);
            form.Controls.Add(btnjoin);
            form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private async Task<Dictionary<Image, string>> doianh()
        {
            try { 
            Dictionary<Image, string> imagePaths = new Dictionary<Image, string>();
            var tcs = new TaskCompletionSource<Dictionary<Image, string>>();

            Thread thread = new Thread(() =>
            {
                try
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog
                    {
                        Multiselect = false,
                        Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                    };

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string selectedFile = openFileDialog.FileName;
                        Image img = Image.FromFile(selectedFile);

                        imagePaths.Add(img, selectedFile);
                        tcs.SetResult(imagePaths);
                    }
                    else
                    {
                        tcs.SetResult(imagePaths);
                    }
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            imagePaths = await tcs.Task;

            return imagePaths;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        private void TaoKenh(string danhmucid, TreeNode node)
        {
            try
            {
                Form form = new Form();
                form.Text = $"Tạo kênh";
                form.Size = new Size(600, 400);
                form.StartPosition = FormStartPosition.CenterParent;
                Label label = new Label();
                label.Text = $"Tạo kênh";
                label.Font = new Font("Arial", 12, FontStyle.Bold);
                label.ForeColor = Color.Black;
                label.AutoSize = true;

                label.PerformLayout();
                label.Location = new Point(20, 20);

                label.TextAlign = ContentAlignment.MiddleCenter;



                Label Loaikenh = new Label();
                Loaikenh.Text = $"Loại kênh";
                Loaikenh.Font = new Font("Arial", 12, FontStyle.Bold);
                Loaikenh.ForeColor = Color.Black;
                Loaikenh.AutoSize = true;

                Loaikenh.PerformLayout();
                Loaikenh.Location = new Point(20, 70);
                Loaikenh.TextAlign = ContentAlignment.MiddleCenter;

                RadioButton vanban = new RadioButton();
                vanban.Text = "Văn bản";
                vanban.Checked = true;
                vanban.Size = new Size(540, 30);
                vanban.Location = new Point(20, 100);

                RadioButton giongnoi = new RadioButton();
                giongnoi.Text = "Giọng nói";
                giongnoi.Checked = false;
                giongnoi.Size = new Size(540, 30);
                giongnoi.Location = new Point(20, 140);

                Label Tenkenh = new Label();
                Tenkenh.Text = $"Tên kênh";
                Tenkenh.Font = new Font("Arial", 12, FontStyle.Bold);
                Tenkenh.ForeColor = Color.Black;
                Tenkenh.AutoSize = true;

                Tenkenh.PerformLayout();
                Tenkenh.Location = new Point(20, 190);
                Tenkenh.TextAlign = ContentAlignment.MiddleCenter;

                TextBox ten = new TextBox();
                ten.Text = $"";
                ten.Font = new Font("Arial", 12, FontStyle.Bold);
                ten.ForeColor = Color.Black;
                ten.Size = new Size(540, 30);

                ten.PerformLayout();
                ten.Location = new Point(20, 230);

                Button btncreate = new Button();
                btncreate.Text = "Tạo kênh";
                btncreate.AutoSize = AutoSize;
                btncreate.Location = new Point(560 - btncreate.Width, 280);
                btncreate.Click += async (s, e) =>
                {
                    if (!string.IsNullOrEmpty(ten.Text) && lb_TenGroup.Text != "")
                    {

                        if (!string.IsNullOrEmpty(_dpname))
                        {
                            if (vanban.Checked == true)
                            {
                                var result = await _channel.SaveKenhToDatabase(_groupid, ten.Text, true, danhmucid);
                                if (result.issuccess)
                                {

                                    if (node != null)
                                    {
                                        node.Nodes.Add($"VanBanChat|{result.channelID}", ten.Text);
                                    }
                                    else tv_Kenh_DanhMuc.Nodes.Add($"VanBanChat|{result.channelID}", ten.Text);
                                    await ChangeLabelLocation();
                                    form.Close();
                                }
                            }
                            else
                            {
                                var result = await _channel.SaveKenhToDatabase(_groupid, ten.Text, false, danhmucid);
                                if (result.issuccess)
                                {
                                    if (node != null)
                                    {
                                        node.Nodes.Add($"CuocGoiVideo|{result.channelID}", ten.Text);
                                    }
                                    else tv_Kenh_DanhMuc.Nodes.Add($"CuoiGoiVideo|{result.channelID}", ten.Text);
                                    await ChangeLabelLocation();
                                    form.Close();
                                }
                            }

                        }
                    }
                    else if (string.IsNullOrEmpty(ten.Text))
                    {
                        MessageBox.Show("Vui lòng nhập tên kênh.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (lb_TenGroup.Text == "") MessageBox.Show("Tạo kênh thất bại");
                };
                Button btnClose = new Button();
                btnClose.Text = "Hủy bỏ";
                btnClose.Location = new Point(560 - btncreate.Width - btnClose.Width - 20, 280);
                btnClose.AutoSize = AutoSize;
                btnClose.Click += (s, e) =>
                {
                    form.Close();
                };
                form.Controls.Add(label);
                form.Controls.Add(btnClose);
                form.Controls.Add(Loaikenh);
                form.Controls.Add(vanban);
                form.Controls.Add(giongnoi);
                form.Controls.Add(ten);
                form.Controls.Add(Tenkenh);
                form.Controls.Add(btncreate);
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void TaoDanhMuc()
        {
            try { 
            Form form = new Form();
            form.Text = $"Tạo danh mục";
            form.Size = new Size(400, 200);
            form.StartPosition = FormStartPosition.CenterParent;
            Label label = new Label();
            label.Text = $"Tạo danh mục";
            label.Font = new Font("Arial", 12, FontStyle.Bold);
            label.ForeColor = Color.Black;
            label.AutoSize = true;
            label.PerformLayout();
            label.Location = new Point(20, 10);

            label.TextAlign = ContentAlignment.MiddleCenter;

            Label Tendanhmuc = new Label();
            Tendanhmuc.Text = $"Tên danh mục";
            Tendanhmuc.Font = new Font("Arial", 12, FontStyle.Bold);
            Tendanhmuc.ForeColor = Color.Black;
            Tendanhmuc.AutoSize = true;

            Tendanhmuc.PerformLayout();
            Tendanhmuc.Location = new Point(20, 50);
            Tendanhmuc.TextAlign = ContentAlignment.MiddleCenter;

            TextBox ten = new TextBox();
            ten.Text = $"";
            ten.Font = new Font("Arial", 12, FontStyle.Bold);
            ten.ForeColor = Color.Black;
            ten.Size = new Size(340, 30);

            ten.PerformLayout();
            ten.Location = new Point(20, 90);

            Button btncreate = new Button();
            btncreate.Text = "Tạo danh mục";
            btncreate.AutoSize = AutoSize;
            btncreate.Location = new Point(360 - btncreate.Width, 130);
            btncreate.Click += async (s, e) =>
            {
                if (!string.IsNullOrEmpty(ten.Text) && lb_TenGroup.Text != "")
                {

                    var result = await _danhmuc.SaveDanhMucToDatabase(_groupid, ten.Text);
                    if (result.issuccess)
                    {
                        TreeNode node = tv_Kenh_DanhMuc.Nodes.Add($"DanhMuc|{result.danhmucID}", ten.Text);
                        await createlabel(node);
                        form.Close();
                    }
                }
                else if (string.IsNullOrEmpty(ten.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên danh mục.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (lb_TenGroup.Text == "") MessageBox.Show("Tạo danh mục thất bại");
            };
            Button btnClose = new Button();
            btnClose.Text = "Hủy bỏ";
            btnClose.Location = new Point(360 - btncreate.Width - btnClose.Width - 20, 130);
            btnClose.AutoSize = AutoSize;
            btnClose.Click += (s, e) =>
            {
                form.Close();
            };
            form.Controls.Add(label);
            form.Controls.Add(btnClose);

            form.Controls.Add(ten);
            form.Controls.Add(Tendanhmuc);
            form.Controls.Add(btncreate);
            form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private async Task createlabel(TreeNode node)
        {
            try { 
            Label label = new Label();
            label.Size = new Size(13, 13);
            label.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            label.ForeColor = System.Drawing.Color.White;
            label.Size = new System.Drawing.Size(13, 13);
            label.Text = "+";
            int videoY = node.Bounds.Location.Y;
            //Point point = label4.Location;
            int x = tv_Kenh_DanhMuc.Location.X + tv_Kenh_DanhMuc.Size.Width - label.Size.Width - 1;
            int y = tv_Kenh_DanhMuc.Location.Y + 1 + videoY;
            label.Location = new Point(x, y);
            label.Click += (s, e) =>
            {
                string[] danhmucid = node.Name.Split('|');
                TaoKenh(danhmucid[1], node);

            };
            P_Kenh_DanhMuc.Controls.Add(label);
            label.BringToFront();
            node.Tag = label;
            if (panelMenu != null) panelMenu.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cp_Menu_Click(object sender, EventArgs e)
        {
            TC_Chat.SelectedIndex = 1; // 1 là menu k có chat
            TC_ServerOrFriend.SelectedIndex = 0; //0 là menu hiện danh sách bạn bè
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TC_ServerOrFriend.SelectedIndex = 1;
            //CreateMenu();
        }
        private void btn_Ketban_Click(object sender, EventArgs e)
        {
            SearchUser friend = new SearchUser(_userid,this);
            friend.ShowDialog();
        }
        private Action actionShowFriendHandler;
        private Action actionShowFriendRequestHandler;
        private async void button2_Click(object sender, EventArgs e)
        {
            showfriendrequest(_userid);

            if (actionShowFriendHandler == null)
                actionShowFriendHandler = () => ShowFriendHandler(_userid);
            if (actionShowFriendRequestHandler == null)
                actionShowFriendRequestHandler = () => ShowFriendRequestHandler(_userid);
            UserSession.ActionUpdateFriend -= actionShowFriendHandler;
            UserSession.ActionUpdateFriendRequest -= actionShowFriendRequestHandler;
            UserSession.ActionUpdateFriendRequest += actionShowFriendRequestHandler;
        }
        public void ShowFriendHandler(string _userid)
        {
            UserSession.RunOnUIThread(new Action(()=> showfriend(_userid)));
        }

        private void ShowFriendRequestHandler(string _userid)
        {
            UserSession.RunOnUIThread(new Action(() => showfriendrequest(_userid)));
        }

        private async void showfriendrequest(string userid1)
        {
            using (HttpClient _httpClient = new HttpClient())
            {
                try
                {
                    flp_Friends.Controls.Clear();
                    string requestUrl = $"{ConfigurationManager.AppSettings["ServerUrl"]}Friend/Request/{userid1}";

                    HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);

                        if (responseData != null && responseData.sender != null)
                        {
                            foreach (string Displayname in responseData.sender)
                            {
                                string userid = await _user.finduserid(Displayname);
                                Image avatarImage = await _avatar.LoadAvatarAsync(userid);

                                Panel userPanel = new Panel
                                {
                                    Width = flp_Friends.Width,
                                    Height = 60,
                                    BorderStyle = BorderStyle.FixedSingle
                                };

                                PictureBox avatarBox = new PictureBox
                                {
                                    Image = avatarImage,
                                    Width = 50,
                                    Height = 50,
                                    SizeMode = PictureBoxSizeMode.Zoom,
                                    Location = new Point(5, 5)
                                };
                                UserSession.ActionAvatarUpdated += async () =>
                                {
                                    avatarBox.Image = await _avatar.LoadAvatarAsync(userid);
                                };
                                Label usernameLabel = new Label
                                {
                                    Text = Displayname,
                                    AutoSize = true,
                                    Location = new Point(65, 20),
                                    ForeColor = Color.White
                                };
                                UserSession.ActionUpdatedpname += async () =>
                                {
                                    usernameLabel.Text = await _user.FindDisplayname(userid);
                                };
                                Button acceptButton = new Button
                                {
                                    Text = "Chấp nhận",
                                    Tag = Displayname,
                                    Location = new Point(userPanel.Width - 190, 15),
                                    Width = 90,
                                    ForeColor = Color.White
                                };

                                acceptButton.Click += async (s, ev) =>
                                {
                                    await AcceptFriendRequest(Displayname);
                                };

                                Button rejectButton = new Button
                                {
                                    Text = "Từ chối",
                                    Tag = Displayname,
                                    Location = new Point(userPanel.Width - 100, 15),
                                    Width = 90,
                                    ForeColor = Color.White
                                };

                                rejectButton.Click += async (s, ev) =>
                                {
                                    await RejectFriendRequest(Displayname,_dpname);
                                };

                                userPanel.Controls.Add(avatarBox);
                                userPanel.Controls.Add(usernameLabel);
                                userPanel.Controls.Add(acceptButton);
                                userPanel.Controls.Add(rejectButton);
                                flp_Friends.Controls.Add(userPanel);
                            }
                            UserSession.ActionDeleteuser += () =>
                            {
                                showfriendrequest(userid1);
                            };
                        }
                        else
                        {
                            MessageBox.Show("Không có dữ liệu trả về.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        if (responseData != null && responseData.receiver != null)
                        {
                            foreach (string Displayname in responseData.receiver)
                            {
                                string userid = await _user.finduserid(Displayname);
                                Image avatarImage = await _avatar.LoadAvatarAsync(userid);

                                Panel userPanel = new Panel
                                {
                                    Width = flp_Friends.Width,
                                    Height = 60,
                                    BorderStyle = BorderStyle.FixedSingle
                                };

                                PictureBox avatarBox = new PictureBox
                                {
                                    Image = avatarImage,
                                    Width = 50,
                                    Height = 50,
                                    SizeMode = PictureBoxSizeMode.Zoom,
                                    Location = new Point(5, 5)
                                };
                                UserSession.ActionAvatarUpdated += async () =>
                                {
                                    avatarBox.Image = await _avatar.LoadAvatarAsync(userid);
                                };
                                Label usernameLabel = new Label
                                {
                                    Text = Displayname,
                                    AutoSize = true,
                                    Location = new Point(65, 20),
                                    ForeColor = Color.White
                                };
                                UserSession.ActionUpdatedpname += async () =>
                                {
                                    usernameLabel.Text = await _user.FindDisplayname(userid);
                                };


                                Button rejectButton = new Button
                                {
                                    Text = "Xóa lời mời kết bạn",
                                    Tag = Displayname,
                                    Location = new Point(userPanel.Width - 120, 15),
                                    AutoSize =true,
                                    ForeColor = Color.White
                                };

                                rejectButton.Click += async (s, ev) =>
                                {
                                    await RejectFriendRequest(_dpname,Displayname);
                                };

                                userPanel.Controls.Add(avatarBox);
                                userPanel.Controls.Add(usernameLabel);
                                userPanel.Controls.Add(rejectButton);
                                flp_Friends.Controls.Add(userPanel);
                            }
                            UserSession.ActionDeleteuser += () =>
                            {
                                showfriendrequest(userid1);
                            };
                        }
                        else
                        {
                            MessageBox.Show("Không có dữ liệu trả về.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi hiện friend request: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private async Task RejectFriendRequest(string senderdpname,string receiverdpname)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string senderId = await _user.finduserid(senderdpname);

                    string receiverId = await _user.finduserid(receiverdpname);

                    var requestBody = new
                    {
                        senderId = int.Parse(senderId),
                        receiverId = int.Parse(receiverId),
                        action = "Decline"
                    };

                    string jsonContent = JsonConvert.SerializeObject(requestBody);
                    StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    string requestUrl = $"{ConfigurationManager.AppSettings["ServerUrl"]}Friend/Respond";
                    HttpResponseMessage response = await client.PostAsync(requestUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        UserSession.UpdateFriendRequest = true;
                        SendUpdate("UpdateFriendRequest");
                        MessageBox.Show($"Đã xóa lời mời kết bạn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Lỗi từ server khi xóa lời mời: {errorContent}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa lời mời: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async Task AcceptFriendRequest(string displayname)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string senderId = await _user.finduserid(displayname);

                    int receiverId = int.Parse(_userid);

                    var requestBody = new
                    {
                        senderId = int.Parse(senderId),
                        receiverId = receiverId,
                        action = "Accept"
                    };

                    string jsonContent = JsonConvert.SerializeObject(requestBody);
                    StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    string requestUrl = $"{ConfigurationManager.AppSettings["ServerUrl"]}Friend/Respond";
                    HttpResponseMessage response = await client.PostAsync(requestUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        UserSession.UpdateFriendRequest = true;
                        SendUpdate("UpdateFriendRequest");
                        MessageBox.Show($"Đã chấp nhận lời mời kết bạn từ {displayname}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Lỗi từ server khi chấp nhận lời mời: {errorContent}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chấp nhận lời mời: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        bool on = false;
        private async void USERINFOR(string gdpname)
        {
            try { 
            Form form = new Form();
            form.Text = $"Thông tin người dùng";
            form.Size = new Size(500, 300);
            form.StartPosition = FormStartPosition.CenterParent;

            form.Deactivate += (s, e) =>
            {
                if (!on)
                    form.Close();

            };
            UserSession.ActionDeleteuser += () =>
            {
                if (form != null && !form.IsDisposed)
                {
                    form.Close();
                }
            };
            string gdpid = await _groupMember.FindGroupDisplayID(_groupid,gdpname);
            CircularPicture userpicture = new CircularPicture();
            try
            {
                userpicture.Image = await _avatar.LoadAvatarAsync(gdpid);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể tải ảnh: {ex.Message}");
                return;
            }
            userpicture.SizeMode = PictureBoxSizeMode.Zoom;
            userpicture.Location = new Point(20, 50);
            userpicture.Size = new Size(50, 50);
            userpicture.Anchor = AnchorStyles.None;
            userpicture.Click += async (s, e) =>
            {
                GroupUser user = new GroupUser(gdpid, _userid,_groupid,this);
                user.ShowDialog();
            };
            UserSession.ActionAvatarUpdated += async () =>
            {
                userpicture.Image = await _avatar.LoadAvatarAsync(gdpid);
            };

            Label label = new Label();
            label.Text = $"{gdpname}";
            UserSession.ActionUpdategdpname += async () =>
            {
                label.Text = await _groupMember.FindGroupDisplayname(gdpid, _groupid);
            };
            label.Font = new Font("Arial", 12, FontStyle.Bold);
            label.ForeColor = Color.Black;
            label.AutoSize = true;
            label.PerformLayout();
            int labelWidth = TextRenderer.MeasureText(label.Text, label.Font).Width;
            label.Location = new Point(20, 120);
            label.TextAlign = ContentAlignment.MiddleCenter;

            var result = await _group.RequestGroupName(gdpid,0);
            var result1 = await _group.RequestGroupName(_userid,0);
            string[] commongroupnames = result.groupidname.Intersect(result1.groupidname).ToArray();
            string[] commonGroupNames = commongroupnames.Take(3).ToArray();
            for (int i = 0; i < commonGroupNames.Length; i++)
            {
                CircularPicture circulargroup = new CircularPicture();
                try
                {
                    string[] group = commonGroupNames[i].Split('|');
                    circulargroup.Image = await _avatar.LoadAvatarGroupAsync(group[0]);
                    circulargroup.SizeMode = PictureBoxSizeMode.Zoom;
                    circulargroup.Text = group[1];
                    circulargroup.Name = $"group|{group[0]}";
                    UserSession.ActionAvatarGroupUpdated += async () =>
                    {
                        circulargroup.Image = await _avatar.LoadAvatarGroupAsync(group[0]);
                    };
                    UserSession.ActionUpdateGroupname += async () =>
                    {
                        circulargroup.Text = await _group.Groupname(group[0]);
                    };
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Không thể tải ảnh: {ex.Message}");
                    return;
                }
                circulargroup.Location = new Point(20 + i * 30, 150);
                circulargroup.Size = new Size(25, 25);
                circulargroup.Anchor = AnchorStyles.None;
                circulargroup.Click += async (s, e) =>
                {
                    GroupUser user = new GroupUser(gdpid, _userid,_groupid,this);
                    user.ShowDialog();
                };
                form.Controls.Add(circulargroup);
            }

            Label label2 = new Label();
            label2.Text = $"{commongroupnames.Length} Máy Chủ Chung";
            label2.Font = new Font("Arial", 12, FontStyle.Bold);
            label2.ForeColor = Color.Black;
            label2.AutoSize = true;
            label2.PerformLayout();
            label2.Location = new Point(20 + commonGroupNames.Length * 30, 150);
            label2.TextAlign = ContentAlignment.MiddleCenter;

            TextBox nhantin = new TextBox();
            nhantin.Text = "";
            nhantin.Font = new Font("Arial", 12, FontStyle.Bold);
            nhantin.ForeColor = Color.Black;
            nhantin.Size = new Size(440, 30);
            nhantin.PerformLayout();
            nhantin.Location = new Point(20, 200);
            nhantin.KeyDown += async (s, e) =>
            {
                if (!string.IsNullOrEmpty(nhantin.Text))
                {
                    on = true;
                    nhantin_KeyDown(s, e, gdpid, nhantin);
                    on = false;
                }
            };

            Button thongtin = new Button();
            thongtin.Text = "...";
            thongtin.AutoSize = AutoSize;
            thongtin.Location = new Point(460 - thongtin.Width, 20);
            thongtin.Click += async (s, e) =>
            {
                if (form1 == null) TrangChu(gdpname, gdpid);
                else
                {
                    if (form2 != null)
                    {
                        form2.Dispose();
                        form2.Close();
                        form2 = null;
                    }
                    form1.Dispose();
                    form1.Close();
                    on = false;
                    form1 = null;
                }
            };

            Button banbe = new Button();
            banbe.Text = "🤵";
            banbe.Location = new Point(460 - thongtin.Width - banbe.Width - 20, 20);
            banbe.AutoSize = AutoSize;
            banbe.Click += async (s, e) =>
            {
                on = true;
                string dpname = await _user.FindDisplayname(gdpid);
                SearchUser user = new SearchUser(_userid,this);
                await user.GuiKetBan(dpname);
                on = false;
            };

            form.Controls.Add(userpicture);
            form.Controls.Add(label);
            if (_gdpname != gdpname)
            {
                form.Controls.Add(label2);
                form.Controls.Add(nhantin);
                form.Controls.Add(thongtin);
                form.Controls.Add(banbe);
            }
            form.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void nhantin_KeyDown(object sender, KeyEventArgs e,string gdpid,TextBox nhantin)
        {
            try { 
            if (e.KeyCode == Keys.Enter)
            {
                string message = nhantin.Text;
                var groupidname = await _group.FindGroup(_userid, gdpid);
                var result = await _user.FindDisplayname(gdpid);
                string channelid="";
                if (string.IsNullOrEmpty(groupidname))
                { 
                    var result1 = await Createnhantinrieng(result, gdpid);
                    channelid = result1.channelid;
                    
                }
                else
                {
                    string[] groupid1 = groupidname.Split('|');
                    var result1 = await _channel.RequestChannelName(groupid1[0]);
                    if(result1.issuccess)
                    {
                        string[] channelid1 = result1.channelidname[0].Split('|');
                        channelid = channelid1[0];
                    }
                }
                var result2 = await _message.SendMessage(channelid, _userid, message);
                nhantin.Text = "";

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<(string groupid, string channelid)> Createnhantinrieng(string dpname, string userid)
        {
            try { 
            var result = await _group.SaveGroupToDatabase("Chathainguoi", 2);
            if (result.issuccess)
            {
                var result1 = await _groupMember.AddMembersToGroup(_userid, result.groupid, _dpname, "Owner");
                var result2 = await _groupMember.AddMembersToGroup(userid, result.groupid, dpname, "Owner");
                if (result1 && result2)
                {
                    var result3 = await _channel.SaveKenhToDatabase(result.groupid, "ChatRieng", true, null);
                    if (result3.issuccess)
                    {
                        UserSession.UpdateGroup = (true, false);
                        SendUpdate("GroupPrivateDislay");
                        return (result.groupid, result3.channelID);
                    }
                }
            }
            return (null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return (null, null);
            }
        }


        Form form1;
        private void TrangChu(string gdpname, string gdpid)
        {
            try { 
            on = true;
            form1 = new Form();
            form1.Text = $"Trang chủ";
            form1.Size = new Size(200, 200);
            form1.StartPosition = FormStartPosition.CenterParent;
            form1.FormClosed += (s1, e1) =>
            {
                if (form2 != null)
                {
                    form2.Dispose();
                    form2.Close();
                    form2 = null;
                }
                on = false;
                form1 = null;

            };
            Button button = new Button();
            button.Text = "Xem ho so day du";
            button.AutoSize = true;
            button.Location = new Point(20, 20);
            button.Click += (s1, e1) =>
            {
                GroupUser user = new GroupUser(gdpid, _userid,_groupid,this);
                user.Show();
            };
            Button moivaomaychu = new Button();
            moivaomaychu.Text = "moi vao may chu";
            moivaomaychu.AutoSize = true;
            moivaomaychu.Location = new Point(20, 60);
            moivaomaychu.Click += async (s1, e1) =>
            {

                if (form2 == null) await moivaogroup(gdpid);
                else
                {
                    form2.Dispose();
                    form2.Close();
                    form2 = null;
                }


            };

            form1.Controls.Add(button);
            form1.Controls.Add(moivaomaychu);
            form1.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        Form form2;
        public async Task moivaogroup(string gdpid)
        {
            try { 
            form2 = new Form();
            form2.Text = $"moi vao may chu";
            form2.AutoSize = AutoSize;
            form2.StartPosition = FormStartPosition.CenterParent;

            // Create a FlowLayoutPanel to hold the buttons
            FlowLayoutPanel flowLayoutPanel1 = new FlowLayoutPanel();
            flowLayoutPanel1.Dock = DockStyle.Fill;  // Dock the panel to fill the form
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;  // Arrange buttons vertically
            flowLayoutPanel1.WrapContents = false;  // Don't wrap buttons to next row
            flowLayoutPanel1.AutoScroll = true; // Enable scrolling if buttons overflow

            // Add the FlowLayoutPanel to the form
            form2.Controls.Add(flowLayoutPanel1);

            // Get group names from the server
            var result = await _group.RequestGroupName(_userid,0);
            if (result.issuccess)
            {
                for (int i = 0; i < result.groupidname.Length; i++)
                {
                    string[] group1 = result.groupidname[i].Split('|');
                    Button moivaogroup = new Button();
                    moivaogroup.Name = $"group|{group1[0]}";
                    moivaogroup.Text = group1[1];
                    UserSession.ActionUpdateGroupname += async () =>
                    {
                        moivaogroup.Text = await _group.Groupname(group1[0]); 
                    };
                    moivaogroup.Size = new Size(120, 30);  // Adjust the size of the buttons
                    moivaogroup.Click += async (s2, e2) =>
                    {
                        string displayname = await _user.FindDisplayname(gdpid);
                        await _groupMember.AddMembersToGroup(gdpid, group1[0],displayname,"Member");
                        UserSession.UpdateGroupMember = (true, true);
                        SendUpdate("UpdateGroupMember");
                    };

                    // Add the button to the FlowLayoutPanel
                    flowLayoutPanel1.Controls.Add(moivaogroup);
                }
            }
            form2.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void button7_Click(object sender, EventArgs e)
        {
            var existingEmojiPanel = TP_ChattingView.Controls.OfType<Panel>().FirstOrDefault(p => p.Name == "emojiPanel");

            if (existingEmojiPanel != null)
            {
                existingEmojiPanel.Visible = !existingEmojiPanel.Visible;
            }
            else
            {
                await InitializeEmojiPanelAsync();
            }
        }

        private static readonly HttpClient client = new HttpClient();

        private async Task InitializeEmojiPanelAsync()
        {
            try { 
            string emojiApiUrl = "https://emoji-api.com/emojis?access_key=ee48e0cefa8273d4ac574b446253f2637877fb12";
            Panel emojiPanel = new Panel
            {
                Name = "emojiPanel",
                Size = new Size(300, 300),
                Location = new Point(496-300, 407-300),
                AutoScroll = true,
                Visible = true
            };
            int x = 10, y = 10;
            var response = await client.GetStringAsync(emojiApiUrl);
            var emojis = JsonConvert.DeserializeObject<Emoji[]>(response);
            foreach (var emoji in emojis)
            {
                {
                    Button emojiPictureBox = new Button
                    {
                        Text = emoji.character,
                        Font = new Font("Segoe UI Emoji", 12),
                        Location = new Point(x, y),
                        Size = new Size(40, 40)
                    };
                    emojiPictureBox.Click += (s, e) => AddEmojiToTextBox(emoji);
                    emojiPanel.Controls.Add(emojiPictureBox);

                    x += 45;
                    if (x > emojiPanel.Width - 40)
                    {
                        x = 10;
                        y += 45;
                    }
                }
            }
            TP_ChattingView.Controls.Add(emojiPanel);
            emojiPanel.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
        }

        private void AddEmojiToTextBox(Emoji emoji)
        {
            tb_Message.Text += emoji.character;
        }

        private async void button8_Click(object sender, EventArgs e)
        {
            try { 
            if (lb_TenKenh.Text == "")
            {
                MessageBox.Show("Tìm kiếm thất bại");
                return;
            }
            flp_Message.Controls.Clear();
            flp_Message.AutoScrollPosition = new Point(0, 0);
            flp_Message.PerformLayout();

            SearchMessage searchMessage = new SearchMessage(_channelid,this);
            if (searchMessage.ShowDialog() == DialogResult.OK) 
            {
                string[] messages = searchMessage.message;
                for (int i = messages.Length - 1; i > 0; i -= 4)
                {
                    if (i - 3 < messages.Length)
                    {
                        string messageid = messages[i - 3];
                        string gdpname = messages[i - 2];
                        string message = messages[i - 1];
                        string filename = messages[i];
                        string[] filenames = filename.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries);
                        await AddMessageToChat(messageid, gdpname, message, filenames);
                        
                    }
                }
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tabControl2_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen p = new Pen(Color.Blue, 4);
            g.DrawRectangle(p, this.TP_ChattingView.Bounds);
        }
        private async void btn_XemListFriend_Click(object sender, EventArgs e)
        {
            showfriend(_userid);
            if (actionShowFriendHandler == null)
                actionShowFriendHandler = () => ShowFriendHandler(_userid);
            if (actionShowFriendRequestHandler == null)
                actionShowFriendRequestHandler = () => ShowFriendRequestHandler(_userid);
            UserSession.ActionUpdateFriend -= actionShowFriendHandler;
            UserSession.ActionUpdateFriend += actionShowFriendHandler;
            UserSession.ActionUpdateFriendRequest -= actionShowFriendRequestHandler;

        }
        public async void showfriend(string userid1)
        {
            using (HttpClient _httpClient = new HttpClient())
            {
                try
                {

                    flp_Friends.Controls.Clear();

                    string requestUrl = $"{ConfigurationManager.AppSettings["ServerUrl"]}Friend/{userid1}";

                    HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);

                        if (responseData != null && responseData.message != null)
                        {
                            foreach (string displayName in responseData.message)
                            {
                                string userid = await _user.finduserid(displayName);
                                Image avatarImage = await _avatar.LoadAvatarAsync(userid);
                                Panel friendPanel = new Panel
                                {
                                    Width = flp_Friends.Width - 20,
                                    Height = 60,
                                    BorderStyle = BorderStyle.FixedSingle
                                };
                                Label usernameLabel = new Label
                                {
                                    Text = displayName,
                                    AutoSize = true,
                                    Location = new Point(65, 20),
                                    ForeColor = Color.White
                                };
                                UserSession.ActionUpdatedpname += async () =>
                                {
                                    usernameLabel.Text = await _user.FindDisplayname(userid);
                                };
                                PictureBox avatarBox = new PictureBox
                                {
                                    Image = avatarImage,
                                    Width = 50,
                                    Height = 50,
                                    SizeMode = PictureBoxSizeMode.Zoom,
                                    Location = new Point(5, 5)
                                };
                                UserSession.ActionAvatarUpdated += async () =>
                                {
                                    avatarBox.Image = await _avatar.LoadAvatarAsync(userid);
                                };
                                Label XoaKetBan = new Label
                                {
                                    Text = "X",
                                    AutoSize = true,
                                    Location = new Point(friendPanel.Width-20, 20),
                                    ForeColor = Color.White
                                };
                                XoaKetBan.Click += async (s,e) => await HamXoaKetBan(displayName, _dpname);
                                

                              
                                // Thêm các điều khiển vào Panel
                                friendPanel.Controls.Add(XoaKetBan);
                                friendPanel.Controls.Add(usernameLabel);
                                friendPanel.Controls.Add(avatarBox);
                                // Thêm Panel vào FlowLayoutPanel
                                flp_Friends.Controls.Add(friendPanel);
                            }
                            UserSession.ActionDeleteuser += () =>
                            {
                                showfriend(userid1);
                            };
                        }
                        else
                        {
                            MessageBox.Show("Không có bạn bè", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        // Hiển thị lỗi từ server nếu có
                        string errorContent = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Lỗi từ server: {errorContent}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi trong quá trình gửi request hoặc hiển thị danh sách
                    MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private async Task HamXoaKetBan(string dpname1, string dpname2)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string userid1 = await _user.finduserid(dpname1);

                    string userid2 = await _user.finduserid(dpname2);

                    var requestBody = new DeleteFriendDTO
                    {
                        UserId_1 = int.Parse(userid1),
                        UserId_2 = int.Parse(userid2),
                    };

                    string jsonContent = JsonConvert.SerializeObject(requestBody);
                    StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    string requestUrl = $"{ConfigurationManager.AppSettings["ServerUrl"]}Friend/DeleteFriend";
                    HttpResponseMessage response = await client.PostAsync(requestUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        UserSession.UpdateFriend = true;
                        SendUpdate("UpdateFriend");
                        MessageBox.Show($"Đã xóa bạn bè", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Lỗi từ server khi xóa bạn: {errorContent}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa bạn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void button10_Click_1(object sender, EventArgs e)
        {
            try { 
            bool canclose=true;
            Form form = new Form();
            form.Text = $"Chọn bạn bè";
            form.Size = new Size(500, 350);
            form.StartPosition = FormStartPosition.CenterParent;

            form.Deactivate += (s1, e1) =>
            {
                if(canclose)
                form.Close();
            };


            Label label = new Label();
            label.Text = $"Chọn bạn bè";
            label.Font = new Font("Arial", 12, FontStyle.Bold);
            label.ForeColor = Color.Black;
            label.AutoSize = true;
            label.PerformLayout();
            int labelWidth = TextRenderer.MeasureText(label.Text, label.Font).Width;
            label.Location = new Point(20, 20);
            label.TextAlign = ContentAlignment.MiddleCenter;


            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.Location = new Point(20, 100);
            panel.Size = new Size(440, 100);
            panel.FlowDirection = FlowDirection.TopDown;  // Arrange buttons vertically
            panel.WrapContents = false;  // Don't wrap buttons to next row
            panel.AutoScroll = true; // Enable scrolling if buttons overflow

            using (HttpClient _httpClient = new HttpClient())
            {
                try
                {

                    flp_Friends.Controls.Clear();

                    string requestUrl = $"{ConfigurationManager.AppSettings["ServerUrl"]}Friend/{_userid}";

                    HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);

                        if (responseData != null && responseData.message != null)
                        {
                            foreach (string displayName in responseData.message)
                            {
                                string userid = await _user.finduserid(displayName);
                                Image avatarImage = await _avatar.LoadAvatarAsync(userid);
                                Panel friendPanel = new Panel
                                {
                                    Width = panel.Width - 20,
                                    Height = 30,
                                    BorderStyle = BorderStyle.FixedSingle,
                                    AutoSize=true,
                                };
                                CheckBox usernamebox = new CheckBox
                                {
                                    Checked = false,
                                    Text = displayName,
                                    Name = userid,
                                    AutoSize = true,
                                    Location = new Point(45, 10),
                                    ForeColor = Color.Black,
                                    TextAlign = ContentAlignment.MiddleCenter,
                                    CheckAlign = ContentAlignment.MiddleRight, 
                                };
                                UserSession.ActionUpdatedpname += async () =>
                                {
                                    usernamebox.Text = await _user.FindDisplayname(userid);
                                };
                                usernamebox.PerformLayout();
                                PictureBox avatarBox = new PictureBox
                                {
                                    Image = avatarImage,
                                    Width = 25,
                                    Height = 25,
                                    SizeMode = PictureBoxSizeMode.Zoom,
                                    Location = new Point(5, 5)
                                };
                                UserSession.ActionAvatarUpdated += async () =>
                                {
                                    avatarBox.Image = await _avatar.LoadAvatarAsync(userid);
                                };

                                friendPanel.Controls.Add(usernamebox);
                                friendPanel.Controls.Add(avatarBox);
                                // Thêm Panel vào FlowLayoutPanel
                                panel.Controls.Add(friendPanel);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Không có dữ liệu bạn bè trả về.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        // Hiển thị lỗi từ server nếu có
                        string errorContent = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Lỗi từ server: {errorContent}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi trong quá trình gửi request hoặc hiển thị danh sách
                    MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


            Button thongtin = new Button();
            thongtin.Text = "Tạo Group";
            thongtin.Location = new Point(20, 250);
            thongtin.Size = new Size(440, 30);
            thongtin.Click += async (s1, e1) =>
            {
                canclose = false;
                List<CheckBox> list = new List<CheckBox>();
                foreach (Control control in panel.Controls)
                {
                    if (control is Panel friendPanel)
                    {
                        foreach (Control subControl in friendPanel.Controls)
                        {
                            if (subControl is CheckBox checkBox && checkBox.Checked)
                            {
                                list.Add(checkBox);
                            }
                        }
                    }
                }
                if (list.Count < 10&&list.Count>0)
                {
                    string imagepath = _find.find("group-1824145_1280.png");
                    var result = await _group.SaveGroupToDatabase("ChatRieng", 1);
                    if (result.issuccess)
                    {
                        var result1 = await _groupMember.AddMembersToGroup(_userid, result.groupid, _dpname, "Owner");
                        foreach (var user in list)
                        {
                            var result2 = await _groupMember.AddMembersToGroup(user.Name, result.groupid, user.Text, "Member");
                        }

                        await _channel.SaveKenhToDatabase(result.groupid, "ChatRieng", true, null);
                        string avatargroupUrl = await _avatar.UploadAvatarGroupAsync(imagepath, result.groupid);
                        if (avatargroupUrl != null)
                        {
                            UserSession.UpdateGroup = (true, false);
                            SendUpdate("GroupPrivateDislay");
                            MessageBox.Show("Avatar group uploaded successfully!");
                        }
                        else
                        {
                            MessageBox.Show("Failed to upload avatar.");
                        }
                        form.Dispose();
                        form.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Nhóm tối đa 10 người.Xin vui lòng chọn dưới 10 người");
                }
                canclose = true;
            };

            form.Controls.Add(label);

            form.Controls.Add(panel);
            form.Controls.Add(thongtin);


            form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_Banbe_Click(object sender, EventArgs e)
        {
            TC_Chat.SelectedIndex = 1;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (!AddGroupMembers)
            {
                TP_ChattingView.Controls.Add(flp_Members);
                flp_Message.Size = new Size(flp_Message.Width-flp_Members.Width, flp_Message.Height);
                flp_Members.BringToFront();
                flp_Members.Controls.Clear();
                AddGroupMembersList();
                AddGroupMembers = true;
            }
            else
            {
                TP_ChattingView.Controls.Remove(flp_Members);
                flp_Message.Size = new Size(824, 415);
                AddGroupMembers = false;
            }
        }


    }
}
