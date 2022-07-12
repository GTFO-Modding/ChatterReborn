using ChatterReborn.Managers;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ChatterReborn.Utils
{
    public static partial class CallBackUtils
    {
        public abstract class CallBackBase 
        {
            public abstract void Execute();


            public void RemoveCallBack()
            {
                CallBackManager.CallBacks.Remove(this);
            }

            public float ExecuteTimer { get; set; }
            public bool Started { get; set; }


            protected void Queue(DelayValue timer)
            {
                if (timer <= 0f)
                {
                    this.Execute();
                    return;
                }

                this.ExecuteTimer = Clock.Time + timer;
                this.Started = false;

                if (CallBackManager.CallBacks.Contains(this))
                {
                    CallBackManager.CallBacks.Remove(this);
                }

                CallBackManager.CallBacks.Add(this);
            }

        }
    }
}
