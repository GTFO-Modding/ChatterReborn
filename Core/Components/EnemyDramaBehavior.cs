using Agents;
using ChatterReborn.Attributes;
using ChatterReborn.Managers;
using ChatterReborn.Utils;
using ChatterRebornSettings;
using Enemies;
using Player;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ChatterReborn.Components
{
    [IL2CPPType]
    public class EnemyDramaBehavior : MonoBehaviour
    {
        private bool m_aggressive;

        private float[] m_playerVisiblity;


        public EnemyAgent m_enemyAgent;

        public EnemyAI EnemyAI { get; private set; }

        private bool m_destroyed;

        public EnemyDramaBehavior(IntPtr pointer) : base(pointer)
        {
        }

        private float GetSightLineMultiplier(bool isVisible, PlayerAgent playerAgent)
        {
            float num = 50f;
            float num2 = 10f;
            float num3 = (this.m_enemyAgent.Position - playerAgent.Position).magnitude;
            num3 = Mathf.Clamp(num3, num2, num);
            float num4 = 1f * (isVisible ? (Mathf.Min(num3, num2) / num2) : (Mathf.Min(num3, num) / num));
            return Clock.Delta * 0.01f * num4 * (isVisible ? 100f : 15f);
        }


        void Awake()
        {
            this.m_playerVisiblity = new float[4];
            this.m_detectedPlayers = new bool[4];
            this.m_enemyAgent = GetComponent<EnemyAgent>();
            this.EnemyAI = this.m_enemyAgent.AI;
            this.m_destroyed = false;
            this.m_aggressive = false;
        }

        private bool[] m_detectedPlayers;

        private void SetTargetVisible(PlayerAgent playerAgent)
        {
            int characterID = playerAgent.CharacterID;
            SetVisibility(playerAgent, true);
            if (!this.m_detectedPlayers[characterID])
            {
                this.m_detectedPlayers[characterID] = true;
                EnemyDetectionManager.Current.m_enemyVisibilites[characterID]++;
                EnemyDetectionManager.Current.m_enemyScores[characterID] += 8f;
            }            
        }

        private void SetVisibility(PlayerAgent playerAgent, bool visible = false)
        {
            int charID = playerAgent.CharacterID;
            float multiplier = this.GetSightLineMultiplier(visible, playerAgent);
            this.m_playerVisiblity[charID] = Mathf.Clamp01(this.m_playerVisiblity[charID] + (visible ? multiplier : -multiplier));
        }

        private void SetTargetNotVisible(PlayerAgent playerAgent)
        {
            int characterID = playerAgent.CharacterID;
            SetVisibility(playerAgent, false);
            if (this.m_detectedPlayers[characterID] && this.m_playerVisiblity[characterID] <= 0f)
            {
                this.m_detectedPlayers[characterID] = false;
                EnemyDetectionManager.Current.m_enemyVisibilites[characterID]--;
                EnemyDetectionManager.Current.m_enemyScores[characterID] -= Settings.Drama.enemyScoreNormal;
            }
        }

        private bool IsTargetVisible(PlayerAgent playerAgent)
        {
            return this.m_enemyAgent.CanSee(playerAgent.gameObject);
        }

        void Update()
        {
            if (this.m_enemyAgent == null || !this.m_enemyAgent.Alive)
            {
                this.DisableEnemyDramaBehavior();
                return;
            }
            UpdateVisibilites();
        }

        

        private void UpdateVisibilites()
        {
            if (PlayerManager.PlayerAgentsInLevel == null)
            {
                return;
            }

            if (EnemyAI.Mode != AgentMode.Agressive)
            {
                return;
            }

            if (!m_aggressive)
            {
                EnemyDetectionManager.AggressiveEnemies++;
                m_aggressive = true;
            }

            List<PlayerAgent> players = Il2cppUtils.ToSystemList(PlayerManager.PlayerAgentsInLevel);
            for (int i = 0; i < players.Count; i++)
            {
                PlayerAgent playerAgent = players[i];
                if (this.IsTargetVisible(playerAgent))
                {
                    this.SetTargetVisible(playerAgent);
                }
                else
                {
                    this.SetTargetNotVisible(playerAgent);
                }
            }            
        }

        private void DeRegisterVisibilitiesToPlayers()
        {
            if (PlayerManager.PlayerAgentsInLevel == null)
            {
                return;
            }
            List<PlayerAgent> players = Il2cppUtils.ToSystemList(PlayerManager.PlayerAgentsInLevel);
            for (int i = 0; i < players.Count; i++)
            {
                PlayerAgent player = players[i];
                if (player != null)
                {
                    this.SetTargetNotVisible(player);
                }
            }
        }

        void OnDestroy()
        {
            DisableEnemyDramaBehavior();
        }

        private void DisableEnemyDramaBehavior()
        {
            if (this.m_destroyed)
            {
                return;
            }


            this.m_destroyed = true;

            if (m_aggressive)
            {
                EnemyDetectionManager.AggressiveEnemies--;
                m_aggressive = false;
            }


            for (int i = 0; i < 4; i++)
            {
                if (this.m_detectedPlayers[i])
                {
                    EnemyDetectionManager.Current.m_enemyVisibilites[i]--;
                    EnemyDetectionManager.Current.m_enemyScores[i] -= Settings.Drama.enemyScoreNormal;
                }
            }
        }


    }
}
