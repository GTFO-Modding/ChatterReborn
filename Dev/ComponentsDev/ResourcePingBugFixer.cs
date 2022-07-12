using ChatterReborn.Attributes;
using ChatterReborn.Utils;
using Player;
using UnityEngine;

namespace ChatterReborn.ComponentsDev
{
    [IL2CPPType(AddComponentOnStart = false, DontDestroyOnLoad = false)]
    public class ResourcePingBugFixer : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                if (PlayerManager.TryGetLocalPlayerAgent(out var agent))
                {
                    LocalPlayerAgent localPlayerAgent = agent.TryCast<LocalPlayerAgent>();
                    bool flag8 = Physics.Raycast(localPlayerAgent.CamPos, localPlayerAgent.FPSCamera.Forward, out var raycastHit, 100f, LayerManager.MASK_APPLY_CARRY_ITEM, QueryTriggerInteraction.Ignore);
                    if (flag8)
                    {
                        PlayerPingTarget pingTarget = new PlayerPingTarget
                        {
                            m_pingTargetStyle = eNavMarkerStyle.PlayerPingLookat
                        };
                        localPlayerAgent.TriggerMarkerPing(pingTarget.TryCast<iPlayerPingTarget>(), localPlayerAgent.FPSCamera.CameraRayObject, raycastHit.point);
                        ChatterDebug.LogDebug("PING!");
                    }
                    else
                    {
                        ChatterDebug.LogError("Didn't get raycast hit!");
                    }                    
                }
                else
                {
                    ChatterDebug.LogError("Didn't get playerAgent!");
                }
            }

            if (false)
            {
                if (PlayerManager.TryGetLocalPlayerAgent(out var agent))
                {
                    LocalPlayerAgent localPlayerAgent = agent.TryCast<LocalPlayerAgent>();
                    bool flag8 = Physics.Raycast(localPlayerAgent.CamPos, localPlayerAgent.FPSCamera.Forward, out var raycastHit, 100f, LayerManager.MASK_PING_TARGET, QueryTriggerInteraction.Ignore);
                    if (flag8)
                    {
                        bool flag = Physics.Raycast(raycastHit.point, localPlayerAgent.FPSCamera.Forward, out var raycastHit2, 0.35f, LayerManager.MASK_APPLY_CARRY_ITEM, QueryTriggerInteraction.Ignore);
                        ChatterDebug.LogMessage("Mask : " + LayerMask.LayerToName(raycastHit.collider.gameObject.layer));
                        if (flag)
                        {
                            ChatterDebug.LogMessage("Mask2 : " + LayerMask.LayerToName(raycastHit2.collider.gameObject.layer));
                            agent.Sound.Post(Wwise.EVENTS.TERMINAL_PING_MARKER_SFX);
                        }
                    }
                    else
                    {
                        ChatterDebug.LogError("Didn't get raycast hit!");
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.CapsLock))
            {
                if (PlayerManager.TryGetLocalPlayerAgent(out var agent))
                {
                    LocalPlayerAgent localPlayerAgent = agent.TryCast<LocalPlayerAgent>();
                    bool flag8 = Physics.Raycast(localPlayerAgent.CamPos, localPlayerAgent.FPSCamera.Forward, out var raycastHit, 100f, LayerManager.MASK_PING_TARGET, QueryTriggerInteraction.Ignore);
                    if (flag8)
                    {
                        var box = raycastHit.collider.GetComponent<BoxCollider>();

                        if (box == null)
                        {
                            box = raycastHit.collider.GetComponentInParent<BoxCollider>();
                        }

                        if (box != null)
                        {
                            
                            ChatterDebug.LogMessage("Gizmo Box!");
                            Collider[] hitColliders = Physics.OverlapBox(box.center, box.transform.localScale / 2, Quaternion.identity, LayerManager.MASK_DEFAULT);
                            foreach (var colider in hitColliders)
                            {
                                var pingTarget = colider.gameObject.GetComponent<iPlayerPingTarget>();

                                if (pingTarget == null)
                                {
                                    pingTarget = colider.gameObject.GetComponentInParent<iPlayerPingTarget>();
                                }
                                if (pingTarget != null)
                                {
                                    GuiManager.NavMarkerLayer.PlaceCustomMarker(NavMarkerOption.LookAt, colider.gameObject, "BOX COLLIDDER", 10f, true);
                                }
                            }
                        }
                        else
                        {
                            ChatterDebug.LogError("No BOX!");
                        }
                    }
                    else
                    {
                        ChatterDebug.LogError("Didn't get raycast hit!");
                    }
                }
            }

        }
    }
}
