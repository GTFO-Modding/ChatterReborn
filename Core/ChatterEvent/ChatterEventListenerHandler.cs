using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.ChatterEvent
{
    public static class ChatterEventListenerHandler<T> where T : struct
    {
        public static void PostEvent(T chatterEvent)
        {
            m_listeners?.Invoke(chatterEvent);
        }

        public static void RegisterListener(IChatterEventListener<T> chatterEventListener)
        {
            m_listeners += chatterEventListener.OnChatterEvent;
        }

        public static void DeRegisterListener(IChatterEventListener<T> chatterEventListener)
        {
            m_listeners -= chatterEventListener.OnChatterEvent;
        }


        private static Action<T> m_listeners;
    }
}
