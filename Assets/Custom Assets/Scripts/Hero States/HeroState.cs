using UnityEngine;

namespace States
{
    public class HeroState : State
    {
        #region Properties and Fields

        private Vector3 surfaceNormal = Vector3.zero;
        public Vector3 SurfaceNormal
        {
            get
            {
                return surfaceNormal;
            }
            private set
            {
                surfaceNormal = value;
            }
        }

        protected HeroMotion hero;
        protected Transform camera;
        protected Transform transform;
        protected Rigidbody rigidbody;

        #endregion

        #region Public Methods

        public HeroState(HeroMotion hero) : base(hero.gameObject)
        {
            this.hero = hero;
            this.camera = hero.Camera.transform;
            this.transform = hero.gameObject.transform;
            this.rigidbody = hero.gameObject.rigidbody;
        }

        public override void Enter()
        {
            Debug.Log("Entering " + Name);
        }

        public override void Exit()
        {
            Debug.Log("Exiting " + Name);
        }

        #endregion

        #region Protected Methods

        protected void UpdateSurfaceNormal(Vector3 origin, Vector3 direction)
        {
            Ray ray = new Ray(origin, direction);

            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit))
            {
                SurfaceNormal = Vector3.zero;
            }

            SurfaceNormal = hit.normal;
            Debug.DrawRay(hit.point, hit.normal, Color.yellow);
        }

        #endregion
    }
}
