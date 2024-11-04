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
    internal class Channel
    {
        public async Task SaveKenhToDatabase(string groupName,string channelname)
        {
            var DKChannel = new DKChannelDTO
            {
                Groupname = groupName,
                Channelname = channelname
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
                MessageBox.Show(message);
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(errorMessage);
                string message = responseData.message;
                MessageBox.Show(message);
            }
        }
        public async Task<string[]> RequestChannelName(string groupname)
        {
            var channelname = new ChannelnameDTO
            {
                Groupname=groupname,
            };
            var json = JsonConvert.SerializeObject(channelname);
            var content = new StringContent(json, Encoding.Unicode, "application/json");
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(ConfigurationManager.AppSettings["ServerUrl"] + "Channel/ChannelName", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                List<string> channelNamesList = new List<string>();
                foreach (var name in responseData.channelName)
                {
                    channelNamesList.Add((string)name);
                }
                string[] channelNamesArray = channelNamesList.ToArray();
                if (channelNamesArray[0] == "0") return null;
                return channelNamesArray;
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
