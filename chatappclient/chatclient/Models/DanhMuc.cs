using chatapp.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLUSER.Models
{
    internal class DanhMuc
    {
        public async Task<bool> SaveDanhMucToDatabase(string groupName,string danhmucname)
        {
            var DKDanhMuc = new DKDanhMucDTO
            {
                Groupname = groupName,
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
        public async Task<string[]> RequestDanhMucName(string groupname)
        {
            var DanhMucName = new DanhMucnameDTO
            {
                Groupname=groupname,
            };
            var json = JsonConvert.SerializeObject(DanhMucName);
            var content = new StringContent(json, Encoding.Unicode, "application/json");
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(ConfigurationManager.AppSettings["ServerUrl"] + "DanhMuc/Danhmucname", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                List<string> danhmucNamesList = new List<string>();
                foreach (var name in responseData.danhmucName)
                {
                    danhmucNamesList.Add((string)name);
                }
                string[] danhmucNamesArray = danhmucNamesList.ToArray();
                if (danhmucNamesArray[0] == "0") return null;
                return danhmucNamesArray;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(errorMessage);
                string message= responseData.message;
                MessageBox.Show(message);
                return null;
            }
        }
    }
}
