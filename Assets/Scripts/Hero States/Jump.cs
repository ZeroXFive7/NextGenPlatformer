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
            AddTransition<ControlledFall>(JumpEnded);
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
            base.FixedUpdate();
            rigidbody.velocity += Hero.JumpForce * SurfaceNormal;
        }

        #endregion

        #region Transitions

        private bool OneSecondElapsed()
        {
            return ActiveTime > 0.1f;
        }

        private bool JumpEnded()
        {
            return IsNotJumping() || OneSecondElapsed();
        }

        private bool IsJumping()
        {
            return InputManager.Jump > 0.5f;
        }

        private bool IsNotJumping()
        {
            return !IsJumping();
        }

        #endregion
    }
}
