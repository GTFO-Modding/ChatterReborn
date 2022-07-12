using ChatterReborn.Attributes;
using ChatterReborn.ChatterEvent;
using ChatterReborn.Extra;
using ChatterReborn.Utils;
using GameData;
using LevelGeneration;
using Player;
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

        protected override void OnLevelCleanup()
        {
            this.m_doors_taken_damage.Clear();
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
