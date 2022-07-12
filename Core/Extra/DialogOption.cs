using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Extra
{
    public struct DialogOption
    {
        public string Label { get; set; }
        public bool Selected { get; set; }

        public DialogOption(string label)
        {
            this.Label = label;
            this.Selected = false;
        }

        public DialogOption(string label, bool selected = false)
        {
            this.Label = label;
            this.Selected = selected;
        }
    }
}
