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

namespace QLUSER
{
    public partial class VideoCall : Form
    {
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        private HubConnection connection;
        private Button btnEndCall;
        private Label lblStatus;
        private ComboBox comboBoxCameras;
        private ListBox listBoxParticipants;
        private WaveInEvent waveIn;
        private WaveOutEvent waveOut;
        private BufferedWaveProvider waveProvider;
        private HubConnection audioConnection;
        private Button btnToggleMic;
        private bool isMicOn = false;
        private bool isSendingData = false;
        string username1;
        private PictureBox[] pictureBoxes;
        private System.Threading.Timer captureTimer;
        private bool isImageDisposed = false;
        private bool isRecevie = false;
        private bool isleft = false;
        private bool isScreenSharing = false;
        private bool picture = false;
        private bool isstop = false;
        private string callId;
        public VideoCall(string username, string groupname,string channelname)
        {
            InitializeComponent();
            username1 = username;
            pictureBoxes = new PictureBox[10];
            callId = groupname + "|" + channelname;
        }

        private async void VideoCall_Load(object sender, EventArgs e)
        {
            InitializeSignalR();
            InitializeAudio();
            InitializeCamera();

        }
        private void SetPictureBoxLayout(int n)
        {
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
                        BorderStyle = BorderStyle.FixedSingle // Thêm border để dễ quan sát
                    };
                }
                else {
                    pictureBox = new PictureBox
                    {
                        Location = new Point(layouts[n - 1].x + (i % 3) * layouts[n - 1].width, layouts[n - 1].y + (i / 3) * layouts[n - 1].height),
                        Size = new Size(layouts[n - 1].width, layouts[n - 1].height),
                        Visible = true,
                        BorderStyle = BorderStyle.FixedSingle // Thêm border để dễ quan sát
                    };
                }
                // Thêm PictureBox vào form
                this.Invoke(new Action(() => this.Controls.Add(pictureBox)));
                pictureBoxes[i] = pictureBox;
            }
        }




        private void InitializeAudio()
        {
            waveIn = new WaveInEvent();
            waveIn.WaveFormat = new WaveFormat(8000, 1);
            waveIn.DataAvailable += WaveIn_DataAvailable;
            waveProvider = new BufferedWaveProvider(waveIn.WaveFormat);
            waveOut = new WaveOutEvent();
            waveOut.Init(waveProvider);
            waveOut.Play();

        }
        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
         
        
                if (isMicOn && connection != null && connection.State == HubConnectionState.Connected)
                {
                    byte[] audioData = e.Buffer.Take(e.BytesRecorded).ToArray();
                    try
                    {
                        isSendingData = true;
                    Task.Run(async () =>
                    {
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
        private void InitializeCamera()
        {

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

        private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {

                Bitmap frame = (Bitmap)eventArgs.Frame.Clone();
                if (frame == null)
                {
                    Console.WriteLine("Received a null frame.");
                    return;
                }

                int originalWidth = frame.Width;
                int originalHeight = frame.Height;

                // Kích thước khung đích
                int targetWidth = pictureBoxes[0].Width;
                int targetHeight = pictureBoxes[0].Height;

                // Tính tỷ lệ khung hình
                float originalAspectRatio = (float)originalWidth / originalHeight;
                float targetAspectRatio = (float)targetWidth / targetHeight;

                // Tạo kích thước mới để thu nhỏ ảnh sao cho một chiều vừa khít với khung đích
                int scaledWidth = targetWidth, scaledHeight = targetHeight;
                //if (originalAspectRatio > targetAspectRatio)
                {
                  //  scaledHeight = targetHeight;
                  //  scaledWidth = (int)(targetHeight * originalAspectRatio);
                }
                //else
                {
                //    scaledWidth = targetWidth;
                  //  scaledHeight = (int)(targetWidth / originalAspectRatio);
                }
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
                    using (var uploadFrame = resizedFrameclone)
                    {
                        if (connection.State == HubConnectionState.Connected)
                        {
                            byte[] frameData = ConvertFrameToByteArray(finalImage);
                            await connection.SendAsync("SendVideoFrame", callId, frameData, username1);
                        }
                    }
                });

                frame.Dispose();
        }

        

        private byte[] ConvertFrameToByteArray(Bitmap frame)
        {
            using (var ms = new MemoryStream())
            {
                frame.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }


        private async void InitializeSignalR()
        {
            if (listBox1.InvokeRequired)
            {
                listBox1.Invoke(new Action(() =>
                {
                    if (!listBox1.Items.Contains(username1))
                    {
                        listBox1.Items.Add(username1);
                        int n = listBox1.Items.Count;
                        SetPictureBoxLayout(n);
                    }
                }));
            }
            else
            {
                if (!listBox1.Items.Contains(username1))
                {
                    listBox1.Items.Add(username1);
                    int n = listBox1.Items.Count;
                    SetPictureBoxLayout(n);
                }
            }
            connection = new HubConnectionBuilder()
                .WithUrl(ConfigurationManager.AppSettings["HubUrl"] + "videoCallHub")
                .Build();

            await connection.StartAsync();
            await connection.SendAsync("JoinCall",callId, username1);


                connection.On<string>("UserJoined", (userId) =>
            {
                isleft = true;
                isRecevie = true;
                this.Invoke(new Action(async () =>
                {
                    if (button4.Text == "Share Screen on")
                    {
                        button4_Click(null, EventArgs.Empty);
                        await AddUserToListBox(userId);
                        button4_Click(null, EventArgs.Empty);
                    }
                    else await AddUserToListBox(userId);
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
                        button4_Click(null, EventArgs.Empty);
                        foreach (var userId in participants)
                        {
                            await AddUserToListBox(userId);
                        }
                        button4_Click(null, EventArgs.Empty);
                    }
                    else
                    {
                        foreach (var userId in participants)
                        {
                            await AddUserToListBox(userId);
                        }
                    }
                }));
                isRecevie = false;
                isleft = false;
            });

            // Xử lý sự kiện khi người dùng rời khỏi cuộc gọi

                
                connection.On<string>("UserLeft", (userId) =>
            {
                isleft = true;
                isRecevie = true;
                this.Invoke(new Action(async () =>
                {
                    if (button4.Text == "Share Screen on")
                    {
                        button4_Click(null, EventArgs.Empty);
                        await RemoveUserFromListBox(userId);
                        button4_Click(null, EventArgs.Empty);
                    }
                    else await RemoveUserFromListBox(userId);
                }));

                isRecevie = false;
                isleft = false;
            });
               
            // Xử lý sự kiện nhận khung hình video từ người khác

                connection.On<byte[], string>("ReceiveVideoFrame", (frameData, userId) =>
            {

                if (isleft) return;
                    isRecevie = true;
                    using (var ms = new MemoryStream(frameData))
                    {
                        Bitmap image = new Bitmap(ms);

                        // Xác định vị trí của userId trong listBox1
                        int index = listBox1.Items.IndexOf(userId);

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
                        }
                    }
                    isRecevie = false;
                
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
                            if(!isstop) 
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

        // Hàm tiện ích để thêm người dùng vào ListBox1
        private async Task AddUserToListBox(string userId)
        {
            if (listBox1.InvokeRequired)
            {
                listBox1.Invoke(new Action(() =>
                {
                    if (!listBox1.Items.Contains(userId))
                    {
                        listBox1.Items.Add(userId);
                        int n = listBox1.Items.Count;
                        SetPictureBoxLayout(n);
                    }
                }));
            }
            else
            {
                if (!listBox1.Items.Contains(userId))
                {
                    listBox1.Items.Add(userId);
                    int n = listBox1.Items.Count;
                    SetPictureBoxLayout(n);
                }
            }
        }

        // Hàm tiện ích để xóa người dùng khỏi ListBox1
        private async Task RemoveUserFromListBox(string userId)
        {
            if (listBox1.InvokeRequired)
            {
                listBox1.Invoke(new Action(() =>
                {
                    listBox1.Items.Remove(userId);
                    int n = listBox1.Items.Count;
                    SetPictureBoxLayout(n);
                }));
            }
            else
            {
                listBox1.Items.Remove(userId);
                int n = listBox1.Items.Count;
                SetPictureBoxLayout(n);
            }
        }




        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if(button4.Text=="Share Screen on")
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
            }
            else
            {
                videoSource.Start();
                button1.Text = "Video on";
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            if (connection != null)
            {
                try
                {
                    await connection.SendAsync("LeaveCall", callId, username1);
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


        private void button2_Click(object sender, EventArgs e)
        {
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
                // Đặt tên mới cho trạng thái của nút khi chia sẻ màn hình
                {
                    button4.Text = "Share Screen on";
                    await StartScreenCapture();
        }
                else
                {
                    // Khi hoàn thành, hiển thị lại trạng thái và cập nhật nút
                    button4.Text = "Share Screen off";
                    await StopScreenSharing();
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

                                int scaledWidth=targetWidth, scaledHeight=targetHeight;
                                //if (originalAspectRatio > targetAspectRatio)
                                {
                                //    scaledHeight = targetHeight;
                                  //  scaledWidth = (int)(targetHeight * originalAspectRatio);
                                }
                               // else
                                {
                                    //scaledWidth = targetWidth;
                                    //scaledHeight = (int)(targetWidth / originalAspectRatio);
                                }
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

                                Task.Run(async () =>
                                {
                                    using (var uploadFrame = resizedFrameclone)
                                    {
                                        if (connection.State == HubConnectionState.Connected)
                                        {
                                            byte[] frameData = ConvertFrameToByteArray(finalImage);
                                            await connection.SendAsync("SendVideoFrame", callId, frameData, username1);
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
                    await connection.SendAsync("JoinCall", callId, username1);
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
