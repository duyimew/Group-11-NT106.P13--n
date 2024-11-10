using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using Microsoft.VisualBasic;

using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Reflection;
using QLUSER.Models;
using QLUSER.DTOs;
using System.Threading;
using System.Net.Http;
namespace QLUSER
{
    public partial class GiaoDien : Form
    {
        string username1;
        Dangnhap DN;
        Group group = new Group();
        Channel channel = new Channel();
        Models.Message message1 =new Models.Message();
        file file1 = new file();
        Token token = new Token();
        Find find1 = new Find();
        private HashSet<string> displayedMessages = new HashSet<string>();
        private bool isReceivingMessages = true;
        private List<string> selectedFilePaths = new List<string>();
        UserAvatar avatar = new UserAvatar();
        public GiaoDien(string username, Dangnhap dn)
        {
            InitializeComponent();
            username1 = username;
            DN = dn;
            _ = ReceiveMessages();
            UserSession.AvatarUpdated += UpdateAvatarDisplay;
        }

        private void UpdateAvatarDisplay()
        {
            cp_ProfilePic.ImageLocation = UserSession.AvatarUrl;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            UserSession.AvatarUpdated -= UpdateAvatarDisplay; 
        }


        private async void bt_thoat_Click(object sender, EventArgs e)
        {
            isReceivingMessages = false;
            await token.GenerateToken(username1);
            DN.Close();
            Environment.Exit(0);
            this.Close();
            
        }
        private async void lb_nhom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Nhóm.SelectedItem != null)
            {
                Tênkênh.Items.Clear();
                flowLayoutPanel1.Controls.Clear();
                flowLayoutPanel1.AutoScrollPosition = new Point(0, 0);
                flowLayoutPanel1.PerformLayout();
                label2.Text = "kênh";
                string selectedGroupName = Nhóm.SelectedItem.ToString();
                label1.Text=selectedGroupName;
                string[] channels = await channel.RequestChannelName(selectedGroupName);
                if(channels !=null)
                {
                    for (int i = 0; i < channels.Length; i++)
                        this.Tênkênh.Items.Add(channels[i]);
                }
                
            }
        }
        private async void bt_taonhom_Click(object sender, EventArgs e)
        {
            isReceivingMessages = false;
            string newGroup = Interaction.InputBox("Enter group name:", "New Group", "");
            if (!string.IsNullOrEmpty(newGroup))
            {
                Nhóm.Items.Add(newGroup);
                await group.SaveGroupToDatabase(newGroup);
                await group.AddMembersToGroup(username1, newGroup);
            }
            isReceivingMessages = true;
            _ = ReceiveMessages();
        }
        

        private async void GiaoDien_Load(object sender, EventArgs e)
        {
            string[] groupname = await group.RequestGroupName(username1);
            
                if (groupname != null)
                {
                    for (int i = 0; i < groupname.Length; i++)
                        this.Nhóm.Items.Add(groupname[i]);
                }
            UserAvatar userAvatar = new UserAvatar();
            Image avatarImage = await userAvatar.LoadAvatarAsync(username1); 

            if (avatarImage != null)
            {
                cp_ProfilePic.Image = avatarImage;
            }
        }
        
        private async void bt_guitinnhan_Click(object sender, EventArgs e)
        {
            if (label2.Text == "kênh") MessageBox.Show("Gửi tin nhắn thất bại");
            else
            {
                isReceivingMessages = false;
                string message = textBox1.Text;
                if (!string.IsNullOrEmpty(label3.Text))
                {
                    await file1.SendFileToServer(username1, label1.Text, label2.Text, message, selectedFilePaths);
                    label3.Text = string.Empty;
                }
                else if (!string.IsNullOrWhiteSpace(message))
                {
                    textBox1.Clear();
                    await message1.SendMessage(label1.Text, label2.Text, username1, message);
                }
                isReceivingMessages = true;
                _ = ReceiveMessages();
            }
        }

      

        private async void bt_taokenh_Click(object sender, EventArgs e)
        {
            isReceivingMessages = false;
            if (label1.Text == "groupname") MessageBox.Show("Tạo kênh thất bại");
            else
            {
                string newKenh = Interaction.InputBox("Enter channel name:", "New Channel", "");
                if (!string.IsNullOrEmpty(newKenh))
                {
                    Tênkênh.Items.Add(newKenh);
                    await channel.SaveKenhToDatabase(label1.Text, newKenh);
                }
            }
            isReceivingMessages = true;
            _ = ReceiveMessages();
        }

        private async void bt_moiuser_Click(object sender, EventArgs e)
        {
            isReceivingMessages = false;
            if (label1.Text == "groupname") MessageBox.Show("Mời bạn bè thất bại");
            else
            {
                string newUser = Interaction.InputBox("Enter Username:", "New User", "");
                if (!string.IsNullOrEmpty(newUser))
                {
                    await group.AddMembersToGroup(newUser, label1.Text);
                }
            }
            isReceivingMessages = true;
            _ = ReceiveMessages();
        }

        public async void lb_Tenkenh_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (Tênkênh.SelectedItem != null)
            {
                displayedMessages.Clear();
                flowLayoutPanel1.Controls.Clear();
                flowLayoutPanel1.AutoScrollPosition = new Point(0, 0);
                flowLayoutPanel1.PerformLayout();
                string selectedKenhName = Tênkênh.SelectedItem.ToString();
                label2.Text = selectedKenhName;
                string[] serverResponse = await message1.ReceiveMessage(label1.Text, label2.Text);
                    if (serverResponse != null)
                    {
                        for (int i = serverResponse.Length - 1; i > 0; i -= 4)
                        {
                            if (i - 3 < serverResponse.Length)
                            {
                                string messageid = serverResponse[i - 3];
                                string username = serverResponse[i - 2];
                                string message = serverResponse[i - 1];
                                string filename = serverResponse[i];
                                string combinedKey = messageid + "|" + username + "|" + message+"|"+ filename;
                                if (!displayedMessages.Contains(combinedKey))
                                {
                                    if (displayedMessages.Count >= 100)
                                    {
                                        displayedMessages.Remove(displayedMessages.First());
                                    }

                                    displayedMessages.Add(combinedKey);
                                    string[] filenames = filename.Split(new string[] {"; "},StringSplitOptions.RemoveEmptyEntries);
                                    AddMessageToChat(username, message, filenames);
                                }
                            }
                        }
                    }
            }
        }

        private async void AddMessageToChat(string username, string messageContent, string[] filename)
        {
            Panel panelMessage = new Panel
            {
                Width = flowLayoutPanel1.Width+40,
                BackColor = Color.FromArgb(66, 69, 73),
                Padding = new Padding(5),
                AutoSize = true,
                Margin = new Padding(10, 0, 0, 0)

            };

            CircularPicture pictureBoxAvatar = new CircularPicture
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                Width = 40, // Kích thước ảnh đại diện
                Height = 40,
                ImageLocation = UserSession.AvatarUrl,
                Image = await avatar.LoadAvatarAsync(username1),

                Margin = new Padding(0, 0, 10, 0)
            };

            UserSession.AvatarUpdated += async () =>
            {
                pictureBoxAvatar.Image = await avatar.LoadAvatarAsync(username1); // Cập nhật ảnh
            };
            panelMessage.Controls.Add(pictureBoxAvatar);

            Label lblUsername = new Label
            {
                Text = username,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(88, 101, 242),
                AutoSize = true,
                Top = 5,
                Left = pictureBoxAvatar.Right + 10
            };


            panelMessage.Controls.Add(lblUsername);
            if (!string.IsNullOrEmpty(messageContent))
            {
                Label lblMessage = new Label
                {
                    Text = $"{messageContent}",
                    Font = new Font("Segoe UI", 10),
                    ForeColor = Color.White,
                    AutoSize = true,
                    Top = lblUsername.Bottom + 5,
                    Left = pictureBoxAvatar.Right + 10

                };
                panelMessage.Controls.Add(lblMessage);
            }
            List<string> nonImageVideoFiles = new List<string>();

            if (filename != null && filename.Length > 0)
            {
                foreach (var Filename in filename)
                {
                    
                    string fileExtension = System.IO.Path.GetExtension(Filename).ToLower();

                    if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png" ||
                        fileExtension == ".bmp" || fileExtension == ".gif")
                    {
                        string filepath = await file1.GetFileUrlAsync(Filename);
                        PictureBox pictureBox = new PictureBox
                        {
                            SizeMode = PictureBoxSizeMode.Zoom,
                            Width = 200,
                            Height = 150,
                            Top = panelMessage.Controls.Count > 1 ? panelMessage.Controls[1].Bottom + 5 : lblUsername.Bottom + 5,
                            Left = pictureBoxAvatar.Right + 10
                        };
                        using (HttpClient client = new HttpClient())
                        {
                            try
                            {
                                byte[] imageBytes = await client.GetByteArrayAsync(filepath);
                                using (MemoryStream ms = new MemoryStream(imageBytes))
                                {
                                    pictureBox.Image = Image.FromStream(ms);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Lỗi khi tải ảnh: " + ex.Message);
                            }
                        }
                        pictureBox.Click += (sender, e) =>
                        {
                            if (MessageBox.Show("Bạn có muốn tải xuống hình ảnh này không?", "Download Image", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                isReceivingMessages = false;
                                file1.DownloadFile(Filename);
                                isReceivingMessages = true;
                            }
                        };

                        panelMessage.Controls.Add(pictureBox);
                        panelMessage.Height += pictureBox.Height + 10;
                    }
                    else
                    {
                        nonImageVideoFiles.Add(Filename);
                    }
                }
            }

            if (nonImageVideoFiles.Count > 0)
            {
                ListView listViewFiles = new ListView
                {
                    View = View.List,
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(47, 49, 54),
                    Width = panelMessage.Width - 10,
                    Height = 40,
                    Scrollable = false,
                    Top = panelMessage.Controls.Count > 1 ? panelMessage.Controls[1].Bottom + 5 : lblUsername.Bottom + 5
                };
                foreach (var FileName in nonImageVideoFiles)
                {
                    listViewFiles.Items.Add(new ListViewItem(FileName)
                    {
                        Tag = FileName
                    });
                }

                listViewFiles.ItemActivate += (sender, e) =>
                {
                    var selectedItem = listViewFiles.SelectedItems[0];
                    string selectedFileName = (string)selectedItem.Tag;

                    if (MessageBox.Show("Bạn có muốn tải xuống tệp này không?", "Download File", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        isReceivingMessages = false;
                        file1.DownloadFile(selectedFileName);
                        isReceivingMessages = true;
                    }
                };

                panelMessage.Controls.Add(listViewFiles);
                panelMessage.Height += listViewFiles.Height + 10;
            }

            flowLayoutPanel1.Controls.Add(panelMessage);
            flowLayoutPanel1.ScrollControlIntoView(panelMessage);
        }


        private async Task ReceiveMessages()
        {
            while (isReceivingMessages)
            {
                if (label1.Text != "groupname" && label2.Text != "kênh")
                {
                    string[] serverResponse = await message1.ReceiveMessage(label1.Text, label2.Text);
                   
                    
                        if (serverResponse != null)
                        {
                            for (int i = serverResponse.Length - 1; i > 0; i -= 4)
                            {
                                if (i - 3 < serverResponse.Length)
                                {
                                    string messageid = serverResponse[i - 3];
                                    string username = serverResponse[i - 2];
                                    string message = serverResponse[i - 1];
                                    string filepath = serverResponse[i];
                                    string combinedKey = messageid + "|" + username + "|" + message + "|" + filepath;
                                    if (!displayedMessages.Contains(combinedKey))
                                    {
                                        if (displayedMessages.Count >= 100)
                                        {
                                            displayedMessages.Remove(displayedMessages.First());
                                        }

                                        displayedMessages.Add(combinedKey);
                                        string[] filepaths = filepath.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries);
                                        AddMessageToChat(username, message, filepaths);
                                    }
                                }
                            }
                        }
                }
                await Task.Delay(2000);
            }
        }
        private void buttonchonfile_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Multiselect = true,
                    Filter = "Document Files|*.txt;*.pdf;*.docx;*.xlsx;*.pptx;*.csv;*.xml;*.json;*.html;*.css|All Files (*.*)|*.*"
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (openFileDialog.FileNames.Length > 5)
                    {
                        this.Invoke(new Action(() =>
                        {
                            MessageBox.Show("Bạn chỉ được chọn tối đa 5 file. Vui lòng chọn lại.");
                        }));
                    }
                    else
                    {
                        string[] selectedFiles = openFileDialog.FileNames;
                        this.Invoke(new Action(() =>
                        {
                            selectedFilePaths.Clear();
                            selectedFilePaths.AddRange(selectedFiles);
                            label3.Text = string.Join(", ", openFileDialog.SafeFileNames);
                        }));
                    }
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

       

        

        private void buttonchonanh_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Multiselect = false,
                    Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Video Files|*.mp4;*.avi;*.mkv;*.mov;*.wmv;*.flv",
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFile = openFileDialog.FileName;
                    this.Invoke(new Action(() =>
                    {
                        selectedFilePaths.Clear();
                        selectedFilePaths.Add(selectedFile);
                        label3.Text = openFileDialog.SafeFileName;
                    }));
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Formuser formuser = new Formuser(username1, DN, this);
            formuser.Show();
        }

        private void btnFriends_Click(object sender, EventArgs e)
        {
            SearchUser searchForm = new SearchUser(username1);
            searchForm.Show();
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
          

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button3.PerformClick();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
