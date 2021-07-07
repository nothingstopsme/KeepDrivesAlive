using System;
using System.ComponentModel;
namespace KeepDrivesAlive
{
    partial class CheckingSettings
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CheckingSettings));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this._autoDetection = new System.Windows.Forms.CheckBox();
            this._autoConfigurationBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._intervalLabel = new System.Windows.Forms.Label();
            this._secsLabel = new System.Windows.Forms.Label();
            this._autoDetectionInterval = new System.Windows.Forms.NumericUpDown();
            this._toolTipForAutoDetection = new System.Windows.Forms.ToolTip(this.components);
            this._saveButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._designationList = new System.Windows.Forms.DataGridView();
            this._enabledColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this._captionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._intervalColumn = new KeepDrivesAlive.DataGridViewNumericUpDownColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this._toolTipForIntervalLabel = new System.Windows.Forms.ToolTip(this.components);
            this._textHolder = new KeepDrivesAlive.SettingsTextHolder();
            ((System.ComponentModel.ISupportInitialize)(this._autoConfigurationBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._autoDetectionInterval)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._designationList)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // _autoDetection
            // 
            resources.ApplyResources(this._autoDetection, "_autoDetection");
            this._autoDetection.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this._autoConfigurationBindingSource, "Enabled", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this._autoDetection.Name = "_autoDetection";
            this._autoDetection.UseVisualStyleBackColor = true;
            // 
            // _autoConfigurationBindingSource
            // 
            this._autoConfigurationBindingSource.DataSource = typeof(KeepDrivesAlive.AutoConfiguration);
            // 
            // _intervalLabel
            // 
            resources.ApplyResources(this._intervalLabel, "_intervalLabel");
            this._intervalLabel.Name = "_intervalLabel";
            // 
            // _secsLabel
            // 
            resources.ApplyResources(this._secsLabel, "_secsLabel");
            this._secsLabel.Name = "_secsLabel";
            // 
            // _autoDetectionInterval
            // 
            this._autoDetectionInterval.DataBindings.Add(new System.Windows.Forms.Binding("Value", this._autoConfigurationBindingSource, "Interval", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this._autoDetectionInterval.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this._autoConfigurationBindingSource, "Enabled", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this._autoDetectionInterval, "_autoDetectionInterval");
            this._autoDetectionInterval.Name = "_autoDetectionInterval";
            // 
            // _saveButton
            // 
            resources.ApplyResources(this._saveButton, "_saveButton");
            this._saveButton.Name = "_saveButton";
            this._saveButton.UseVisualStyleBackColor = true;
            // 
            // _cancelButton
            // 
            resources.ApplyResources(this._cancelButton, "_cancelButton");
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this._designationList);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // _designationList
            // 
            this._designationList.AllowUserToResizeColumns = false;
            this._designationList.AllowUserToResizeRows = false;
            this._designationList.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._designationList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this._designationList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._designationList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._enabledColumn,
            this._captionColumn,
            this._intervalColumn});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this._designationList.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(this._designationList, "_designationList");
            this._designationList.EnableHeadersVisualStyles = false;
            this._designationList.Name = "_designationList";
            this._designationList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this._designationList.RowTemplate.Height = 27;
            // 
            // _enabledColumn
            // 
            this._enabledColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this._enabledColumn.DataPropertyName = "Enabled";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.NullValue = false;
            this._enabledColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this._enabledColumn.FillWeight = 15F;
            resources.ApplyResources(this._enabledColumn, "_enabledColumn");
            this._enabledColumn.Name = "_enabledColumn";
            // 
            // _captionColumn
            // 
            this._captionColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._captionColumn.DataPropertyName = "Caption";
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            this._captionColumn.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this._captionColumn, "_captionColumn");
            this._captionColumn.Name = "_captionColumn";
            // 
            // _intervalColumn
            // 
            this._intervalColumn.DataPropertyName = "Interval";
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.NullValue = false;
            this._intervalColumn.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this._intervalColumn, "_intervalColumn");
            this._intervalColumn.Name = "_intervalColumn";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this._saveButton);
            this.panel1.Controls.Add(this._cancelButton);
            this.panel1.Name = "panel1";
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this._autoDetection);
            this.panel2.Controls.Add(this._intervalLabel);
            this.panel2.Controls.Add(this._secsLabel);
            this.panel2.Controls.Add(this._autoDetectionInterval);
            this.panel2.Name = "panel2";
            // 
            // _textHolder
            // 
            resources.ApplyResources(this._textHolder, "_textHolder");
            // 
            // CheckingSettings
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Name = "CheckingSettings";
            ((System.ComponentModel.ISupportInitialize)(this._autoConfigurationBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._autoDetectionInterval)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._designationList)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox _autoDetection;
        private System.Windows.Forms.Label _intervalLabel;
        private System.Windows.Forms.Label _secsLabel;
        private System.Windows.Forms.BindingSource _autoConfigurationBindingSource;
        private System.Windows.Forms.NumericUpDown _autoDetectionInterval;
        private System.Windows.Forms.ToolTip _toolTipForAutoDetection;
        private System.Windows.Forms.Button _saveButton;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView _designationList;        
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;

        private KeepDrivesAlive.SettingsTextHolder _textHolder;
        private System.Windows.Forms.ToolTip _toolTipForIntervalLabel;
        private System.Windows.Forms.DataGridViewCheckBoxColumn _enabledColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _captionColumn;
        private DataGridViewNumericUpDownColumn _intervalColumn;               
        

        
        
    }
}