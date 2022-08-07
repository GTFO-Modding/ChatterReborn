using ChatterReborn.Attributes;
using ChatterReborn.Data;
using Globals;
using Player;
using System;
using System.Reflection;

namespace ChatterReborn.Utils
{
    public class GlobalPatcher
    {
        private static bool m_inited;
        public static void InitiatePatches()
        {
            if (m_inited) return;

            m_inited = true;
            m_patcher = new ChatterPatcher<GlobalPatcher>("ChatterGlobal");

            m_patcher.Patch<ElevatorRide>(nameof(ElevatorRide.StartElevatorRide), ChatterPatchType.Postfix, BindingFlags.Public | BindingFlags.Static);
            m_patcher.Patch<ElevatorRide>(nameof(ElevatorRide.OnGSWantToStartExpedition), ChatterPatchType.Postfix, BindingFlags.Public | BindingFlags.Static);
            m_patcher.Patch<ElevatorRide>(nameof(ElevatorRide.DropinElevatorExit), ChatterPatchType.Postfix, BindingFlags.Public | BindingFlags.Static);
            m_patcher.Patch<Global>(nameof(Global.OnLevelCleanup), ChatterPatchType.Postfix, BindingFlags.Public | BindingFlags.Static);
            m_patcher.Patch<Global>(nameof(Global.OnResetSession), ChatterPatchType.Postfix, BindingFlags.Public | BindingFlags.Static);
            m_patcher.Patch<PlayerAgent>(nameof(PlayerAgent.Setup), ChatterPatchType.Postfix, BindingFlags.Public | BindingFlags.Instance);
            m_patcher.Patch<PlayerAgent>(nameof(PlayerAgent.OnDespawn), ChatterPatchType.Postfix, BindingFlags.Public | BindingFlags.Instance);


        }

        [MethodDecoderToken]
        static void PlayerAgent__Setup__Postfix(PlayerAgent __instance, int characterID)
        {
            if (__instance.IsLocallyOwned)
            {
                ManagerInit.ManagerHandler.OnRegisterLocalPlayerAgent(__instance.TryCast<LocalPlayerAgent>());
            }
            else
            {
                ManagerInit.ManagerHandler.OnRegisterPlayerAgent(__instance);
            }
        }


        [MethodDecoderToken]
        static void PlayerAgent__OnDespawn__Postfix(PlayerAgent __instance)
        {
            if (__instance.IsLocallyOwned)
            {
                ManagerInit.ManagerHandler.DeRegisterLocalPlayerAgent(__instance.TryCast<LocalPlayerAgent>());
            }
            else
            {
                ManagerInit.ManagerHandler.DeRegisterPlayerAgent(__instance);
            }
        }


        [MethodDecoderToken]
        private static void ElevatorRide__StartElevatorRide__Postfix()
        {
            ManagerInit.ManagerHandler.OnStartElevatorRide();
        }


        [MethodDecoderToken]
        private static void ElevatorRide__OnGSWantToStartExpedition__Postfix()
        {
            ManagerInit.ManagerHandler.OnStartExpedition();
        }


        [MethodDecoderToken]
        private static void ElevatorRide__DropinElevatorExit__Postfix()
        {
            ManagerInit.ManagerHandler.OnDropInElevatorExit();
        }


        [MethodDecoderToken]
        private static void Global__OnLevelCleanup__Postfix()
        {
            ManagerInit.ManagerHandler.OnLevelCleanup();
        }


        [MethodDecoderToken]
        private static void Global__OnResetSession__Postfix()
        {
            ManagerInit.ManagerHandler.OnResetSession();
        }


        private static ChatterPatcher<GlobalPatcher> m_patcher;
    }
}
