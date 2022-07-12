using Agents;
using ChatterReborn.Managers;
using Player;
using UnityEngine;

namespace ChatterReborn.Components.PlayerBotActionsMonitor
{
    public class PlayerBotActionShareResourcePackDescriptorMonitor : PlayerBotActionDescriptorMonitorBase<PlayerBotActionShareResourcePack, PlayerBotActionShareResourcePack.Descriptor>
    {

        public PlayerBotActionShareResourcePackDescriptorMonitor(PlayerBotAIRootMonitor aiActionMonitor) : base(aiActionMonitor)
        {
            this.m_actionDesc = aiActionMonitor.m_rootAction.m_shareResourceAction;
        }

        public override void UpdateMonitor()
        {
            UpdateShareResource();
        }


        private void UpdateShareResource()
        {        
            if (this.m_actionDesc.Status == PlayerBotActionBase.Descriptor.StatusType.Active)
            {
                var actionBase = this.GetActionBase;
                if (actionBase != null && actionBase.m_state != m_lastActionState)
                {
                    m_lastActionState = actionBase.m_state;
                    if (m_lastActionState == PlayerBotActionShareResourcePack.State.Apply)
                    {
                        OnApplyResourcePack(m_actionDesc.m_receiver);
                    }
                }
                
            }
        }

        private void OnApplyResourcePack(PlayerAgent receiver)
        {
            if (receiver != null)
            {
                if (receiver != this.BotAgent)
                {
                    bool isMoving = receiver.Noise == Agent.NoiseType.Run || receiver.Noise == Agent.NoiseType.Walk;
                    if (isMoving)
                    {
                        if (ConfigurationManager.AllowBotsToParticipateEnabled && ConfigurationManager.InteractionGiveResourcePacksCommentEnabled)
                        {
                            if (IsLocallyOwned)
                            {
                                ExtendedPlayerManager.StartApplyResourcePackDialog(this.BotAgent);
                            }
                        }
                    }
                }
            }
        }

        private PlayerBotActionShareResourcePack.State m_lastActionState;

        
    }
}
