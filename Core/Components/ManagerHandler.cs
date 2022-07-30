using ChatterReborn.Attributes;
using ChatterReborn.Managers;
using ChatterReborn.Utils;
using Globals;
using Il2CppInterop.Runtime.Attributes;
using LevelGeneration;
using Player;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ChatterReborn.Components
{
    [IL2CPPType]
    public class ManagerHandler : MonoBehaviour
    {
        public ManagerHandler(IntPtr p) : base(p)
        {
        }

        void Awake()
        {
            _managers = new List<IChatterManager>();

            LG_Factory.OnFactoryBuildDone += (Action)OnBuildDone;
        }


        void Update()
        {
            for (int i = 0; i < _managers.Count; i++)
            {
                _managers[i].Update();
            }
        }

        void FixedUpdate()
        {
            for (int i = 0; i < _managers.Count; i++)
            {
                _managers[i].FixedUpdate();
            }
        }


        [HideFromIl2Cpp]
        public void OnRegisterPlayerAgent(PlayerAgent playerAgent)
        {
            for (int i = 0; i < _managers.Count; i++)
            {
                _managers[i].On_Registered_PlayerAgent(playerAgent);
            }
        }

        [HideFromIl2Cpp]
        public void OnStartElevatorRide()
        {
            for (int i = 0; i < _managers.Count; i++)
            {
                _managers[i].OnStartElevatorRide();
            }
        }

        [HideFromIl2Cpp]
        public void OnStartExpedition()
        {
            for (int i = 0; i < _managers.Count; i++)
            {
                _managers[i].OnStartExpedition();
            }
        }

        [HideFromIl2Cpp]
        public void OnDropInElevatorExit()
        {
            for (int i = 0; i < _managers.Count; i++)
            {
                _managers[i].OnDropInElevatorExit();
            }
        }

        [HideFromIl2Cpp]
        public void OnRegisterLocalPlayerAgent(LocalPlayerAgent localPlayerAgent)
        {
            for (int i = 0; i < _managers.Count; i++)
            {
                _managers[i].On_Registered_LocalPlayerAgent(localPlayerAgent);
            }
        }

        [HideFromIl2Cpp]
        public void DeRegisterPlayerAgent(PlayerAgent playerAgent)
        {
            for (int i = 0; i < _managers.Count; i++)
            {
                _managers[i].On_DeRegistered_PlayerAgent(playerAgent);
            }
        }

        [HideFromIl2Cpp]
        public void DeRegisterLocalPlayerAgent(LocalPlayerAgent localPlayerAgent)
        {
            for (int i = 0; i < _managers.Count; i++)
            {
                _managers[i].On_DeRegistered_LocalPlayerAgent(localPlayerAgent);
            }
        }

        [HideFromIl2Cpp]
        public void OnLevelCleanup()
        {
            for (int i = 0; i < _managers.Count; i++)
            {
                _managers[i].OnLevelCleanUp();
            }
        }
        [HideFromIl2Cpp]
        public void OnResetSession()
        {
            for (int i = 0; i < _managers.Count; i++)
            {
                _managers[i].OnResetSession();
            }
        }

        [HideFromIl2Cpp]
        public void OnElevatorArrived()
        {
            for (int i = 0; i < _managers.Count; i++)
            {
                _managers[i].OnElevatorArrived();
            }
        }

        [HideFromIl2Cpp]
        public void OnBuildDone()
        {
            for (int i = 0; i < _managers.Count; i++)
            {
                _managers[i].OnBuildDone();
            }
        }


        private List<IChatterManager> _managers;


        [HideFromIl2Cpp]
        public List<IChatterManager> Managers => _managers;

    }
}
