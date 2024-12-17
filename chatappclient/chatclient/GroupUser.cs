using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using QLUSER.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Configuration;
using NAudio.CoreAudioApi;
namespace QLUSER
{
    public partial class GroupUser : Form
    {
        GiaoDien _gd;
        UserAvatar avatar = new UserAvatar();
        Group group =new Group();
        GroupMember member = new GroupMember();
        string gdpid;
        string gdpidtk;
        string gdpname1 = "";
        string gdpnametk1;
        string _groupid;
        User user = new User();
        public GroupUser(string userid,string useridtk,string groupid,GiaoDien gd)
        {
            InitializeComponent();
            _gd = gd;
            gdpid = userid;
            _groupid=groupid;
            gdpidtk = useridtk;
            if(userid != useridtk)
            {
                label3.Visible = true;
                label4.Visible = true;
            }
            UserSession.ActionDeleteuser += () =>
            {
                this.Close();
            };
        }

        private async void GroupUser_Load(object sender, EventArgs e)
        {
            try { 
            gdpname1 = await member.FindGroupDisplayname(gdpid,_groupid);
            gdpnametk1 = await member.FindGroupDisplayname(gdpidtk, _groupid);
            circularPicture1.Image = await avatar.LoadAvatarAsync(gdpid);
            UserSession.ActionAvatarUpdated += UpdateAvatarDisplay;    
            circularPicture1.SizeMode = PictureBoxSizeMode.Zoom;
            circularPicture1.Anchor = AnchorStyles.None;
            label1.Text= gdpname1;
            UserSession.ActionUpdategdpname += async () =>
            {
                gdpname1 = await member.FindGroupDisplayname(gdpid, _groupid);
                gdpnametk1 = await member.FindGroupDisplayname(gdpidtk, _groupid);
                label1.Text = gdpname1;
            };
            UserSession.ActionUpdateGroup += () => {
                if (this != null && !this.IsDisposed)
                {
                    this.Close();
                }
            };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void UpdateAvatarDisplay()
        {
            circularPicture1.Image = await avatar.LoadAvatarAsync(gdpid);
          
        }
        private async void label2_Click(object sender, EventArgs e)
        {

            flowLayoutPanel1.Controls.Clear();
            Label label = new Label();
            label.Text = "Vai trò";
            flowLayoutPanel1.Controls.Add(label);
            Label role = new Label();
            role.Text =await member.FindOneUserRole(_groupid,gdpid);
            UserSession.ActionUpdateRole += async () =>
            {
                role.Text = await member.FindOneUserRole(_groupid, gdpid);
            };
            flowLayoutPanel1.Controls.Add(role);
            flowLayoutPanel1.Padding = new Padding(20, 0, 0, 10);
            Label label1 = new Label();
            label1.Text = "Gia nhập từ";
            Panel panel = new Panel();
            panel.AutoSize = true;
            CircularPicture circularPicture = new CircularPicture();
            circularPicture.Image = await avatar.LoadAvatarGroupAsync(_groupid);
            circularPicture.Location = new Point(20, 10);
            circularPicture.Size = new Size(25, 25);
            UserSession.ActionAvatarGroupUpdated += async () =>
            {
                circularPicture.Image = await avatar.LoadAvatarGroupAsync(_groupid);
            };
            panel.Controls.Add(circularPicture);
            Label time = new Label();
            time.Location = new Point(50, 15);
            time.Text = await member.FindDateJointime(gdpid, _groupid);
            panel.Controls.Add(time);
            flowLayoutPanel1.Controls.Add(label1);
            flowLayoutPanel1.Controls.Add(panel);
        }

        private Action actionShowFriendHandler;
        private void label3_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            showfriend(gdpid); 
            if (actionShowFriendHandler == null)
                actionShowFriendHandler = () => ShowFriendHandler(gdpid);
            UserSession.ActionUpdateFriend -= actionShowFriendHandler;

        }
        public void ShowFriendHandler(string _userid)
        {
            UserSession.RunOnUIThread(new Action(() => showfriend(_userid)));
        }
        public async void showfriend(string userid1)
        {
            using (HttpClient _httpClient = new HttpClient())
            {
                try
                {

                    flowLayoutPanel1.Controls.Clear();

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
                                string userid = await user.finduserid(displayName);
                                Image avatarImage = await avatar.LoadAvatarAsync(userid);
                                Panel friendPanel = new Panel
                                {
                                    Width = flowLayoutPanel1.Width - 20,
                                    Height = 60,
                                    BorderStyle = BorderStyle.FixedSingle
                                };
                                Label usernameLabel = new Label
                                {
                                    Text = displayName,
                                    AutoSize = true,
                                    Location = new Point(65, 20),
                                    ForeColor = Color.Black
                                };
                                UserSession.ActionUpdatedpname += async () =>
                                {
                                    usernameLabel.Text = await user.FindDisplayname(userid);
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
                                    avatarBox.Image = await avatar.LoadAvatarAsync(userid);
                                };

                                // Thêm các điều khiển vào Panel
                                friendPanel.Controls.Add(usernameLabel);
                                friendPanel.Controls.Add(avatarBox);
                                // Thêm Panel vào FlowLayoutPanel
                                flowLayoutPanel1.Controls.Add(friendPanel);
                            }
                            UserSession.ActionDeleteuser += () =>
                            {
                                showfriend(userid1);
                            };
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
        }
        private async void label4_Click(object sender, EventArgs e)
        {
            try { 
            flowLayoutPanel1.Controls.Clear();
            var result = await group.RequestGroupName(gdpid,0);
            var result1 = await group.RequestGroupName(gdpidtk, 0);
            string[] commonGroupNames = result.groupidname.Intersect(result1.groupidname).ToArray();

            for (int i = 0; i < commonGroupNames.Length; i++)
            {
                string[] groupid = commonGroupNames[i].Split('|');
                CircularPicture circulargroup = new CircularPicture();
                try
                {
                    circulargroup.Image = await avatar.LoadAvatarGroupAsync(groupid[0]);
                    circulargroup.SizeMode = PictureBoxSizeMode.Zoom;
                    circulargroup.Name = commonGroupNames[i];

                    UserSession.ActionAvatarGroupUpdated += async () =>
                    {
                        circulargroup.Image = await avatar.LoadAvatarGroupAsync(groupid[0]);
                        circulargroup.Name = commonGroupNames[i];
                    };
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Không thể tải ảnh: {ex.Message}");
                    return;
                }

                // Set the circular avatar properties
                circulargroup.Size = new Size(25, 25);
                circulargroup.Anchor = AnchorStyles.None;

                // Create label for group name
                Label label = new Label();
                label.Text = groupid[1];
                UserSession.ActionUpdateGroupname += async () =>
                {
                    label.Text = await group.Groupname(groupid[0]);
                };
                label.TextAlign = ContentAlignment.MiddleLeft;  // Align label text to the left

                // Create FlowLayoutPanel to hold the avatar and label
                FlowLayoutPanel panel = new FlowLayoutPanel();
                panel.AutoSize = true;
                panel.Padding = new Padding(5, 0, 0, 0);  // Add some padding between the avatar and the label
                panel.FlowDirection = FlowDirection.LeftToRight; // Arrange the label and avatar horizontally
                panel.WrapContents = false;  // Don't wrap contents to new lines

                // Add the avatar and label to the FlowLayoutPanel
                panel.Controls.Add(circulargroup);
                panel.Controls.Add(label);

                // Add the FlowLayoutPanel to the main FlowLayoutPanel
                flowLayoutPanel1.Controls.Add(panel);
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
