using chatapp.DataAccess;
using chatapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;
namespace chatserver.HUB
{
    public class MessageHub : Hub
    {
        private static readonly Dictionary<string, List<string>> CallParticipants = new();


        public async Task JoinGroup(string channelid, string groupdisplayname)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, channelid);
            if (!CallParticipants.ContainsKey(channelid))
            {
                CallParticipants[channelid] = new List<string>();
            }
            var participants = CallParticipants[channelid];
            if (!participants.Contains(groupdisplayname))
            {
                participants.Add(groupdisplayname);
            }
        }

        public async Task LeaveGroup(string channelid, string groupdisplayname)
        {
            if (CallParticipants.ContainsKey(channelid))
            {
                CallParticipants[channelid].Remove(groupdisplayname);
                if (CallParticipants[channelid].Count == 0)
                {
                    CallParticipants.Remove(channelid);
                }
            }
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, channelid);
        }

        public async Task SendMessage(string messageid,string groupdisplayname, string channelid, string message, string[] filenames)
        {
            await Clients.Group(channelid).SendAsync("ReceiveMessage", messageid,message, groupdisplayname, filenames);
        }


        public async Task SendAvataUpdate(string url, string channelid)
        {
            await Clients.Others.SendAsync("ReceiveAvataUpdate", url);
        }
    }
}
