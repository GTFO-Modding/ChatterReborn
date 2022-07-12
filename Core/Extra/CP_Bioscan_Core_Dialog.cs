using ChainedPuzzles;
using ChatterReborn.Utils;
using GameData;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Player;
using UnityEngine;
using il2cppGeneric = Il2CppSystem.Collections.Generic;

namespace ChatterReborn.Extra
{
    public class CP_Bioscan_Core_Dialog : CP_ChainedPuzzleCore_Base_Dialog
    {

        private CP_Bioscan_Core m_BioScan;


        protected override Vector3 Position
        {
            get
            {
                return this.m_BioScan.m_position;
            }
        }

        protected override Vector3 TipPos
        {
            get
            {
                return this.m_BioScan.m_spline.TipPos;
            }
        }




        public CP_Bioscan_Core_Dialog(CP_Bioscan_Core bioScan, bool isFinal) : base(isFinal)
        {
            this.m_BioScan = bioScan;
            this.m_delayScanningProggressDialogue = new CallBackUtils.CallBack(TriggerScanningProgressDialogue);
        }

        private void TriggerScanningProgressDialogue()
        {
            this.TriggerDialogueCloseToPosition(this.m_BioScan.m_position, 0f, GD.PlayerDialog.bio_scan_working, 10f);
        }


        private CallBackUtils.CallBack m_delayScanningProggressDialogue;

        private PlayerRequirement PlayerRequirement
        {
            get
            {
                return this.m_BioScan.PlayerScanner.ScanPlayersRequired;
            }
        }





        public void OnStateChangeDialog(eBioscanStatus status, float progress, il2cppGeneric.List<PlayerAgent> playersInScan, int playersMax, Il2CppStructArray<bool> reqItemStatus, bool isDropinState)
        {
            switch (status)
            {
                case eBioscanStatus.Disabled:
                    break;
                case eBioscanStatus.SplineReveal:
                    this.TriggerDialogueCloseToPosition(TipPos, UnityEngine.Random.Range(0.5f, 1f), GD.PlayerDialog.bio_scan_follow_holo_path, 6f);
                    break;
                case eBioscanStatus.Waiting:
                    this.m_delayScanningProggressDialogue.RemoveCallBack();
                    break;
                case eBioscanStatus.Scanning:
                    if (this.PlayerRequirement == PlayerRequirement.All)
                    {
                        this.m_delayScanningProggressDialogue.QueueCallBack(UnityEngine.Random.Range(1f, 1.5f));
                    }
                    break;
                case eBioscanStatus.TimedOut:
                    break;
                default:
                    break;
            }
        }

    }
}
