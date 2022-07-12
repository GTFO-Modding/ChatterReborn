using Agents;
using ChatterReborn.Machines;
using ChatterReborn.Managers;
using ChatterReborn.Utils;
using ChatterReborn.Utils.Machine;
using Enemies;
using Gear;
using Player;

namespace ChatterReborn.WieldingItemStates
{
    public class WI_Base : MachineState<WieldingItemMachine>
    {
        protected int GetEnemiesDetected(bool onlyTrackAggressives = false)
        {
            int count = 0;
            if (Machine.GetWieldingItem<EnemyScanner>(out var enemyScanner) && enemyScanner.m_enemiesDetected != null)
            {
                var enemiesDetected = Il2cppUtils.ToSystemList(enemyScanner.m_enemiesDetected);
                for (int i = 0; i < enemiesDetected.Count; i++)
                {
                    EnemyAgent enemy = enemiesDetected[i];
                    if (enemy.Alive && (!onlyTrackAggressives || enemy.AI.Mode == AgentMode.Agressive))
                    {
                        float dis = (enemy.transform.position - this.Owner.transform.position).magnitude;
                        if (dis < m_bioTrackerRange)
                        {
                            count++;
                        }
                    }

                }
            }
            
            return count;
        }

        private const float m_bioTrackerRange = 35f;

        protected bool IsEnemieScannerHeldAndAimed => Machine.GetWieldingItem<EnemyScanner>(out var scanner) && scanner.AimButtonHeld;
        protected bool IsBulletWeaponHeldAndAimming => Machine.GetWieldingItem<BulletWeapon>(out var weapon) && weapon.AimButtonHeld;



        protected void StartEnemyScannerDialog(uint dialogID)
        {
            if (ConfigurationManager.BioTrackerCommentsEnabled && DramaManager.CurrentStateEnum != DRAMA_State.Sneaking)
            {
                if (GameStateManager.IsInExpedition)
                {
                    this.Owner.WantToStartDialog(dialogID, true);
                }               
            }
        }

        protected void StartWeaponDialog(uint dialogID)
        {
            if (DramaManager.CurrentStateEnum != DRAMA_State.Sneaking)
            {
                if (GameStateManager.IsInExpedition)
                {
                    this.Owner.WantToStartDialog(dialogID, true);
                }               
            }
        }

        protected LocalPlayerAgent Owner
        {
            get
            {
                return Machine.Owner;
            }
        }



        public WieldingItemMachine Machine => (WieldingItemMachine)base.m_machine;
    }
}
