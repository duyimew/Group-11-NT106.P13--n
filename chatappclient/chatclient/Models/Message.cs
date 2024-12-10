using chatclient.DTOs.Message;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading.Channels;
using chatclient.DTOs.Group;

namespace QLUSER.Models
{
    internal class Message
    {
        public async Task<(bool issuccess, string messageid)> SendMessage(string ChannelID, string UserID, string message)
        {
            var sendmess = new SendmessDTO
            {
                Message =message,
                ChannelID = ChannelID,
                UserID= UserID,
            };
            var json = JsonConvert.SerializeObject(sendmess);
            var content = new StringContent(json, Encoding.Unicode, "application/json");
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(ConfigurationManager.AppSettings["ServerUrl"] + "Message/SendMessage", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                string Message = responseData.message;
                string messageid = responseData.messageid;
                return (true, messageid);
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                string Message = responseData.message;
                return (false, null);
            }
        }
        public async Task<(bool issuccess, string[] messagetext)> ReceiveMessage(string channelid)
        {
            var receivemess = new ReceivemessDTO
            {
                ChannelID = channelid
            };
            var json = JsonConvert.SerializeObject(receivemess);
            var content = new StringContent(json, Encoding.Unicode, "application/json");
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(ConfigurationManager.AppSettings["ServerUrl"] + "Message/ReceiveMessage", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                List<string> MessagetextList = new List<string>();
                foreach (var text in responseData.messagetext)
                {
                    MessagetextList.Add((string)text);
                }
                string[] MessagetextArray = MessagetextList.ToArray();
                return (true,MessagetextArray);
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                string Message = responseData.message;
                MessageBox.Show(Message);
                return (false, null);
            }
        }
        public async Task<bool> EditMessage(string messagetext, string messageid)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var payload = new EditMessageRequestDTO
                    {
                        messageid = messageid,
                        newmessage= messagetext
                    };

                    string url = $"{ConfigurationManager.AppSettings["ServerUrl"]}Message/EditMessage";

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
        public async Task<string> Onemessage(string messageid)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var payload = new OneMessageDTO
                    {
                        messageid=messageid,
                    };

                    string url = $"{ConfigurationManager.AppSettings["ServerUrl"]}Message/OneMessage";

                    var jsonContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                        string messagetext = responseData.messagetext;
                        return messagetext;
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
        public async Task<bool> DeleteMessage(string messageid)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var payload = new DeleteMessageRequestDTO
                    {
                        messageid=messageid,
                    };

                    string url = $"{ConfigurationManager.AppSettings["ServerUrl"]}Message/DeleteMessage";

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
