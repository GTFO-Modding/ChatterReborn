using ChatterReborn.Managers;
using GameData;
using Player;
using UnityEngine;

namespace ChatterReborn.Extra
{
    public abstract class CP_ChainedPuzzleCore_Base_Dialog
    {

        protected abstract Vector3 Position { get; }
        protected abstract Vector3 TipPos { get; }

        public CP_ChainedPuzzleCore_Base_Dialog(bool isFinal)
        {
            this.m_isFinal = isFinal;
        }

        protected void TriggerDialogueCloseToPosition(Vector3 pos, float dialogDelay, uint dialogID, float range = 0f)
        {
            if (PlayerManager.TryGetLocalPlayerAgent(out PlayerAgent localPlayer))
            {
                Vector3 vector = (localPlayer.Position - pos);
                if (range == 0f || vector.magnitude < range)
                {
                    if (ConfigurationManager.ChainedPuzzleDialoguesEnabled)
                    {
                        PrisonerDialogManager.DelayDialog(dialogDelay, dialogID, localPlayer);
                    }
                }
            }
        }

        public void OnScanDone(int _)
        {
            this.TriggerDialogueCloseToPosition(Position, UnityEngine.Random.Range(0.5f, 1f), !m_isFinal ? GD.PlayerDialog.generic_move_to_the_next_one : GD.PlayerDialog.generic_done, 0f);
        }

        protected bool m_isFinal;
    }
}
