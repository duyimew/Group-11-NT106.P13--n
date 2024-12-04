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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace QLUSER
{
    public partial class GroupUser : Form
    {
        UserAvatar avatar = new UserAvatar();
        Group group =new Group();
        string username1="";
        string usernametk1;
        User user = new User();
        public GroupUser(string username,string usernametk)
        {
            InitializeComponent();
            username1 = username;
            usernametk1 = usernametk;
        }

        private async void GroupUser_Load(object sender, EventArgs e)
        {
            circularPicture1.Image = await avatar.LoadAvatarAsync(username1);
            circularPicture1.SizeMode = PictureBoxSizeMode.Zoom;
            circularPicture1.Anchor = AnchorStyles.None;
            label1.Text= username1;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            Label label = new Label();
            label.Text = "Vai trò";
            flowLayoutPanel1.Controls.Add(label);
            flowLayoutPanel1.Padding = new Padding(20, 0, 0, 10);
            Label label1 = new Label();
            label1.Text = "Gia nhập từ";
            flowLayoutPanel1.Controls.Add(label1);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
        }

        private async void label4_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            string userid1 = await user.finduserid(username1);
            string useridtk1 = await user.finduserid(usernametk1);
            var result = await group.RequestGroupName(userid1);
            var result1 = await group.RequestGroupName(useridtk1);
            string[] commonGroupNames = result.groupidname.Intersect(result1.groupidname).ToArray();

            for (int i = 0; i < commonGroupNames.Length; i++)
            {
                string[] groupid = commonGroupNames[i].Split('|');
                CircularPicture circulargroup = new CircularPicture();
                try
                {
                    circulargroup.Image = await avatar.LoadAvatarGroupAsync(groupid[0]);
                    circulargroup.SizeMode = PictureBoxSizeMode.Zoom;
                    circulargroup.Name = commonGroupNames[i];

                    UserSession.AvatarGroupUpdated += async () =>
                    {
                        circulargroup.Image = await avatar.LoadAvatarGroupAsync(groupid[0]);
                        circulargroup.Name = commonGroupNames[i];
                    };
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Không thể tải ảnh: {ex.Message}");
                    return;
                }

                // Set the circular avatar properties
                circulargroup.Size = new Size(25, 25);
                circulargroup.Anchor = AnchorStyles.None;

                // Create label for group name
                Label label = new Label();
                label.Text = commonGroupNames[i];
                label.TextAlign = ContentAlignment.MiddleLeft;  // Align label text to the left

                // Create FlowLayoutPanel to hold the avatar and label
                FlowLayoutPanel panel = new FlowLayoutPanel();
                panel.AutoSize = true;
                panel.Padding = new Padding(5, 0, 0, 0);  // Add some padding between the avatar and the label
                panel.FlowDirection = FlowDirection.LeftToRight; // Arrange the label and avatar horizontally
                panel.WrapContents = false;  // Don't wrap contents to new lines

                // Add the avatar and label to the FlowLayoutPanel
                panel.Controls.Add(circulargroup);
                panel.Controls.Add(label);

                // Add the FlowLayoutPanel to the main FlowLayoutPanel
                flowLayoutPanel1.Controls.Add(panel);
            }
        }


    }
}
