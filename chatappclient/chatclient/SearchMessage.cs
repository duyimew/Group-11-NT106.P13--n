using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Configuration;
using chatclient.DTOs.Message;
using System.Web.UI.WebControls;
using QLUSER.Models;
namespace QLUSER
{
    public partial class SearchMessage : Form
    {
        GiaoDien _gd;
        public string[] message;
        private string channelid1;
        public SearchMessage(string channelid,GiaoDien gd)
        {
            InitializeComponent();
            _gd= gd;
            channelid1 = channelid;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string messagetext = textBox1.Text;
            string username = textBox2.Text;
            bool Hinhanh = checkBox1.Checked;
            bool tep = checkBox2.Checked;
            string truocngay = textBox4.Text;
            string trongngay = textBox5.Text;
            string saungay = textBox6.Text;

            string dateFormat = "dd/MM/yyyy";

            if (!string.IsNullOrEmpty(truocngay) && !DateTime.TryParseExact(truocngay, dateFormat, null, System.Globalization.DateTimeStyles.None, out _))
            {
                MessageBox.Show("Ngày phải theo định dạng dd/MM/yyyy", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!string.IsNullOrEmpty(trongngay) && !DateTime.TryParseExact(trongngay, dateFormat, null, System.Globalization.DateTimeStyles.None, out _))
            {
                MessageBox.Show("Ngày phải theo định dạng dd/MM/yyyy", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!string.IsNullOrEmpty(saungay) && !DateTime.TryParseExact(saungay, dateFormat, null, System.Globalization.DateTimeStyles.None, out _))
            {
                MessageBox.Show("Ngày phải theo định dạng dd/MM/yyyy", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }



            var result = await SearchMessagesFromServer(messagetext, username, Hinhanh, tep, truocngay, trongngay, saungay);
            if (result.issucess)
            {
                message = new string[result.message.Length];
                message = result.message;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private async Task<(bool issucess, string[] message)> SearchMessagesFromServer(string messagetext, string username, bool hinhanh, bool tep, string beforeDate, string onDate, string afterDate)
        {
            var SearchMessage = new SearchMessageDTO
            {
                channelid = channelid1,
                messagetext = messagetext,
                username = username,
                hinhanh = hinhanh,
                tep = tep,
                beforeDate = beforeDate,
                onDate = onDate,
                afterDate = afterDate
            };
            var json = JsonConvert.SerializeObject(SearchMessage);
            var content = new StringContent(json, Encoding.Unicode, "application/json");
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(ConfigurationManager.AppSettings["ServerUrl"] + "Message/SearchMessage", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                List<string> MessagetextList = new List<string>();
                foreach (var text in responseData.messagetext)
                {
                    MessagetextList.Add((string)text);
                }
                string[] MessagetextArray = MessagetextList.ToArray();
                return (true, MessagetextArray);
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                string Message = responseData.message;
                MessageBox.Show(Message);
                return (false, null);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked && checkBox2.Checked)
            {
                checkBox1.Checked = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked && checkBox2.Checked)
            {
                checkBox2.Checked = false;
            }
        }

        private void SearchMessage_Load(object sender, EventArgs e)
        {
            UserSession.ActionUpdateGroup += () => {
                if (this != null && !this.IsDisposed)
                {
                    this.Close();
                }
            };
        }
    }
}
