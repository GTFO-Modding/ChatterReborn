using System.Collections.Generic;

namespace ChatterReborn.Element
{

    public abstract class CustomElementBase
    {
        public bool enabled { get; set; }
        public uint ID { get; set; }
        public string name { get; set; }

        public virtual void OnPostSetup()
        {

        }
    }
}
