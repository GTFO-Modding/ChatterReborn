namespace ChatterReborn.Drama_Chatter_States
{
    public class DRAMA_Chatter_Encounter : DRAMA_Chatter_Combat
    {
        protected override bool StartCombatDialogue => false;

        public override bool IsCombatState => false;
    }
}
