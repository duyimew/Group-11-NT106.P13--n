using chatapp.Data;
using chatapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using chatapp.DataAccess;
using chatserver.DTOs.Message;
using chatserver.DTOs.Group;
namespace chatapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly ChatAppContext _context;
        private readonly SendMess _SendMess;
        private readonly ReceiveMess _receivemess;
        private readonly SearchMess _searchmess;

        public MessageController(ChatAppContext context, SendMess sendMess, ReceiveMess receivemess,SearchMess searchMess)
        {
            _context = context;
            _SendMess = sendMess;
            _receivemess = receivemess;
            _searchmess = searchMess;
        }

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] SendmessDTO request)
        {
            string[] userInfo = { "", request.Message, request.ChannelID,request.UserID };
            string[] registrationResult = await _SendMess.SendMessAsync(userInfo);
            if (registrationResult[0] == "1")
            {
                return Ok(new { message = "Gửi tin nhắn thành công", messageid = registrationResult[1] });
            }
            else
            {
                return BadRequest(new { message = registrationResult[0] });
            }
        }
        [HttpPost("ReceiveMessage")]
        public async Task<IActionResult> ReceiveMessage([FromBody] ReceivemessDTO request)
        {
            string[] userInfo = { "", request.ChannelID };
            string[] registrationResult = await _receivemess.ReceiveMessAsync(userInfo);
            if (registrationResult[0] == "1")
            {
                var Messagetext = registrationResult.Skip(1).ToArray();
                return Ok(new { messagetext = Messagetext });
            }
            else
            {
                return BadRequest(new { message = registrationResult[0] });
            }
        }
        [HttpPost("EditMessage")]
        public async Task<IActionResult> EditMessage([FromBody] EditMessageRequestDTO request)
        {
            try
            {

                var message = await _context.Messages.FirstOrDefaultAsync(g => g.MessageId == int.Parse(request.messageid));
                if (message == null)
                {
                    return NotFound(new { message = "message not found." });
                }

                message.MessageText = request.newmessage;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Edit message successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }


        [HttpPost("OneMessage")]
        public async Task<IActionResult> OneMessage([FromBody] OneMessageDTO request)
        {
            try
            {
                var message = await _context.Messages.FirstOrDefaultAsync(g => g.MessageId == int.Parse(request.messageid));
                if (message == null)
                {
                    return NotFound(new { message = "message not found." });
                }

                string messagetext = message.MessageText;

                return Ok(new { messagetext = messagetext });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }
        [HttpDelete("DeleteMessage")]
        public async Task<IActionResult> DeleteMessage([FromBody] DeleteMessageRequestDTO request)
        {
            try
            {
                var message = await _context.Messages.FirstOrDefaultAsync(d => d.MessageId == int.Parse(request.messageid));

                if (message == null)
                {
                    return NotFound(new { message = "message not found." });
                }

                _context.Messages.Remove(message);

                await _context.SaveChangesAsync();

                return Ok(new { message = "message deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }
        [HttpPost("SearchMessage")]
        public async Task<IActionResult> SearchMessage([FromBody] SearchMessageDTO request)
        {

            string[] userInfo = { "", request.messagetext, request.username, request.channelid, request.hinhanh.ToString(), request.tep.ToString(), request.beforeDate, request.onDate, request.afterDate };
            string[] registrationResult = await _searchmess.SearchMessAsync(userInfo);
            if (registrationResult[0] == "1")
            {
                var Messagetext = registrationResult.Skip(1).ToArray();
                return Ok(new { messagetext = Messagetext });
            }
            else
            {
                return BadRequest(new { message = registrationResult[0] });
            }


        }
    }
}
