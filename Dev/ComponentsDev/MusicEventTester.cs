using ChatterReborn.Attributes;
using UnityEngine;

namespace ChatterRebornDev.ComponentsDev
{
    [IL2CPPType(AddComponentOnStart = true, DontDestroyOnLoad = true)]
    public class MusicEventTester : MonoBehaviour
    {


        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                MusicManager.Machine.Sound.SetSwitch(SWITCHES.MUSIC.GROUP, SWITCHES.MUSIC.SWITCH.COMBAT);
            }
            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                MusicManager.Machine.Sound.SetSwitch(SWITCHES.COMBAT_REGULAR_OR_HIDDEN.GROUP, SWITCHES.COMBAT_REGULAR_OR_HIDDEN.SWITCH.REGULAR);
            }
            if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                MusicManager.Machine.Sound.SetSwitch(SWITCHES.COMBAT_REGULAR_OR_HIDDEN.GROUP, SWITCHES.COMBAT_REGULAR_OR_HIDDEN.SWITCH.HIDDEN);
            }
        }
    }
}
