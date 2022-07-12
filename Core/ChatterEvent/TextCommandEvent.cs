using SNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.ChatterEvent
{
    public struct TextCommandEvent
    {
        public SNet_Player source;
        public uint textId;
        public SNet_Player destination;
    }
}
