using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

namespace LeagueExtender
{
    public class LolClient : IWin32Window
    {

        private ExternalFeatures.RECT _RECT;
        private Size _OrginalSize;
        private CancellationTokenSource _tokenSource;
        private ExternalFeatures.RECT _lastRect;
        private Action _eventAction;
        private bool _movingWindow;

        public event EventHandler MovedWindow;
        public event EventHandler PlacedWindow;
        public event EventHandler MinimizedWindow;
        public event EventHandler ClosedWindow;
        public event EventHandler ClickedWindow;

        private void OnMovedWindow()
        {
            if (MovedWindow != null)
            {
                MovedWindow(this, EventArgs.Empty);
            }
        }
        
        private void OnMinimizedWindow()
        {
            if (MinimizedWindow != null)
            {
                MinimizedWindow(this, EventArgs.Empty);
            }
        }

        private void OnPlacedWindow()
        {
            if (PlacedWindow != null)
            {
                PlacedWindow(this, EventArgs.Empty);
            }
        }

        private void OnClosedWindow()
        {
            if (ClosedWindow != null)
            {
                ClosedWindow(this, EventArgs.Empty);
            }
        }

        private void OnClickedWindow()
        {
            if (ClickedWindow != null)
            {
                ClickedWindow(this, EventArgs.Empty);
            }
        }

        public IntPtr Handle { get; set; }

        /// <summary>
        /// The Size of the window, avoid setting this property since the window won't resize its controls.
        /// </summary>
        public Size Size
        {
            get
            {
                this._RECT = this.GetRect();
                if (this._RECT.Top != -32000)
                {
                    return new Size(this._RECT.Right - this._RECT.Left, this._RECT.Bottom - this._RECT.Top);
                }
                return this._OrginalSize;
            }
            set
            {
                ExternalFeatures.SetWindowPos(this.Handle, IntPtr.Zero, this.Location.X, this.Location.Y, value.Width, value.Height, ExternalFeatures.SetWindowPosFlags.SWP_ASYNCWINDOWPOS);
            }
        }

        public Size OriginalSize
        {
            get
            {
                return this._OrginalSize;
            }
        }

        public Point Location
        {
            get
            {
                this._RECT = this.GetRect();
                if (this._RECT.Top != -32000)
                {
                    return new Point(this._RECT.Left, this._RECT.Top);
                }
                return new Point(-2000, -2000);
            }
        }

        public bool IsMinimized
        {
            get
            {
                return this.GetRect().Top == -32000;
            }
        }

        public bool IsAlive
        {
            get
            {
                return Process.GetProcessesByName("LolClient").Length > 0;
            }
        }

        ~LolClient()
        {
            this._tokenSource.Cancel();
        }

        public LolClient()
        {
            Process[] procs = Process.GetProcessesByName("LolClient");
            if (procs.Length == 0)
            {
                throw new Exception("LolClient not found!");
            }
            this.Handle = procs[0].MainWindowHandle;
            this._tokenSource = new CancellationTokenSource();
            this._lastRect = this.GetRect();
            this._movingWindow = false;
            

            this._OrginalSize = this.GetSize(true);

        }

        //public LolClient(IntPtr Handle)
        //{
        //    this.Handle = Handle;
        //    this._OrginalSize = this.GetSize(true);
        //}


        

        /// <summary>
        /// Returns the size of the window
        /// </summary>
        /// <param name="Force">Forces to get a result (Actives window if wasnt activated before)</param>
        /// <returns></returns>
        private Size GetSize(bool Force = false)
        {
            if (Force)
            {
                ExternalFeatures.SetForegroundWindow(this.Handle);
            }
            ExternalFeatures.RECT rcSize = this.GetRect();
            if (rcSize.Top != -32000)
            {
                return new Size(rcSize.Right - rcSize.Left, rcSize.Bottom - rcSize.Top);
            }
            return new Size(0, 0);
        }

        private ExternalFeatures.RECT GetRect(bool Force = false)
        {
            if (Force)
            {
                ExternalFeatures.SetForegroundWindow(this.Handle);
            }
            ExternalFeatures.RECT rcSize;
            ExternalFeatures.GetWindowRect(this.Handle, out rcSize);
            return rcSize;
        }

        public void ListenForEvents()
        {
            var context = TaskScheduler.FromCurrentSynchronizationContext();
            try
            {
                Task.Factory.StartNew(() =>
                {
                    int clicksDetected = 0;
                    for (;;)
                    {
                        if (this._tokenSource.Token.IsCancellationRequested)
                        {
                            this._tokenSource.Token.ThrowIfCancellationRequested();
                        }
                        ExternalFeatures.RECT rect = this.GetRect();

                        if (rect.Top != this._lastRect.Top || rect.Left != this._lastRect.Left)
                        {
                            Task.Factory.StartNew(() => this.OnMovedWindow(), this._tokenSource.Token, TaskCreationOptions.None, context);
                            this._movingWindow = true;
                        }

                        if (rect.Top == this._lastRect.Top && rect.Left == this._lastRect.Left && this._movingWindow && Control.MouseButtons == MouseButtons.None)
                        {
                            this._movingWindow = false;
                            Task.Factory.StartNew(() => this.OnPlacedWindow(), this._tokenSource.Token, TaskCreationOptions.None, context);
                        }

                        if (Control.MousePosition.X >= this.Location.X &&
                            Control.MousePosition.X <= this.Location.X + this.Size.Width &&
                            Control.MousePosition.Y >= this.Location.Y &&
                            Control.MousePosition.Y <= this.Location.Y + this.Size.Height &&
                            !this._movingWindow &&
                            Control.MouseButtons == MouseButtons.Left)
                        {
                            clicksDetected++;
                            if (clicksDetected > 20)
                            {
                                //Task.Factory.StartNew(() => this.OnClickedWindow(), this._tokenSource.Token, TaskCreationOptions.None, context);
                            }
                        }
                        else
                        {
                            clicksDetected = 0;
                        }

                        if (!this.IsAlive)
                        {
                            Task.Factory.StartNew(() => this.OnClosedWindow(), this._tokenSource.Token, TaskCreationOptions.None, context);
                        }

                        this._lastRect = rect;
                        Thread.Sleep(1);
                    }
                });
            }
            catch (Exception) { }
        }
    }
}
