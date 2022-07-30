using ChatterReborn.Attributes;
using ChatterReborn.Extra;
using Il2CppInterop.Runtime.Injection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ChatterReborn.Utils
{
    public static class Il2cppPackager
    {

        private static List<Type> AssemblyTypes => AssemblyUtils.CurrentAssemblies;

        public static void RegisterIl2cppTypes()
        {
            foreach (var type in AssemblyTypes)
            {
                IL2CPPTypeAttribute customAttribute = type.GetCustomAttribute<IL2CPPTypeAttribute>();
                if (customAttribute != null)
                {
                    if (ClassInjector.IsTypeRegisteredInIl2Cpp(type))
                    {
                        continue;
                    }
                    ClassInjector.RegisterTypeInIl2Cpp(type);
                    ChatterDebug.LogMessage("Registering il2cpp monobehavior : " + type.Name);

                    if (customAttribute.AddComponentOnStart)
                    {
                        m_packagesOnLoaded.Add(new ComponentPackage
                        {
                            Type = type,
                            DontDestroyOnLoad = customAttribute.DontDestroyOnLoad
                        });
                    }
                }
            }
        }


        public static void AddComponentsUponStartUp()
        {
            if (m_packagesOnLoaded.Count == 0)
            {
                return;
            }
            foreach (var pak in m_packagesOnLoaded)
            {
                Il2CppSystem.Type il2cppType = pak.Il2CPPType;
                if (il2cppType != null)
                {
                    var component = new UnityEngine.GameObject().AddComponent(il2cppType);
                    ChatterDebug.LogDebug("Adding GameObject with il2cpp monobehavior : " + pak.Type.Name);
                    if (pak.DontDestroyOnLoad)
                    {
                        UnityEngine.Object.DontDestroyOnLoad(component);
                    }
                }
                else
                {
                    ChatterDebug.LogError("ERROR : failed getting Il2cpp Type from type " + pak.Type.Name);
                }
            }
        }

        private static List<ComponentPackage> m_packagesOnLoaded = new List<ComponentPackage>();
    }
}
