using UnityEngine;
using UnityFSM;

namespace HeroStates
{
    public class WallRun : HeroState
    {
        private Vector3 velocityDir;
        private Vector3 surfaceForward;
        private Vector3 surfaceRight;
        private float speed;
        private float angle;

        public WallRun(FSM fsm)
            : base(fsm)
        {
            // Add Transitions.
            AddTransition<Move>(() => { return InputManager.Grip < 0.25f || ((int)CollisionFlags & (int)CollisionFlags.Sides) == 0; });
            AddTransition<Jump>(() => { return InputManager.Jump > 0.5f; });
        }

        public override void Enter()
        {
            base.Enter();
            SurfaceLocation = UnityEngine.CollisionFlags.Sides;

            surfaceForward = MathHelper.ProjectVectorToPlane(Vector3.up, SurfaceNormal).normalized;
            surfaceRight = Vector3.Cross(SurfaceNormal, surfaceForward);

            Quaternion rotation = Quaternion.FromToRotation((PreviousState as HeroState).SurfaceNormal, SurfaceNormal);
            velocityDir = rotation * transform.forward;

            speed = InputManager.MovementInput.sqrMagnitude;

            //if (Vector3.Dot(velocityDir, surfaceForward) > Vector3.Dot(velocityDir, surfaceRight))
            //{
            //    velocityDir = surfaceForward;
            //}
            //else
            //{
            //    velocityDir = surfaceRight;
            //}
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            animator.SetFloat("Speed", speed);

            if (InputManager.MovementInput.sqrMagnitude > 0.1f)
            {
                Vector3 inputSurfaceSpace = InputManager.MovementInput.x * surfaceRight + InputManager.MovementInput.z * surfaceForward.normalized;
                Debug.DrawRay(transform.position, inputSurfaceSpace, Color.blue);
                velocityDir = Vector3.Lerp(velocityDir, inputSurfaceSpace, Time.deltaTime).normalized;
            }

            rigidbody.velocity = velocityDir * speed * Hero.MaxSpeed;

            Debug.DrawRay(transform.position, velocityDir, Color.magenta);
        }
    }
}
