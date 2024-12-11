using chatclient.DTOs.Group;
using chatclient.DTOs.User;
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
        public async Task<(bool issuccess, string groupid)> SaveGroupToDatabase(string groupName)
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
                string groupid = responseData.groupid;
                MessageBox.Show(message);
                return (true,groupid);
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(errorMessage);
                string message = responseData.message;
                MessageBox.Show(message);
                return (false,null);
            }
        }
        
        public async Task<(bool issuccess, string[] groupidname)> RequestGroupName(string userid)
        {
            var Groupname = new GroupnameDTO
            {
                UserID = userid,
            };
            var json = JsonConvert.SerializeObject(Groupname);
            var content = new StringContent(json, Encoding.Unicode, "application/json");
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(ConfigurationManager.AppSettings["ServerUrl"] + "Group/GroupName", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                List<string> groupidNamesList = new List<string>();
                foreach (var name in responseData.groupidname)
                {
                    groupidNamesList.Add((string)name);
                }
                string[] groupidNamesArray = groupidNamesList.ToArray();
                return (true,groupidNamesArray);
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(errorMessage);
                string message = responseData.message;
                MessageBox.Show(message);
                return (false,null);
            }
        }
        public async Task<bool> RenameGroup(string newgroupname, string groupid)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var payload = new RenameGroupRequestDTO
                    {
                        NewGroupName = newgroupname,
                        GroupId = groupid
                    };

                    string url = $"{ConfigurationManager.AppSettings["ServerUrl"]}Group/RenameGroup";

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
        public async Task<string> Groupname(string groupid)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var payload = new OneGroupNameDTO
                    {
                        groupId = groupid
                    };

                    string url = $"{ConfigurationManager.AppSettings["ServerUrl"]}Group/OneGroupname";

                    var jsonContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                        string groupname = responseData.groupname;
                        return groupname;
                    }
                    else
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Failed to rename group: " + errorResponse);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error renaming group: " + ex.Message);
                return null;
            }
        }




        public async Task<bool> DeleteGroup(string groupid)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var payload = new DeleteGroupRequestDTO
                    {
                        groupid = groupid
                    };

                    var request = new HttpRequestMessage(HttpMethod.Delete, $"{ConfigurationManager.AppSettings["ServerUrl"]}Group/DeleteGroup")
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
