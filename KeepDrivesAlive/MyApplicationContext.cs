using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace KeepDrivesAlive
{
    class MyApplicationContext : ApplicationContext
    {
        

        public MyApplicationContext()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MyApplicationContext));

            _contextMenu.MenuItems.Add(_monitorItem);            
            resources.ApplyResources(this._monitorItem, "_monitorItem");            
            _monitorItem.Click += new EventHandler(this.OpenMonitor);

            _contextMenu.MenuItems.Add(_settingsItem);            
            resources.ApplyResources(this._settingsItem, "_settingsItem");            
            _settingsItem.Click += new EventHandler(this.OpenSettings);

            _contextMenu.MenuItems.Add(_exitItem);            
            resources.ApplyResources(this._exitItem, "_exitItem");            
            _exitItem.Click += new EventHandler(this.Exit);

            

            _notifyIcon.ContextMenu = _contextMenu;
            resources.ApplyResources(this._notifyIcon, "_notifyIconToolTip");                      
            _notifyIcon.Icon = Properties.Resources.hard_disk_multi_size;
            _notifyIcon.Visible = true;
        }


        private void OnRefreshingDriveList(object sender, Monitor.OnRefreshingDrivesEventArgs ea)
        {

            _driveChecker.Refresh(ea.Callback);
        }

        private void OpenMonitor(object sender, EventArgs e)
        {
            Monitor monitor = new Monitor(this.OnRefreshingDriveList);
            monitor.FormClosed += OnMonitorClosed;            
            monitor.Show();
            ((MenuItem)sender).Enabled = false;

        }

        private void OpenSettings(object sender, EventArgs e)
        {
            CheckingSettings settings = new CheckingSettings();
            settings.FormClosed += OnSettingsClosed;
            settings.Show();
            ((MenuItem)sender).Enabled = false;
            
        }

        private void OnMonitorClosed(object sender, FormClosedEventArgs e)
        {
            Monitor monitor = (Monitor)sender;
            monitor.FormClosed -= OnMonitorClosed;            
            _monitorItem.Enabled = true;
        }

        private void OnSettingsClosed(object sender, FormClosedEventArgs e)
        {
            CheckingSettings settings = (CheckingSettings)sender;
            settings.FormClosed -= OnSettingsClosed;
            if (settings.IsModified)
            {
                _driveChecker.Reload();
            }
            _settingsItem.Enabled = true;
        }

        private async void Exit(object Sender, EventArgs e)
        {
            _notifyIcon.Visible = false;
            await _driveChecker.Close();
            //Application.Exit();
            this.ExitThread();
        }



        private readonly System.Windows.Forms.NotifyIcon _notifyIcon = new NotifyIcon();
        private readonly System.Windows.Forms.ContextMenu _contextMenu = new ContextMenu();
        private readonly System.Windows.Forms.MenuItem _exitItem = new MenuItem();
        private readonly System.Windows.Forms.MenuItem _settingsItem = new MenuItem();
        private readonly System.Windows.Forms.MenuItem _monitorItem = new MenuItem();

        private readonly DriveChecker _driveChecker = new DriveChecker();
    }
}
