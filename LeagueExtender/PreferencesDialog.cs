using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace LeagueExtender
{
    public class PreferencesDialog : Form
    {

        Button btnExit;
        Label lblSummonerName;
        Label lblRegion;
        TextBox txtSummonerName;
        TextBox txtRegion;

        public PreferencesDialog(LolClient client)
        {
            this.Text = "LeagueExtender Preferences";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Icon = LeagueExtender.Properties.Resources.gear_blue_ico;

            InitializeComponent();

            this.FormClosing += PreferencesDialog_FormClosing;
            this.Load += PreferencesDialog_Load;
        }

        void PreferencesDialog_Load(object sender, EventArgs e)
        {
            if (File.Exists("data.les"))
            {
                Settings settings = Settings.Load("data.les");
                this.txtSummonerName.Text = settings.SummonerName;
                this.txtRegion.Text = settings.Region;
            }
        }

        void PreferencesDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Save("data.les", new Settings { SummonerName = this.txtSummonerName.Text, Region = this.txtRegion.Text });
        }

        private void InitializeComponent()
        {
            btnExit = new Button();
            btnExit.Text = "Exit LeagueExtender";
            btnExit.Width = 130;
            btnExit.Location = new Point(this.Size.Width / 2 - btnExit.Width / 2, this.Size.Height - 45 - btnExit.Height);
            btnExit.Click += btnExit_Click;

            lblSummonerName = new Label
            {
                Text = "Summoner Name:",
                Location = new Point(10, 10)
            };

            lblRegion = new Label
            {
                Text = "Region:",
                Location = new Point(10, 40)
            };

            txtSummonerName = new TextBox
            {
                Location = new Point(lblSummonerName.Location.X + lblSummonerName.Width, 10),
                Width = this.Width - lblSummonerName.Location.X - lblSummonerName.Width - 30
            };

            txtRegion = new TextBox
            {
                Location = new Point(lblSummonerName.Location.X + lblSummonerName.Width, 40),
                Width = this.Width - lblSummonerName.Location.X - lblSummonerName.Width - 30
            };

            this.Controls.Add(lblSummonerName);
            this.Controls.Add(txtSummonerName);
            this.Controls.Add(lblRegion);
            this.Controls.Add(txtRegion);
            this.Controls.Add(btnExit);
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
