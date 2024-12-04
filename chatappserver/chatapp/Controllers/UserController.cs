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
    public class UserController : ControllerBase
    {
        private readonly ChatAppContext _context;
        private readonly Inforuser _inforuser;
        private readonly FindUser _findUser;
        public UserController(ChatAppContext context, Inforuser inforuser, FindUser findUser)
        {
            _context = context;
            _inforuser = inforuser;
            _findUser = findUser;
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

        [HttpPost("FindUser")]
        public async Task<IActionResult> FindUser([FromBody] InforuserDTO request)
        {
            string[] userName = { "", request.Username };
            string[] result = await _findUser.FindUserAsync(userName);
            if (result[0] == "1")
            {
                return Ok(new { message = result.Skip(1) });
            }
            else
            {
                return BadRequest(new { message = result[0] });
            }
        }
        [HttpPost("FindUserID")]
        public async Task<IActionResult> FindUserID([FromBody] InforuserDTO request)
        {
            string[] userName = { "", request.Username };
            string[] result = await _findUser.FindUserIDAsync(userName);
            if (result[0] == "1")
            {
                return Ok(new { message = result.Skip(1) });
            }
            else
            {
                return BadRequest(new { message = result[0] });
            }
        }
    }
}
