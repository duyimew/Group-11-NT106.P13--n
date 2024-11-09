using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.Drawing;

namespace QLUSER.Models
{
    internal class UserAvatar
    {
        
        public async Task<string> UploadAvatarAsync(string filePath, string username)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                using (var content = new MultipartFormDataContent())
                {
                    using (var fileStream = File.OpenRead(filePath))
                    {
                        var fileContent = new ByteArrayContent(File.ReadAllBytes(filePath));
                        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data"); 
                        content.Add(fileContent, "avatar", Path.GetFileName(filePath)); 

                        content.Add(new StringContent(username), "username");

                        string url = $"{ConfigurationManager.AppSettings["ServerUrl"]}File/upload-avatar?username={username}";

                        var response = await client.PostAsync(url, content);

                        if (response.IsSuccessStatusCode)
                        {
                            var responseContent = await response.Content.ReadAsStringAsync();
                            var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                           
                            return responseData.avatarUrl;
                        }
                        else
                        {
                            var errorResponse = await response.Content.ReadAsStringAsync();
                            MessageBox.Show("Failed to upload avatar: " + errorResponse);
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error uploading avatar: " + ex.Message);
                return null;
            }
        }
        public async Task<Image> LoadAvatarAsync(string username)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string fullUrl = $"{ConfigurationManager.AppSettings["ServerUrl"] + "File/get-avatar"}?username={Uri.EscapeDataString(username)}";
                    var response = await client.GetAsync(new Uri(fullUrl));

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);

                        string avatarUrl = responseData.avatarUrl;

                        using (var imageStream = await client.GetStreamAsync(avatarUrl))
                        {
                            return Image.FromStream(imageStream);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Failed to load avatar: " + response.ReasonPhrase);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading avatar: " + ex.Message);
                return null;
            }
        }
    }
}
