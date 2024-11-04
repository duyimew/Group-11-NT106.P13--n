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
    public class MessageController : ControllerBase
    {
        private readonly ChatAppContext _context;
        private readonly SendMess _SendMess;
        private readonly ReceiveMess _receivemess;
        public MessageController(ChatAppContext context, SendMess sendMess, ReceiveMess receivemess)
        {
            _context = context;
            _SendMess = sendMess;
            _receivemess = receivemess;
        }

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] SendmessDTO request)
        {
            string[] userInfo = { "", request.Message, request.Channelname,request.Groupname,request.Username };
            string[] registrationResult = await _SendMess.SendMessAsync(userInfo);
            if (registrationResult[0] == "1")
            {
                return Ok(new { message = "Gửi tin nhắn thành công" });
            }
            else if (registrationResult[0] == "0")
            {
                return Ok(new { message = "Gửi tin nhắn thất bại" });
            }
            else
            {
                return BadRequest(new { message = registrationResult[0] });
            }
        }
        [HttpPost("ReceiveMessage")]
        public async Task<IActionResult> ReceiveMessage([FromBody] ReceivemessDTO request)
        {
            string[] userInfo = { "", request.Groupname,request.Channelname };
            string[] registrationResult = await _receivemess.ReceiveMessAsync(userInfo);
            if (registrationResult[0] == "1")
            {
                var Messagetext = registrationResult.Skip(1).ToArray();
                return Ok(new { messagetext = Messagetext });
            }
            else if (registrationResult[0] == "0")
            {
                return Ok(new { messagetext = registrationResult });
            }
            else
            {
                return BadRequest(new { message = registrationResult[0] });
            }
        }
    }
}
