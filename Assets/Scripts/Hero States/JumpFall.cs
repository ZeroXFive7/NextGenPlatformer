using UnityEngine;
using UnityFSM;

namespace HeroStates
{
    public class JumpFall : Fall
    {
        #region Public Methods

        public JumpFall(FSM fsm)
            : base(fsm)
        {
            AddTransition<WallRun>(CanWallRun);
            this.gravity = Hero.JumpGravity;
            this.maxSpeed = Hero.MaxAirSpeed;
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        #endregion
    }
}
