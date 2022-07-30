using ChainedPuzzles;
using ChatterReborn.Extra;
using ChatterReborn.Utils;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Player;
using System;

namespace ChatterReborn.Managers
{
    public class ChainedPuzzleDialogManager : ChatterManager<ChainedPuzzleDialogManager>
    {
        public override void OnElevatorArrived()
        {
            if (ChainedPuzzleManager.Current == null || ChainedPuzzleManager.Current.m_instances == null)
            {
                this.DebugPrint("Failed adding OnSyncStateChanges to bio scans..", Data.eLogType.Error);
                return;
            }
            var instances = Il2cppUtils.ToSystemList(ChainedPuzzleManager.Current.m_instances);
            for (int i = 0; i < instances.Count; i++)
            {
                var chainedPuzzleInstance = instances[i];
                if (chainedPuzzleInstance != null && chainedPuzzleInstance.m_chainedPuzzleCores != null)
                {
                    iChainedPuzzleCore[] cores = chainedPuzzleInstance.m_chainedPuzzleCores;
                    if (cores.Length > 0)
                    {
                        for (int j = 0; j < cores.Length; j++)
                        {
                            var core = cores[j];
                            if (core != null)
                            {
                                bool isFinal = j == cores.Length - 1;
                                if (Il2cppUtils.Convert(core, out CP_Bioscan_Core cP_Bioscan_Core))
                                {
                                    var dialog_core = new CP_Bioscan_Core_Dialog(cP_Bioscan_Core, isFinal);

                                    cP_Bioscan_Core.add_OnPuzzleDone((Action<int>)dialog_core.OnScanDone);
                                    cP_Bioscan_Core.m_sync.add_OnSyncStateChange((Action<eBioscanStatus, float, Il2CppSystem.Collections.Generic.List<PlayerAgent>, int, Il2CppStructArray<bool>, bool>)dialog_core.OnStateChangeDialog);

                                }
                                else if (Il2cppUtils.Convert(core, out CP_Cluster_Core cP_Cluster_Core))
                                {
                                    var dialog_cluster_core = new CP_Cluster_Core_Dialog(cP_Cluster_Core, isFinal);
                                    cP_Cluster_Core.add_OnPuzzleDone((Action<int>)dialog_cluster_core.OnScanDone);
                                    cP_Cluster_Core.m_sync.add_OnSyncStateChange((Il2CppSystem.Action<eClusterStatus, float, bool>)dialog_cluster_core.OnStateChangeDialog);
                                }
                            }
                        }
                    }

                }
            }
        }
    }
}
