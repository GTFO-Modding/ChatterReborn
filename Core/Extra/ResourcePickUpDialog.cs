using ChatterReborn.Data;
using ChatterReborn.Managers;
using ChatterReborn.Utils;
using GameData;
using Gear;
using Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChatterReborn.Extra
{
    public class ResourcePickUpDialog : ItemPickUpDialog<ResourcePackPickup>
    {
        public ResourcePickUpDialog(ResourcePackPickup pack) : base(pack)
        {
        }

        public override void Setup()
        {
            this.m_item_pickup.m_interact.add_OnPickedUpByPlayer((Action<PlayerAgent>)this.OnResourcePickUp);
            this.BaseSetup();
        }

        private void OnResourcePickUp(PlayerAgent interactionSourceAgent)
        {
            ResourcePackPickup resourcePackPickup = this.m_item_pickup;
            if (resourcePackPickup != null && interactionSourceAgent.IsLocallyOwned)
            {
                if (ConfigurationManager.ResourcePickUpCommentsEnabled)
                {
                    ExtendedPlayerManager.OnItemTakeDialog(interactionSourceAgent, this.DataBlockID);
                }                
            }
        }

    }
}
