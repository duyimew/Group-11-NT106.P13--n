using chatapp.DTOs;
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

namespace QLUSER.Models
{
    internal class Message
    {
        public async Task SendMessage(string groupName, string channelname,string username,string message)
        {
            var sendmess = new SendmessDTO
            {
                Groupname = groupName,
                Message =message,
                Channelname = channelname,
                Username=username,
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
                MessageBox.Show(Message);
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                string Message = responseData.message;
                MessageBox.Show(Message);
            }
        }
        public async Task<string[]> ReceiveMessage(string groupname, string channelname)
        {
            var receivemess = new ReceivemessDTO
            {
                Groupname = groupname,
                Channelname = channelname
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
                if (MessagetextArray[0] == "0") return null;
                return MessagetextArray;
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                string Message = responseData.message;
                MessageBox.Show(Message);
                return null;
            }
        }
        
    }
}
