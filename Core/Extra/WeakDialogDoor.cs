using ChatterReborn.Managers;
using ChatterReborn.Utils;
using LevelGeneration;
using Player;
using SNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Extra
{
    public class WeakDialogDoor : BaseDialogDoor<LG_WeakDoor>
    {
        public WeakDialogDoor(LG_WeakDoor door) : base(door)
        {
            this.m_door = door;
        }

        public void OnOpenWeakDoor(pDoorState state, bool enabled)
        {
            this.m_OpenDoorCallBack.QueueCallBack(new MinMaxTimer(0.5f, 1f), this.m_door.transform.position, this.m_door.TryCast<iLG_Door_Core>());
        }


        public void OnReceiveDamage(float damageDelta, float totalDamageTaken, bool sourceZPos, bool isDropin, SNet_Player player)
        {
            DoorManager.AddDoorTakenDamage(this);
        }
    }
}
