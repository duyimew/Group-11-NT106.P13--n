using chatapp.Data;
using chatapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using chatapp.DataAccess;
using chatserver.DTOs.Channel;
using chatserver.DTOs.Group;
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
        [HttpPost("RenameChannel")]
        public async Task<IActionResult> RenameChannel([FromBody] RenameChannelRequestDTO request)
        {
            try
            {

                var Channel = await _context.Channels.FirstOrDefaultAsync(g => g.ChannelId == int.Parse(request.channelId));
                if (Channel == null)
                {
                    return NotFound(new { message = "Channel not found." });
                }

                Channel.ChannelName = request.newchannelName;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Rename Channel successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error:{ex.Message}" });
            }
        }


        [HttpPost("OneChannelname")]
        public async Task<IActionResult> OneChannelname([FromBody] OneChannelNameDTO request)
        {
            try
            {
                var Channel = await _context.Channels.FirstOrDefaultAsync(g => g.ChannelId == int.Parse(request.channelid));
                if (Channel == null)
                {
                    return NotFound(new { message = "Channel not found." });
                }

                string Channelname = Channel.ChannelName;

                return Ok(new { channelname = Channelname });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error:{ex.Message}" });
            }
        }
        [HttpDelete("DeleteChannel")]
        public async Task<IActionResult> DeleteChannel([FromBody] DeleteChannelRequestDTO request)
        {
            try
            {
                var channel = await _context.Channels.FirstOrDefaultAsync(c => c.ChannelId == int.Parse(request.ChannelId));

                if (channel == null)
                {
                    return NotFound(new { message = "Channel not found." });
                }
                _context.Channels.Remove(channel);

                await _context.SaveChangesAsync();

                return Ok(new { message = "Channel deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }


    }
}
