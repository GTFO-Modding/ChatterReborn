using ChatterReborn.Data;
using ChatterReborn.Utils;
using Player;
using System;

namespace ChatterReborn.Managers
{
    public abstract class ChatterManager<T> : IChatterManager where T : ChatterManager<T>
    {

        protected virtual void Setup()
        {
        }


        protected virtual void PostSetup()
        {
        }



        private void BasePostSetup()
        {
            DebugPrint("Post Setting up");
            ManagerInit.ManagerHandler.Managers.Add(this);
        }


        private void BaseSetup()
        {
            this.DebugPrint("New Manager Initialized", eLogType.Message);            
        }


        public virtual void OnLevelCleanUp()
        {

        }

        public virtual void OnResetSession()
        {

        }

        public virtual void OnElevatorArrived()
        {
        }

        public virtual void OnDropElevatorExit()
        {
        }

        public virtual void OnBuildDone()
        {
        }

        public virtual void OnStartExpedition()
        {
        }

        public virtual void OnDropInElevatorExit()
        {
        }




        public virtual void Update()
        {
        }

        public virtual void FixedUpdate()
        {
        }

        private bool m_firstSetup = false;


        private bool m_postSetup = false;
        public void Initialize(bool firstSetup = false)
        {
            if (firstSetup)
            {
                if (!m_firstSetup)
                {
                    m_patcher = new ChatterPatcher<T>(typeof(T).Name);
                    this.DebugPrint("Adding ChatterPatcher");
                    Setup();
                    BaseSetup();
                    m_firstSetup = true;
                }
                else
                {
                    this.DebugPrint("Has already been intialized!", eLogType.Warning);
                }
            }
            else
            {
                if (!m_postSetup)
                {
                    PostSetup();
                    BasePostSetup();
                    m_postSetup = true;
                }
                else
                {
                    this.DebugPrint("Has already been PostSetup!", eLogType.Warning);
                }
            }
        }

        public static T Current
        {
            get
            {
                if (m_manager == null)
                {
                    ChatterDebug.LogWarning("Manager " + typeof(T).Name + " has not been setup yet!");
                }

                return m_manager;
            }
        }

        protected ChatterPatcher<T> m_patcher;

        protected void DebugPrint(object message, eLogType logType = eLogType.Debug)
        {
            m_debugLogger.DebugPrint(message, logType);
        }

        public virtual void On_Registered_PlayerAgent(PlayerAgent playerAgent) { }

        public virtual void On_DeRegistered_PlayerAgent(PlayerAgent playerAgent) { }

        public virtual void On_Registered_LocalPlayerAgent(LocalPlayerAgent localPlayerAgent) { }

        public virtual void On_DeRegistered_LocalPlayerAgent(LocalPlayerAgent localPlayerAgent) { }

        public virtual void OnStartElevatorRide()
        {
        }

        public virtual void OnDropinElevatorExit()
        {
        }

        private static T m_manager = Activator.CreateInstance<T>();

        private DebugLoggerObject m_debugLogger = new DebugLoggerObject(typeof(T).Name);

    }
}
