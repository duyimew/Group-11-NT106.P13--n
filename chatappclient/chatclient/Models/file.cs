using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Newtonsoft.Json;
using System.Net.NetworkInformation;
using QLUSER.DTOs;
using Newtonsoft.Json.Linq;
using System.Net.Sockets;
using System.Threading;
using Microsoft.AspNetCore.SignalR.Client;
namespace QLUSER.Models
{
    internal class file
    {
        public static Image userAvatarImage;
        public async Task<(bool issuccess, string messatid)> SendFileToServer(string userid, string channelid, string message, List<string> selectedFilePaths)
        {
            try
            {

                foreach (string filepath in selectedFilePaths)
                {
                    await UploadFileAsync(filepath);
                }
                string[] filenames = selectedFilePaths.Select(Path.GetFileName).ToArray();
                var sendfile = new savefileinfoDTO
                { 
                UserID=userid,
                ChannelID=channelid,
                Message=message,
                filename=filenames
                };
                var json = JsonConvert.SerializeObject(sendfile);
                var content = new StringContent(json, Encoding.Unicode, "application/json");
                HttpClient client = new HttpClient();
                var response = await client.PostAsync(ConfigurationManager.AppSettings["ServerUrl"] + "File/savefileinfo", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    string Message = responseData.message;
                    string messatid = responseData.messatid;
                    MessageBox.Show(Message);
                    return (true,messatid);
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<dynamic>(errorMessage);
                    string message1= responseData.message;
                    MessageBox.Show(message1);
                    return (false, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi gửi file: {ex.Message}");
                return (false, null);
            }
        }
        private async Task UploadFileAsync(string filePath)
        {
            using (HttpClient client = new HttpClient())
            {

                using (var content = new MultipartFormDataContent())
                {
                    var fileContent = new ByteArrayContent(File.ReadAllBytes(filePath));
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                    content.Add(fileContent, "file", Path.GetFileName(filePath));

                    var response = await client.PostAsync(ConfigurationManager.AppSettings["ServerUrl"] + "File/upload", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        var responseData = JsonConvert.DeserializeObject<dynamic>(result);
                        string Message = responseData.message;
                        MessageBox.Show(Message);
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Upload thất bại. Mã lỗi: {response.StatusCode}. Nội dung lỗi: {errorContent}");
                    }
                }
            }
        }
        public void DownloadFile(string FileName)
        {
            Thread thread = new Thread(() =>
            {
                try
                {
                    using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                    {
                        saveFileDialog.FileName = FileName;
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            DownloadFileFromServer(FileName, saveFileDialog.FileName);
                            MessageBox.Show("File downloaded successfully!", "Download", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to download file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            
        }
        public async void DownloadFileFromServer(string Filename, string localFilePath)
        {
            string fileUrl = await GetFileUrlAsync(Filename);
            if (!string.IsNullOrEmpty(fileUrl))
            {
                await DownloadFileAsync(fileUrl, localFilePath);
                MessageBox.Show("Download thành công! File lưu tại: " + localFilePath);
            }
            else
            {
                MessageBox.Show("Không tìm thấy file.");
            }
        }
        public async Task<string> GetFileUrlAsync(string fileName)
        {
            using (HttpClient client = new HttpClient())
            {
                string fullUrl = $"{ConfigurationManager.AppSettings["ServerUrl"] + "File/download"}?fileName={Uri.EscapeDataString(fileName)}";
                var response = await client.GetAsync(new Uri(fullUrl));
                var rawResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var responseData = JsonConvert.DeserializeObject<dynamic>(rawResponse);
                    return responseData.fileUrl;
                }
                else
                {
                    MessageBox.Show($"Không tìm thấy file. Mã lỗi: {response.StatusCode}");
                }
            }
            return null;
        }

        public async Task DownloadFileAsync(string fileUrl, string savePath)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(new Uri(fileUrl));
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Failed to download file. Status code: {response.StatusCode}, Reason: {response.ReasonPhrase}");
                    return;
                }
                response.EnsureSuccessStatusCode();

                using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                {
                    await response.Content.CopyToAsync(fileStream);
                }
            }
        }
        


    }
}
