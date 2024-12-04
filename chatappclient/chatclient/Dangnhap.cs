using Newtonsoft.Json;
using QLUSER.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using QLUSER.Models;
using System.Configuration;
namespace QLUSER
{
    public partial class Dangnhap : Form
    {
        User user = new User();
        Find find =new Find();

        public Dangnhap()
        {
            InitializeComponent();
            this.Hide();
        }
        private void bt_DK_Click(object sender, EventArgs e)
        {
            try
            {
                Dangky dk = new Dangky(this);
                dk.Show();
                this.Hide();
            }
            catch (Exception ex)
            { MessageBox.Show("Lỗi mở form đảng ký" + ex.Message); }
        }
        private void bt_thoat_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            { MessageBox.Show("Lỗi tắt form đăng nhập " + ex.Message); }
        }
        private async void bt_DN_Click(object sender, EventArgs e)
        {
            try
            {
                string username = tb_username.Text;
                string password = tb_pwd.Text;
                string hashpwd = user.HashPassword(password);
                if (hashpwd == null)
                {
                    MessageBox.Show("Mã hóa thất bại");
                    return;
                }
                var dangnhap = new DangNhapDTO
                {
                    Username = username,
                    HashPassword = hashpwd,
                };
                var json = JsonConvert.SerializeObject(dangnhap);
                var content = new StringContent(json, Encoding.Unicode, "application/json");
                HttpClient client = new HttpClient();
                var response = await client.PostAsync(ConfigurationManager.AppSettings["ServerUrl"] + "DangNhap/DangNhap", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    string message = responseData.message;
                    string userid = responseData.userid;
                    MessageBox.Show(message);
                    GiaoDien giaodien = new GiaoDien(username, userid, this);
                    giaodien.Show();
                    this.Hide();
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
                MessageBox.Show("Lỗi đăng nhập" + ex.Message);
            }
        }


        private void Dangnhap_Load(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
