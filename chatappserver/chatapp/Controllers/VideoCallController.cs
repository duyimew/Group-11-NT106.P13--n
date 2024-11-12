using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace chatapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VideoController : ControllerBase
    {
        private static readonly Dictionary<string, byte[]> videoFrames = new Dictionary<string, byte[]>();
        private static readonly Dictionary<string, byte[]> audioData = new Dictionary<string, byte[]>(); // Lưu âm thanh

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFrame(string userId, IFormFile frame)
        {
            if (frame == null || frame.Length == 0)
            {
                return BadRequest("No frame uploaded.");
            }

            using (var ms = new MemoryStream())
            {
                await frame.CopyToAsync(ms);
                videoFrames[userId] = ms.ToArray();
            }
            return Ok();
        }

        [HttpGet("getFrame")]
        public IActionResult GetFrame(string userId)
        {
            if (videoFrames.ContainsKey(userId))
            {
                return File(videoFrames[userId], "image/jpeg");
            }
            return NotFound();
        }

        [HttpPost("uploadAudio")]
        public async Task<IActionResult> UploadAudio(string userId, IFormFile audio)
        {
            if (audio == null || audio.Length == 0)
            {
                return BadRequest("No audio data uploaded.");
            }

            using (var ms = new MemoryStream())
            {
                await audio.CopyToAsync(ms);
                audioData[userId] = ms.ToArray();
            }
            return Ok();
        }

        [HttpGet("getAudio")]
        public IActionResult GetAudio(string userId)
        {
            if (audioData.ContainsKey(userId))
            {
                return File(audioData[userId], "audio/wav");
            }
            return NotFound();
        }
    }
}
