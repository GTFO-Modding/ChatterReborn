using ChainedPuzzles;
using ChatterReborn.ChatterEvent;
using ChatterReborn.Data;
using ChatterReborn.Machines;
using ChatterReborn.Managers;
using ChatterReborn.Utils;
using ChatterReborn.Utils.Machine;
using ChatterRebornSettings;
using GameData;
using Player;
using System.Collections.Generic;
using UnityEngine;
using static ChatterReborn.Utils.CallBackUtils;

namespace ChatterReborn.Drama_Chatter_States
{
    public class DRAMA_Chatter_Base : MachineState<DramaChatterMachine>
    {

        public virtual bool IsCombatState => false;


        protected PlayerAgent Owner
        {
            get
            {
                return this.m_owner;
            }
        }


        public virtual void OnEnemyDetected()
        {

        }

        public virtual void OnEnemyUnDetected()
        {
        }

        protected void TryToOrderBackPlayer()
        {
            if (Settings.Misc.AllowOrderBioScan_Drama_State)
            {
                if (!this.m_shout_triggered || this.m_next_shout_trigger_time < Time.time)
                {
                    this.m_shout_triggered = true;
                    this.m_next_shout_trigger_time = Time.time + Random.Range(0.25f, 0.75f);
                    this.OnOrderBackToBioScanPrisoner();
                }
            }            
        }

        private void UpdateSeperatedFromGroup()
        {
            float farthestDistance = 0f;
            float myDistanceFromThem = 0f;
            List<PlayerAgent> teammates = ExtendedPlayerManager.AllPlayersInLevel;
            for (int i = 0; i < teammates.Count; i++)
            {
                var firstTeammate = teammates[i];
                if (this.Owner != firstTeammate)
                {
                    myDistanceFromThem = Mathf.Max(myDistanceFromThem, Vector3.Distance(this.Owner.Position, firstTeammate.Position));
                    for (int j = 0; i < teammates.Count; i++)
                    {
                        var secondTeammate = teammates[j];
                        if (secondTeammate != firstTeammate)
                        {
                            if (firstTeammate.Alive && firstTeammate.Alive)
                            {
                                float distance = Vector3.Distance(firstTeammate.Position, secondTeammate.Position);
                                farthestDistance = Mathf.Max(distance, farthestDistance);
                            }
                        }
                    }
                }
                
            }

            if (farthestDistance > 15f)
            {
                return;
            }

            if (farthestDistance < 20f)
            {
                return;
            }

            if (CoolDownManager.HasCooldown(CoolDownType.SeperatedFromGroup))
            {
                return;
            }

            this.Owner.WantToStartDialog(GD.PlayerDialog.CL_BRB, false);
            CoolDownManager.ApplyCooldown(CoolDownType.SeperatedFromGroup, 180f);
        }







        private void OnOrderBackToBioScanPrisoner()
        {
            if (ChainedPuzzleManager.Current.m_instances == null)
            {
                return;
            }

            if (ChainedPuzzleManager.Current.m_instances.Count <= 0)
            {
                return;
            }


            if (this.Owner == null)
            {
                return;
            }



            bool[] playersInTeamScan = new bool[4];


            bool isLocalPlayerInScan = false;

            List<ChainedPuzzleInstance> chainedpuzzleInstances = Il2cppUtils.ToSystemList(ChainedPuzzleManager.Current.m_instances);
            for (int i2 = 0; i2 < chainedpuzzleInstances.Count; i2++)
            {
                ChainedPuzzleInstance instance = chainedpuzzleInstances[i2];

                if (instance != null && instance.m_chainedPuzzleCores != null)
                {
                    for (int i1 = 0; i1 < instance.m_chainedPuzzleCores.Count; i1++)
                    {
                        iChainedPuzzleCore chainedPuzzleCore = instance.m_chainedPuzzleCores[i1];

                        if (chainedPuzzleCore != null)
                        {
                            CP_Bioscan_Core core = chainedPuzzleCore.TryCast<CP_Bioscan_Core>();
                            if (core != null)
                            {
                                bool isScanning = core.m_sync.GetCurrentState().status == eBioscanStatus.Waiting | core.m_sync.GetCurrentState().status == eBioscanStatus.Scanning;
                                if (core != null && core.m_sync != null && isScanning && core.m_playerScanner != null && core.m_playerScanner.ScanPlayersRequired == PlayerRequirement.All)
                                {
                                    if (core.PlayersOnScan != null)
                                    {
                                        var m_players = Il2cppUtils.ToSystemList(core.PlayersOnScan);
                                        for (int i = 0; i < m_players.Count; i++)
                                        {
                                            PlayerAgent player = m_players[i];
                                            playersInTeamScan[player.CharacterID] = true;
                                            if (player.CharacterID == this.m_owner.CharacterID)
                                            {
                                                isLocalPlayerInScan = true;
                                            }
                                        }
                                    }
                                }
                            }


                        }

                    }
                }

            }

            if (!isLocalPlayerInScan)
            {
                return;
            }

            if (this.Owner.FPSCamera == null || this.Owner.FPSCamera.CameraRayDist > 35f || this.Owner.FPSCamera.CameraRayObject == null)
            {
                return;
            }


            var camRayObj = this.Owner.FPSCamera.CameraRayObject;

            var targetPlayer = camRayObj.GetComponentInParent<PlayerAgent>();
            if (targetPlayer != null)
            {
                if (!playersInTeamScan[targetPlayer.CharacterID])
                {
                    uint dialogID;
                    if (this.m_chainedpuzzle_dialogs.TryGetValue(targetPlayer.PlayerCharacterFilter, out dialogID))
                    {
                        if (ConfigurationManager.IrritatedScanningCommentEnabled)
                        {
                            this.Owner.WantToStartDialog(dialogID);
                        }
                    }
                }
            }


        }

        public void SetOwner(PlayerAgent owner)
        {
            this.m_owner = owner;
        }


        protected PlayerBackpack GetBackpack
        {
            get
            {
                return PlayerBackpackManager.GetBackpack(this.m_owner.Owner);
            }
        }


        protected bool HasOwner
        {
            get
            {
                return this.m_owner != null;
            }
        }

        public override void Update()
        {

        }

        public override void Setup()
        {
            this.m_depleted_consumable = new CallBack<PlayerAgent>(this.TriggerDepletedConsumablesDialogue);
            this.m_spitter_explodeCallBack = new CallBack<InfectionSpitter>(this.OnExplodedComment);
            this.m_revive_commentCallBack = new CallBack(OnRevivedComment);
            this.m_triggerDialogCallback = new CallBack<uint, bool>(this.TriggerDialog);
            this.m_first_spitter = false;
        }



        public override void Enter()
        {

        }

        public override void Exit()
        {

        }

        public override void OnDestroyed()
        {
            this.m_depleted_consumable.RemoveCallBack();
            this.m_spitter_explodeCallBack.RemoveCallBack();
            this.m_revive_commentCallBack.RemoveCallBack();
            this.m_triggerDialogCallback.RemoveCallBack();
        }

        public override void OnLevelCleanUp()
        {
            this.m_depleted_consumable.RemoveCallBack();
            this.m_spitter_explodeCallBack.RemoveCallBack();
            this.m_revive_commentCallBack.RemoveCallBack();
        }

        public virtual void OnDamagedEnemy(EnemyDamageEvent damageEvent)
        {
        }

        public virtual void OnLocalDamage(float damage)
        {
        }

        public virtual void OnTeammatesDamage(float damage)
        {
        }


        public virtual void OnThrowConsumable(ItemEquippable itemthrown)
        {
            if (itemthrown.Owner.Alive && this.m_machine.AllowedToParticipate)
            {
                var playerBackpack = PlayerBackpackManager.GetBackpack(itemthrown.Owner.Owner);
                if (playerBackpack != null && playerBackpack.AmmoStorage != null && playerBackpack.AmmoStorage.ConsumableAmmo.BulletsInPack == 0)
                {
                    if (playerBackpack.AmmoStorage.ConsumableAmmo.AmmoMaxCap > 1f)
                    {
                        this.m_depleted_consumable?.QueueCallBack(new MinMaxTimer(0.5f, 1f), itemthrown.Owner);
                    }
                }
            }

        }

        public virtual void OnSpitterExplode(InfectionSpitter infectionSpitter)
        {

        }

        private void OnExplodedComment(InfectionSpitter infectionSpitter)
        {
            var playerAgent = this.m_owner;
            if (playerAgent is null || infectionSpitter is null)
            {
                return;
            }

            if (this.m_first_spitter && this.m_spitter_comment_cooldown_t > Clock.Time)
            {
                return;
            }


            float magnitude = (playerAgent.EyePosition - infectionSpitter.m_position).magnitude;

            if (magnitude > 5f)
            {
                return;
            }

            if (playerAgent.Alive)
            {
                this.m_spitter_comment_cooldown_t = Clock.Time + 25f;
                this.m_first_spitter = true;

                WeightHandler<uint> weightHandler = WeightHandler<uint>.CreateWeightHandler();
                weightHandler.AddValue(GD.PlayerDialog.bit_by_parasite, 2f);
                weightHandler.AddValue(GD.PlayerDialog.get_parasite, 1f);

                if (ConfigurationManager.OnExplodeSpitterCommentsEnabled && UnityEngine.Random.value < 0.65 && this.m_machine.AllowedToParticipate)
                {
                    PlayerDialogManager.WantToStartDialogForced(weightHandler.Best.Value, playerAgent);
                }

            }
        }

        protected void TriggerDepletedConsumablesDialogue(PlayerAgent user)
        {
            if (user != null && user.IsLocallyOwned && user.Alive && ConfigurationManager.ConsumableDepletedCommentsEnabled)
            {
                PlayerDialogManager.WantToStartDialogForced(GD.PlayerDialog.consumable_depleted_generic, user);
            }
        }

        protected void TriggerDialog(uint dialogID, bool force = false)
        {
            PlayerDialogManager.WantToStartDialog(dialogID, this.m_owner.CharacterID, false, force);
        }

        public virtual void OtherPlayerSyncWield(ItemEquippable wield)
        {
        }
        public virtual void Revived()
        {
            if (this.HasOwner)
            {
                CallBack callback = new CallBack(OnRevivedComment);
                callback.QueueCallBack(1f);
            }
        }

        protected void OnRevivedComment()
        {
            if (ConfigurationManager.RevivedCommentEnabled && this.m_machine.AllowedToParticipate)
            {
                this.Owner.WantToStartDialog(GD.PlayerDialog.CL_ThankYou, false);
            }
        }

        protected void WantToStartDialog(uint dialogID)
        {
            this.Owner.WantToStartDialog(dialogID);
        }




        public void SetBot(bool enabled)
        {
            this.m_isbot = enabled;
        }


        private CallBack<PlayerAgent> m_depleted_consumable;

        protected CallBack<InfectionSpitter> m_spitter_explodeCallBack;

        protected CallBack m_revive_commentCallBack;

        protected CallBack<uint, bool> m_triggerDialogCallback;

        private float m_spitter_comment_cooldown_t;

        private bool m_first_spitter;

        protected PlayerAgent m_owner;


        protected bool m_isbot;

        private Dictionary<DialogCharFilter, uint> m_chainedpuzzle_dialogs = new Dictionary<DialogCharFilter, uint>()
        {
            {
                DialogCharFilter.Char_G,
                GD.PlayerDialog.order_to_bio_scan_woods
            },
            {
                DialogCharFilter.Char_T,
                GD.PlayerDialog.order_to_bio_scan_dauda
            },
            {
                DialogCharFilter.Char_F,
                GD.PlayerDialog.order_to_bio_scan_hackett
            },
            {
                DialogCharFilter.Char_O,
                GD.PlayerDialog.order_to_bio_scan_bishop
            }
        };
        private bool m_shout_triggered;

        private float m_next_shout_trigger_time;


        protected new void DebugPrint(string txt)
        {
            base.DebugPrint(" characterID [" + this.m_owner.CharacterID + "] " + txt);
        }


    }
}
