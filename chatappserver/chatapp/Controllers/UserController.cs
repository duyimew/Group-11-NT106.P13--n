﻿using chatapp.Data;
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
                var user = await _context.Users.FirstOrDefaultAsync(g => g.UserId == int.Parse(request.UserId));
                if (user == null)
                {
                    return NotFound(new { message = "user not found." });
                }

                user.Displayname = request.newdisplayname;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Rename displayname successfully!" });
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
                var user = await _context.Users.FirstOrDefaultAsync(d => d.UserId == int.Parse(request.userid));

                if (user == null)
                {
                    return NotFound(new { message = "user not found." });
                }

                _context.Users.Remove(user);

                await _context.SaveChangesAsync();

                return Ok(new { message = "user deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }
    }
}
