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
    public class ChannelController : ControllerBase
    {
        private readonly ChatAppContext _context;
        private readonly DKChannel _dkchannel;
        private readonly Channelname _channelname;
        public ChannelController(ChatAppContext context, DKChannel dkchannel, Channelname channelname)
        {
            _context = context;
            _dkchannel = dkchannel;
            _channelname = channelname;
        }

        [HttpPost("DKChannel")]
        public async Task<IActionResult> DKChannel([FromBody] DKChannelDTO request)
        {
            string[] userInfo = { "", request.Channelname,request.GroupID,request.ischat.ToString(),request.danhmucID };
            string[] registrationResult = await _dkchannel.DangkyChannelAsync(userInfo);
            if (registrationResult[0] == "1")
            {
                return Ok(new { message = "Đăng ký channel thành công", channelID = registrationResult[1] });
            }
            else
            {
                return BadRequest(new { message = registrationResult[0] });
            }
        }
        [HttpPost("ChannelName")]
        public async Task<IActionResult> ChannelName([FromBody] ChannelnameDTO request)
        {
            string[] userInfo = { "", request.GroupID };
            string[] registrationResult = await _channelname.ChannelNameAsync(userInfo);
            if (registrationResult[0] == "1")
            {
                var ChannelIDNames = registrationResult.Skip(1).ToArray();
                return Ok(new { ChannelIDName = ChannelIDNames });
            }
            else
            {
                return BadRequest(new { message = registrationResult[0] });
            }
        }
    }
}
