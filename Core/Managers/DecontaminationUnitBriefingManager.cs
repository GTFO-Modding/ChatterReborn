using Player;
using SNetwork;
using UnityEngine;

namespace ChatterReborn.Managers
{
    public class PlayerLobbyVideoCreatorManager : ChatterManager<PlayerLobbyVideoCreatorManager>
    {
        private const bool m_allow = false;


        public override void Update()
        {
            UpdateMasterCommands();
        }

        private static void UpdateMasterCommands()
        {
            if (!m_allow)
            {
                return;
            }
            string nickName = "";
            pMasterCommand pMasterCommand = new pMasterCommand
            {
                type = eMasterCommandType.PickAvatar,
                refA = -1,
            };
            if (Input.GetKeyDown(KeyCode.F1))
            {
                pMasterCommand.refA = 0;
                nickName = "Woods";
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                pMasterCommand.refA = 1;
                nickName = "Dauda";
            }
            else if (Input.GetKeyDown(KeyCode.F3))
            {
                pMasterCommand.refA = 2;
                nickName = "Hackett";
            }
            else if (Input.GetKeyDown(KeyCode.F4))
            {
                pMasterCommand.refA = 3;
                nickName = "Bishop";
            }

            if (pMasterCommand.refA != -1)
            {
                SNet.Sync.LocalMasterCommand(pMasterCommand);
                Color characterColor = PlayerManager.GetStaticPlayerColor(pMasterCommand.refA);

                PlayerManager.GetLocalPlayerAgent().Owner.NickName = "<color=#" + ColorUtility.ToHtmlStringRGB(characterColor) + ">" + nickName;
            }
        }
    }

    
}
