using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLUSER.Models
{
    public class CircularPicture : PictureBox
    {
        private string _text = string.Empty;

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                Invalidate();
            }
        }
        private string _filepath = string.Empty;

        public string filepath
        {
            get => _filepath;
            set
            {
                _filepath = value;
                Invalidate();
            }
        }

        public Font TextFont { get; set; } = new Font("Arial", 10, FontStyle.Bold);
        public Color TextColor { get; set; } = Color.Black;
        protected override void OnPaint(PaintEventArgs e)
        {
            // Tạo vùng hình tròn để cắt hình ảnh
            GraphicsPath g = new GraphicsPath();
            g.AddEllipse(0, 0, ClientSize.Width, ClientSize.Height);
            this.Region = new System.Drawing.Region(g);

            // Nếu có hình ảnh, vẽ nó theo kiểu SizeMode
            if (Image != null)
            {
                // Vẽ hình ảnh với kích thước và vị trí phù hợp
                var imgRectangle = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);

                switch (SizeMode)
                {
                    case PictureBoxSizeMode.Normal:
                        // Vẽ hình ảnh ở kích thước thực tế (không thay đổi kích thước)
                        e.Graphics.DrawImage(Image, imgRectangle);
                        break;
                    case PictureBoxSizeMode.StretchImage:
                        // Vẽ hình ảnh với kích thước phù hợp với PictureBox
                        e.Graphics.DrawImage(Image, imgRectangle);
                        break;
                    case PictureBoxSizeMode.AutoSize:
                        // Vẽ hình ảnh với kích thước tự động (giữ kích thước thực của ảnh)
                        e.Graphics.DrawImage(Image, new Point(0, 0));
                        break;
                    case PictureBoxSizeMode.CenterImage:
                        // Vẽ hình ảnh ở giữa PictureBox
                        var x = (ClientSize.Width - Image.Width) / 2;
                        var y = (ClientSize.Height - Image.Height) / 2;
                        e.Graphics.DrawImage(Image, new Rectangle(x, y, Image.Width, Image.Height));
                        break;
                    case PictureBoxSizeMode.Zoom:
                        // Vẽ hình ảnh với tỷ lệ phù hợp (giữ tỷ lệ gốc của ảnh)
                        e.Graphics.DrawImage(Image, imgRectangle);
                        break;
                }
            }

            base.OnPaint(e);
        }
    }
}