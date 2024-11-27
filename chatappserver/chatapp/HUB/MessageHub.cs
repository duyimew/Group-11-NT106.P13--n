using chatapp.DataAccess;
using chatapp.DTOs;
using chatapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace chatserver.HUB
{
    public class MessageHub : Hub
    {
        private static readonly Dictionary<string, List<string>> CallParticipants = new();


        public async Task JoinGroup(string tennhom,string tenkenh, string username)
        {
            string groupid=tennhom+"|"+tenkenh;
            await Groups.AddToGroupAsync(Context.ConnectionId, groupid);
            if (!CallParticipants.ContainsKey(groupid))
            {
                CallParticipants[groupid] = new List<string>();
            }
            var participants = CallParticipants[groupid];
            if (!participants.Contains(username))
            {
                participants.Add(username);
            }
        }

        public async Task LeaveGroup(string tennhom, string tenkenh, string username)
        {
            string groupid = tennhom + "|" + tenkenh;
            if (CallParticipants.ContainsKey(groupid))
            {
                CallParticipants[groupid].Remove(username);
                if (CallParticipants[groupid].Count == 0)
                {
                    CallParticipants.Remove(groupid);
                }
            }
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupid);
        }

        public async Task SendMessage(string username, string groupname, string channelname, string message, string[] filenames)
        {
            string groupid = groupname + "|" + channelname;
            await Clients.Group(groupid).SendAsync("ReceiveMessage", message, username, filenames);
        }
    }
}
