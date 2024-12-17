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

namespace QLUSER
{
    public partial class CaiDatDanhMuc : Form
    {
        string _danhmucid = null;
        string _danhmucname = null;
        DanhMuc _danhmuc = new DanhMuc();
        Channel _channel = new Channel();
        GiaoDien _gd;
        public CaiDatDanhMuc(string danhmucid,string danhmucname,GiaoDien gd)
        {
            InitializeComponent();
            _danhmucid = danhmucid;
            _danhmucname = danhmucname;
            _gd = gd;
        }

        private void CaiDatDanhMuc_Load(object sender, EventArgs e)
        {
            label3.Text = _danhmucname;
            textBox1.Text = _danhmucname;
            UserSession.ActionUpdatedanhmucname += async () =>
            {
                var danhmuc = await _danhmuc.RequestonedanhmucName(_danhmucid);
                _danhmucname = danhmuc.danhmucname;
                label3.Text = _danhmucname;
                textBox1.Text = _danhmucname;
            };
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != _danhmucname && !string.IsNullOrEmpty(textBox1.Text))
            {
                var result = await _danhmuc.RenameDanhMuc(_danhmucid, textBox1.Text);
                if (result)
                {
                    _danhmucname = textBox1.Text;
                    UserSession.Renamedanhmucname = true;
                    _gd.SendUpdate("Updatedanhmucname");
                    MessageBox.Show("Tên danh mục đã được cập nhật thành công!", "Thông Báo");
                }
                else MessageBox.Show("Tên danh mục cập nhật thất bại!", "Thông Báo");
            }
            else
            {
                MessageBox.Show("Tên danh mục không thay đổi.", "Thông Báo");
            }
        }

        private async void label2_Click(object sender, EventArgs e)
        {
            try
            {
                var result = DialogResult.No;

                result = MessageBox.Show(
            "Bạn có chắc chắn muốn xóa danh mục này không?",
            "Xác nhận xóa danh mục",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning
        );



                // Kiểm tra kết quả người dùng chọn
                if (result == DialogResult.Yes)
                {

                    var remove = await _danhmuc.Deletedanhmuc(_danhmucid);
                    if (remove)
                    {
                        UserSession.UpdateDanhMuc = true;
                        _gd.SendUpdate("UpdateDanhMuc");
                        MessageBox.Show("Xóa danh mục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();

                    }
                }
                else
                {
                    // Hủy thao tác
                    MessageBox.Show("Thao tác xóa danh mục đã bị hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
