using ChatterReborn.Element;
using System;

namespace ChatterReborn.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomElementTypeAttribute : Attribute
    {
        private readonly ElementType _elementType;

        public ElementType ElementType => _elementType;

        public CustomElementTypeAttribute(ElementType elementType)
        {
            this._elementType = elementType;
        }
    }
}
