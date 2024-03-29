﻿using Player;

namespace ChatterReborn.Managers
{
    public interface IChatterManager
    {
        void Initialize(bool firstSetup);
        void Update();
        void FixedUpdate();

        void On_Registered_PlayerAgent(PlayerAgent playerAgent);
        void On_DeRegistered_PlayerAgent(PlayerAgent playerAgent);

        void On_Registered_LocalPlayerAgent(LocalPlayerAgent localPlayerAgent);
        void On_DeRegistered_LocalPlayerAgent(LocalPlayerAgent localPlayerAgent);

        void OnLevelCleanUp();
        void OnResetSession();

        void OnGUI();
        void OnStartElevatorRide();
        void OnElevatorArrived();
        void OnDropinElevatorExit();
        void OnStartExpedition();
        void OnDropInElevatorExit();
        void OnBuildDone();

    }
}