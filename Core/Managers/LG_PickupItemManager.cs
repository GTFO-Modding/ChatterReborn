using AIGraph;
using ChatterReborn.Extra;
using ChatterReborn.Utils;
using LevelGeneration;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ChatterReborn.Managers
{

    public class LG_PickupItemManager : ChatterManager<LG_PickupItemManager>
    {
        public static bool GetItemsFromStorageContainer(LG_ResourceContainer_Storage storage, out List<Item> storageItems)
        {
            storageItems = new List<Item>();
            var boxColliders = storage.GetComponentsInChildren<BoxCollider>();
            if (boxColliders != null && boxColliders.Count > 0)
            {
                ChatterDebug.LogMessage("Got " + boxColliders.Length + " box colliders from storage container " + storage.name);

                Collider[] colliders = Physics.OverlapSphere(storage.transform.position, 2f, LayerManager.MASK_APPLY_CARRY_ITEM, QueryTriggerInteraction.Ignore);

                if (colliders.Length > 0)
                {
                    for (int i = 0; i < colliders.Length; i++)
                    {
                        Collider collider = colliders[i];
                        var itemComp = collider.GetComponentInParent<Item>();
                        if (itemComp != null)
                        {
                            bool isInside = false;
                            for (int k = 0; k < boxColliders.Count; k++)
                            {
                                BoxCollider bc = boxColliders[k];
                                if (bc.bounds.Contains(itemComp.transform.position))
                                {
                                    isInside = true;
                                    break;
                                }
                            }
                            ChatterDebug.LogMessage("\tAn item detected -> " + itemComp.PublicName + " is in resource container : " + isInside);
                            if (isInside)
                            {
                                storageItems.Add(itemComp);
                            }                            
                        }
                    }
                }
            }
            else
            {
                ChatterDebug.LogError("Couldn't get box colliders!!");
            }
            return storageItems.Count > 0;
        }



    }
}
