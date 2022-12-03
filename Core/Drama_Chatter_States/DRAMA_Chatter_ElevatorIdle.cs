using ChatterReborn.Managers;
using ChatterReborn.Utils;
using GameData;

namespace ChatterReborn.Drama_Chatter_States
{
    public class DRAMA_Chatter_ElevatorIdle : DRAMA_Chatter_Base
    {
        public override void Enter()
        {
            this.CommonEnter();
        }
        public override void SyncEnter()
        {
            this.CommonEnter();
        }

        private void CommonEnter()
        {
            this.m_level_cleanup_dialog.QueueCallBack(2f);
        }


        private void OnLevelCleanUpDialog()
        {
            if (this.m_level_clean_up_dialog)
            {
                PrisonerDialogManager.WantToStartLocalDialog(GD.PlayerDialog.glottal_stop);
                this.m_level_clean_up_dialog = false;
            }

        }

        public override void OnDestroyed()
        {
            this.m_level_cleanup_dialog.RemoveCallBack();
            base.OnDestroyed();
        }

        public override void OnLevelCleanUp()
        {
            this.m_level_clean_up_dialog = this.m_machine.LastStateName == DRAMA_State.Combat;
            base.OnLevelCleanUp();
        }

        public override void Setup()
        {
            this.m_level_cleanup_dialog = new CallBackUtils.CallBack(this.OnLevelCleanUpDialog);
            base.Setup();
        }

        private CallBackUtils.CallBack m_level_cleanup_dialog;

        private bool m_level_clean_up_dialog = false;
    }
}
