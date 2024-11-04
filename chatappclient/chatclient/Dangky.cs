using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net.Sockets;
using System.Net.Http;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Net;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Threading;
using System.Net.NetworkInformation;
using QLUSER.DTOs;
using Newtonsoft.Json;
using QLUSER.Models;
using System.Configuration;
namespace QLUSER
{
    public partial class Dangky : Form
    {
        private Dangnhap DN;
        User user = new User();
        public Dangky(Dangnhap dn)
        {
            InitializeComponent();
            DN = dn;
        }
        private void bt_thoat_Click(object sender, EventArgs e)
        {
            try
            {
                DN.Show();
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Lỗi thoát đăng ký " + ex.Message);
            }
        }
        private async void bt_DK_Click(object sender, EventArgs e)
        {
            try
            {
                string username = tb_username.Text;
                string password = tb_pwd.Text;
                string confirmPassword = tb_cfpwd.Text;
                string email = tb_email.Text;
                string ten = tb_hoten.Text;
                string ngaysinh = tb_ngsinh.Text;
                if (password != confirmPassword)
                {
                    MessageBox.Show("Mật khẩu và xác nhận mật khẩu không trùng khớp.");
                    return;
                }
                if (!user.IsValidEmail(email))
                {
                    MessageBox.Show("Email không đúng định dạng.");
                    return;
                }
                string hashpwd = user.HashPassword(password);
                if (hashpwd == null)
                {
                    MessageBox.Show("Mã hóa thất bại.");
                    return;
                }
                var DangKy = new DangKyDTO
                {
                    Username = username,
                    HashPassword=hashpwd,
                    Email=email,
                    Ten=ten,
                    NgaySinh=ngaysinh
                };
                var json = JsonConvert.SerializeObject(DangKy);
                var content = new StringContent(json, Encoding.Unicode, "application/json");
                HttpClient client = new HttpClient();
                var response = await client.PostAsync(ConfigurationManager.AppSettings["ServerUrl"] + "Dangky/DangKy", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    string message = responseData.message;
                    MessageBox.Show(message);
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<dynamic>(errorMessage);
                    string message = responseData.message;
                    MessageBox.Show(message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đăng ký " + ex.Message);
            }
        }
    }
}




