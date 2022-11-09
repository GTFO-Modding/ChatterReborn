using ChatterReborn.Data;
using ChatterReborn.Extra;
using ChatterReborn.Utils;
using GameData;
using Gear;
using HarmonyLib;
using LevelGeneration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatterReborn.Managers
{
    public class DialogItemManager : ChatterManager<DialogItemManager>
    {
        protected override void Setup()
        {
            this.m_pickupDialogs = new Dictionary<int, BasePickUpDialog>();
        }

        public override void OnLevelCleanUp()
        {
            this.m_pickupDialogs.Clear();
        }

        protected override void PostSetup()
        {
            this.m_patcher.Patch<GateKeyItem>(nameof(GateKeyItem.Setup), HarmonyPatchType.Postfix, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            this.m_patcher.Patch<GenericSmallPickupItem_Core>(nameof(GenericSmallPickupItem_Core.SetupFromLevelgen), HarmonyPatchType.Postfix, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            this.m_patcher.Patch<HeavyFogRepellerFirstPerson>(nameof(HeavyFogRepellerFirstPerson.Setup), HarmonyPatchType.Postfix, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            this.m_patcher.Patch<HeavyFogRepellerFirstPerson>(nameof(HeavyFogRepellerFirstPerson.OnWield), HarmonyPatchType.Postfix, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            this.m_patcher.Patch<ResourcePackFirstPerson>(nameof(ResourcePackFirstPerson.Setup), HarmonyPatchType.Postfix, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            this.m_patcher.Patch<ResourcePackPickup>(nameof(ResourcePackPickup.Setup), HarmonyPatchType.Postfix, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            this.m_patcher.Patch<ThrowingWeapon>(nameof(ThrowingWeapon.Throw), HarmonyPatchType.Postfix, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance, new Type[] { typeof(float) } );
        }

        private static void ThrowingWeapon__Throw__Postfix(ThrowingWeapon __instance)
        {
            DramaChatterManager.GetMachine(__instance.Owner)?.CurrentState?.OnThrowConsumable(__instance);
        }

        private static void ResourcePackPickup__Setup__Postfix(ResourcePackPickup __instance)
        {
            ResourcePickUpDialog pack = new ResourcePickUpDialog(__instance);
            DialogItemManager.Current.SetupPickUpInstance(__instance, pack);
        }

        private static void ResourcePackFirstPerson__Setup__Postfix(ResourcePackFirstPerson __instance)
        {
            ResourceFirstPersonDialog pack = new ResourceFirstPersonDialog(__instance);
            Current.SetupPickUpInstance(__instance, pack);
        }

        private static void HeavyFogRepellerFirstPerson__Setup__Postfix(HeavyFogRepellerFirstPerson __instance, ItemDataBlock data)
        {
            DialogHeavyFogRepellerFirstPerson dialogRepeller = new DialogHeavyFogRepellerFirstPerson(__instance);
            Current.SetupPickUpInstance(__instance, dialogRepeller);
        }


        private static void HeavyFogRepellerFirstPerson__OnWield__Postfix(HeavyFogRepellerFirstPerson __instance)
        {
            OnWield(__instance);
        }

        private static void GenericSmallPickupItem_Core__SetupFromLevelgen__Postfix(GenericSmallPickupItem_Core __instance)
        {
            var dialogitem = new GenericSmallItemPickUp(__instance);
            Current.SetupPickUpInstance(__instance, dialogitem);
        }

        private static void GateKeyItem__Setup__Postfix(GateKeyItem __instance)
        {
            var dialogitem = new KeyItemPickUpDialog(__instance.keyPickupCore);
            DialogItemManager.Current.SetupPickUpInstance(__instance.keyPickupCore, dialogitem);
        }


        public static void OnWield(Item wield)
        {
            BasePickUpDialog basePickUpDialog;
            if (Current.m_pickupDialogs.TryGetValue(wield.GetInstanceID(), out basePickUpDialog))
            {
                basePickUpDialog.OnWield();
            }
        }

        public override void Update()
        {
            for (int i = 0; i < AllItems.Count; i++)
            {
                BasePickUpDialog item = AllItems[i];
                if (item != null)
                {
                    item.Update();
                }
            }
        }

        public void SetupPickUpInstance(Item item, BasePickUpDialog pickUpDialog)
        {
            int ID = item.GetInstanceID();

            if (this.m_pickupDialogs.ContainsKey(ID))
            {
                ChatterDebug.LogWarning("An ItemPickUpDialog is attempted to be added that already exist!!");
                return;
            }

            pickUpDialog.Setup();

            this.m_pickupDialogs.Add(ID, pickUpDialog);
        }

        public static List<BasePickUpDialog> AllItems
        {
            get
            {
                return Current.m_pickupDialogs.Values.ToList();
            }
        }
        private Dictionary<int, BasePickUpDialog> m_pickupDialogs;

    }
}
