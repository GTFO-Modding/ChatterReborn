namespace ChatterRebornSettings
{

    /// <summary>
    ///  This is all for the chatter reborn version
    /// </summary>
    /// 

    public static class Settings
    {
        public static class Plugin
        {
            public const string Plugin_GUI = "CHTR";
            public const string Plugin_Name = "chatter-reborn";
            public const string Plugin_Version = "0.3.5";
            public const bool DebugLogsEnabled = false;
        }


        public static class ChatterMethodTokens
        {
            public const uint StartMainGame__Start__Postfix = 5508391;
            public const uint PlayerAgent__Setup__Postfix = 3463302;
            public const uint PlayerAgent__OnDespawn__Postfix = 8491382;
            public const uint ElevatorRide__StartElevatorRide__Postfix = 6349339;
            public const uint ElevatorRide__OnGSWantToStartExpedition__Postfix = 6401713;
            public const uint ElevatorRide__DropinElevatorExit__Postfix = 8333250;
            public const uint Global__OnLevelCleanup__Postfix = 4660874;
            public const uint Global__OnResetSession__Postfix = 1202949;
            public const uint Dam_EnemyDamageBase__BulletDamage__Postfix = 8294999;
            public const uint Dam_PlayerDamageLocal__ReceiveBulletDamage__Prefix = 2275379;
            public const uint PlayerVoice__VoiceCallback__Prefix = 768007;
            public const uint Dam_EnemyDamageBase__MeleeDamage__Postfix = 2959159;
            public const uint Dam_PlayerDamageBase__OnIncomingDamage__Postfix = 8279422;
            public const uint Dam_PlayerDamageBase__ReceiveTentacleAttackDamage__Postfix = 6876021;
            public const uint Dam_SyncedDamageBase__ShooterProjectileDamage__Postfix = 7270743;
            public const uint Dam_PlayerDamageBase__ReceiveMeleeDamage__Postfix = 7168294;
        }


        public static class Misc
        {
            public const bool Debug_SpecifigPingManager = false;

            public const bool Debug_DevToolLogger = false;

            public const bool DialogCastingDebugEnabled = false;

            public const bool AllowBondingDialogue = false;

            public const bool ElementsEnabled = false;

            public const bool LoadDevComponents = false;

            public const bool AllowOrderBioScan_Drama_State = false;
        }

        public static class PlayerBot
        {

            public const bool EnablePlayerMonitor = true;
        }

        public static class Drama
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
    
}
