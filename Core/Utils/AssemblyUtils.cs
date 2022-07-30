using ChatterRebornSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Utils
{
    public static class AssemblyUtils
    {

        public static bool LoadAssembly(string assemblyName, out Assembly assemblyLoaded)
        {
            assemblyLoaded = null;
            try
            {
                assemblyLoaded = Assembly.Load(assemblyName);
                if (assemblyLoaded != null)
                {
                    ChatterDebug.LogMessage("ChatterReborn loaded another ASM : " + assemblyName);
                    return true;
                }
            }
            catch (Exception e)
            {
                ChatterDebug.LogError("Could not load " + assemblyName + ": it does not exist! ERROR : " + e.Message);
            }
            return false;
        }


        public static bool DoesAssemblyExist(string stringLookUp)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.FullName.Contains(stringLookUp))
                {
                    return true;
                }
            }

            return false;
        }

        public static void LogAllAssemblies()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                ChatterDebug.LogMessage("Assembly : " + assembly.FullName);
            }

        }


        public static void LoadDevComponents()
        {

            if (!MiscSettings.LoadDevComponents)
            {
                return;
            }

            if (AssemblyUtils.LoadAssembly("ChatterRebornDev", out var assemblyLoaded))
            {
                CurrentAssemblies.AddRange(assemblyLoaded.GetTypes());
            }
        }


        public static List<Type> CurrentAssemblies { get; set; } = new List<Type>();
    }
}
