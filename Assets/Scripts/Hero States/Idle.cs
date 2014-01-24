using UnityEngine;
using UnityFSM;

namespace HeroStates
{
    public class Idle : HeroState
    {
        public Idle(FSM fsm)
            : base(fsm)
        {
            AddTransition<Move>(IsMoving);
            AddTransition<Jump>(IsJumping);
        }

        public override void Enter()
        {
            base.Enter();
            SurfaceLocation = UnityEngine.CollisionFlags.Below;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            rigidbody.velocity = new Vector3(0.0f, rigidbody.velocity.y, 0.0f);

            animator.SetFloat("Speed", 0.0f);
        }

        #region Transitions

        private bool IsMoving()
        {
            return Input.GetAxis("Horizontal Movement") != 0.0f || Input.GetAxis("Vertical Movement") != 0.0f;
        }

        private bool IsJumping()
        {
            return InputManager.Jump > 0.5f;
        }

        #endregion
    }
}
