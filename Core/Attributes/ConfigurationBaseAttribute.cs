using ChatterReborn.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    internal abstract class ConfigurationBaseAttribute : Attribute
    {
        public string Title { get; set; }

        public abstract string Header { get; }
        public string Desc { get; set; }
        public abstract Type ValueType { get; }

        public abstract ConfigurationType ConfigType { get; }

        public ConfigurationBaseAttribute(string title, string desc)
        {
            this.Title = title;
            this.Desc = desc;
        }
    }
}
