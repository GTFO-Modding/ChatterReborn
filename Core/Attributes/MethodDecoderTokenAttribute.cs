using ChatterReborn.Utils;
using System;

namespace ChatterReborn.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class MethodDecoderTokenAttribute : Attribute
    {
        internal static uint LastTokenID { get; set; } = 4234245245;
    }
}
