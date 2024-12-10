using Azure.Storage.Blobs;
using chatapp.Data;
using chatapp.DataAccess;
using chatapp.Models;
using chatserver.DTOs.Message;
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
        string[] userInfo = new string[4 + request.filename.Length];
        userInfo[0] = "";
        userInfo[1] = request.Message;
        userInfo[2] = request.ChannelID;
        userInfo[3] = request.UserID;
        for (int i = 0; i < request.filename.Length; i++)
        {
            userInfo[4 + i] = request.filename[i];
        }
        string[] registrationResult = await _savefileinfo.savefileinfoAsync(userInfo);
        if (registrationResult[0] == "1")
        {
            string result = "";
            for(int i=1; i<registrationResult.Length-1;i++)
            {
                result += registrationResult[i] +"|";
            }
            result += registrationResult[registrationResult.Length-1];
            return Ok(new { message = "Gửi tin nhắn thành công", messatid=result });
        }
        else
        {
            return BadRequest(new { message = registrationResult[0] });
        }

    }
    [HttpPost("upload-avatar")]
    public async Task<IActionResult> UploadAvatar(IFormFile avatar, string userid)
    {
        try
        {
            if (avatar == null || avatar.Length == 0)
                return BadRequest("No file uploaded");

            // Tạo BlobServiceClient để kết nối với Azure Blob Storage
            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerName); // Sử dụng container dành riêng cho ảnh đại diện

            // Tạo tên blob độc nhất, có thể sử dụng userId để đảm bảo ảnh của mỗi người dùng là duy nhất
            string blobName = $"{userid}_avatar_{Path.GetExtension(avatar.FileName)}";
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            using (var stream = avatar.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }
            string avatarUrl = blobClient.Uri.ToString();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == int.Parse(userid));
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
    [HttpPost("upload-avatar-group")]
    public async Task<IActionResult> UploadAvatarGroup(IFormFile avatargroup, string groupid)
    {
        try
        {
            if (avatargroup == null || avatargroup.Length == 0)
                return BadRequest("No file uploaded");

            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerName); // Sử dụng container dành riêng cho ảnh đại diện

            string blobName = $"{groupid}_avatargroup_{Path.GetExtension(avatargroup.FileName)}";
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            using (var stream = avatargroup.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }
            string avatargroupUrl = blobClient.Uri.ToString();
            var group = await _context.Groups.FirstOrDefaultAsync(u => u.GroupId == int.Parse(groupid));
            if (group != null)
            {
                group.GroupAva = avatargroupUrl;
                await _context.SaveChangesAsync();
            }

            return Ok(new { message = "Avatar group uploaded successfully!", AvatarGroupUrl = avatargroupUrl });
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }
    [HttpGet("get-avatar")]
    public async Task<IActionResult> GetAvatarUrl(string userid)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == int.Parse(userid));

        if (user == null || string.IsNullOrEmpty(user.UserAva))
        {
            return NotFound("Avatar not found");
        }

        return Ok(new { AvatarUrl = user.UserAva });
    }
    [HttpGet("get-avatar-group")]
    public async Task<IActionResult> GetAvatarGroupUrl(string groupid)
    {
        var group = await _context.Groups.FirstOrDefaultAsync(u => u.GroupId == int.Parse(groupid));

        if (group == null || string.IsNullOrEmpty(group.GroupAva))
        {
            return NotFound("Avatar not found");
        }

        return Ok(new { AvatarGroupUrl = group.GroupAva });
    }

}
