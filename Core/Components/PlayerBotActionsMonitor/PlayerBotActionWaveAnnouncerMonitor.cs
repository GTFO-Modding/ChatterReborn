using ChatterReborn.Managers;
using GameData;
using System.Collections.Generic;
using UnityEngine;

namespace ChatterReborn.Components.PlayerBotActionsMonitor
{
    public class PlayerBotActionWaveAnnouncerMonitor : PlayerBotMonitorBase
    {
        public void FixedUpdateActionMonitor()
        {
        }

        public void LateUpdateActionMonitor()
        {
        }

        public void Setup(PlayerBotAIRootMonitor root)
        {
            m_botAIActionMonitor = root;
        }

        public void UpdateActionMonitor()
        {

        }

        

        private PlayerBotAIRootMonitor m_botAIActionMonitor;

        public PlayerBotActionWaveAnnouncerMonitor(PlayerBotAIRootMonitor aiActionMonitor) : base(aiActionMonitor)
        {
        }

        private class SurvivalWaveAnnouncer
        {
            public Vector3 m_roarPosition;

            private List<uint> m_directionsDialogs = new List<uint>();

            public void AnnounceWaveDirection()
            {
                float teamGroupPositionX = 0f;
                float teamGroupPositionZ = 0f;

                int count = 0;
                foreach (var player in ExtendedPlayerManager.GetAllPlayers)
                {
                    teamGroupPositionX += player.Position.x;
                    teamGroupPositionZ += player.Position.z;
                    count++;
                }

                if (count <= 0)
                {
                    return;
                }


                Vector3 playerGroupPosition = new Vector3(teamGroupPositionX / count, m_roarPosition.y, teamGroupPositionZ / count);

                if (m_roarPosition.z > playerGroupPosition.z)
                {
                    
                    m_directionsDialogs.Add(GD.PlayerDialog.CL_North);
                }
                else
                {
                    m_directionsDialogs.Add(GD.PlayerDialog.CL_South);
                }


                var angle = Vector3.AngleBetween(playerGroupPosition, m_roarPosition);
                if (playerGroupPosition.z < m_roarPosition.z)
                {
                    m_directionsDialogs.Add(GD.PlayerDialog.CL_North);
                }
                else
                {
                    m_directionsDialogs.Add(GD.PlayerDialog.CL_South);
                }

                if (playerGroupPosition.x < m_roarPosition.x)
                {
                    m_directionsDialogs.Add(GD.PlayerDialog.CL_West);
                }
                else
                {
                    m_directionsDialogs.Add(GD.PlayerDialog.CL_East);
                }
            }
        }
    }
}
