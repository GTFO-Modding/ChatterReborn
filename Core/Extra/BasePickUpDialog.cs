using ChatterReborn.Managers;
using ChatterReborn.Utils;
using GameData;
using Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Extra
{
    public class BasePickUpDialog
    {
        protected void BaseSetup()
        {
            this.m_triggerDialogCallBack = new CallBackUtils.CallBack<PlayerAgent, uint>(this.TriggerPickUpDialog);
            this.m_triggerDialogForcedCallBack = new CallBackUtils.CallBack<PlayerAgent, uint>(this.TriggerPickUpDialogForced);
            this.m_triggerWantToSay = new CallBackUtils.CallBack<PlayerAgent, uint>(this.TriggerPickSay);
        }

        public virtual void Setup()
        {
            this.BaseSetup();
        }



        private void TriggerPickUpDialog(PlayerAgent user, uint dialogID)
        {
            if (dialogID == 0U)
            {
                return;
            }

            if (ExtendedPlayerManager.IsPlayerAgentAlive(user) && user.IsLocallyOwned)
            {
                PlayerDialogManager.WantToStartDialog(dialogID, user);
            }
        }

        private void TriggerPickUpDialogForced(PlayerAgent user, uint dialogID)
        {
            if (dialogID == 0U)
            {
                return;
            }

            if (ExtendedPlayerManager.IsPlayerAgentAlive(user) && user.IsLocallyOwned)
            {
                PlayerDialogManager.WantToStartDialogForced(dialogID, user);
            }
        }

        public virtual void Update()
        {

        }


        private void TriggerPickSay(PlayerAgent user, uint wantToSay)
        {
            if (wantToSay == 0U)
            {
                return;
            }

            if (ExtendedPlayerManager.IsPlayerAgentAlive(user) && user.IsLocallyOwned)
            {
                PlayerVoiceManager.WantToSay(user.CharacterID, wantToSay);
            }
        }


        public virtual void OnWield()
        {

        }

        protected CallBackUtils.CallBack<PlayerAgent, uint> m_triggerDialogCallBack;

        protected CallBackUtils.CallBack<PlayerAgent, uint> m_triggerDialogForcedCallBack;

        protected CallBackUtils.CallBack<PlayerAgent, uint> m_triggerWantToSay;

        protected GameDataBlockBase<ItemDataBlock> m_dataBlock;

        public string ItemName
        {
            get
            {
                return this.m_dataBlock.name;
            }
        }
        public uint DataBlockID
        {
            get
            {
                return this.m_dataBlock.persistentID;
            }
        }
    }
}
