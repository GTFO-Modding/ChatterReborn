using Agents;
using ChatterReborn.Data;
using ChatterReborn.Managers;
using ChatterReborn.Utils;
using GameData;
using Gear;
using Player;
using System;

namespace ChatterReborn.Extra
{
    public class ResourceFirstPersonDialog : ItemPickUpDialog<ResourcePackFirstPerson>
    {
        private bool last_button_down;

        private bool DEBUG_TEST_ON_YOURSELF = false;

        private CallBackUtils.CallBack<PlayerAgent> m_packDialog;

        public ResourceFirstPersonDialog(ResourcePackFirstPerson resource) : base(resource)
        {

        }

        public override void Setup()
        {
            this.m_packDialog = new CallBackUtils.CallBack<PlayerAgent>(this.CheckPackForDialog);
            this.m_item_pickup.m_interactApplyResource.add_OnInteractionTriggered((Action<PlayerAgent>)this.CheckResourcePacks);
            this.last_button_down = false;
            SetupResourcePack();
            this.BaseSetup();
        }

        private void SetupResourcePack()
        {
            switch (this.m_item_pickup.m_packType)
            {
                case eResourceContainerSpawnType.Health:
                    this.NeedsResourcePack = new ReceiverNeedsResourcePack(this.NeedsHealth);
                    break;
                case eResourceContainerSpawnType.AmmoWeapon:
                    this.NeedsResourcePack = new ReceiverNeedsResourcePack(this.NeedsAmmo);
                    break;
                case eResourceContainerSpawnType.AmmoTool:
                    this.NeedsResourcePack = new ReceiverNeedsResourcePack(this.NeedsTool);
                    break;
                case eResourceContainerSpawnType.Disinfection:
                    this.NeedsResourcePack = new ReceiverNeedsResourcePack(this.NeedsDisinfection);
                    break;
            }
        }

        private delegate bool ReceiverNeedsResourcePack(PlayerAgent receiver);

        private ReceiverNeedsResourcePack NeedsResourcePack { get; set; }

        private bool NeedsAmmo(PlayerAgent receiver)
        {
            return receiver.NeedWeaponAmmo();
        }

        private bool NeedsTool(PlayerAgent receiver)
        {
            return receiver.NeedToolAmmo();
        }
        private bool NeedsHealth(PlayerAgent receiver)
        {
            return receiver.NeedHealth();
        }
        private bool NeedsDisinfection(PlayerAgent receiver)
        {
            return receiver.NeedDisinfection();
        }


        private void CheckResourcePacks(PlayerAgent playerAgent)
        {
            if (playerAgent.IsLocallyOwned)
            {
                this.m_packDialog.QueueCallBack(new MinMaxTimer(0.15f, 0.1f), playerAgent);
            }
        }

        private void CheckPackForDialog(PlayerAgent user)
        {
            if (PlayerBackpackManager.GetBackpack(user.Owner).AmmoStorage.GetBulletsInPack(AmmoType.ResourcePackRel) < 1)
            {
                if (ConfigurationManager.ConsumableDepletedCommentsEnabled)
                {
                    PlayerDialogManager.WantToStartDialogForced(GD.PlayerDialog.consumable_depleted_generic, user);
                }                
            }
        }

        public override void Update()
        {
            if (this.m_item_pickup == null || this.m_item_pickup.Owner == null || !this.m_item_pickup.Owner.IsLocallyOwned)
            {
                return;
            }

            if (this.m_item_pickup.m_actionReceiver == null)
            {
                return;
            }


            var receiver = this.m_item_pickup.m_actionReceiver.TryCast<PlayerAgent>();

            if (receiver != null && (DEBUG_TEST_ON_YOURSELF || !receiver.IsLocallyOwned))
            {
                if (this.last_button_down != this.m_item_pickup.m_lastButtonDown)
                {
                    this.last_button_down = this.m_item_pickup.m_lastButtonDown;
                    bool isMoving = receiver.Noise == Agent.NoiseType.Run || receiver.Noise == Agent.NoiseType.Walk;
                    if (this.last_button_down && receiver.Locomotion != null && isMoving)
                    {
                        if (this.m_item_pickup.Owner.IsLocallyOwned)
                        {
                            if (NeedsResourcePack != null && NeedsResourcePack.Invoke(receiver))
                            {
                                if (ConfigurationManager.InteractionGiveResourcePacksCommentEnabled)
                                {
                                    ExtendedPlayerManager.StartApplyResourcePackDialog(this.m_item_pickup.Owner);
                                }                                
                            }
                        }
                    }
                }
            }
        }
    }
}
