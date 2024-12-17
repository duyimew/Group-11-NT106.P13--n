using QLUSER.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace QLUSER
{
    public partial class CaiDatKenh : Form
    {
        string _channelid;
        string _channelname;
        string _danhmucid=null;
        string _danhmucname=null;
        DanhMuc _danhmuc =new DanhMuc();
        Channel _channel = new Channel();
        GiaoDien _gd;
        public CaiDatKenh(string channelid,string channelname,string danhmucid,GiaoDien gd)
        {
            InitializeComponent();
            _channelid = channelid;
            _channelname = channelname;
            _gd = gd;
            if (danhmucid != null)
            {
                _danhmucid = danhmucid;
            }
        }

        private async void CaiDatKenh_Load(object sender, EventArgs e)
        {
            if (_danhmucid!=null)
            {
                var danhmuc = await _danhmuc.RequestonedanhmucName(_danhmucid);
                _danhmucname = danhmuc.danhmucname;
                UserSession.ActionUpdateChannelname += async () =>
                {
                    var channel = await _channel.RequestoneChannelName(_channelid);
                    _channelname = channel.channelname;
                    label3.Text = _channelname + " " + _danhmucname;
                    textBox1.Text = _channelname;
                };
                UserSession.ActionUpdatedanhmucname += async () =>
                {
                    danhmuc = await _danhmuc.RequestonedanhmucName(_danhmucid);
                    _danhmucname = danhmuc.danhmucname;
                    label3.Text = _channelname + " " + _danhmucname;
                };
                label3.Text = _channelname + " " + _danhmucname;
                textBox1.Text = _channelname;
            }
            else
            {
                label3.Text = _channelname;
                textBox1.Text = _channelname;
                UserSession.ActionUpdateChannelname += async () =>
                {
                    var channel = await _channel.RequestoneChannelName(_channelid);
                    
                    _channelname = channel.channelname;
                    label3.Text = _channelname;
                    textBox1.Text = _channelname;  
                };
            }

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text!= _channelname &&!string.IsNullOrEmpty(textBox1.Text))
            {
                var result =await _channel.RenameChannel(_channelid, textBox1.Text);
                if(result)
                {
                    _channelname = textBox1.Text;
                    UserSession.Renamechannelname = true;
                    _gd.SendUpdate("UpdateChannelname");
                    MessageBox.Show("Tên kênh đã được cập nhật thành công!", "Thông Báo");
                }
                else MessageBox.Show("Tên kênh cập nhật thất bại!", "Thông Báo");
            }
            else
            {
                MessageBox.Show("Tên kênh không thay đổi.", "Thông Báo");
            }
        
        }

        private async void label2_Click(object sender, EventArgs e)
        {
            try
            {
                var result = DialogResult.No;

                result = MessageBox.Show(
            "Bạn có chắc chắn muốn xóa kênh này không?",
            "Xác nhận xóa kênh",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning
        );



                // Kiểm tra kết quả người dùng chọn
                if (result == DialogResult.Yes)
                {
                    
                    var remove = await _channel.DeleteChannel(_channelid);
                    if (remove)
                    {
                        UserSession.UpdateChannel = true;
                        _gd.SendUpdate("UpdateChannel");
                        MessageBox.Show("Xóa kênh thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();

                    }
                }
                else
                {
                    // Hủy thao tác
                    MessageBox.Show("Thao tác xóa kênh đã bị hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
