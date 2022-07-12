using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Patches
{
    [HarmonyPatch(typeof(ElevatorRide))]
    public class Patch_ElevatorRide
    {
        [HarmonyPatch(nameof(ElevatorRide.OnGSWantToStartExpedition))]
        [HarmonyPostfix]
        static void OnGSWantToStartExpedition()
        {
            if (OnStartExpedition != null)
            {
                OnStartExpedition();
            }
        }

        [HarmonyPatch(nameof(ElevatorRide.DropinElevatorExit))]
        [HarmonyPostfix]
        static void AfterOnDropinElevatorExit()
        {
            if (OnDropinElevatorExit != null)
            {
                OnDropinElevatorExit();
            }
        }

        [HarmonyPatch(nameof(ElevatorRide.StartElevatorRide))]
        [HarmonyPostfix]
        static void AfterOnStartElevatorRide()
        {
            if (OnStartElevatorRide != null)
            {
                OnStartElevatorRide();
            }
        }

        public static Action OnStartExpedition;

        public static Action OnStartElevatorRide;

        public static Action OnDropinElevatorExit;
    }
}
