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


        public async Task SendAvatarDisplay(bool update)
        {
            await Clients.Others.SendAsync("ReceiveAvatarDisplay", update);
        }
        public async Task SendGroupDislay(bool update)
        {
            await Clients.Others.SendAsync("ReceiveGroupDislay", update);
        }
        public async Task SendGroupPrivateDislay(bool update)
        {
            await Clients.Others.SendAsync("ReceiveGroupPrivateDislay", update);
        }
        public async Task Senddpname(bool update)
        {
            await Clients.Others.SendAsync("Receivedpname", update);
        }
        public async Task SendAvatarGroupUpdated(bool update)
        {
            await Clients.Others.SendAsync("ReceiveAvatarGroupUpdated", update);
        }
        public async Task SendDeleteuser(bool update)
        {
            await Clients.Others.SendAsync("ReceiveDeleteuser", update);
        }
        public async Task SendUpdateFriend(bool update)
        {
            await Clients.Others.SendAsync("ReceiveUpdateFriend", update);
        }
        public async Task SendUpdateFriendRequest(bool update)
        {
            await Clients.Others.SendAsync("ReceiveUpdateFriendRequest", update);
        }
        public async Task SendUpdategdpname(bool update)
        {
            await Clients.Others.SendAsync("ReceiveUpdategdpname", update);
        }
        public async Task SendUpdateGroupMember(bool update)
        {
            await Clients.Others.SendAsync("ReceiveUpdateGroupMember", update);
        }

        public async Task SendUpdateChannel(bool update)
        {
            await Clients.Others.SendAsync("ReceiveUpdateChannel", update);
        }
        public async Task SendUpdateChannelname(bool update)
        {
            await Clients.Others.SendAsync("ReceiveUpdateChannelname", update);
        }
        public async Task SendUpdateDanhMuc(bool update)
        {
            await Clients.Others.SendAsync("ReceiveUpdateDanhMuc", update);
        }
        public async Task SendUpdatedanhmucname(bool update)
        {
            await Clients.Others.SendAsync("ReceiveUpdatedanhmucname", update);
        }
        public async Task SendUpdateGroupMemberPrivate(bool update)
        {
            await Clients.Others.SendAsync("ReceiveUpdateGroupMemberPrivate", update);
        }
        public async Task SendUpdateGroupname(bool update)
        {
            await Clients.Others.SendAsync("ReceiveUpdateGroupname", update);
        }

        public async Task SendUpdateMessage(bool update)
        {
            await Clients.Others.SendAsync("ReceiveUpdateMessage", update);
        }
        public async Task SendUpdateMessageText(bool update)
        {
            await Clients.Others.SendAsync("ReceiveUpdateMessageText", update);
        }
        public async Task SendUpdateRole(bool update)
        {
            await Clients.Others.SendAsync("ReceiveUpdateRole", update);
        }
    }
}
