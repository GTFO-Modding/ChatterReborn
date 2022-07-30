using ChatterReborn.Managers;
using ChatterReborn.Utils;
using Gear;
using LevelGeneration;
using Player;
using UnityEngine;

namespace ChatterReborn.Extra
{
    public class LG_PickupItemDescriptor
    {

        public Item m_item;

        public LG_ResourceContainer_Storage m_storage;

        private bool m_validItem;

        private bool m_setup;


        private BoxCollider m_collider;

        public void OnSyncState(ePickupItemStatus status, pPickupPlacement placement, PlayerAgent interactor, bool isRecall)
        {
            if (status == ePickupItemStatus.PickedUp)
            {
                this.m_storage = null;
            }
            else if (status == ePickupItemStatus.PlacedInLevel)
            {
                if (!this.m_validItem)
                {
                    ChatterDebug.LogError("Not a valid item..");
                    return;
                }
                if (m_item == null)
                {
                    ChatterDebug.LogError("item is null???");
                    return;
                }
            }

        }


        private void OnTakeResourcePack(PlayerAgent interactor)
        {

        }

        private void OnTakeItem(PlayerAgent interactor)
        {

        }


        public void Setup()
        {
            if (this.m_setup)
            {
                return;
            }

            this.m_setup = true;
            this.m_validItem = this.m_item.IsCppType<ResourcePackPickup>() || this.m_item.IsCppType<ConsumablePickup_Core>();
            if (this.m_validItem)
            {
                var storage = this.m_item.GetComponentInParent<LG_ResourceContainer_Storage>();
                LG_PickupItemManager.PickUpDescriptors.Set(this.m_item.GetInstanceID(), this);
                if (storage != null)
                {
                    var collider = storage.GetComponentInParent<BoxCollider>();

                    if (collider != null)
                    {
                        ChatterDebug.LogMessage("Registering a new storage collider");
                        SpecificPingManager.StorageColliders.Set(collider.GetInstanceID(), collider);
                    }
                    this.m_storage = storage;
                    ChatterDebug.LogWarning(m_item.name + " setting up a storage origin to " + this.m_storage.name);
                }
                else
                {
                    ChatterDebug.LogError(m_item.name + " doesn't have a storage origin!");
                }
            }

        }
    }
}
