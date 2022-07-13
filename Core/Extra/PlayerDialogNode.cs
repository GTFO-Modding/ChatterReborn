namespace ChatterReborn.Extra
{
    public class PlayerDialogNode
    {

        public string Label { get; set; }
        public uint DialogID { get; set; }

        public string Description { get; set; }

        public PlayerDialogNode NextNode { get; set; }

        public PlayerDialogNode PreviousNode { get; set; }

        public PlayerDialogNodeMenu ChildMenu { get; set; }

        public int Index { get; set; }
    }
}
