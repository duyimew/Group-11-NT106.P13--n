using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace chatserver.HUB
{
    public class VideoCallHub : Hub
    {
        private static readonly Dictionary<string, List<string>> CallParticipants = new();


        public async Task JoinCall(string callId, string groupdisplayname)
        {
           
                    await Groups.AddToGroupAsync(Context.ConnectionId, callId);
                    if (!CallParticipants.ContainsKey(callId))
                    {
                        CallParticipants[callId] = new List<string>();
                    }

                    var participants = CallParticipants[callId];
                    await Clients.Caller.SendAsync("ExistingParticipants", participants);
                    if (!participants.Contains(groupdisplayname))
                    {
                        participants.Add(groupdisplayname);
                    }
                    await Clients.OthersInGroup(callId).SendAsync("UserJoined", groupdisplayname);

        }

        public async Task LeaveCall(string callId, string groupdisplayname)
        {

                    if (CallParticipants.ContainsKey(callId))
                    {
                        CallParticipants[callId].Remove(groupdisplayname);
                        if (CallParticipants[callId].Count == 0)
                        {
                            CallParticipants.Remove(callId);
                        }
                    }
                    await Clients.OthersInGroup(callId).SendAsync("UserLeft", groupdisplayname);

                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, callId);
        }

        public async Task SendVideoFrame(string callId, byte[] frameData, string groupdisplayname)
        {
            
                await Clients.OthersInGroup(callId).SendAsync("ReceiveVideoFrame", frameData, groupdisplayname);
               
        }

        public async Task SendAudio(string callId, byte[] audioData)
        {
           
                await Clients.OthersInGroup(callId).SendAsync("ReceiveAudio", audioData);
       
        }
    }
}