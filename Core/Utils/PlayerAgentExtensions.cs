using Player;
using SNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Utils
{
    public static class PlayerAgentExtensions
    {
        public static void Say(this PlayerAgent playerAgent, uint eventID)
        {
            PlayerVoiceManager.WantToSay(playerAgent.CharacterID, eventID);
        }


        public static void SayAndStartDialog(this PlayerAgent playerAgent, uint eventID, uint startDialog)
        {
            PlayerVoiceManager.WantToSayAndStartDialog(playerAgent.CharacterID, eventID, startDialog);
        }

        public static List<PlayerAgent> GetAllBotPlayerAgents()
        {
            List<PlayerAgent> bots = new List<PlayerAgent>();

            List<SNet_Player> allBots = SNet.Core.GetAllBots().ToSystemList();

            for (int i = 0; i < allBots.Count; i++)
            {
                SNet_Player snetAgent = allBots[i];
                PlayerAgent playerAgent = snetAgent?.PlayerAgent?.TryCast<PlayerAgent>();
                if (playerAgent != null)
                {
                    bots.Add(playerAgent);
                }
            }

            return bots;
        }


        public static bool IsInfectionStable(this PlayerAgent playerAgent)
        {
            return playerAgent.Damage.Infection + playerAgent.Damage.GetHealthRel() <= 1f;
        }

        public static bool IsBotOwned(this PlayerAgent playerAgent)
        {
            return playerAgent.Owner.IsBot && SNet.IsMaster;
        }


        public static void WantToStartDialog(this PlayerAgent playerAgent, uint dialogID, bool forced = false)
        {
            if (forced)
            {
                PlayerDialogManager.WantToStartDialogForced(dialogID, playerAgent);
            }
            else
            {
                PlayerDialogManager.WantToStartDialog(dialogID, playerAgent);
            }
        }
    }
}
