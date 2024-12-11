using Microsoft.AspNetCore.SignalR.Client;
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
        GroupMember member = new GroupMember();
        string gdpid;
        string gdpidtk;
        string gdpname1 = "";
        string gdpnametk1;
        string _groupid;
        User user = new User();
        public GroupUser(string gdpname,string gdpnametk,string groupid)
        {
            InitializeComponent();
            gdpname1 = gdpname;
            _groupid=groupid;
            gdpnametk1 = gdpnametk;
            if(gdpname != gdpnametk)
            {
                label3.Visible = true;
                label4.Visible = true;
            }
        }

        private async void GroupUser_Load(object sender, EventArgs e)
        {
            gdpid = await member.FindGroupDisplayID(_groupid,gdpname1);
            gdpidtk = await member.FindGroupDisplayID(_groupid, gdpnametk1);
            circularPicture1.Image = await avatar.LoadAvatarAsync(gdpid);
            UserSession.AvatarUpdated += UpdateAvatarDisplay;    
            circularPicture1.SizeMode = PictureBoxSizeMode.Zoom;
            circularPicture1.Anchor = AnchorStyles.None;
            label1.Text= gdpname1;
            UserSession.AvatarGroupCreated += () => {
                if (this != null && !this.IsDisposed)
                {
                    this.Close();
                }
            };
        }
        private async void UpdateAvatarDisplay()
        {
            circularPicture1.Image = await avatar.LoadAvatarAsync(gdpid);
          
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
            var result = await group.RequestGroupName(gdpid);
            var result1 = await group.RequestGroupName(gdpidtk);
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
                label.Text = groupid[1];
                UserSession.UpdateGroupname += async () =>
                {
                    label.Text = await group.Groupname(groupid[0]);
                };
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

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
