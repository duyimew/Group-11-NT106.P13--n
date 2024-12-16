using chatapp.Data;
using chatapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using chatapp.DataAccess;
using chatserver.DTOs.User;
using chatserver.DTOs.Channel;
using chatserver.DTOs.GroupMember;
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
            string[] userInfo = { "", request.displayname };
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
        public async Task<IActionResult> FindUser([FromBody] FindUserDTO request)
        {
            string[] userName = { "", request.displayname };
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
        public async Task<IActionResult> FindUserID([FromBody] FindUserIDDTO request)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(g => g.Displayname == request.displayname);
                if (user == null)
                {
                    return NotFound(new { message = "user not found." });
                }

                int userid = user.UserId;

                return Ok(new { userid = userid.ToString() });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }
        [HttpPost("FindDisplayname")]
        public async Task<IActionResult> FindDisplayname([FromBody] FindDisplaynameDTO request)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(g => g.UserId == int.Parse(request.UserId));
                if (user == null)
                {
                    return NotFound(new { message = "user not found." });
                }

                string displayname = user.Displayname;

                return Ok(new { displayname = displayname });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }
        [HttpPost("FindUsername")]
        public async Task<IActionResult> FindUsername([FromBody] FindDisplaynameDTO request)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(g => g.UserId == int.Parse(request.UserId));
                if (user == null)
                {
                    return NotFound(new { message = "user not found." });
                }

                string username = user.Username;

                return Ok(new { username = username });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }
        [HttpPost("FindCreatetime")]
        public async Task<IActionResult> FindCreatetime([FromBody] FindCreatetimeDTO request)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(g => g.UserId == int.Parse(request.UserId));
                if (user == null)
                {
                    return NotFound(new { message = "user not found." });
                }

                DateTime TIME = user.CreatedAt;

                return Ok(new { time = TIME });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }
        [HttpPost("RenameDisplayname")]
        public async Task<IActionResult> RenameDisplayname([FromBody] RenameDisplaynameDTO request)
        {
            try
            {
                // Tìm người dùng theo UserId
                var user = await _context.Users.FirstOrDefaultAsync(g => g.UserId == int.Parse(request.UserId));
                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                // Kiểm tra nếu Displayname mới đã tồn tại
                var isDisplaynameTaken = await _context.Users.AnyAsync(g => g.Displayname == request.newdisplayname);
                if (isDisplaynameTaken)
                {
                    return BadRequest(new { message = "Displayname is already taken." });
                }

                // Cập nhật Displayname
                user.Displayname = request.newdisplayname;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Rename displayname successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }

        [HttpPost("RenameUsername")]
        public async Task<IActionResult> RenameUsername([FromBody] RenameUsernameDTO request)
        {
            try
            {
                // Tìm user hiện tại dựa trên UserId
                var user = await _context.Users.FirstOrDefaultAsync(g => g.UserId == int.Parse(request.UserId));
                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                // Kiểm tra xem Username mới đã tồn tại chưa
                var existingUser = await _context.Users.FirstOrDefaultAsync(g => g.Username == request.newusername);
                if (existingUser != null)
                {
                    return BadRequest(new { message = "Username already exists. Please choose a different username." });
                }

                // Cập nhật Username
                user.Username = request.newusername;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Rename username successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }

        [HttpPost("RenameEmail")]
        public async Task<IActionResult> RenameEmail([FromBody] RenameEmailDTO request)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(g => g.UserId == int.Parse(request.UserId));
                if (user == null)
                {
                    return NotFound(new { message = "user not found." });
                }

                user.Email = request.newemail;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Rename email successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }
        [HttpPost("RenameTen")]
        public async Task<IActionResult> RenameTen([FromBody] RenameTenDTO request)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(g => g.UserId == int.Parse(request.UserId));
                if (user == null)
                {
                    return NotFound(new { message = "user not found." });
                }

                user.FullName = request.newten;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Rename ten successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }
        [HttpPost("RenameNgaySinh")]
        public async Task<IActionResult> RenameNgaySinh([FromBody] RenameNgaySinhDTO request)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(g => g.UserId == int.Parse(request.UserId));
                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                if (DateTime.TryParseExact(request.newngaysinh, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var parsedDate))
                {
                    user.Birthday = parsedDate;
                    await _context.SaveChangesAsync();

                    return Ok(new { message = "Rename ngay sinh successfully!" });
                }
                else
                {
                    return BadRequest(new { message = "Invalid date format. Please use dd/MM/yyyy." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }

        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserRequestDTO request)
        {
            try
            {
                var userId = int.Parse(request.userid);

                // Lấy user cần xóa
                var user = await _context.Users.FirstOrDefaultAsync(d => d.UserId == userId);

                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                // Xóa các bản ghi liên quan trong bảng Friends
                var friends = _context.Friends.Where(f => f.UserId_1 == userId || f.UserId_2 == userId);
                _context.Friends.RemoveRange(friends);
                var friendrequests = _context.FriendRequests.Where(f => f.SenderId == userId || f.ReceiverId == userId);
                _context.FriendRequests.RemoveRange(friendrequests);
                // Xóa user
                _context.Users.Remove(user);

                await _context.SaveChangesAsync();

                return Ok(new { message = "User deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }

    }
}
