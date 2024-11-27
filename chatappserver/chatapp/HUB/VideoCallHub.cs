using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace chatserver.HUB
{
    public class VideoCallHub : Hub
    {
        private static readonly Dictionary<string, List<string>> CallParticipants = new();


        public async Task JoinCall(string callId, string userId)
        {
           
                    await Groups.AddToGroupAsync(Context.ConnectionId, callId);
                    if (!CallParticipants.ContainsKey(callId))
                    {
                        CallParticipants[callId] = new List<string>();
                    }

                    var participants = CallParticipants[callId];
                    await Clients.Caller.SendAsync("ExistingParticipants", participants);
                    if (!participants.Contains(userId))
                    {
                        participants.Add(userId);
                    }
                    await Clients.OthersInGroup(callId).SendAsync("UserJoined", userId);

        }

        public async Task LeaveCall(string callId, string userId)
        {

                    if (CallParticipants.ContainsKey(callId))
                    {
                        CallParticipants[callId].Remove(userId);
                        if (CallParticipants[callId].Count == 0)
                        {
                            CallParticipants.Remove(callId);
                        }
                    }
                    await Clients.OthersInGroup(callId).SendAsync("UserLeft", userId);

                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, callId);
        }

        public async Task SendVideoFrame(string callId, byte[] frameData, string userId)
        {
            
                await Clients.OthersInGroup(callId).SendAsync("ReceiveVideoFrame", frameData, userId);
               
        }

        public async Task SendAudio(string callId, byte[] audioData)
        {
           
                await Clients.OthersInGroup(callId).SendAsync("ReceiveAudio", audioData);
       
        }
    }
}