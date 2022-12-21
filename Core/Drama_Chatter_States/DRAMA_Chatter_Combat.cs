using ChatterReborn.ChatterEvent;
using ChatterReborn.Data;
using ChatterReborn.Managers;
using ChatterReborn.Utils;
using GameData;
using Gear;
using Player;
using System;
using UnityEngine;

namespace ChatterReborn.Drama_Chatter_States
{
    public class DRAMA_Chatter_Combat : DRAMA_Chatter_Base
    {

        
        public static readonly Vector2 Combat_CallOut_Chance = new Vector2(0.005f, 0.025f);
        public static readonly Vector2 Combat_CallOut_Delay = new Vector2(10f, 3f);

        public static readonly Vector2 Combat_Chatter_Tension = new Vector2(50f, 100f);

        public override void Update()
        {
            CheckHeatSituation();
            TryToOrderBackPlayer();
            UpdateCombatLocalChatter();
        }

        private void UpdateCombatLocalChatter()
        {
            if (this.Owner.Inventory.WieldedItem == null)
            {
                return;
            }

            var weapon = this.Owner.Inventory.WieldedItem.TryCast<BulletWeapon>();
            if (weapon == null)
            {
                return;
            }
            if (Time.time < weapon.m_lastFireTime + 2f)
            {
                if (EnemiesInLineOfSight > 2)
                {
                    this.UpdateCombatChatter();
                }
            }
        }

        public override void SyncUpdate()
        {
            CheckHeatSituation();                                 
        }

        private bool IsStillInCombat
        {
            get
            {
                switch (DramaManager.CurrentStateEnum)
                {                    
                    case DRAMA_State.Encounter:
                    case DRAMA_State.Combat:
                    case DRAMA_State.Survival:
                    case DRAMA_State.IntentionalCombat:
                        return true;
                }
                return false;
            }
        }


        private void CheckHeatSituation()
        {
            if (EnemyDetectionManager.EnemiesAlerted == 1)
            {
                if (!this.m_last_enemy_remaining)
                {
                    this.m_last_enemy_remaining = true;
                    this.m_last_enemy_remaining_duration = Clock.Time + 10f;
                }
            }
            else if (EnemyDetectionManager.EnemiesAlerted > 1)
            {
                this.m_last_enemy_remaining = false;
                if (this.m_heat_level > 20f)
                {
                    this.m_triggerEndCombatDialog = true;
                }
            }



            this.m_heat_level = Math.Max(this.m_machine.Intensity, this.m_heat_level);
        }


        protected virtual bool StartCombatDialogue => EnemyDetectionManager.EnemiesAlerted > 7 || Mastermind.HasEventOfType(eMastermindEventType.SurvivalWave);

        private int EnemiesInLineOfSight
        {
            get
            {

                if (this.Owner.CourseNode == null)
                {
                    return 0;
                }


                var enemies = Il2cppUtils.ToSystemList(AIGraph.AIG_CourseGraph.GetReachableEnemiesInNodes(this.Owner.CourseNode, 3));
                int enemiesAlertedVisible = 0;
                for (int i = 0; i < enemies.Count; i++)
                {
                    var enemy = enemies[i];

                    if (enemy != null && enemy.Alive && enemy.AI.Mode == Agents.AgentMode.Agressive)
                    {
                        if (this.Owner.CanSee(enemy.gameObject))
                        {
                            Vector3 targetDir = enemy.Position - this.Owner.Position;
                            float angle = Vector3.Angle(targetDir, this.Owner.Forward);
                            if (angle < 6f)
                            {
                                enemiesAlertedVisible++;
                            }
                        }
                    }
                }


                return enemiesAlertedVisible;
            }
        }

        public void UpdateCombatChatter()
        {
            if (this.m_machine.Intensity < Combat_Chatter_Tension.x)
            {
                return;
            }



            float tension_val = (Mathf.Min(this.m_machine.Intensity, Combat_Chatter_Tension.y) - Combat_Chatter_Tension.x) / (Combat_Chatter_Tension.y - Combat_Chatter_Tension.x);

            if (_combatChatterStarted)
            {
                float delay = Mathf.Lerp(Combat_CallOut_Delay.x, Combat_CallOut_Delay.y, tension_val);
                float delay_t = LastCombatChatter + delay;
                if (Time.time < delay_t)
                {
                    return;
                }
            }

            float chance = Mathf.Lerp(Combat_CallOut_Chance.x, Combat_CallOut_Chance.y, tension_val);

            if (chance < UnityEngine.Random.value)
            {
                return;
            }

            LastCombatChatter = Time.time;
            if (ConfigurationManager.PersistentCombatChatterEnabled)
            {
                this.Owner.WantToStartDialog(GD.PlayerDialog.idle_combat, true);
            }

        }


        private float LastCombatChatter
        {
            get
            {
                return _lastCombatChatterTime;
            }
            set
            {
                _lastCombatChatterTime = value;
                _combatChatterStarted = true;
            }
        }

        private float _lastCombatChatterTime;

        private bool _combatChatterStarted;

        public void OnHitReaction()
        {
            if (this.m_damageVoiceTimer < Time.time)
            {
                uint afterGruntDialogID = this.Owner.Damage.GetHealthRel() < 0.3f ? GD.PlayerDialog.low_health_limit : GD.PlayerDialog.after_damage_generic;
                PlayerVoiceManager.WantToSayAndStartDialog(this.Owner.CharacterID, EVENTS.PLAY_LOWHEALTHGRUNT01_1A, afterGruntDialogID);
                this.m_damageVoiceTimer = Time.time + 3f;
            }
        }

        public void LoadLastCombatData()
        {
            if (this.m_machine.HasLastCombatData)
            {
                this.m_accumulatedLocalDamage = this.m_machine.m_lastCombatData.localDamageReceived;
                this.m_accumulatedTeamDamage = this.m_machine.m_lastCombatData.teamDamageReceived;
                this.m_scoutAlerted = this.m_machine.m_lastCombatData.scoutAlerted;
                this.m_combatStateEnterTime = this.m_machine.m_lastCombatData.combatStateEnterTime;
                this.m_triggerEndCombatDialog = this.m_machine.m_lastCombatData.triggerCombatEndDialog;
                this.m_heat_level = this.m_machine.m_lastCombatData.heatLevel;
                this.m_machine.HasLastCombatData = false;
            }            
        }




        public override void Setup()
        {
            this.m_start_idle_dialog = new CallBackUtils.CallBack(this.TriggerStartCombat);
            this.m_end_dialog = new CallBackUtils.CallBack<bool>(TriggerEndCombatDialogue);
            this.m_idle_combat = new CallBackUtils.CallBack(this.TriggerIdleCobmatDialog);
            this.m_hear_hunter_group = new CallBackUtils.CallBack(this.TriggerHordeDialog);
            this.killed_single_monster = new CallBackUtils.CallBack(this.TriggerSingleEnemyKilled);
            base.Setup();
        }

        public override void Enter()
        {
            this.m_accumulatedLocalDamage = 0f;
            this.CommonEnter();
        }

        public override void SyncEnter()
        {
            this.CommonEnter();
        }

        private void CommonEnter()
        {

            this.m_start_idle_dialog.QueueCallBack(UnityEngine.Random.Range(2f, 3f));
            this.m_accumulatedTeamDamage = 0f;
            this.m_triggerEndCombatDialog = false;
            this.m_killed_last_monster = false;
            this.m_combatStateEnterTime = this.StateChangeTime;
            this.m_last_enemy_remaining = false;
            this.m_heat_level = 0f;
            this.LoadLastCombatData();
        }

        private void TriggerIdleCobmatDialog()
        {
            if (this.HasOwner && this.Owner.Alive)
            {
                WeightHandler<uint> combatComs = WeightHandler<uint>.CreateWeightHandler();
                if (DramaManager.PlayersSeparated)
                {
                    if (ConfigurationManager.GroupStayCloseCombatDialoguesEnabled && this.m_machine.AllowedToParticipate)
                    {
                        combatComs.AddValue(GD.PlayerDialog.group_is_not_together, 3f);
                    }
                }


                ExtendedPlayerManager.GetWeaponStats(this.m_owner, out float totalAmmoRel, out int weaponCount);
                if (weaponCount > 0 && totalAmmoRel < 0.05f)
                {
                    combatComs.AddValue(GD.PlayerDialog.CL_INeedAmmo, 1f);
                }

                if (combatComs.TryToGetBestValue(out WeightValue<uint> val))
                {
                    PlayerDialogManager.WantToStartDialog(val.Value, this.m_owner);
                }
            }

            this.NextDialogLoop();
        }

        public void ScoutScreamed()
        {
            this.m_scoutAlerted = true;
        }


        public override bool IsCombatState => true;
        private void TriggerEndCombatDialogue(bool scoutAlerted)
        {
            if (this.m_machine.CurrentStateName == DRAMA_State.Alert || this.m_machine.CurrentStateName == DRAMA_State.Exploration)
            {
                if (!this.HasOwner || !this.Owner.Alive)
                {
                    return;
                }


                if (!ConfigurationManager.EndOfCombatDialogueEnabled)
                {
                    ChatterDebug.LogWarning("Could not trigger End Combat Dialogue. User does not have the option enabled.");
                    return;
                }

                if (!this.m_machine.AllowedToParticipate)
                {
                    return;
                }

                float totalTimeSpentInCombat = Clock.Time - this.m_combatStateEnterTime;
                if (totalTimeSpentInCombat < 60f)
                {
                    if (totalTimeSpentInCombat < 8f && UnityEngine.Random.value < 0.25f)
                    {
                        PlayerDialogManager.WantToStartDialog(GD.PlayerDialog.glottal_stop, this.Owner);
                        ChatterDebug.LogWarning("Combat duration was too short. BUT we can trigger glottal_stop dialogue.");
                        return;
                    }
                    ChatterDebug.LogWarning("Could not trigger End Combat Dialogue. Combat duration was too short.");
                    return;
                }

                if (EnemyDetectionManager.EnemiesCloseBy)
                {
                    ChatterDebug.LogWarning("Could not trigger End Combat Dialogue. Enemies are still present.");
                    return;
                }

                if (EnemyDetectionManager.AnyEnemiesAlerted)
                {
                    ChatterDebug.LogWarning("Could not trigger End Combat Dialogue. There are enemies still alerted.");
                    return;
                }



                if (!this.m_triggerEndCombatDialog)
                {
                    ChatterDebug.LogWarning("Could not trigger End Combat Dialogue. Heat was too low.");
                    return;
                }

                uint combatResultsDialog = GD.PlayerDialog.encounter_over_good;
                float damageTaken = this.m_accumulatedTeamDamage + this.m_accumulatedLocalDamage;
                float playerDataHealth = DramaManager.PlayerData.health;
                if (scoutAlerted)
                {
                    combatResultsDialog = GD.PlayerDialog.encounter_over_scout;
                }
                else if (damageTaken > playerDataHealth * 2f || this.m_heat_level > 100f)
                {
                    combatResultsDialog = GD.PlayerDialog.encounter_over_bad;
                }
                else if (damageTaken > playerDataHealth || this.m_heat_level > 60f)
                {
                    combatResultsDialog = GD.PlayerDialog.encounter_over_average;
                }

                this.DebugPrint("Now Triggering Combat End Dialogue : " + PlayerDialogDataBlock.GetBlockName(combatResultsDialog));
                this.Owner.WantToStartDialog(combatResultsDialog);
            }
        }

        private void TriggerSingleEnemyKilled()
        {
            this.Owner.WantToStartDialog(GD.PlayerDialog.killed_single_monster);
        }

        private void TriggerHordeDialog()
        {
            if (this.HasOwner)
            {
                if (ConfigurationManager.HearHunterGroupDialogueEnabled)
                {
                    if (this.m_machine.AllowedToParticipate)
                    {
                        this.Owner.WantToStartDialog(GD.PlayerDialog.attracted_monsters_intentional, false);
                    }
                }
            }
        }

        private void NextDialogLoop()
        {
            this.m_idle_combat.QueueCallBack(new MinMaxTimer(this.m_idle_delay.x, this.m_idle_delay.y));
        }

        private bool CanTriggerLastEnemyKilledDialogue
        {
            get
            {
                if (!ConfigurationManager.KilledSingleMonsterCommentEnabled)
                {
                    return false;
                }

                if (!this.m_machine.AllowedToParticipate)
                {
                    return false;
                }


                if (!this.m_killed_last_monster)
                {
                    return false;
                }

                if (!this.m_last_enemy_remaining)
                {
                    return false;
                }

                if (this.m_last_enemy_remaining_duration > Clock.Time)
                {
                    return false;
                }

                if (this.m_last_enemy_killed_expire < Clock.Time)
                {
                    return false;
                }

                return true;
            }
        }

        private void CleanAllCallBacks()
        {
            this.m_hear_hunter_group.RemoveCallBack();
            this.killed_single_monster.RemoveCallBack();
            this.m_end_dialog.RemoveCallBack();
            this.m_idle_combat.RemoveCallBack();
            this.m_start_idle_dialog.RemoveCallBack();
        }

        public override void OnDestroyed()
        {
            this.CleanAllCallBacks();
            base.OnDestroyed();
        }

        public override void OnLevelCleanUp()
        {            
            this.CleanAllCallBacks();
            base.OnLevelCleanUp();
        }

        private void CommonExit()
        {

            bool isStillInCombat = this.IsStillInCombat;

            if (isStillInCombat)
            {
                this.DebugPrint("We are still in combat");
            }
            if (EnemyDetectionManager.AnyEnemiesAlerted || isStillInCombat)
            {
                this.SaveCombatData();
            }

            if (!isStillInCombat)
            {
                if (EnemyDetectionManager.EnemiesAlerted > 7)
                {
                    this.m_hear_hunter_group.QueueCallBack(UnityEngine.Random.Range(0.5f, 1f));
                }
                else if (this.CanTriggerLastEnemyKilledDialogue)
                {
                    ChatterDebug.LogWarning("Triggering last enemy killed dialogue...");
                    this.killed_single_monster.QueueCallBack(UnityEngine.Random.Range(0.5f, 1f));
                }
                else
                {
                    this.m_end_dialog.QueueCallBack(new MinMaxTimer(3f, 5f), this.m_scoutAlerted);
                }
            }
                       
            this.m_idle_combat.RemoveCallBack();
            this.m_start_idle_dialog.RemoveCallBack();
            this.m_killed_last_monster = false;
            this.m_scoutAlerted = false;
        }

        private void SaveCombatData()
        {
            this.m_machine.m_lastCombatData = new CombatData
            {
                teamDamageReceived = this.m_accumulatedTeamDamage,
                localDamageReceived = this.m_accumulatedLocalDamage,
                scoutAlerted = this.m_scoutAlerted,
                combatStateEnterTime = this.m_combatStateEnterTime,
                triggerCombatEndDialog = this.m_triggerEndCombatDialog,
                heatLevel = this.m_heat_level
            };
            this.m_machine.HasLastCombatData = true;
        }

        public override void Exit()
        {
            this.CommonExit();
        }

        public override void SyncExit()
        {
            this.CommonExit();
        }

        private void TriggerStartCombat()
        {
            if (!PlayerManager.PlayersAreSeparated() && this.StartCombatDialogue)
            {
                if (this.HasOwner)
                {
                    if (ConfigurationManager.StartCombatDialogueEnabled)
                    {
                        if (this.m_machine.AllowedToParticipate)
                        {
                            bool success = this.IsStillInCombat;
                            if (success)
                            {
                                this.m_owner.WantToStartDialog(GD.PlayerDialog.combat_start, false);
                            }                                
                        }
                    }
                }
            }

            this.NextDialogLoop();
        }

        public override void OnDamagedEnemy(EnemyDamageEvent damageEvent)
        {
            if (damageEvent.m_damageReceiver == null)
            {
                return;
            }

            if (damageEvent.m_attacker == null)
            {
                return;
            }

            eEnemyType enemyType = damageEvent.m_damageReceiver.EnemyData.EnemyType;
            if (enemyType != eEnemyType.Boss && enemyType != eEnemyType.MiniBoss)
            {
                if (damageEvent.m_killed && EnemyDetectionManager.EnemiesAlerted <= 1)
                {
                    //this.m_killed_last_monster = EnemyDetectionManager.IsEnemyLastAggressive(damageEvent.m_damageReceiver);
                    this.m_killed_last_monster = true;
                    if (this.m_killed_last_monster)
                    {
                        this.m_last_enemy_killed_expire = Clock.Time + 3f;
                        ChatterDebug.LogWarning("Killed last monster chance to trigger related dialogue??");
                    }
                }
            }
            else if (enemyType == eEnemyType.Boss)
            {
                string key = CoolDownType.WeakSpotAttacked + "_" + this.Owner.CharacterID;
                if (!CoolDownManager.HasCooldown(key))
                {
                    if (damageEvent.m_damageReceiver.EnemyDataID == GD.Enemy.Tank)
                    {
                        this.Owner.WantToStartDialog(GD.PlayerDialog.heal_spray_apply_enemy);
                        CoolDownManager.ApplyCooldown(key, 5f);
                    }
                }
            }
        }

        public override void OnLocalDamage(float damage)
        {
            this.m_accumulatedLocalDamage += damage;
        }

        public override void OnTeammatesDamage(float damage)
        {
            this.m_accumulatedTeamDamage += damage;
        }

        private float m_accumulatedTeamDamage;

        private bool m_triggerEndCombatDialog;

        private bool m_killed_last_monster;

        private float m_combatStateEnterTime;

        private bool m_last_enemy_remaining;
        private float m_heat_level;
        private float m_last_enemy_remaining_duration;

        private bool m_scoutAlerted = false;

        private Vector2 m_idle_delay = new Vector2(25f, 40f);

        private CallBackUtils.CallBack m_start_idle_dialog;

        private CallBackUtils.CallBack<bool> m_end_dialog;


        private CallBackUtils.CallBack m_idle_combat;

        private CallBackUtils.CallBack m_hear_hunter_group;

        private CallBackUtils.CallBack killed_single_monster;
        private float m_last_enemy_killed_expire;
        private float m_damageVoiceTimer;
        private float m_accumulatedLocalDamage;
    }
}
