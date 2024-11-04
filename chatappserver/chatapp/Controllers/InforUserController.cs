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
    public class InforUserController : ControllerBase
    {
        private readonly ChatAppContext _context;
        private readonly Inforuser _inforuser;
        public InforUserController(ChatAppContext context, Inforuser inforuser)
        {
            _context = context;
            _inforuser= inforuser;
        }

        [HttpPost("InforUser")]
        public async Task<IActionResult> InforUser([FromBody] InforuserDTO request)
        {
            string[] userInfo = { "", request.Username};
            string[] registrationResult = await _inforuser.InforUserAsync(userInfo);
            if (registrationResult[0] == "1")
            {
                return Ok(new { email = registrationResult[1], ten = registrationResult[2], ngaysinh = registrationResult[3] });
            }
            else
            {
                return BadRequest(new { message = registrationResult[0] });
            }
        }
    }
}
