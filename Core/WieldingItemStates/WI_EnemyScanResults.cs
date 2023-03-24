using ChatterReborn.Data;
using ChatterReborn.Managers;
using ChatterRebornSettings;
using GameData;
using System;

namespace ChatterReborn.WieldingItemStates
{
    public class WI_EnemyScanResults : WI_Base
    {
        public override void Enter()
        {
            uint dialogID = GD.PlayerDialog.on_scan_no_enemies;
            if (enemiesFound > 0)
            {
                dialogID = MaxReached ? GD.PlayerDialog.on_scan_many_enemies : GD.PlayerDialog.on_scan_few_enemies;
            }
            this.StartEnemyScannerDialog(dialogID);
            CoolDownManager.ApplyCooldown(CoolDownType.EnemyScanDialog, 5f);
        }

        public void ResetEnemyCount()
        {
            enemiesFound = 0;
        }

        public bool IsCountHigher(int count)
        {
            bool isHigherCount = enemiesFound < count;
            enemiesFound = Math.Max(count, enemiesFound);
            return isHigherCount;
        }

        public override void Update()
        {
            if (!this.IsEnemieScannerHeldAndAimed)
            {
                this.m_machine.ChangeState(WI_State.Deciding);
            }
        }

        private int enemiesFound;

        public bool MaxReached => enemiesFound > 8;
    }
}
