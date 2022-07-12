using ChatterReborn.Extra;
using Player;
using SNetwork;

namespace ChatterReborn.Components.PlayerBotActionsMonitor
{
    public abstract class PlayerBotMonitorBase
    {

        public virtual void UpdateMonitor() { }
        public virtual void FixedUpdateMonitor() { }
        public virtual void LaterUpdateMonitor() { }


        public PlayerBotMonitorBase(PlayerBotAIRootMonitor rootMonitor)
        {
            this.m_rootMonitor = rootMonitor;
            this.m_rootMonitor.AddMonitor(this);
        }

        protected PlayerAgent BotAgent => this.m_rootMonitor.m_agent;

        public bool IsLocallyOwned => SNet.IsMaster;
        public PlayerAIBot Bot => m_rootMonitor.m_bot;

        protected PlayerBotAIRootMonitor m_rootMonitor;

        protected PlayerBotMonitorCollection MonitorCollection => m_rootMonitor.MonitorCollection;

    }
}
