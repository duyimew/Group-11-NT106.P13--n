using chatapp.Data;
using chatapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using chatapp.DataAccess;
using chatserver.DTOs.Friends;
namespace chatapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FriendController : ControllerBase
    {
        private readonly ChatAppContext _context;
        private readonly Friend _friend;

        public FriendController(ChatAppContext context, Friend friend)
        {
            _context = context;
            _friend = friend;
        }

        [HttpGet("{userid}")]
        public async Task<IActionResult> List(string userid)
        {
            string[] result = await _friend.FriendListAsync(userid);
            if (result[0] == "1")
            {
                return Ok(new { message = result.Skip(1) });
            }
            else if (result[0] =="0")
            {
                return Ok();
            }
            else
            {
                return BadRequest(new { message = result[0] });
            }
        }

        [HttpGet("Request/{userid}")]
        public async Task<IActionResult> ListSentRequest(string userid)
        {
            var result = await _friend.ListSentRequest(userid);
            if (result.listsender[0] == "1" || result.listreceiver[0] =="1")
            {
                return Ok(new { sender=result.listsender.Skip(1),receiver = result.listreceiver.Skip(1) });
            }
            else
            {
                return BadRequest(new { messagesender = result.listsender[0], messagereceiver = result.listreceiver[0] });
            }
        }

        [HttpPost("Request")]
        public async Task<IActionResult> AddFriendRequest([FromBody] SendFriendRequestDTO request)
        {
            string[] result = await _friend.SendFriendRequest(request);
            if (result[0] == "1")
            {
                return Ok(new { message = "Gửi kết bạn thành công!".ToString() });
            }
            else
            {
                return BadRequest(new { message = result[0] });
            }
        }

        [HttpPost("Respond")]
        public async Task<IActionResult> RespondFriendRequest([FromBody] RespondFriendRequestDTO request)
        {
            string[] result = await _friend.RespondFriendRequest(request);
            if (result[0] == "1")
            {
                return Ok(new { message = result[1] });
            }
            else
            {
                return BadRequest(new { message = result[0] });
            }
        }

        [HttpPost("DeleteFriend")]
        public async Task<IActionResult> DeleteFriend([FromBody] DeleteFriendDTO request)
        {
            if (request == null || request.UserId_1 <= 0 || request.UserId_2 <= 0)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });
            }

            try
            {
                // Tìm quan hệ bạn bè trong database
                var friendship = await _context.Friends
                    .FirstOrDefaultAsync(f => (f.UserId_1 == request.UserId_1 && f.UserId_2 == request.UserId_2) ||
                                              (f.UserId_1 == request.UserId_2 && f.UserId_2 == request.UserId_1));

                if (friendship == null)
                {
                    return NotFound(new { message = "Quan hệ bạn bè không tồn tại." });
                }

                // Xóa quan hệ bạn bè
                _context.Friends.Remove(friendship);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Quan hệ bạn bè đã được xóa thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Đã xảy ra lỗi: {ex.Message}" });
            }
        }


    }
}
