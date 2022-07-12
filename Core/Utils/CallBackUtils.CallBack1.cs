using System;
using System.Collections.Generic;
using UnityEngine;

namespace ChatterReborn.Utils
{
    public static partial class CallBackUtils
    {
        public class CallBack<T1> : CallBackBase<Action<T1>>
        {
            public CallBack(Action<T1> action) : base(action)
            {
            }
            

            public override void Execute()
            {
                this.m_action(par1);
            }
            public void QueueCallBack(DelayValue timer, T1 p1)
            {
                this.par1 = p1;
                this.Queue(timer);
            }


            T1 par1;

        }





    }
}
