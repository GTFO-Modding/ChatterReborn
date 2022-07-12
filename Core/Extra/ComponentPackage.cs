using Il2CppInterop.Runtime;
using System;

namespace ChatterReborn.Extra
{
    public class ComponentPackage
    {
        public Type Type { get; set; }

        public Il2CppSystem.Type Il2CPPType => Il2CppType.From(Type);

        public bool DontDestroyOnLoad { get; set; }
    }
}
