using Newtonsoft.Json;
using QLUSER.DTOs;
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
namespace QLUSER
{
    public partial class Formuser : Form
    {
        Dangnhap DN;
        Find find= new Find();
        string username1;
        Token token = new Token();
        GiaoDien GD;
        public Formuser(string username, Dangnhap dN,GiaoDien gd)
        {
            InitializeComponent();
            username1 = username;
            DN = dN;
            GD = gd;
        }

        private async void Formuser_Load(object sender, EventArgs e)
        {
            try
            {
                string[] text = await get(username1);
                tb_username.Text = username1;
                tb_email.Text = text[0];
                tb_HienTen.Text = text[1];
                tb_HIenNgSinh.Text = text[2];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi không lấy được thông tin người dùng: " + ex.Message);
            }
        }

        private async Task<string[]> get(string username)
        {
            string[] text = new string[3];
            text[0] = "";
            text[1] = "";
            text[2] = "";
            try
            {
                var InforUser = new InforuserDTO
                {
                    Username=username,
                };
                var json = JsonConvert.SerializeObject(InforUser);
                var content = new StringContent(json, Encoding.Unicode, "application/json");
                HttpClient client = new HttpClient();
                var response = await client.PostAsync(ConfigurationManager.AppSettings["ServerUrl"] + "InforUser/InforUser", content);
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

        private void bt_dangxuat_Click(object sender, EventArgs e)
        {
            string tokenFile = find.find("token.txt");
            if (tokenFile != null && File.Exists(tokenFile))
            {
                File.Delete(tokenFile);
            }
            MessageBox.Show("Đăng xuất thành công!");
            DN.Show();
            GD.Close();
            this.Close();
        }
    }
}
