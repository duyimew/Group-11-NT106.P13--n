using QLUSER.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using chatclient.DTOs.User;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading;

using System.Configuration;
namespace QLUSER
{
    public partial class CaiDatNguoiDung : Form
    {
        bool change = false;
        Dangnhap DN;
        string _userid;
        Find find = new Find();
        string _groupid=null;
        UserAvatar userAvatar = new UserAvatar();
        Group _group = new Group();
        string _displayname;
        string _username;
        Token token = new Token();
        GiaoDien GD;
        file file1 = new file();
        UserAvatar avatar = new UserAvatar();
        User _user = new User();
        GroupMember groupMember = new GroupMember();
        private List<(string id,string name)> groups = new List<(string,string)>();
        public CaiDatNguoiDung(string userid, Dangnhap dN, GiaoDien gd,string username,int tagepage,string groupid)
        {
            InitializeComponent();
            _userid = userid;
            DN = dN;
            GD = gd;
            _username = username;
            tabControl1.SelectedIndex = tagepage;
            _groupid = groupid;
            UserSession.ActionAvatarUpdated += UpdateAvatarDisplay;
            UserSession.ActionUpdateusername += UpdateUsername;
            UserSession.ActionUpdatedpname += Updatedpname;
        }
        private async void UpdateUsername()
        {
            try { 
            _username = await _user.FindUsername(_userid);
            label4.Text = _username;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void Updatedpname()
        {
            try { 
            _displayname = await _user.FindDisplayname(_userid);
            lb_uname.Text = _displayname;
            label2.Text = _displayname;
            label12.Text = _displayname;
            textBox1.Text = _displayname;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            UserSession.ActionAvatarUpdated -= UpdateAvatarDisplay;
            UserSession.ActionUpdateusername -= UpdateUsername;
            UserSession.ActionUpdatedpname -= Updatedpname;
        }

        private async void button1_Click(object sender, EventArgs e)
        {

            if (change)
            {
                Image image = await userAvatar.LoadAvatarAsync(_userid);
                circularPicture1.Image = image;
                circularPicture2.Image = image;
                circularPicture3.Image = image;
                change = false;
            }
            if (textBox1.Text != _displayname && !string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = _displayname;
            }
            if (textBox2.Text != label14.Text && !string.IsNullOrWhiteSpace(textBox2.Text))
            {
                textBox2.Text = label14.Text;
            }
            tabControl1.SelectedIndex = 0;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (change)
            {
                Image image = await userAvatar.LoadAvatarAsync(_userid);
                circularPicture1.Image = image;
                circularPicture2.Image = image;
                circularPicture3.Image = image;
                change = false;
            }
            if (textBox1.Text != _displayname && !string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text= _displayname;
            }
            if (textBox2.Text != label14.Text && !string.IsNullOrWhiteSpace(textBox2.Text))
            {
                textBox2.Text = label14.Text;
            }
            tabControl1.SelectedIndex = 1;
        }

        private async void button15_Click(object sender, EventArgs e)
        {
            if (change)
            {
                Image image = await userAvatar.LoadAvatarAsync(_userid);
                circularPicture1.Image = image;
                circularPicture2.Image = image;
                circularPicture3.Image = image;
                change = false;
            }
            if (textBox1.Text != _displayname && !string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = _displayname;
            }
            if (textBox2.Text != label14.Text && !string.IsNullOrWhiteSpace(textBox2.Text))
            {
                textBox2.Text = label14.Text;
            }
            tabControl1.SelectedIndex = 1;
        }

        private async void button14_Click(object sender, EventArgs e)
        {
            if (change)
            {
                Image image = await userAvatar.LoadAvatarAsync(_userid);
                circularPicture1.Image = image;
                circularPicture2.Image = image;
                circularPicture3.Image = image;
                change = false;
            }
            if (textBox1.Text != _displayname && !string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = _displayname;
            }
            if (textBox2.Text != label14.Text && !string.IsNullOrWhiteSpace(textBox2.Text))
            {
               textBox2.Text = label14.Text;
            }
           
            tabControl1.SelectedIndex = 2;
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            if (change)
            {
                Image image = await userAvatar.LoadAvatarAsync(_userid);
                circularPicture1.Image = image;
                circularPicture2.Image = image;
                circularPicture3.Image = image;
                change = false;
            }
            if (textBox1.Text != _displayname && !string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = _displayname;
            }
            if (textBox2.Text != label14.Text && !string.IsNullOrWhiteSpace(textBox2.Text))
            {
                textBox2.Text = label14.Text;
            }
            tabControl1.SelectedIndex = 1;
        }

        private async void button12_Click(object sender, EventArgs e)
        {
            if (change)
            {
                Image image = await userAvatar.LoadAvatarAsync(_userid);
                circularPicture1.Image = image;
                circularPicture2.Image = image;
                circularPicture3.Image = image;
                change = false;
            }
            if (textBox1.Text != _displayname && !string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = _displayname;
            }
            if (textBox2.Text != label14.Text && !string.IsNullOrWhiteSpace(textBox2.Text))
            {
                textBox2.Text = label14.Text;
            }
            tabControl1.SelectedIndex = 1;
        }

        private async void button13_Click(object sender, EventArgs e)
        {
            if (change)
            {
                Image image = await userAvatar.LoadAvatarAsync(_userid);
                circularPicture1.Image = image;
                circularPicture2.Image = image;
                circularPicture3.Image = image;
                change = false;
            }
            if (textBox1.Text != _displayname && !string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = _displayname;
            }
            if (textBox2.Text != label14.Text && !string.IsNullOrWhiteSpace(textBox2.Text))
            {
                textBox2.Text = label14.Text;
            }
            tabControl1.SelectedIndex = 2;
        }

        private async void button3_Click(object sender, EventArgs e)
        {

                string tokenFile = find.find("token.txt");
                if (tokenFile != null && File.Exists(tokenFile))
                {
                    File.Delete(tokenFile);
                }
                MessageBox.Show("Đăng xuất thành công!");
                if (GD.connection != null)
                    await GD.StopSignalR();
                DN.Show();
                this.Close();
                GD.Close();
            

        }

        private async void CaiDatNguoiDung_Load(object sender, EventArgs e)
        {
            try
            {
                _displayname = await _user.FindDisplayname(_userid);
                string[] text = await get(_displayname);
                lb_uname.Text = _displayname;
                label2.Text = _displayname;
                label12.Text = _displayname;
                textBox1.Text = _displayname;
                label4.Text = _username;
                lb_mail.Text = text[0];
                lb_name.Text = text[1];
                lb_bd.Text = text[2];
                var result=await _group.RequestGroupName(_userid, 0);
                if(result.issuccess)
                {
                    if (result.groupidname.Length!=0)
                    {
                        foreach (var group in result.groupidname)
                        {
                            string[] groupidname = group.Split('|');
                            groups.Add((groupidname[0], groupidname[1]));
                            comboBox1.Items.Add(groupidname[1]);
                        }
                        if(_groupid!=null)
                        {
                            int index = groups.FindIndex(g => g.Item1 == _groupid);
                            if (index >= 0)
                            {
                                comboBox1.SelectedIndex = index;
                            }
                            else
                            {
                                MessageBox.Show("Group ID không tồn tại trong danh sách.");
                            }
                        }
                        else comboBox1.SelectedIndex = 0;
                    }
                    else
                    {
                        label14.Text = "null";
                        textBox2.Text = "null";
                    }
                }
                UserSession.ActionUpdateGroup += () => {
                    if (this != null && !this.IsDisposed)
                    {
                        this.Close();
                    }
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi không lấy được thông tin người dùng: " + ex.Message);
            }

                Image image = await userAvatar.LoadAvatarAsync(_userid);
                circularPicture1.Image = image;
                circularPicture2.Image = image;
                circularPicture3.Image = image;
            
        }
        private async void UpdateAvatarDisplay()
        {
            Image image = await userAvatar.LoadAvatarAsync(_userid);
            circularPicture1.Image = image;
            circularPicture2.Image = image;
            circularPicture3.Image = image;
        }

        private async Task<string[]> get(string displayname)
        {
            string[] text = new string[3];
            text[0] = "";
            text[1] = "";
            text[2] = "";
            try
            {
                var InforUser = new InforuserDTO
                {
                    displayname = displayname,
                };
                var json = JsonConvert.SerializeObject(InforUser);
                var content = new StringContent(json, Encoding.Unicode, "application/json");
                HttpClient client = new HttpClient();
                var response = await client.PostAsync(ConfigurationManager.AppSettings["ServerUrl"] + "User/InforUser", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    text[0] = responseData.email;
                    text[1] = responseData.ten;
                    text[2] = responseData.ngaysinh;
                    return text;
                }
                else
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    string message = responseData.message;
                    MessageBox.Show(message);
                    return text;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đăng nhập" + ex.Message);
                return text;
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tắt form thông tin người dùng " + ex.Message);
            }
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            try {    
            string newDisplayName = Microsoft.VisualBasic.Interaction.InputBox(
           $"Nhập tên hiển thị mới:",
           "Đổi Tên Hiển Thị",
           _displayname
            );
            if (newDisplayName!=_displayname&&!string.IsNullOrWhiteSpace(newDisplayName))
            {
                var result=await _user.RenameDisplayname(newDisplayName, _userid);
                if (result)
                {
                    _displayname = newDisplayName;
                    lb_uname.Text = newDisplayName;
                    label2.Text = newDisplayName;
                    label12.Text = newDisplayName;
                    textBox1.Text = newDisplayName;
                    UserSession.Renamedpname = true;
                    GD.SendUpdate("dpname");
                    MessageBox.Show("Tên hiển thị đã được cập nhật thành công!", "Thông Báo");
                }
                else MessageBox.Show("Tên hiển thị cập nhật thất bại!", "Thông Báo");
            }
            else
            {
                MessageBox.Show("Tên hiển thị không thay đổi.", "Thông Báo");
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private Action actionupdategdpname;
        private async void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try { 
            string selectedItem = comboBox1.SelectedIndex.ToString();
            string groupid = groups[int.Parse(selectedItem)].id;
            string groupdisplayname = await groupMember.FindGroupDisplayname(_userid, groupid);
            label14.Text = groupdisplayname;
            label14.Name = groupid;
            textBox2.Text = groupdisplayname;
                if (actionupdategdpname != null)
                    UserSession.ActionUpdategdpname -= actionupdategdpname;

                actionupdategdpname = () => hamupdategdpname(groupid);
                UserSession.ActionUpdategdpname += actionupdategdpname;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void hamupdategdpname(string groupid)
        {
            string groupdisplayname1 = await groupMember.FindGroupDisplayname(_userid, groupid);
            label14.Text = groupdisplayname1;
            textBox2.Text = groupdisplayname1;
        }
        private async void button8_Click(object sender, EventArgs e)
        {
            try { 
            string newusername = Microsoft.VisualBasic.Interaction.InputBox(
           $"Nhập tên đăng nhập mới:",
           "Đổi Tên đăng nhập",
           _username
            );
            if (newusername != _username && !string.IsNullOrWhiteSpace(newusername))
            {
                var result = await _user.Renameusername(newusername, _userid);
                if (result)
                {
                    _username = newusername;
                    label4.Text = newusername;
                    UserSession.Renameusername = true;
                    MessageBox.Show(" Tên đăng nhập đã được cập nhật thành công!", "Thông Báo"); }

                else MessageBox.Show(" Tên đăng nhập cập nhật thất bại!", "Thông Báo");
            }
            else
            {
                MessageBox.Show(" Tên đăng nhập không thay đổi.", "Thông Báo");
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void button9_Click(object sender, EventArgs e)
        {
            try { 
            string newemail = Microsoft.VisualBasic.Interaction.InputBox(
           $"Nhập email mới:",
           "Đổi email",
           lb_mail.Text
            );

            if (newemail != lb_mail.Text && !string.IsNullOrWhiteSpace(newemail)&& _user.IsValidEmail(newemail))
            {
                var result = await _user.Renameemail(newemail, _userid);
                if (result)
                {
                    lb_mail.Text = newemail;
                    MessageBox.Show(" Email đã được cập nhật thành công!", "Thông Báo");
                }
                else MessageBox.Show(" Email cập nhật thất bại!", "Thông Báo");
            }
            else
            {
                MessageBox.Show(" Email không thay đổi.", "Thông Báo");
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void button10_Click(object sender, EventArgs e)
        {
            try { 
            string newten = Microsoft.VisualBasic.Interaction.InputBox(
           $"Nhập Fullname mới:",
           "Đổi Fullname",
           lb_name.Text
            );
            if (newten != lb_name.Text && !string.IsNullOrWhiteSpace(newten))
            {
                var result = await _user.Renameten(newten, _userid);
                if (result)
                {
                    lb_name.Text = newten;
                    MessageBox.Show(" Fullname đã được cập nhật thành công!", "Thông Báo"); }
                else MessageBox.Show(" Fullname cập nhật thất bại!", "Thông Báo");
            }
            else
            {
                MessageBox.Show(" Fullname không thay đổi.", "Thông Báo");
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private async void button11_Click(object sender, EventArgs e)
        {
            try { 
            string newngaysinh = Microsoft.VisualBasic.Interaction.InputBox(
           $"Nhập ngày sinh mới:",
           "Đổi ngày sinh",
        lb_bd.Text
            );
            if (newngaysinh != lb_bd.Text && !string.IsNullOrWhiteSpace(newngaysinh))
            {
                var result = await _user.Renamengaysinh(newngaysinh, _userid);
                if (result)
                {
                    lb_bd.Text = newngaysinh;
                    MessageBox.Show(" ngày sinh đã được cập nhật thành công!", "Thông Báo");
                }
                else MessageBox.Show(" ngày sinh cập nhật thất bại!", "Thông Báo");
            }
            else
            {
                MessageBox.Show(" ngày sinh không thay đổi.", "Thông Báo");
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void button7_Click(object sender, EventArgs e)
        {
            try { 
            // Hiển thị hộp thoại xác nhận
            var result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa tài khoàn này không?",
                "Xác nhận xóa tài khoản",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            // Kiểm tra kết quả người dùng chọn
            if (result == DialogResult.Yes)
            {
                var result1 = await _group.RequestGroupName(_userid, 2);
                var result2 = await _group.RequestGroupName(_userid, 1);
                if(result1.groupidname!=null)
                {
                    foreach(var group in result1.groupidname)
                    {
                        string[] groupid = group.Split('|');
                        await _group.RemoveMemberFromGroup(groupid[0], _userid);
                    }
                }
                if (result2.groupidname != null)
                {
                    foreach (var group in result2.groupidname)
                    {
                        string[] groupid = group.Split('|');
                        await _group.RemoveMemberFromGroup(groupid[0], _userid);
                    }
                }
                var delete =await _user.DeleteUser(_userid);
                if (delete)
                {
                    UserSession.DeleteUser = true;
                    GD.SendUpdate("Deleteuser");
                    if (GD.connection != null)
                        await GD.StopSignalR();
                    MessageBox.Show("Tài khoản đã được xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    string tokenFile = find.find("token.txt");
                    if (tokenFile != null && File.Exists(tokenFile))
                    {
                        File.Delete(tokenFile);
                    }
                    DN.Show();
                    GD.Close();
                    this.Close();
                }
            }
            else
            {
                // Hủy thao tác
                MessageBox.Show("Thao tác xóa tài khoản đã bị hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(async () =>
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                            circularPicture2.Invoke((MethodInvoker)(() =>
                            {
                                if (circularPicture2.Image != null)
                                {
                                    circularPicture2.Image.Dispose();
                                }
                                circularPicture2.Image = Image.FromFile(openFileDialog.FileName);
                                change = true;
                                circularPicture2.filepath = openFileDialog.FileName;
                            }));
                    }
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private async void button17_Click(object sender, EventArgs e)
        {
            try { 
            if(change)
            {
                            string avatarUrl = await avatar.UploadAvatarAsync(circularPicture2.filepath, _userid);

                            if (avatarUrl != null)
                            {
                                UserSession.AvatarUrl = true;
                                GD.SendUpdate("AvatarDisplay");
                                MessageBox.Show("Avatar uploaded successfully!");
                                circularPicture1.Invoke((MethodInvoker)(() =>
                                {
                                    if (circularPicture1.Image != null)
                                    {
                                        circularPicture1.Image.Dispose();
                                    }
                                    circularPicture1.Image = Image.FromFile(circularPicture2.filepath);
                                }));
                                circularPicture3.Invoke((MethodInvoker)(() =>
                    {
                        if (circularPicture3.Image != null)
                        {
                            circularPicture3.Image.Dispose();
                        }
                        circularPicture3.Image = Image.FromFile(circularPicture2.filepath);
                    }));
                }
                            else
                            {
                                MessageBox.Show("Failed to upload avatar.");
                            }

                change = false;
              
            }


                if (textBox1.Text != _displayname && !string.IsNullOrWhiteSpace(textBox1.Text))
                {
                var result = await _user.RenameDisplayname(textBox1.Text, _userid);
                if (result)
                {

                    _displayname = textBox1.Text;
                    lb_uname.Text = textBox1.Text;
                    label2.Text = textBox1.Text;
                    label12.Text = textBox1.Text;
                    UserSession.Renamedpname = true;
                    GD.SendUpdate("dpname");
                    MessageBox.Show("Tên hiển thị đã được cập nhật thành công!", "Thông Báo");
                }
                else MessageBox.Show("Tên hiển thị cập nhật thất bại!", "Thông Báo");
            }
                else
                {
                    MessageBox.Show("Tên hiển thị không thay đổi.", "Thông Báo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button19_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(async () =>
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        circularPicture3.Invoke((MethodInvoker)(() =>
                        {
                            if (circularPicture3.Image != null)
                            {
                                circularPicture3.Image.Dispose();
                            }
                            circularPicture3.Image = Image.FromFile(openFileDialog.FileName);
                            change = true;
                            circularPicture3.filepath = openFileDialog.FileName;
                        }));
                    }
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private async void button18_Click(object sender, EventArgs e)
        {
            try { 
            if (change)
            {
                string avatarUrl = await avatar.UploadAvatarAsync(circularPicture3.filepath, _userid);

                if (avatarUrl != null)
                {
                    UserSession.AvatarUrl = true;
                    GD.SendUpdate("AvatarDisplay");
                    MessageBox.Show("Avatar uploaded successfully!");
                    circularPicture1.Invoke((MethodInvoker)(() =>
                    {
                        if (circularPicture1.Image != null)
                        {
                            circularPicture1.Image.Dispose();
                        }
                        circularPicture1.Image = Image.FromFile(circularPicture3.filepath);
                    }));
                    circularPicture2.Invoke((MethodInvoker)(() =>
                    {
                        if (circularPicture2.Image != null)
                        {
                            circularPicture2.Image.Dispose();
                        }
                        circularPicture2.Image = Image.FromFile(circularPicture3.filepath);
                    }));
                }
                else
                {
                    MessageBox.Show("Failed to upload avatar.");
                }

                change = false;

            }


            if (textBox2.Text != label14.Text && !string.IsNullOrWhiteSpace(textBox2.Text))
            {
                var result = await groupMember.RenameGroupDisplayname(textBox2.Text, label14.Name,_userid);
                if (result)
                {
                    label14.Text = textBox2.Text;
                    UserSession.Renamegdpname = (true,true);
                    GD.SendUpdate("Updategdpname");
                    MessageBox.Show("Biệt danh máy chủ đã được cập nhật thành công!", "Thông Báo");
                }
                else MessageBox.Show("Biệt danh máy chủ cập nhật thất bại!", "Thông Báo");
            }
            else
            {
                MessageBox.Show("Biệt danh máy chủ không thay đổi.", "Thông Báo");
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btn_ChangePass_Click(object sender, EventArgs e)
        {
            ChangePass cp = new ChangePass(_userid);
            cp.Show();
        }
    }
}
