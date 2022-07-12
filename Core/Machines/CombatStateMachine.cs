using ChatterReborn.CombatStates;
using ChatterReborn.Data;
using ChatterReborn.Managers;
using ChatterReborn.Utils;
using ChatterReborn.Utils.Machine;
using Player;
using SNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Machines
{
    public class CombatStateMachine : StateMachine<COMBAT_Base>
    {

        public void Setup(PlayerAgent playerAgent)
        {
            m_debugLogger = new DebugLoggerObject("CombatState Player" + playerAgent.PlayerSlotIndex);
            this.m_owner = playerAgent;
            this.SetupEnum<CombatState>();
            var m_start_state = new COMBAT_None();
            this.AddState(CombatState.None, m_start_state);
            this.AddState(CombatState.Regular, new COMBAT_Regular());
            this.AddState(CombatState.Hidden, new COMBAT_Hidden());
            this.AddState(CombatState.Extreme, new COMBAT_Extreme());
            this.StartState = m_start_state;

            this.DEBUG_ENABLED = true;
        }


        public override bool IsLocallyOwned => this.m_owner.IsLocallyOwned;

        public bool IsInCombat
        {
            get
            {
                switch (DramaManager.CurrentStateEnum)
                {
                    case DRAMA_State.Combat:
                        return true;
                    case DRAMA_State.Encounter:
                        return true;
                    case DRAMA_State.IntentionalCombat:
                        return true;
                    case DRAMA_State.Survival:
                        return true;
                }

                return false;
            }
        }
        public bool GoToRegular
        {
            get
            {
                return EnemiesDetection > 0 && !GoToExtreme;
            }
        }

        public bool GoToExtreme
        {
            get
            {
                return EnemiesDetection > 7;
            }
        }

        private int EnemiesDetection => EnemyDetectionManager.EnemiesSeenCharacter(this.Owner.CharacterID);

        public void ChangeState(CombatState combatState)
        {
            m_debugLogger.DebugPrint("Now changing to -> " + combatState);
            base.ChangeState(combatState);
        }

        public bool GoToHidden
        {
            get
            {
                return !GoToRegular;
            }
        }

        private DebugLoggerObject m_debugLogger;
        private PlayerAgent m_owner;


        public PlayerAgent Owner => m_owner;

        public bool AllowedToParticipate
        {
            get
            {
                if (this.IsLocallyOwned)
                {
                    return true;
                }
                if (this.m_owner.IsBotOwned())
                {
                    return ConfigurationManager.AllowBotsToParticipateEnabled;
                }
                return SNet.IsMaster && !this.m_owner.Owner.IsBot && DramaChatterManager.IsAllowedToParticipate(this.Owner.CharacterID);
            }
        }

    }
}
