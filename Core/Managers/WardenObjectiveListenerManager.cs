using ChatterReborn.ChatterEvent;
using ChatterReborn.Utils;
using GameData;
using LevelGeneration;
using Player;

namespace ChatterReborn.Managers
{
    public class WardenObjectiveListenerManager : ChatterManager<WardenObjectiveListenerManager>, IChatterEventListener<WardenObjectiveStatus>
    {

        protected override void PostSetup()
        {
            //ChatterEventListenerHandler<WardenObjectiveStatus>.RegisterListener(this);
        }


        public void OnChatterEvent(WardenObjectiveStatus objStatus)
        {
            /*if (objStatus.newStatus == eWardenObjectiveStatus.Started)
            {
                return;
            }
            if (objStatus.newStatus == eWardenObjectiveStatus.WardenObjectivePartiallySolved)
            {
                return;
            }

            if (objStatus.newStatus == eWardenObjectiveStatus.WardenObjectiveItemSolved)
            {
                if (objStatus.layer == LG_LayerType.MainLayer)
                {
                    uint dialogID = GD.PlayerDialog.get_to_elevator;
                    var itemCollection = WardenObjectiveManager.GetObjectiveItemCollection(LG_LayerType.MainLayer);
                    bool hasItemRequirementExit = WardenObjectiveManager.m_elevatorExitWinConditionItem != null || WardenObjectiveManager.m_customGeoExitWinConditionItem != null;
                    if (hasItemRequirementExit && itemCollection.Count != 0)
                    {
                        dialogID = GD.PlayerDialog.get_to_elevator_with_thing;
                    }
                    if (PlayerManager.TryGetLocalPlayerAgent(out PlayerAgent sourceAgent))
                    {
                        sourceAgent.WantToStartDialog(dialogID);
                    }
                }                
            }     */       
        }
    }
}
