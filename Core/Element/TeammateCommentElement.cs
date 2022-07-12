using ChatterReborn.Attributes;
using ChatterReborn.Data;

namespace ChatterReborn.Element
{
    [CustomElementType(ElementType.TeammateComment)]
    public class TeammateCommentElement : CustomElementBase
    {
        public uint DialogID { get; set; }
        public bool PositionBased { get; set; }
        public float Radius { get; set; }
        public FixedVector3 Position { get; set; }
    }
}
