using Player;
using SNetwork;

namespace ChatterReborn.Components.PlayerBotActionsMonitor
{
    public abstract class PlayerBotActionDescriptorMonitorBase<A, D> : PlayerBotMonitorBase where A : PlayerBotActionBase where D : PlayerBotActionBase.Descriptor
    {
        protected PlayerBotActionDescriptorMonitorBase(PlayerBotAIRootMonitor aiActionMonitor) : base(aiActionMonitor)
        {
        }

        protected D m_actionDesc;       

        protected A GetActionBase => this.m_actionDesc.ActionBase != null ? this.m_actionDesc.ActionBase.TryCast<A>() : null;

        protected PlayerBotActionBase.Descriptor.StatusType m_lastStatus;

        
    }
}
