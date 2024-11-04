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
    public class TokenController : Controller
    {
        private readonly ChatAppContext _context;
        private readonly Token _token;
        public TokenController(ChatAppContext context, Token token)
        {
            _context = context;
            _token = token;
        }

        [HttpPost("GenerateToken")]
        public async Task<IActionResult> GenerateToken([FromBody] TokenDTO request)
        {
            string[] registrationResult = await _token.GenerateToken(request.Username);
            if (registrationResult[0] == "1")
            {
                return Ok(new { token = registrationResult[1] });
            }
            else
            {
                return BadRequest(new { message = registrationResult[0] });
            }
        }
    }
}
