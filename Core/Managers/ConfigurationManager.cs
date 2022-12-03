using ChatterReborn.Attributes;
using ChatterReborn.Data;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx.Configuration;

namespace ChatterReborn.Managers
{
    public class ConfigurationManager : ChatterManager<ConfigurationManager>
    {
        protected override void PostSetup()
        {
            ApplyConfigurations();
        }

        private static void ApplyConfigurations()
        {
            ConfigurationManager.Config = new ConfigFile(ConfigurationManager.CONFIG_PATH, true);
            var fields = typeof(ConfigurationManager).GetFields(BindingFlags.Static | BindingFlags.Public);
            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo fieldInfo = fields[i];
                var methodInfo = typeof(ConfigurationManager).GetMethod("GetConfigDefinition", BindingFlags.Static | BindingFlags.NonPublic);

                if (methodInfo != null)
                {
                    ConfigurationBaseAttribute custom_attribute = fieldInfo.GetCustomAttribute<ConfigurationBaseAttribute>();
                    if (custom_attribute != null && custom_attribute.ConfigurationType != ConfigurationType.None)
                    {
                        var genericMethod = methodInfo.MakeGenericMethod(custom_attribute.ValueType);
                        if (genericMethod != null)
                        {
                            List<object> args = new List<object>
                            {
                                fieldInfo,
                                custom_attribute.Title,
                                custom_attribute.Header,
                                custom_attribute.Desc
                            };
                            if (custom_attribute.ConfigurationType == ConfigurationType.Toggle)
                            {
                                args.Add(((ConfigurationToggleAttribute)custom_attribute).Default);
                            }
                            genericMethod.Invoke(null, args.ToArray());
                        }
                    }
                }
            }
        }

        private static void GetConfigDefinition<V>(FieldInfo fieldInfo, string title, string header, string desc, V defaultValue)
        {
            fieldInfo.SetValue(null, defaultValue);
            ConfigDefinition configDefinition = new ConfigDefinition(title, header);
            ConfigurationManager.Config.Bind<V>(configDefinition, defaultValue, new ConfigDescription(desc, null, null));
            ConfigEntry<V> configEntry;
            if (ConfigurationManager.Config.TryGetEntry<V>(configDefinition, out configEntry))
            {
                fieldInfo.SetValue(null, configEntry.Value);
            }
        }


        public static readonly string CONFIG_PATH = Path.Combine(BepInEx.Paths.ConfigPath, "ChatterReborn.cfg");

        //public static readonly string CONFIG_PATH = Path.Combine(Paths.ConfigPath, "ChatterRebornDev.cfg");

        public static ConfigFile Config;


        [ConfigurationToggle("Stealth Commands Shortcut", "When close to a hibernating sleeper, a prompt will appear at the buttom to announce stealth commands to your teammates..", true)]
        public static bool StealthCommandsEnabled;

        [ConfigurationToggle("Expedition Failed Death Scream", "Your character will release a death scream upon the expedition failed screen..", false)]
        public static bool ExpeditionFailedDeathScreamEnabled;

        [ConfigurationToggle("Resource Pack Received Confirmation", "Your character will say thank you to the person giving you a resource pack..", true)]
        public static bool ResourcePackRecievedConfirmationEnabled;

        [ConfigurationToggle("Stealth Kill Comments", "Your character will have stealth kill voice lines upon killing sleepers during sneaking..", true)]
        public static bool SneakKillCommentsEnabled;

        [ConfigurationToggle("Hacking Minigame dialogues", "Your character will have dialogue upon hacking security locks.", false)]
        public static bool HackingMiniGameCommentsEnabled;

        [ConfigurationToggle("Random Potential comments", "Your character will make periodic comments during exploration.", false)]
        public static bool ExplorationDialogueEnabled;


        [ConfigurationToggle("Resource Pickup Comment", "Your character will make comments upon picking up resource packs.", false)]
        public static bool ResourcePickUpCommentsEnabled;


        [ConfigurationToggle("Combat End Dialogue", "Your character will comment after a hectic battle with the monsters.", true)]
        public static bool EndOfCombatDialogueEnabled;


        [ConfigurationToggle("Combat Enter Dialogue", "Your character will make a battle cry upon entering combat.", true)]
        public static bool StartCombatDialogueEnabled;


        [ConfigurationToggle("Open Doors Dialogue", "Your character will have dialogue upon opening doors.", true)]
        public static bool OpenDoorsCommentEnabled;

        [ConfigurationToggle("Force Player in Team Scan", "If set to true and you are pointing/aiming at a player that's not in a team scan, your character will irritably shout at them. ", false)]
        public static bool IrritatedScanningCommentEnabled;

        [ConfigurationToggle("Fog Turbine Carrying Comment", "Your character will warn others to stay close when carrying a Fog turbine.", false)]
        public static bool HeavyFogRepellerCommmentEnabled;


        [ConfigurationToggle("Give Resource Packs Comment", "Your character tell the player you are trying to give resource packs to hold still.", true)]
        public static bool InteractionGiveResourcePacksCommentEnabled;


        [ConfigurationToggle("Intro Expedition Dialogue", "Your character will initiate a dialogue when starting an expedition.", true)]
        public static bool ExpeditionIntroDialogueEnabled;


        [ConfigurationToggle("BioTracker Information", "Your character will inform your teammates about the pings while using the Bio Tracker when ADS'ing.", true)]
        public static bool BioTrackerCommentsEnabled;


        [ConfigurationToggle("Consumable Depleted", "Your character will make a little comment when running out of consumables..", true)]
        public static bool ConsumableDepletedCommentsEnabled;

        public static bool ElevatorDropDialogueEnabled;

        [ConfigurationToggle("Health reminder", "Your character remind other players on your current health status.", true)]
        public static bool MedicCommentsEnabled;

        [ConfigurationToggle("Ammo reminder", "Your character remind other players on your current ammo status when switching weapons.", true)]
        public static bool AmmoCommentsEnabled;

        [ConfigurationToggle("Infection reminder", "Your character will cough and sneeze depending your infection level.", false)]
        public static bool InfectionCommentsEnabled;

        [ConfigurationToggle("Incoming monster group", "Your character will remind others that there are still monsters aggroed even after the combat music stops.", true)]
        public static bool HearHunterGroupDialogueEnabled;

        [ConfigurationToggle("Bioscan dialogues", "Your character will have dialogues regarding holo-paths, scan completions, and scanning progresses if set to true.", true)]
        public static bool ChainedPuzzleDialoguesEnabled;

        [ConfigurationToggle("Killed last monster", "Your character will make a quick comment after killing the last monster..", false)]
        public static bool KilledSingleMonsterCommentEnabled;

        [ConfigurationToggle("Let their be Light in the darkness", "Your character will make a comment when another teammate , whose close to you, turns on a long range flashlight.", true)]
        public static bool FlashLightInDarknessDialogueEnabled;

        [ConfigurationToggle("Key Pick Up Dialogue", "Your character will inform others on what keycard you picked up.", false)]
        public static bool KeyCardPickUpDialoguesEnabled;

        [ConfigurationToggle("Spitter Irritated Noises", "Your character will have a 75% chance to make irritated noises if a spitter explodes on you.", true)]
        public static bool OnExplodeSpitterCommentsEnabled;

        [ConfigurationToggle("Monster Light Sensitivity Warning", "Your character will warn any players flashing any monsters.", false)]
        public static bool FlashLightSensitivityDialoguesEnabled;

        [ConfigurationToggle("Better Voice Intensity", "Your character's voice intensity will adapt to the current heat level of the combat.", true)]
        public static bool VoiceIntensityAdapterEnabled;

        [ConfigurationToggle("Door Pings", "Your character will have pinging voice lines for doors. If a horde is banging on a door, ping the door and your character will warn others...", true)]
        public static bool PingDoorDialoguesEnabled;

        [ConfigurationToggle("Reviving Comments", "Your character thank the person reviving you.", true)]
        public static bool RevivedCommentEnabled;

        [ConfigurationToggle("Group Up Combat", "Your character will tell others to come group up when far away and in combat", false)]
        public static bool GroupStayCloseCombatDialoguesEnabled;

        [ConfigurationToggle("Include Bots", "If you are the host, the bots will also benefit from the mod.", true)]
        public static bool AllowBotsToParticipateEnabled;

        [ConfigurationToggle("Respond to coms", "Your character will respond back to the player using the com on you", true)]
        public static bool ComResponseEnabled;

        [ConfigurationToggle("Persistent Combat Chatter", "Your character will often banter during combat when shooting sleepers in your line of sight (The more aggroed monsters in AO, the more chance your character will trigger the banter).", true)]
        public static bool PersistentCombatChatterEnabled;

        [ConfigurationToggle("Friendly Fire Apology", "Your character will apologize upon accidentally shooting at your teammates.", false)]        
        public static bool FriendlyFireApologyEnabled;
    }
}
