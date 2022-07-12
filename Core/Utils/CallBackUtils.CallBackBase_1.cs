using System;

namespace ChatterReborn.Utils
{
    public static partial class CallBackUtils
    {
        public abstract class CallBackBase<T> : CallBackBase
        {
            protected T m_action;

            public CallBackBase(T action)
            {
                this.m_action = action;
            }
        }
    }
}
