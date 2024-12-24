using chatclient.DTOs.Channel;
using chatclient.DTOs.Danhmuc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLUSER.Models
{
    internal class DanhMuc
    {
        public async Task<(bool issuccess, string danhmucID)> SaveDanhMucToDatabase(string groupid,string danhmucname)
        {
            var DKDanhMuc = new DKDanhMucDTO
            {
                GroupID = groupid,
                DanhMucname = danhmucname,
            };
            var json = JsonConvert.SerializeObject(DKDanhMuc);
            var content = new StringContent(json, Encoding.Unicode, "application/json");
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(ConfigurationManager.AppSettings["ServerUrl"] + "DanhMuc/DKDanhMuc", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                string message = responseData.message;
                string danhmucID = responseData.danhmucID;
                //MessageBox.Show(message);
                return (true,danhmucID);
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(errorMessage);
                string message = responseData.message;
                MessageBox.Show(message);
                return (false, null);
            }
        }
        public async Task<(bool issuccess, string[] danhmucidname)> RequestDanhMucName(string groupid)
        {
            var DanhMucName = new DanhMucnameDTO
            {
                GroupID=groupid,
            };
            var json = JsonConvert.SerializeObject(DanhMucName);
            var content = new StringContent(json, Encoding.Unicode, "application/json");
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(ConfigurationManager.AppSettings["ServerUrl"] + "DanhMuc/Danhmucname", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                List<string> danhmucidNamesList = new List<string>();
                foreach (var name in responseData.danhmucIDName)
                {
                    danhmucidNamesList.Add((string)name);
                }
                string[] danhmucidNamesArray = danhmucidNamesList.ToArray();
                return (true,danhmucidNamesArray);
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(errorMessage);
                string message= responseData.message;
                MessageBox.Show(message);
                return (false,null);
            }
        }
        public async Task<(bool issuccess, string danhmucname)> RequestonedanhmucName(string danhmucid)
        {
            var OneDanhmucName = new OneDanhmucNameDTO
            {
                danhmucid = danhmucid,
            };
            var json = JsonConvert.SerializeObject(OneDanhmucName);
            var content = new StringContent(json, Encoding.Unicode, "application/json");
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(ConfigurationManager.AppSettings["ServerUrl"] + "DanhMuc/OneDanhmucname", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                string danhmucname1 = responseData.danhmucname;
                return (true, danhmucname1);
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(errorMessage);
                string message = responseData.message;
                MessageBox.Show(message);
                return (false, null);
            }
        }
        public async Task<bool> RenameDanhMuc(string danhmucid, string newdanhmucname)
        {
            var RenameDanhmucRequest = new RenameDanhmucRequestDTO
            {
                newdanhmucname = newdanhmucname,
                danhmucid = danhmucid,
            };
            var json = JsonConvert.SerializeObject(RenameDanhmucRequest);
            var content = new StringContent(json, Encoding.Unicode, "application/json");
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(ConfigurationManager.AppSettings["ServerUrl"] + "DanhMuc/RenameDanhmuc", content);
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
                var errorMessage = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(errorMessage);
                string message = responseData.message;
                MessageBox.Show(message);
                return false;
            }
        }
        public async Task<bool> Deletedanhmuc(string danhmucid)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var payload = new DeleteDanhmucRequestDTO
                    {
                        danhmucid = danhmucid
                    };

                    var request = new HttpRequestMessage(HttpMethod.Delete, $"{ConfigurationManager.AppSettings["ServerUrl"]}DanhMuc/DeleteDanhmuc")
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json")
                    };

                    var response = await client.SendAsync(request);

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
                        MessageBox.Show("Failed to delete group: " + errorResponse);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting group: " + ex.Message);
                return false;
            }
        }
    }
}
