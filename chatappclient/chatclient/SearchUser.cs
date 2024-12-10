
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
        private string _displayname;
        public SearchUser(string displayname)
        {
            InitializeComponent();
            _displayname = displayname;
        }

        private Panel UserRow(string displayname)
        {
            Panel userRow = new Panel {
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

            username.DoubleClick += new EventHandler(async (obj, args) =>
            {
                var requestInfo = new SendFriendRequestDTO 
                {
                    sender = _displayname,
                    receiver = displayname
                };
                var json = JsonConvert.SerializeObject(requestInfo);
                var content = new StringContent(json, Encoding.Unicode, "application/json");
                HttpClient client = new HttpClient();
                var response = await client.PostAsync(ConfigurationManager.AppSettings["ServerUrl"] + "Friend/Request", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    var message = responseData.message.ToString();
                    MessageBox.Show(message);
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<dynamic>(errorMessage);
                    string message = responseData.message.ToString();
                    MessageBox.Show(message);
                }
            });

            userRow.Controls.Add(username);
            return userRow;
        }

        private async void btnSearch_ClickAsync(object sender, EventArgs e)
        {
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
    }
}
