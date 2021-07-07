namespace KeepDrivesAlive
{
    partial class Monitor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor));
            this._drivesMonitoredLabel = new System.Windows.Forms.Label();
            this._monitorTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _drivesMonitoredLabel
            // 
            resources.ApplyResources(this._drivesMonitoredLabel, "_drivesMonitoredLabel");
            this._drivesMonitoredLabel.Name = "_drivesMonitoredLabel";
            // 
            // _monitorTextBox
            // 
            resources.ApplyResources(this._monitorTextBox, "_monitorTextBox");
            this._monitorTextBox.BackColor = System.Drawing.SystemColors.WindowText;
            this._monitorTextBox.ForeColor = System.Drawing.SystemColors.Window;
            this._monitorTextBox.Name = "_monitorTextBox";
            this._monitorTextBox.ReadOnly = true;
            // 
            // Monitor
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._monitorTextBox);
            this.Controls.Add(this._drivesMonitoredLabel);
            this.Name = "Monitor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _drivesMonitoredLabel;
        private System.Windows.Forms.TextBox _monitorTextBox;
    }
}