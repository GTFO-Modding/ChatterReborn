using Agents;
using AIGraph;
using ChatterReborn.Attributes;
using ChatterReborn.Utils;
using Enemies;
using GameData;
using Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChatterReborn.Managers
{
    public class EnemyDetectionManager : ChatterManager<EnemyDetectionManager>
    {
        protected override void Setup()
        {
            this.m_enemyVisibilites = new int[4];
            this.m_enemyScores = new float[4];
        }


        private List<EnemyAgent> GetReachableEnemiesForPlayerAgent(PlayerAgent playerAgent)
        {
            List<EnemyAgent> AllreachableEnemiesInNodes = new List<EnemyAgent>();

            if (playerAgent != null && playerAgent.CourseNode != null)
            {
                var reachables = AIG_CourseGraph.GetReachableEnemiesInNodes(playerAgent.CourseNode, 3).ToArray();
                if (reachables != null && reachables.Count > 0)
                {
                    for (int i = reachables.Length - 1; i >= 0; i--)
                    {
                        EnemyAgent enemyAgent = reachables[i];
                        if (enemyAgent != null && enemyAgent.Alive)
                        {
                            AllreachableEnemiesInNodes.Add(enemyAgent);
                        }

                    }
                }
            }

            return AllreachableEnemiesInNodes;

        }

        public static int AggressiveEnemies;

        public static bool IsScout(EnemyAgent enemyAgent)
        {
            ChatterDebug.LogWarning("Checking if enemyAgent [" + enemyAgent.name + "] is a Scout");

            if (enemyAgent.IsScout)
            {
                ChatterDebug.LogWarning("\t<<<<This is a Scout, with IsScout..>>>>");
                return true;
            }

            if (enemyAgent.AI.Mode == AgentMode.Scout)
            {
                ChatterDebug.LogWarning("\t<<<<This is a Scout, AgentMode = Scout>>>>>");
                return true;
            }




            ChatterDebug.LogError("\t<<<<This is not a scout>>>>>");
            return false;
        }


        private static bool CheckVisibleEnemies
        {
            get
            {
                if (ExtendedPlayerManager.AllPlayersInLevel != null)
                {
                    for (int i = ExtendedPlayerManager.AllPlayersInLevel.Count - 1; i >= 0; i--)
                    {
                        PlayerAgent playerAgent = ExtendedPlayerManager.AllPlayersInLevel[i];
                        if (playerAgent != null && playerAgent.CourseNode != null)
                        {
                            List<EnemyAgent> reachableEnemiesInNodes = Current.GetReachableEnemiesForPlayerAgent(playerAgent);

                            if (reachableEnemiesInNodes.Count > 0)
                            {
                                for (int i1 = reachableEnemiesInNodes.Count - 1; i1 >= 0; i1--)
                                {
                                    EnemyAgent enemyAgent1 = reachableEnemiesInNodes[i1];
                                    if (enemyAgent1)
                                    {
                                        Vector3 vector = playerAgent.Position - enemyAgent1.ListenerPosition;
                                        if (playerAgent.CanSee(enemyAgent1.gameObject) || vector.magnitude < 30f)
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                return false;
            }
        }

        private static bool AnyEnemiesReachablewithinPlayers
        {
            get
            {
                if (ExtendedPlayerManager.AllPlayersInLevel != null)
                {
                    for (int i = 0; i < ExtendedPlayerManager.AllPlayersInLevel.Count; i++)
                    {
                        PlayerAgent playerAgent = ExtendedPlayerManager.AllPlayersInLevel[i];
                        if (playerAgent != null && playerAgent.CourseNode != null)
                        {
                            List<EnemyAgent> reachableEnemiesInNodes = Current.GetReachableEnemiesForPlayerAgent(playerAgent);

                            if (reachableEnemiesInNodes.Count > 0)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
        }




        public static void SetupEnemy(EnemyAgent enemyAgent, AgentMode mode)
        {
            if (mode == AgentMode.Scout)
            {
                enemyAgent.IsScout = true;
            }
        }


        public static bool AnyEnemiesAlerted
        {
            get
            {
                return AggressiveEnemies > 0;
            }
        }

        public static int EnemiesAlerted
        {
            get
            {
                return AggressiveEnemies;
            }
        }


        public static int LocalEnemiesSeen
        {
            get
            {
                if (PlayerManager.TryGetLocalPlayerAgent(out PlayerAgent agent))
                {
                    return EnemiesSeenCharacter(agent.CharacterID);
                }
                return 0;
            }
        }


        public static int EnemiesSeenCharacter(int characterID) => Current.m_enemyVisibilites[characterID];

        public float[] m_enemyScores;


        public int[] m_enemyVisibilites;


        public static bool EnemiesCloseBy
        {
            get
            {
                if (!AnyEnemiesReachablewithinPlayers)
                {
                    return false;
                }

                return CheckVisibleEnemies;
            }
        }


    }
}
