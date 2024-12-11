using chatapp.Data;
using chatapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using chatapp.DataAccess;
using chatserver.DTOs.GroupMember;
using chatserver.DTOs.User;
using chatserver.DTOs.Danhmuc;
using System.Text.RegularExpressions;
namespace chatapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupMemberController : ControllerBase
    {
        private readonly ChatAppContext _context;
        private readonly AddUser _adduser;
        private readonly RequestUserRole _requestUserRole;
        public GroupMemberController(ChatAppContext context,AddUser adduser,RequestUserRole requestUserRole)
        {
            _context = context;
            _adduser = adduser;
            _requestUserRole = requestUserRole;
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromBody] AddUserDTO request)
        {
            string[] userInfo = { "", request.UserID,request.GroupID,request.displayname,request.role };
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

        [HttpPost("UserRole")]
        public async Task<IActionResult> UserRole([FromBody] UserRoleDTO request)
        {
            string[] userInfo = { "", request.GroupId };
            string[] registrationResult = await _requestUserRole.RequestUserRoleAsync(userInfo);
            if (registrationResult[0] == "1")
            {
                var userRoleNameId = registrationResult.Skip(1).ToArray();
                return Ok(new { userRoleNameId = userRoleNameId });
            }
            else
            {
                return BadRequest(new { message = registrationResult[0] });
            }
        }
        [HttpPost("OneUserRole")]
        public async Task<IActionResult> OneUserRole([FromBody] OneUserRoleDTO request)
        {
            try
            {
                var groupmember = await _context.GroupMembers.FirstOrDefaultAsync(d => d.GroupId == int.Parse(request.groupid) && d.UserId == int.Parse(request.UserId));

                if (groupmember == null)
                {
                    return NotFound(new { message = "groupmember not found." });
                }

                string role=groupmember.Role;


                return Ok(new { role = role });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }
        [HttpPost("RenameRole")]
        public async Task<IActionResult> RenameRole([FromBody] RenameRoleDTO request)
        {
            try
            {
                var groupmember = await _context.GroupMembers.FirstOrDefaultAsync(d => d.GroupId == int.Parse(request.groupid) && d.UserId == int.Parse(request.UserId));

                if (groupmember == null)
                {
                    return NotFound(new { message = "groupmember not found." });
                }

                groupmember.Role = request.newrole;

                await _context.SaveChangesAsync();

                return Ok(new { message = "change role successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }
        [HttpPost("FindGroupDisplayname")]
        public async Task<IActionResult> FindGroupDisplayname([FromBody] FindGroupDisplaynameDTO request)
        {
            try
            {
                var user = await _context.GroupMembers.FirstOrDefaultAsync(d => d.GroupId == int.Parse(request.groupid) && d.UserId == int.Parse(request.UserId));
                if (user == null)
                {
                    return NotFound(new { message = "user not found." });
                }

                string groupdisplayname = user.GroupDisplayname;

                return Ok(new { groupdisplayname = groupdisplayname });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }
        [HttpPost("FindGroupDisplayID")]
        public async Task<IActionResult> FindGroupDisplayID([FromBody] FindGroupDisplayIDDTO request)
        {
            try
            {
                var user = await _context.GroupMembers.FirstOrDefaultAsync(g => g.GroupDisplayname == request.groupdisplayname&& g.GroupId == int.Parse(request.groupid));
                if (user == null)
                {
                    return NotFound(new { message = "user not found." });
                }

                string groupdisplayid = user.UserId.ToString();

                return Ok(new { groupdisplayid = groupdisplayid });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }
        [HttpPost("FindJointime")]
        public async Task<IActionResult> FindJointime([FromBody] FindJointimeDTO request)
        {
            try
            {
                var groupmember = await _context.GroupMembers.FirstOrDefaultAsync(g => g.UserId == int.Parse(request.UserId)&& g.GroupId == int.Parse(request.groupid));
                if (groupmember == null)
                {
                    return NotFound(new { message = "user not found." });
                }

                DateTime TIME = groupmember.JoinedAt;

                return Ok(new { time = TIME });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }
        [HttpPost("RenamegroupDisplayname")]
        public async Task<IActionResult> RenamegroupDisplayname([FromBody] RenamegroupDisplaynameDTO request)
        {
            try
            {
                var groupmember = await _context.GroupMembers.FirstOrDefaultAsync(d => d.GroupId == int.Parse(request.groupid) && d.UserId == int.Parse(request.UserId));
                if (groupmember == null)
                {
                    return NotFound(new { message = "groupmember not found." });
                }

                groupmember.GroupDisplayname = request.newgroupdisplayname;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Rename groupdisplayname successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }
        [HttpDelete("DeleteGroupMember")]
        public async Task<IActionResult> DeleteGroupMember([FromBody] DeleteGroupMemberRequestDTO request)
        {
            try
            {
                var groupmember = await _context.GroupMembers.FirstOrDefaultAsync(d => d.GroupId == int.Parse(request.groupid)&&d.UserId == int.Parse(request.userid));

                if (groupmember == null)
                {
                    return NotFound(new { message = "groupmember not found." });
                }

                _context.GroupMembers.Remove(groupmember);

                await _context.SaveChangesAsync();

                return Ok(new { message = "groupmember deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }

     




    }
}
