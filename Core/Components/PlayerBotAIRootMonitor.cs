using ChatterReborn.Attributes;
using ChatterReborn.Components.PlayerBotActionsMonitor;
using ChatterReborn.Data;
using ChatterReborn.Extra;
using ChatterReborn.Machines;
using ChatterReborn.Utils;
using Il2CppInterop.Runtime.Attributes;
using Player;
using System.Collections.Generic;
using UnityEngine;

namespace ChatterReborn.Components
{

    [IL2CPPType]
    public class PlayerBotAIRootMonitor : MonoBehaviour
    {

        void Awake()
        {
            m_bot = m_agent.gameObject.GetComponent<PlayerAIBot>();
            m_monitors = new List<PlayerBotMonitorBase>();
            m_debugLogger = new DebugLoggerObject("PlayerBotAIActionMonitor");
            m_monitorCollection = new PlayerBotMonitorCollection();
        }

        [HideFromIl2Cpp]
        public void Setup(DramaChatterMachine machine)
        {
            m_chatterMachine = machine;
            m_agent = machine.Owner;
        }

        void Update()
        {
            if (!IsRootActive())
            {
                return;
            }

            for (int i = 0; i < m_monitors.Count; i++)
            {
                m_monitors[i].UpdateMonitor();
            }
        }

        void FixedUpdate()
        {
            if (!IsRootActive())
            {
                return;
            }

            for (int i = 0; i < m_monitors.Count; i++)
            {
                m_monitors[i].FixedUpdateMonitor();
            }
        }

        void LateUpdate()
        {
            if (!IsRootActive())
            {
                return;
            }

            for (int i = 0; i < m_monitors.Count; i++)
            {
                m_monitors[i].LaterUpdateMonitor();
            }
        }





        [HideFromIl2Cpp]
        private bool IsRootActive()
        {
            if (m_rootAction == null)
            {
                SetupRootAction();
            }

            return m_rootAction != null;
        }

        void SetupRootAction()
        {
            if (m_bot.m_rootAction == null)
            {
                return;
            }


            if (m_bot.m_rootAction.ActionBase == null || !m_bot.m_rootAction.ActionBase.Convert(out m_rootAction))
            {
                return;
            }

            InitiateMonitors();
            m_debugLogger.DebugPrint("RootPlayerBotAction - Has been gotten! for characterID " + m_agent.CharacterID, eLogType.Message);
        }

        private void InitiateMonitors()
        {
            if (m_monitorsSetup)
            {
                m_monitors.Clear();
                m_monitorCollection = new PlayerBotMonitorCollection();
                m_debugLogger.DebugPrint("<<<<<<<<Resetting monitors>>>>>>>", eLogType.Error);
            }

            m_monitorCollection.AttackDescMonitor = new PlayerBotActionAttackDescriptorMonitor(this);
            m_monitorCollection.ShareResourcePackDescMonitor = new PlayerBotActionShareResourcePackDescriptorMonitor(this);
            m_monitorCollection.CollectItemDescMonitor = new PlayerBotActionCollectItemDescriptorMonitor(this);
            m_monitorCollection.IdleDescMonitor = new PlayerBotActionIdleDescriptorMonitor(this);
            m_monitorCollection.SneakingMonitor = new PlayerBotSneakingMonitor(this);
            this.m_monitorsSetup = true;
        }

        public PlayerAgent m_agent;

        public PlayerAIBot m_bot;

        public RootPlayerBotAction m_rootAction;

        private DramaChatterMachine m_chatterMachine;



        [HideFromIl2Cpp]
        public DramaChatterMachine ChatterMachine => m_chatterMachine;

        private List<PlayerBotMonitorBase> m_monitors;


        private DebugLoggerObject m_debugLogger;

        public static float PlayerBotSpecialCallOutTimer;

        private PlayerBotMonitorCollection m_monitorCollection;
        private bool m_monitorsSetup;

        [HideFromIl2Cpp]
        public PlayerBotMonitorCollection MonitorCollection => m_monitorCollection;


        [HideFromIl2Cpp]
        public void AddMonitor(PlayerBotMonitorBase monitor)
        {
            m_monitors.Add(monitor);
        }

        
    }
}
