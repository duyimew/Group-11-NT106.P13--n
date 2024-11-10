using chatapp.Data;
using chatapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using chatapp.DTOs;
using chatapp.DataAccess;
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
            _friend= friend;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> List(int userId)
        {
            string[] result = await _friend.FriendListAsync(userId);
            if (result[0] == "1")
            {
                return Ok(new { message = result.Skip(1) });
            }
            else
            {
                return BadRequest(new { message = result[0] });
            }
        }

        [HttpGet("Request/{userId}")]
        public async Task<IActionResult> ListSentRequest(int userId)
        {
            string[] result = await _friend.ListSentRequest(userId);
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

    }
}
