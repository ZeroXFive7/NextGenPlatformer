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

        public WallRun(FSM fsm)
            : base(fsm)
        {
            // Add Transitions.
            AddTransition<Move>(() => { return InputManager.Grip < 0.25f || ((int)CollisionFlags & (int)CollisionFlags.Sides) == 0; });
            AddTransition<Jump>(() => { return distance > maxDistance; });
            AddTransition<Jump>(() => { return InputManager.Jump > 0.5f; });
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
            maxDistance = 5.0f;
            distance = 0.0f;
            prevPosition = transform.position;
        }

        public override void FixedUpdate()
        {
            distance += (transform.position - prevPosition).magnitude;
            prevPosition = transform.position;
            Debug.Log(distance);

            base.FixedUpdate();
            animator.SetFloat("Speed", speed);

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

            rigidbody.velocity = velocityDir * speed * Hero.MaxSpeed;

            Debug.DrawRay(transform.position, velocityDir, Color.magenta);
        }
    }
}
