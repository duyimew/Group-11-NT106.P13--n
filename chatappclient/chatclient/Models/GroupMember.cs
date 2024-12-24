using chatclient.DTOs.GroupMember;
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
using chatclient.DTOs.Danhmuc;
using Newtonsoft.Json.Linq;

namespace QLUSER.Models
{
    internal class GroupMember
    {

        public async Task<bool> AddMembersToGroup(string userid, string groupid, string displayname, string role)
        {
            var AddUser = new AddUserDTO
            {
                UserID = userid,
                GroupID = groupid,
                displayname = displayname,
                role = role
            };
            var json = JsonConvert.SerializeObject(AddUser);
            var content = new StringContent(json, Encoding.Unicode, "application/json");
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(ConfigurationManager.AppSettings["ServerUrl"] + "GroupMember/AddUser", content);
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
                var errorMessage = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(errorMessage);
                string message = responseData.message;
                //MessageBox.Show(message);
                return false;
            }
        }
        public async Task<string[]> FindUserRoleNameId(string groupid)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var payload = new UserRoleDTO
                    {
                        GroupId = groupid,
                    };

                    string url = $"{ConfigurationManager.AppSettings["ServerUrl"]}GroupMember/UserRole";

                    var jsonContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                        List<string> UserRoleidNamesList = new List<string>();
                        if (responseData.userRoleNameId == null) return null;
                        foreach (var name in responseData.userRoleNameId)
                        {
                            UserRoleidNamesList.Add((string)name);
                        }
                        string[] UserRoleidNamesArray = UserRoleidNamesList.ToArray();
                        return UserRoleidNamesArray;

                    }
                    else
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Failed to retrieve group members: " + errorResponse);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving group members: " + ex.Message);
                return null;
            }
        }
        public async Task<string> FindOneUserRole(string groupid, string userid)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var payload = new OneUserRoleDTO
                    {
                        groupid = groupid,
                        UserId = userid
                    };

                    string url = $"{ConfigurationManager.AppSettings["ServerUrl"]}GroupMember/OneUserRole";

                    var jsonContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                        string UserRole = responseData.role;
                        return UserRole;

                    }
                    else
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Failed to retrieve group members: " + errorResponse);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving group members: " + ex.Message);
                return null;
            }
        }
        public async Task<string> FindGroupDisplayname(string userid, string groupid)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var payload = new FindGroupDisplaynameDTO
                    {
                        UserId = userid,
                        groupid = groupid
                    };

                    string url = $"{ConfigurationManager.AppSettings["ServerUrl"]}GroupMember/FindGroupDisplayname";

                    var jsonContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                        string groupdisplayname = responseData.groupdisplayname;
                        return groupdisplayname;
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

        public async Task<JArray> FindGroupMembers(string groupId)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string query = $"groupId={Uri.EscapeDataString(groupId)}";

                    string url = $"{ConfigurationManager.AppSettings["ServerUrl"]}GroupMember/ListUser?{query}";

                    var response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                        var members = responseData.users;
                        return members;
                    }
                    else
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Failed to get group members: " + errorResponse);
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

        public async Task<string> FindGroupDisplayID(string groupid,string gdpname)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var payload = new FindGroupDisplayIDDTO
                    {
                        groupid = groupid,
                        groupdisplayname = gdpname,
                    };

                    string url = $"{ConfigurationManager.AppSettings["ServerUrl"]}GroupMember/FindGroupDisplayID";

                    var jsonContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                        string groupdisplayid = responseData.groupdisplayid;
                        return groupdisplayid;
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
        public async Task<string> FindJointime(string userid,string groupid)
        {
            var Username = new FindJointimeDTO
            {
                UserId = userid,
                groupid = groupid
            };
            var json = JsonConvert.SerializeObject(Username);
            var content = new StringContent(json, Encoding.Unicode, "application/json");
            HttpClient client = new HttpClient();

            var response = await client.PostAsync(ConfigurationManager.AppSettings["ServerUrl"] + "GroupMember/FindJointime", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                DateTime time = responseData.time;
                DateTime now = DateTime.Now;
                TimeSpan timeDifference = now - time;
                int totalDays = (int)timeDifference.TotalDays;

                if (totalDays < 30)
                {
                    return $"{totalDays} ngày trước";
                }
                else if (totalDays < 365)
                {
                    int months = totalDays / 30;
                    return $"{months} tháng trước";
                }
                else
                {
                    int years = totalDays / 365;
                    return $"{years} năm trước";
                }
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(errorMessage);
                string message = responseData.message;
                return null;
            }
        }
        public async Task<string> FindDateJointime(string userid, string groupid)
        {
            var Username = new FindJointimeDTO
            {
                UserId = userid,
                groupid = groupid
            };
            var json = JsonConvert.SerializeObject(Username);
            var content = new StringContent(json, Encoding.Unicode, "application/json");
            HttpClient client = new HttpClient();

            var response = await client.PostAsync(ConfigurationManager.AppSettings["ServerUrl"] + "GroupMember/FindJointime", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                DateTime time = responseData.time;

                // Trả về ngày theo định dạng dd/MM/yyyy
                return time.ToString("dd/MM/yyyy");
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(errorMessage);
                string message = responseData.message;
                return null; // hoặc có thể return message nếu bạn muốn trả thông báo lỗi
            }
        }

        public async Task<bool> RenameGroupDisplayname(string newgroupdisplayname, string groupid, string userid)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var payload = new RenamegroupDisplaynameDTO
                    {
                        newgroupdisplayname = newgroupdisplayname,
                        groupid = groupid,
                        UserId = userid
                    };

                    string url = $"{ConfigurationManager.AppSettings["ServerUrl"]}GroupMember/RenamegroupDisplayname";

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


        public async Task<bool> DeleteGroupMember(string userid, string groupid)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var payload = new DeleteGroupMemberRequestDTO
                    {
                        userid = userid,
                        groupid = groupid
                    };

                    var request = new HttpRequestMessage(HttpMethod.Delete, $"{ConfigurationManager.AppSettings["ServerUrl"]}GroupMember/DeleteGroupMember")
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


        public async Task<bool> Goborole(string groupid,string userid)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var payload = new RenameRoleDTO
                    {
                        newrole = "Member",
                        groupid = groupid,
                        UserId=userid,
                    };

                    string url = $"{ConfigurationManager.AppSettings["ServerUrl"]}GroupMember/RenameRole";

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
        public async Task<bool> themrole(string groupid, string userid,string role)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var payload = new RenameRoleDTO
                    {
                        newrole = role,
                        groupid = groupid,
                        UserId = userid,
                    };

                    string url = $"{ConfigurationManager.AppSettings["ServerUrl"]}GroupMember/RenameRole";

                    var jsonContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                        string message = responseData.message;
                        MessageBox.Show(message);return true;
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
