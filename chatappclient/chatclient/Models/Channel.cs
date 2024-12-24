using chatclient.DTOs.Channel;
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
    internal class Channel
    {
        public async Task<(bool issuccess,string channelID)> SaveKenhToDatabase(string groupID, string channelname,bool ischat,string danhmucid)
        {

            if (danhmucid == null) danhmucid = "null";
            var DKChannel = new DKChannelDTO
            {
                GroupID = groupID,
                Channelname = channelname,
                ischat=ischat,
                danhmucID = danhmucid
                
            };
            var json = JsonConvert.SerializeObject(DKChannel);
            var content = new StringContent(json, Encoding.Unicode, "application/json");
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(ConfigurationManager.AppSettings["ServerUrl"] + "Channel/DKChannel", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                string message = responseData.message;
                string channelID = responseData.channelID;
                //MessageBox.Show(message);
                return (true,channelID);
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
        public async Task<(bool issuccess, string[] channelidname)> RequestChannelName(string groupid)
        {
            var channelname = new ChannelnameDTO
            {
                GroupID=groupid,
            };
            var json = JsonConvert.SerializeObject(channelname);
            var content = new StringContent(json, Encoding.Unicode, "application/json");
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(ConfigurationManager.AppSettings["ServerUrl"] + "Channel/ChannelName", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                List<string> channelidNamesList = new List<string>();
                foreach (var name in responseData.channelIDName)
                {
                    channelidNamesList.Add((string)name);
                }
                string[] channelidNamesArray = channelidNamesList.ToArray();
                return (true,channelidNamesArray);
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
        public async Task<(bool issuccess, string channelname)> RequestoneChannelName(string channelid)
        {
            var onechannelname = new OneChannelNameDTO
            {
               channelid=channelid,
            };
            var json = JsonConvert.SerializeObject(onechannelname);
            var content = new StringContent(json, Encoding.Unicode, "application/json");
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(ConfigurationManager.AppSettings["ServerUrl"] + "Channel/OneChannelname", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                string channelname1 = responseData.channelname;
                return (true, channelname1);
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
        public async Task<bool> RenameChannel(string channelid,string newchannelname)
        {
            var RenameChannelRequest = new RenameChannelRequestDTO
            {
                newchannelName=newchannelname,
                channelId = channelid
            };
            var json = JsonConvert.SerializeObject(RenameChannelRequest);
            var content = new StringContent(json, Encoding.Unicode, "application/json");
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(ConfigurationManager.AppSettings["ServerUrl"] + "Channel/RenameChannel", content);
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
        public async Task<bool> DeleteChannel(string channelid)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var payload = new DeleteChannelRequestDTO
                    {
                        ChannelId = channelid
                    };

                    var request = new HttpRequestMessage(HttpMethod.Delete, $"{ConfigurationManager.AppSettings["ServerUrl"]}Channel/DeleteChannel")
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
