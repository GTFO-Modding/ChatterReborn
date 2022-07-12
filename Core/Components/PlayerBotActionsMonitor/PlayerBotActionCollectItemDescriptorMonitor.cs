using ChatterReborn.Data;
using ChatterReborn.Managers;
using Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Components.PlayerBotActionsMonitor
{
    public class PlayerBotActionCollectItemDescriptorMonitor : PlayerBotActionDescriptorMonitorBase<PlayerBotActionCollectItem, PlayerBotActionCollectItem.Descriptor>
    {
        public PlayerBotActionCollectItemDescriptorMonitor(PlayerBotAIRootMonitor aiActionMonitor) : base(aiActionMonitor)
        {
            this.m_actionDesc = aiActionMonitor.m_rootAction.m_collectItemAction;            
        }

        public override void UpdateMonitor()
        {
            if (this.m_actionDesc.Status != m_lastStatus)
            {
                m_lastStatus = this.m_actionDesc.Status;
                OnSwitchStatus();
            }
        }


        private void OnSwitchStatus()
        {
            if (m_lastStatus == PlayerBotActionBase.Descriptor.StatusType.Successful)
            {
                if (this.m_actionDesc.TargetItem != null)
                {
                    if (ConfigurationManager.AllowBotsToParticipateEnabled)
                    {
                        if (IsLocallyOwned && ConfigurationManager.ResourcePickUpCommentsEnabled)
                        {
                            ExtendedPlayerManager.OnItemTakeDialog(this.BotAgent, this.m_actionDesc.TargetItem.ItemDataBlock.persistentID);
                        }                            
                    }
                }
            }
        }


    }
}
