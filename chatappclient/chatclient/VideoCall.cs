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

        public VideoCall(string username)
        {
            InitializeComponent();
            username1 = username;

            Random random = new Random();
            //username1 = random.ToString();
            pictureBoxes = new PictureBox[10];
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
                if(pictureBox!=null)
                pictureBox.Invoke(new Action(() => this.Controls.Remove(pictureBox)));
            }

            // Các thông số vị trí và kích thước tương ứng với từng số lượng PictureBox
            var layouts = new (int x, int y, int width, int height)[]
            {
        (10, 10, 750, 450),  // 1 PictureBox
        (10, 10, 375, 300),  // 2 PictureBox
        (10, 10, 375, 225),  // 3 PictureBox
        (10, 10, 375, 225),  // 4 PictureBox
        (10, 10, 375, 150),  // 5 PictureBox
        (10, 10, 375, 150),  // 6 PictureBox
        (10, 10, 250, 150),  // 7 PictureBox
        (10, 10, 250, 150),  // 8 PictureBox
        (10, 10, 250, 150),  // 9 PictureBox
            };

            // Thêm lại n PictureBox mới vào form
            for (int i = 0; i < n && i < layouts.Length; i++)
            {
                PictureBox pictureBox;
                // Tạo PictureBox mới với vị trí và kích thước từ layouts
                if (n < 7)
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
        private async void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {

            if (isMicOn && connection != null && connection.State == HubConnectionState.Connected)
            {
                byte[] audioData = e.Buffer.Take(e.BytesRecorded).ToArray();
                try
                {
                    isSendingData = true;
                    await connection.SendAsync("SendAudio", "1", audioData);
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
            int scaledWidth, scaledHeight;
            if (originalAspectRatio > targetAspectRatio)
            {
                scaledHeight = targetHeight;
                scaledWidth = (int)(targetHeight * originalAspectRatio);
            }
            else
            {
                scaledWidth = targetWidth;
                scaledHeight = (int)(targetWidth / originalAspectRatio);
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
                pictureBoxes[0].Image = finalImage;
            }));

            Task.Run(async () =>
            {
                using (var uploadFrame = resizedFrameclone)
                {
                    byte[] frameData = ConvertFrameToByteArray(uploadFrame);
                    await connection.SendAsync("SendVideoFrame", "1",frameData, username1);
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

        private async void BtnEndCall_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Status: Ending Call...";

            
            lblStatus.Text = "Status: Call Ended";
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
            await connection.SendAsync("JoinCall","1", username1);
            connection.On<string>("UserJoined", (userId) =>
            {
                AddUserToListBox(userId);
            });
            connection.On<IEnumerable<string>>("ExistingParticipants", (participants) =>
            {
                foreach (var userId in participants)
                {
                    AddUserToListBox(userId);
                }
            });

            // Xử lý sự kiện khi người dùng rời khỏi cuộc gọi
            connection.On<string>("UserLeft", (userId) =>
            {
                RemoveUserFromListBox(userId);
            });

            // Xử lý sự kiện nhận khung hình video từ người khác
            connection.On<byte[], string>("ReceiveVideoFrame", (frameData, userId) =>
            {
                using (var ms = new MemoryStream(frameData))
                {
                    Bitmap image = new Bitmap(ms);
                    pictureBoxes[1].Invoke(new Action(() =>
                    {
                        if (pictureBoxes[1].Image != null)
                        {
                            pictureBoxes[1].Image.Dispose();
                        }
                        pictureBoxes[0].SizeMode = PictureBoxSizeMode.StretchImage;
                        pictureBoxes[0].Image = image;
                    }));
                }
            });

            // Xử lý sự kiện nhận âm thanh
            connection.On<byte[]>("ReceiveAudio", (audioData) =>
            {
                waveProvider.AddSamples(audioData, 0, audioData.Length);
            });
        }

        // Hàm tiện ích để thêm người dùng vào ListBox1
        private void AddUserToListBox(string userId)
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
        private void RemoveUserFromListBox(string userId)
        {
            if (listBox1.InvokeRequired)
            {
                listBox1.Invoke(new Action(() =>
                {
                    listBox1.Items.Remove(userId);
                }));
            }
            else
            {
                listBox1.Items.Remove(userId);
            }
        }




        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
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
            // Gửi yêu cầu rời cuộc gọi đến server
            if (connection != null)
            {
                try
                {
                    await connection.SendAsync("LeaveCall", "1",username1);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error sending LeaveCall: " + ex.Message);
                }
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

            // Dừng và giải phóng kết nối SignalR
            if (connection != null)
            {
                await connection.StopAsync();
                await connection.DisposeAsync();
                connection = null;
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
    }
}
