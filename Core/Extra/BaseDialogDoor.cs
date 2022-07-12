using ChatterReborn.Data;
using ChatterReborn.Managers;
using ChatterReborn.Utils;
using GameData;
using LevelGeneration;
using Player;
using SNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChatterReborn.Extra
{
    public abstract class BaseDialogDoor<T> where T : UnityEngine.Object
    {
        public BaseDialogDoor(T door)
        {
            this.m_door = door;
            this.m_OpenDoorCallBack = new CallBackUtils.CallBack<Vector3, iLG_Door_Core>(this.OpenCommonDoor);
            this.m_dialogId = GD.PlayerDialog.on_start_open_door;
            this.instanceID = door.GetInstanceID();
        }

        public virtual void OnOpenDoor(pDoorState pDoorState, bool enabled)
        {
        }

        protected void OpenCommonDoor(Vector3 doorPosition, iLG_Door_Core door)
        {
            if (door == null)
            {
                return;
            }

            if (door.LastStatus != eDoorStatus.Open)
            {
                return;
            }

            if (this.usedAlready)
            {
                return;
            }

            if (DEBUG_ENABLED)
            {
                ChatterDebug.LogWarning("Placing a Custom Waypoint for current state " + doorPosition);
            }

            GlobalEventManager.LastDoorOpened = Time.time;

            this.usedAlready = true;
            List<PlayerAgent> playerAgents = new List<PlayerAgent>();
            playerAgents.Add(PlayerManager.GetLocalPlayerAgent());
            if (SNet.IsMaster && ConfigurationManager.AllowBotsToParticipateEnabled)
            {
                playerAgents.AddRange(PlayerAgentExtensions.GetAllBotPlayerAgents());
            }
            PlayerAgent chosenPlayerAgent = null;
            float bestDis = 0f;
            foreach (var player in playerAgents)
            {
                float dis = (player.Position - doorPosition).magnitude;
                if (player != null && (chosenPlayerAgent == null || dis < bestDis))
                {
                    if (dis < m_triggerDistance)
                    {
                        chosenPlayerAgent = player;
                        bestDis = dis;
                    }
                }
            }
            if (chosenPlayerAgent != null)
            {
                ChatterDebug.LogWarning("Player Name " + chosenPlayerAgent.Owner.NickName + " triggerd door open dialog [" + PlayerDialogDataBlock.GetBlockName(this.m_dialogId) + "]");
                if (ConfigurationManager.OpenDoorsCommentEnabled)
                {
                    chosenPlayerAgent.WantToStartDialog(this.m_dialogId, false);
                }
            }
        }

        protected T m_door;

        protected float m_triggerDistance = 10f;

        public T Door
        {
            get
            {
                return m_door;
            }
        }
        protected CallBackUtils.CallBack<Vector3, iLG_Door_Core> m_OpenDoorCallBack;
        protected uint m_dialogId;
        public int instanceID;
        protected iLG_Door_Core m_idoor;

        protected bool hasEnemyActive;

        protected bool usedAlready;

        protected bool DEBUG_ENABLED { get; set; }
    }
}
