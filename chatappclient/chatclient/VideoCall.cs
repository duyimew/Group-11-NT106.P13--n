using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using Microsoft.AspNetCore.SignalR.Client;
using System.Linq;
using NAudio.Wave;
using System.Configuration;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Threading;
using System.Diagnostics.Tracing;
using System.Runtime.InteropServices;
using QLUSER.Models;
using System.Reflection;
using Microsoft.VisualBasic.ApplicationServices;

namespace QLUSER
{
    public partial class VideoCall : Form
    {
        GiaoDien _gd;
        GroupMember _groupMember = new GroupMember();
        UserAvatar _avatar = new UserAvatar();
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        private HubConnection connection;
        private Label lblStatus;
        string _userid;
        string _groupid;
        private ListBox listBoxParticipants;
        private WaveInEvent waveIn;
        private WaveOutEvent waveOut;
        private BufferedWaveProvider waveProvider;
        private bool isMicOn = false;
        private bool isSendingData = false;
        string _groupdisplayname;
        private PictureBox[] pictureBoxes;
        private System.Threading.Timer captureTimer;
        private bool isImageDisposed = false;
        private bool isRecevie = false;
        private bool isleft = false;
        private bool isScreenSharing = false;
        private bool picture = false;
        private bool isstop = false;
        private string callId;
        public VideoCall(string groupdisplayname, string channelid,string groupid,GiaoDien gd)
        {
            InitializeComponent();
            _gd = gd;
            _groupdisplayname = groupdisplayname;
            pictureBoxes = new PictureBox[10];
            callId = channelid;
            _groupid = groupid;
            UserSession.ActionDeleteuser += () =>
            {
                if (this != null && !this.IsDisposed)
                {
                    this.Close();
                }
            };
        }

        private async void VideoCall_Load(object sender, EventArgs e)
        {
            try {
                _userid = await _groupMember.FindGroupDisplayID(_groupid, _groupdisplayname);
            InitializeSignalR();
            InitializeAudio();
            InitializeCamera();
                foreach(var item in listBox1.Items )
                {
                    string gdpname= item.ToString();
                    string userid = await _groupMember.FindGroupDisplayID(_groupid, gdpname);
                    Image avatar = await _avatar.LoadAvatarAsync(userid);
                    int index = listBox1.Items.IndexOf(gdpname);
                    if (index >= 0 && index < pictureBoxes.Length)
                    {
                        pictureBoxes[index].Invoke(new Action(() =>
                        {
                            if (pictureBoxes[index].Image != null)
                            {
                                pictureBoxes[index].Image.Dispose();
                            }
                            pictureBoxes[index].SizeMode = PictureBoxSizeMode.StretchImage;
                            pictureBoxes[index].Image = avatar;
                            UserSession.ActionAvatarUpdated += async () =>
                            {
                                avatar = await _avatar.LoadAvatarAsync(userid);
                                pictureBoxes[index].Image = avatar;
                            };
                        }));
                    }
                    
                }
                UserSession.ActionUpdateGroup += () => {
                if (this != null && !this.IsDisposed)
                {
                    this.Close();
                }
            };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UserSession.ActionUpdategdpname += async () =>
            {
                string oldgdpname = _groupdisplayname;
                _groupdisplayname = await _groupMember.FindGroupDisplayname(_userid, _groupid);
                string newgdpname = _groupdisplayname;
                RenameUserInListBox(oldgdpname, newgdpname);
                await Task.Run(async () =>
                {
                    if (connection.State == HubConnectionState.Connected)
                        {
                            await connection.SendAsync("RenamegdpnameInCall", callId, oldgdpname, newgdpname);
                        }
                });
            };
        }
        private async void SetPictureBoxLayout(int n)
        {
            try { 
                
            // Xóa tất cả các PictureBox hiện tại khỏi form
            foreach (var pictureBox in pictureBoxes)
            {
                if (pictureBox != null)
                    pictureBox.Invoke(new Action(() => this.Controls.Remove(pictureBox)));
            }

            // Các thông số vị trí và kích thước tương ứng với từng số lượng PictureBox
            var layouts = new (int x, int y, int width, int height)[]
            {
        (10, 10, 800, 450),  // 1 PictureBox
        (10, 10, 400, 225),  // 2 PictureBox
        (10, 10, 400, 225),  // 3 PictureBox
        (10, 10, 400, 225),  // 4 PictureBox
        (10, 10, 800/3, 150),  // 5 PictureBox
        (10, 10, 800/3, 150),  // 6 PictureBox
        (10, 10, 800/3, 150),  // 7 PictureBox
        (10, 10, 800/3, 150),  // 8 PictureBox
        (10, 10, 800/3, 150),  // 9 PictureBox
            };

            // Thêm lại n PictureBox mới vào form
            for (int i = 0; i < n && i < layouts.Length; i++)
            {
                PictureBox pictureBox;
                // Tạo PictureBox mới với vị trí và kích thước từ layouts
                if (n < 5)
                {
                    pictureBox = new PictureBox
                    {
                        Location = new Point(layouts[n - 1].x + (i % 2) * layouts[n - 1].width, layouts[n - 1].y + (i / 2) * layouts[n - 1].height),
                        Size = new Size(layouts[n - 1].width, layouts[n - 1].height),
                        Visible = true,
                        BorderStyle = BorderStyle.FixedSingle, // Thêm border để dễ quan sát
                    };
                }
                else
                {
                    pictureBox = new PictureBox
                    {
                        Location = new Point(layouts[n - 1].x + (i % 3) * layouts[n - 1].width, layouts[n - 1].y + (i / 3) * layouts[n - 1].height),
                        Size = new Size(layouts[n - 1].width, layouts[n - 1].height),
                        Visible = true,
                        BorderStyle = BorderStyle.FixedSingle,// Thêm border để dễ quan sát

                    };
                }
                    // Thêm PictureBox vào form
                    pictureBoxes[i] = pictureBox;
                    this.Invoke(new Action(() => this.Controls.Add(pictureBoxes[i])));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void InitializeAudio()
        {
            try { 
            waveIn = new WaveInEvent();
            waveIn.WaveFormat = new WaveFormat(8000, 1);
            waveIn.DataAvailable += WaveIn_DataAvailable;
            waveProvider = new BufferedWaveProvider(waveIn.WaveFormat);
            waveOut = new WaveOutEvent();
            waveOut.Init(waveProvider);
            waveOut.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {

            try { 
            if (isMicOn && connection != null && connection.State == HubConnectionState.Connected)
            {
                byte[] audioData = e.Buffer.Take(e.BytesRecorded).ToArray();
                try
                {
                    isSendingData = true;
                    Task.Run(async () =>
                    {
                        if (listBox1.Items.Count == 1 && listBox1.Items[0].ToString() == _groupdisplayname)
                        {
                            return;
                        }
                        if (connection.State == HubConnectionState.Connected)
                        {
                            await connection.SendAsync("SendAudio", callId, audioData);
                        }
                    });
                }
                finally
                {
                    isSendingData = false;
                }
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void InitializeCamera()
        {
            try { 
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count > 0)
            {
                videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
                videoSource.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);
            }
            else
            {
                MessageBox.Show("No video sources found.");
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try { 
            Bitmap frame = (Bitmap)eventArgs.Frame.Clone();
            if (frame == null)
            {
                Console.WriteLine("Received a null frame.");
                return;
            }

            int originalWidth = frame.Width;
            int originalHeight = frame.Height;

            int targetWidth = pictureBoxes[0].Width;
            int targetHeight = pictureBoxes[0].Height;

            float originalAspectRatio = (float)originalWidth / originalHeight;
            float targetAspectRatio = (float)targetWidth / targetHeight;

            int scaledWidth = targetWidth, scaledHeight = targetHeight;

            Bitmap resizedImage = new Bitmap(scaledWidth, scaledHeight);
            using (Graphics g = Graphics.FromImage(resizedImage))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(frame, 0, 0, scaledWidth, scaledHeight);
            }

            // Cắt ảnh để vừa với khung đích
            int cropX = (scaledWidth - targetWidth) / 2;
            int cropY = (scaledHeight - targetHeight) / 2;
            Rectangle cropArea = new Rectangle(cropX, cropY, targetWidth, targetHeight);
            Bitmap finalImage = resizedImage.Clone(cropArea, resizedImage.PixelFormat);
            Bitmap resizedFrameclone = (Bitmap)finalImage.Clone();
            // Hiển thị ảnh đã cắt trong PictureBox

            pictureBoxes[0].Invoke(new Action(() =>
            {
                if (pictureBoxes[0].Image != null)
                {
                    pictureBoxes[0].Image.Dispose();
                }
                pictureBoxes[0].SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBoxes[0].Image = (Bitmap)finalImage.Clone();
            }));

            Task.Run(async () =>
            {
                if (listBox1.Items.Count == 1 && listBox1.Items[0].ToString() == _groupdisplayname)
                {
                    return;
                }
                using (var uploadFrame = resizedFrameclone)
                {
                    if (connection.State == HubConnectionState.Connected)
                    {
                        byte[] frameData = ConvertFrameToByteArray(finalImage);
                        await connection.SendAsync("SendVideoFrame", callId, frameData, _groupdisplayname);
                    }
                }
            });

            frame.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private byte[] ConvertFrameToByteArray(Bitmap frame)
        {
            using (var ms = new MemoryStream())
            {
                frame.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        private byte[] ConvertImageToByteArray(Bitmap frame)
        {
            using (var ms = new MemoryStream())
            {
                frame.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }
        private async void InitializeSignalR()
        {
            try {
                if (listBox1.InvokeRequired)
                {
                    listBox1.Invoke(new Action(() =>
                    {
                        if (!listBox1.Items.Contains(_groupdisplayname))
                        {
                            listBox1.Items.Add(_groupdisplayname);
                            int n = listBox1.Items.Count;
                            SetPictureBoxLayout(n);
                        }
                    }));
                }
                else
                {
                    if (!listBox1.Items.Contains(_groupdisplayname))
                    {
                        listBox1.Items.Add(_groupdisplayname);
                        int n = listBox1.Items.Count;
                        SetPictureBoxLayout(n);
                    }
                }
                connection = new HubConnectionBuilder()
                    .WithUrl(ConfigurationManager.AppSettings["HubUrl"] + "videoCallHub")
                    .Build();

                await connection.StartAsync();
                await connection.SendAsync("JoinCall", callId, _groupdisplayname);


                connection.On<string>("UserJoined", (groupdisplayname) =>
                {
                    isleft = true;
                    isRecevie = true;
                    this.Invoke(new Action(async () =>
                    {
                        if (button4.Text == "Share Screen on")
                        {
                            button4.Text = "Share Screen off";
                            await StopScreenSharing();
                            await AddUserToListBox(groupdisplayname);
                            button4.Text = "Share Screen on";
                            await StartScreenCapture();
                        }
                        else await AddUserToListBox(groupdisplayname);
                    }));
                    isRecevie = false;
                    isleft = false;
                });


                connection.On<IEnumerable<string>>("ExistingParticipants", (participants) =>
                {
                    isleft = true;
                    isRecevie = true;
                    this.Invoke(new Action(async () =>
                    {
                        if (button4.Text == "Share Screen on")
                        {
                            button4.Text = "Share Screen off";
                            await StopScreenSharing();
                            foreach (var groupdisplayname in participants)
                            {
                                await AddUserToListBox(groupdisplayname);
                            }
                            button4.Text = "Share Screen on";
                            await StartScreenCapture();
                        }
                        else
                        {
                            foreach (var groupdisplayname in participants)
                            {
                                await AddUserToListBox(groupdisplayname);
                            }
                        }
                    }));
                    isRecevie = false;
                    isleft = false;
                });
                connection.On<string, string>("GroupNameChanged", (oldGroupDisplayName, newGroupDisplayName) =>
                {
                    isleft = true;
                    isRecevie = true;
                    this.Invoke(new Action(async () =>
                    {
                        if (button4.Text == "Share Screen on")
                        {
                            button4.Text = "Share Screen off";
                            await StopScreenSharing();
                            await RenameUserInListBox(oldGroupDisplayName, newGroupDisplayName);
                            button4.Text = "Share Screen on";
                            await StartScreenCapture();
                        }
                        else await RenameUserInListBox(oldGroupDisplayName, newGroupDisplayName);
                    }));
                    isRecevie = false;
                    isleft = false;
                });
                // Xử lý sự kiện khi người dùng rời khỏi cuộc gọi


                connection.On<string>("UserLeft", (groupdisplayname) =>
                {
                    isleft = true;
                    isRecevie = true;
                    this.Invoke(new Action(async () =>
                    {
                        if (button4.Text == "Share Screen on")
                        {
                            button4.Text = "Share Screen off";
                            await StopScreenSharing();
                            await RemoveUserFromListBox(groupdisplayname);
                            button4.Text = "Share Screen on";
                            await StartScreenCapture();
                        }
                        else await RemoveUserFromListBox(groupdisplayname);
                    }));

                    isRecevie = false;
                    isleft = false;
                });

                // Xử lý sự kiện nhận khung hình video từ người khác

                connection.On<byte[], string>("ReceiveVideoFrame", (frameData, groupdisplayname) =>
                {
                    try
                    {
                        if (frameData == null || frameData.Length == 0)
                        {
                            MessageBox.Show("Received empty frame data.");
                            return;
                        }
                        if (isleft) return;
                        isRecevie = true;
                        using (var ms = new MemoryStream(frameData))
                        {
                            Bitmap image = new Bitmap(ms);

                            // Xác định vị trí của userId trong listBox1
                            int index = listBox1.Items.IndexOf(groupdisplayname);

                            // Kiểm tra nếu userId tồn tại trong listBox1
                            if (index >= 0 && index < pictureBoxes.Length)
                            {
                                pictureBoxes[index].Invoke(new Action(() =>
                            {
                                if (pictureBoxes[index].Image != null)
                                {
                                    pictureBoxes[index].Image.Dispose();
                                }
                                pictureBoxes[index].SizeMode = PictureBoxSizeMode.StretchImage;
                                pictureBoxes[index].Image = image;
                            }));
                            }
                            else
                            {
                                // Nếu userId không hợp lệ hoặc vượt quá số lượng PictureBoxes
                                image.Dispose(); // Giải phóng bộ nhớ
                                MessageBox.Show($"Invalid groupDisplayName: {groupdisplayname}");
                            }
                        }
                        isRecevie = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error displaying received frame: " + ex.Message);
                    }
                });
                connection.On<string>("ReceiveImage", async (groupdisplayname) =>
                {
                    try
                    {
                        isleft=true;
                        isRecevie = true;
                        string userid = await _groupMember.FindGroupDisplayID(_groupid, groupdisplayname);
                        Image avatar = await _avatar.LoadAvatarAsync(userid);
                        int index = listBox1.Items.IndexOf(groupdisplayname);
                        if (index >= 0 && index < pictureBoxes.Length)
                        {
                            pictureBoxes[index].Invoke(new Action(() =>
                            {
                                if (pictureBoxes[index].Image != null)
                                {
                                    pictureBoxes[index].Image.Dispose();
                                }
                                pictureBoxes[index].SizeMode = PictureBoxSizeMode.StretchImage;
                                pictureBoxes[index].Image = avatar;
                            }));
                        }
                        isRecevie = false;
                        isleft = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error displaying received frame: " + ex.Message);
                    }
                });

                // Xử lý sự kiện nhận âm thanh

                connection.On<byte[]>("ReceiveAudio", (audioData) =>
            {

                isRecevie = true;
                waveProvider.AddSamples(audioData, 0, audioData.Length);
                isRecevie = false;

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
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Hàm tiện ích để thêm người dùng vào ListBox1
        private async Task AddUserToListBox(string groupdisplayname)
        {
            try
            {
                if (listBox1.InvokeRequired)
                {
                    listBox1.Invoke(new Action(() =>
                    {
                        if (!listBox1.Items.Contains(groupdisplayname))
                        {
                            listBox1.Items.Add(groupdisplayname);
                            int n = listBox1.Items.Count;
                            SetPictureBoxLayout(n);
                        }
                    }));
                }
                else
                {
                    if (!listBox1.Items.Contains(groupdisplayname))
                    {
                        listBox1.Items.Add(groupdisplayname);
                        int n = listBox1.Items.Count;
                        SetPictureBoxLayout(n);
                    }
                }
                foreach (var item in listBox1.Items)
                {
                    string gdpname = item.ToString();
                    string userid = await _groupMember.FindGroupDisplayID(_groupid, gdpname);
                    Image avatar = await _avatar.LoadAvatarAsync(userid);
                    int index = listBox1.Items.IndexOf(gdpname);
                    if (index >= 0 && index < pictureBoxes.Length)
                    {
                        pictureBoxes[index].Invoke(new Action(() =>
                        {
                            if (pictureBoxes[index].Image != null)
                            {
                                pictureBoxes[index].Image.Dispose();
                            }
                            pictureBoxes[index].SizeMode = PictureBoxSizeMode.StretchImage;
                            pictureBoxes[index].Image = avatar;
                            UserSession.ActionAvatarUpdated += async () =>
                            {
                                avatar = await _avatar.LoadAvatarAsync(userid);
                                pictureBoxes[index].Image = avatar;
                            };
                        }));
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async Task RenameUserInListBox(string oldGroupDisplayName, string newGroupDisplayName)
        {
            try
            {
                if (listBox1.InvokeRequired)
                {
                    listBox1.Invoke(new Action(() =>
                    {
                        UpdateListBoxAtSamePosition(oldGroupDisplayName, newGroupDisplayName);
                    }));
                }
                else
                {
                    UpdateListBoxAtSamePosition(oldGroupDisplayName, newGroupDisplayName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateListBoxAtSamePosition(string oldGroupDisplayName, string newGroupDisplayName)
        {
            // Kiểm tra nếu tên cũ tồn tại
            int index = listBox1.Items.IndexOf(oldGroupDisplayName);
            if (index != -1)
            {
                // Xóa tên cũ
                listBox1.Items.RemoveAt(index);
                // Thêm tên mới vào đúng vị trí
                listBox1.Items.Insert(index, newGroupDisplayName);
            }
            
        }

        // Hàm tiện ích để xóa người dùng khỏi ListBox1
        private async Task RemoveUserFromListBox(string groupdisplayname)
        {
            try
            {
                if (listBox1.InvokeRequired)
                {
                    listBox1.Invoke(new Action(() =>
                    {
                        listBox1.Items.Remove(groupdisplayname);
                        int n = listBox1.Items.Count;
                        SetPictureBoxLayout(n);
                    }));
                }
                else
                {
                    listBox1.Items.Remove(groupdisplayname);
                    int n = listBox1.Items.Count;
                    SetPictureBoxLayout(n);
                }
                foreach (var item in listBox1.Items)
                {
                    string gdpname = item.ToString();
                    string userid = await _groupMember.FindGroupDisplayID(_groupid, gdpname);
                    Image avatar = await _avatar.LoadAvatarAsync(userid);
                    int index = listBox1.Items.IndexOf(gdpname);
                    if (index >= 0 && index < pictureBoxes.Length)
                    {
                        pictureBoxes[index].Invoke(new Action(() =>
                        {
                            if (pictureBoxes[index].Image != null)
                            {
                                pictureBoxes[index].Image.Dispose();
                            }
                            pictureBoxes[index].SizeMode = PictureBoxSizeMode.StretchImage;
                            pictureBoxes[index].Image = avatar;
                            UserSession.ActionAvatarUpdated += async () =>
                            {
                                avatar = await _avatar.LoadAvatarAsync(userid);
                                pictureBoxes[index].Image = avatar;
                            };
                        }));
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try { 
            if (button4.Text == "Share Screen on")
            {
                button4.Text = "Share Screen off";
                await StopScreenSharing();
                await Task.Delay(100);
            }
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                await Task.Run(() => videoSource.WaitForStop());
                button1.Text = "Video off";
                    await Task.Delay(200);
                    try
                    {
                        int i = 0;
                        while (i < 2)
                            if (connection.State == HubConnectionState.Connected)
                            {
                                await connection.SendAsync("SendImage", callId, _groupdisplayname);
                                i++;
                            }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error sending avatar: " + ex.Message);
                    }
                    Image avatar = await _avatar.LoadAvatarAsync(_userid);

                    pictureBoxes[0].Invoke(new Action(() =>
                    {
                        if (pictureBoxes[0].Image != null)
                        {
                            pictureBoxes[0].Image.Dispose();
                        }
                        pictureBoxes[0].SizeMode = PictureBoxSizeMode.StretchImage;
                        pictureBoxes[0].Image = avatar;
                    }));
                    UserSession.ActionAvatarUpdated += async () =>
                    {
                        avatar = await _avatar.LoadAvatarAsync(_userid);
                        pictureBoxes[0].Image = avatar;
                    };
                }
            else
            {
                videoSource.Start();
                button1.Text = "Video on";
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            try { 
            if (connection != null)
            {
                try
                {
                    await connection.SendAsync("LeaveCall", callId, _groupdisplayname);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error sending LeaveCall: " + ex.Message);
                }
            }
            if (button4.Text == "Share Screen on")
            {
                button4.Text = "Share Screen off";
                await StopScreenSharing();
                await Task.Delay(100);
            }
            // Dừng và giải phóng video nguồn nếu đang chạy
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                await Task.Run(() => videoSource.WaitForStop());
            }

            // Dừng và giải phóng âm thanh đầu vào nếu đang ghi
            if (waveIn != null)
            {
                waveIn.StopRecording();
                waveIn.Dispose();
                waveIn = null;
            }

            // Chờ quá trình gửi dữ liệu kết thúc
            while (isSendingData)
            {
                await Task.Delay(100);
            }

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
            this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            try { 
            if (isMicOn)
            {
                waveIn.StopRecording();
                button2.Text = "Mic Off";
                isMicOn = false;
            }
            else
            {
                waveIn.StartRecording();
                button2.Text = "Mic On";
                isMicOn = true;
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // Tạm dừng các chức năng hiện tại (nếu có)
                if (videoSource != null && videoSource.IsRunning)
                {
                    videoSource.SignalToStop();
                    await Task.Run(() => videoSource.WaitForStop());
                    button1.Text = "Video off";
                }
                if (button4.Text == "Share Screen off")
                {
                    button4.Text = "Share Screen on";
                    await StartScreenCapture();
                }
                else
                {
                    button4.Text = "Share Screen off";
                    await StopScreenSharing();
                    await Task.Delay(200);
                    try
                    {
                        int i = 0;
                        while(i<2)
                        if (connection.State == HubConnectionState.Connected)
                        {
                            await connection.SendAsync("SendImage", callId, _groupdisplayname);
                            i++;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error sending avatar: " + ex.Message);
                    }
                    Image avatar = await _avatar.LoadAvatarAsync(_userid);

                        pictureBoxes[0].Invoke(new Action(() =>
                        {
                            if (pictureBoxes[0].Image != null)
                            {
                                pictureBoxes[0].Image.Dispose();
                            }
                            pictureBoxes[0].SizeMode = PictureBoxSizeMode.StretchImage;
                            pictureBoxes[0].Image = avatar;
                        }));
                    UserSession.ActionAvatarUpdated += async () =>
                    {
                        avatar = await _avatar.LoadAvatarAsync(_userid);
                        pictureBoxes[0].Image = avatar;
                    };

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while sharing screen: " + ex.Message);
            }
        }
        private async Task StartScreenCapture()
        {
            try
            {
                Rectangle bounds = System.Windows.Forms.Screen.PrimaryScreen.Bounds;

                if (captureTimer == null)
                {
                    captureTimer = new System.Threading.Timer(new TimerCallback(async _ =>
                    {
                        try
                        {
                            if (button4.Text != "Share Screen on")
                                return;

                            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
                            {
                                using (Graphics g = Graphics.FromImage(bitmap))
                                {
                                    g.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size);
                                }
                                Bitmap frame = bitmap;
                                if (frame == null)
                                {
                                    Console.WriteLine("Received a null frame.");
                                    return;
                                }

                                int originalWidth = frame.Width;
                                int originalHeight = frame.Height;
                                int targetWidth = pictureBoxes[0].Width;
                                int targetHeight = pictureBoxes[0].Height;
                                float originalAspectRatio = (float)originalWidth / originalHeight;
                                float targetAspectRatio = (float)targetWidth / targetHeight;

                                int scaledWidth = targetWidth, scaledHeight = targetHeight;

                                Bitmap resizedImage = new Bitmap(scaledWidth, scaledHeight);
                                using (Graphics g = Graphics.FromImage(resizedImage))
                                {
                                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                    g.DrawImage(frame, 0, 0, scaledWidth, scaledHeight);
                                }

                                int cropX = (scaledWidth - targetWidth) / 2;
                                int cropY = (scaledHeight - targetHeight) / 2;
                                Rectangle cropArea = new Rectangle(cropX, cropY, targetWidth, targetHeight);
                                Bitmap finalImage = resizedImage.Clone(cropArea, resizedImage.PixelFormat);
                                Bitmap resizedFrameclone = (Bitmap)finalImage.Clone();

                                pictureBoxes[0].Invoke(new Action(() =>
                                {
                                    if (pictureBoxes[0].Image != null)
                                    {
                                        pictureBoxes[0].Image.Dispose();
                                    }
                                    pictureBoxes[0].SizeMode = PictureBoxSizeMode.StretchImage;
                                    pictureBoxes[0].Image = (Bitmap)finalImage.Clone();
                                }));

                                await Task.Run(async () =>
                                {
                                    if (listBox1.Items.Count == 1 && listBox1.Items[0].ToString() == _groupdisplayname)
                                    {
                                        return;
                                    }
                                    using (var uploadFrame = resizedFrameclone)
                                    {
                                        if (connection.State == HubConnectionState.Connected)
                                        {
                                            byte[] frameData = ConvertFrameToByteArray(finalImage);
                                            await connection.SendAsync("SendVideoFrame", callId, frameData, _groupdisplayname);
                                        }

                                    }
                                });

                                frame.Dispose();

                            }
                        }
                        catch (Exception timerEx)
                        {
                            MessageBox.Show("Error in capture timer: " + timerEx.Message);
                        }
                    }), null, 0, 100);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while starting screen capture: " + ex.Message);
            }
        }

        private async Task ReconnectSignalR()
        {
            try
            {
                if (connection.State != HubConnectionState.Connected)
                {
                    await connection.StartAsync();
                    await connection.SendAsync("JoinCall", callId, _groupdisplayname);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Reconnection failed: " + ex.Message);
            }
        }




        // Hàm dừng chia sẻ màn hình
        private async Task StopScreenSharing()
        {
            try
            {
                captureTimer?.Dispose();

                captureTimer = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while stopping screen sharing: " + ex.Message);
            }
        }
    }
}
