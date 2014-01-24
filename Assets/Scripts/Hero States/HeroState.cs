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

        protected HeroMotion Hero;

        protected Animator animator;

        #endregion

        #region Public Methods

        public HeroState(FSM fsm)
            : base(fsm)
        {
            Hero = (HeroMotion)fsm;
            animator = gameObject.GetComponent<Animator>();
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
            CollisionFlags = CollisionFlags.None;
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
