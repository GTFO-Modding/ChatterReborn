namespace ChatterReborn.Managers
{
    public class StaticGlobalManager : ChatterManager<StaticGlobalManager>
    {
        public static bool HeavyFogRepellerDialogEnabled { get; set; } = true;
        public static bool VoiceIntensityAdapterEnabled { get; set; } = true;
    }
}
