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
        private readonly Friend _friendList;

        public FriendController(ChatAppContext context, Friend friendList)
        {
            _context = context;
            _friendList = friendList;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> List(int userId)
        {
            string[] result = await _friendList.FriendListAsync(userId);
            if (result[0] == "1")
            {
                return Ok(new { users = result.Skip(1) });
            }
            else
            {
                return BadRequest(new { message = result[0] });
            }
        }

        [HttpGet("Request")]
        public async Task<IActionResult> ListSentRequest(int userId)
        {
            string[] result = await _friendList.FriendListAsync(userId);
            if (result[0] == "1")
            {
                return Ok(new { users = result.Skip(1) });
            }
            else
            {
                return BadRequest(new { message = result[0] });
            }
        }


    }
}
