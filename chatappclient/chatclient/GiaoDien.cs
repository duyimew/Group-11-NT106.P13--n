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
using System.Xml.Linq;


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
        private FlowLayoutPanel panelMenu; // Lưu tham chiếu đến menu
        private bool isMenuVisible = false; // Trạng thái hiển thị menu
        DanhMuc danhmuc = new DanhMuc();
        private bool isFirstClick = true;
        public GiaoDien(string username, Dangnhap dn)
        {
            InitializeComponent();
            username1 = username;
            DN = dn;
            UserSession.AvatarUpdated += UpdateAvatarDisplay;
            UserSession.AvatarGroupCreated += UpdateGroupDislay;
            tabControl1.SelectedIndex = 1;
        }

        private async void UpdateAvatarDisplay()
        {
            cp_ProfilePic.Image = await avatar.LoadAvatarAsync(username1);
            while (true)
            {
                if (connection != null && connection.State == HubConnectionState.Connected)
                {
                    await connection.SendAsync("SendAvataUpdate", UserSession.AvatarUrl,label1.Text,label2.Text);
                    break;
                }
            }
        }
        private async void UpdateGroupDislay()
        {
            flowLayoutPanel2.Controls.Clear();
            flowLayoutPanel2.AutoScrollPosition = new Point(0, 0);
            flowLayoutPanel2.PerformLayout();
            string[] groupname = await group.RequestGroupName(username1);
            LoadGroup(groupname);
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
        

        private async void TreeView_AfterExpandCollapse(object sender, TreeViewEventArgs e)
        {
            await ChangeLabelLocation();
        }
        private async Task ChangeLabelLocation()
        {
            var matchedNodes = treeView1.Nodes
            .Cast<TreeNode>()
            .Where(node => node.Name.Contains("danhmuc"))
            .ToList();
            foreach (var node in matchedNodes)
            {
                Label label = (Label)node.Tag;
                int x = treeView1.Location.X + treeView1.Size.Width - label.Size.Width - 1;
                int nodeY = node.Bounds.Location.Y;
                int y = treeView1.Location.Y + 1 + nodeY;
                label.Location = new Point(x, y);
            }
        }
        private async Task CloseLabel()
        {
            var matchedNodes = treeView1.Nodes
            .Cast<TreeNode>()
            .Where(node => node.Name.Contains("danhmuc"))
            .ToList();
            foreach (var node in matchedNodes)
            {
                Label label = (Label)node.Tag;
                label.Parent?.Controls.Remove(label);
                label.Dispose();
                node.Tag = null;
            }
        }
        private async void GiaoDien_Load(object sender, EventArgs e)
        {
            string[] groupname = await group.RequestGroupName(username1);
            LoadGroup(groupname);
            UserAvatar userAvatar = new UserAvatar();
            Image avatarImage = await userAvatar.LoadAvatarAsync(username1); 

            if (avatarImage != null)
            {
                cp_ProfilePic.Image = avatarImage;
            }
        }
        private async void LoadGroup(string[] groupname)
        {
            //CircularPicture circularfriend = new CircularPicture();
            //try
            //{
            //    circularfriend.Image = global::QLUSER.Properties.Resources._379512_chat_icon;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Không thể tải ảnh: {ex.Message}");
            //    return;
            //}
            //circularfriend.Size = new Size(50, 50);
            //circularfriend.SizeMode = PictureBoxSizeMode.Zoom;
            //circularfriend.Anchor = AnchorStyles.None;
            //circularfriend.Click += (s, e) =>
            //{
            //    SearchUser searchForm = new SearchUser(username1);
            //    searchForm.Show();
            //};
            //flowLayoutPanel2.Controls.Add(circularfriend);
            if (groupname != null)
            {
                for (int i = 0; i < groupname.Length; i++)
                {
                    CircularPicture circulargroup = new CircularPicture();
                    try
                    {
                        circulargroup.Image = await avatar.LoadAvatarGroupAsync(groupname[i]);
                        circulargroup.SizeMode = PictureBoxSizeMode.Zoom;
                        circulargroup.Name=groupname[i];
                        UserSession.AvatarGroupUpdated += async () =>
                        {
                            circulargroup.Image = await avatar.LoadAvatarGroupAsync(groupname[i]);
                            circulargroup.Name = groupname[i];
                        };
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Không thể tải ảnh: {ex.Message}");
                        return;
                    }
                    circulargroup.Size = new Size(50, 50);

                    circulargroup.Anchor = AnchorStyles.None;
                    circulargroup.Click += async (s, e) =>
                    {
                            tabControl1.SelectedIndex = 0;
                        ServerOrFriend.SelectedIndex = 1;
                            if (connection != null) await StopSignalR();
                            CloseLabel();
                            treeView1.Nodes.Clear();
                            flowLayoutPanel1.Controls.Clear();
                            flowLayoutPanel1.AutoScrollPosition = new Point(0, 0);
                            flowLayoutPanel1.PerformLayout();
                            label2.Text = "kênh";
                            string selectedGroupName = circulargroup.Name.ToString();
                            label1.Text = selectedGroupName;
                            label6.Text = selectedGroupName;
                            string[] danhmucs = await danhmuc.RequestDanhMucName(selectedGroupName);
                            if(danhmucs!=null)
                            {
                            for (int ib = 0; ib < danhmucs.Length; ib++)
                            {
                                TreeNode node =treeView1.Nodes.Add($"danhmuc|{danhmucs[ib]}",danhmucs[ib]);
                                createlabel(node);
                            }
                            }
                            string[] channels = await channel.RequestChannelName(selectedGroupName);
                            if (channels != null)
                            {
                                for (int ia = 0; ia < channels.Length; ia++)
                                {
                                    string[] channel = channels[ia].Split('|');
                                if (channel[2] == "True")
                                {
                                    if (!string.IsNullOrEmpty(channel[1]))
                                    {
                                        TreeNode[] nodes = treeView1.Nodes.Find($"danhmuc|{channel[1]}", false);
                                        if (nodes != null) nodes[0].Nodes.Add($"VanBanChat|{channel[0]}",channel[0]);
                                    }
                                    else treeView1.Nodes.Add($"VanBanChat|{channel[0]}", channel[0]);
                                }
                                else if (channel[2] == "False")
                                {
                                    if (channel[1] != null)
                                    {
                                        TreeNode[] nodes = treeView1.Nodes.Find($"danhmuc|{channel[1]}", false);
                                        if (nodes != null) nodes[0].Nodes.Add($"CuocGoiVideo|{channel[0]}", channel[0]);
                                    }
                                    else treeView1.Nodes.Add($"CuocGoiVideo|{channel[0]}", channel[0]);
                                }
                                }
                            }
                            treeView1.AfterExpand += TreeView_AfterExpandCollapse;
                            treeView1.AfterCollapse += TreeView_AfterExpandCollapse;
                        
                    };
                    flowLayoutPanel2.Controls.Add(circulargroup);
                }
            }
            
            CircularPicture circularadd = new CircularPicture();

            try
            {
                circularadd.Image = global::QLUSER.Properties.Resources.add_icon_png_14;
                circularadd.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể tải ảnh: {ex.Message}");
                return;
            }
            circularadd.Size = new Size(50, 50);

            circularadd.Anchor = AnchorStyles.None;
            circularadd.Click += (s, e) =>
            {
                circularadd_Click();
            };
            flowLayoutPanel2.Controls.Add(circularadd);

            flowLayoutPanel2.FlowDirection = FlowDirection.LeftToRight; 
            flowLayoutPanel2.WrapContents = true;
            //flowLayoutPanel2.AutoScroll = true;
            flowLayoutPanel2.MouseWheel += FlowLayoutPanel2_MouseWheel;

            flowLayoutPanel2.Padding = new Padding((flowLayoutPanel2.ClientSize.Width - 50) / 2, 10, 0, 0); 
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
                Width = 40,
                Height = 40,
                ImageLocation = UserSession.AvatarUrl,
                Image = await avatar.LoadAvatarAsync(username),

                Margin = new Padding(0, 0, 10, 0)
            };
            pictureBoxAvatar.Click += (s, e) =>
            {

                USERINFOR(username);
            };
            UserSession.AvatarUpdated += async () =>
            {
                pictureBoxAvatar.Image = await avatar.LoadAvatarAsync(username);
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



        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button3.PerformClick();
                e.Handled = true;
                e.SuppressKeyPress = true;
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
            connection.On<string>("ReceiveAvataUpdate", async (url) =>
            {
                UserSession.AvatarUrl = url;
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
            if (selectedNode != null)
            {
                
                if (selectedNode.Name.Contains("VanBanChat"))
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
                else if(selectedNode.Name.Contains("CuocGoiVideo"))
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
        private void CreateMenu()
        {
            if (panelMenu == null)
            {
                panelMenu = new FlowLayoutPanel();
                panelMenu.Dock = DockStyle.Fill;
                panelMenu.BackColor = Color.FromArgb(47, 49, 54);
                panelMenu.FlowDirection = FlowDirection.TopDown;
                panelMenu.WrapContents = false;
                panelMenu.AutoScroll = true;

                panel11.Controls.Add(panelMenu);
                AddMenuItem(panelMenu, "Mời Mọi Người");
                AddMenuItem(panelMenu, "Cài đặt máy chủ");
                AddMenuItem(panelMenu, "Tạo kênh");
                AddMenuItem(panelMenu, "Tạo Danh Mục");
                AddMenuItem(panelMenu, "Chỉnh Sửa Hồ Sơ Máy Chủ");
            }

        }


        private void AddMenuItem(FlowLayoutPanel parentPanel, string text)
        {
            Panel menuItem = new Panel();
            menuItem.Height = 25;
            menuItem.Width = parentPanel.Width;
            menuItem.BackColor = Color.FromArgb(54, 57, 63);
            menuItem.Padding = new Padding(10, 0, 0, 0);

            Label label = new Label();
            label.Text = text;
            label.ForeColor = Color.White;
            label.Font = new Font("Arial", 10, FontStyle.Regular);
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.Click += (s, e) =>
            {
                switch(text)
                {
                    case "Mời Mọi Người": MoiMoiNguoi(label1.Text); break;
                    case "Tạo kênh": TaoKenh(null,null); break;
                    case "Tạo Danh Mục": TaoDanhMuc(); break;
                }
            };
            menuItem.Controls.Add(label);
            parentPanel.Controls.Add(menuItem);
        }


        private void button5_Click_1(object sender, EventArgs e)
        {
                ServerOrFriend.SelectedIndex = 2;
                CreateMenu();   
            

        }
        private void MoiMoiNguoi(string group)
        {
            Form form = new Form();
            form.Text = $"Mời bạn bè vào {group}";
            form.Size = new Size(400, 200);
            form.StartPosition = FormStartPosition.CenterParent;

            Label label = new Label();
            label.Text = $"Mời bạn bè vào nhóm: {group}";
            label.Font = new Font("Arial", 12, FontStyle.Bold);
            label.ForeColor = Color.Black;
            label.AutoSize = true;
            label.Location = new Point(20, 20);

            TextBox textbox = new TextBox();
            textbox.Text = $"{group}";
            textbox.Width = 340;
            textbox.Location = new Point(20, 60);
            textbox.ReadOnly = true;

            Button btnCopy = new Button();
            btnCopy.Text = "Sao chép";
            btnCopy.Location = new Point(20, 100);
            btnCopy.Click += (s, e) =>
            {
                Thread staThread = new Thread(() =>
                {
                    try
                    {
                        Clipboard.SetText(textbox.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi sao chép: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                });
                staThread.SetApartmentState(ApartmentState.STA);
                staThread.Start();
            };
            Button btnClose = new Button();
            btnClose.Text = "Đóng";
            btnClose.Location = new Point(120, 100);
            btnClose.Click += (s, e) => form.Close();
            form.Controls.Add(label);
            form.Controls.Add(textbox);
            form.Controls.Add(btnCopy);
            form.Controls.Add(btnClose);
            form.ShowDialog();
        }
        private void circularadd_Click()
        {
            Form form = new Form();
            form.Text = $"Tạo máy chủ của bạn";
            form.Size = new Size(400, 300);
            form.StartPosition = FormStartPosition.CenterParent;
            Label label = new Label();
            label.Text = $"Tạo máy chủ của bạn";
            label.Font = new Font("Arial", 12, FontStyle.Bold);
            label.ForeColor = Color.Black;
            label.AutoSize = true;

            label.PerformLayout();
            int labelWidth = TextRenderer.MeasureText(label.Text, label.Font).Width; 
            label.Location = new Point((form.ClientSize.Width - labelWidth) / 2, 50);

            label.TextAlign = ContentAlignment.MiddleCenter;

            Button btncreate = new Button();
            btncreate.Text = "Tạo Group mới";
            btncreate.Location = new Point(20, 100);
            btncreate.Size = new Size(340,30);
            btncreate.Click += (s, e) =>
            {
                form.Hide();
                creategroup(form);
            };
            Button btnClose = new Button();
            btnClose.Text = "Đóng";
            btnClose.Location = new Point(300, 20);
            btnClose.Click += (s, e) => form.Close();

            Label label1 = new Label();
            label1.Text = $"Bạn đã nhận được lời mời";
            label1.Font = new Font("Arial", 12, FontStyle.Bold);
            label1.ForeColor = Color.Black;
            label1.AutoSize = true;

            label1.PerformLayout(); 
            int labelWidth1 = TextRenderer.MeasureText(label1.Text, label1.Font).Width; 
            label1.Location = new Point((form.ClientSize.Width - labelWidth1) / 2, 150); 

            label1.TextAlign = ContentAlignment.MiddleCenter;

            Button btnjoin = new Button();
            btnjoin.Text = "Tham gia group";
            btnjoin.Location = new Point(20, 200);
            btnjoin.Size = new Size(340, 30);
            btnjoin.Click += (s, e) =>
            {
                form.Hide();
                joingroup(form);
            };
            form.Controls.Add(label);
            form.Controls.Add(btncreate);
            form.Controls.Add(btnClose);
            form.Controls.Add(label1);
            form.Controls.Add(btnjoin);
            form.Show();
        }
        private async Task creategroup(Form form1)
        {
            Image image = global::QLUSER.Properties.Resources.group_1824145_1280;
            string imagepath = find1.find("group-1824145_1280.png");
            Form form = new Form();
            form.Text = $"Tùy chỉnh máy chủ của bạn"; 
            form.Size = new Size(600, 400);
            form.StartPosition = FormStartPosition.CenterParent;
            Label label = new Label();
            label.Text = $"Tùy chỉnh máy chủ của bạn";
            label.Font = new Font("Arial", 12, FontStyle.Bold);
            label.ForeColor = Color.Black;
            label.AutoSize = true;

            label.PerformLayout();
            int labelWidth = TextRenderer.MeasureText(label.Text, label.Font).Width;
            label.Location = new Point((form.ClientSize.Width - labelWidth) / 2, 20);

            label.TextAlign = ContentAlignment.MiddleCenter;

            CircularPicture grouppicture = new CircularPicture();
            try
            {
                grouppicture.Image = global::QLUSER.Properties.Resources.Upload_Icon_Logo_PNG_Clipart_Background;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể tải ảnh: {ex.Message}");
                return;
            }
            grouppicture.SizeMode = PictureBoxSizeMode.Zoom;
            grouppicture.Location = new Point((form.ClientSize.Width - grouppicture.Width) / 2, 50);
            grouppicture.Size = new Size(100, 100);
            grouppicture.Anchor = AnchorStyles.None;
            grouppicture.Click += async (s, e) =>
            {
                Dictionary<Image, string> imagePaths = await doianh();

                if (imagePaths != null && imagePaths.Count > 0)
                {
                    Image selectedImage = imagePaths.Keys.First();
                    grouppicture.Image = selectedImage;

                    image = selectedImage;

                    imagepath = imagePaths[selectedImage];
                }

            };

            Label tenmaychu = new Label();
            tenmaychu.Text = $"Tên máy chủ";
            tenmaychu.Font = new Font("Arial", 12, FontStyle.Bold);
            tenmaychu.ForeColor = Color.Black;
            tenmaychu.AutoSize = true;

            tenmaychu.PerformLayout();
            tenmaychu.Location = new Point(20, 200);
            tenmaychu.TextAlign = ContentAlignment.MiddleCenter;

            TextBox ten = new TextBox();
            ten.Text = $"máy chủ của {username1}";
            ten.Font = new Font("Arial", 12, FontStyle.Bold);
            ten.ForeColor = Color.Black;
            ten.Size = new Size(540,30);

            ten.PerformLayout(); 
            ten.Location = new Point(20, 250);

            Button btncreate = new Button();
            btncreate.Text = "Tạo";
            btncreate.AutoSize = AutoSize;
            btncreate.Location = new Point(560 - btncreate.Width, 320);
            btncreate.Click += async (s, e) =>
            {
                if (!string.IsNullOrEmpty(ten.Text))
                {
                    bool dk = await group.SaveGroupToDatabase(ten.Text);
                    if (dk)
                    {
                        await group.AddMembersToGroup(username1, ten.Text);
                        await danhmuc.SaveDanhMucToDatabase(ten.Text, "Chat");
                        await danhmuc.SaveDanhMucToDatabase(ten.Text, "Video");
                        await channel.SaveKenhToDatabase(ten.Text, "ChatChung", true,"Chat");
                        await channel.SaveKenhToDatabase(ten.Text, "Videochung", false,"Video");
                        string avatargroupUrl = await avatar.UploadAvatarGroupAsync(imagepath, ten.Text);
                        if (avatargroupUrl != null)
                        {
                            UserSession.AvatarGroupUrl = (avatargroupUrl, true);
                            MessageBox.Show("Avatar group uploaded successfully!");
                        }
                        else
                        {
                            MessageBox.Show("Failed to upload avatar.");
                        }
                        form.Close();
                        form1.Close();
                    }

                }
            };
            Button btnClose = new Button();
            btnClose.Text = "Trở lại";
            btnClose.Location = new Point(20, 320);
            btnClose.AutoSize = AutoSize;
            btnClose.Click += (s, e) =>
            {
                form1.Show();
                form.Close();
            };
            form.Controls.Add(label);
            form.Controls.Add(btnClose);
            form.Controls.Add(tenmaychu);
            form.Controls.Add(ten);
            form.Controls.Add(grouppicture);
            form.Controls.Add(btnClose);
            form.Controls.Add(btncreate);
            form.Show();

        }
        private async Task joingroup(Form form1)
        {

            Form form = new Form();
            form.Text = $"Tham gia máy chủ";
            form.Size = new Size(600, 200);
            form.StartPosition = FormStartPosition.CenterParent;
            Label label = new Label();
            label.Text = $"Tham gia máy chủ";
            label.Font = new Font("Arial", 12, FontStyle.Bold);
            label.ForeColor = Color.Black;
            label.AutoSize = true;

            label.PerformLayout();
            int labelWidth = TextRenderer.MeasureText(label.Text, label.Font).Width;
            label.Location = new Point((form.ClientSize.Width - labelWidth) / 2, 20);

            label.TextAlign = ContentAlignment.MiddleCenter;

            

            Label tenmaychu = new Label();
            tenmaychu.Text = $"Liên kết mời";
            tenmaychu.Font = new Font("Arial", 12, FontStyle.Bold);
            tenmaychu.ForeColor = Color.Black;
            tenmaychu.AutoSize = true;

            tenmaychu.PerformLayout();
            tenmaychu.Location = new Point(20, 50);
            tenmaychu.TextAlign = ContentAlignment.MiddleCenter;

            TextBox ten = new TextBox();
            ten.Text = $"";
            ten.Font = new Font("Arial", 12, FontStyle.Bold);
            ten.ForeColor = Color.Black;
            ten.Size = new Size(540, 30);

            ten.PerformLayout();
            ten.Location = new Point(20, 80);

            Button btnjoin = new Button();
            btnjoin.Text = "Tham gia máy chủ";
            btnjoin.AutoSize = AutoSize;
            btnjoin.Location = new Point(560 - btnjoin.Width, 120);
            btnjoin.Click += async (s, e) =>
            {
                if (!string.IsNullOrEmpty(ten.Text))
                {
                   
                        if (!string.IsNullOrEmpty(username1))
                        {
                            bool add = await group.AddMembersToGroup(username1, ten.Text);
                            if (add)
                            {
                                string avatargroupUrl = await avatar.LoadGroupUrlAsync(ten.Text);
                                if (avatargroupUrl != null)
                                {
                                    UserSession.AvatarGroupUrl = (avatargroupUrl, true);
                                }
                                form.Close();
                                form1.Close();
                            }
                        }
                }
            };
            Button btnClose = new Button();
            btnClose.Text = "Trở lại";
            btnClose.Location = new Point(20, 120);
            btnClose.AutoSize = AutoSize;
            btnClose.Click += (s, e) =>
            {
                form1.Show();
                form.Close();
            };
            form.Controls.Add(label);
            form.Controls.Add(btnClose);
            form.Controls.Add(tenmaychu);
            form.Controls.Add(ten);
            form.Controls.Add(btnjoin);
            form.Show();

        }
        private async Task<Dictionary<Image, string>> doianh()
        {
            Dictionary<Image, string> imagePaths = new Dictionary<Image, string>();
            var tcs = new TaskCompletionSource<Dictionary<Image, string>>();

            Thread thread = new Thread(() =>
            {
                try
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog
                    {
                        Multiselect = false,
                        Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                    };

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string selectedFile = openFileDialog.FileName;
                        Image img = Image.FromFile(selectedFile);

                        imagePaths.Add(img, selectedFile);
                        tcs.SetResult(imagePaths);
                    }
                    else
                    {
                        tcs.SetResult(imagePaths);
                    }
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            imagePaths = await tcs.Task;

            return imagePaths;
        }
        private void TaoKenh(string danhmuc,TreeNode node)
        {
            Form form = new Form();
            form.Text = $"Tạo kênh";
            form.Size = new Size(600, 400);
            form.StartPosition = FormStartPosition.CenterParent;
            Label label = new Label();
            label.Text = $"Tạo kênh";
            label.Font = new Font("Arial", 12, FontStyle.Bold);
            label.ForeColor = Color.Black;
            label.AutoSize = true;

            label.PerformLayout(); 
            label.Location = new Point(20, 20);

            label.TextAlign = ContentAlignment.MiddleCenter;



            Label Loaikenh = new Label();
            Loaikenh.Text = $"Loại kênh";
            Loaikenh.Font = new Font("Arial", 12, FontStyle.Bold);
            Loaikenh.ForeColor = Color.Black;
            Loaikenh.AutoSize = true;

            Loaikenh.PerformLayout();
            Loaikenh.Location = new Point(20, 70);
            Loaikenh.TextAlign = ContentAlignment.MiddleCenter;

            RadioButton vanban = new RadioButton();
            vanban.Text = "Văn bản";
            vanban.Checked = true;
            vanban.Size=new Size(540,30);
            vanban.Location = new Point(20, 100);

            RadioButton giongnoi = new RadioButton();
            giongnoi.Text = "Giọng nói";
            giongnoi.Checked = false;
            giongnoi.Size = new Size(540, 30);
            giongnoi.Location = new Point(20, 140);

            Label Tenkenh = new Label();
            Tenkenh.Text = $"Tên kênh";
            Tenkenh.Font = new Font("Arial", 12, FontStyle.Bold);
            Tenkenh.ForeColor = Color.Black;
            Tenkenh.AutoSize = true;

            Tenkenh.PerformLayout();
            Tenkenh.Location = new Point(20, 190);
            Tenkenh.TextAlign = ContentAlignment.MiddleCenter;

            TextBox ten = new TextBox();
            ten.Text = $"";
            ten.Font = new Font("Arial", 12, FontStyle.Bold);
            ten.ForeColor = Color.Black;
            ten.Size = new Size(540, 30);

            ten.PerformLayout();
            ten.Location = new Point(20, 230);

            Button btncreate = new Button();
            btncreate.Text = "Tạo kênh";
            btncreate.AutoSize = AutoSize;
            btncreate.Location = new Point(560 - btncreate.Width, 280);
            btncreate.Click += async (s, e) =>
            {
                if (!string.IsNullOrEmpty(ten.Text)&& label1.Text != "groupname")
                {

                    if (!string.IsNullOrEmpty(username1))
                    {
                        if (vanban.Checked == true)
                        {
                            bool ok=await channel.SaveKenhToDatabase(label1.Text, ten.Text, true, danhmuc);
                            if (ok)
                            {
                                if (node != null)
                                {
                                    node.Nodes.Add($"VanBanChat|{ten.Text}", ten.Text);
                                }
                                else treeView1.Nodes.Add($"VanBanChat|{ten.Text}", ten.Text);
                                ChangeLabelLocation();
                                form.Close();
                            }
                        }
                        else
                        {
                            bool ok= await channel.SaveKenhToDatabase(label1.Text, ten.Text, false, danhmuc);
                            if (ok)
                            {
                                if (node != null)
                                {
                                    node.Nodes.Add($"CuocGoiVideo|{ten.Text}", ten.Text);
                                }
                                else treeView1.Nodes.Add($"CuoiGoiVideo|{ten.Text}", ten.Text);
                                ChangeLabelLocation();
                                form.Close();
                            }
                        }

                    }
                }
                else if(string.IsNullOrEmpty(ten.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên kênh.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if(label1.Text == "groupname") MessageBox.Show("Tạo kênh thất bại");
            };
            Button btnClose = new Button();
            btnClose.Text = "Hủy bỏ";
            btnClose.Location = new Point(560-btncreate.Width-btnClose.Width-20, 280);
            btnClose.AutoSize = AutoSize;
            btnClose.Click += (s, e) =>
            {
                form.Close();
            };
            form.Controls.Add(label);
            form.Controls.Add(btnClose);
            form.Controls.Add(Loaikenh);
            form.Controls.Add(vanban);
            form.Controls.Add(giongnoi);
            form.Controls.Add(ten);
            form.Controls.Add(Tenkenh);
            form.Controls.Add(btncreate);
            form.Show();

        }
        private void TaoDanhMuc()
        {
            Form form = new Form();
            form.Text = $"Tạo danh mục";
            form.Size = new Size(400, 200);
            form.StartPosition = FormStartPosition.CenterParent;
            Label label = new Label();
            label.Text = $"Tạo danh mục";
            label.Font = new Font("Arial", 12, FontStyle.Bold);
            label.ForeColor = Color.Black;
            label.AutoSize = true;
            label.PerformLayout();
            label.Location = new Point(20, 10);

            label.TextAlign = ContentAlignment.MiddleCenter;

            Label Tendanhmuc = new Label();
            Tendanhmuc.Text = $"Tên danh mục";
            Tendanhmuc.Font = new Font("Arial", 12, FontStyle.Bold);
            Tendanhmuc.ForeColor = Color.Black;
            Tendanhmuc.AutoSize = true;

            Tendanhmuc.PerformLayout();
            Tendanhmuc.Location = new Point(20, 50);
            Tendanhmuc.TextAlign = ContentAlignment.MiddleCenter;

            TextBox ten = new TextBox();
            ten.Text = $"";
            ten.Font = new Font("Arial", 12, FontStyle.Bold);
            ten.ForeColor = Color.Black;
            ten.Size = new Size(340, 30);

            ten.PerformLayout();
            ten.Location = new Point(20, 90);

            Button btncreate = new Button();
            btncreate.Text = "Tạo danh mục";
            btncreate.AutoSize = AutoSize;
            btncreate.Location = new Point(360 - btncreate.Width, 130);
            btncreate.Click += async (s, e) =>
            {
                if (!string.IsNullOrEmpty(ten.Text) && label1.Text != "groupname")
                {

                    bool ok = await danhmuc.SaveDanhMucToDatabase(label1.Text, ten.Text);
                    if (ok)
                    {
                        TreeNode node = treeView1.Nodes.Add($"danhmuc|{ten.Text}", ten.Text);
                        await createlabel(node);
                        form.Close();
                    }
                }
                else if (string.IsNullOrEmpty(ten.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên danh mục.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (label1.Text == "groupname") MessageBox.Show("Tạo danh mục thất bại");
            };
            Button btnClose = new Button();
            btnClose.Text = "Hủy bỏ";
            btnClose.Location = new Point(360 - btncreate.Width - btnClose.Width - 20, 130);
            btnClose.AutoSize = AutoSize;
            btnClose.Click += (s, e) =>
            {
                form.Close();
            };
            form.Controls.Add(label);
            form.Controls.Add(btnClose);

            form.Controls.Add(ten);
            form.Controls.Add(Tendanhmuc);
            form.Controls.Add(btncreate);
            form.Show();

        }
        private async Task createlabel(TreeNode node)
        {
            Label label = new Label();
            label.Size = new Size(13, 13);
            label.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            label.ForeColor = System.Drawing.Color.White;
            label.Size = new System.Drawing.Size(13, 13);
            label.Text = "+";
            int videoY = node.Bounds.Location.Y;
            //Point point = label4.Location;
            int x = treeView1.Location.X + treeView1.Size.Width - label.Size.Width - 1;
            int y = treeView1.Location.Y + 1 + videoY;
            label.Location = new Point(x, y);
            label.Click += (s, e) =>
            {
                TaoKenh(node.Text,node);

            };
            panel12.Controls.Add(label);
            label.BringToFront();
            node.Tag = label;
            if(panelMenu!=null) panelMenu.BringToFront();
        }

        private void cp_Menu_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1; // 1 là menu k có chat
            ServerOrFriend.SelectedIndex = 0; //0 là menu hiện danh sách bạn bè
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ServerOrFriend.SelectedIndex = 1;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void ServerTab_Click(object sender, EventArgs e)
        {

        }

        private void btn_Ketban_Click(object sender, EventArgs e)
        {
            SearchUser friend = new SearchUser(username1);
            friend.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        void FlowLayoutPanel2_MouseWheel(object sender, MouseEventArgs e)
        {
            int delta = e.Delta > 0 ? -50 : 50;

            Point currentScrollPos = flowLayoutPanel2.AutoScrollPosition;

            flowLayoutPanel2.AutoScrollPosition = new Point(
                Math.Abs(currentScrollPos.X),
                Math.Abs(currentScrollPos.Y) + delta
            );
        }

        bool on = false;
        private async void USERINFOR(string username)
        {
            Form form = new Form();
            form.Text = $"Thông tin người dùng";
            form.Size = new Size(500, 300);
            form.StartPosition = FormStartPosition.CenterParent;

            form.Deactivate += (s, e) =>
            {
                if(!on)
                form.Close();

            };
            CircularPicture userpicture = new CircularPicture();
            try
            {
                userpicture.Image = await avatar.LoadAvatarAsync(username);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể tải ảnh: {ex.Message}");
                return;
            }
            userpicture.SizeMode = PictureBoxSizeMode.Zoom;
            userpicture.Location = new Point(20, 50);
            userpicture.Size = new Size(50, 50);
            userpicture.Anchor = AnchorStyles.None;
            userpicture.Click += async (s, e) =>
            {
                GroupUser user = new GroupUser(username,username1);
                user.Show();
            };
            UserSession.AvatarUpdated += async () =>
            {
                userpicture.Image = await avatar.LoadAvatarAsync(username);
            };

            Label label = new Label();
            label.Text = $"{username}";
            label.Font = new Font("Arial", 12, FontStyle.Bold);
            label.ForeColor = Color.Black;
            label.AutoSize = true;
            label.PerformLayout();
            int labelWidth = TextRenderer.MeasureText(label.Text, label.Font).Width;
            label.Location = new Point(20, 120);
            label.TextAlign = ContentAlignment.MiddleCenter;

            string[] groupname = await group.RequestGroupName(username);
            string[] groupname1 = await group.RequestGroupName(username1);
            string[] commongroupnames = groupname.Intersect(groupname1).ToArray();
            string[] commonGroupNames = commongroupnames.Take(3).ToArray();
            for (int i = 0; i < commonGroupNames.Length; i++)
            {
                CircularPicture circulargroup = new CircularPicture();
                try
                {
                    circulargroup.Image = await avatar.LoadAvatarGroupAsync(commonGroupNames[i]);
                    circulargroup.SizeMode = PictureBoxSizeMode.Zoom;
                    circulargroup.Name = commonGroupNames[i];
                    UserSession.AvatarGroupUpdated += async () =>
                    {
                        circulargroup.Image = await avatar.LoadAvatarGroupAsync(commonGroupNames[i]);
                        circulargroup.Name = commonGroupNames[i];
                    };
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Không thể tải ảnh: {ex.Message}");
                    return;
                }
                circulargroup.Location = new Point(20 + i * 30, 150);
                circulargroup.Size = new Size(25, 25);
                circulargroup.Anchor = AnchorStyles.None;
                circulargroup.Click += async (s, e) =>
                {
                    GroupUser user = new GroupUser(username,username1);
                    user.Show();
                };
                form.Controls.Add(circulargroup);
            }

            Label label1 = new Label();
            label1.Text = $"{commongroupnames.Length} Máy Chủ Chung";
            label1.Font = new Font("Arial", 12, FontStyle.Bold);
            label1.ForeColor = Color.Black;
            label1.AutoSize = true;
            label1.PerformLayout();
            label1.Location = new Point(20 + commonGroupNames.Length * 30, 150);
            label1.TextAlign = ContentAlignment.MiddleCenter;

            TextBox nhantin = new TextBox();
            nhantin.Text = "";
            nhantin.Font = new Font("Arial", 12, FontStyle.Bold);
            nhantin.ForeColor = Color.Black;
            nhantin.Size = new Size(440, 30);
            nhantin.PerformLayout();
            nhantin.Location = new Point(20, 200);

            Button thongtin = new Button();
            thongtin.Text = "...";
            thongtin.AutoSize = AutoSize;
            thongtin.Location = new Point(460 - thongtin.Width, 20);
            thongtin.Click += async (s, e) =>
            {
                 if(form1==null)TrangChu(username);
                 else
                 {
                    if(form2!=null)
                    {
                        form2.Dispose();
                        form2.Close();
                        form2 = null;
                    }
                    form1.Dispose();
                    form1.Close();
                    on = false;
                    form1 = null;
                 }
            };

            Button banbe = new Button();
            banbe.Text = "banbe";
            banbe.Location = new Point(460 - thongtin.Width - banbe.Width - 20, 20);
            banbe.AutoSize = AutoSize;
            banbe.Click += (s, e) =>
            {
                SearchUser user = new SearchUser(username1);
                user.Show();
            };

            form.Controls.Add(userpicture);
            form.Controls.Add(label);
            if (username!=username1)
            { 
                form.Controls.Add(label1);
                form.Controls.Add(nhantin);
                form.Controls.Add(thongtin);
                form.Controls.Add(banbe);
            }
            form.Show();
        }
        Form form1;
        private void TrangChu(string username)
        {
            on = true;
            form1 = new Form();
            form1.Text = $"Trang chủ";
            form1.Size = new Size(200, 200);
            form1.StartPosition = FormStartPosition.CenterParent;
            form1.FormClosed += (s1, e1) =>
            {
                if (form2 != null)
                {
                    form2.Dispose();
                    form2.Close();
                    form2 = null;
                }
                on = false;
                form1 = null;

            };
            Button button = new Button();
            button.Text = "Xem ho so day du";
            button.AutoSize = true;
            button.Location = new Point(20, 20);
            button.Click += (s1, e1) =>
            {
                GroupUser user = new GroupUser(username,username1);
                user.Show();
            };
            Button moivaomaychu = new Button();
            moivaomaychu.Text = "moi vao may chu";
            moivaomaychu.AutoSize = true;
            moivaomaychu.Location = new Point(20, 60);
            moivaomaychu.Click += async (s1, e1) =>
            {

                if (form2 == null) moivaogroup(username);
                else
                {
                    form2.Dispose();
                    form2.Close();
                    form2 = null;
                }
                

            };
            
            form1.Controls.Add(button);
            form1.Controls.Add(moivaomaychu);
            form1.Show();
        }
        Form form2;
        private async void moivaogroup(string username)
        {
            form2 = new Form();
            form2.Text = $"moi vao may chu";
            form2.AutoSize = AutoSize;
            form2.StartPosition = FormStartPosition.CenterParent;

            // Create a FlowLayoutPanel to hold the buttons
            FlowLayoutPanel flowLayoutPanel1 = new FlowLayoutPanel();
            flowLayoutPanel1.Dock = DockStyle.Fill;  // Dock the panel to fill the form
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;  // Arrange buttons vertically
            flowLayoutPanel1.WrapContents = false;  // Don't wrap buttons to next row
            flowLayoutPanel1.AutoScroll = true; // Enable scrolling if buttons overflow

            // Add the FlowLayoutPanel to the form
            form2.Controls.Add(flowLayoutPanel1);

            // Get group names from the server
            string[] groupname2 = await group.RequestGroupName(username1);

            for (int i = 0; i < groupname2.Length; i++)
            {
                // Create a button for each group
                Button moivaogroup = new Button();
                moivaogroup.Text = $"{groupname2[i]}";
                moivaogroup.Name = groupname2[i];
                moivaogroup.Size = new Size(120, 30);  // Adjust the size of the buttons
                moivaogroup.Click += async (s2, e2) =>
                {
                    await group.AddMembersToGroup(username, moivaogroup.Name);
                };

                // Add the button to the FlowLayoutPanel
                flowLayoutPanel1.Controls.Add(moivaogroup);
            }
            form2.Show();
            
        }
    }
}
