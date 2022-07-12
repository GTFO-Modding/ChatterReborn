using ChatterReborn.Data;
using GameData;
using Gear;
using Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Extra
{
    public class ResourcePackPingDescriptor : ItemPingDescriptorBase
    {

        public uint m_dialogID_Low;

        public uint m_dialogID_Unique;

        public float m_dialogID_UniqueChance;

        public uint m_dialogID_AlreadyCarrying;

        public bool m_hasAmmo = false;

        public float m_ammo = 0f;

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
                if (this.m_dialogID_Low > 0U)
                {
                    PlayerDialogManager.WantToStartDialogForced(this.m_dialogID_Low, playerAgent);
                    return;
                }
            }

            if (m_dialogID_Unique > 0U && m_dialogID_UniqueChance > 0f)
            {
                if (m_dialogID_UniqueChance == 1f || UnityEngine.Random.value < m_dialogID_UniqueChance)
                {
                    PlayerDialogManager.WantToStartDialogForced(this.m_dialogID_Unique, playerAgent);
                    return;
                }
            }

            base.PlayPingDialog(playerAgent);
        }

    }
}
