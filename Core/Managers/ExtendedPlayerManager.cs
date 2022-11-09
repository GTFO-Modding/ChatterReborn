using AIGraph;
using ChatterReborn.Data;
using ChatterReborn.Extra;
using ChatterReborn.Utils;
using GameData;
using HarmonyLib;
using LevelGeneration;
using Player;
using System.Collections.Generic;
using UnityEngine;

namespace ChatterReborn.Managers
{
    public class ExtendedPlayerManager : ChatterManager<ExtendedPlayerManager>
    {
        protected override void PostSetup()
        {
            this.m_patcher.Patch<PlayerInventorySynced>(nameof(PlayerInventorySynced.SyncWieldItem), HarmonyPatchType.Postfix, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            this.m_patcher.Patch<PlayerAgent>(nameof(PlayerAgent.CourseNode), HarmonyPatchType.Postfix, MethodType.Setter, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            this.m_patcher.Patch<PlayerAgent>(nameof(PlayerAgent.TriggerMarkerPing), HarmonyPatchType.Postfix, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            this.m_patcher.Patch<PlayerInventoryBase>(nameof(PlayerInventoryBase.DoReload), HarmonyPatchType.Postfix, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        }

        static void PlayerInventoryBase__DoReload__Postfix(PlayerInventoryBase __instance)
        {
            if (__instance.m_wieldedItem != null)
            {
                if (__instance.Owner.Owner.IsBot && __instance.m_wieldedItem.WeaponComp.m_wasOutOfAmmo)
                {
                    __instance.Owner.WantToStartDialog(GD.PlayerDialog.on_reload_weapon_was_out, false);
                }
            }

        }


        static void PlayerAgent__CourseNode__Postfix(PlayerAgent __instance, AIG_CourseNode value)
        {
            if (__instance.CourseNode == value)
            {
                __instance.WantToStartDialog(GD.PlayerDialog.dark_area_enter, false);
            }
        }

        static void PlayerInventorySynced__SyncWieldItem__Postfix(ItemEquippable item)
        {
            DramaChatterManager.OtherPlayerSyncWield(item);
        }

        public static float GetOverallHealth
        {
            get
            {
                float num = 0f;
                float num2 = 0f;
                List<PlayerAgent> list = AllPlayersInLevel;
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        PlayerAgent playerAgent = list[i];
                        if (playerAgent != null)
                        {
                            num2 += playerAgent.Damage.GetHealthRel();
                            num += 1f;
                        }
                    }
                }
                return num2 / num;
            }
        }

        public static bool IsPlayerAgentAlive(PlayerAgent playerAgent)
        {
            return playerAgent != null && playerAgent.Alive;
        }

        public static bool LightsAvailable(PlayerAgent player)
        {

            LG_LightCollection lg_LightCollection = LG_LightCollection.Create(player.CourseNode, player.Position, LG_LightCollectionSorting.Distance, 3f);
            if (lg_LightCollection != null)
            {
                var collected = lg_LightCollection.collectedLights;
                for (int i = 0; i < collected.Count; i++)
                {
                    var coll = collected[i];
                    if (coll.light.isActiveAndEnabled && coll.light.GetIntensity() > 0f)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        static void PlayerAgent__TriggerMarkerPing__Postfix(PlayerAgent __instance, ref iPlayerPingTarget target, GameObject targetGameObject, Vector3 worldPos)
        {
            if (target != null && targetGameObject != null && __instance != null && (__instance.IsLocallyOwned || __instance.IsBotOwned()))
            {
                DevToolManager.LogComponents(targetGameObject);

                if (__instance.IsLocallyOwned)
                {
                    LG_ResourceContainer_Storage lg_ResourceContainer_Storage = targetGameObject.GetComponentInParent<LG_ResourceContainer_Storage>();
                    if (lg_ResourceContainer_Storage != null)
                    {
                        bool hit = Physics.Raycast(worldPos, __instance.FPSCamera.Forward, out var raycastHit2, 0.4f, LayerManager.MASK_APPLY_CARRY_ITEM, QueryTriggerInteraction.Ignore);
                        if (hit)
                        {
                            var itemComp = raycastHit2.collider.GetComponent<Item>();
                            if (itemComp == null)
                            {
                                itemComp = raycastHit2.collider.GetComponentInParent<Item>();
                            }
                            if (itemComp != null && itemComp.PickupInteraction != null && itemComp.PickupInteraction.IsActive)
                            {
                                targetGameObject = itemComp.gameObject;
                            }
                        }
                    }
                }

                if (SpecificPingManager.TryToGetItemPingDescriptor(worldPos, targetGameObject, out ItemPingDescriptorBase bestItemPingDescriptor))
                {
                    target.PingTargetStyle = bestItemPingDescriptor.m_style;

                    bestItemPingDescriptor.PlayPingDialog(__instance);
                }
                else if (target.PingTargetStyle == eNavMarkerStyle.PlayerPingDisinfection)
                {
                    PlayerDialogManager.WantToStartDialogForced(GD.PlayerDialog.CL_DisinfectionHere, __instance);
                }
                else if (target.PingTargetStyle == eNavMarkerStyle.PlayerPingResourceBox)
                {
                    PlayerDialogManager.WantToStartDialogForced(GD.PlayerDialog.found_resource_box, __instance);
                }
                else if (target.PingTargetStyle == eNavMarkerStyle.PlayerPingResourceLocker)
                {
                    PlayerDialogManager.WantToStartDialogForced(GD.PlayerDialog.found_resource_locker, __instance);
                }
                else if (target.PingTargetStyle == eNavMarkerStyle.PlayerPingTerminal)
                {
                    PlayerDialogManager.WantToStartDialogForced(GD.PlayerDialog.CL_Terminal, __instance);
                }
                else
                {
                    LG_SecurityDoor lG_SecurityDoor = targetGameObject.GetComponentInParent<LG_SecurityDoor>();
                    if (lG_SecurityDoor != null)
                    {
                        string key = CoolDownType.PingSecurityDoor + "_" + __instance.CharacterID;
                        if (!CoolDownManager.HasCooldown(key))
                        {
                            uint securityDoorDialog = GD.PlayerDialog.security_door_check;
                            if (lG_SecurityDoor.IsCheckpointDoor)
                            {
                                WeightHandler<uint> dialogOptions = WeightHandler<uint>.CreateWeightHandler();

                                dialogOptions.AddValue(GD.PlayerDialog.found_node_door, 2f);
                                dialogOptions.AddValue(GD.PlayerDialog.waypoint_to_checkpoint_activated, 3f);

                                securityDoorDialog = dialogOptions.Best.Value;
                                if (lG_SecurityDoor.LastStatus == eDoorStatus.Closed_LockedWithChainedPuzzle_Alarm)
                                {
                                    securityDoorDialog = GD.PlayerDialog.apex_door_to_checkpoint_spot;
                                }

                            }
                            else if (lG_SecurityDoor.LastStatus == eDoorStatus.Closed_LockedWithNoKey)
                            {
                                securityDoorDialog = GD.PlayerDialog.try_to_open_node_door;
                            }
                            if (ConfigurationManager.PingDoorDialoguesEnabled)
                            {
                                PlayerDialogManager.WantToStartDialogForced(securityDoorDialog, __instance);
                            }
                            CoolDownManager.ApplyCooldown(key, 5f);
                        }
                    }
                    else
                    {
                        DoorManager.TriggerPingDoorDialog(__instance, targetGameObject.GetComponentInParent<LG_WeakDoor>());
                    }
                }
            }
        }

        public static void GetWeaponStats(PlayerAgent playerAgent, out float totalAmmoRel, out int weaponCount)
        {
            totalAmmoRel = 0f;
            weaponCount = 0;

            PlayerBackpack backpack = PlayerBackpackManager.GetBackpack(playerAgent.Owner);

            BackpackItem backpackItem;
            if (backpack.TryGetBackpackItem(InventorySlot.GearStandard, out backpackItem))
            {
                weaponCount++;
                totalAmmoRel += backpack.AmmoStorage.StandardAmmo.RelInPack;
            }
            BackpackItem backpackItem2;
            if (backpack.TryGetBackpackItem(InventorySlot.GearSpecial, out backpackItem2))
            {
                weaponCount++;
                totalAmmoRel += backpack.AmmoStorage.SpecialAmmo.RelInPack;
            }

            totalAmmoRel /= weaponCount;
        }


        public static void GetToolAmmo(PlayerAgent playerAgent, out float toolAmmoRel)
        {
            toolAmmoRel = 0f;
            PlayerBackpack backpack = PlayerBackpackManager.GetBackpack(playerAgent.Owner);
            if (backpack.TryGetBackpackItem(InventorySlot.GearClass, out _))
            {
                toolAmmoRel += backpack.AmmoStorage.ClassAmmo.RelInPack;
            }
        }


        public static int GetTotalAlivePlayers
        {
            get
            {
                int amount = 0;
                for (int i = 0; i < AllPlayersInLevel.Count; i++)
                {
                    PlayerAgent playerAgent = AllPlayersInLevel[i];
                    if (playerAgent != null && playerAgent.Alive)
                    {
                        amount++;
                    }
                }
                return amount;

            }
        }

        public static List<PlayerAgent> GetNonLocalPlayers
        {
            get
            {
                List<PlayerAgent> playerAgents = Il2cppUtils.ToSystemList(PlayerManager.PlayerAgentsInLevel);

                for (int i = 0; i < playerAgents.Count; i++)
                {
                    if (playerAgents[i].IsLocallyOwned)
                    {
                        playerAgents.RemoveAt(i);
                    }
                }

                return playerAgents;
            }
        }

        public static List<PlayerAgent> GetAllPlayers
        {
            get
            {
                return Il2cppUtils.ToSystemList(PlayerManager.PlayerAgentsInLevel);
            }
        }
        public static bool IsCharacterIDLocal(int CharacterID)
        {
            PlayerAgent playerAgent;
            return PlayerManager.TryGetLocalPlayerAgent(out playerAgent) && playerAgent.CharacterID == CharacterID;
        }

        public static bool IscharacterIDBot(int CharacterID)
        {
            foreach (var player in PlayerManager.PlayerAgentsInLevel)
            {
                if (player.CharacterID == CharacterID)
                {
                    var bot = player.GetComponent<PlayerBotAI>();
                    if (bot != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsPlayerSeperated(PlayerAgent playerAgent)
        {
            for (int i = 0; i < AllPlayersInLevel.Count; i++)
            {
                PlayerAgent playerAgent2 = AllPlayersInLevel[i];
                if (playerAgent2 != null && playerAgent2.Alive)
                {
                    Vector3 vector3 = (playerAgent2.Position - playerAgent.Position);
                    if (vector3.magnitude < 40f)
                    {
                        return false;
                    }
                }
            }
            return true;
        }



        public static List<PlayerAgent> AllPlayersInLevel
        {
            get
            {
                return Il2cppUtils.ToSystemList(PlayerManager.PlayerAgentsInLevel);
            }
        }


        public static void StartApplyResourcePackDialog(PlayerAgent playerAgent)
        {
            string cooldownKey = CoolDownType.GiveResource.ToString() + playerAgent.CharacterID;
            if (!CoolDownManager.HasCooldown(cooldownKey))
            {
                WeightHandler<uint> weightHandler = WeightHandler<uint>.CreateWeightHandler();
                weightHandler.AddValue(GD.PlayerDialog.CL_Wait, 3f);
                weightHandler.AddValue(GD.PlayerDialog.CL_GoSlow, 2f);
                weightHandler.AddValue(GD.PlayerDialog.heal_spray_apply_teammate, 5f);             

                if (weightHandler.HasAny)
                {
                    playerAgent.WantToStartDialog(weightHandler.Best.Value, true);
                }
                CoolDownManager.ApplyCooldown(cooldownKey, 2f);
            }
        }


        public static void OnItemTakeDialog(PlayerAgent interactionSourceAgent, uint itemID)
        {
            string cooldownKey = CoolDownType.PickUpResource.ToString() + interactionSourceAgent.CharacterID;
            if (CoolDownManager.HasCooldown(cooldownKey))
            {
                return;
            }

            PlayerBackpack backPack = PlayerBackpackManager.GetLocalOrSyncBackpack(interactionSourceAgent.Owner);
            WeightHandler<uint> dialogHandler = WeightHandler<uint>.CreateWeightHandler();
            if (itemID == GD.Item.AmmoPackWeapon || itemID == GD.Item.AmmoPackTool)
            {
                dialogHandler.AddValue(GD.PlayerDialog.on_pick_up_ammo, 1f);
                if (itemID == GD.Item.AmmoPackWeapon)
                {
                    if (backPack != null && (backPack.AmmoStorage.StandardAmmo.RelInPack + backPack.AmmoStorage.SpecialAmmo.RelInPack) / 2f < 0.2f)
                    {
                        dialogHandler.AddValue(GD.PlayerDialog.picked_up_ammo_depleted, 3f);
                    }
                }
            }
            else if (itemID == GD.Item.MediPack || itemID == GD.Item.DisinfectionPack)
            {
                dialogHandler.AddValue(GD.PlayerDialog.on_pick_up_health, 2f);
                float healthRel = interactionSourceAgent.Damage.GetHealthRel();
                if (itemID == GD.Item.MediPack)
                {
                    if (healthRel <= 0.3f)
                    {
                        dialogHandler.AddValue(GD.PlayerDialog.on_pick_up_health_when_low, 3f);
                        if (healthRel <= 0.2f)
                        {
                            dialogHandler.AddValue(GD.PlayerDialog.found_health_station_response, 1f);
                        }
                    }
                }                
            }

            if (dialogHandler.HasAny)
            {
                uint pickUpDialogID = dialogHandler.Best.Value;
                ChatterDebug.LogWarning("ExtendedPlayerManager.OnItemTakeDialog, triggering resource pickUpDialogID " + pickUpDialogID);
                if (UnityEngine.Random.value < 0.75f)
                {
                    float randomDelay = UnityEngine.Random.Range(0.5f, 0.8f);
                    PrisonerDialogManager.DelayDialogForced(randomDelay, dialogHandler.Best.Value, interactionSourceAgent);
                }
                CoolDownManager.ApplyCooldown(cooldownKey, 5f);
            }
        }
    }
}
