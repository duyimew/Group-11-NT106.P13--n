using Azure.Identity;
using Azure.Storage.Blobs;
using chatapp.Data;
using chatapp.DataAccess;
using chatapp.DTOs;
using chatapp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

[Route("api/[controller]")]
[ApiController]
public class FileController : ControllerBase
{
    private readonly string _connectionString;
    private readonly string _containerName = "dbfile";
    private readonly ChatAppContext _context;
    private readonly savefileinfo _savefileinfo;
    public FileController(ChatAppContext context, savefileinfo savefileinfo,IConfiguration configuration)
    {
        _context = context;
        _savefileinfo = savefileinfo;
        _connectionString = configuration.GetConnectionString("ChatAppStorage");
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
}
