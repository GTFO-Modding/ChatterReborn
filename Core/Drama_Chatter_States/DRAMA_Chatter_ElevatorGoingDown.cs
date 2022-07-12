using ChatterReborn.Managers;
using ChatterReborn.Utils;
using GameData;
using Player;
using UnityEngine;

namespace ChatterReborn.Drama_Chatter_States
{
    public class DRAMA_Chatter_ElevatorGoingDown : DRAMA_Chatter_Base
    {

        public override void Setup()
        {
            this.m_elevator_drop = new CallBackUtils.CallBack(this.OnElevatorDrop);
            base.Setup();
        }


        public override void Enter()
        {
            this.CommonEnter();
        }

        public override void SyncEnter()
        {
            this.CommonEnter();
        }

        private void CommonEnter()
        {
            this.m_elevator_drop.QueueCallBack(UnityEngine.Random.Range(this.m_elevator_comment.x, this.m_elevator_comment.y));
        }

        public override void OnDestroyed()
        {
            this.m_elevator_drop.RemoveCallBack();
            base.OnDestroyed();
        }

        public override void OnLevelCleanUp()
        {
            this.m_elevator_drop.RemoveCallBack();
            base.OnLevelCleanUp();
        }

        private void OnElevatorDrop()
        {
            PlayerAgent playerAgent = PlayerManager.GetLocalPlayerAgent();
            if (playerAgent != null && playerAgent.Alive && ConfigurationManager.ElevatorDropDialogueEnabled)
            {
                PlayerDialogManager.WantToStartDialog(GD.PlayerDialog.just_before_elevator_drop, playerAgent);
            }

        }


        private Vector2 m_elevator_comment = new Vector2(0.5f, 1.25f);

        private CallBackUtils.CallBack m_elevator_drop;
    }
}
