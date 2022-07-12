using ChatterReborn.Managers;
using HarmonyLib;
using LevelGeneration;

namespace ChatterReborn.Patches
{
    [HarmonyPatch(typeof(LG_ResourceContainer_Storage))]
    class Patch_LG_ResourceContainer_Storage
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(LG_ResourceContainer_Storage.EnablePickupInteractions))]
        static void Post_EnablePickupInteractions(LG_ResourceContainer_Storage __instance)
        {
            foreach (var interaction in __instance.PickupInteractions)
            {
                var item = interaction.GetComponentInParent<Item>();
                if (item != null)
                {
                    LG_PickupItemManager.SetStorage(item, __instance);
                }
            }
        }

        /*[HarmonyPostfix]
        [HarmonyPatch(nameof(LG_ResourceContainer_Storage.SetSpawnNode))]
        static void Post_SetSpawnNode(LG_ResourceContainer_Storage __instance, UnityEngine.GameObject obj)
        {
            Item item = obj.GetComponent<Item>();

            if (item == null)
            {
                item = obj.GetComponent<LG_PickupItem>();
            }


            ItemManager.RegisterItem(item);
        }*/
    }
}
