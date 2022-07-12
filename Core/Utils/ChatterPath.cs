using Il2CppSystem.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Utils
{
    public static class ChatterPath
    {
        public static string PluginPath
        {
            get
            {
                return BepInEx.Paths.PluginPath;
            }
        }

    }
}
