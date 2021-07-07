using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections;

namespace KeepDrivesAlive
{    

    class DriveConfigurationCollection : ConfigurationElementCollection
    {

        protected override
            ConfigurationElement CreateNewElement()
        {
            return new DriveConfiguration();
        }

        protected override
            ConfigurationElement CreateNewElement(string caption)
        {
            return new DriveConfiguration(caption);
        }

        protected override Object
            GetElementKey(ConfigurationElement element)
        {
            return ((DriveConfiguration)element).Caption;            
        }

        public void Add(DriveConfiguration driveConfig)
        {            
            BaseAdd(driveConfig, true);            
        }


        public void Remove(string caption)
        {
            BaseRemove(caption);            
        }

        public void Clear()
        {
            BaseClear();            
        }

        new public DriveConfiguration this[string caption]
        {
            get
            {
                return (DriveConfiguration)BaseGet(caption);
            }
        }
       
    }

    class DriveConfiguration : AutoConfiguration
    {        
        
        public DriveConfiguration()
        {

        }

        public DriveConfiguration(string caption) 
        {
            Caption = caption;
        }

        public DriveConfiguration(DriveConfiguration copyFrom): base(copyFrom)
        {
            Caption = copyFrom.Caption;            
        }        

        [ConfigurationProperty("caption",
            IsRequired = true,
            IsKey = true,
            DefaultValue=DEFAULT_CAPTION)]
        public string Caption
        {
            get
            {
                return (string)this["caption"];
            }
            set
            {
                if (value != null)
                {                    
                    this["caption"] = value;
                    OnPropertyChanged();
                }
            }
        }


        public const string DEFAULT_CAPTION = "Unknown";

    }



    class AutoConfiguration : ConfigurationElement, INotifyPropertyChanged
    {        

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                _propertyChanged += value;
            }
            remove
            {
                _propertyChanged -= value;
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string property=null)
        {
            if (_propertyChanged != null)
                _propertyChanged(this, new PropertyChangedEventArgs(property));
        }

        
        
        

        public AutoConfiguration()
        {

        }

        public AutoConfiguration(AutoConfiguration copyFrom)
        {
            Enabled = copyFrom.Enabled;
            Interval = copyFrom.Interval;
        }
        

        [ConfigurationProperty("enabled",
            IsRequired = true, DefaultValue=true)]
        public bool Enabled
        {
            get
            {
                return (bool)this["enabled"];
            }
            set
            {                
                this["enabled"] = value;
                OnPropertyChanged();
            }
        }

        [ConfigurationProperty("interval",
            IsRequired = true, DefaultValue = DEFAULT_VALUE)]
        [IntegerValidator(MinValue = MINIMUM_VALUE, MaxValue = MAXIMUM_VALUE,
            ExcludeRange = false)]
        public int Interval
        {
            get
            {
                return (int)this["interval"];
            }
            set
            {                
                this["interval"] = value;
                OnPropertyChanged();
            }
        }


        public const int DEFAULT_VALUE = 60;
        public const int MAXIMUM_VALUE = 60 * 60;
        public const int MINIMUM_VALUE = 10;

        private event PropertyChangedEventHandler _propertyChanged;
    }



    class DrivesSection : ConfigurationSection
    {        

        [ConfigurationProperty("Drives")]
        [ConfigurationCollection(typeof(DriveConfigurationCollection))]
        public DriveConfigurationCollection Drives
        {
            get
            {
                return (DriveConfigurationCollection)base["Drives"];
            }
        }

        [ConfigurationProperty("Auto")]
        public AutoConfiguration Auto
        {
            get
            {
                return (AutoConfiguration)base["Auto"];
            }            
        }

        public static readonly string Name = typeof(DrivesSection).Name;
    }


    
}
