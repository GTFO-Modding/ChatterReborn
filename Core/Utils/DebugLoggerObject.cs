using ChatterReborn.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Utils
{
    public class DebugLoggerObject
    {
        private string _preffix;

        public DebugLoggerObject(string id)
        {
            _preffix = id;
        }

        public void DebugPrint(object o, eLogType eLogType = eLogType.Debug)
        {
            switch (eLogType)
            {
                case eLogType.None:
                    break;
                case eLogType.Debug:
                    ChatterDebug.LogDebug(_preffix + " - " + o);
                    break;
                case eLogType.Message:
                    ChatterDebug.LogMessage(_preffix + " - " + o);
                    break;
                case eLogType.Warning:
                    ChatterDebug.LogWarning(_preffix + " - " + o);
                    break;
                case eLogType.Error:
                    ChatterDebug.LogError(_preffix + " - " + o);
                    break;
                default:
                    break;
            }
        }
    }
}
