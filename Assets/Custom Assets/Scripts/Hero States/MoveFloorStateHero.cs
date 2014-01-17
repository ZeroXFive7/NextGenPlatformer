using UnityEngine;

namespace States
{
    class MoveFloorStateHero : HeroState
    {
        #region Fields

        #endregion

        #region Public Methods

        public MoveFloorStateHero(HeroMotion hero) : base(hero) { }

        public override void Enter()
        {
            Debug.Log("Entering " + Name);
        }

        public override void Exit()
        {
            Debug.Log("Exiting " + Name);
        }

        public override void Update()
        {
            UpdateSurfaceNormal(transform.position, -transform.up);

            Vector3 input = new Vector3(Input.GetAxis("Horizontal Movement"), 0.0f, Input.GetAxis("Vertical Movement"));

            Vector3 cameraForward = MathHelper.ProjectVectorToPlane(camera.forward, SurfaceNormal);
            cameraForward.Normalize();

            Debug.DrawRay(transform.position, cameraForward, Color.magenta);
            Vector3 movementForward = Quaternion.FromToRotation(Vector3.forward, cameraForward) * input;

            float speed = input.sqrMagnitude;
            rigidbody.velocity = movementForward * speed * hero.MaxSpeed;
        }

        #endregion
    }
}
