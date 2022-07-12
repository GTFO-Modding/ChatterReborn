using System;
using System.Collections.Generic;


namespace ChatterReborn.Utils
{
    public static partial class CallBackUtils
    {
        public class CallBack<T1, T2, T3> : CallBackBase<Action<T1, T2, T3>>
        {
            public CallBack(Action<T1, T2, T3> action) : base(action)
            {
            }

            public override void Execute()
            {
                this.m_action(par1, par2, par3);
            }


            
            public void QueueCallBack(DelayValue timer, T1 p1, T2 p2, T3 p3)
            {
                par1 = p1;
                par2 = p2;
                par3 = p3;
                this.Queue(timer);

            }

            T1 par1;
            T2 par2;
            T3 par3;
        }
    }
}
