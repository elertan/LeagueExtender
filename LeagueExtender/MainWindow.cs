using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace LeagueExtender
{
    public partial class MainWindow : Form
    {

        LolClient LolClient;

        ~MainWindow() // For stopping a movement task later
        {
            
        }

        public MainWindow()
        {
            this.LolClient = new LolClient();

            //this.ShowInTaskbar = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //this.TransparencyKey = Color.Yellow;
            this.BackColor = Color.Yellow;

            this.Load += MainWindow_Load;

            this.LolClient.MovedWindow += LolClient_MovedWindow;
            this.LolClient.MinimizedWindow += LolClient_MinimizedWindow;
        }

        void LolClient_MinimizedWindow(object sender, EventArgs e)
        {

        }

        void LolClient_MovedWindow(object sender, EventArgs e)
        {

        }

        void MainWindow_Load(object sender, EventArgs e)
        {
            this.MoveToFront();
            this.Location = this.LolClient.Location;
            Button b = new Button { BackColor = Color.Transparent, Text = "Test" };
            this.Controls.Add(b);
        }

        private void MoveToFront()
        {
            ExternalFeatures.SetForegroundWindow(this.Handle);
        }
       
    }
}
