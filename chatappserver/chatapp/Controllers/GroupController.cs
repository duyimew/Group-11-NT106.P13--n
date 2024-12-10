using chatapp.Data;
using chatapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using chatapp.DataAccess;
using chatserver.DTOs.Group;
using chatserver.DTOs.User;
using chatserver.DTOs.Danhmuc;
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
            string[] userInfo = { "", request.UserID,request.GroupID,request.displayname };
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
        [HttpPost("FindGroupDisplayname")]
        public async Task<IActionResult> FindGroupDisplayname([FromBody] InforuserDTO request)
        {
            try
            {
                var user = await _context.GroupMembers.FirstOrDefaultAsync(g => g.UserId == int.Parse(request.UserId));
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
        public async Task<IActionResult> FindGroupDisplayID([FromBody] InforuserDTO request)
        {
            try
            {
                var user = await _context.GroupMembers.FirstOrDefaultAsync(g => g.GroupDisplayname == request.groupdisplayname);
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
        [HttpPost("RenamegroupDisplayname")]
        public async Task<IActionResult> RenamegroupDisplayname([FromBody] InforuserDTO request)
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
    }
}
