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
                m_musicType = "COMBAT";
            }
            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                m_regular_or_hidden = "REGULAR";
                MusicManager.Machine.Sound.SetSwitch(SWITCHES.COMBAT_REGULAR_OR_HIDDEN.GROUP, SWITCHES.COMBAT_REGULAR_OR_HIDDEN.SWITCH.REGULAR);
            }
            if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                m_regular_or_hidden = "HIDDEN";
                MusicManager.Machine.Sound.SetSwitch(SWITCHES.COMBAT_REGULAR_OR_HIDDEN.GROUP, SWITCHES.COMBAT_REGULAR_OR_HIDDEN.SWITCH.HIDDEN);
            }


            if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                m_combat_type = "COMMON";
                CellSound.SetState(STATES.COMBAT_TYPE.GROUP, STATES.COMBAT_TYPE.STATE.COMBAT_COMMON);
            }
            if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                m_combat_type = "BOSS";
                CellSound.SetState(STATES.COMBAT_TYPE.GROUP, STATES.COMBAT_TYPE.STATE.COMBAT_BOSS);
            }
        }


        private string m_musicType = "NONE";

        private string m_regular_or_hidden = "NONE";

        private string m_combat_type = "NONE";

        void OnGUI()
        {
            string content = "MUSIC: " + m_musicType + "\nCOMBAT_REGULAR_OR_HIDDEN : " + m_regular_or_hidden + "\nCOMBAT_TYPE : " + m_combat_type;
            GUI.Label(new Rect(250f,250f, 1000f,1000f), content, m_style);
        }

        void Start()
        {
            m_style = new GUIStyle();

            m_style.fontSize = 18;
            m_style.normal.textColor = Color.white;


        }

        private GUIStyle m_style;
    }
}
