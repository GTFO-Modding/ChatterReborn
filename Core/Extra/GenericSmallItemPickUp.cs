using ChatterReborn.Managers;
using GameData;
using LevelGeneration;
using Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ChatterReborn.Extra
{
    public class GenericSmallItemPickUp : ItemPickUpDialog<GenericSmallPickupItem_Core>
    {
        public GenericSmallItemPickUp(GenericSmallPickupItem_Core item) : base(item)
        {
        }

        public override void Setup()
        {
            //this.m_item_pickup.m_interact.add_OnPickedUpByPlayer((Action<PlayerAgent>)this.OnPickUpMissionItem);
            this.BaseSetup();
        }


        private void OnPickUpMissionItem(PlayerAgent interactionAgent)
        {
            /*if (interactionAgent.IsLocallyOwned && interactionAgent.CourseNode != null)
            {
                LG_LayerType playerLayer = interactionAgent.CourseNode.LayerType;

                var block = WardenObjectiveManager.ActiveWardenObjective(playerLayer);
                if (block != null && block.Type == eWardenObjectiveType.GatherSmallItems)
                {
                    if (WardenObjectiveManager.GetObjectiveItemCollection(playerLayer) != null)
                    {
                        uint dialogID = GD.PlayerDialog.found_item_generic;
                        int count = WardenObjectiveManager.GetObjectiveItemCollection(playerLayer).Count;
                        if (count == 1)
                        {
                            dialogID = GD.PlayerDialog.found_item_generic;
                        }
                        else if (block.Gather_MaxPerZone == count)
                        {
                            dialogID = GD.PlayerDialog.found_generic_item_final;
                        }
                        else if (count > 1)
                        {
                            dialogID = GD.PlayerDialog.found_item_generic;
                        }
                        float randomDelay = UnityEngine.Random.Range(0.5f, 1f);
                        this.m_triggerDialogCallBack.QueueCallBack(randomDelay, interactionAgent, dialogID);
                    }

                }

            }*/
        }
    }
}
