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
        public async Task<(bool issuccess, string groupid)> SaveGroupToDatabase(string groupName,int isprivate)
        {
            var DKGroup = new DKGroupDTO
            {
                Groupname = groupName,
                Isprivate= isprivate.ToString()
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
                //MessageBox.Show(message);
                return (true,groupid);
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(errorMessage);
                string message = responseData.message;
                //MessageBox.Show(message);
                return (false,null);
            }
        }
        
        public async Task<(bool issuccess, string[] groupidname)> RequestGroupName(string userid,int isprivate)
        {
            var Groupname = new GroupnameDTO
            {
                UserID = userid,
                Isprivate = isprivate.ToString()
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
                //MessageBox.Show(message);
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
                        //MessageBox.Show(message);
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
        public async Task<string> FindMaLoiMoi(string groupid)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var payload = new FindMaLoiMoiDTO
                    {
                        groupid = groupid
                    };

                    string url = $"{ConfigurationManager.AppSettings["ServerUrl"]}Group/FindMaLoiMoi";

                    var jsonContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                        string maloimoi = responseData.maloimoi;
                        return maloimoi;
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
        public async Task<string> FindGroupID(string maloimoi)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var payload = new FindGroupIDDTO
                    {
                        MaLoiMoi = maloimoi
                    };

                    string url = $"{ConfigurationManager.AppSettings["ServerUrl"]}Group/FindGroupID";

                    var jsonContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                        string groupid = responseData.groupid;
                        return groupid;
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
        public async Task<string> FindGroup(string userid1,string userid2)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var payload = new FindGroupDTO
                    {
                        UserID1 = userid1,
                        UserID2=userid2
                    };

                    string url = $"{ConfigurationManager.AppSettings["ServerUrl"]}Group/FindGroup";

                    var jsonContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, jsonContent);

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
                        if(groupidNamesArray.Length  == 0) { return null; }
                        else return groupidNamesArray[0];
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
        public async Task<bool> RemoveMemberFromGroup(string groupId, string userId)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Tạo payload cho request
                    var payload = new RemoveMemberFromGroupDTO
                    {
                        groupid = groupId,
                        userid = userId
                    };

                    // Tạo request với HTTP method DELETE
                    var request = new HttpRequestMessage(HttpMethod.Delete, $"{ConfigurationManager.AppSettings["ServerUrl"]}Group/RemoveMember")
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json")
                    };
                    // Gửi request
                    var response = await client.SendAsync(request);

                    // Kiểm tra phản hồi từ server
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
                        MessageBox.Show("Failed to remove member: " + errorResponse);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error removing member: " + ex.Message);
                return false;
            }
        }




    }
}
