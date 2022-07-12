using ChatterReborn.Components;
using ChatterReborn.Utils;
using ChatterRebornSettings;
using Player;

namespace ChatterReborn.Managers
{
    public class PrisonerDialogManager : ChatterManager<PrisonerDialogManager>
    {
        public static void WantToStartDialog(uint dialogID, int playerID)
        {
            PlayerDialogManager.WantToStartDialog(dialogID, playerID, false, false);
        }




        public static bool IsInDialog
        {
            get
            {
                //return PlayerDialogManager.Current.m_activeDialog != null;
                return false;
            }
        }
        public static void WantToStartDialogForced(uint dialogID, int playerID)
        {
            PlayerDialogManager.WantToStartDialog(dialogID, playerID, false, true);
        }

        public static void WantToStartLocalDialog(uint dialogID)
        {
            if (PlayerManager.TryGetLocalPlayerAgent(out PlayerAgent playerAgent))
            {
                if (playerAgent != null && dialogID > 0U)
                {
                    PlayerDialogManager.WantToStartDialog(dialogID, playerAgent);
                }
            }
        }

        public static void WantToStartLocalDialogForced(uint dialogID)
        {
            if (PlayerManager.TryGetLocalPlayerAgent(out PlayerAgent playerAgent))
            {
                if (playerAgent != null && dialogID > 0U)
                {
                    PlayerDialogManager.WantToStartDialogForced(dialogID, playerAgent);
                }
            }
        }


        public static CallBackUtils.CallBackBase DelayDialogForced(DelayValue timer, uint dialogID, PlayerAgent agent)
        {
            CallBackUtils.CallBack<uint, int> callback = new CallBackUtils.CallBack<uint, int>(WantToStartDialogForced);
            callback.QueueCallBack(timer, dialogID, agent.CharacterID);
            return callback;
        }

        public static CallBackUtils.CallBackBase DelayDialog(DelayValue timer, uint dialogID, PlayerAgent agent)
        {
            CallBackUtils.CallBack<uint, int> callback = new CallBackUtils.CallBack<uint, int>(WantToStartDialog);
            callback.QueueCallBack(timer, dialogID, agent.CharacterID);
            return callback;
        }




        public static CallBackUtils.CallBackBase DelayLocalDialog(DelayValue timer, uint dialogID)
        {
            CallBackUtils.CallBack<uint> callback = new CallBackUtils.CallBack<uint>(WantToStartLocalDialog);
            callback.QueueCallBack(timer, dialogID);
            return callback;
        }

        public static CallBackUtils.CallBackBase DelayLocalDialogForced(DelayValue timer, uint dialogID)
        {
            CallBackUtils.CallBack<uint> callback = new CallBackUtils.CallBack<uint>(WantToStartLocalDialogForced);
            callback.QueueCallBack(timer, dialogID);
            return callback;
        }



        protected override void Setup()
        {
            if (MiscSettings.DialogCastingDebugEnabled)
            {
                PlayerDialogManager.Current.m_dialogCastingDirector.DEBUG_ENABLED = true;
            }


            base.Setup();
        }

    }
}
