using BepInEx.Logging;
using ChatterRebornSettings;

namespace ChatterReborn.Utils
{
    public static class ChatterDebug
    {
        static ChatterDebug()
        {
            Logger.Sources.Add(ChatterDebug.logger);
        }

        public static void Verbose(object msg)
        {
            if (LogSettings.Verbose)
            {
                ChatterDebug.logger.LogInfo(msg);
            }
            
        }

        public static void LogDebug(object msg)
        {
            if (LogSettings.Debug)
                ChatterDebug.logger.LogDebug(msg);
        }

        public static void LogMessage(object msg)
        {
            if (LogSettings.Message)
                ChatterDebug.logger.LogMessage(msg);
        }

        public static void LogError(object msg)
        {
            if (LogSettings.Error)
                ChatterDebug.logger.LogError(msg);
        }

        public static void LogWarning(object msg)
        {
            if (LogSettings.Warning)
                ChatterDebug.logger.LogWarning(msg);
        }

        private static readonly ManualLogSource logger = new ManualLogSource(ChatterEntrySettings.Plugin_Name);

    }
}
