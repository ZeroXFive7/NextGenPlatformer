using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    #region Fields

    public Vector3 Forward = Vector3.zero;

    [SerializeField]
    private float maxJumpHeight = 2.0f;
    [SerializeField]
    private float gravity = 10.0f;
    [SerializeField]
    private float maxSpeed = 10.0f;
    [SerializeField]
    private Transform camera = null;

    private PlatformSide platformSide = PlatformSide.Below;

    // Input Variables.
    private Vector3 movementInput = Vector3.zero;
    private float gripInput = 0.0f;
    private float jumpInput = 0.0f;

    // Platform Collision Variables.
    private PlatformInfo[] platforms = new PlatformInfo[3];

    // Wall Running Variables.
    private float wallRunStamina = 0.0f;
    private float wallRunTireRate = 1.0f;
    private Vector3 wallRunDirection = Vector3.zero;
    private bool falling = true;

    private CapsuleCollider capsule;


    #endregion

    #region Unity Methods

    void Start()
    {
        capsule = collider as CapsuleCollider;
    }

    void Update()
    {
        GetInput();
        //UpdateForward();
        Move();
    }

    #endregion

    #region Unity Events

    void OnCollisionStay(Collision collisionInfo)
    {
        Vector3 sideNormal;
        foreach (ContactPoint contact in collisionInfo.contacts)
        {
            PlatformSide side = GetContactSide(contact.point, out sideNormal);
            platforms[(int)side].Normal += sideNormal;
            platforms[(int)side].IsColliding = true;
        }

        for (var i = 0; i < 3; i++)
        {
            platforms[i].Normal.Normalize();
        }

        if (platforms[(int)PlatformSide.Above].IsColliding)
        {
            Debug.DrawRay(transform.position, platforms[(int)PlatformSide.Above].Normal, Color.yellow);
        }

        if (platforms[(int)PlatformSide.Below].IsColliding)
        {
            Debug.DrawRay(transform.position, platforms[(int)PlatformSide.Below].Normal, Color.blue);
        }

        if (platforms[(int)PlatformSide.Side].IsColliding)
        {
            Debug.DrawRay(transform.position, platforms[(int)PlatformSide.Side].Normal, Color.red);
        }
    }

    #endregion

    #region Field Update Methods

    private void GetInput()
    {
        movementInput = new Vector3(Input.GetAxis("Horizontal Movement"), 0.0f, Input.GetAxis("Vertical Movement"));
        gripInput = Input.GetAxis("Grip");

        jumpInput = Input.GetAxis("Jump");
    }

    private void UpdateForward()
    {
        /*
         * If was not on wall and is now on wall
         *      mode = wallRun
         *      wallRunTime = wallRunConst / speed
         *      forward = project forward on to wall.
         *      maxAngle = 
         */
    }

    #endregion

    #region Movement Methods

    private void Move()
    {
        if (wallRunStamina > 0.0f)
        {
            Debug.Log(wallRunStamina);
            wallRunStamina -= wallRunTireRate * Time.deltaTime;

            Vector3 jumpVelocity = Vector3.zero;
            if (jumpInput > 0.5f)
            {
                jumpVelocity = platforms[(int)PlatformSide.Side].Normal * Mathf.Sqrt(maxJumpHeight * gravity);
            }
            
            float rotation = 0.0f;
            rigidbody.velocity = Quaternion.Euler(0.0f, rotation, 0.0f) * wallRunDirection * maxSpeed + jumpVelocity;

            if (gripInput < 0.5f)
            {
                wallRunStamina = 0.0f;
                falling = true;
            }
        }
        else
        {
            if (platforms[(int)PlatformSide.Below].IsColliding)
            {
                Vector3 jumpVelocity = Vector3.zero;
                if (jumpInput > 0.5f)
                {
                    jumpVelocity = platforms[(int)PlatformSide.Below].Normal * Mathf.Sqrt(maxJumpHeight * gravity);
                }

                Vector3 cameraForward = MathHelper.ProjectVectorToPlane(camera.forward, platforms[(int)PlatformSide.Below].Normal);
                cameraForward.Normalize();
                Debug.DrawRay(transform.position, cameraForward, Color.magenta);
                Forward = cameraForward;

                Vector3 movementForward = ThumbstickToSurface(cameraForward);

                float speed = movementInput.sqrMagnitude;
                rigidbody.velocity = movementForward * speed * maxSpeed + jumpVelocity;

                falling = false;
            }

            if (!falling && platforms[(int)PlatformSide.Side].IsColliding)
            {
                if (Vector3.Dot(Forward, -platforms[(int)PlatformSide.Side].Normal) > 0.0f && gripInput > 0.5f)
                {
                    wallRunStamina = rigidbody.velocity.sqrMagnitude;
                    wallRunDirection = Vector3.up;
                }
            }

            rigidbody.AddForce(new Vector3(0.0f, -gravity * rigidbody.mass, 0.0f));
        }

        for (var i = 0; i < 3; i++)
        {
            platforms[i].IsColliding = false;
            platforms[i].Normal = Vector3.zero;
        }
    }

    private Vector3 ThumbstickToSurface(Vector3 cameraForward)
    {
        Vector3 movementForward = Vector3.zero;
        Vector3 movementRight = Vector3.zero;

        if (platformSide != PlatformSide.Side)
        {
            movementForward = cameraForward;
            movementRight = Vector3.Cross(cameraForward, platforms[(int)PlatformSide.Below].Normal);
        }
        else
        {
            movementForward = MathHelper.ProjectVectorToPlane(Vector3.up, platforms[(int)PlatformSide.Side].Normal);
            movementForward.Normalize();
            movementRight = Vector3.Cross(movementForward, platforms[(int)PlatformSide.Side].Normal);
        }

        Vector3 movement = movementForward * movementInput.z - movementRight * movementInput.x;
        movement.Normalize();

        return movement;
    }

    private PlatformSide GetContactSide(Vector3 contactPoint, out Vector3 sideNormal)
    {
        Vector3 tubeHeight = transform.up * (capsule.height - capsule.radius);
        Vector3 tubeBottom = collider.bounds.center - 0.5f * tubeHeight;

        Vector3 contactOffset = contactPoint - tubeBottom;

        float c1 = Vector3.Dot(contactOffset, tubeHeight);
        if (c1 < 0.0f)
        {
            sideNormal = GetContactNormal(contactPoint, tubeBottom);
            return PlatformSide.Below;
        }

        float c2 = Vector3.Dot(tubeHeight, tubeHeight);
        if (c2 < c1)
        {
            Vector3 tubeTop = tubeBottom + tubeHeight;
            sideNormal = GetContactNormal(contactPoint, tubeTop);
            return PlatformSide.Above;
        }

        Vector3 tubeSide = tubeBottom + c1 / c2 * tubeHeight;
        sideNormal = GetContactNormal(contactPoint, tubeSide);
        return PlatformSide.Side;
    }

    private Vector3 GetContactNormal(Vector3 destination, Vector3 origin)
    {
        RaycastHit hit;
        Ray ray = new Ray(origin, destination - origin);
        Physics.Raycast(ray, out hit);

        return hit.normal;
    }

    #endregion

    #region Structures

    private struct PlatformInfo
    {
        public bool IsColliding;
        public Vector3 Normal;
    }

    #endregion
}

public enum PlatformSide
{
    Below, Above, Side
}