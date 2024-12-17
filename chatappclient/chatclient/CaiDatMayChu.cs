
using Microsoft.VisualBasic;
using NAudio.CoreAudioApi;
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
        Dangnhap _dn;
        GiaoDien _gd;
        string _gdpname;
        string _groupid;
        string _userid;
        string _userrole;
        int _number;
        string _groupname;
        private List<(string gdpname, string UserId, string UserRole)> users = new List<(string, string, string)>();
        User _user = new User();
        UserAvatar _avatar = new UserAvatar();
        Group _group = new Group();
        GroupMember _groupMember = new GroupMember();
        string chonvaitro = "";
        public CaiDatMayChu(string groupname, string groupid, string userid,GiaoDien gd,Dangnhap dn)
        {
            InitializeComponent();
            _gd = gd;
            _dn = dn;
            label1.Text = groupname;
            label1.Name = groupid;
            textBox1.Text = groupname;
            _groupid = groupid;
            _groupname = groupname;
            _userid = userid;
            UserSession.ActionUpdategdpname += Updategdpname;
            UserSession.ActionUpdateGroupname += UpdateGroupname;
        }



        private async void Updategdpname()
        {
            try
            {
                _gdpname = await _groupMember.FindGroupDisplayname(_userid, _groupid);
            }
            catch (Exception ex)
            {
                // Log hoặc xử lý lỗi
                MessageBox.Show($"Error in Updategdpname: {ex.Message}");
            }
        }

        private async void UpdateGroupname()
        {
            try
            {
                string groupname = await _group.Groupname(_groupid);
                label1.Text = groupname;
                textBox1.Text = groupname;
                _groupname = groupname;
            }
            catch (Exception ex)
            {
                // Log hoặc xử lý lỗi
                MessageBox.Show($"Error in UpdateGroupname: {ex.Message}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                tabControl1.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                // Log hoặc xử lý lỗi
                MessageBox.Show($"Error in button1_Click: {ex.Message}");
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                tabControl1.SelectedIndex = 1;
                string[] roles = await _groupMember.FindUserRoleNameId(_groupid);
                if (roles != null)
                {
                    users.Clear();
                    foreach (var role in roles)
                    {
                        string[] userole = role.Split('|');
                        if (!users.Any(user => user.Item2 == userole[0]))
                        {
                            users.Add((userole[1], userole[0], userole[2]));
                        }
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
                UserSession.ActionUpdateGroupMember -= hambutton2click;
                UserSession.ActionUpdateRole -= hambutton2click;
                UserSession.ActionDeleteuser -= hambutton2click;
                UserSession.ActionUpdateGroupMember += hambutton2click;
                UserSession.ActionUpdateRole += hambutton2click;
                UserSession.ActionDeleteuser += hambutton2click;
            }
            catch (Exception ex)
            {
                // Log hoặc xử lý lỗi
                MessageBox.Show($"Error in button2_Click: {ex.Message}");
            }
        }
        private void hambutton2click()
        {
            button2_Click(null, EventArgs.Empty);
        }
        /*private async void button3_Click(object sender, EventArgs e)
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
                ListViewItem item = listView1.Items.Add(listViewItem1);
                //createbacham(item);
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

        }*/

        private async void button3_Click(object sender, EventArgs e)
        {
            try
            {
                listView1.FullRowSelect = true;
                users.Clear();
                listView1.Items.Clear();
                string[] roles = await _groupMember.FindUserRoleNameId(_groupid);
                if (roles != null)
                {
                    foreach (var role in roles)
                    {
                        string[] userole = role.Split('|');
                        if (!users.Any(user => user.Item2 == userole[0]))
                        {
                            users.Add((userole[1], userole[0], userole[2]));
                        }
                    }
                }

                foreach (var user in users)
                {
                    string createtime = await _user.FindCreatetime(user.UserId);
                    string jointime = await _groupMember.FindJointime(user.UserId, _groupid);
                    ListViewItem listViewItem = new ListViewItem(new string[]
                    {
                user.gdpname,
                jointime,
                createtime,
                user.UserRole,
                "⋮",
                    });
                    listView1.Items.Add(listViewItem);
                }
                listView1.MouseClick += ListView1_MouseClick;
                tabControl1.SelectedIndex = 2;
                UserSession.ActionUpdateGroupMember -= hambutton3click;
                UserSession.ActionUpdateRole -= hambutton3click;
                UserSession.ActionDeleteuser -= hambutton3click;
                UserSession.ActionUpdateGroupMember += hambutton3click;
                UserSession.ActionUpdateRole += hambutton3click;
                UserSession.ActionDeleteuser += hambutton3click;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void hambutton3click()
        {
            button3_Click(null, EventArgs.Empty);
        }
        
        private void ListView1_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.SubItems.Count > 4) 
                {
                    Rectangle iconBounds = new Rectangle(item.SubItems[4].Bounds.X + item.SubItems[4].Bounds.Width - 20, item.SubItems[4].Bounds.Y, 20, item.SubItems[4].Bounds.Height);

                    if (iconBounds.Contains(e.Location))
                    {
                        string gdpname = item.SubItems[0].Text;
                        formusergroup(gdpname,true);
                        break;
                    }
                }
            }
        }
        public async void formusergroup(string groupdisplayname,bool isformcaidatmaychu)
        {
            try { 
            string userid = await _groupMember.FindGroupDisplayID(_groupid, groupdisplayname);
            UserSession.ActionUpdategdpname += async () =>
            {
                groupdisplayname = await _groupMember.FindGroupDisplayname(userid, _groupid);
            };
            string username = await _user.FindDisplayname(userid);
            UserSession.ActionUpdateusername += async () =>
            {
                username = await _user.FindUsername(userid);
            };
                string userrole = await _groupMember.FindOneUserRole(_groupid, userid);
                int number = 1;
                switch (userrole)
                {
                    case "Member": number = 1; break;
                    case "Mod": number = 2; break;
                    case "Admin": number = 3; break;
                    case "Owner": number = 4; break;
                }
                UserSession.ActionUpdateRole += async () =>
                {
                    userrole = await _groupMember.FindOneUserRole(_groupid, userid);
                    switch (userrole)
                    {
                        case "Member": number = 1; break;
                        case "Mod": number = 2; break;
                        case "Admin": number = 3; break;
                        case "Owner": number = 4; break;
                    }
                };
                _userrole = await _groupMember.FindOneUserRole(_groupid, _userid);
            _gdpname = await _groupMember.FindGroupDisplayname(_userid, _groupid);
            bool on = false;
            Form form = new Form();
            form.Text = $"Thông tin";
            form.StartPosition = FormStartPosition.CenterParent;
            form.AutoSize = true;
            form.Deactivate += (s, e) =>
            {
                if(!on)
                form.Close();
            };
            switch (_userrole)
            { 
                case "Member": _number = 1; break;
                case "Mod": _number = 2; break;
                case "Admin": _number = 3; break;
                case "Owner": _number = 4; break;
            }
            UserSession.ActionDeleteuser += () =>
            {
                if (form != null && !form.IsDisposed)
                {
                    
                    form.Close();
                }
            };
            FlowLayoutPanel panel1 = new FlowLayoutPanel
            {
                Location = new Point(20, 20),
                AutoSize = true,
                FlowDirection = FlowDirection.TopDown, // Sắp xếp các nút theo chiều dọc
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(5),
            };

            // Danh sách các nút
            List<Button> buttons = new List<Button> {}; 

            if(groupdisplayname == _gdpname)
            {
                buttons.Add(new Button { Text = "Hồ Sơ" });
                buttons.Add(new Button { Text = "Chỉnh sửa hồ sơ máy chủ" });
                buttons.Add(new Button { Text = "Vai trò" });
            }
            else
            {
                buttons.Add(new Button { Text = "Hồ Sơ" });
                if (_userrole == "Admin" || _userrole == "Owner") buttons.Add(new Button { Text = "Đổi biệt danh" });

                buttons.Add(new Button { Text = "Mời vào máy chủ" });

                buttons.Add(new Button { Text = "Thêm bạn" });
                if (_userrole == "Admin" || _userrole == "Owner") buttons.Add(new Button { Text = "Đuổi user" });
                buttons.Add(new Button { Text = "Vai trò" });
                if (isformcaidatmaychu &&_userrole == "Owner") buttons.Add(new Button { Text = "Chuyển quyền sở hữu" });

            }
            foreach (var btn in buttons)
            {

                btn.Width = 200; // Thiết lập chiều rộng đồng nhất
                btn.Height = 40; // Thiết lập chiều cao đồng nhất
                btn.ForeColor = Color.Black;
                btn.TextAlign = ContentAlignment.MiddleCenter;
                btn.Margin = new Padding(5); // Khoảng cách giữa các nút

                btn.Click += async (s, e) =>
                {
                    on = true;

                    switch (btn.Text)
                    {
                        case "Hồ Sơ":
                            GroupUser groupUser = new GroupUser(userid,_userid, _groupid, _gd);
                            groupUser.ShowDialog();
                            break;
                        case "Đổi biệt danh":
                            if(_number<number)
                            {
                                MessageBox.Show("Không có đủ quyền hạn để đổi biệt danh");
                                break;
                            }
                            string newNickname = Interaction.InputBox(
                "Nhập biệt danh mới:",
                "Đổi Biệt Danh",
                "");
                            if (newNickname != groupdisplayname && !string.IsNullOrWhiteSpace(newNickname))
                            {
                                var result = await _groupMember.RenameGroupDisplayname(newNickname, _groupid, userid);
                                if (result)
                                {
                                    UserSession.Renamegdpname = (true, true);
                                    _gd.SendUpdate("Updategdpname");
                                    MessageBox.Show("Biệt danh máy chủ đã được cập nhật thành công!", "Thông Báo");
                                }
                                else MessageBox.Show("Biệt danh máy chủ cập nhật thất bại!", "Thông Báo");
                            }
                            break;
                        case "Mời vào máy chủ":
                            await _gd.moivaogroup(userid);
                            break;
                        case "Thêm bạn":
                            string dpname = await _user.FindDisplayname(userid);
                            SearchUser searchUser = new SearchUser(_userid, _gd);
                            await searchUser.GuiKetBan(dpname);
                            break;
                        case "Đuổi user":
                            if (_number < number)
                            {
                                MessageBox.Show("Không có đủ quyền hạn để đuổi người dùng này");
                                break;
                            }
                            await _gd.roinhom(_groupid,true,userid,0);
                            break;
                        case "Vai trò":
                            await showuserrole(userid);
                            break;
                        case "Chỉnh sửa hồ sơ máy chủ":
                            CaiDatNguoiDung caiDatNguoiDung = new CaiDatNguoiDung(_userid, _dn, _gd, username, 2, _groupid);
                            caiDatNguoiDung.ShowDialog();
                            break;
                        case "Chuyển quyền sở hữu":
                            var resultthemrole= await _groupMember.themrole(_groupid,userid, "Owner");
                            var resultgorole= await _groupMember.Goborole(_groupid, _userid);
                            if (resultthemrole && resultgorole)
                            {
                                UserSession.UpdateUserRole = (true, true);
                                _gd.SendUpdate("UpdateRole");
                            }
                            break;
                        default:
                            break;
                    }
                    on = false;
                };

                panel1.Controls.Add(btn);
            }

            form.Controls.Add(panel1);
            form.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async Task showuserrole(string userid)
        {
            try
            {
                Form form2 = new Form
                {
                    Text = "hien role",
                    AutoSize = true,
                    StartPosition = FormStartPosition.CenterParent
                };

                // Create a FlowLayoutPanel to hold the buttons
                FlowLayoutPanel flowLayoutPanel1 = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,            // Dock the panel to fill the form
                    FlowDirection = FlowDirection.TopDown, // Arrange buttons vertically
                    WrapContents = false,            // Don't wrap buttons to next row
                    AutoScroll = true                // Enable scrolling if buttons overflow
                };

                // Add the FlowLayoutPanel to the form
                form2.Controls.Add(flowLayoutPanel1);

                int number = 1;
                string role = await _groupMember.FindOneUserRole(_groupid, userid);

                switch (role)
                {
                    case "Member": number = 1; break;
                    case "Mod": number = 2; break;
                    case "Admin": number = 3; break;
                    case "Owner": number = 4; break;
                }

                var buttons = new List<CheckBox>
        {
            new CheckBox { Text = "Member"},
            new CheckBox { Text = "Mod" },
            new CheckBox { Text = "Admin" },
            new CheckBox { Text = "Owner" },
        };

                foreach (var btn in buttons)
                {
                    btn.Width = 200; // Thiết lập chiều rộng đồng nhất
                    btn.Height = 40; // Thiết lập chiều cao đồng nhất
                    btn.ForeColor = Color.Black;
                    btn.TextAlign = ContentAlignment.MiddleCenter;
                    btn.Margin = new Padding(5); // Khoảng cách giữa các nút
                    btn.Checked = role == btn.Text;

                    btn.Click += async (s, e) =>
                    {
                        try
                        {
                            if (role == btn.Text)
                            {
                                foreach (var Btn in buttons)
                                {
                                    Btn.Checked = Btn.Text == role;
                                }
                                return;
                            }

                            if (role != btn.Text)
                            {
                                if (btn.Text == "Owner")
                                {
                                    foreach (var Btn in buttons)
                                    {
                                        Btn.Checked = Btn.Text == role;
                                    }
                                    return;
                                }

                                if (number >= _number)
                                {
                                    foreach (var Btn in buttons)
                                    {
                                        Btn.Checked = Btn.Text == role;
                                    }
                                    return;
                                }

                                if (!btn.Checked) return;

                                foreach (var otherBtn in buttons)
                                {
                                    if (otherBtn != btn)
                                    {
                                        otherBtn.Checked = false;
                                    }
                                }

                                var themrole = await _groupMember.themrole(_groupid, userid, btn.Text);
                                role = btn.Text;

                                switch (role)
                                {
                                    case "Member": number = 1; break;
                                    case "Mod": number = 2; break;
                                    case "Admin": number = 3; break;
                                    case "Owner": number = 4; break;
                                }

                                if (themrole)
                                {
                                    UserSession.UpdateUserRole = (true, true);
                                    _gd.SendUpdate("UpdateRole");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log hoặc xử lý lỗi
                            MessageBox.Show($"Error in button click: {ex.Message}");
                        }
                    };

                    flowLayoutPanel1.Controls.Add(btn);
                }

                form2.ShowDialog();
            }
            catch (Exception ex)
            {
                // Log hoặc xử lý lỗi
                MessageBox.Show($"Error in showuserrole: {ex.Message}");
            }
        }


        private async void button5_Click(object sender, EventArgs e)
        {
            _userrole = await _groupMember.FindOneUserRole(_groupid, _userid);
            if (_userrole != "Owner")
            {
                MessageBox.Show("Bạn không có quyền xóa group");
                return;
            }
            try { 
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
                var delete =await _group.DeleteGroup(_groupid);
                if (delete)
                {
                    UserSession.UpdateGroup = (true, true);
                    _gd.SendUpdate("GroupDislay");
                    MessageBox.Show("Nhóm đã được xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            else
            {
                // Hủy thao tác
                MessageBox.Show("Thao tác xóa nhóm đã bị hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void CaiDatMayChu_Load(object sender, EventArgs e)
        {
            try { 
            _gdpname = await _groupMember.FindGroupDisplayname(_userid,_groupid);
            _userrole = await _groupMember.FindOneUserRole(_groupid, _userid);
            switch (_userrole)
            {
                case "Member": _number = 1; break;
                case "Mod": _number = 2; break;
                case "Admin": _number = 3; break;
                case "Owner": _number = 4; break;
            }
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
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            try { 
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
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
        }


        private void button4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private async void button12_Click(object sender, EventArgs e)
        {
            try { 
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
                        string userrole = await _groupMember.FindOneUserRole(_groupid, checkBox.Name);
                        int number=1;
                        switch (userrole)
                        {
                            case "Member": number = 1; break;
                            case "Mod": number = 2; break;
                            case "Admin": number = 3; break;
                            case "Owner": number = 4; break;
                        }
                        if (_number <= number)
                        {
                            MessageBox.Show("Không có đủ quyền hạn để đổi role");
                            return;
                        }
                        var themrole=await _groupMember.themrole(_groupid, checkBox.Name, chonvaitro);
                        if (themrole)
                        {
                            UserSession.UpdateUserRole = (true, true);
                            _gd.SendUpdate("UpdateRole");

                            var index = users.FindIndex(user => user.UserId == checkBox.Name);

                            if (index != -1)
                            {
                                var user = users[index];
                                users[index] = (user.gdpname, user.UserId, chonvaitro);

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
                                        Text = user.gdpname,
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
                                        Xoathanhvien(user.gdpname, user.UserId, user.UserRole);
                                    };
                                    panel.Controls.Add(xoa);
                                    flowLayoutPanel1.Controls.Add(panel);
                                }
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
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void label30_Click(object sender, EventArgs e)
        {
            try { 
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
                        Text = user.gdpname,
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
                        Xoathanhvien(user.gdpname, user.UserId, user.UserRole);
                    };
                    panel.Controls.Add(xoa);
                    flowLayoutPanel1.Controls.Add(panel);
                }
            }
                UserSession.ActionUpdateGroupMember -= hamlabel30click;
                UserSession.ActionUpdateRole -= hamlabel30click;
                UserSession.ActionDeleteuser -= hamlabel30click;
                UserSession.ActionUpdateGroupMember += hamlabel30click;
                UserSession.ActionUpdateRole += hamlabel30click;
                UserSession.ActionDeleteuser += hamlabel30click;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void hamlabel30click()
        {
            label30_Click(null, EventArgs.Empty);
        }
        private async void label27_Click(object sender, EventArgs e)
        {
            try { 
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
                        Text = user.        gdpname,
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
                        Xoathanhvien(user.gdpname, user.UserId, user.UserRole);
                    };
                    panel.Controls.Add(xoa);
                    flowLayoutPanel1.Controls.Add(panel);
                }
            }
                UserSession.ActionUpdateGroupMember -= hamlabel27click;
                UserSession.ActionUpdateRole -= hamlabel27click;
                UserSession.ActionDeleteuser -= hamlabel27click;
                UserSession.ActionUpdateGroupMember += hamlabel27click;
                UserSession.ActionUpdateRole += hamlabel27click;
                UserSession.ActionDeleteuser += hamlabel27click;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void hamlabel27click()
        {
            label27_Click(null, EventArgs.Empty);
        }
        

        private async void label25_Click(object sender, EventArgs e)
        {
            try { 
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
                        Text = user.gdpname,
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
                        Xoathanhvien(user.gdpname, user.UserId, user.UserRole);
                    };
                    panel.Controls.Add(xoa);
                    flowLayoutPanel1.Controls.Add(panel);
                }
            }
                UserSession.ActionUpdateGroupMember -= hamlabel25click;
                UserSession.ActionUpdateRole -= hamlabel25click;
                UserSession.ActionDeleteuser -= hamlabel25click;
                UserSession.ActionUpdateGroupMember += hamlabel25click;
                UserSession.ActionUpdateRole += hamlabel25click;
                UserSession.ActionDeleteuser += hamlabel25click;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void hamlabel25click()
        {
            label25_Click(null, EventArgs.Empty);
        }

        private async void label24_Click(object sender, EventArgs e)
        {
            try { 
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
                        Text = user.gdpname,
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
                        Xoathanhvien(user.gdpname, user.UserId, user.UserRole);
                    };
                    panel.Controls.Add(xoa);
                    flowLayoutPanel1.Controls.Add(panel);
                }
            }
                UserSession.ActionUpdateGroupMember -= hamlabel24click;
                UserSession.ActionUpdateRole -= hamlabel24click;
                UserSession.ActionDeleteuser -= hamlabel24click;
                UserSession.ActionUpdateGroupMember += hamlabel24click;
                UserSession.ActionUpdateRole += hamlabel24click;
                UserSession.ActionDeleteuser += hamlabel24click;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void hamlabel24click()
        {
            label24_Click(null, EventArgs.Empty);
        }

        private async void Xoathanhvien(string gdpname, string userid, string role)
        {
            try { 
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
                string userrole = await _groupMember.FindOneUserRole(_groupid, userid);
                int number = 1;
                switch (userrole)
                {
                    case "Member": number = 1; break;
                    case "Mod": number = 2; break;
                    case "Admin": number = 3; break;
                    case "Owner": number = 4; break;
                }
                if (_number <= number)
                {
                    MessageBox.Show("Không có đủ quyền hạn để đổi role");
                    return;
                }
                var gorole=await _groupMember.Goborole(_groupid, userid);
                if (gorole)
                {
                    UserSession.UpdateUserRole = (true, true);
                    _gd.SendUpdate("UpdateRole");
                    var index = users.FindIndex(user => user.UserId == userid);

                    if (index != -1)
                    {
                        var user = users[index];
                        users[index] = (user.gdpname, user.UserId, "Member");

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
                                Text = user.gdpname,
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
                                Xoathanhvien(user.gdpname, user.UserId, user.UserRole);
                            };
                            panel.Controls.Add(xoa);
                            flowLayoutPanel1.Controls.Add(panel);
                        }
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
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private async void btnSearch_Click(object sender, EventArgs e)
        {
            try { 
            if (chonvaitro == "" || string.IsNullOrEmpty(txtUser.Text)) return;
            var members = users
                .Where(user => user.UserRole.Equals(chonvaitro, StringComparison.OrdinalIgnoreCase)
                               && user.gdpname.Contains(txtUser.Text))
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
                        Text = user.gdpname,
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
                        Xoathanhvien(user.gdpname, user.UserId, user.UserRole);
                    };
                    panel.Controls.Add(xoa);
                    flowLayoutPanel1.Controls.Add(panel);
                }
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
    }

