using ChatterReborn.Attributes;
using ChatterReborn.Data;
using ChatterReborn.Utils;
using System.Collections.Generic;

namespace ChatterReborn.Element
{
    [CustomElementType(ElementType.EnemyFilter)]
    public class EnemyFilterElement : CustomElementBase
    {
        public CustomRundown CustomRundownKey { get; set; } = CustomRundown.None;
        public EnemyFilter EnemyFilter { get; set; } = EnemyFilter.None;
        public List<uint> EnemyIDs { get; set; } = new List<uint>();

        public override void OnPostSetup()
        {
            ChatterDebug.LogMessage("Post Setup for EnemyFilterElement for elementID : " + this.ID);
        }
    }
}
