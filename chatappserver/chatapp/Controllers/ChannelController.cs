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
            string[] userInfo = { "", request.Channelname,request.Groupname };
            string[] registrationResult = await _dkchannel.DangkyChannelAsync(userInfo);
            if (registrationResult[0] == "1")
            {
                return Ok(new { message = "Đăng ký channel thành công" });
            }
            else
            {
                return BadRequest(new { message = registrationResult[0] });
            }
        }
        [HttpPost("ChannelName")]
        public async Task<IActionResult> ChannelName([FromBody] ChannelnameDTO request)
        {
            string[] userInfo = { "", request.Groupname };
            string[] registrationResult = await _channelname.ChannelNameAsync(userInfo);
            if (registrationResult[0] == "1")
            {
                var ChannelNames = registrationResult.Skip(1).ToArray();
                return Ok(new { ChannelName = ChannelNames });
            }
            else if(registrationResult[0] == "0")
            {
                return Ok(new { ChannelName = registrationResult });
            }
            else
            {
                return BadRequest(new { message = registrationResult[0] });
            }
        }
    }
}
