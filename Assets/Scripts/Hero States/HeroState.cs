using UnityEngine;
using UnityFSM;

namespace HeroStates
{
    public class HeroState : State
    {
        #region Properties and Fields

        public CollisionFlags CollisionFlags = CollisionFlags.None;

        private Vector3 surfaceNormalAbove;
        public Vector3 SurfaceNormalAbove
        {
            get
            {
                return surfaceNormalAbove;
            }
        }

        private Vector3 surfaceNormalBelow;
        public Vector3 SurfaceNormalBelow
        {
            get
            {
                return surfaceNormalBelow;
            }
        }

        private Vector3 surfaceNormalSides;
        public Vector3 SurfaceNormalSides
        {
            get
            {
                return surfaceNormalSides;
            }
        }

        public  CollisionFlags SurfaceLocation = CollisionFlags.None;

        public Vector3 SurfaceNormal
        {
            get
            {
                switch (SurfaceLocation)
                {
                    case UnityEngine.CollisionFlags.Above:
                        return SurfaceNormalAbove;
                    case UnityEngine.CollisionFlags.Below:
                        return SurfaceNormalBelow;
                    case UnityEngine.CollisionFlags.Sides:
                        return SurfaceNormalSides;
                    default:
                        return Vector3.zero;
                }
            }
        }

        protected float Speed;

        protected float Momentum = 0.0f;

        protected HeroMotion Hero;

        protected Animator animator;

        protected InputManager InputManager;

        #endregion

        #region Public Methods

        public HeroState(FSM fsm)
            : base(fsm)
        {
            Hero = (HeroMotion)fsm;
            animator = gameObject.GetComponent<Animator>();
            InputManager = gameObject.GetComponent<InputManager>();
        }

        public override void Enter()
        {
            Debug.LogWarning("Entering " + Name);

            if (!(PreviousState is HeroState))
            {
                return;
            }

            var prev = PreviousState as HeroState;
            surfaceNormalAbove = prev.surfaceNormalAbove;
            surfaceNormalBelow = prev.surfaceNormalBelow;
            surfaceNormalSides = prev.surfaceNormalSides;
            CollisionFlags = prev.CollisionFlags;
            SurfaceLocation = prev.SurfaceLocation;
            Momentum = prev.Momentum;
            Speed = prev.Speed;
        }

        public override void Exit()
        {
            Debug.LogWarning("Exiting " + Name);
        }

        public override void OnCollisionStay(Collision collision)
        {
            var capsule = collider as CapsuleCollider;
            Vector3 tubeHeight = (capsule.height - 2.0f * capsule.radius) * transform.up;
            Vector3 tubeBottom = collider.bounds.center - 0.5f * tubeHeight;

            foreach (var contact in collision.contacts)
            {
                CollisionFlags |= CollisionLocation(tubeBottom, tubeHeight, contact);
            }

            if (((int)CollisionFlags & (int)CollisionFlags.Above) != 0)
            {
                surfaceNormalAbove = CollidingSurfaceNormal(tubeBottom, transform.up);
            }

            if (((int)CollisionFlags & (int)CollisionFlags.Below) != 0)
            {
                surfaceNormalBelow = CollidingSurfaceNormal(tubeBottom, -transform.up);
            }

            if (((int)CollisionFlags & (int)CollisionFlags.Sides) != 0)
            {
                surfaceNormalSides = CollidingSurfaceNormal(tubeBottom, transform.forward);
            }
        }

        public override void FixedUpdate()
        {
            //Debug.Log("Momentum " + Momentum);
            CollisionFlags = CollisionFlags.None;
        }
        
        #endregion

        #region Protected Methods

        protected Vector3 DirectionRelativeToBasisOnSurface(Vector3 localDirection, Vector3 baseForward, Vector3 surfaceNormal)
        {
            Vector3 globalForwardSS = MathHelper.ProjectVectorToPlane(Vector3.forward, surfaceNormal).normalized;
            Vector3 baseForwardSS = MathHelper.ProjectVectorToPlane(baseForward, surfaceNormal).normalized;

            Quaternion transformToBaseSS = Quaternion.FromToRotation(globalForwardSS, baseForwardSS);
            Vector3 relativeDirection = (transformToBaseSS * localDirection).normalized;

            return MathHelper.ProjectVectorToPlane(relativeDirection, surfaceNormal).normalized;
        }

        protected bool IsMoving()
        {
            return Input.GetAxis("Horizontal Movement") != 0.0f || Input.GetAxis("Vertical Movement") != 0.0f;
        }

        protected bool IsIdle()
        {
            return !IsMoving();
        }

        protected bool IsCollidingBelow()
        {
            return ((int)CollisionFlags & (int)CollisionFlags.Below) != 0;
        }

        protected bool IsNotCollidingBelow()
        {
            return !IsCollidingBelow();
        }

        protected bool IsCollidingSides()
        {
            return ((int)CollisionFlags & (int)CollisionFlags.Sides) != 0;
        }

        protected bool CanWallRun()
        {
            return IsCollidingSides() &&
                InputManager.Grip >= 0.25f &&
                MathHelper.ProjectVectorToPlane(rigidbody.velocity, SurfaceNormal).sqrMagnitude > Hero.MinWallRunSpeed * Hero.MinWallRunSpeed;
        }

        #endregion

        #region Private Methods

        private CollisionFlags CollisionLocation(Vector3 tubeBottom, Vector3 tubeHeight, ContactPoint contact)
        {
            Vector3 contactOffset = contact.point - tubeBottom;

            float c1 = Vector3.Dot(contactOffset, tubeHeight);
            if (c1 < 0.0f)
            {
                return CollisionFlags.Below;
            }

            float c2 = Vector3.Dot(tubeHeight, tubeHeight);
            if (c2 < c1)
            {
                return CollisionFlags.Above;
            }

            return CollisionFlags.Sides;
        }

        private Vector3 CollidingSurfaceNormal(Vector3 origin, Vector3 direction)
        {
            Ray ray = new Ray(origin, direction);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);

            return hit.normal;
        }

        #endregion
    }
}
