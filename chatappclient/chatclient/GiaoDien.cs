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
using Microsoft.AspNetCore.SignalR.Client;
using System.Configuration;
using AForge.Video;
using NAudio.Wave;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.ApplicationServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
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
        private HubConnection connection=null;
        private bool isstop=false;
        private bool isRecevie = false;
        private TreeNode chatNode; // Khai báo biến toàn cục
        private TreeNode videoNode; // Khai báo biến toàn cục
        private Button createChatChannelButton; // Khai báo nút toàn cục
        private Button createVideoChannelButton; // Khai báo nút toàn cục
        public GiaoDien(string username, Dangnhap dn)
        {
            InitializeComponent();
            username1 = username;
            DN = dn;
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
            if (connection != null)
                await StopSignalR();
            await token.GenerateToken(username1);
            DN.Close();
            Environment.Exit(0);
            this.Close();
            
        }
        private async void lb_nhom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Nhóm.SelectedItem != null)
            {
                if (connection != null) await StopSignalR();
                treeView1.Nodes.Clear();
                flowLayoutPanel1.Controls.Clear();
                flowLayoutPanel1.AutoScrollPosition = new Point(0, 0);
                flowLayoutPanel1.PerformLayout();
                label2.Text = "kênh";
                string selectedGroupName = Nhóm.SelectedItem.ToString();
                label1.Text = selectedGroupName;
                chatNode = new TreeNode("Chat");
                videoNode = new TreeNode("Video");
                treeView1.Nodes.Add(chatNode);
                treeView1.Nodes.Add(videoNode);
                label4.Visible = true;
                label5.Visible = true;
                string[] channels = await channel.RequestChannelName(selectedGroupName);
                if (channels != null)
                {
                    for (int i = 0; i < channels.Length; i++)
                    {
                        string[] channel = channels[i].Split('|');
                        if (channel[1] == "True") chatNode.Nodes.Add(channel[0]);
                        else if (channel[1] == "False") videoNode.Nodes.Add(channel[0]);
                    }
                }
                await change();
                treeView1.AfterExpand += TreeView_AfterExpandCollapse;
                treeView1.AfterCollapse += TreeView_AfterExpandCollapse;
            }
        }

        private async void TreeView_AfterExpandCollapse(object sender, TreeViewEventArgs e)
        {
            await change();
        }
        private async Task change()
        {
            int videoY = videoNode.Bounds.Location.Y;
            Point point = label4.Location;
            int x = point.X;
            int y = point.Y + videoY;
            label5.Location = new Point(x, y);
        }
        private async void bt_taonhom_Click(object sender, EventArgs e)
        {
            string newGroup = Interaction.InputBox("Enter group name:", "New Group", "");
            if (!string.IsNullOrEmpty(newGroup))
            {
                Nhóm.Items.Add(newGroup);
                await group.SaveGroupToDatabase(newGroup);
                await group.AddMembersToGroup(username1, newGroup);
                await channel.SaveKenhToDatabase(newGroup, "Chung", true);
                await channel.SaveKenhToDatabase(newGroup, "chung", false);
            }
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
                string message = textBox1.Text;
                if (!string.IsNullOrEmpty(label3.Text))
                {
                    textBox1.Clear();
                    string[] filenames = selectedFilePaths.Select(Path.GetFileName).ToArray();
                    while (true)
                    {
                        if (connection != null && connection.State == HubConnectionState.Connected)
                        {
                            await connection.SendAsync("SendMessage", username1, label1.Text, label2.Text, message, filenames);
                            break;
                        }
                    }
                    await connection.SendAsync("SendMessage", username1, label1.Text, label2.Text, message, filenames);
                    await file1.SendFileToServer(connection,username1, label1.Text, label2.Text, message, selectedFilePaths);
                    label3.Text = string.Empty;
                }
                else if (!string.IsNullOrWhiteSpace(message))
                {
                    textBox1.Clear();
                    while (true)
                    {
                        if (connection!=null && connection.State == HubConnectionState.Connected)
                        {
                            await connection.SendAsync("SendMessage", username1, label1.Text, label2.Text, message, null);
                            break;
                        }
                    }
                    await message1.SendMessage(connection, label1.Text, label2.Text, username1, message);
                }
            }
        }

      



        private async void bt_moiuser_Click(object sender, EventArgs e)
        {
            if (label1.Text == "groupname") MessageBox.Show("Mời bạn bè thất bại");
            else
            {
                string newUser = Interaction.InputBox("Enter Username:", "New User", "");
                if (!string.IsNullOrEmpty(newUser))
                {
                    await group.AddMembersToGroup(newUser, label1.Text);
                }
            }
        }

        

        private async Task AddMessageToChat(string username, string messageContent, string[] filename)
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
                Image = await avatar.LoadAvatarAsync(username),

                Margin = new Padding(0, 0, 10, 0)
            };

            UserSession.AvatarUpdated += async () =>
            {
                pictureBoxAvatar.Image = await avatar.LoadAvatarAsync(username1);
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
        }
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

        private void button7_Click(object sender, EventArgs e)
        {
            if (label1.Text != "groupname" && label2.Text != "kênh")
            {
                VideoCall vc = new VideoCall(username1, label1.Text, label2.Text);
                vc.Show();
            }
        }
        private async Task InitializeSignalR()
        {
            connection = new HubConnectionBuilder()
            .WithUrl(ConfigurationManager.AppSettings["HubUrl"] + "messageHub")
            .ConfigureLogging(logging =>
            {
            logging.AddConsole();
            logging.SetMinimumLevel(LogLevel.Debug);
            })
            .Build();
            await connection.StartAsync();
            await connection.SendAsync("JoinGroup", label1.Text,label2.Text , username1);
            connection.On<string,string,string[]>("ReceiveMessage", async (message, username, filenames) =>
            {
                try
                {
                    isRecevie = true;
                    this.Invoke(new Action(async () =>
                    {
                        await AddMessageToChat(username, message, filenames);
                    }));
                    isRecevie = false;
                }
                catch(Exception ex)
                { MessageBox.Show(ex.Message); }

            });
            connection.Closed += async (exception) =>
            {
                while (true)
                {
                    try
                    {
                        if (!isstop)
                            await ReconnectSignalR();
                        break;
                    }
                    catch (Exception reconnectEx)
                    {
                        MessageBox.Show("Reconnect failed: " + reconnectEx.Message);
                    }
                }
            };

        }
        private async Task ReconnectSignalR()
        {
            try
            {
                if (connection.State != HubConnectionState.Connected)
                {
                    await connection.StartAsync();
                    await connection.SendAsync("JoinGroup", label1.Text, label2.Text, username1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Reconnection failed: " + ex.Message);
            }
        }
        private async Task StopSignalR()
        {
            if (connection != null)
            {
                try
                {
                    await connection.SendAsync("LeaveGroup", label1.Text, label2.Text, username1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error sending LeaveCall: " + ex.Message);
                }
            }
            await Task.Delay(100);
            while (true)
            {
                if (!isRecevie)
                {
                    if (connection != null)
                    {
                        isstop = true;
                        await connection.StopAsync();
                        await connection.DisposeAsync();
                        connection = null;
                    }
                    break;
                }
            }
        }

        private async void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selectedNode = e.Node;
            string parentName;
            if (selectedNode != null)
            {
                if (selectedNode.Parent != null)
                {
                    parentName = selectedNode.Parent.Text;
                }
                else
                {
                    return;
                }
                if (parentName == "Chat")
                {
                    if (connection != null)
                        await StopSignalR();
                    displayedMessages.Clear();
                    flowLayoutPanel1.Controls.Clear();
                    flowLayoutPanel1.AutoScrollPosition = new Point(0, 0);
                    flowLayoutPanel1.PerformLayout();
                    string selectedKenhName = selectedNode.Text.ToString();
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
                                string combinedKey = messageid + "|" + username + "|" + message + "|" + filename;
                                if (!displayedMessages.Contains(combinedKey))
                                {
                                    if (displayedMessages.Count >= 100)
                                    {
                                        displayedMessages.Remove(displayedMessages.First());
                                    }

                                    displayedMessages.Add(combinedKey);
                                    string[] filenames = filename.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries);
                                    await AddMessageToChat(username, message, filenames);
                                }
                            }
                        }
                    }
                    await InitializeSignalR();
                }
                else if(parentName =="Video")
                {
                    VideoCall vc = new VideoCall(username1,label1.Text,label2.Text);
                    vc.Show();
                    if (connection != null)
                        await StopSignalR();
                    displayedMessages.Clear();
                    flowLayoutPanel1.Controls.Clear();
                    flowLayoutPanel1.AutoScrollPosition = new Point(0, 0);
                    flowLayoutPanel1.PerformLayout();
                    string selectedKenhName = selectedNode.Text.ToString();
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
                                string combinedKey = messageid + "|" + username + "|" + message + "|" + filename;
                                if (!displayedMessages.Contains(combinedKey))
                                {
                                    if (displayedMessages.Count >= 100)
                                    {
                                        displayedMessages.Remove(displayedMessages.First());
                                    }

                                    displayedMessages.Add(combinedKey);
                                    string[] filenames = filename.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries);
                                    await AddMessageToChat(username, message, filenames);
                                }
                            }
                        }
                    }
                    await InitializeSignalR();
                }
            }
        }



        private async void label4_Click_1(object sender, EventArgs e)
        {
            if (label1.Text == "groupname") MessageBox.Show("Tạo kênh thất bại");
            else
            {
                string newKenh = Interaction.InputBox("Enter channel name:", "New Channel", "");
                if (!string.IsNullOrEmpty(newKenh))
                {
                    TreeNode generalNode = new TreeNode(newKenh);
                    chatNode.Nodes.Add(generalNode);
                    await change();
                    await channel.SaveKenhToDatabase(label1.Text, newKenh,true);
                }
            }
        }

        private async void label5_Click(object sender, EventArgs e)
        {
            if (label1.Text == "groupname") MessageBox.Show("Tạo kênh thất bại");
            else
            {
                string newKenh = Interaction.InputBox("Enter channel name:", "New Channel", "");
                if (!string.IsNullOrEmpty(newKenh))
                {
                    TreeNode meetingNode = new TreeNode(newKenh);
                    videoNode.Nodes.Add(meetingNode);
                    await change();
                    await channel.SaveKenhToDatabase(label1.Text, newKenh,false);
                }
            }
        }
    }
}
