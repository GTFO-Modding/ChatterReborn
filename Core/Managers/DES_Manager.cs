using ChatterReborn.Machines;
using ChatterReborn.WieldingItemStates;
using Player;

namespace ChatterReborn.Managers
{
    public class DES_Manager : ChatterManager<DES_Manager>
    {



        public override void Update()
        {
            if (!m_hasScanner)
            {
                return;
            }
            this.machine.Update();
        }

        public override void On_Registered_LocalPlayerAgent(LocalPlayerAgent localPlayerAgent)
        {
            Current.machine = new WieldingItemMachine();
            Current.machine.Setup(localPlayerAgent);
            m_hasScanner = true;
        }

        public override void On_DeRegistered_LocalPlayerAgent(LocalPlayerAgent localPlayerAgent)
        {
            Current.machine.ChangeState(WI_State.Deciding);
            m_hasScanner = false;
        }

        private WieldingItemMachine machine;

        private bool m_hasScanner = false;
    }
}
