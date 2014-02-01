using UnityEngine;
using UnityFSM;

namespace HeroStates
{
    public class MovementStage : HeroState
    {
        protected float maxAnimationSpeed;
        protected float maxSpeed;
        protected float rotationRate;

        public MovementStage(FSM fsm)
            : base(fsm)
        {
            // Add Transitions.
            AddTransition<Idle>(IsIdle);
            AddTransition<Jump>(() => { return InputManager.JumpReleased; });
            AddTransition<ControlledFall>(IsNotCollidingBelow);
            AddTransition<WallRun>(CanWallRun);
        }

        public override void Enter()
        {
            base.Enter();
            SurfaceLocation = UnityEngine.CollisionFlags.Below;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            // Compute target movement.
            Vector3 movementDirection = DirectionRelativeToBasisOnSurface(InputManager.MovementInput, Hero.Camera.forward, SurfaceNormal);
            Vector3 globalForward = DirectionRelativeToBasisOnSurface(Vector3.forward, Vector3.forward, SurfaceNormal);

            // Update momentum.
            float inputMagnitude = InputManager.MovementInput.sqrMagnitude;
            float dot = Vector3.Dot(MathHelper.ProjectVectorToPlane(transform.forward, SurfaceNormal), movementDirection);
            UpdateMomentum(dot, inputMagnitude);

            // Update rotation.
            Quaternion rotation = Quaternion.FromToRotation(globalForward, movementDirection);
            Quaternion targetRotation = Quaternion.Euler(0.0f, rotation.eulerAngles.y, 0.0f);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * rotationRate);

            // Update velocity.
            Speed = inputMagnitude * maxSpeed;
            rigidbody.velocity = MathHelper.ProjectVectorToPlane(transform.forward, SurfaceNormal).normalized * Speed;

            // Update animation.
            animator.SetFloat("Speed", inputMagnitude * maxAnimationSpeed);

            float angle = Vector3.Angle(transform.forward, movementDirection);
            float direction = angle * (Vector3.Cross(movementDirection, transform.forward).y >= 0.0f ? -1.0f : 1.0f) / 180.0f;
            animator.SetFloat("Direction", direction);
        }

        protected virtual void UpdateMomentum(float directionDeltaDotProduct, float inputMagnitude)
        {
            if (directionDeltaDotProduct > 0.5f)
            {
                Momentum += inputMagnitude * Time.deltaTime * directionDeltaDotProduct;
            }
            else if (directionDeltaDotProduct < -0.5f)
            {
                Momentum = 0.0f;
            }
        }
    }
}
