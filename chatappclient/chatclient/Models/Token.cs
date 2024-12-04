using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using QLUSER.DTOs;
namespace QLUSER.Models
{
    internal class Token
    {
        Find find = new Find();
        public async Task GenerateToken(string username)
        {
            var token = new TokenDTO
            {
                Username = username,
            };
            var json = JsonConvert.SerializeObject(token);
            var content = new StringContent(json, Encoding.Unicode, "application/json");
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(ConfigurationManager.AppSettings["ServerUrl"] + "Token/GenerateToken", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                string token1 = responseData.token;
                string path = find.GetProjectRootDirectory();
                FileStream fs = new FileStream(path + "\\token.txt", FileMode.Create);
                fs.Close();
                string filepath = find.find("token.txt");
                File.WriteAllText(filepath, username + "|" + token1);
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                string Message = responseData.message;
                MessageBox.Show(Message);
            }

        }
        public async Task CheckTokenAndShowForm()
        {
            try
            {
                string filepath = find.find("token.txt");
                if (File.Exists(filepath))
                {
                    var TDDangNhap = new TDDangNhapDTO
                    {
                        Token = File.ReadAllLines(filepath)[0]
                    };
                    var json = JsonConvert.SerializeObject(TDDangNhap);
                    var content = new StringContent(json, Encoding.Unicode, "application/json");
                    HttpClient client = new HttpClient();
                    var response = await client.PostAsync(ConfigurationManager.AppSettings["ServerUrl"] + "TDDangNhap/TDDangNhap", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                        Dangnhap dangnhap = new Dangnhap();
                        GiaoDien giaoDien = new GiaoDien(responseData.username,responseData.userid, dangnhap);
                        Application.Run(giaoDien);
                        dangnhap.Show();
                        Environment.Exit(0);
                    }
                    else
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                        MessageBox.Show(responseData.message);
                        Dangnhap dangnhap = new Dangnhap();
                        Application.Run(dangnhap);
                        Environment.Exit(0);
                    }
                }
                else
                {
                    Dangnhap dangnhap = new Dangnhap();
                    Application.Run(dangnhap);
                    Environment.Exit(0);
                }
            }
            catch (Exception)
            {
                Dangnhap dangnhap = new Dangnhap();
                Application.Run(dangnhap);
                Environment.Exit(0);
            }
        }
    }
}
