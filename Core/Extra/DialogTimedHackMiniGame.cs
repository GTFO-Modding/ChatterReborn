using ChatterReborn.Data;
using ChatterReborn.Managers;
using ChatterReborn.Utils;
using GameData;
using Player;
using System;
using UnityEngine;

namespace ChatterReborn.Extra
{
    public class DialogTimedHackMiniGame
    {
        public DialogTimedHackMiniGame(HackingMinigame_TimingGrid minigame)
        {
            this.m_minigame = minigame;
            this.m_hackingGameKey = minigame.m_minigameRoot.gameObject.GetInstanceID();
            this.m_current_level = 1;
            this.m_triggerMissedCallBack = new CallBackUtils.CallBack<int>(TriggerMissedDialogue);
            this.m_triggerHitCallBack = new CallBackUtils.CallBack<int,int>(TriggerHitDialog);
            
        }


        public void LoadLastProgress()
        {
            HackingManager.TryToLoadProgress(this);
        }

        public void Load(HackingMiniGameProgress progress)
        {
            this.m_max_misses = progress.max_misses;
            this.m_highest_level = progress.highest_level;
            this.m_current_level = progress.amounts_missed;
        }


        private HackingMinigame_TimingGrid m_minigame;

        private void TriggerHitDialog(int hits, int highestHits)
        {
            PlayerAgent playerAgent = PlayerManager.GetLocalPlayerAgent();

            if (playerAgent == null)
            {
                return;
            }    

            


            uint hitDialogID = 0U;

            if (this.m_minigame.m_puzzleDone)
            {
                if (this.m_amount_missed > 3)
                {
                    hitDialogID = GD.PlayerDialog.hacking_successful_problematic;
                }
                else if (this.m_amount_missed == 0)
                {
                    hitDialogID = GD.PlayerDialog.hacking_successful_flawless;
                }
                else
                {
                    hitDialogID = GD.PlayerDialog.hacking_successful_regular;
                }
            }
            else if (hits > highestHits)
            {
                if (hits == 2)
                {
                    hitDialogID = GD.PlayerDialog.hacking_correct_first;
                }
                else if (hits == 3)
                {
                    hitDialogID = GD.PlayerDialog.hacking_correct_second;
                }
            }


            if (hitDialogID > 0U)
            {
                ChatterDebug.LogWarning("Triggering hit dialogue : " + PlayerDialogDataBlock.GetBlockName(hitDialogID));

                if (ConfigurationManager.HackingMiniGameCommentsEnabled)
                {
                    PlayerDialogManager.WantToStartDialogForced(hitDialogID, playerAgent);
                }
                
            }
            else
            {
                ChatterDebug.LogWarning("No Dialogue triggered on hit for puzzle level " + this.m_minigame.m_puzzleLevel);
            }
        }

        public void Update()
        {
            if (IsGamePaused)
            {
                return;
            }


            if (InputMapper.GetButtonDown.Invoke(InputAction.Fire, this.m_minigame.m_tool.m_inuputFocusState))
            {
                if (this.IsInTarget())
                {
                    this.OnVerifyHit();
                }
                else
                {
                    this.OnMissed();
                }
            }
        }

        private void TriggerMissedDialogue(int amountMissed)
        {

            PlayerAgent playerAgent = PlayerManager.GetLocalPlayerAgent();

            if (playerAgent == null)
            {
                return;
            }

            if (amountMissed > this.m_max_misses)
            {
                return;
            }


            uint failedDialog = 0U;
            if (amountMissed == 1)
            {
                failedDialog = GD.PlayerDialog.hacking_wrong_first;
            }
            else if (amountMissed == 2)
            {
                failedDialog = GD.PlayerDialog.hacking_wrong_second;
            }
            else if (amountMissed == 3)
            {
                failedDialog = GD.PlayerDialog.hacking_wrong_third;
            }


            if (failedDialog != 0U)
            {
                ChatterDebug.LogWarning("Triggering missed dialogue " + failedDialog);
                if (ConfigurationManager.HackingMiniGameCommentsEnabled)
                {
                    playerAgent.WantToStartDialog(failedDialog, true);
                }
            }


        }

        private bool IsInTarget()
        {
            return this.m_minigame.IsInSelectorRadius(this.m_minigame.m_movingRow);
        }

        private bool IsGamePaused
        {
            get
            {
                return this.m_minigame.GamePauseTimer > Clock.Time;
            }
        }
        private string[] Results => new string[]
        {
            "Results : ",
            "\n\tPuzzleLevel : " + this.m_current_level,
            "\n\tAmount missed : " + this.m_amount_missed
        };

        public int HackingGameKey => m_hackingGameKey;

        public void OnVerifyHit()
        {
            this.m_current_level++;
            ChatterDebug.LogWarning("On Verify Hit");
            ChatterDebug.LogWarning(string.Concat(Results));            
            this.m_triggerHitCallBack.QueueCallBack(0.35f, this.m_current_level, this.m_highest_level);
            this.SaveStatus();
        }


        public void OnMissed()
        {

            ChatterDebug.LogWarning("On Missed");
            ChatterDebug.LogWarning(string.Concat(Results));

            this.m_amount_missed++;        

            this.m_triggerMissedCallBack.QueueCallBack(0.35f, this.m_amount_missed);


            this.m_current_level = Mathf.Clamp(this.m_current_level - 1, 1, 4);
            SaveStatus();
        }

        private void SaveStatus()
        {
            this.m_highest_level = Math.Max(this.m_current_level, this.m_highest_level);
            HackingManager.SaveHackingStatus(this.m_hackingGameKey, new HackingMiniGameProgress
            {
                amounts_missed = this.m_amount_missed,
                highest_level = this.m_highest_level,
                max_misses = this.m_max_misses,
            });
        }

        private int m_hackingGameKey;
        private int m_amount_missed = 0;

        private int m_max_misses = 5;

        private int m_current_level;        

        private int m_highest_level;

        private CallBackUtils.CallBack<int> m_triggerMissedCallBack;

        private CallBackUtils.CallBack<int, int> m_triggerHitCallBack;
    }
}
