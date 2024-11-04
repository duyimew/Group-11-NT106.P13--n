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
    public class DangNhapController : Controller
    {
        private readonly ChatAppContext _context;
        private readonly DangNhap _dangnhap;
        public DangNhapController(ChatAppContext context, DangNhap dangnhap)
        {
            _context = context;
            _dangnhap = dangnhap;
        }

        [HttpPost("DangNhap")]
        public async Task<IActionResult> DangNhap([FromBody] DangNhapDTO request)
        {
            string[] userInfo = { "", request.Username, request.HashPassword};
            string[] registrationResult = await _dangnhap.DangNhapUserAsync(userInfo);
            if (registrationResult[0] == "1")
            {
                return Ok(new { message = "Đăng nhập thành công"});
            }
            else
            {
                return BadRequest(new { message = registrationResult[0] });
            }
        }
    }
}
