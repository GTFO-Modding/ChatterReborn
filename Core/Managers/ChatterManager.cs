using ChatterReborn.Data;
using ChatterReborn.Patches;
using ChatterReborn.Utils;
using LevelGeneration;
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
            Patch_GlobalMessage.OnCleanUps += this.OnLevelCleanup;
            this.DebugPrint("Patching LevelCleanUps");

            Patch_GlobalMessage.OnResetSessions += this.OnResetSession;
            this.DebugPrint("Patching OnResetSessions");

            ElevatorRide.add_OnElevatorHasArrived((Action)this.OnElevatorArrived);

            this.DebugPrint("Adding ActionCallback add_OnElevatorHasArrived");

            Patch_ElevatorRide.OnDropinElevatorExit += this.OnDropElevatorExit;

            this.DebugPrint("Adding ActionCallback OnDropinElevatorExit");

            LG_Factory.add_OnFactoryBuildDone(new Action(this.OnBuildDone));
            this.DebugPrint("Adding ActionCallback OnFactoryBuildDone delegate to");

            Patch_ElevatorRide.OnStartElevatorRide += this.OnStartElevatorRide;
            this.DebugPrint("Adding ActionCallback OnStartElevatorRide delegate to");

        }

        protected virtual void OnElevatorArrived()
        {
        }

        protected virtual void OnDropElevatorExit()
        {
        }

        protected virtual void OnBuildDone()
        {
        }

        protected virtual void OnStartElevatorRide()
        {
        }


        protected virtual void OnLevelCleanup()
        {

        }

        protected virtual void OnResetSession()
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

        protected void DebugPrint(object message, eLogType logType = eLogType.Debug)
        {
            m_debugLogger.DebugPrint(message, logType);
        }

        public virtual void On_Registered_PlayerAgent(PlayerAgent playerAgent) { }

        public virtual void On_DeRegistered_PlayerAgent(PlayerAgent playerAgent) { }

        public virtual void On_Registered_LocalPlayerAgent(LocalPlayerAgent localPlayerAgent) { }

        public virtual void On_DeRegistered_LocalPlayerAgent(LocalPlayerAgent localPlayerAgent) { }

        private static T m_manager = Activator.CreateInstance<T>();

        private DebugLoggerObject m_debugLogger = new DebugLoggerObject(typeof(T).Name);

    }
}
