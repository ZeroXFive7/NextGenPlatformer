using UnityEngine;
using UnityFSM;

namespace HeroStates
{
    class Jump : HeroState
    {
        #region Public Methods

        public Jump(FSM fsm)
            : base(fsm)
        {
            AddTransition<ControlledFall>(() => { return true; });
        }

        public override void Enter()
        {
            base.Enter();
            animator.SetBool("Jump", true);

            SurfaceLocation = ((HeroState)PreviousState).SurfaceLocation;
        }

        public override void Exit()
        {
            base.Exit();
            animator.SetBool("Jump", false);
        }

        public override void FixedUpdate()
        {
            float jumpForce = (InputManager.LastJump < Hero.LargeJumpBound) ? Hero.SmallJumpForce : Hero.LargeJumpForce;
            rigidbody.velocity += jumpForce * SurfaceNormal;
        }

        #endregion
    }
}
