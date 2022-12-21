using ChatterReborn.Utils;
using Gear;
using Player;

namespace ChatterReborn.Extra
{
    public class ResourcePackPingDescriptor : ItemPingDescriptor
    {

        public WeightHandler<uint> DialogIDs_LowAmmo;


        private bool m_hasAmmo = false;

        private float m_ammo = 0f;

        public override void TryToApplyAmmo(Item item)
        {
            ResourcePackPickup resourcePackPickup = item.TryCast<ResourcePackPickup>();
            if (resourcePackPickup != null)
            {
                this.m_hasAmmo = true;
                this.m_ammo = resourcePackPickup.GetCustomData().ammo;
            }
            else
            {
                this.m_hasAmmo = false;
                this.m_ammo = 0f;
            }
        }

        public override void PlayPingDialog(PlayerAgent playerAgent)
        {
            if (this.m_hasAmmo && this.m_ammo <= 40f)
            {
                if (DialogIDs_LowAmmo != null && DialogIDs_LowAmmo.TryToGetBestValue(out WeightValue<uint> best))
                {
                    playerAgent.WantToStartDialog(best.Value, true);
                    return;
                }
            }

            base.PlayPingDialog(playerAgent);
        }


    }
}
