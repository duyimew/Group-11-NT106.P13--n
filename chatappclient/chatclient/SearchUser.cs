
using Newtonsoft.Json;
using chatclient.DTOs.User;
using chatclient.DTOs.Friends;
using QLUSER.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace QLUSER
{
    public partial class SearchUser : Form
    {
        GiaoDien _gd;
        private string _displayname;
        private string _userid;
        User _user = new User();
        public SearchUser(string userid, GiaoDien gd)
        {
            InitializeComponent();
            _userid = userid;
            _gd = gd;
        }

        private async Task<Panel> UserRow(string displayname)
        {
            try { 
            string userid = await _user.finduserid(displayname);
            UserSession.ActionUpdatedpname += async () => {
                displayname = await _user.FindDisplayname(userid);
                
            };

            Panel userRow = new Panel
            {
                Height = 30,
                Width = 360,
                Dock = DockStyle.Top
            };
            Label username = new Label
            {
                Text = displayname,
                Dock = DockStyle.Fill,
                ForeColor = Color.White,
            };
            UserSession.ActionUpdatedpname += async () => {
                displayname = await _user.FindDisplayname(userid);
                username.Text = displayname;
            };
            username.DoubleClick += new EventHandler(async (obj, args) =>
            {
                HttpClient client = new HttpClient();
                try
                {
                    string serverUrl = ConfigurationManager.AppSettings["ServerUrl"];
                    var senderData = new { displayname = _displayname };
                    var senderJson = JsonConvert.SerializeObject(senderData);
                    var senderContent = new StringContent(senderJson, Encoding.UTF8, "application/json");

                    var findSenderResponse = await client.PostAsync(serverUrl + "User/FindUserID", senderContent);

                    var receiverData = new { displayname = displayname };
                    var receiverJson = JsonConvert.SerializeObject(receiverData);
                    var receiverContent = new StringContent(receiverJson, Encoding.UTF8, "application/json");

                    var findReceiverResponse = await client.PostAsync(serverUrl + "User/FindUserID", receiverContent);
                    if (!findSenderResponse.IsSuccessStatusCode || !findReceiverResponse.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Không tìm thấy người dùng.");
                        return;
                    }
                    var senderResponseContent = await findSenderResponse.Content.ReadAsStringAsync();
                    var senderResponse = JsonConvert.DeserializeObject<dynamic>(senderResponseContent);

                    var receiverResponseContent = await findReceiverResponse.Content.ReadAsStringAsync();
                    var receiverResponse = JsonConvert.DeserializeObject<dynamic>(receiverResponseContent);

                    int senderID = senderResponse.userid;
                    int receiverID = receiverResponse.userid;

                    if (senderID == receiverID)
                    {
                        MessageBox.Show("Không thể gửi yêu cầu kết bạn cho chính mình.");
                        return;
                    }

                    var requestInfo = new SendFriendRequestDTO
                    {
                        senderID = senderID,
                        receiverID = receiverID
                    };

                    var json = JsonConvert.SerializeObject(requestInfo);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(serverUrl + "Friend/Request", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                        _gd.SendUpdate("UpdateFriendRequest");
                        MessageBox.Show(responseData.message.ToString());
                    }
                    else
                    {
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        var errorData = JsonConvert.DeserializeObject<dynamic>(errorMessage);
                        MessageBox.Show(errorData.message.ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            });

            userRow.Controls.Add(username);
            return userRow;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private async void btnSearch_ClickAsync(object sender, EventArgs e)
        {
            try { 
            panelList.Controls.Clear();
            var Username = new InforuserDTO
            {
                displayname = txtUser.Text ?? "",
            };
            var json = JsonConvert.SerializeObject(Username);
            var content = new StringContent(json, Encoding.Unicode, "application/json");
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(ConfigurationManager.AppSettings["ServerUrl"] + "User/FindUser", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                foreach (var user in (JsonConvert.DeserializeObject<dynamic>(responseContent).message))
                {
                    panelList.Controls.Add(UserRow(user.ToString()));
                }
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(errorMessage);
                string message = responseData.message;
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void SearchUser_Load(object sender, EventArgs e)
        {
            try { 
            _displayname = await _user.FindDisplayname(_userid);
            UserSession.ActionUpdatedpname += async () => {
                _displayname = await _user.FindDisplayname(_userid);
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
    }
}
