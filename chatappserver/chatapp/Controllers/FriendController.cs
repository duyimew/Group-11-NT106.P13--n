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
            else
            {
                return BadRequest(new { message = result[0] });
            }
        }

        [HttpGet("Request/{userid}")]
        public async Task<IActionResult> ListSentRequest(string userid)
        {
            string[] result = await _friend.ListSentRequest(userid);
            if (result[0] == "1")
            {
                return Ok(new { message = result.Skip(1) });
            }
            else
            {
                return BadRequest(new { message = result[0] });
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

        [HttpPost("DeleteRequest")]
        public async Task<IActionResult> DeleteRequest([FromBody] DeleteFriendRequestDTO request)
        {
            if (request == null || request.SenderId <= 0 || request.ReceiverId <= 0)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });
            }

            try
            {
                var friendRequest = await _context.FriendRequests
                    .FirstOrDefaultAsync(fr => (fr.SenderId == request.SenderId && fr.ReceiverId == request.ReceiverId) ||
                                               (fr.SenderId == request.ReceiverId && fr.ReceiverId == request.SenderId));

                if (friendRequest == null)
                {
                    return NotFound(new { message = "Lời mời kết bạn không tồn tại." });
                }

                _context.FriendRequests.Remove(friendRequest);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Lời mời kết bạn đã được xóa thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Đã xảy ra lỗi: {ex.Message}" });
            }
        }

    }
}
