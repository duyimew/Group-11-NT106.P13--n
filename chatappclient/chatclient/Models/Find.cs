using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLUSER.Models
{
    internal class Find
    {
        public string GetProjectRootDirectory()
        {
            try
            {
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string projectDirectory = Directory.GetParent(currentDirectory).Parent.Parent.Parent.FullName;
                return projectDirectory;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy thư mục gốc của dự án: " + ex.Message);
                return null;
            }
        }

        
        private string FindFile(string fileName, string directory)
        {
            try
            {
                // Kiểm tra file trong thư mục hiện tại
                foreach (string file in Directory.GetFiles(directory, fileName, SearchOption.TopDirectoryOnly))
                {
                    return file;
                }

                // Duyệt qua từng thư mục con
                foreach (string subDirectory in Directory.GetDirectories(directory))
                {
                    try
                    {
                        // Gọi đệ quy để tìm file trong thư mục con
                        string result = FindFile(fileName, subDirectory);
                        if (!string.IsNullOrEmpty(result))
                        {
                            return result;
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        // Bỏ qua lỗi truy cập không được phép
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi tìm kiếm file trong thư mục: {subDirectory}\n{ex.Message}");
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Bỏ qua lỗi truy cập không được phép
            }
            catch (DirectoryNotFoundException)
            {
                // Bỏ qua lỗi thư mục không tìm thấy
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm file: {ex.Message}");
            }

            // Trả về null nếu không tìm thấy file
            return null;
        }

        public string find(string name)
        {
            string fileNameToSearch = name;
            string projectDirectory = GetProjectRootDirectory();
            string filePath = FindFile(fileNameToSearch, projectDirectory);

            if (!string.IsNullOrEmpty(filePath))
            {
                return filePath;
            }
            else
            {
                return null;
            }
        }
    }
}
