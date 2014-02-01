using UnityEngine;
using UnityFSM;

namespace HeroStates
{
    class ControlledFall : HeroState
    {
        #region Fields

        private Vector3 yVelocity;
        private Vector3 xzVelocity;

        private float maxSpeed;

        #endregion

        #region Public Methods

        public ControlledFall(FSM fsm)
            : base(fsm)
        {
            AddTransition<MovementStage1>(IsCollidingBelow);
            AddTransition<WallRun>(CanWallRun);
            AddTransition<Respawn>(() => { return transform.position.y < -5.0f; });
        }

        public override void Enter()
        {
            base.Enter();
            yVelocity = new Vector3(0.0f, rigidbody.velocity.y, 0.0f);
            xzVelocity = MathHelper.ProjectVectorToPlane(rigidbody.velocity, Vector3.up);

            SurfaceLocation = CollisionFlags.Below;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            Vector3 movementDirection = DirectionRelativeToBasisOnSurface(InputManager.MovementInput, Hero.Camera.forward, SurfaceNormal);
            Vector3 globalForward = DirectionRelativeToBasisOnSurface(Vector3.forward, Vector3.forward, SurfaceNormal);

            xzVelocity += movementDirection * InputManager.MovementInput.sqrMagnitude * Hero.MaxAirSpeed * Time.deltaTime;
            if (xzVelocity.sqrMagnitude > Speed * Speed)
            {
                xzVelocity = xzVelocity.normalized * Speed;
            }

            yVelocity -= Hero.Gravity * Vector3.up * Time.deltaTime;

            rigidbody.velocity = yVelocity + xzVelocity;

            Quaternion rotation = Quaternion.FromToRotation(globalForward, movementDirection);
            Quaternion targetRotation = Quaternion.Euler(0.0f, rotation.eulerAngles.y, 0.0f);
            transform.localRotation = targetRotation;

            animator.SetFloat("Speed", 0.0f);
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
