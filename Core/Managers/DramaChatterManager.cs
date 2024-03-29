﻿using CellMenu;
using ChatterReborn.Data;
using ChatterReborn.Drama_Chatter_States;
using ChatterReborn.Machines;
using ChatterReborn.Utils;
using ChatterRebornSettings;
using GameData;
using HarmonyLib;
using LevelGeneration;
using Player;
using SNetwork;
using System;
using System.Reflection;

namespace ChatterReborn.Managers
{
    public class DramaChatterManager : ChatterManager<DramaChatterManager>
    {

        protected override void PostSetup()
        {
            this.m_patcher.Patch<DramaManager>(nameof(DramaManager.ChangeState), HarmonyPatchType.Postfix, BindingFlags.Static | BindingFlags.Public);
            this.m_patcher.Patch<DramaManager>(nameof(DramaManager.CheckSyncedPlayerStates), HarmonyPatchType.Postfix, BindingFlags.Static | BindingFlags.Public);
            this.m_patcher.Patch<LockMelterFirstPerson>(nameof(LockMelterFirstPerson.Setup), HarmonyPatchType.Postfix, BindingFlags.Instance | BindingFlags.Public);
            this.m_patcher.Patch<CM_PageExpeditionFail>(nameof(CM_PageExpeditionFail.OnEnable), HarmonyPatchType.Postfix, BindingFlags.Instance | BindingFlags.Public);
            this.m_patcher.Patch<InfectionSpitter>(nameof(InfectionSpitter.DoExplode), HarmonyPatchType.Postfix, BindingFlags.Instance | BindingFlags.Public);
        }


        private static void InfectionSpitter__DoExplode__Postfix(InfectionSpitter __instance)
        {
            DramaChatterManager.CurrentState.OnSpitterExplode(__instance);
        }

        private static void DramaManager__ChangeState__Postfix(DRAMA_State state, bool doSync)
        {
            DramaChatterManager.OnChangeState(state, doSync);
        }

        static void LockMelterFirstPerson__Setup__Postfix(LockMelterFirstPerson __instance)
        {
            void OnTriggered(PlayerAgent playerAgent)
            {
                DramaChatterMachine dramaChatterMachine = DramaChatterManager.GetMachine(__instance.Owner);
                dramaChatterMachine?.CurrentState?.OnThrowConsumable(__instance);
            }
            __instance.m_interactApplyResource.add_OnInteractionTriggered(new Action<PlayerAgent>(OnTriggered));
        }

        private static void DramaManager__CheckSyncedPlayerStates__Postfix(out bool hasCombat, out bool hasEncounter, out bool hasSneaking)
        {
            hasCombat = false;
            hasEncounter = false;
            hasSneaking = false;
            if (SNet.HasMaster)
            {
                DRAMA_State master_DramaState = DramaManager.SyncedPlayerStates[SNet.Master.CharacterIndex];
                if (master_DramaState == DRAMA_State.Encounter)
                {
                    hasEncounter = true;
                }
                if (master_DramaState == DRAMA_State.Combat || master_DramaState == DRAMA_State.IntentionalCombat || master_DramaState == DRAMA_State.Survival)
                {
                    hasCombat = true;
                }
                if (master_DramaState == DRAMA_State.Sneaking)
                {
                    hasSneaking = true;
                }
            }
        }

        private static void CM_PageExpeditionFail__OnEnable__Postfix(CM_PageExpeditionFail __instance)
        {
            if (__instance.m_isSetup)
            {
                if (ConfigurationManager.ExpeditionFailedDeathScreamEnabled)
                {
                    PrisonerDialogManager.DelayLocalDialogForced(UnityEngine.Random.Range(1f, 3f), GameData.GD.PlayerDialog.death_scream);
                }
                Current.DebugPrint("<<<<<<<<<DEATH SCREAM>>>>>>>>>>", eLogType.Message);
            }
        }

        public static void OtherPlayerSyncWield(ItemEquippable item)
        {
            for (int i = 0; i < Current.m_machines.Length; i++)
            {
                DramaChatterMachine machine = Current.m_machines[i];
                if (machine != null)
                {
                    machine.CurrentState.OtherPlayerSyncWield(item);
                }
            }
        }


        public static DramaChatterMachine GetMachine(PlayerAgent playerAgent)
        {
            return DramaChatterManager.Current.m_machines[playerAgent.CharacterID];
        }

        public override void Update()
        {
            if (!IsSetup)
            {
                return;
            }

            if (m_updateAction != null)
            {
                m_updateAction();
            }
        }

        public Action m_updateAction;


        public static void OnChangeState(DRAMA_State state, bool doSync)
        {
            for (int i = 0; i < Current.m_machines.Length; i++)
            {
                var machine = Current.m_machines[i];
                if (machine != null)
                {
                    machine.ChangeState(state);
                    if (doSync)
                    {
                        machine.WantToSyncDramaState(state);
                    }
                }
            }
        }

        

        public override void OnElevatorArrived()
        {
            if (WardenObjectiveManager.Current != null && WardenObjectiveManager.Current.TryGetActiveWardenObjectiveData(LG_LayerType.MainLayer, out var wardenObjectiveDataBlock))
            {
                if (wardenObjectiveDataBlock != null)
                {
                    bool hasEndlessWave = false;
                    foreach (var wave in wardenObjectiveDataBlock.WavesOnElevatorLand)
                    {
                        if (wave.TriggerAlarm)
                        {
                            hasEndlessWave = true;
                            break;
                        }
                    }
                    uint dialogToStart = GD.PlayerDialog.expedition_start_generic;
                    if (wardenObjectiveDataBlock.Type == eWardenObjectiveType.GatherSmallItems || wardenObjectiveDataBlock.Type == eWardenObjectiveType.RetrieveBigItems)
                    {
                        dialogToStart = GD.PlayerDialog.expedition_start_scavenge;
                    }
                    else if (wardenObjectiveDataBlock.Type == eWardenObjectiveType.ClearAPath && hasEndlessWave)
                    {
                        dialogToStart = GD.PlayerDialog.get_to_checkpoint;
                        if (WardenObjectiveManager.Current.m_objectiveItemCollection.TryGetValue(LayerChainIndex.ActiveChainIndex(LG_LayerType.MainLayer), out var items))
                        {
                            if (items != null && items.Count > 0)
                            {
                                dialogToStart = GD.PlayerDialog.get_to_checkpoint_with_thing;
                            }
                        }                       
                    }
                    if (ConfigurationManager.ExpeditionIntroDialogueEnabled)
                    {
                        PrisonerDialogManager.DelayLocalDialog(new MinMaxTimer(0.5f, 2f), dialogToStart);
                    }
                }
            }            
        }


        
        public override void On_Registered_PlayerAgent(PlayerAgent playerAgent)
        {            
            Current.SetupMachineForPlayer(playerAgent);
        }

        public override void On_Registered_LocalPlayerAgent(LocalPlayerAgent localPlayerAgent)
        {
            Current.SetupMachineForPlayer(localPlayerAgent);
        }

        public override void On_DeRegistered_PlayerAgent(PlayerAgent playerAgent)
        {
            Current.UnregisterMachineForPlayer(playerAgent);
        }
        public override void On_DeRegistered_LocalPlayerAgent(LocalPlayerAgent localPlayerAgent)
        {
            Current.UnregisterMachineForPlayer(localPlayerAgent);
        }

        protected override void Setup()
        {
            this.m_machines = new DramaChatterMachine[4];
            this._allow_participation = new bool[4];
            this.IsSetup = true;
        }

        public static void SetupMachine(PlayerAgent player)
        {
            Current.SetupMachineForPlayer(player);
        }

        public static void EnableParticipation(int characterID, bool enabled)
        {
            if (Settings.Drama.AllowParticipation)
            {
                Current._allow_participation[characterID] = enabled;
            }            
        }

        public static bool IsAllowedToParticipate(int characterID) => Current._allow_participation[characterID];

        private bool[] _allow_participation;

        private void SetupMachineForPlayer(PlayerAgent player)
        {
            int characterID = player.CharacterID;
            bool isBot = player.Owner.IsBot;
            bool isLocallyOwned = player.IsLocallyOwned;
            if (this.m_machines[characterID] != null)
            {
                ChatterDebug.LogWarning("Already setup for characterID " + characterID);
                return;
            }

            ChatterDebug.LogWarning("Is CharacterID " + characterID + " bot : " + isBot);

            this.m_machines[characterID] = new DramaChatterMachine();
            DramaChatterMachine machine = this.m_machines[characterID];
            machine.Setup(player);
            
            if (isLocallyOwned)
            {
                m_local_machine = machine;
            }

            if (GameStateManager.CurrentStateName == eGameStateName.InLevel)
            {
                if (SNet.IsMaster && isBot)
                {
                    this.DebugPrint("DramaMachine setup mid-game, changing the state -> " + DramaManager.CurrentStateEnum, eLogType.Warning);
                    machine.ChangeState(DramaManager.CurrentStateEnum);
                    machine.WantToSyncDramaState(DramaManager.CurrentStateEnum);                    
                }
            }

            if (isLocallyOwned)
            {
                ChatterDebug.LogWarning("[DialogDramaStateManager.SetupMachineForPlayer] Local DramaMachine has been set up for characterID " + characterID);
            }
            else if (isBot)
            {
                ChatterDebug.LogWarning("[DialogDramaStateManager.SetupMachineForPlayer] Bot DramaMachine has been set up for characterID " + characterID);
            }
            else
            {
                ChatterDebug.LogWarning("[DialogDramaStateManager.SetupMachineForPlayer] Husk DramaMachine has been set up for characterID " + characterID);
            }
        }


        public static void UnregisterMachine(PlayerAgent player)
        {
            Current.UnregisterMachineForPlayer(player);
        }

        private void UnregisterMachineForPlayer(PlayerAgent player)
        {
            int characterID = player.CharacterID;
            bool isLocallyOwned = player.IsLocallyOwned;
            if (this.m_machines[characterID] == null)
            {
                ChatterDebug.LogWarning("[DialogDramaStateManager.SetupMachineForPlayer] characterID " + characterID + " has no machine already");
                return;
            }


            var machine = this.m_machines[characterID];
            this.m_machines[characterID] = null;
            machine.OnDestroyed();
            
            if (isLocallyOwned)
            {
                this.m_local_machine = null;
            }

            ChatterDebug.LogWarning("[DialogDramaStateManager.UnregisterMachineForPlayer] BotDramaMachine has been unregistered for characterID " + characterID);
        }

        public override void OnLevelCleanUp()
        {
            for (int i = 0; i < m_machines.Length; i++)
            {
                DramaChatterMachine machine = this.m_machines[i];
                if (machine != null)
                {
                    machine.OnLevelCleanUp();
                }
            }

        }

        private DramaChatterMachine[] m_machines;



        public static DRAMA_Chatter_Base CurrentState
        {
            get
            {
                return Current.m_local_machine.CurrentState;
            }
        }

        public DramaChatterMachine[] PlayerDramaMachines
        {
            get
            {
                return this.m_machines;
            }
        }



        private DramaChatterMachine m_local_machine;

        private bool IsSetup;



    }
}
