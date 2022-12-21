using ChatterReborn.Data;
using ChatterReborn.Extra;
using ChatterReborn.Utils;
using ChatterRebornSettings;
using GameData;
using LevelGeneration;
using Player;
using System.Collections.Generic;
using UnityEngine;

namespace ChatterReborn.Managers
{
    public class SpecificPingManager : ChatterManager<SpecificPingManager>
    {
        private DictionaryExtended<ItemType, ItemPingDescriptor> m_itemsDescriptor;




        protected override void Setup()
        {
            m_itemsDescriptor = new DictionaryExtended<ItemType, ItemPingDescriptor>();
            m_itemsDescriptor.Add(ItemType.Ammo, new ResourcePackPingDescriptor
            {
                DialogIDs = WeightHandler<uint>.CreateWeightHandler(new List<WeightValue<uint>>
                {
                    new WeightValue<uint>(GD.PlayerDialog.found_ammo, 8f),
                    new WeightValue<uint>(GD.PlayerDialog.CL_AmmoHere, 1f),
                }),
                DialogIDs_LowAmmo = WeightHandler<uint>.CreateWeightHandler(new List<WeightValue<uint>>
                {
                    new WeightValue<uint>(GD.PlayerDialog.found_ammo_little, 1f)
                }),
                PingStyle = eNavMarkerStyle.PlayerPingAmmo
            });
            m_itemsDescriptor.Add(ItemType.MedPack, new ResourcePackPingDescriptor
            {
                DialogIDs = WeightHandler<uint>.CreateWeightHandler(new List<WeightValue<uint>>
                {
                    new WeightValue<uint>(GD.PlayerDialog.found_meds, 8f),
                    new WeightValue<uint>(GD.PlayerDialog.CL_MedPackHere, 1f),
                }),
                DialogIDs_LowAmmo = WeightHandler<uint>.CreateWeightHandler(new List<WeightValue<uint>>
                {
                    new WeightValue<uint>(GD.PlayerDialog.found_meds_little, 1f)
                }),
                PingStyle = eNavMarkerStyle.PlayerPingHealth,
            });
            m_itemsDescriptor.Add(ItemType.Tool, new ResourcePackPingDescriptor
            {
                DialogIDs = WeightHandler<uint>.CreateWeightHandler(new List<WeightValue<uint>>
                {
                    new WeightValue<uint>(GD.PlayerDialog.CL_ToolRefillHere, 1f),
                }),
                PingStyle = eNavMarkerStyle.PlayerPingAmmo,
            });

            m_itemsDescriptor.Add(ItemType.DisInfect, new ResourcePackPingDescriptor
            {
                DialogIDs = WeightHandler<uint>.CreateWeightHandler(new List<WeightValue<uint>>
                {
                    new WeightValue<uint>(GD.PlayerDialog.CL_DisinfectionHere, 1f),
                }),
                PingStyle = eNavMarkerStyle.PlayerPingDisinfection,
            });

            m_itemsDescriptor.Add(ItemType.BuffSyringe, new ItemPingDescriptor
            {
                DialogIDs = WeightHandler<uint>.CreateWeightHandler(new List<WeightValue<uint>>
                {
                    new WeightValue<uint>(GD.PlayerDialog.CL_ThereIsAYellowSyringeHere, 1f),
                }),
                PingStyle = eNavMarkerStyle.PlayerPingCarryItem,
            });

            m_itemsDescriptor.Add(ItemType.FogRepeller, new ItemPingDescriptor
            {
                DialogIDs = WeightHandler<uint>.CreateWeightHandler(new List<WeightValue<uint>>
                {
                    new WeightValue<uint>(GD.PlayerDialog.CL_ThereIsAFogRepellerHere, 1f),
                }),
                PingStyle = eNavMarkerStyle.PlayerPingCarryItem,
            });
            m_itemsDescriptor.Add(ItemType.GlowStick, new ItemPingDescriptor
            {
                DialogIDs = WeightHandler<uint>.CreateWeightHandler(new List<WeightValue<uint>>
                {
                    new WeightValue<uint>(GD.PlayerDialog.CL_ThereAreGlowSticksHere, 1f),
                }),
                PingStyle = eNavMarkerStyle.PlayerPingCarryItem,
            });
            m_itemsDescriptor.Add(ItemType.GlowStick_Halloween, new ItemPingDescriptor
            {
                DialogIDs = WeightHandler<uint>.CreateWeightHandler(new List<WeightValue<uint>>
                {
                    new WeightValue<uint>(GD.PlayerDialog.CL_ThereAreGlowSticksHere, 1f),
                }),
                PingStyle = eNavMarkerStyle.PlayerPingCarryItem,
            });
            m_itemsDescriptor.Add(ItemType.GlowStick_Yellow, new ItemPingDescriptor
            {
                DialogIDs = WeightHandler<uint>.CreateWeightHandler(new List<WeightValue<uint>>
                {
                    new WeightValue<uint>(GD.PlayerDialog.CL_ThereAreGlowSticksHere, 1f),
                }),
                PingStyle = eNavMarkerStyle.PlayerPingCarryItem,
            });
            m_itemsDescriptor.Add(ItemType.GlowStick_Christmas, new ItemPingDescriptor
            {
                DialogIDs = WeightHandler<uint>.CreateWeightHandler(new List<WeightValue<uint>>
                {
                    new WeightValue<uint>(GD.PlayerDialog.CL_ThereAreGlowSticksHere, 1f),
                }),
                PingStyle = eNavMarkerStyle.PlayerPingCarryItem,
            });
            m_itemsDescriptor.Add(ItemType.GlueGrenade, new ItemPingDescriptor
            {
                DialogIDs = WeightHandler<uint>.CreateWeightHandler(new List<WeightValue<uint>>
                {
                    new WeightValue<uint>(GD.PlayerDialog.CL_ThereIsAFoamGrenadeHere, 1f),
                }),
                PingStyle = eNavMarkerStyle.PlayerPingCarryItem,
            });
            m_itemsDescriptor.Add(ItemType.HealthSyringe, new ItemPingDescriptor
            {
                DialogIDs = WeightHandler<uint>.CreateWeightHandler(new List<WeightValue<uint>>
                {
                    new WeightValue<uint>(GD.PlayerDialog.CL_ThereIsARedSyringeHere, 1f),
                }),
                PingStyle = eNavMarkerStyle.PlayerPingCarryItem,
            });
            m_itemsDescriptor.Add(ItemType.LongRangeFlashLight, new ItemPingDescriptor
            {
                DialogIDs = WeightHandler<uint>.CreateWeightHandler(new List<WeightValue<uint>>
                {
                    new WeightValue<uint>(GD.PlayerDialog.CL_ThereIsALongRangeFlashlightHere, 1f),
                }),
                PingStyle = eNavMarkerStyle.PlayerPingCarryItem,
            });
            m_itemsDescriptor.Add(ItemType.Tripmine_Explosive, new ItemPingDescriptor
            {
                DialogIDs = WeightHandler<uint>.CreateWeightHandler(new List<WeightValue<uint>>
                {
                    new WeightValue<uint>(GD.PlayerDialog.CL_ThereIsATripMineHere, 1f),
                }),
                PingStyle = eNavMarkerStyle.PlayerPingCarryItem,
            });
            m_itemsDescriptor.Add(ItemType.Tripmine_Glue, new ItemPingDescriptor
            {
                DialogIDs = WeightHandler<uint>.CreateWeightHandler(new List<WeightValue<uint>>
                {
                    new WeightValue<uint>(GD.PlayerDialog.CL_ThereIsAFoamMineHere, 1f),
                }),
                PingStyle = eNavMarkerStyle.PlayerPingCarryItem,
            });
        }

        

        public static bool TryToGetItemPingDescriptor(Vector3 pingWorldPos, GameObject targetGameObject, out ItemPingDescriptor bestItemPingDescriptor)
        {
            bestItemPingDescriptor = null;
            Item targetItem = targetGameObject.GetComponentInParent<Item>();
            if (targetItem != null)
            {
                if (TryGetDescriptorFromItem(targetItem.ItemDataBlock.persistentID, out bestItemPingDescriptor))
                {
                    bestItemPingDescriptor.TryToApplyAmmo(targetItem);
                }
            }

            


            if (bestItemPingDescriptor == null)
            {
                LG_ResourceContainer_Storage lg_ResourceContainer_Storage = targetGameObject.GetComponentInParent<LG_ResourceContainer_Storage>();
                if (lg_ResourceContainer_Storage != null)
                {
                    float closestStorageItemDistance = 0f;
                    Item bestStorageItem = null;
                    if (LevelGenerationExtManager.GetItemsFromStorageContainer(lg_ResourceContainer_Storage, out List<Item> storageItems))
                    {
                        foreach (Item storageItem in storageItems)
                        {
                            if (storageItem.ItemDataBlock != null && (storageItem.ItemDataBlock.inventorySlot == InventorySlot.Consumable || storageItem.ItemDataBlock.inventorySlot == InventorySlot.ResourcePack))
                            {
                                if (storageItem.PickupInteraction != null && storageItem.PickupInteraction.IsActive)
                                {
                                    float storageItemDistance = Vector3.Distance(storageItem.transform.position, pingWorldPos);
                                    if (bestStorageItem == null || closestStorageItemDistance > storageItemDistance)
                                    {
                                        if (TryGetDescriptorFromItem(storageItem.ItemDataBlock.persistentID, out ItemPingDescriptor descriptor))
                                        {
                                            bestStorageItem = storageItem;
                                            closestStorageItemDistance = storageItemDistance;
                                            bestItemPingDescriptor = descriptor;
                                            bestItemPingDescriptor.TryToApplyAmmo(storageItem);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return bestItemPingDescriptor != null;
        }



        public override void Update()
        {
            this.UpdateDebugItems();
        }

        private void UpdateDebugItems()
        {
            if (Settings.Misc.Debug_SpecifigPingManager)
            {
                if (Input.GetKeyDown(KeyCode.L) && PlayerManager.TryGetLocalPlayerAgent(out PlayerAgent playerAgent) && playerAgent.CourseNode != null)
                {
                    var items = GameObject.FindObjectsOfType<Item>();

                    if (items != null)
                    {
                        foreach (var item in items)
                        {
                            if (AIGraph.AIG_CourseNode.TryGetCourseNode(playerAgent.DimensionIndex, item.transform.position, 1000f, out AIGraph.AIG_CourseNode node))
                            {
                                if (node.m_zone.Alias == playerAgent.CourseNode.m_zone.Alias)
                                {
                                    DevToolManager.LogComponents(item.gameObject);
                                }
                            }
                        }
                    }
                }
            }
        }


        public static bool TryGetDescriptorFromItem(uint itemID, out ItemPingDescriptor itemPingDescriptorBase)
        {
            return Current.m_itemsDescriptor.TryGetValue((ItemType)itemID, out itemPingDescriptorBase);
        }

    }
}
