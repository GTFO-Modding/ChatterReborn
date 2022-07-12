using ChatterReborn.Managers;
using ChatterReborn.Utils;
using GameData;
using Gear;
using Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChatterReborn.Extra
{
    public class DialogHeavyFogRepellerFirstPerson : ItemPickUpDialog<HeavyFogRepellerFirstPerson>
    {
        public DialogHeavyFogRepellerFirstPerson(HeavyFogRepellerFirstPerson turbine) : base(turbine)
        {

        }

        private CallBackUtils.CallBack m_pickup_repellerCallBack;

        public override void OnWield()
        {
            float randomDelay = UnityEngine.Random.Range(2f, 3f);
            this.m_pickup_repellerCallBack.QueueCallBack(randomDelay);
        }

        public override void Setup()
        {
            this.m_pickup_repellerCallBack = new CallBackUtils.CallBack(this.TriggerTurbineDialogue);
        }

        private void TriggerTurbineDialogue()
        {
            if (this.m_item_pickup == null || this.m_item_pickup.Owner == null)
            {
                return;
            }

            pItemData_Custom customData = this.m_item_pickup.GetCustomData();
            if (CarryItemWithGlobalStateManager.TryGetItemInstance(eCarryItemWithGlobalStateType.FogRepeller, customData.byteId, out iCarryItemWithGlobalState iCarryItemWithGlobalState))
            {
                HeavyFogRepellerGlobalState heavyFogRepellerGlobalState = iCarryItemWithGlobalState.TryCast<HeavyFogRepellerGlobalState>();
                if (heavyFogRepellerGlobalState != null && heavyFogRepellerGlobalState.m_repellerSphere != null)
                {
                    if (ConfigurationManager.HeavyFogRepellerCommmentEnabled && StaticGlobalManager.HeavyFogRepellerDialogEnabled)
                    {
                        this.m_item_pickup.Owner.WantToStartDialog(GD.PlayerDialog.decon_unit_stay_close_reminder);
                    }                    
                    ChatterDebug.LogWarning("Triggering Heavy Fog Repeller Dialogue...");
                }
                else
                {
                    ChatterDebug.LogWarning("HeavyFog Repeller instance did not meet all criteria for dialogue trigger...");
                }
            }
            else
            {
                ChatterDebug.LogWarning("Failed getting HeavyFog Repeller instance...");
            }


        }

    }
}
