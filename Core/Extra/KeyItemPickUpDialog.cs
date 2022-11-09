using ChatterReborn.Managers;
using ChatterReborn.Utils;
using GameData;
using Player;
using System;

namespace ChatterReborn.Extra
{
    public class KeyItemPickUpDialog : ItemPickUpDialog<KeyItemPickup_Core>
    {
        public KeyItemPickUpDialog(KeyItemPickup_Core interactableItem) : base(interactableItem)
        {
            switch (this.DataBlockID)
            {
                case GD.Item.Pickup_redKeyCard:
                    this.m_keyPickUpDialog = GD.PlayerDialog.picked_up_keycard_red_b;
                    this.m_keyPickUpDialogPlural = GD.PlayerDialog.picked_up_keycard_red_a;
                    break;
                case GD.Item.Pickup_blueKeyCard:
                    this.m_keyPickUpDialog = GD.PlayerDialog.picked_up_keycard_blue_b;
                    this.m_keyPickUpDialogPlural = GD.PlayerDialog.picked_up_keycard_blue_a;
                    break;
                case GD.Item.Pickup_greenKeyCard:
                    this.m_keyPickUpDialog = GD.PlayerDialog.picked_up_keycard_green_b;
                    this.m_keyPickUpDialogPlural = GD.PlayerDialog.picked_up_keycard_blue_a;
                    break;
                case GD.Item.Pickup_yellowKeyCard:
                    this.m_keyPickUpDialog = GD.PlayerDialog.picked_up_keycard_yellow_b;
                    this.m_keyPickUpDialogPlural = GD.PlayerDialog.picked_up_keycard_yellow_a;
                    break;
                case GD.Item.Pickup_whiteKeyCard:
                    this.m_keyPickUpDialog = GD.PlayerDialog.picked_up_keycard_white_b;
                    this.m_keyPickUpDialogPlural = GD.PlayerDialog.picked_up_keycard_white_a;
                    break;
                case GD.Item.Pickup_greyKeyCard:
                    this.m_keyPickUpDialog = GD.PlayerDialog.picked_up_keycard_grey_b;
                    this.m_keyPickUpDialogPlural = GD.PlayerDialog.picked_up_keycard_grey_a;
                    break;
                case GD.Item.Pickup_blackKeyCard:
                    this.m_keyPickUpDialog = GD.PlayerDialog.picked_up_keycard_black_b;
                    this.m_keyPickUpDialogPlural = GD.PlayerDialog.picked_up_keycard_black_a;
                    break;
                case GD.Item.Pickup_orangeKeyCard:
                    this.m_keyPickUpDialog = GD.PlayerDialog.picked_up_keycard_orange_b;
                    this.m_keyPickUpDialogPlural = GD.PlayerDialog.picked_up_keycard_orange_a;
                    break;
                case GD.Item.Pickup_purpleKeyCard:
                    this.m_keyPickUpDialog = GD.PlayerDialog.picked_up_keycard_purple_b;
                    this.m_keyPickUpDialogPlural = GD.PlayerDialog.picked_up_keycard_purple_a;
                    break;
                case GD.Item.Pickup_brownKeyCard:
                    this.m_keyPickUpDialog = GD.PlayerDialog.picked_up_keycard_beige_b;
                    this.m_keyPickUpDialogPlural = GD.PlayerDialog.picked_up_keycard_beige_a;
                    break;
                case GD.Item.Pickup_bulkheadKey:
                    this.m_keyPickUpDialog = GD.PlayerDialog.found_the_item;
                    this.m_keyPickUpDialogPlural = GD.PlayerDialog.found_item_generic;
                    break;
                default:
                    ChatterDebug.LogWarning("Invalid KeyCard for DataBlockID " + interactableItem.KeyItem.DataBlockID + " either a vanilla error or the rundown developer is using a keycard color outside the default ones, in this case let it slide but no dialogue will play when key is picked up");
                    break;
            }
        }

        public override void Setup()
        {
            this.m_item_pickup.m_interact.add_OnPickedUpByPlayer(new Action<PlayerAgent>(this.OnPickUpKey));
            base.BaseSetup();
        }

        private void OnPickUpKey(PlayerAgent interactionSourceAgent)
        {
            if (this.m_keyPickUpDialog > 0U)
            {
                ChatterDebug.LogWarning("[KeyItemPickUpDialog.OnPickUp] triggering m_keyPickUpDialog " + this.m_keyPickUpDialog);
                if (ConfigurationManager.KeyCardPickUpDialoguesEnabled && (interactionSourceAgent.IsLocallyOwned || interactionSourceAgent.IsBotOwned()))
                {
                    this.m_triggerDialogForcedCallBack.QueueCallBack(new MinMaxTimer(0.5f, 0.75f), interactionSourceAgent, this.m_keyPickUpDialog);
                }
            }

        }

        private uint m_keyPickUpDialog;

        private uint m_keyPickUpDialogPlural;
    }
}
