using UnityEngine;
using UnityFSM;

namespace HeroStates
{
    class Jump : HeroState
    {
        private float jumpMagnitude;
        private float jumpRemaining;

        private Vector3 initVelocity;

        #region Public Methods

        public Jump(FSM fsm)
            : base(fsm)
        {
            AddTransition<JumpFall>(() => { return InputManager.JumpReleased || jumpRemaining < 0.0f; });
        }

        public override void Enter()
        {
            base.Enter();
            animator.SetBool("Jump", true);

            jumpMagnitude = 0.0f;
            jumpRemaining = Hero.MaxJumpTime;
            initVelocity = rigidbody.velocity;
            SurfaceLocation = ((HeroState)PreviousState).SurfaceLocation;
        }

        public override void Exit()
        {
            base.Exit();
            animator.SetBool("Jump", false);
        }

        public override void FixedUpdate()
        {
            jumpRemaining -= Time.deltaTime;

            if (jumpMagnitude == 0.0f)
            {
                jumpMagnitude = Hero.JumpHeight;
            }
            else
            {
                jumpMagnitude += (Time.deltaTime / Hero.MaxJumpTime) * (jumpRemaining / Hero.MaxJumpTime) * Hero.JumpHeight;
            }

            rigidbody.velocity = initVelocity + jumpMagnitude * SurfaceNormal;
        }

        #endregion
    }
}
