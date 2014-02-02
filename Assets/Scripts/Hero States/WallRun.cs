using UnityEngine;
using UnityFSM;

namespace HeroStates
{
    public class WallRun : HeroState
    {
        private Vector3 initVelocityDir;
        private Vector3 velocityDir;
        private Vector3 surfaceForward;
        private Vector3 surfaceRight;
        private float speed;
        private float maxAngle;

        private float maxDistance;
        private Vector3 prevPosition;
        private float distance;

        private Vector3 gravityDir;

        public WallRun(FSM fsm)
            : base(fsm)
        {
            // Add Transitions.
            AddTransition<MovementStage1>(() => { return Detached() && Momentum < Hero.Stage2MinMomentum; });
            AddTransition<MovementStage2>(() => { return Detached() && Momentum < Hero.Stage3MinMomentum; });
            AddTransition<Jump>(() => { return InputManager.JumpPressed; });
            AddTransition<UncontrolledFall>(() => { return InputManager.Grip == 0.0f; });
        }

        public override void Enter()
        {
            base.Enter();
            SurfaceLocation = UnityEngine.CollisionFlags.Sides;

            surfaceForward = MathHelper.ProjectVectorToPlane(Vector3.up, SurfaceNormal).normalized;
            surfaceRight = Vector3.Cross(SurfaceNormal, surfaceForward);

            Quaternion rotation = Quaternion.FromToRotation((PreviousState as HeroState).SurfaceNormal, SurfaceNormal);
            initVelocityDir = rotation * transform.forward;
            velocityDir = initVelocityDir;

            speed = InputManager.MovementInput.sqrMagnitude;
            maxAngle = 30.0f;
            maxDistance = 20.0f;
            distance = 0.0f;
            prevPosition = transform.position;

            gravityDir = MathHelper.ProjectVectorToPlane(-Vector3.up, SurfaceNormal);
        }

        public override void FixedUpdate()
        {
            distance += (transform.position - prevPosition).magnitude;
            prevPosition = transform.position;
            
            base.FixedUpdate();
            animator.SetFloat("Speed", speed);

            if (distance > maxDistance)
            {
                rigidbody.velocity += gravityDir * 0.005f * Hero.Gravity;
                return;
            }

            if (InputManager.MovementInput.sqrMagnitude > 0.1f)
            {
                Vector3 inputSurfaceSpace = InputManager.MovementInput.x * surfaceRight + InputManager.MovementInput.z * surfaceForward.normalized;
                float angle = Vector3.Angle(velocityDir, inputSurfaceSpace);
                if (angle > maxAngle)
                {
                    float scalar = Vector3.Cross(initVelocityDir, inputSurfaceSpace).y > 0.0f ? -1.0f : 1.0f;
                    inputSurfaceSpace = Quaternion.AngleAxis(scalar * maxAngle, SurfaceNormal) * initVelocityDir;
                }

                Debug.DrawRay(transform.position, inputSurfaceSpace, Color.blue);
                velocityDir = Vector3.Lerp(velocityDir, inputSurfaceSpace, Time.deltaTime).normalized;
            }

            rigidbody.velocity = velocityDir * Speed;

            Debug.DrawRay(transform.position, velocityDir, Color.magenta);
        }

        private bool Detached()
        {
            return InputManager.Grip < 0.25f || ((int)CollisionFlags & (int)CollisionFlags.Sides) == 0;
        }
    }
}
