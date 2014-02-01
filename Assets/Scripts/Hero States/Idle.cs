using UnityEngine;
using UnityFSM;

namespace HeroStates
{
    public class Idle : HeroState
    {
        float skidTime;

        private float initialMomentum;
        private Vector3 initialNormalVelocity;

        public Idle(FSM fsm)
            : base(fsm)
        {
            AddTransition<Move>(IsMoving);
            AddTransition<Jump>(() => { return InputManager.JumpReleased; });
        }

        public override void Enter()
        {
            base.Enter();

            SurfaceLocation = UnityEngine.CollisionFlags.Below;
            skidTime = 0.0f;
            initialMomentum = Momentum;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            skidTime = Mathf.Min(skidTime + Time.deltaTime, 1.0f);
            Momentum = Mathf.Lerp(initialMomentum, 0.0f, skidTime);

            rigidbody.velocity = rigidbody.velocity.normalized * Momentum;

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
