using System;

namespace ChatterReborn.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class IL2CPPTypeAttribute : Attribute
    {
        public bool AddComponentOnStart { get; set; }

        public bool DontDestroyOnLoad { get; set; }
    }
}