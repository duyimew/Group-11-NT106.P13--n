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
    public class GroupController : ControllerBase
    {
        private readonly ChatAppContext _context;
        private readonly DKGroup _dkgroup;
        private readonly AddUser _adduser;
        private readonly Groupname _groupname;
        public GroupController(ChatAppContext context,DKGroup dkgroup,AddUser adduser,Groupname groupname)
        {
            _context = context;
            _dkgroup = dkgroup;
            _adduser = adduser;
            _groupname = groupname;
        }

        [HttpPost("DKGroup")]
        public async Task<IActionResult> DKGroup([FromBody] DKGroupDTO request)
        {
            string[] userInfo = { "", request.Groupname };
            string[] registrationResult = await _dkgroup.DangkyGroupAsync(userInfo);
            if (registrationResult[0] == "1")
            {
                return Ok(new { message = "Đăng ký group thành công", groupid = registrationResult[1] });
            }
            else
            {
                return BadRequest(new { message = registrationResult[0] });
            }
        }
        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromBody] AddUserDTO request)
        {
            string[] userInfo = { "", request.UserID,request.GroupID };
            string[] registrationResult = await _adduser.AddUserAsync(userInfo);
            if (registrationResult[0] == "1")
            {
                return Ok(new { message = "Thêm người dùng thành công" });
            }
            else
            {
                return BadRequest(new { message = registrationResult[0] });
            }
        }
        [HttpPost("GroupName")]
        public async Task<IActionResult> GroupName([FromBody] GroupnameDTO request)
        {
            string[] userInfo = { "", request.UserID };
            string[] registrationResult = await _groupname.GroupNameAsync(userInfo);
            if (registrationResult[0] == "1")
            {
                var groupidNames = registrationResult.Skip(1).ToArray();
                return Ok(new { groupidname = groupidNames });
            }
            else
            {
                return BadRequest(new { message = registrationResult[0] });
            }
        }
    }
}
