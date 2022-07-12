using ChatterReborn.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Attributes
{
    internal class ConfigurationToggleAttribute : ConfigurationBaseAttribute
    {
        public ConfigurationToggleAttribute(string title, string desc, bool defaultValue) : base(title, desc)
        {
            this.Default = defaultValue;
        }

        public override ConfigurationType ConfigurationType => ConfigurationType.Toggle;

        public override string Header => "Enabled";

        public override Type ValueType => typeof(bool);
        public bool Default { get; set; }
    }
}
