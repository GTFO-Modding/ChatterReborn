using ChatterReborn.Attributes;

namespace ChatterReborn.Element
{

    [CustomElementType(ElementType.SecurityDoor)]
    public class SecurityDoorElement : CustomElementBase
    {
        public string GOName { get; set; }        
        public int ZoneIndex { get; set; }
        public int ExpeditionIndex { get; set; }
    }
}
