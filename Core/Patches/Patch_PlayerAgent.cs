using AIGraph;
using ChatterReborn.Data;
using ChatterReborn.Extra;
using ChatterReborn.Managers;
using ChatterReborn.Utils;
using GameData;
using HarmonyLib;
using LevelGeneration;
using Player;
using UnityEngine;

namespace ChatterReborn.Patches
{
    [HarmonyPatch(typeof(PlayerAgent))]
    public class Patch_PlayerAgent
    {

        public static bool LastDialogEnabled = false;


        [HarmonyPostfix]
        [HarmonyPatch(MethodType.Setter)]
        [HarmonyPatch(nameof(PlayerAgent.CourseNode))]

        static void OnCourseNode(PlayerAgent __instance, AIG_CourseNode value)
        {
            if (__instance.m_courseNode == value)
            {
                __instance.WantToStartDialog(GD.PlayerDialog.dark_area_enter, false);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(PlayerAgent.Setup))]
        static void Post_Setup(PlayerAgent __instance, int characterID)
        {
            //DramaChatterManager.RegisterAgent(__instance);
            if (__instance.IsLocallyOwned)
            {
                ManagerInit.ManagerHandler.RegisterLocalPlayerAgent(__instance.TryCast<LocalPlayerAgent>());
            }
            else
            {
                ManagerInit.ManagerHandler.RegisterPlayerAgent(__instance);
            }            
        }


        [HarmonyPostfix]
        [HarmonyPatch(nameof(PlayerAgent.OnDespawn))]
        static void Post_OnDespawn(PlayerAgent __instance)
        {
            //DramaChatterManager.DeRegisterAgent(__instance);
            if (__instance.IsLocallyOwned)
            {
                ManagerInit.ManagerHandler.DeRegisterLocalPlayerAgent(__instance.TryCast<LocalPlayerAgent>());
            }
            else
            {
                ManagerInit.ManagerHandler.DeRegisterPlayerAgent(__instance);
            }
        }


        [HarmonyPrefix]
        [HarmonyPatch(nameof(PlayerAgent.TriggerMarkerPing))]
        static void BeforePing(PlayerAgent __instance, ref iPlayerPingTarget target, GameObject targetGameObject, Vector3 worldPos)
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


    

    }
}
