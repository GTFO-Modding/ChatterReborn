﻿using ChatterReborn.Utils;
using ChatterRebornSettings;
using GameData;
using LevelGeneration;

namespace ChatterReborn.WieldingItemStates
{
    public class WI_EnemyScanning : WI_Base
    {
        private WI_EnemyScanResults Results_State => (WI_EnemyScanResults)this.m_machine.GetState(WI_State.EnemyScanResults);


        public override void Enter()
        {
            this.m_scanningQueue = 0f;
            this.m_scanningGoal = EnemyScannerSettings.ScanningGoal_No_Enemies;
            m_enemyFound = false;
            Results_State.ResetEnemyCount();            
        }

        private bool GetSecurityActiveEnemyWaveData(out float possibleScore)
        {
            possibleScore = 0f;
            var door = GetSecurityDoorObject;

            if (door == null)
            {
                return false;
            }


            if (door.ActiveEnemyWaveData == null || !door.ActiveEnemyWaveData.HasActiveEnemyWave)
            {
                return false;
            }


            if (door.ActiveEnemyWaveData.EnemyGroupInArea > 0)
            {
                var block = EnemyGroupDataBlock.GetBlock(door.ActiveEnemyWaveData.EnemyGroupInArea);
                if (block != null)
                {
                    if (door.ActiveEnemyWaveData.EnemyGroupsInArea > 0)
                    {
                        possibleScore += block.MaxScore * door.ActiveEnemyWaveData.EnemyGroupsInArea;
                    }
                }
            }
            

            if (door.ActiveEnemyWaveData.EnemyGroupInfrontOfDoor > 0)
            {
                var block2 = EnemyGroupDataBlock.GetBlock(door.ActiveEnemyWaveData.EnemyGroupInfrontOfDoor);
                if (block2 != null)
                {
                    possibleScore += block2.MaxScore;
                }
            }
            

            return true;
        }

        public override void Update()
        {

            if (!this.IsEnemieScannerHeldAndAimed)
            {
                this.m_machine.ChangeState(WI_State.Deciding);
                return;
            }

            this.m_scanningQueue += Clock.Delta;
            int enemiesDetectedCount = this.GetEnemiesDetected(DramaManager.CurrentStateEnum == DRAMA_State.Combat);
            if (this.GetSecurityActiveEnemyWaveData(out float possibleScore))
            {
                enemiesDetectedCount += (int)possibleScore;
            }
            if (Results_State.IsCountHigher(enemiesDetectedCount) && !m_enemyFound)
            {
                m_enemyFound = true;
                this.m_scanningQueue = 0f;
                this.m_scanningGoal = EnemyScannerSettings.ScanningGoal_Max;
                return;
            }         

            if (this.m_scanningQueue > this.m_scanningGoal)
            {
                this.m_machine.ChangeState(WI_State.EnemyScanResults);
            }
        }

        private LG_SecurityDoor GetSecurityDoorObject
        {
            get
            {
                if (this.Owner.FPSCamera == null)
                {
                    return null;
                }
                if (this.Owner.FPSCamera.CameraRayObject == null)
                {
                    return null;
                }
                if (this.Owner.FPSCamera.CameraRayDist > 7f)
                {
                    return null;
                }
                return this.Owner.FPSCamera.CameraRayObject.GetAbsoluteComponent<LG_SecurityDoor>();
            }
        }

        private float m_scanningQueue;
        private float m_scanningGoal = 0f;

        private bool m_enemyFound;
    }
}
