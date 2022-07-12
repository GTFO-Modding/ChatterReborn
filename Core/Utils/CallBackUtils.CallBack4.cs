using System;
using System.Collections.Generic;


namespace ChatterReborn.Utils
{
    public static partial class CallBackUtils
    {
        public class CallBack<T1, T2, T3, T4> : CallBackBase<Action<T1, T2, T3, T4>>
        {
            public CallBack(Action<T1, T2, T3, T4> action) : base(action)
            {
            }

            public override void Execute()
            {
                this.m_action(par1, par2, par3, par4);
            }

            public void QueueCallBack(DelayValue timer, T1 p1, T2 p2, T3 p3, T4 p4)
            {
                par1 = p1;
                par2 = p2;
                par3 = p3;
                par4 = p4;
                this.Queue(timer);
            }


            T1 par1;
            T2 par2;
            T3 par3;
            T4 par4;
        }





    }
}
