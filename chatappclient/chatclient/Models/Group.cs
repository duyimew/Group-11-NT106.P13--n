using chatapp.DTOs;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLUSER.Models
{
    internal class Group
    {
        public async Task<bool> SaveGroupToDatabase(string groupName)
        {
            var DKGroup = new DKGroupDTO
            {
                Groupname = groupName,
            };
            var json = JsonConvert.SerializeObject(DKGroup);
            var content = new StringContent(json, Encoding.Unicode, "application/json");
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(ConfigurationManager.AppSettings["ServerUrl"] + "Group/DKGroup", content);
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
        public async Task<bool> AddMembersToGroup(string username, string groupname)
        {
            var AddUser = new AddUserDTO
            {
                Username= username,
                Groupname = groupname
            };
            var json = JsonConvert.SerializeObject(AddUser);
            var content = new StringContent(json, Encoding.Unicode, "application/json");
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(ConfigurationManager.AppSettings["ServerUrl"] + "Group/AddUser", content);
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
        public async Task<string[]> RequestGroupName(string username)
        {
            var Groupname = new GroupnameDTO
            {
                Username = username,
            };
            var json = JsonConvert.SerializeObject(Groupname);
            var content = new StringContent(json, Encoding.Unicode, "application/json");
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(ConfigurationManager.AppSettings["ServerUrl"] + "Group/GroupName", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                List<string> groupNamesList = new List<string>();
                foreach (var name in responseData.groupname)
                {
                    groupNamesList.Add((string)name);
                }
                string[] groupNamesArray = groupNamesList.ToArray();
                if (groupNamesArray[0] == "0") return null;
                return groupNamesArray;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(errorMessage);
                string message = responseData.message;
                MessageBox.Show(message);
                return null;
            }
        }
    }
}
