using ChatterReborn.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChatterReborn.Managers
{
    public class GlobalEventManager : ChatterManager<GlobalEventManager>
    {
        public override void OnLevelCleanUp()
        {
            m_firstdoorOpened = false;
        }

        public static float LastDoorOpened
        {
            get
            {
                return Current.m_lastDoorOpened;
            }
            set
            {
                Current.m_lastDoorOpened = value;
                Current.m_firstdoorOpened = true;
            }
        }


        public static bool PrisonersLost => Current.m_firstdoorOpened && Time.time - Current.m_lastDoorOpened > 600f;

        private float m_lastDoorOpened;

        private bool m_firstdoorOpened;
    }
}
