using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeepDrivesAlive
{
    [ToolboxItem(false)]
    class TextHolder : Component
    {
        protected string Lookup([CallerMemberName] string property = null)
        {
            string value;
            if (!_textTable.TryGetValue(property, out value))
                value = "";

            return value;
        }

        protected void Store(string value, [CallerMemberName] string property = null)
        {
            if (value != null)
                _textTable[property] = value;
        }        

        private Dictionary<string, string> _textTable = new Dictionary<string, string>();
    }

    [ToolboxItem(true)]
    class SettingsTextHolder : TextHolder 
    { 
        [Localizable(true)]
        public string NewRowHeaderText
        {
            get { return Lookup(); }
            set { Store(value); }
        }

        [Localizable(true)]
        public string RulesDuplicatedMessage
        {
            get { return Lookup(); }
            set { Store(value); }
        }

        [Localizable(true)]
        public string RulesDuplicatedTitle
        {
            get { return Lookup(); }
            set { Store(value); }
        }

        [Localizable(true)]
        public string AutoDetectionToolTip
        {
            get { return Lookup(); }
            set { Store(value); }
        }

        [Localizable(true)]
        public string IntervalToolTip
        {
            get { return Lookup(); }
            set { Store(value); }
        }

    }
}
