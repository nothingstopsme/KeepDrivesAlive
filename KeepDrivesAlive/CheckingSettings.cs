using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeepDrivesAlive
{
    

    public partial class CheckingSettings : Form
    {        
        public CheckingSettings()
        {
            InitializeComponent();             
            
            

            _autoDetectionInterval.Maximum = AutoConfiguration.MAXIMUM_VALUE;
            _autoDetectionInterval.Minimum = AutoConfiguration.MINIMUM_VALUE;
            _autoDetectionInterval.Value = AutoConfiguration.DEFAULT_VALUE;                                                                             
            

            DrivesSection drivesSection = (DrivesSection)ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).GetSection(DrivesSection.Name);
            this._autoConfigurationBindingSource.DataSource = drivesSection.Auto;

            _drivesDataSource = new List<DriveConfiguration>(drivesSection.Drives.Count);
            foreach(DriveConfiguration config in drivesSection.Drives)
            {
                _drivesDataSource.Add(new DriveConfiguration(config));
                _enabledCheckAll &= config.Enabled;
            }
                
            //_drivesDataSource = new List<DriveConfiguration>(drivesSection.Drives.Cast<DriveConfiguration>().Select(each => new DriveConfiguration(each)));            
            System.Windows.Forms.BindingSource driveConfigurationBindingSource = new System.Windows.Forms.BindingSource { DataSource = _drivesDataSource };
            _designationList.AutoGenerateColumns = false;
            _designationList.DataSource = driveConfigurationBindingSource;

            _designationList.RowsAdded += onRowsAdded;
            _designationList.ColumnHeaderMouseClick += onColumnHeaderClicked;

            _toolTipForAutoDetection.SetToolTip(_autoDetection, _textHolder.AutoDetectionToolTip);
            _toolTipForIntervalLabel.SetToolTip(_intervalLabel, _textHolder.IntervalToolTip);            

            this._saveButton.Click += onSaveClicked;
            this._cancelButton.Click += onCancelClicked;

            
        }

        private void onRowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataGridView designationList = (DataGridView)sender;

            if (designationList.CurrentRow != null)
                designationList.CurrentRow.HeaderCell.Value = "";
            for (int rowOffset = 0; rowOffset < e.RowCount; ++rowOffset)
            {
                int rowIndex = e.RowIndex + rowOffset;
                DataGridViewRow row = designationList.Rows[rowIndex];
                if (rowIndex == designationList.NewRowIndex)
                    row.HeaderCell.Value = _textHolder.NewRowHeaderText;
                else
                    row.HeaderCell.Value = "";

                

            }
            
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Properties.Settings.Default.LastSettingsWindowStateSaved)
            {                
                Location = Properties.Settings.Default.LastSettingsWindowLocation;
                Size = Properties.Settings.Default.LastSettingsWindowSize;                
                WindowState = Properties.Settings.Default.LastSettingsWindowState;
            }
        }
        

        protected override void OnClosing(CancelEventArgs e)
        {
            

            if (this.WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.LastSettingsWindowLocation = Location;
                Properties.Settings.Default.LastSettingsWindowSize = Size;
            }
            else
            {
                Properties.Settings.Default.LastSettingsWindowLocation = RestoreBounds.Location;
                Properties.Settings.Default.LastSettingsWindowSize = RestoreBounds.Size;
            }

            Properties.Settings.Default.LastSettingsWindowState = WindowState;
            Properties.Settings.Default.LastSettingsWindowStateSaved = true;
            Properties.Settings.Default.Save();

            base.OnClosing(e);
        }

        private void onColumnHeaderClicked(object sender, DataGridViewCellMouseEventArgs e)
        {

            DataGridView designationList = (DataGridView)sender;
            if (designationList.Columns[e.ColumnIndex] == _enabledColumn)
            {                
                _enabledCheckAll = !_enabledCheckAll;
                DataGridViewCell currentCell = designationList.CurrentCell;
                //setting the CurrentCell to null and setting it back later to trigger the display update
                designationList.CurrentCell = null;
                //skipping the last row, which is reserved for the new row creation
                for (int rowIndex = 0; rowIndex < designationList.RowCount - 1; ++rowIndex)
                {
                    designationList.Rows[rowIndex].Cells[e.ColumnIndex].Value = _enabledCheckAll;                                        
                }
                designationList.CurrentCell = currentCell;
            }
        }
        

        private void onSaveClicked(object sender, EventArgs e)
        {
            Configuration configurationToBeSaved = ((AutoConfiguration)_autoConfigurationBindingSource.DataSource).CurrentConfiguration;
            DrivesSection drivesSection = (DrivesSection)configurationToBeSaved.GetSection(DrivesSection.Name);
            DriveConfigurationCollection driveCollection = drivesSection.Drives;

            driveCollection.Clear();
            HashSet<string> captionSet = new HashSet<string>();
            foreach(DriveConfiguration config in _drivesDataSource)
            {
                if(captionSet.Contains(config.Caption))
                {
                    string message = _textHolder.RulesDuplicatedMessage + config.Caption;
                    string title = _textHolder.RulesDuplicatedTitle;
                    MessageBox.Show(this, message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                    captionSet.Add(config.Caption);
                
                driveCollection.Add(config);                
            }            
            configurationToBeSaved.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(DrivesSection.Name);
            _isModified = true;
            Close();

        }

        private void onCancelClicked(object sender, EventArgs e)
        {
            Close();
        }

        public bool IsModified
        {
            get
            {
                return _isModified;
            }
        }
        
        private bool _isModified = false;              
        private List<DriveConfiguration> _drivesDataSource;
        private bool _enabledCheckAll = true;
        
        
    }
}
