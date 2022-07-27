using Agents;
using ChatterReborn.ChatterEvent;
using ChatterReborn.Data;
using ChatterReborn.Managers;
using ChatterReborn.Utils;
using Enemies;
using GameData;
using Player;
using UnityEngine;

namespace ChatterReborn.Drama_Chatter_States
{
    public class DRAMA_Chatter_Sneaking : DRAMA_Chatter_Base
    {

        public override void OnDamagedEnemy(EnemyDamageEvent damageEvent)
        {
            if (damageEvent.m_damageType != DamageType.Melee)
            {
                return;
            }

            if (!damageEvent.m_killed)
            {
                return;
            }


            if (damageEvent.m_attacker != null && damageEvent.m_damageReceiver != null)
            {
                float chance = 1f;
                if (this.HasOwner && this.Owner.PlayerCharacterFilter == DialogCharFilter.Char_F)
                {
                    chance = 0.5f;
                    ChatterDebug.LogWarning("\tThe player is Hackett and has sneaking scout kill voicelines...");
                }

                bool roll = chance == 1f || UnityEngine.Random.value < chance;

                if (EnemyDetectionManager.IsScout(damageEvent.m_damageReceiver))
                {
                    ChatterDebug.LogWarning("Scout is supposed to be dead, changing to Alert DRAMA_State...");
                    if (roll)
                    {
                        DramaManager.ChangeState(DRAMA_State.Alert, true);
                    }
                }
                else
                {
                    if (!this.m_first_kill || this.m_kill_comment_cooldown < Clock.Time)
                    {
                        this.m_first_kill = true;
                        this.m_kill_comment_cooldown = Clock.Time + UnityEngine.Random.Range(this.m_cooldown_kill.x, this.m_cooldown_kill.y);
                        if (UnityEngine.Random.value <= 0.75f && ConfigurationManager.SneakKillCommentsEnabled)
                        {
                            PrisonerDialogManager.DelayDialogForced(1f, GD.PlayerDialog.on_enemy_kill, this.m_owner);
                        }
                    }
                }
            }

        }



        public override void Setup()
        {
            this.m_cooldown_kill = new Vector2(3f, 5f);
            this.m_first_sensitive_light = false;
            this.m_sensitive_light_t = 0f;
            base.Setup();
        }

        private void CheckFlashLightSensitivity()
        {
            if (!ConfigurationManager.FlashLightSensitivityDialoguesEnabled)
            {
                return;
            }

            if (!this.Machine.AllowedToParticipate)
            {
                return;
            }

            if (m_first_sensitive_light && this.m_sensitive_light_t > Clock.Time)
            {
                return;
            }


            m_first_sensitive_light = true;
            this.m_sensitive_light_t = Clock.Time + UnityEngine.Random.Range(1f, 1.25f);

            


            for (int i1 = 0; i1 < ExtendedPlayerManager.AllPlayersInLevel.Count; i1++)
            {
                PlayerAgent playerAgent = ExtendedPlayerManager.AllPlayersInLevel[i1];
                if (playerAgent != null && playerAgent != this.Owner)
                {
                    float distance = (playerAgent.Position - this.Owner.Position).magnitude;
                    if (distance < 10f)
                    {
                        var enemies = AIGraph.AIG_CourseGraph.GetReachableEnemiesInNodes(this.Owner.CourseNode, 2);
                        for (int i = 0; i < enemies.Count; i++)
                        {
                            EnemyAgent enemy = enemies[i];
                            if (enemy.Alive && enemy.AI != null && enemy.AI.Mode == AgentMode.Hibernate && !enemy.IsScout)
                            {
                                Vector3 vector = playerAgent.EyePosition - enemy.m_position;
                                float magnitude = vector.magnitude;
                                if (playerAgent.HasDetectionMod(out distance) && magnitude < distance)
                                {
                                    if (playerAgent.Inventory.FlashlightEnabled)
                                    {
                                        PlayerDialogManager.WantToStartDialog(GD.PlayerDialog.monster_light_sensitivity, this.Owner);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }

            }
        }

        public override void Update()
        {
            this.CheckFlashLightSensitivity();
            this.TryToOrderBackPlayer();
        }

        private Vector2 m_cooldown_kill;
        private bool m_first_sensitive_light;

        private float m_sensitive_light_t;
        private float m_kill_comment_cooldown;
        private bool m_first_kill;
    }
}
