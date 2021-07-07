using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeepDrivesAlive
{   
    public partial class Monitor : Form
    {
        public class OnRefreshingDrivesEventArgs
        {
            public OnRefreshingDrivesEventArgs(DriveChecker.OnRefreshedEventHandler callback)
            {
                _callback = callback;
            }

            public DriveChecker.OnRefreshedEventHandler Callback {
                get { return _callback; }
            }

            private DriveChecker.OnRefreshedEventHandler _callback = null;
        }

        public delegate void OnRefreshingDrivesEventHandler(object sender, OnRefreshingDrivesEventArgs ea);

        public Monitor(OnRefreshingDrivesEventHandler refreshingEventHandler)
        {
            InitializeComponent();
                        

            _refreshingEventHandler = refreshingEventHandler;
            _refreshingTimer.Interval = 5000;
            _cachedEventArgs = new OnRefreshingDrivesEventArgs(this.UpdateMonitor);
            
        }

        private void OnRefreshing(object sender, EventArgs ea)
        {            
            _monitorTextBox.Text = "";
            _monitorTextBox.Select(_monitorTextBox.Text.Length, 0);
            _monitorTextBox.Update();
            if (_refreshingEventHandler != null)
            {
                _refreshingEventHandler(this, _cachedEventArgs);
            }
        }



        private void UpdateMonitor(object sender, DriveChecker.OnRefreshedEventArgument updates)                          
        {
            if (IsDisposed)
                return;

            Invoke((Action)(
                () => {
                    _refreshingTimer.Stop();
                    if (updates.Drives.Count > 0)
                    {
                        _monitorTextBox.Text += string.Format("{0}\r\n", updates.Group);
                        foreach (string name in updates.Drives)
                        {
                            _monitorTextBox.Text += string.Format("\t{0}\r\n", name);
                        }
                        _monitorTextBox.Text += "\r\n";

                        _monitorTextBox.Select(_monitorTextBox.Text.Length, 0);
                        _monitorTextBox.Update();
                    }
                    _refreshingTimer.Start();
                }
                ));
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Properties.Settings.Default.LastMonitorWindowStateSaved)
            {
                Location = Properties.Settings.Default.LastMonitorWindowLocation;
                Size = Properties.Settings.Default.LastMonitorWindowSize;
                WindowState = Properties.Settings.Default.LastMonitorWindowState;
            }

            _refreshingTimer.Tick += OnRefreshing;
            _refreshingTimer.Start();
            //doing a manual trigger to get an immediate update right after this form is loaded
            OnRefreshing(null, null);
            
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            _refreshingTimer.Stop();
            _refreshingTimer.Tick -= OnRefreshing;

            if (this.WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.LastMonitorWindowLocation = Location;
                Properties.Settings.Default.LastMonitorWindowSize = Size;
            }
            else
            {
                Properties.Settings.Default.LastMonitorWindowLocation = RestoreBounds.Location;
                Properties.Settings.Default.LastMonitorWindowSize = RestoreBounds.Size;
            }

            Properties.Settings.Default.LastMonitorWindowState = WindowState;
            Properties.Settings.Default.LastMonitorWindowStateSaved = true;
            Properties.Settings.Default.Save();

            base.OnClosing(e);
        }


        private OnRefreshingDrivesEventHandler _refreshingEventHandler;
        private OnRefreshingDrivesEventArgs _cachedEventArgs;

        private System.Windows.Forms.Timer _refreshingTimer = new System.Windows.Forms.Timer();
    }
}
