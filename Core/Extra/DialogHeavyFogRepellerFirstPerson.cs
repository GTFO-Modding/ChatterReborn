using ChatterReborn.Managers;
using ChatterReborn.Utils;
using GameData;
using Gear;

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
            this.m_pickup_repellerCallBack.QueueCallBack(new MinMaxTimer(2f,3f));
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

            if (this.DataBlockID == GD.Item.Carry_HeavyFogRepeller && ConfigurationManager.HeavyFogRepellerCommmentEnabled && StaticGlobalManager.HeavyFogRepellerDialogEnabled)
            {
                this.m_item_pickup.Owner.WantToStartDialog(GD.PlayerDialog.decon_unit_stay_close_reminder, true);
            }

        }

    }
}
