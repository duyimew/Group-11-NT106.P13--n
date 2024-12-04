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
    }
}
