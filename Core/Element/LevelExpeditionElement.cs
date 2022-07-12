using ChatterReborn.Data;
using System.Collections.Generic;

namespace ChatterReborn.Element
{
    public class LevelExpeditionElement : CustomElementBase
    {
        public uint LevelLayoutID { get; set; }

        public class ZoneData
        {
            public int ZoneIndex { get; set; }
            public List<ZoneAction> ZoneEnterActions { get; set; }
        }
    }
}
