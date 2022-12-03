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
            if (ChatterEntrySettings.DebugLogsEnabled)
            {
                ChatterDebug.logger.LogInfo(msg);
            }
            
        }

        public static void LogDebug(object msg)
        {
            if (ChatterEntrySettings.DebugLogsEnabled)
                ChatterDebug.logger.LogDebug(msg);
        }

        public static void LogMessage(object msg)
        {
            if (ChatterEntrySettings.DebugLogsEnabled)
                ChatterDebug.logger.LogMessage(msg);
        }

        public static void LogError(object msg)
        {
            if (ChatterEntrySettings.DebugLogsEnabled)
                ChatterDebug.logger.LogError(msg);
        }

        public static void LogWarning(object msg)
        {
            if (ChatterEntrySettings.DebugLogsEnabled)
                ChatterDebug.logger.LogWarning(msg);
        }

        private static readonly ManualLogSource logger = new ManualLogSource(ChatterEntrySettings.Plugin_Name);

    }
}
