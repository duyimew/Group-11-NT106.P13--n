using QLUSER.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLUSER
{
    public partial class CaiDatMayChu : Form
    {
        string _groupdisplayname;
        UserAvatar avatar=new UserAvatar();
        Group group =new Group();
        public CaiDatMayChu(string groupname,string groupid,string groupdisplayname)
        {
            InitializeComponent();
            label1.Text= groupname;
            label1.Name= groupid;
            _groupdisplayname = groupdisplayname;
            textBox1.Text = groupname;
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void CaiDatMayChu_Load(object sender, EventArgs e)
        {
            circularPicture1.Image = await avatar.LoadAvatarGroupAsync(label1.Name);
            circularPicture1.Text = label1.Text;
            circularPicture1.Name = $"group|{label1.Name}";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(async () =>
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string avatarUrl = await avatar.UploadAvatarGroupAsync(openFileDialog.FileName, label1.Name);

                        if (avatarUrl != null)
                        {
                            UserSession.AvatarGroupUrl = (avatarUrl,false);
                            MessageBox.Show("Avatar group uploaded successfully!");
                            circularPicture1.Invoke((MethodInvoker)(() =>
                            {
                                if (circularPicture1.Image != null)
                                {
                                    circularPicture1.Image.Dispose();
                                }
                                circularPicture1.Image = Image.FromFile(openFileDialog.FileName);
                            }));
                        }
                        else
                        {
                            MessageBox.Show("Failed to upload avatar.");
                        }
                    }
                }
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private async void button10_Click(object sender, EventArgs e)
        {
            if(textBox1.Text!=label1.Text)
            {
                string newgroupname=textBox1.Text;
                var result = await group.RenameGroup(newgroupname, label1.Name);
                if(result)
                {
                    UserSession.RenameGroupname = newgroupname;
                    label1.Text = textBox1.Text;
                    circularPicture1.Text = textBox1.Text;
                }
            }
        }
    }
}
