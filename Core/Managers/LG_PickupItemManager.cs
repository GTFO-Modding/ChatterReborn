using ChatterReborn.Extra;
using ChatterReborn.Utils;
using LevelGeneration;
using System;
using System.Collections.Generic;

namespace ChatterReborn.Managers
{
    [Obsolete("This should be reworked since it's useless at the moment..")]
    public class LG_PickupItemManager : ChatterManager<LG_PickupItemManager>
    {
        protected override void Setup()
        {
            this.m_pickUp_descriptors = new DictionaryExtended<int, LG_PickupItemDescriptor>();
           
        }


        public static bool GetItemsFromStorage(LG_ResourceContainer_Storage storage, out List<Item> storageItems)
        {
            storageItems = new List<Item>();
            foreach (var desc in Current.m_pickUp_descriptors)
            {
                if (desc.Value != null && desc.Value.m_item != null)
                {
                    if (desc.Value.m_storage != null)
                    {
                        if (desc.Value.m_storage == storage)
                        {
                            storageItems.Add(desc.Value.m_item);
                        }
                    }
                }
            }
            return storageItems.Count > 0;
        }

        public override void OnLevelCleanUp()
        {
            this.m_pickUp_descriptors.Clear();
        }

        public static void SetStorage(Item targetItem, LG_ResourceContainer_Storage storage)
        {
            foreach (var desc in Current.m_pickUp_descriptors)
            {
                if (desc.Value != null && desc.Value.m_item == targetItem)
                {
                    desc.Value.m_storage = storage;
                    Current.DebugPrint("Setting an origin storage to " + desc.Value.m_item.name);
                    return;
                }
            }
        }


        private DictionaryExtended<int, LG_PickupItemDescriptor> m_pickUp_descriptors;

        public static DictionaryExtended<int, LG_PickupItemDescriptor> PickUpDescriptors { get => Current.m_pickUp_descriptors; }
    }
}
