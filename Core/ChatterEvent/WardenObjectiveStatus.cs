using LevelGeneration;

namespace ChatterReborn.ChatterEvent
{
    public struct WardenObjectiveStatus
    {
        public readonly bool isRecall;
        public readonly LG_LayerType layer;
        public readonly pWardenObjectiveState newState;
        public readonly eWardenObjectiveStatus oldStatus;
        public readonly eWardenObjectiveStatus newStatus;
        public readonly int oldIndex;
        public readonly int newIndex;
        public readonly eWardenSubObjectiveStatus oldSub;
        public readonly eWardenSubObjectiveStatus newSub;

        public WardenObjectiveStatus(bool isRecall, LG_LayerType layer, pWardenObjectiveState newState, eWardenObjectiveStatus oldStatus, eWardenObjectiveStatus newStatus, int oldIndex, int newIndex, eWardenSubObjectiveStatus oldSub, eWardenSubObjectiveStatus newSub)
        {
            this.isRecall = isRecall;
            this.layer = layer;
            this.newState = newState;
            this.oldStatus = oldStatus;
            this.newStatus = newStatus;
            this.oldIndex = oldIndex;
            this.newIndex = newIndex;
            this.oldSub = oldSub;
            this.newSub = newSub;
        }
    }
}
