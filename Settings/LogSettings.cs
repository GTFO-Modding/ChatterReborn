using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterRebornSettings
{
    public static class LogSettings
    {
        public const bool Debug = All || false;
        public const bool Message = All || false;
        public const bool Verbose = All || false;
        public const bool Warning = All || false;
        public const bool Error = All || false;

        public const bool All = true;
    }
}
