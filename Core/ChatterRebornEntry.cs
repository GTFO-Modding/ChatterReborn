using BepInEx.IL2CPP;
using ChatterReborn.Attributes;
using ChatterReborn.Extra;
using ChatterReborn.Utils;
using ChatterRebornSettings;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ChatterReborn
{
    [BepInEx.BepInPlugin(ChatterEntrySettings.Plugin_GUI, ChatterEntrySettings.Plugin_Name, ChatterEntrySettings.Plugin_Version)]
    public class ChatterRebornEntry : BasePlugin
    {

        private static List<Type> AssemblyTypes { get; set; } = new List<Type>();
        internal void RegisterIl2cppTypes()
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

        internal static void AddComponentsUponStartUp()
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

        public override void Load()
        {
            
            
            AssemblyTypes.AddRange(Assembly.GetExecutingAssembly().GetTypes());
            LoadDevComponents();
            this.RegisterIl2cppTypes();
            harmonyInstance = new Harmony("chatter");
            harmonyInstance.PatchAll();
        }

        private Harmony harmonyInstance;

        public static void OnGameLoaded()
        {
            if (m_started)
            {
                return;
            }
            

            m_started = true;
            AddComponentsUponStartUp();
            ManagerInit.SetupAllMangers();
            ChatterDebug.LogMessage("CHATTER INITIATED!");
        }

        private static void LoadDevComponents()
        {

            if (!MiscSettings.LoadDevComponents)
            {
                return;
            }

            if (AssemblyUtils.LoadAssembly("ChatterRebornDev", out var assemblyLoaded))
            {
                AssemblyTypes.AddRange(assemblyLoaded.GetTypes());
            }
        }


        private static bool m_started;


    }
}
