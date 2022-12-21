using UnityEngine;
using ChatterReborn.Data;
using System;

namespace ChatterReborn.Attributes
{
    internal class ConfigurationKeyCodeAttribute : ConfigurationBaseAttribute
    {
        public ConfigurationKeyCodeAttribute(string title, string desc, KeyCode defaultKey) : base(title, desc)
        {
            DefaultKey = defaultKey;
        }

        public override string Header => "KeyCode";

        public override Type ValueType => typeof(KeyCode);

        public override ConfigurationType ConfigType => ConfigurationType.KeyBind;

        public KeyCode DefaultKey;
    }
}
