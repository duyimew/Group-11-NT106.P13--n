using Azure.Storage.Blobs;
using chatapp.Data;
using chatapp.DataAccess;
using chatapp.DTOs;
using chatapp.Models;
using chatserver.DataAccess;
using chatserver.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class FileController : ControllerBase
{
    private readonly string _connectionString = "DefaultEndpointsProtocol=https;AccountName=dbfilechat;AccountKey=2Ryv6N7JC8mZsBmXtyngkK+P3rNYuBjHLA7oyQDoTUrPykpS9urkknY9ZvQczMi1fxxNFrYLEahQ+AStJC6xCA==;EndpointSuffix=core.windows.net"; // Thay bằng connection string của bạn
    private readonly string _containerName = "dbfile";
    private readonly ChatAppContext _context;
    private readonly savefileinfo _savefileinfo;
    private readonly UserAvatar? _avatar;
    public FileController(ChatAppContext context, savefileinfo savefileinfo)
    {
        _context = context;
        _savefileinfo = savefileinfo;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

            string blobName = Path.GetFileName(file.FileName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }

            return Ok(new { message="Upload thành công!" });
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }

    [HttpGet("download")]
    public IActionResult GetFileUrl(string fileName)
    {
        BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerName);
        BlobClient blobClient = containerClient.GetBlobClient(fileName);
        string fileUrl = Uri.UnescapeDataString(blobClient.Uri.ToString());

        return Ok(new { FileUrl = fileUrl });
    }
    [HttpPost("savefileinfo")]
    public async Task<IActionResult> savefileinfo([FromBody] savefileinfoDTO request)
    {
        string[] userInfo = new string[6 + request.filename.Length];
        userInfo[0] = "";
        userInfo[1] = request.Message;
        userInfo[2] = request.Channelname;
        userInfo[3] = request.Groupname;
        userInfo[4] = request.Username;
        for (int i = 0; i < request.filename.Length; i++)
        {
            userInfo[5 + i] = request.filename[i];
        }
        string[] registrationResult = await _savefileinfo.savefileinfoAsync(userInfo);
        if (registrationResult[0] == "1")
        {
            return Ok(new { message = "Gửi tin nhắn thành công" });
        }
        else
        {
            return BadRequest(new { message = registrationResult[0] });
        }

    }
    [HttpPost("upload-avatar")]
    public async Task<IActionResult> UploadAvatar(IFormFile avatar, string username)
    {
        try
        {
            if (avatar == null || avatar.Length == 0)
                return BadRequest("No file uploaded");

            // Tạo BlobServiceClient để kết nối với Azure Blob Storage
            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerName); // Sử dụng container dành riêng cho ảnh đại diện

            // Tạo tên blob độc nhất, có thể sử dụng userId để đảm bảo ảnh của mỗi người dùng là duy nhất
            string blobName = $"{username}_avatar_{Path.GetExtension(avatar.FileName)}";
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            using (var stream = avatar.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }
            string avatarUrl = blobClient.Uri.ToString();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user != null)
            {
                user.UserAva = avatarUrl;
                await _context.SaveChangesAsync();
            }

            return Ok(new { message = "Avatar uploaded successfully!", AvatarUrl = avatarUrl });
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }
    [HttpGet("get-avatar")]
    public async Task<IActionResult> GetAvatarUrl(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

        if (user == null || string.IsNullOrEmpty(user.UserAva))
        {
            return NotFound("Avatar not found");
        }

        return Ok(new { AvatarUrl = user.UserAva });
    }


}
