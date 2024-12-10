using Newtonsoft.Json;
using chatclient.DTOs.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using QLUSER.Models;
using System.Configuration;
using System.Threading;
using System.Net.Http.Headers;
namespace QLUSER
{
    public partial class Formuser : Form
    {
        Dangnhap DN;
        string userid;
        Find find = new Find();
        string _displayname;
        Token token = new Token();
        GiaoDien GD;
        file file1 = new file();
        UserAvatar avatar = new UserAvatar();
        User _user = new User();
        public Formuser(string displayname, Dangnhap dN, GiaoDien gd)
        {
            InitializeComponent();
            _displayname = displayname;
            DN = dN;
            GD = gd;
            UserSession.AvatarUpdated += UpdateAvatarDisplay;
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            UserSession.AvatarUpdated -= UpdateAvatarDisplay;
        }

        private async void Formuser_Load(object sender, EventArgs e)
        {
            try
            {
                userid = await _user.finduserid(_displayname);
                string[] text = await get(_displayname);
                lb_uname.Text = _displayname;
                lb_mail.Text = text[0];
                lb_name.Text = text[1];
                lb_bd.Text = text[2];

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi không lấy được thông tin người dùng: " + ex.Message);
            }
            UserAvatar userAvatar = new UserAvatar();
            Image avatarImage = await userAvatar.LoadAvatarAsync(userid);

            if (avatarImage != null)
            {
                circularPicture1.Image = avatarImage;
            }

        }
        private async void UpdateAvatarDisplay()
        {
            circularPicture1.Image = await avatar.LoadAvatarAsync(userid);
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


        private void bt_thoat_Click(object sender, EventArgs e)
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

        private async void bt_dangxuat_Click(object sender, EventArgs e)
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
            GD.Close();
            this.Close();
        }

        private void lb_username_Click(object sender, EventArgs e)
        {

        }

        private void btn_ChangeAva_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(async () =>
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string avatarUrl = await avatar.UploadAvatarAsync(openFileDialog.FileName, userid);

                        if (avatarUrl != null)
                        {
                            UserSession.AvatarUrl = avatarUrl;
                            MessageBox.Show("Avatar uploaded successfully!");
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

        private void circularPicture1_Click(object sender, EventArgs e)
        {

        }
    }
}
