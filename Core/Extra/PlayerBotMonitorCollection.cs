using ChatterReborn.Components.PlayerBotActionsMonitor;

namespace ChatterReborn.Extra
{
    public class PlayerBotMonitorCollection
    {
        public PlayerBotActionAttackDescriptorMonitor AttackDescMonitor { get; set; }
        public PlayerBotActionShareResourcePackDescriptorMonitor ShareResourcePackDescMonitor { get; set; }
        public PlayerBotActionCollectItemDescriptorMonitor CollectItemDescMonitor { get; set; }
        public PlayerBotActionIdleDescriptorMonitor IdleDescMonitor { get; set; }
        public PlayerBotSneakingMonitor SneakingMonitor { get; set; }
    }
}
