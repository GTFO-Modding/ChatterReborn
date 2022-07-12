using ChatterReborn.Data;
using ChatterReborn.Managers;
using ChatterReborn.Utils;
using GameData;
using Player;
using UnityEngine;
using static ChatterReborn.Utils.CallBackUtils;

namespace ChatterReborn.CombatStates
{
    public class COMBAT_Extreme : COMBAT_Base
    {

        public override void Update()
        {
            CommonUpdate();
        }

        public override void SyncUpdate()
        {
            CommonUpdate();
        }

        private void CommonUpdate()
        {
            if (!this.m_machine.IsInCombat)
            {
                this.m_machine.ChangeState(CombatState.None);
                return;
            }

            if (this.m_machine.GoToRegular)
            {
                this.m_machine.ChangeState(CombatState.Regular);
                return;
            }

            if (this.m_machine.GoToHidden)
            {
                this.m_machine.ChangeState(CombatState.Hidden);
            }
        }


        public override void Setup()
        {
            this.m_callback_triggerSurvivalDialog = new CallBack(TriggerSurvivalDialog);    
        }


        public override void Exit()
        {
            this.m_callback_triggerSurvivalDialog.RemoveCallBack();
        }

        public override void SyncExit()
        {
            this.m_callback_triggerSurvivalDialog.RemoveCallBack();
        }


        public override void Enter()
        {
            this.CommonEnter();
        }

        public override void SyncEnter()
        {
            this.CommonEnter();
        }

        private void CommonEnter()
        {
            this.DebugPrint("Now trying to iniate combat_start dialogue.");
            this.m_callback_triggerSurvivalDialog.QueueCallBack(new MinMaxTimer(0.75f, 2f));
        }
        
        private void TriggerSurvivalDialog()
        {

            if (m_initial_chatter && m_initial_chatterTimer > Time.time)
            {
                return;
            }

            if (PlayerManager.PlayersAreSeparated())
            {
                return;
            }


            m_initial_chatter = true;
            m_initial_chatterTimer = Time.time + 60f;

            if (ConfigurationManager.StartCombatDialogueEnabled && this.m_machine.AllowedToParticipate)
            {
                this.m_machine.Owner.WantToStartDialog(GD.PlayerDialog.combat_start, false);
            }
            
        }

        private bool m_initial_chatter;

        private float m_initial_chatterTimer;


        private CallBack m_callback_triggerSurvivalDialog;
    }
}
