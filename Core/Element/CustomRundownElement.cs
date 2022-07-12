using ChatterReborn.Attributes;
using ChatterReborn.Data;

namespace ChatterReborn.Element
{

    [CustomElementType(ElementType.CustomRundown)]
    public class CustomRundownElement : CustomElementBase
    {
        public CustomRundown CustomRundownKey { get; set; }
    }
}
