using ChatterReborn.Attributes;
using ChatterReborn.ChatterEvent;
using ChatterReborn.Extra;
using ChatterReborn.Utils;
using GameData;
using HarmonyLib;
using LevelGeneration;
using Player;
using SNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Managers
{
    public class DoorManager : ChatterManager<DoorManager>
    {

        protected override void Setup()
        {
            this.m_doors_taken_damage = new Dictionary<int, float>();
        }

        public override void OnLevelCleanUp()
        {
            this.m_doors_taken_damage.Clear();
        }


        protected override void PostSetup()
        {
            this.m_patcher.Patch<LG_SecurityDoor>(nameof(LG_SecurityDoor.Setup), HarmonyPatchType.Postfix, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            this.m_patcher.Patch<LG_WeakDoor>(nameof(LG_WeakDoor.Setup), HarmonyPatchType.Postfix, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        }

        static void LG_SecurityDoor__Setup__Postfix(LG_SecurityDoor __instance)
        {
            if (__instance != null)
            {
                SecurityDialogDoor dialogdoor = new SecurityDialogDoor(__instance);
                __instance.m_sync.add_OnDoorStateChange(new Action<pDoorState, bool>(dialogdoor.OnOpenSecurityDoor));
            }
            else
            {
                Current.DebugPrint("No LG_SecurityDoor ???");
            }
        }

        static void LG_WeakDoor__Setup__Postfix(LG_WeakDoor __instance)
        {
            if (__instance != null)
            {
                WeakDialogDoor dialogdoor = new WeakDialogDoor(__instance);
                __instance.m_sync.add_OnDoorStateChange(new Action<pDoorState, bool>(dialogdoor.OnOpenWeakDoor));
                __instance.m_sync.add_OnDoorGotDamage(new Action<float, float, bool, bool, SNet_Player>(dialogdoor.OnReceiveDamage));
            }
            else
            {
                Current.DebugPrint("No LG_WeakDoor ???");
            }

        }

        public static void AddDoorTakenDamage(WeakDialogDoor weakDialogDoor)
        {
            float coolDown = Clock.Time + 2;
            if (!Current.m_doors_taken_damage.ContainsKey(weakDialogDoor.instanceID))
            {
                Current.m_doors_taken_damage.Add(weakDialogDoor.instanceID, coolDown);
                return;
            }
            Current.m_doors_taken_damage[weakDialogDoor.instanceID] = coolDown;
        }


        public static void TriggerPingDoorDialog(PlayerAgent player, LG_WeakDoor door)
        {
            if (door == null)
            {
                return;
            }
            float lasthit;
            if (Current.m_doors_taken_damage.TryGetValue(door.GetInstanceID(), out lasthit))
            {
                if (lasthit > Clock.Time && door.LastStatus != eDoorStatus.Destroyed)
                {
                    if (ConfigurationManager.PingDoorDialoguesEnabled)
                    {
                        player.WantToStartDialog(GD.PlayerDialog.monsters_breaking_door, true);
                    }                    
                }
            }
        }


        public Dictionary<int, float> m_doors_taken_damage;
    }
}
