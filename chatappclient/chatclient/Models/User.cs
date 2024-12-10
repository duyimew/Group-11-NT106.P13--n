using chatclient.DTOs.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLUSER.Models
{

    internal class User
    {
        

        public string HashPassword(string password)
        {
            try
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                    StringBuilder builder = new StringBuilder();
                    foreach (byte b in bytes)
                    {
                        builder.Append(b.ToString("x2"));
                    }
                    return builder.ToString();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi khi hash mật khẩu");
                return null;
            }
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        public async Task<string> finduserid(string displayname)
        {
            var Username = new InforuserDTO
            {
                displayname = displayname,
            };
            var json = JsonConvert.SerializeObject(Username);
            var content = new StringContent(json, Encoding.Unicode, "application/json");
            HttpClient client = new HttpClient();
            
            var response = await client.PostAsync(ConfigurationManager.AppSettings["ServerUrl"] + "User/FindUserID", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                string userid = responseData.userid;
                return userid;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(errorMessage);
                string message = responseData.message;
                return null;
            }
        }
        public async Task<string> FindDisplayname(string userid)
        {
            var Username = new InforuserDTO
            {
                UserId=userid
            };
            var json = JsonConvert.SerializeObject(Username);
            var content = new StringContent(json, Encoding.Unicode, "application/json");
            HttpClient client = new HttpClient();

            var response = await client.PostAsync(ConfigurationManager.AppSettings["ServerUrl"] + "User/FindDisplayname", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                string displayname = responseData.displayname;
                return displayname;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(errorMessage);
                string message = responseData.message;
                return null;
            }
        }
        public async Task<bool> RenameDisplayname(string newdisplayname, string userid)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var payload = new InforuserDTO
                    {
                        newdisplayname = newdisplayname,
                        UserId = userid
                    };

                    string url = $"{ConfigurationManager.AppSettings["ServerUrl"]}User/RenameDisplayname";

                    var jsonContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                        string message = responseData.message;
                        MessageBox.Show(message);
                        return true;
                    }
                    else
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Failed to rename group: " + errorResponse);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error renaming group: " + ex.Message);
                return false;
            }
        }
    }
}
