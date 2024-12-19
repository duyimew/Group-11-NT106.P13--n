using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using System.Configuration;

namespace QLUSER
{
    public partial class ForgotPass : Form
    {
        public ForgotPass()
        {
            InitializeComponent();
        }

        private async void btn_SendEmail_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            if (!string.IsNullOrEmpty(email))
            {
                await SendOtpAsync(email);
            }
            else
            {
                MessageBox.Show("Vui lòng nhập email.");
            }

        }
        public async Task SendOtpAsync(string email)
        {
            using (var client = new HttpClient())
            {

                var payload = new { Email = email };
                var jsonPayload = JsonConvert.SerializeObject(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                // Gửi POST request
                var response = await client.PostAsync(ConfigurationManager.AppSettings["ServerUrl"] + "User/MailOTP", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("OTP đã được gửi thành công.");
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Lỗi: {error}");
                }
            }
        }

        public async Task VerifyOtpAsync(string otpCode)
        {
            try
            {
                // Tạo HTTP client
                using (var client = new HttpClient())
                {
                    // Tạo payload và nội dung gửi
                    var payload = new { OtpCode = otpCode };
                    var jsonPayload = JsonConvert.SerializeObject(payload);
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    // Gửi yêu cầu POST tới server
                    var serverUrl = ConfigurationManager.AppSettings["ServerUrl"] + "User/VerifyAndSendNewPassword";
                    var response = await client.PutAsync(serverUrl, content);

                    // Xử lý phản hồi từ server
                    if (response.IsSuccessStatusCode)
                    {
                        

                        MessageBox.Show(" Mật khẩu mới đã được gửi qua email của bạn.");
                    }
                    else
                    {
                        // Parse lỗi từ server
                        var error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Lỗi từ server: {error}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                // Lỗi kết nối tới server
                MessageBox.Show($"Lỗi kết nối tới server: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Lỗi không mong muốn khác
                MessageBox.Show($"Lỗi không mong muốn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private async void btn_Verify_Click(object sender, EventArgs e)
        {
            string otpCode = txtOTP.Text.Trim();
            if (!string.IsNullOrEmpty(otpCode))
            {
                await VerifyOtpAsync(otpCode);
            }
            else
            {
                MessageBox.Show("Vui lòng nhập mã OTP.");
            }
        }
    }
}
