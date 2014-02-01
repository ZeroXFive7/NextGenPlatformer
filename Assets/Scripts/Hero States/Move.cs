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
            AddTransition<Jump>(() => { return InputManager.JumpReleased; });
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
            // Run animation for sprint: long leaping steps.

            //If player flicks joystick in a direction they will get a running start in that direction.
            // Otherwise they will accelerate more slowly towards target velocity.

            base.FixedUpdate();

            // Calculate New Movement Direction.
            Vector3 cameraForward = MathHelper.ProjectVectorToPlane(Hero.Camera.forward, SurfaceNormal).normalized;
            Vector3 forwardProjection = MathHelper.ProjectVectorToPlane(Vector3.forward, SurfaceNormal).normalized;
            
            Vector3 targetForward = (Quaternion.FromToRotation(forwardProjection, cameraForward) * InputManager.MovementInput).normalized;
            targetForward = MathHelper.ProjectVectorToPlane(targetForward, SurfaceNormal).normalized;

            // Calculate New Rotation.
            float rotationAngle = Vector3.Angle(forwardProjection, targetForward);
            rotationAngle *= (Vector3.Cross(forwardProjection, targetForward).y <= 0.0f) ? -1.0f : 1.0f;

            Momentum = Mathf.Max(Momentum - 5.0f * Time.deltaTime * InputManager.Grip, 0.0f);
            Momentum = Mathf.Min(Momentum + Time.deltaTime * InputManager.MovementInput.sqrMagnitude, 5.0f);

            // Update.
            rigidbody.velocity = targetForward * Momentum * Hero.MaxSpeed;
            animator.SetFloat("Speed", InputManager.MovementInput.sqrMagnitude);
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
