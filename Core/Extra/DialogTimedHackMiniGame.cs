using ChatterReborn.Managers;
using ChatterReborn.Utils;
using GameData;
using Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChatterReborn.Extra
{
    public class DialogTimedHackMiniGame
    {
        public DialogTimedHackMiniGame(HackingMinigame_TimingGrid minigame)
        {
            this.m_minigame = minigame;
            this.m_current_level = 1;
            this.m_triggerMissedCallBack = new CallBackUtils.CallBack(TriggerMissedDialogue);
            this.m_triggerHitCallBack = new CallBackUtils.CallBack(TriggerHitDialog);
        }


        private HackingMinigame_TimingGrid m_minigame;

        private void TriggerHitDialog()
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
            else if (this.m_current_level == 2)
            {
                hitDialogID = GD.PlayerDialog.hacking_correct_first;
            }
            else if (this.m_current_level == 3)
            {
                hitDialogID = GD.PlayerDialog.hacking_correct_second;
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

        private void TriggerMissedDialogue()
        {

            PlayerAgent playerAgent = PlayerManager.GetLocalPlayerAgent();

            if (playerAgent == null)
            {
                return;
            }


            uint failedDialog = 0U;
            if (this.m_amount_missed == 1)
            {
                failedDialog = GD.PlayerDialog.hacking_wrong_first;
            }
            else if (this.m_amount_missed == 2)
            {
                failedDialog = GD.PlayerDialog.hacking_wrong_second;
            }
            else if (this.m_amount_missed == 3)
            {
                failedDialog = GD.PlayerDialog.hacking_wrong_third;
            }


            if (failedDialog > 0U)
            {
                ChatterDebug.LogWarning("Triggering missed dialogue " + failedDialog);
                if (ConfigurationManager.HackingMiniGameCommentsEnabled)
                {
                    PlayerDialogManager.WantToStartDialogForced(failedDialog, playerAgent);
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

        public void OnVerifyHit()
        {
            this.m_current_level++;
            if (this.m_current_level != 3)
            {
                if (this.m_current_level < this.m_highest_level)
                {
                    ChatterDebug.LogWarning("Current Level is not the same!!");
                    return;
                }
            }
            


            this.SaveLevel();

            ChatterDebug.LogWarning("On Verify Hit");
            ChatterDebug.LogWarning(string.Concat(Results));

            this.m_triggerHitCallBack.QueueCallBack(0.35f);

        }


        public void OnMissed()
        {

            ChatterDebug.LogWarning("On Missed");
            ChatterDebug.LogWarning(string.Concat(Results));

            this.m_amount_missed++;

            if (this.m_amount_missed < this.m_max_misses)
            {
                this.m_triggerMissedCallBack.QueueCallBack(0.35f);
            }


            this.m_current_level = Mathf.Clamp(this.m_current_level - 1, 0, 4);
        }

        private void SaveLevel()
        {
            this.m_highest_level = Math.Max(this.m_current_level, this.m_highest_level);
        }

        private int m_amount_missed = 0;

        private int m_max_misses = 5;

        private int m_current_level;

        private CallBackUtils.CallBack m_triggerMissedCallBack;

        private CallBackUtils.CallBack m_triggerHitCallBack;

        private float m_highest_level;
    }
}
