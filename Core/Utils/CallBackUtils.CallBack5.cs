using System;
using System.Collections.Generic;


namespace ChatterReborn.Utils
{
    public static partial class CallBackUtils
    {
        public class CallBack<T1, T2, T3, T4, T5> : CallBackBase<Action<T1, T2, T3, T4, T5>>
        {
            public CallBack(Action<T1, T2, T3, T4, T5> action) : base(action)
            {
            }

            public override void Execute()
            {
                this.m_action(par1, par2, par3, par4, par5);
            }

            public void QueueCallBack(DelayValue timer, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
            {
                par1 = p1;
                par2 = p2;
                par3 = p3;
                par4 = p4;
                par5 = p5;
                this.Queue(timer);
            }

            T1 par1;
            T2 par2;
            T3 par3;
            T4 par4;
            T5 par5;
        }





    }
}
