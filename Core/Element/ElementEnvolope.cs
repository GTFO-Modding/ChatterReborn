using System.Collections.Generic;

namespace ChatterReborn.Element
{
    public class ElementEnvolope<T> where T : CustomElementBase
    {
        public List<T> Elements { get; set; }

        public uint LastElementID { get; set; }
    }
}
