using QLUSER.DTOs;
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
        
    }
}
