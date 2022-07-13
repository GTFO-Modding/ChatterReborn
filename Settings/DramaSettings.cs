using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterRebornSettings
{
    public static class DramaSettings
    {
        public const bool timePassedEnabled = true;

        public const float enemyScoreNormal = 8f;

        public const float enemyScoreForCombatIntensity = 40f;

        /// <summary>
        /// This will allow other people who don't have the mod to participate.
        /// </summary>
        public const bool AllowParticipation = false;
        /// <summary>
        /// Set the debug for the DramaChatterMachine
        /// </summary>
        public const bool MachineDebugEnabled = false;
    }
}
