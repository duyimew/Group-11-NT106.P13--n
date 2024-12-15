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
        private readonly FindGroup _findgroup;
        public GroupController(ChatAppContext context,DKGroup dkgroup,AddUser adduser,Groupname groupname,FindGroup findGroup)
        {
            _context = context;
            _dkgroup = dkgroup;
            _adduser = adduser;
            _groupname = groupname;
            _findgroup = findGroup;
        }

        [HttpPost("DKGroup")]
        public async Task<IActionResult> DKGroup([FromBody] DKGroupDTO request)
        {
            string[] userInfo = { "", request.Groupname,request.Isprivate };
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
            string[] userInfo = { "", request.UserID,request.Isprivate };
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
        [HttpPost("FindGroup")]
        public async Task<IActionResult> FindGroup([FromBody] FindGroupDTO request)
        {
            string[] userInfo = { "", request.UserID1, request.UserID2 };
            string[] registrationResult = await _findgroup.FindGroupAsync(userInfo);
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
        [HttpPost("FindMaLoiMoi")]
        public async Task<IActionResult> FindMaLoiMoi([FromBody] FindMaLoiMoiDTO request)
        {
            try
            {

                var group = await _context.Groups.FirstOrDefaultAsync(g => g.GroupId == int.Parse(request.groupid));
                if (group == null)
                {
                    return NotFound(new { message = "Group not found." });
                }
                string maloimoi = group.MaLoiMoi;

                return Ok(new { maloimoi = maloimoi });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }
        [HttpPost("FindGroupID")]
        public async Task<IActionResult> FindGroupID([FromBody] FindGroupIDDTO request)
        {
            try
            {

                var group = await _context.Groups.FirstOrDefaultAsync(g => g.MaLoiMoi == request.MaLoiMoi);
                if (group == null)
                {
                    return NotFound(new { message = "Ma Loi Moi not found." });
                }
                string groupid = group.GroupId.ToString();

                return Ok(new { groupid = groupid });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
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

        [HttpDelete("RemoveMember")]
        public async Task<IActionResult> RemoveMember([FromBody] RemoveMemberFromGroupDTO request)
        {
            try
            {
                // Lấy thông tin nhóm
                var group = await _context.Groups
                                          .Include(g => g.GroupMembers)
                                          .FirstOrDefaultAsync(g => g.GroupId == int.Parse(request.groupid));

                if (group == null)
                    return NotFound(new { message = "Group not found" });

                // Xóa thành viên khỏi nhóm
                var member = group.GroupMembers.FirstOrDefault(m => m.UserId == int.Parse(request.userid));
                if (member != null)
                {
                    _context.GroupMembers.Remove(member);
                    await _context.SaveChangesAsync();
                }

                // Kiểm tra nếu nhóm là private và số lượng thành viên < 2
                if (group.Isprivate > 0 && group.GroupMembers.Count < 2)
                {
                    _context.Groups.Remove(group);
                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Group deleted because it had fewer than 2 members" });
                }

                return Ok(new { message = "Member removed from group" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


    }
}
