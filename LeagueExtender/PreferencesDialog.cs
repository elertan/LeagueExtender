using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeagueExtender
{
    public class PreferencesDialog : Form
    {
        public PreferencesDialog(LolClient client)
        {
            this.Text = "LeagueExtender Preferences";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Icon = LeagueExtender.Properties.Resources.gear_blue_ico;

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Button btnExit = new Button();
            btnExit.Text = "Exit LeagueExtender";
            btnExit.Width = 130;
            btnExit.Location = new Point(this.Size.Width / 2 - btnExit.Width / 2, this.Size.Height - 45 - btnExit.Height);
            btnExit.Click += btnExit_Click;

            this.Controls.Add(btnExit);
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
