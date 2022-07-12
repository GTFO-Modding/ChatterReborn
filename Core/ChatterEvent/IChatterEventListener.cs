using ChatterReborn.ChatterEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.ChatterEvent
{
    public interface IChatterEventListener<T> where T : struct
    {
        void OnChatterEvent(T chatterEvent);
    }

}
