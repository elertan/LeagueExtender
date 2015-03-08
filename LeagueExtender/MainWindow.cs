using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace LeagueExtender
{
    public partial class MainWindow : Form
    {

        LolClient LolClient;
        PreferencesDialog prefDialog;
        LolClientBrowser lolNexusBrowser;
        LolClientBrowser lolKingBrowser;

        PictureBox gearImg;
        Button btnLolNexus;
        Button btnLolKing;

        public bool lolNexusBrowserOpen = false;
        public bool lolKingBrowserOpen = false;

        ~MainWindow() // For stopping a movement task later
        {
            
        }

        public MainWindow()
        {
            this.LolClient = new LolClient();
            this.prefDialog = new PreferencesDialog(this.LolClient);

            InitializeComponents();

            this.ShowInTaskbar = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.TransparencyKey = Color.Yellow;
            this.BackColor = Color.Yellow;

            this.Load += MainWindow_Load;

            this.LolClient.MovedWindow += LolClient_MovedWindow;
            this.LolClient.PlacedWindow += LolClient_PlacedWindow;
            this.LolClient.MinimizedWindow += LolClient_MinimizedWindow;
            this.LolClient.ClosedWindow += LolClient_ClosedWindow;
            this.LolClient.ClickedWindow += LolClient_ClickedWindow;
        }

        void LolClient_ClickedWindow(object sender, EventArgs e)
        {
            this.MoveToFront();
        }

        void LolClient_ClosedWindow(object sender, EventArgs e)
        {
            Application.Exit();
        }

        void LolClient_PlacedWindow(object sender, EventArgs e)
        {
            this.Location = this.GetLocationForLolClient(this.LolClient);
            this.Show();
            this.MoveToFront();
        }

        void LolClient_MinimizedWindow(object sender, EventArgs e)
        {
            
        }

        void LolClient_MovedWindow(object sender, EventArgs e)
        {
            this.Hide();
        }

        void MainWindow_Load(object sender, EventArgs e)
        {
            this.MoveToFront();
            this.Location = this.GetLocationForLolClient(this.LolClient);
            this.Size = this.GetSizeForLolClient(this.LolClient);

            this.LolClient.ListenForEvents();

        }

        private void MoveToFront()
        {
            ExternalFeatures.SetForegroundWindow(this.Handle);
        }

        private Point GetLocationForLolClient(LolClient client)
        {
            Point orgLocation = client.Location;
            return new Point(orgLocation.X + 160, orgLocation.Y + 20);
        }

        private Size GetSizeForLolClient(LolClient client)
        {
            return new Size(this.Size.Width + 100, 60);
        }

        private void InitializeComponents()
        {
            gearImg = new PictureBox();
            gearImg.Image = LeagueExtender.Properties.Resources.gear_blue;
            gearImg.Size = gearImg.Image.Size;
            gearImg.MouseHover += gearImg_MouseHover;
            gearImg.MouseLeave += gearImg_MouseLeave;
            gearImg.Click += gearImg_Click;

            btnLolNexus = new Button
            {
                Location = new Point(35, 0),
                BackColor = Color.Transparent,
                Size = new Size(120, 30),
                Text = "LolNexus"
            };
            btnLolNexus.Click += btnLolNexus_Click;
            btnLolNexus.MouseHover += new EventHandler(btnHover);
            btnLolNexus.MouseLeave += new EventHandler(btnLeave);

            btnLolKing = new Button
            {
                Location = new Point(170, 0),
                BackColor = Color.Transparent,
                Size = new Size(120, 30),
                Text = "LolKing",
                Enabled = false
            };
            btnLolKing.Click += btnLolKing_Click;
            btnLolKing.MouseHover += new EventHandler(btnHover);
            btnLolKing.MouseLeave += new EventHandler(btnLeave);

            lolNexusBrowser = new LolClientBrowser(LolClient, String.Format("http://www.lolnexus.com/{0}/search?name={1}&region={0}", Settings.Load("data.les").Region, Settings.Load("data.les").SummonerName.Replace(" ", "+")));
            lolNexusBrowser.Hide();

            lolKingBrowser = new LolClientBrowser(LolClient, String.Format("http://www.lolking.net/search?name={1}&region={0}", Settings.Load("data.les").Region, Settings.Load("data.les").SummonerName.Replace(" ", "+")));
            lolKingBrowser.Hide();

            this.Controls.Add(btnLolKing);
            this.Controls.Add(btnLolNexus);
            this.Controls.Add(gearImg);
        }

        private void btnHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void btnLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        void btnLolKing_Click(object sender, EventArgs e)
        {
            if (!lolKingBrowserOpen)
            {
                lolKingBrowser.SetActive(true);
                lolKingBrowser.Show();
                lolKingBrowser.SetUrl(String.Format("http://www.lolking.net/search?name={1}&region={0}", Settings.Load("data.les").Region, Settings.Load("data.les").SummonerName.Replace(" ", "+")));
            }
            else
            {
                lolKingBrowser.Hide();
                lolKingBrowser.SetActive(false);
            }
            lolKingBrowserOpen = !lolKingBrowserOpen;
        }

        void btnLolNexus_Click(object sender, EventArgs e)
        {
            if (!lolNexusBrowserOpen)
            {
                lolNexusBrowser.SetActive(true);
                lolNexusBrowser.Show();
                lolNexusBrowser.SetUrl(String.Format("http://www.lolnexus.com/{0}/search?name={1}&region={0}", Settings.Load("data.les").Region, Settings.Load("data.les").SummonerName.Replace(" ", "+")));
            }
            else
            {
                lolNexusBrowser.Hide();
                lolNexusBrowser.SetActive(false);
            }
            lolNexusBrowserOpen = !lolNexusBrowserOpen;
        }

        void gearImg_Click(object sender, EventArgs e)
        {
            this.prefDialog.ShowDialog(this.LolClient);
        }

        void gearImg_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        void gearImg_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }
       
    }
}
