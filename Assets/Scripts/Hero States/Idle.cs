using UnityEngine;
using UnityFSM;

namespace HeroStates
{
    public class Idle : HeroState
    {
        private float initialMomentum;
        private Vector3 initialNormalVelocity;

        public Idle(FSM fsm)
            : base(fsm)
        {
            AddTransition<MovementStage1>(IsMoving);
            AddTransition<Jump>(() => { return InputManager.JumpReleased; });
        }

        public override void Enter()
        {
            base.Enter();

            SurfaceLocation = UnityEngine.CollisionFlags.Below;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            Momentum = Mathf.Max(Momentum - 5.0f * Time.deltaTime, 0.0f);
            rigidbody.velocity = Vector3.zero;

            animator.SetFloat("Speed", 0.0f);
        }

        #region Transitions

        private bool IsMoving()
        {
            return Input.GetAxis("Horizontal Movement") != 0.0f || Input.GetAxis("Vertical Movement") != 0.0f;
        }

        #endregion
    }
}
