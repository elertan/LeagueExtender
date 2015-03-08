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
            PictureBox pb = new PictureBox();
            pb.Image = LeagueExtender.Properties.Resources.gear_blue;
            pb.Size = pb.Image.Size;
            pb.MouseHover += pb_MouseHover;
            pb.MouseLeave += pb_MouseLeave;
            pb.Click += pb_Click;

            this.Controls.Add(pb);
        }

        void pb_Click(object sender, EventArgs e)
        {
            this.prefDialog.ShowDialog(this.LolClient);
        }

        void pb_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        void pb_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }
       
    }
}
