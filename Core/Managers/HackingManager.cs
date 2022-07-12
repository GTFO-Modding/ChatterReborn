using ChatterReborn.Data;
using ChatterReborn.Extra;
using System.Collections.Generic;

namespace ChatterReborn.Managers
{
    public class HackingManager : ChatterManager<HackingManager>
    {
        protected override void Setup()
        {
            this.m_saved_minigames = new Dictionary<int, DialogTimedHackMiniGame>();
        }
        protected override void OnLevelCleanup()
        {
            this.m_saved_minigames.Clear();
        }

        public static void OnVerifyHit()
        {
            if (Current.m_current_minigame == null)
            {
                Current.DebugPrint("sucessful hit on an unknown hacking minigame??", eLogType.Error);
                return;
            }



            Current.m_current_minigame.OnVerifyHit();
        }

        public void EndGame()
        {
            Current.m_current_minigame = null;
        }

        public static void OnFailedHit()
        {
            if (Current.m_current_minigame == null)
            {
                Current.DebugPrint("failed hit on an unknown hacking minigame??", eLogType.Error);
                return;
            }

            Current.m_current_minigame.OnMissed();
        }


        public void SetupCurrentMiniGame(HackingMinigame_TimingGrid miniGame)
        {
            this.DebugPrint("Setting up a new HackingMinigame_TimingGrid for dialogs");
            this.m_current_minigame = new DialogTimedHackMiniGame(miniGame);
        }

        public override void Update()
        {
            if (this.m_current_minigame == null)
            {
                return;
            }

            this.m_current_minigame.Update();
        }


        private Dictionary<int, DialogTimedHackMiniGame> m_saved_minigames;

        private DialogTimedHackMiniGame m_current_minigame;
    }
}
