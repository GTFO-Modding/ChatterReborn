using System;


namespace ChatterReborn.Utils
{
    public static partial class CallBackUtils
    {
        public class CallBack : CallBackBase<Action>
        {

            public CallBack(Action action) : base(action)
            {
            }

            public override void Execute()
            {
                this.m_action();
            }


            public void QueueCallBack(DelayValue timer)
            {
                this.Queue(timer);
            }
        }
    }
}
