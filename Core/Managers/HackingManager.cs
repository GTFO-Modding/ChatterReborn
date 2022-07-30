using ChatterReborn.Data;
using ChatterReborn.Extra;
using System.Collections.Generic;

namespace ChatterReborn.Managers
{
    public class HackingManager : ChatterManager<HackingManager>
    {
        protected override void Setup()
        {
            this.m_saved_progress = new Dictionary<int, HackingMiniGameProgress>();
        }
        public override void OnLevelCleanUp()
        {
            this.m_saved_progress.Clear();
        }

        protected override void PostSetup()
        {
            this.m_patcher.Patch<HackingMinigame_TimingGrid>(nameof(HackingMinigame_TimingGrid.StartGame), ChatterPatchType.Postfix, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            this.m_patcher.Patch<HackingMinigame_TimingGrid>(nameof(HackingMinigame_TimingGrid.EndGame), ChatterPatchType.Postfix, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        }


        static void HackingMinigame_TimingGrid__StartGame__Postfix(HackingMinigame_TimingGrid __instance)
        {
            HackingManager.Current.SetupCurrentMiniGame(__instance);
        }
        

        static void HackingMinigame_TimingGrid__EndGame__Postfix(HackingMinigame_TimingGrid __instance)
        {
            HackingManager.Current.EndGame();
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

        public static void SaveHackingStatus(int key, HackingMiniGameProgress status)
        {
            if (!Current.m_saved_progress.ContainsKey(key))
            {
                Current.m_saved_progress.Add(key, status);
                return;
            }

            Current.m_saved_progress[key] = status;
        }

        public override void Update()
        {
            if (this.m_current_minigame == null)
            {
                return;
            }

            this.m_current_minigame.Update();
        }


        public static void TryToLoadProgress(DialogTimedHackMiniGame game)
        {
            if (Current.m_saved_progress.TryGetValue(game.HackingGameKey, out HackingMiniGameProgress progress))
            {
                game.Load(progress);
            }
        }


        private Dictionary<int, HackingMiniGameProgress> m_saved_progress;

        private DialogTimedHackMiniGame m_current_minigame;
    }
}
