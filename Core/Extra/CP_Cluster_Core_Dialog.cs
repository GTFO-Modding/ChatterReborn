using ChainedPuzzles;
using GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChatterReborn.Extra
{
    public class CP_Cluster_Core_Dialog : CP_ChainedPuzzleCore_Base_Dialog
    {
        public CP_Cluster_Core_Dialog(CP_Cluster_Core cluster, bool isFinal) : base(isFinal)
        {
            this.m_cluster = cluster;
        }
        protected override Vector3 Position
        {
            get
            {
                return this.m_cluster.transform.position;
            }
        }

        protected override Vector3 TipPos
        {
            get
            {
                return this.m_cluster.m_spline.TipPos;
            }
        }

        public void OnStateChangeDialog(eClusterStatus clusterStatus, float progress, bool b)
        {
            if (clusterStatus == eClusterStatus.SplineReveal)
            {
                this.TriggerDialogueCloseToPosition(TipPos, UnityEngine.Random.Range(0.5f, 1f), GD.PlayerDialog.bio_scan_follow_holo_path, 6f);
            }
        }


        private CP_Cluster_Core m_cluster;
    }
}
