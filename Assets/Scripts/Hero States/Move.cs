using UnityEngine;
using UnityFSM;

namespace HeroStates
{
    class Move : HeroState
    {
        #region Public Methods

        public Move(FSM fsm)
            : base(fsm)
        {
            AddTransition<Idle>(IsIdle);
            AddTransition<Jump>(IsJumping);
            AddTransition<ControlledFall>(IsNotCollidingBelow);
            AddTransition<WallRun>(CanWallRun);
        }

        public override void Enter()
        {
            base.Enter();
            SurfaceLocation = UnityEngine.CollisionFlags.Below;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            Debug.DrawRay(transform.position, SurfaceNormal, Color.yellow);

            Vector3 cameraForward = MathHelper.ProjectVectorToPlane(Hero.Camera.forward, SurfaceNormal).normalized;
            Vector3 forwardProjection = MathHelper.ProjectVectorToPlane(Vector3.forward, SurfaceNormal).normalized;

            Vector3 movementForward = (Quaternion.FromToRotation(forwardProjection, cameraForward) * InputManager.MovementInput).normalized;
            movementForward = MathHelper.ProjectVectorToPlane(movementForward, SurfaceNormal).normalized;

            float speed = InputManager.MovementInput.sqrMagnitude;
            rigidbody.velocity = movementForward * speed * Hero.MaxSpeed;
            animator.SetFloat("Speed", speed);

            float rotationAngle = Vector3.Angle(forwardProjection, movementForward);
            rotationAngle *= (Vector3.Cross(forwardProjection, movementForward).y <= 0.0f) ? -1.0f : 1.0f;

            transform.rotation = Quaternion.Euler(0.0f, rotationAngle, 0.0f);
        }

        #endregion

        #region Transitions

        private bool IsMoving()
        {
            return Input.GetAxis("Horizontal Movement") != 0.0f || Input.GetAxis("Vertical Movement") != 0.0f;
        }

        private bool IsIdle()
        {
            return !IsMoving();
        }

        private bool IsJumping()
        {
            return InputManager.Jump > 0.5f;
        }

        private bool IsCollidingBelow()
        {
            return ((int)CollisionFlags & (int)CollisionFlags.Below) != 0;
        }

        private bool IsNotCollidingBelow()
        {
            return !IsCollidingBelow();
        }

        private bool IsCollidingSides()
        {
            return ((int)CollisionFlags & (int)CollisionFlags.Sides) != 0;
        }

        private bool CanWallRun()
        {
            return IsCollidingSides() && 
                InputManager.Grip >= 0.25f && 
                MathHelper.ProjectVectorToPlane(rigidbody.velocity, SurfaceNormal).sqrMagnitude > Hero.MinWallRunSpeed * Hero.MinWallRunSpeed;
        }

        #endregion
    }
}
