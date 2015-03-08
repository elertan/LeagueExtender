﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeagueExtender
{
    public class LolClientBrowser : Form
    {

        LolClient clntLol;
        WebBrowser wBrowser;

        bool isActive = false;

        public LolClientBrowser(LolClient client, string url)
        {
            clntLol = client;
            InitializeComponent(url);

            this.ShowInTaskbar = false;
            this.wBrowser.ScriptErrorsSuppressed = true;
            this.wBrowser.WebBrowserShortcutsEnabled = false;
            this.wBrowser.IsWebBrowserContextMenuEnabled = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Load += LolClientBrowser_Load;

            clntLol.MovedWindow += clntLol_MovedWindow;
            clntLol.PlacedWindow += clntLol_PlacedWindow;
        }

        void LolClientBrowser_Load(object sender, EventArgs e)
        {
            this.Location = GetLocationForLolClient(clntLol);
            this.Size = GetSizeForLolClient(clntLol);

            this.isActive = true;
        }

        void clntLol_PlacedWindow(object sender, EventArgs e)
        {
            if (this.isActive)
            {
                this.Location = GetLocationForLolClient(clntLol);
                ExternalFeatures.SetForegroundWindow(this.Handle);
                this.Show();
            }
        }

        void clntLol_MovedWindow(object sender, EventArgs e)
        {
            this.Hide(); 
        }

        public void SetUrl(string url)
        {
            wBrowser.Url = new Uri(url);
        }

        private void InitializeComponent(string url)
        {
            wBrowser = new WebBrowser
            {
                Url = new Uri(url),
                Dock = DockStyle.Fill
            };

            this.Controls.Add(wBrowser);
        }

        private Point GetLocationForLolClient(LolClient client)
        {
            Point ptClient = client.Location;
            ptClient.Offset(0, 80);
            return ptClient;
        }

        private Size GetSizeForLolClient(LolClient client)
        {
            Size szClient = client.Size;
            return new Size(szClient.Width, szClient.Height - 80);
        }

        public void SetActive(bool active)
        {
            this.isActive = active;
            this.Location = GetLocationForLolClient(clntLol);
        }

    }
}
