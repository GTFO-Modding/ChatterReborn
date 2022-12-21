using ChatterReborn.Utils;
using LevelGeneration;
using System.Collections.Generic;
using UnityEngine;

namespace ChatterReborn.Managers
{

    public class LevelGenerationExtManager : ChatterManager<LevelGenerationExtManager>
    {
        public static bool GetItemsFromStorageContainer(LG_ResourceContainer_Storage storage, out List<Item> storageItems)
        {
            storageItems = new List<Item>();
            Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppArrayBase<BoxCollider> boxColliders = storage.GetComponentsInChildren<BoxCollider>();
            if (boxColliders != null && boxColliders.Count > 0)
            {
                Collider[] colliders = Physics.OverlapSphere(storage.transform.position, 2f, LayerManager.MASK_APPLY_CARRY_ITEM, QueryTriggerInteraction.Ignore);
                 if (colliders.Length > 0)
                {
                    for (int i = 0; i < colliders.Length; i++)
                    {
                        Collider collider = colliders[i];
                        var itemComp = collider.GetComponentInParent<Item>();
                        if (itemComp != null)
                        {
                            for (int k = 0; k < boxColliders.Count; k++)
                            {
                                BoxCollider bc = boxColliders[k];
                                if (bc.bounds.Contains(itemComp.transform.position))
                                {
                                    storageItems.Add(itemComp);
                                    break;
                                }
                            }                        
                        }
                    }
                }
            }
            return storageItems.Count > 0;
        }



    }
}
