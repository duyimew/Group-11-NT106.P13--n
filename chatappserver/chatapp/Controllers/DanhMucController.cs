using chatapp.Data;
using chatapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using chatapp.DataAccess;
using chatserver.DTOs.Danhmuc;
using chatserver.DTOs.Group;
using chatserver.DTOs.Channel;
namespace chatapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DanhMucController : ControllerBase
    {
        private readonly ChatAppContext _context;
        private readonly DKDanhMuc _DKDanhMuc;
        private readonly Danhmucname _Danhmucname;
        public DanhMucController(ChatAppContext context, DKDanhMuc DKDanhMuc, Danhmucname Danhmucname)
        {
            _context = context;
            _DKDanhMuc = DKDanhMuc;
            _Danhmucname = Danhmucname;
        }

        [HttpPost("DKDanhMuc")]
        public async Task<IActionResult> DKDanhMuc([FromBody] DKDanhMucDTO request)
        {
            string[] userInfo = { "", request.DanhMucname,request.GroupID };
            string[] registrationResult = await _DKDanhMuc.DangkyDanhmucAsync(userInfo);
            if (registrationResult[0] == "1")
            {
                return Ok(new { message = "Đăng ký danh mục thành công",danhmucID = registrationResult[1] });
            }
            else
            {
                return BadRequest(new { message = registrationResult[0] });
            }
        }
        [HttpPost("Danhmucname")]
        public async Task<IActionResult> Danhmucname([FromBody] DanhMucnameDTO request)
        {
            string[] userInfo = { "", request.GroupID };
            string[] registrationResult = await _Danhmucname.DanhmucnameAsync(userInfo);
            if (registrationResult[0] == "1")
            {
                var DanhmucIDname = registrationResult.Skip(1).ToArray();
                return Ok(new { DanhmucIDName = DanhmucIDname });
            }
            else
            {
                return BadRequest(new { message = registrationResult[0] });
            }
        }
        [HttpPost("RenameDanhmuc")]
        public async Task<IActionResult> RenameDanhmuc([FromBody] RenameDanhmucRequestDTO request)
        {
            try
            {

                var danhmuc = await _context.danhmuc.FirstOrDefaultAsync(g => g.DanhmucId == int.Parse(request.danhmucid));
                if (danhmuc == null)
                {
                    return NotFound(new { message = "danhmuc not found." });
                }

                danhmuc.DanhmucName = request.newdanhmucname;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Rename danhmuc successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }


        [HttpPost("OneDanhmucname")]
        public async Task<IActionResult> OneDanhmucname([FromBody] OneDanhmucNameDTO request)
        {
            try
            {
                var danhmuc = await _context.danhmuc.FirstOrDefaultAsync(g => g.DanhmucId == int.Parse(request.danhmucid));
                if (danhmuc == null)
                {
                    return NotFound(new { message = "danhmuc not found." });
                }

                string danhmucname = danhmuc.DanhmucName;

                return Ok(new { danhmucname = danhmucname });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }
        [HttpDelete("DeleteDanhmuc")]
        public async Task<IActionResult> DeleteDanhmuc([FromBody] DeleteDanhmucRequestDTO request)
        {
            try
            {
                var danhmuc = await _context.danhmuc
            .Include(c => c.Channels)
            .FirstOrDefaultAsync(d => d.DanhmucId == int.Parse(request.danhmucid));

                if (danhmuc == null)
                {
                    return NotFound(new { message = "danh muc not found." });
                }

                foreach (var channel in danhmuc.Channels.ToList())
                {
                    _context.Channels.Remove(channel);
                }

                _context.danhmuc.Remove(danhmuc);

                await _context.SaveChangesAsync();

                return Ok(new { message = "danh muc deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }
    }
}
