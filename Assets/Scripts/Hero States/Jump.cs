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
            //animator.SetBool("Jump", true);

            SurfaceLocation = ((HeroState)PreviousState).SurfaceLocation;
        }

        public override void Exit()
        {
            base.Exit();
            //animator.SetBool("Jump", false);
        }

        public override void FixedUpdate()
        {
            float jumpForce = (InputManager.LastJump < Hero.LargeJumpBound) ? Hero.SmallJumpForce : Hero.LargeJumpForce;

            //float jumpTime = Mathf.Min(InputManager.JumpTime / Hero.MaxJumpTime, 1.0f);

            //if (jumpTime < Hero.MediumJumpBound)
            //{
            //    jumpForce = Mathf.Lerp(Hero.SmallJumpForce, Hero.MediumJumpForce, (jumpTime / Hero.MediumJumpBound));
            //}
            //else if (jumpTime < Hero.LargeJumpBound)
            //{
            //    jumpForce = Mathf.Lerp(Hero.MediumJumpForce, Hero.LargeJumpForce, (jumpTime - Hero.MediumJumpBound) / (Hero.LargeJumpBound - Hero.MediumJumpBound));
            //}
            //else
            //{
            //    jumpForce = Hero.LargeJumpForce;
            //}

            rigidbody.velocity += jumpForce * SurfaceNormal;
        }

        #endregion
    }
}
