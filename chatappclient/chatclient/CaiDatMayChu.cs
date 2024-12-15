
using QLUSER.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Security;
//using System.Windows.Controls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace QLUSER
{
    public partial class CaiDatMayChu : Form
    {
        GiaoDien _gd;
        string _groupdisplayname;
        string _groupid;
        string _userid;
        string _groupname;
        string[] usermemner;
        private List<(string Username, string UserId, string UserRole)> users = new List<(string, string, string)>();
        User _user = new User();
        UserAvatar _avatar = new UserAvatar();
        Group _group = new Group();
        GroupMember _groupMember = new GroupMember();
        string chonvaitro = "";
        public CaiDatMayChu(string groupname, string groupid, string groupdisplayname,GiaoDien gd)
        {
            InitializeComponent();
            _gd = gd;
            label1.Text = groupname;
            label1.Name = groupid;
            textBox1.Text = groupname;
            _groupid = groupid;
            _groupdisplayname = groupdisplayname;
            _groupname = groupname;
            UserSession.ActionUpdategdpname += Updategdpname;
            UserSession.ActionUpdateGroupname += UpdateGroupname;
        }

        

        private async void Updategdpname()
        {
            _groupdisplayname = await _groupMember.FindGroupDisplayname(_userid,_groupid);
        }
        private async void UpdateGroupname()
        {
            string groupname = await _group.Groupname(_groupid);
            label1.Text = groupname;
            textBox1.Text = groupname;
            _groupname = groupname;
        }
        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }
        private async void button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
            string[] roles = await _groupMember.FindUserRoleNameId(_groupid);
            if(roles!=null)
                foreach (var role in roles)
            {
                string[] userole = role.Split('|');
                    if (!users.Any(user => user.Item2 == userole[0]))
                    {
                        users.Add((userole[1], userole[0], userole[2]));
                    }
            }
            var members = users.Where(user => user.UserRole.Equals("Member", StringComparison.OrdinalIgnoreCase)).ToList();
            label11.Text = members.Count.ToString();
            var mods = users.Where(user => user.UserRole.Equals("Mod", StringComparison.OrdinalIgnoreCase)).ToList();
            label18.Text = mods.Count.ToString();
            var admins = users.Where(user => user.UserRole.Equals("Admin", StringComparison.OrdinalIgnoreCase)).ToList();
            label20.Text = admins.Count.ToString();
            var owners = users.Where(user => user.UserRole.Equals("Owner", StringComparison.OrdinalIgnoreCase)).ToList();
            label22.Text = owners.Count.ToString();
            UserSession.ActionUpdateGroupMember += () =>
            {
                button2_Click(null, EventArgs.Empty);
            };
            UserSession.ActionUpdateRole += () =>
            {
                button2_Click(null, EventArgs.Empty);
            };
            UserSession.ActionDeleteuser += () =>
            {
                button2_Click(null, EventArgs.Empty);
            };
        }
        private async void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            string[] roles = await _groupMember.FindUserRoleNameId(_groupid);
            if (roles != null)
                foreach (var role in roles)
                {
                    string[] userole = role.Split('|');
                    if (!users.Any(user => user.Item2 == userole[0]))
                    {
                        users.Add((userole[1], userole[0], userole[2]));
                    }
                }
            foreach (var user in users)
            {
                string createtime = await _user.FindCreatetime(user.UserId);
                string jointime = await _groupMember.FindJointime(user.UserId, _groupid);
                ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {user.Username,jointime,createtime,user.UserRole}, -1);
                listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {listViewItem1});
            }
            tabControl1.SelectedIndex = 2;
            UserSession.ActionUpdateGroupMember += () =>
            {
                button3_Click(null, EventArgs.Empty);
            };
            UserSession.ActionUpdateRole += () =>
            {
                button3_Click(null, EventArgs.Empty);
            };
            UserSession.ActionDeleteuser += () =>
            {
                button3_Click(null, EventArgs.Empty);
            };
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại xác nhận
            var result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa nhóm này không?",
                "Xác nhận xóa nhóm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            // Kiểm tra kết quả người dùng chọn
            if (result == DialogResult.Yes)
            {
                // Thực hiện xóa nhóm
                await _group.DeleteGroup(_groupid);
                UserSession.UpdateGroup = (true, true);
                _gd.SendUpdate("GroupDislay");
                MessageBox.Show("Nhóm đã được xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                // Hủy thao tác
                MessageBox.Show("Thao tác xóa nhóm đã bị hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void CaiDatMayChu_Load(object sender, EventArgs e)
        {
            _userid = await _groupMember.FindGroupDisplayID(_groupid,_groupdisplayname);
            circularPicture1.Image = await _avatar.LoadAvatarGroupAsync(_groupid);
            circularPicture1.Text = _groupname;
            circularPicture1.Name = _groupid;
            UserSession.ActionAvatarGroupUpdated += async () =>
            {
                circularPicture1.Image = await _avatar.LoadAvatarGroupAsync(_groupid);
            };
            UserSession.ActionUpdateGroupname += async () =>
            {
                circularPicture1.Text = await _group.Groupname(_groupid);
            };
        }
        private void button6_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(async () =>
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string avatarUrl = await _avatar.UploadAvatarGroupAsync(openFileDialog.FileName, _groupid);

                        if (avatarUrl != null)
                        {
                            UserSession.AvatarGroupUrl = true;
                            _gd.SendUpdate("AvatarGroupUpdated");
                            MessageBox.Show("Avatar group uploaded successfully!");
                            circularPicture1.Invoke((MethodInvoker)(() =>
                            {
                                if (circularPicture1.Image != null)
                                {
                                    circularPicture1.Image.Dispose();
                                }
                                circularPicture1.Image = Image.FromFile(openFileDialog.FileName);
                            }));
                        }
                        else
                        {
                            MessageBox.Show("Failed to upload avatar.");
                        }
                    }
                }
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private async void button10_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != _groupname)
            {
                string newgroupname = textBox1.Text;
                var result = await _group.RenameGroup(newgroupname, label1.Name);
                if (result)
                {
                    UserSession.RenameGroupname = true;
                    _gd.SendUpdate("UpdateGroupname");
                    _groupname = newgroupname;
                    label1.Text = newgroupname;
                    circularPicture1.Text = newgroupname;
                }
            }
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private async void button12_Click(object sender, EventArgs e)
        {
            if (chonvaitro == "") return;
            Form form = new Form();
            form.Text = $"Thêm người dùng";
            form.Size = new Size(600, 400);
            form.StartPosition = FormStartPosition.CenterParent;

            Label label = new Label();
            label.Text = "Thêm thành viên";
            label.Font = new Font("Arial", 12, FontStyle.Bold);
            label.ForeColor = Color.Black;
            label.AutoSize = true;
            label.PerformLayout();
            label.Location = new Point((600 - label.Width) / 2, 20);
            label.TextAlign = ContentAlignment.MiddleCenter;

            Label label1 = new Label();
            label1.Text = $"{chonvaitro}";
            label1.Font = new Font("Arial", 12, FontStyle.Bold);
            label1.ForeColor = Color.Black;
            label1.AutoSize = true;
            label1.PerformLayout();
            label1.Location = new Point((600 - label1.Width) / 2, 50);
            label1.TextAlign = ContentAlignment.MiddleCenter;
            Label label2 = new Label();
            label2.Text = "Thành viên";
            label2.Font = new Font("Arial", 12, FontStyle.Bold);
            label2.ForeColor = Color.Black;
            label2.AutoSize = true;
            label2.PerformLayout();
            label2.Location = new Point(20, 80);
            label2.TextAlign = ContentAlignment.MiddleCenter;
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
            flowLayoutPanel.AutoScroll = true;
            flowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            flowLayoutPanel.Location = new System.Drawing.Point(20, 100);
            flowLayoutPanel.Padding = new System.Windows.Forms.Padding(10, 0, 10, 10);
            flowLayoutPanel.Size = new System.Drawing.Size(560, 150);
            flowLayoutPanel.WrapContents = false;
            string[] groupdisplaynameid = await _groupMember.FindUserRoleNameId(_groupid);
            if (groupdisplaynameid != null)
                foreach (var user in groupdisplaynameid)
            {
                string[] user1 = user.Split('|');
                CheckBox checkBox = new CheckBox();
                checkBox.Checked = false;
                checkBox.Text = $"{user1[1]}";
                    UserSession.ActionUpdategdpname += async () =>
                            {
                                checkBox.Text = await _groupMember.FindGroupDisplayname(user1[0], _groupid);
                            };
                checkBox.Name = $"{user1[0]}";
                checkBox.Font = new Font("Arial", 12, FontStyle.Bold);
                checkBox.ForeColor = Color.Black;
                checkBox.AutoSize = true;
                checkBox.PerformLayout();
                checkBox.TextAlign = ContentAlignment.MiddleCenter;
                flowLayoutPanel.Controls.Add(checkBox);
            }
            Button btncreate = new Button();
            btncreate.Text = "thêm";
            btncreate.AutoSize = AutoSize;
            btncreate.Location = new Point(560 - btncreate.Width, 320);
            btncreate.Click += async (s1, e1) =>
            {

                foreach (Control control in flowLayoutPanel.Controls)
                {
                    if (control is CheckBox checkBox && checkBox.Checked)
                    {
                        _groupMember.themrole(_groupid, checkBox.Name, chonvaitro);
                        UserSession.UpdateUserRole = (true, true);
                        _gd.SendUpdate("UpdateRole");
                        var index = users.FindIndex(user => user.UserId == checkBox.Name);

                        if (index != -1)
                        {
                            var user = users[index];
                            users[index] = (user.Username, user.UserId, chonvaitro);

                        }
                        var members = users.Where(user => user.UserRole.Equals(chonvaitro, StringComparison.OrdinalIgnoreCase)).ToList();

                        flowLayoutPanel1.Controls.Clear();
                        if (members == null)
                        {
                            Panel panel = new Panel
                            {
                                Width = flowLayoutPanel1.Width,
                                AutoSize = true,
                            };
                            Label lblUsername = new Label
                            {
                                Text = "Không tìm thấy thành viên nào",
                                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                                ForeColor = Color.FromArgb(88, 101, 242),
                                AutoSize = true,
                            };


                            panel.Controls.Add(lblUsername);
                            flowLayoutPanel1.Controls.Add(panel);
                        }
                        else
                        {
                            foreach (var user in members)
                            {
                                Panel panel = new Panel
                                {
                                    Width = flowLayoutPanel1.Width - 40,
                                    AutoSize = true,
                                };

                                CircularPicture pictureBoxAvatar = new CircularPicture
                                {
                                    SizeMode = PictureBoxSizeMode.Zoom,
                                    Width = 25,
                                    Height = 25,
                                    Image = await _avatar.LoadAvatarAsync(user.UserId),
                                };
                                UserSession.ActionAvatarUpdated += async () =>
                                {
                                    pictureBoxAvatar.Image = await _avatar.LoadAvatarAsync(user.UserId);
                                };
                                panel.Controls.Add(pictureBoxAvatar);

                                Label lblUsername = new Label
                                {
                                    Text = user.Username,
                                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                                    ForeColor = Color.FromArgb(88, 101, 242),
                                    AutoSize = true,
                                    Left = pictureBoxAvatar.Right + 10
                                };
                                UserSession.ActionUpdategdpname += async () =>
                                {
                                    lblUsername.Text = await _groupMember.FindGroupDisplayname(user.UserId, _groupid);
                                };
                                panel.Controls.Add(lblUsername);
                                Button xoa = new Button
                                {
                                    Text = "X",
                                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                                    ForeColor = Color.FromArgb(88, 101, 242),
                                    Size = pictureBoxAvatar.Size,
                                    Location = new Point(panel.Width - 20, pictureBoxAvatar.Location.Y)

                                };
                                xoa.Click += (s2, e2) =>
                                {
                                    Xoathanhvien(user.Username, user.UserId, user.UserRole);
                                };
                                panel.Controls.Add(xoa);
                                flowLayoutPanel1.Controls.Add(panel);
                            }
                        }
                    }
                }

            };
            Button btnClose = new Button();
            btnClose.Text = "Hủy bỏ";
            btnClose.Location = new Point(560 - btncreate.Width - btnClose.Width - 20, 320);
            btnClose.AutoSize = AutoSize;
            btnClose.Click += (s1, e1) =>
            {
                form.Close();
            };
            UserSession.ActionDeleteuser += () =>
            {
                form.Close();
            };
            form.Controls.Add(label);
            form.Controls.Add(label1);
            form.Controls.Add(label2);
            form.Controls.Add(flowLayoutPanel);
            form.Controls.Add(btncreate);
            form.Controls.Add(btnClose);
            form.Show();
        }

        private async void label30_Click(object sender, EventArgs e)
        {
            chonvaitro = "Member";
            var members = users.Where(user => user.UserRole.Equals(chonvaitro, StringComparison.OrdinalIgnoreCase)).ToList();

            flowLayoutPanel1.Controls.Clear();
            if (members.Count == 0)
            {
                Panel panel = new Panel
                {
                    Width = flowLayoutPanel1.Width,
                    AutoSize = true,
                };
                Label lblUsername = new Label
                {
                    Text = "Không tìm thấy thành viên nào",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    ForeColor = Color.FromArgb(88, 101, 242),
                    AutoSize = true,
                };


                panel.Controls.Add(lblUsername);
                flowLayoutPanel1.Controls.Add(panel);
            }
            else
            {
                foreach (var user in members)
                {
                    Panel panel = new Panel
                    {
                        Width = flowLayoutPanel1.Width - 40,
                        AutoSize = true,
                    };

                    CircularPicture pictureBoxAvatar = new CircularPicture
                    {
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Width = 25,
                        Height = 25,
                        Image = await _avatar.LoadAvatarAsync(user.UserId),
                    };
                    UserSession.ActionAvatarUpdated += async () =>
                    {
                        pictureBoxAvatar.Image = await _avatar.LoadAvatarAsync(user.UserId);
                    };
                    panel.Controls.Add(pictureBoxAvatar);

                    Label lblUsername = new Label
                    {
                        Text = user.Username,
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        ForeColor = Color.FromArgb(88, 101, 242),
                        AutoSize = true,
                        Left = pictureBoxAvatar.Right + 10
                    };
                    UserSession.ActionUpdategdpname += async () =>
                    {
                        lblUsername.Text = await _groupMember.FindGroupDisplayname(user.UserId, _groupid);
                    };
                    panel.Controls.Add(lblUsername);
                    Button xoa = new Button
                    {
                        Text = "X",
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        ForeColor = Color.FromArgb(88, 101, 242),
                        Size = pictureBoxAvatar.Size,
                        Location = new Point(panel.Width - 20, pictureBoxAvatar.Location.Y)

                    };
                    xoa.Click += (s1, e1) =>
                    {
                        Xoathanhvien(user.Username, user.UserId, user.UserRole);
                    };
                    panel.Controls.Add(xoa);
                    flowLayoutPanel1.Controls.Add(panel);
                }
            }
            UserSession.ActionUpdateGroupMember += () =>
            {
                label30_Click(null, EventArgs.Empty);
            };
            UserSession.ActionUpdateRole += () =>
            {
                label30_Click(null, EventArgs.Empty);
            };
            UserSession.ActionDeleteuser += () =>
            {
                label30_Click(null, EventArgs.Empty);
            };
        }

        private async void label27_Click(object sender, EventArgs e)
        {
            chonvaitro = "Mod";
            var members = users.Where(user => user.UserRole.Equals(chonvaitro, StringComparison.OrdinalIgnoreCase)).ToList();

            flowLayoutPanel1.Controls.Clear();
            if (members.Count == 0)
            {
                Panel panel = new Panel
                {
                    Width = flowLayoutPanel1.Width,
                    AutoSize = true,
                };
                Label lblUsername = new Label
                {
                    Text = "Không tìm thấy thành viên nào",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    ForeColor = Color.FromArgb(88, 101, 242),
                    AutoSize = true,
                };


                panel.Controls.Add(lblUsername);
                flowLayoutPanel1.Controls.Add(panel);
            }
            else
            {
                foreach (var user in members)
                {
                    Panel panel = new Panel
                    {
                        Width = flowLayoutPanel1.Width - 40,
                        AutoSize = true,
                    };

                    CircularPicture pictureBoxAvatar = new CircularPicture
                    {
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Width = 25,
                        Height = 25,
                        Image = await _avatar.LoadAvatarAsync(user.UserId),
                    };
                    UserSession.ActionAvatarUpdated += async () =>
                    {
                        pictureBoxAvatar.Image = await _avatar.LoadAvatarAsync(user.UserId);
                    };
                    panel.Controls.Add(pictureBoxAvatar);

                    Label lblUsername = new Label
                    {
                        Text = user.Username,
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        ForeColor = Color.FromArgb(88, 101, 242),
                        AutoSize = true,
                        Left = pictureBoxAvatar.Right + 10
                    };
                    UserSession.ActionUpdategdpname += async () =>
                    {
                        lblUsername.Text = await _groupMember.FindGroupDisplayname(user.UserId, _groupid);
                    };
                    panel.Controls.Add(lblUsername);
                    Button xoa = new Button
                    {
                        Text = "X",
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        ForeColor = Color.FromArgb(88, 101, 242),
                        Size = pictureBoxAvatar.Size,
                        Location = new Point(panel.Width - 20, pictureBoxAvatar.Location.Y)

                    };
                    xoa.Click += (s1, e1) =>
                    {
                        Xoathanhvien(user.Username, user.UserId, user.UserRole);
                    };
                    panel.Controls.Add(xoa);
                    flowLayoutPanel1.Controls.Add(panel);
                }
            }
            UserSession.ActionUpdateGroupMember += () =>
            {
                label27_Click(null, EventArgs.Empty);
            };
            UserSession.ActionUpdateRole += () =>
            {
                label27_Click(null, EventArgs.Empty);
            };
            UserSession.ActionDeleteuser += () =>
            {
                label27_Click(null, EventArgs.Empty);
            };
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private async void label25_Click(object sender, EventArgs e)
        {
            chonvaitro = "Owner";
            var members = users.Where(user => user.UserRole.Equals(chonvaitro, StringComparison.OrdinalIgnoreCase)).ToList();

            flowLayoutPanel1.Controls.Clear();
            if (members.Count == 0)
            {
                Panel panel = new Panel
                {
                    Width = flowLayoutPanel1.Width,
                    AutoSize = true,
                };
                Label lblUsername = new Label
                {
                    Text = "Không tìm thấy thành viên nào",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    ForeColor = Color.FromArgb(88, 101, 242),
                    AutoSize = true,
                };


                panel.Controls.Add(lblUsername);
                flowLayoutPanel1.Controls.Add(panel);
            }
            else
            {
                foreach (var user in members)
                {
                    Panel panel = new Panel
                    {
                        Width = flowLayoutPanel1.Width - 40,
                        AutoSize = true,
                    };

                    CircularPicture pictureBoxAvatar = new CircularPicture
                    {
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Width = 25,
                        Height = 25,
                        Image = await _avatar.LoadAvatarAsync(user.UserId),
                    };
                    UserSession.ActionAvatarUpdated += async () =>
                    {
                        pictureBoxAvatar.Image = await _avatar.LoadAvatarAsync(user.UserId);
                    };
                    panel.Controls.Add(pictureBoxAvatar);

                    Label lblUsername = new Label
                    {
                        Text = user.Username,
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        ForeColor = Color.FromArgb(88, 101, 242),
                        AutoSize = true,
                        Left = pictureBoxAvatar.Right + 10
                    };
                    UserSession.ActionUpdategdpname += async () =>
                    {
                        lblUsername.Text = await _groupMember.FindGroupDisplayname(user.UserId, _groupid);
                    };
                    panel.Controls.Add(lblUsername);
                    Button xoa = new Button
                    {
                        Text = "X",
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        ForeColor = Color.FromArgb(88, 101, 242),
                        Size = pictureBoxAvatar.Size,
                        Location = new Point(panel.Width - 20, pictureBoxAvatar.Location.Y)

                    };
                    xoa.Click += (s1, e1) =>
                    {
                        Xoathanhvien(user.Username, user.UserId, user.UserRole);
                    };
                    panel.Controls.Add(xoa);
                    flowLayoutPanel1.Controls.Add(panel);
                }
            }
            UserSession.ActionUpdateGroupMember += () =>
            {
                label25_Click(null, EventArgs.Empty);
            };
            UserSession.ActionUpdateRole += () =>
            {
                label25_Click(null, EventArgs.Empty);
            };
            UserSession.ActionDeleteuser += () =>
            {
                label25_Click(null, EventArgs.Empty);
            };
        }

        private async void label24_Click(object sender, EventArgs e)
        {
            chonvaitro = "Admin";
            var members = users.Where(user => user.UserRole.Equals(chonvaitro, StringComparison.OrdinalIgnoreCase)).ToList();

            flowLayoutPanel1.Controls.Clear();
            if (members.Count == 0)
            {
                Panel panel = new Panel
                {
                    Width = flowLayoutPanel1.Width,
                    AutoSize = true,
                };
                Label lblUsername = new Label
                {
                    Text = "Không tìm thấy thành viên nào",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    ForeColor = Color.FromArgb(88, 101, 242),
                    AutoSize = true,
                };


                panel.Controls.Add(lblUsername);
                flowLayoutPanel1.Controls.Add(panel);
            }
            else
            {
                foreach (var user in members)
                {
                    Panel panel = new Panel
                    {
                        Width = flowLayoutPanel1.Width - 40,
                        AutoSize = true,
                    };

                    CircularPicture pictureBoxAvatar = new CircularPicture
                    {
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Width = 25,
                        Height = 25,
                        Image = await _avatar.LoadAvatarAsync(user.UserId),
                    };
                    UserSession.ActionAvatarUpdated += async () =>
                    {
                        pictureBoxAvatar.Image = await _avatar.LoadAvatarAsync(user.UserId);
                    };
                    panel.Controls.Add(pictureBoxAvatar);

                    Label lblUsername = new Label
                    {
                        Text = user.Username,
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        ForeColor = Color.FromArgb(88, 101, 242),
                        AutoSize = true,
                        Left = pictureBoxAvatar.Right + 10
                    };
                    UserSession.ActionUpdategdpname += async () =>
                    {
                        lblUsername.Text = await _groupMember.FindGroupDisplayname(user.UserId, _groupid);
                    };
                    panel.Controls.Add(lblUsername);
                    Button xoa = new Button
                    {
                        Text = "X",
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        ForeColor = Color.FromArgb(88, 101, 242),
                        Size = pictureBoxAvatar.Size,
                        Location = new Point(panel.Width - 20, pictureBoxAvatar.Location.Y)

                    };
                    xoa.Click += (s1, e1) =>
                    {
                        Xoathanhvien(user.Username, user.UserId, user.UserRole);
                    };
                    panel.Controls.Add(xoa);
                    flowLayoutPanel1.Controls.Add(panel);
                }
            }
            UserSession.ActionUpdateGroupMember += () =>
            {
                label24_Click(null, EventArgs.Empty);
            };
            UserSession.ActionUpdateRole += () =>
            {
                label24_Click(null, EventArgs.Empty);
            };
            UserSession.ActionDeleteuser += () =>
            {
                label24_Click(null, EventArgs.Empty);
            };
        }

        private async void label11_Click(object sender, EventArgs e)
        {

        }

        private async void label18_Click(object sender, EventArgs e)
        {

        }

        private async void label20_Click(object sender, EventArgs e)
        {

        }

        private async void label22_Click(object sender, EventArgs e)
        {
        }
        private async void Xoathanhvien(string gdpname, string userid, string role)
        {
            Form form = new Form();
            form.Text = $"Xóa thành viên";
            form.Size = new Size(400, 200);
            form.StartPosition = FormStartPosition.CenterParent;

            Label label = new Label();
            label.Text = "Xóa thành viên";
            label.Font = new Font("Arial", 12, FontStyle.Bold);
            label.ForeColor = Color.Black;
            label.AutoSize = true;
            label.PerformLayout();
            label.Location = new Point(20, 20);
            label.TextAlign = ContentAlignment.MiddleCenter;

            Label label1 = new Label();
            label1.Text = $"Xóa {gdpname} khỏi vai trò {role}";
            label1.Font = new Font("Arial", 12, FontStyle.Bold);
            label1.ForeColor = Color.Black;
            label1.AutoSize = true;
            label1.PerformLayout();
            label1.Location = new Point(20, 50);
            label1.TextAlign = ContentAlignment.MiddleCenter;
            UserSession.ActionUpdategdpname += async () =>
            {
                string gdpname1 = await _groupMember.FindGroupDisplayname(userid,_groupid);
                label1.Text = $"Xóa {gdpname1} khỏi vai trò {role}";
            };
            Button btncreate = new Button();
            btncreate.Text = "Gỡ bỏ";
            btncreate.AutoSize = AutoSize;
            btncreate.Location = new Point(340 - btncreate.Width, 120);
            btncreate.Click += async (s, e) =>
            {
                _groupMember.Goborole(_groupid, userid);
                UserSession.UpdateUserRole = (true, true);
                _gd.SendUpdate("UpdateRole");
                var index = users.FindIndex(user => user.UserId == userid);

                if (index != -1)
                {
                    var user = users[index];
                    users[index] = (user.Username, user.UserId, "Member");

                }
                var members = users.Where(user => user.UserRole.Equals(chonvaitro, StringComparison.OrdinalIgnoreCase)).ToList();

                flowLayoutPanel1.Controls.Clear();
                if (members == null)
                {
                    Panel panel = new Panel
                    {
                        Width = flowLayoutPanel1.Width,
                        AutoSize = true,
                    };
                    Label lblUsername = new Label
                    {
                        Text = "Không tìm thấy thành viên nào",
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        ForeColor = Color.FromArgb(88, 101, 242),
                        AutoSize = true,
                    };


                    panel.Controls.Add(lblUsername);
                    flowLayoutPanel1.Controls.Add(panel);
                }
                else
                {
                    foreach (var user in members)
                    {
                        Panel panel = new Panel
                        {
                            Width = flowLayoutPanel1.Width - 40,
                            AutoSize = true,
                        };

                        CircularPicture pictureBoxAvatar = new CircularPicture
                        {
                            SizeMode = PictureBoxSizeMode.Zoom,
                            Width = 25,
                            Height = 25,
                            Image = await _avatar.LoadAvatarAsync(user.UserId),
                        };
                        UserSession.ActionAvatarUpdated += async () =>
                        {
                            pictureBoxAvatar.Image = await _avatar.LoadAvatarAsync(user.UserId);
                        };
                        panel.Controls.Add(pictureBoxAvatar);

                        Label lblUsername = new Label
                        {
                            Text = user.Username,
                            Font = new Font("Segoe UI", 10, FontStyle.Bold),
                            ForeColor = Color.FromArgb(88, 101, 242),
                            AutoSize = true,
                            Left = pictureBoxAvatar.Right + 10
                        };
                        UserSession.ActionUpdategdpname += async () =>
                        {
                            lblUsername.Text = await _groupMember.FindGroupDisplayname(user.UserId, _groupid);
                        };
                        panel.Controls.Add(lblUsername);
                        Button xoa = new Button
                        {
                            Text = "X",
                            Font = new Font("Segoe UI", 10, FontStyle.Bold),
                            ForeColor = Color.FromArgb(88, 101, 242),
                            Size = pictureBoxAvatar.Size,
                            Location = new Point(panel.Width - 20, pictureBoxAvatar.Location.Y)

                        };
                        xoa.Click += (s1, e1) =>
                        {
                            Xoathanhvien(user.Username, user.UserId, user.UserRole);
                        };
                        panel.Controls.Add(xoa);
                        flowLayoutPanel1.Controls.Add(panel);
                    }
                }
            
            };
            Button btnClose = new Button();
            btnClose.Text = "Hủy bỏ";
            btnClose.Location = new Point(340 - btncreate.Width - btnClose.Width - 20, 120);
            btnClose.AutoSize = AutoSize;
            btnClose.Click += (s, e) =>
            {
                form.Close();
            };
            UserSession.ActionDeleteuser += () =>
            {
                form.Close();
            };
            form.Controls.Add(label);
            form.Controls.Add(label1);
            form.Controls.Add(btncreate);
            form.Controls.Add(btnClose);
            form.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            if (chonvaitro == "" || string.IsNullOrEmpty(txtUser.Text)) return;
            var members = users
                .Where(user => user.UserRole.Equals(chonvaitro, StringComparison.OrdinalIgnoreCase)
                               && user.Username.Contains(txtUser.Text))
                .ToList();
            flowLayoutPanel1.Controls.Clear();
            if (members.Count == 0)
            {
                Panel panel = new Panel
                {
                    Width = flowLayoutPanel1.Width,
                    AutoSize = true,
                };
                Label lblUsername = new Label
                {
                    Text = "Không tìm thấy thành viên nào",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    ForeColor = Color.FromArgb(88, 101, 242),
                    AutoSize = true,
                };


                panel.Controls.Add(lblUsername);
                flowLayoutPanel1.Controls.Add(panel);
            }
            else
            {
                foreach (var user in members)
                {
                    Panel panel = new Panel
                    {
                        Width = flowLayoutPanel1.Width - 40,
                        AutoSize = true,
                    };

                    CircularPicture pictureBoxAvatar = new CircularPicture
                    {
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Width = 25,
                        Height = 25,
                        Image = await _avatar.LoadAvatarAsync(user.UserId),
                    };
                    UserSession.ActionAvatarUpdated += async () =>
                    {
                        pictureBoxAvatar.Image = await _avatar.LoadAvatarAsync(user.UserId);
                    };
                    panel.Controls.Add(pictureBoxAvatar);

                    Label lblUsername = new Label
                    {
                        Text = user.Username,
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        ForeColor = Color.FromArgb(88, 101, 242),
                        AutoSize = true,
                        Left = pictureBoxAvatar.Right + 10
                    };
                    UserSession.ActionUpdategdpname += async () =>
                    {
                        lblUsername.Text = await _groupMember.FindGroupDisplayname(user.UserId, _groupid);
                    };
                    panel.Controls.Add(lblUsername);
                    Button xoa = new Button
                    {
                        Text = "X",
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        ForeColor = Color.FromArgb(88, 101, 242),
                        Size = pictureBoxAvatar.Size,
                        Location = new Point(panel.Width - 20, pictureBoxAvatar.Location.Y)

                    };
                    xoa.Click += (s1, e1) =>
                    {
                        Xoathanhvien(user.Username, user.UserId, user.UserRole);
                    };
                    panel.Controls.Add(xoa);
                    flowLayoutPanel1.Controls.Add(panel);
                }
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
    }

