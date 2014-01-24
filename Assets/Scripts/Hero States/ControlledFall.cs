using UnityEngine;
using UnityFSM;

namespace HeroStates
{
    class ControlledFall : HeroState
    {
        #region Fields

        private float maxSpeedSqr;
        private Vector3 yVelocity;
        private Vector3 xzVelocity;

        #endregion

        #region Public Methods

        public ControlledFall(FSM fsm)
            : base(fsm)
        {
            AddTransition<Move>(IsCollidingBelow);
            AddTransition<WallRun>(CanWallRun);
        }

        public override void Enter()
        {
            base.Enter();
            yVelocity = new Vector3(0.0f, rigidbody.velocity.y, 0.0f);
            xzVelocity = MathHelper.ProjectVectorToPlane(rigidbody.velocity, Vector3.up);

            maxSpeedSqr = Mathf.Max(xzVelocity.sqrMagnitude, Hero.MaxAirSpeed * Hero.MaxAirSpeed);

            SurfaceLocation = CollisionFlags.Below;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            Vector3 cameraForward = MathHelper.ProjectVectorToPlane(Hero.Camera.forward, Vector3.up);
            cameraForward.Normalize();

            Vector3 movementForward = Quaternion.FromToRotation(Vector3.forward, cameraForward) * InputManager.MovementInput;
            movementForward.Normalize();

            float speed = InputManager.MovementInput.sqrMagnitude;

            xzVelocity += movementForward * speed * Hero.AirSpeed * Time.deltaTime;
            if (xzVelocity.sqrMagnitude > maxSpeedSqr)
            {
                xzVelocity = xzVelocity.normalized * Hero.MaxAirSpeed;
            }

            yVelocity -= Hero.Gravity * Vector3.up * Time.deltaTime;

            rigidbody.velocity = yVelocity + xzVelocity;

            float rotationAngle = Vector3.Angle(Vector3.forward, movementForward);
            rotationAngle *= (Vector3.Cross(Vector3.forward, movementForward).y <= 0.0f) ? -1.0f : 1.0f;

            transform.rotation = Quaternion.Euler(0.0f, rotationAngle, 0.0f);

            animator.SetFloat("Speed", speed);
        }

        #endregion

        #region Transitions

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
