using ChatterReborn;
using Globals;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatterReborn.Patches
{
    public class Patch_GlobalMessage
    {
        [HarmonyPatch(typeof(Global))]
        internal class Inject_Global
        {
            [HarmonyPostfix]
            [HarmonyPatch("OnLevelCleanup")]
            internal static void Post_OnLevelCleanup()
            {

                if (OnCleanUps != null)
                {
                    OnCleanUps();
                }
            }

            [HarmonyPostfix]
            [HarmonyPatch("OnResetSession")]
            internal static void Post_OnResetSession()
            {

                if (OnResetSessions != null)
                {
                    OnResetSessions();
                }
            }
        }

        public static Action OnCleanUps;

        public static Action OnResetSessions;
    }
}
