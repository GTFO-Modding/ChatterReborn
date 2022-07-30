using BepInEx.IL2CPP;
using ChatterReborn.Data;
using ChatterReborn.Utils;
using ChatterRebornSettings;
using HarmonyLib;
using System.Reflection;

namespace ChatterReborn
{
    [BepInEx.BepInPlugin(ChatterEntrySettings.Plugin_GUI, ChatterEntrySettings.Plugin_Name, ChatterEntrySettings.Plugin_Version)]
    public class ChatterRebornEntry : BasePlugin
    {         

        public override void Load()
        {
            Instance = this;
            AssemblyUtils.CurrentAssemblies.AddRange(Assembly.GetExecutingAssembly().GetTypes());
            AssemblyUtils.LoadDevComponents();
            Il2cppPackager.RegisterIl2cppTypes();
            //m_harmonyInstance = new Harmony("chatter");
            //m_harmonyInstance.PatchAll();
            InitiateEntryPatches();
        }

        private void InitiateEntryPatches()
        {
            m_init_patcher = new ChatterPatcher<ChatterRebornEntry>("ChatterEntry");
            m_init_patcher.Patch<StartMainGame>(nameof(StartMainGame.Start), ChatterPatchType.Postfix, BindingFlags.Public | BindingFlags.Instance);
        }

        private static void StartMainGame__Start__Postfix()
        {
            if (m_started)
            {
                return;
            }


            m_started = true;
            GlobalPatcher.InitiatePatches();
            Il2cppPackager.AddComponentsUponStartUp();            
            ManagerInit.SetupAllMangers();
            ChatterDebug.LogMessage("CHATTER INITIATED!");
        }

        private Harmony m_harmonyInstance;

        public static Harmony HarmonyInstance => Instance.m_harmonyInstance;

        public static ChatterRebornEntry Instance;  


        private static bool m_started;


        private ChatterPatcher<ChatterRebornEntry> m_init_patcher;


    }
}
