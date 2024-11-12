using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace chatserver.HUB
{
    public class VideoCallHub : Hub
    {
        // Danh sách các userId của người tham gia cho từng callId
        private static readonly Dictionary<string, List<string>> CallParticipants = new();

        public async Task JoinCall(string callId, string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, callId);

            // Khởi tạo danh sách người tham gia nếu callId chưa tồn tại
            if (!CallParticipants.ContainsKey(callId))
            {
                CallParticipants[callId] = new List<string>();
            }

            var participants = CallParticipants[callId];

            // Gửi danh sách người tham gia hiện tại cho người vừa tham gia
            await Clients.Caller.SendAsync("ExistingParticipants", participants);

            // Thêm userId vào danh sách và gửi thông báo cho các thành viên khác
            if (!participants.Contains(userId))
            {
                participants.Add(userId);
                await Clients.OthersInGroup(callId).SendAsync("UserJoined", userId);
            }
        }

        public async Task LeaveCall(string callId, string userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, callId);

            // Loại bỏ userId khỏi danh sách và kiểm tra nếu nhóm trống
            if (CallParticipants.ContainsKey(callId))
            {
                CallParticipants[callId].Remove(userId);
                if (CallParticipants[callId].Count == 0)
                {
                    CallParticipants.Remove(callId);
                }
            }

            // Gửi thông báo cho nhóm về việc người dùng rời khỏi cuộc gọi
            await Clients.Group(callId).SendAsync("UserLeft", userId);
        }

        public async Task SendVideoFrame(string callId, byte[] frameData, string userId)
        {
            // Gửi video frame chỉ cho các thành viên khác trong nhóm
            await Clients.OthersInGroup(callId).SendAsync("ReceiveVideoFrame", frameData, userId);
        }

        public async Task SendAudio(string callId, byte[] audioData)
        {
            // Gửi audio chỉ cho các thành viên khác trong nhóm
            await Clients.OthersInGroup(callId).SendAsync("ReceiveAudio", audioData);
        }
    }
}
