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
    public class DangkyController : ControllerBase
    {
        private readonly ChatAppContext _context;
        private readonly Dangky _dangky;
        public DangkyController(ChatAppContext context, Dangky dangky)
        {
            _context = context;
            _dangky = dangky;
        }

        [HttpPost("DangKy")]
        public async Task<IActionResult> DangKy([FromBody] DangKyDTO request)
        {
            string[] userInfo = { "", request.Username, request.HashPassword, request.Email, request.Ten, request.NgaySinh };
            string[] registrationResult = await _dangky.DangkyUserAsync(userInfo);
            if (registrationResult[0] == "1")
            {
                return Ok(new { message = "Đăng ký thành công" });
            }
            else
            {
                return BadRequest(new { message = registrationResult[0] });
            }
        }
    }
}
