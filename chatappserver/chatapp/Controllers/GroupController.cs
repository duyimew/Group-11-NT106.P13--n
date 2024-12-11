using chatapp.Data;
using chatapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using chatapp.DataAccess;
using chatserver.DTOs.Group;
using chatserver.DTOs.User;
using chatserver.DTOs.Danhmuc;
using System.Text.RegularExpressions;
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

        [HttpPost("RenameGroup")]
        public async Task<IActionResult> RenameGroup([FromBody] RenameGroupRequestDTO request)
        {
            try
            {

                var group = await _context.Groups.FirstOrDefaultAsync(g => g.GroupId == int.Parse(request.GroupId));
                if (group == null)
                {
                    return NotFound(new { message = "Group not found." });
                }

                group.GroupName = request.NewGroupName;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Rename group successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }

        [HttpPost("OneGroupname")]
        public async Task<IActionResult> OneGroupname([FromBody] OneGroupNameDTO request)
        {
            try
            {
                var group = await _context.Groups.FirstOrDefaultAsync(g => g.GroupId == int.Parse(request.groupId));
                if (group == null)
                {
                    return NotFound(new { message = "Group not found." });
                }

                string groupname = group.GroupName;

                return Ok(new { groupname=groupname });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }
        
        [HttpDelete("DeleteGroup")]
        public async Task<IActionResult> DeleteGroup([FromBody] DeleteGroupRequestDTO request)
        {
            try
            {
                var group = await _context.Groups.FirstOrDefaultAsync(d => d.GroupId == int.Parse(request.groupid));

                if (group == null)
                {
                    return NotFound(new { message = "group not found." });
                }

                _context.Groups.Remove(group);

                await _context.SaveChangesAsync();

                return Ok(new { message = "group deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }
        
        
    }
}
