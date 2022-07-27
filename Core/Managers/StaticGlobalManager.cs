using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Managers
{
    public class StaticGlobalManager : ChatterManager<StaticGlobalManager>
    {
        public static bool HeavyFogRepellerDialogEnabled { get; set; } = true;
        public static bool VoiceIntensityAdapterEnabled { get; set; } = true;
    }
}
