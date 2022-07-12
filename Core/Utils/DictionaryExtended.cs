using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Utils
{
    public class DictionaryExtended<T1, T2> : Dictionary<T1, T2>
    {
        public void Set(T1 key, T2 value)
        {
            if (!this.ContainsKey(key))
            {
                this.Add(key, value);
                return;
            }

            this[key] = value;
        }
    }
}
