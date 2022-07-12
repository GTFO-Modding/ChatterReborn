using System;

namespace ChatterReborn.Utils
{
    public static partial class CallBackUtils
    {
        public class CallBack<T1, T2> : CallBackBase<Action<T1, T2>>
        {
            public CallBack(Action<T1, T2> action) : base(action)
            {
            }            

            public override void Execute()
            {
                this.m_action(par1, par2);
            }

            public void QueueCallBack(DelayValue timer, T1 p1, T2 p2)
            {
                par1 = p1;
                par2 = p2;
                this.Queue(timer);
            }


            T1 par1;
            T2 par2;


        }





    }
}
