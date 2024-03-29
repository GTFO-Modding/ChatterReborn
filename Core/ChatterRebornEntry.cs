﻿using BepInEx.Unity.IL2CPP;
using ChatterReborn.Attributes;
using ChatterReborn.Utils;
using ChatterRebornSettings;
using HarmonyLib;
using System.Reflection;

namespace ChatterReborn
{
    [BepInEx.BepInPlugin(Settings.Plugin.Plugin_GUI, Settings.Plugin.Plugin_Name, Settings.Plugin.Plugin_Version)]
    public class ChatterRebornEntry : BasePlugin
    {         

        public override void Load()
        {
            Instance = this;
            AssemblyUtils.CurrentAssemblies.AddRange(Assembly.GetExecutingAssembly().GetTypes());
            AssemblyUtils.LoadDevComponents();
            MethodTokenUtils.GenerateMethodTokenIDsFile();
            Il2cppPackager.RegisterIl2cppTypes();
            //m_harmonyInstance = new Harmony("chatter");
            //m_harmonyInstance.PatchAll();
            InitiateEntryPatches();
        }

        private void InitiateEntryPatches()
        {
            m_init_patcher = new ChatterPatcher<ChatterRebornEntry>("ChatterEntry");
            m_init_patcher.Patch<StartMainGame>(new MethodToken(nameof(StartMainGame.Start), Settings.ChatterMethodTokens.StartMainGame__Start__Postfix), HarmonyPatchType.Postfix, BindingFlags.Public | BindingFlags.Instance);
        }


        [MethodDecoderToken]
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


        public static ChatterRebornEntry Instance;  

        private static bool m_started;


        private ChatterPatcher<ChatterRebornEntry> m_init_patcher;


    }
}
