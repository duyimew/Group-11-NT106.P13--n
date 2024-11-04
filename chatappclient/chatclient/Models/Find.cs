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
                foreach (string file in Directory.GetFiles(directory, fileName, SearchOption.TopDirectoryOnly))
                {
                    return file;
                }

                foreach (string subDirectory in Directory.GetDirectories(directory))
                {
                    try
                    {
                        foreach (string file in Directory.GetFiles(subDirectory, fileName, SearchOption.TopDirectoryOnly))
                        {
                            return file;
                        }

                        string result = FindFile(fileName, subDirectory);
                        if (!string.IsNullOrEmpty(result))
                        {
                            return result;
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                    }
                    catch (Exception)
                    {
                        MessageBox.Show($"Lỗi khi tìm kiếm file trong thư mục: {subDirectory}");
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (DirectoryNotFoundException)
            {
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi khi tìm kiếm file");
            }
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
