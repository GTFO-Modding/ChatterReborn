using ChatterReborn.Utils;
using System.Collections.Generic;

namespace ChatterReborn.Managers
{
    public class CallBackManager : ChatterManager<CallBackManager>
    {
        protected override void Setup()
        {
            this.m_callbacks = new List<CallBackUtils.CallBackBase>();
        }

        public override void OnLevelCleanUp()
        {
            this.m_callbacks.Clear();
        }

        public override void Update()
        {
            for (int i = m_callbacks.Count - 1; i >= 0; i--)
            {
                CallBackUtils.CallBackBase callback = this.m_callbacks[i];
                if (!callback.Started)
                {
                    callback.Started = true;
                    continue;
                }

                if (callback.ExecuteTimer < Clock.Time)
                {
                    m_callbacks.Remove(callback);
                    callback.Execute();
                }
            }
        }

        public static List<CallBackUtils.CallBackBase> CallBacks
        {
            get
            {
                return Current.m_callbacks;
            }
        }


        private List<CallBackUtils.CallBackBase> m_callbacks;
    }
}
