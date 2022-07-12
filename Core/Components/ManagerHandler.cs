using ChatterReborn.Attributes;
using ChatterReborn.Managers;
using Il2CppInterop.Runtime.Attributes;
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
        public void RegisterPlayerAgent(PlayerAgent playerAgent)
        {
            for (int i = 0; i < _managers.Count; i++)
            {
                _managers[i].On_Registered_PlayerAgent(playerAgent);
            }
        }

        [HideFromIl2Cpp]
        public void RegisterLocalPlayerAgent(LocalPlayerAgent localPlayerAgent)
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


        private List<IChatterManager> _managers;


        [HideFromIl2Cpp]
        public List<IChatterManager> Managers => _managers;

    }
}
