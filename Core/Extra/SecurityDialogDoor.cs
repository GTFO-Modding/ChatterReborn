using ChatterReborn.Managers;
using ChatterReborn.Utils;
using GameData;
using LevelGeneration;
using Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChatterReborn.Extra
{
    public class SecurityDialogDoor : BaseDialogDoor<LG_SecurityDoor>
    {

        public SecurityDialogDoor(LG_SecurityDoor door) : base(door)
        {

        }

        public void OnOpenSecurityDoor(pDoorState state, bool enabled)
        {
            if (this.m_door.ActiveEnemyWaveData != null && this.m_door.ActiveEnemyWaveData.HasActiveEnemyWave)
            {
                this.m_dialogId = GD.PlayerDialog.apex_door_fight_anticipation;
                this.m_triggerDistance = 20f;
            }
            this.m_OpenDoorCallBack.QueueCallBack(new MinMaxTimer(1f, 2f), this.m_door.transform.position, this.m_door.TryCast<iLG_Door_Core>());
        }

    }
}
