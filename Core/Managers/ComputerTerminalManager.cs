using ChatterReborn.Attributes;
using ChatterReborn.Utils;
using LevelGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Managers
{
    public class ComputerTerminalManager : ChatterManager<ComputerTerminalManager>
    {
        public static bool AnyTerminalsPlayingAudio
        {
            get
            {
                if (LG_ComputerTerminalManager.Current == null)
                {
                    return false;
                }
                if (LG_ComputerTerminalManager.Current.m_terminals == null || LG_ComputerTerminalManager.Current.m_terminals.Count <= 0)
                {
                    return false;
                }
                foreach (var termpair in LG_ComputerTerminalManager.Current.m_terminals)
                {
                    if (termpair != null && termpair.Value != null && termpair.Value.CurrentStateName == TERM_State.DoPlayAudioFile)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}
