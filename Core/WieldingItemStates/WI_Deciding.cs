using ChatterReborn.Data;
using ChatterReborn.Managers;

namespace ChatterReborn.WieldingItemStates
{
    public class WI_Deciding : WI_Base
    {
        public override void Update()
        {
            if (this.IsEnemieScannerHeldAndAimed && !CoolDownManager.HasCooldown(CoolDownType.EnemyScanDialog))
            {
                Machine.ChangeState(WI_State.EnemyScanning);
                return;
            }
        }
    }
}
