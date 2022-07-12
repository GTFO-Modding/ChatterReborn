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
        private DictionaryExtended<ItemType, ItemPingDescriptorBase> m_itemsDescriptor;

        protected override void OnLevelCleanup()
        {
            StorageColliders.Clear();
        }


        protected override void Setup()
        {
            m_itemsDescriptor = new DictionaryExtended<ItemType, ItemPingDescriptorBase>();
            m_itemsDescriptor.Add(ItemType.Ammo, new ResourcePackPingDescriptor
            {
                m_dialogID = GD.PlayerDialog.found_ammo,
                m_dialogID_Low = GD.PlayerDialog.found_ammo_little,
                m_dialogID_Unique = GD.PlayerDialog.CL_AmmoHere,
                m_dialogID_AlreadyCarrying = GD.PlayerDialog.CL_IGotAmmoSomeoneElseCarryThis,
                m_dialogID_UniqueChance = 0.2f,
                m_style = eNavMarkerStyle.PlayerPingAmmo,
            });
            m_itemsDescriptor.Add(ItemType.MedPack, new ResourcePackPingDescriptor
            {
                m_dialogID = GD.PlayerDialog.found_meds,
                m_dialogID_Low = GD.PlayerDialog.found_meds_little,
                m_dialogID_Unique = GD.PlayerDialog.CL_MedPackHere,
                m_dialogID_UniqueChance = 0.2f,
                m_style = eNavMarkerStyle.PlayerPingHealth,
            });
            m_itemsDescriptor.Add(ItemType.Tool, new ResourcePackPingDescriptor
            {
                m_dialogID = GD.PlayerDialog.CL_ToolRefillHere,
                m_style = eNavMarkerStyle.PlayerPingAmmo,
            });

            m_itemsDescriptor.Add(ItemType.DisInfect, new ResourcePackPingDescriptor
            {
                m_dialogID = GD.PlayerDialog.CL_DisinfectionHere,
                m_style = eNavMarkerStyle.PlayerPingDisinfection,
            });

            m_itemsDescriptor.Add(ItemType.BuffSyringe, new ItemPingDescriptor
            {
                m_dialogID = GD.PlayerDialog.CL_ThereIsAYellowSyringeHere,
                m_style = eNavMarkerStyle.PlayerPingCarryItem,
            });

            m_itemsDescriptor.Add(ItemType.FogRepeller, new ItemPingDescriptor
            {
                m_dialogID = GD.PlayerDialog.CL_ThereIsAFogRepellerHere,
                m_style = eNavMarkerStyle.PlayerPingCarryItem,
            });
            m_itemsDescriptor.Add(ItemType.GlowStick, new ItemPingDescriptor
            {
                m_dialogID = GD.PlayerDialog.CL_ThereAreGlowSticksHere,
                m_style = eNavMarkerStyle.PlayerPingCarryItem,
            });
            m_itemsDescriptor.Add(ItemType.GlowStick_HW, new ItemPingDescriptor
            {
                m_dialogID = GD.PlayerDialog.CL_ThereAreGlowSticksHere,
                m_style = eNavMarkerStyle.PlayerPingCarryItem,
            });
            m_itemsDescriptor.Add(ItemType.GlowStick_Xmas, new ItemPingDescriptor
            {
                m_dialogID = GD.PlayerDialog.CL_ThereAreGlowSticksHere,
                m_style = eNavMarkerStyle.PlayerPingCarryItem,
            });
            m_itemsDescriptor.Add(ItemType.GlueGrenade, new ItemPingDescriptor
            {
                m_dialogID = GD.PlayerDialog.CL_ThereIsAFoamGrenadeHere,
                m_style = eNavMarkerStyle.PlayerPingCarryItem,
            });
            m_itemsDescriptor.Add(ItemType.HealthSyringe, new ItemPingDescriptor
            {
                m_dialogID = GD.PlayerDialog.CL_ThereIsARedSyringeHere,
                m_style = eNavMarkerStyle.PlayerPingCarryItem,
            });
            m_itemsDescriptor.Add(ItemType.LongRangeFlashLight, new ItemPingDescriptor
            {
                m_dialogID = GD.PlayerDialog.CL_ThereIsALongRangeFlashlightHere,
                m_style = eNavMarkerStyle.PlayerPingCarryItem,
            });
            m_itemsDescriptor.Add(ItemType.Tripmine_Explosive, new ItemPingDescriptor
            {
                m_dialogID = GD.PlayerDialog.CL_ThereIsATripMineHere,
                m_style = eNavMarkerStyle.PlayerPingCarryItem,
            });
            m_itemsDescriptor.Add(ItemType.Tripmine_Glue, new ItemPingDescriptor
            {
                m_dialogID = GD.PlayerDialog.CL_ThereIsAFoamMineHere,
                m_style = eNavMarkerStyle.PlayerPingCarryItem,
            });
            base.Setup();
        }

        public static DictionaryExtended<int, BoxCollider> StorageColliders = new DictionaryExtended<int, BoxCollider>();


        

        public static bool TryToGetItemPingDescriptor(Vector3 worldPos, GameObject targetGameObject, out ItemPingDescriptorBase bestItemPingDescriptor)
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

                if (lg_ResourceContainer_Storage == null)
                {
                    Current.DebugPrint("Didn't get <LG_ResourceContainer_Storage> from targetGameObject trying from GetComponentInParent instead");
                    lg_ResourceContainer_Storage = targetGameObject.GetComponent<LG_ResourceContainer_Storage>();
                }

                if (lg_ResourceContainer_Storage != null)
                {
                    Current.DebugPrint("Got <LG_ResourceContainer_Storage> from targetGameObject ...");
                    float closest_magnitude = 0f;
                    Item override_component = null;
                    if (LG_PickupItemManager.GetItemsFromStorage(lg_ResourceContainer_Storage, out List<Item> storageItems))
                    {
                        foreach (Item storageItem in storageItems)
                        {
                            if (storageItem.ItemDataBlock != null && (storageItem.ItemDataBlock.inventorySlot == InventorySlot.Consumable || storageItem.ItemDataBlock.inventorySlot == InventorySlot.ResourcePack))
                            {
                                if (storageItem.PickupInteraction != null && storageItem.PickupInteraction.IsActive)
                                {
                                    float magnitude = (worldPos - storageItem.transform.position).magnitude;
                                    if (override_component == null || closest_magnitude > magnitude)
                                    {
                                        if (TryGetDescriptorFromItem(storageItem.ItemDataBlock.persistentID, out ItemPingDescriptorBase descriptorBase))
                                        {
                                            override_component = storageItem;
                                            closest_magnitude = magnitude;
                                            bestItemPingDescriptor = descriptorBase;
                                            bestItemPingDescriptor.TryToApplyAmmo(storageItem);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (bestItemPingDescriptor == null)
            {
                Current.DebugPrint("No <ItemPingDescriptorBase>!!", eLogType.Error);
            }

            return bestItemPingDescriptor != null;
        }



        public override void Update()
        {
            this.UpdateDebugItems();
        }

        private void UpdateDebugItems()
        {
            if (MiscSettings.Debug_SpecifigPingManager)
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


        public static bool TryGetDescriptorFromItem(uint itemID, out ItemPingDescriptorBase itemPingDescriptorBase)
        {
            return Current.m_itemsDescriptor.TryGetValue((ItemType)itemID, out itemPingDescriptorBase);
        }

    }
}
