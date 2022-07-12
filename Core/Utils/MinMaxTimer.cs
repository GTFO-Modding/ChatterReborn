using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Utils
{
    public struct MinMaxTimer
    {

        
        public MinMaxTimer(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public float min;
        public float max;
    }
}
