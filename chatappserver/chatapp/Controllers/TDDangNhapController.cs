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
    public class TDDangNhapController : ControllerBase
    {
        private readonly ChatAppContext _context;
        private readonly TDDangNhap _tddangnhap;
        public TDDangNhapController(ChatAppContext context, TDDangNhap tddangnhap)
        {
            _context = context;
            _tddangnhap = tddangnhap;
        }

        [HttpPost("TDDangNhap")]
        public async Task<IActionResult> TDDangNhap([FromBody] TDDangNhapDTO request)
        {
            string[] user = request.Token.Split("|");
            string[] userInfo = { "", user[0], user[1] };
            string[] registrationResult = await _tddangnhap.TDDangNhapAsync(userInfo);
            if (registrationResult[0] == "1")
            {
                return Ok(new { Username = registrationResult[1], userid = registrationResult[2] });
            }
            else
            {
                return BadRequest(new { message = registrationResult[0] });
            }
        }
    }
}
