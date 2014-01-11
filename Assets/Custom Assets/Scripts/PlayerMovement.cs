using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private float maxJumpHeight = 2.0f;
    [SerializeField]
    private float gravity = 10.0f;
    [SerializeField]
    private float maxSpeed = 10.0f;
    [SerializeField]
    private Transform camera = null;

    private PlatformSide platformSide = PlatformSide.Below;
    private Vector3 platformNormal = Vector3.up;
  
    private Vector3 movementInput = Vector3.zero;
    private float gripInput = 0.0f;
    private float jumpInput = 0.0f;
    private float previousJumpInput = 0.0f;

    private bool isGrounded = false;

    private CapsuleCollider capsule;

    #endregion

    #region Unity Methods

    void Start ()
    {
        capsule = collider as CapsuleCollider;
  	}
	
	void Update ()
    {
        GetInput();
        GetPlatform();
        Move();
	}

    #endregion

    #region Unity Events

    void OnCollisionStay()
    {
        isGrounded = true;
    }

    #endregion

    #region Field Update Methods

    private void GetInput()
    {
        movementInput = new Vector3(Input.GetAxis("Horizontal Movement"), 0.0f, Input.GetAxis("Vertical Movement"));
        gripInput = Input.GetAxis("Grip");

        previousJumpInput = jumpInput;
        jumpInput = Input.GetAxis("Jump");
    }

    private void GetPlatform()
    {
    }

    #endregion

    #region Movement Methods

    private void Move()
    {
        if (isGrounded)
        {
            Vector3 jumpVelocity = Vector3.zero;
            if (jumpInput < previousJumpInput)
            {
                Debug.Log(previousJumpInput);
                jumpVelocity = platformNormal * Mathf.Sqrt(previousJumpInput * maxJumpHeight * gravity);
            }

            if (platformSide == PlatformSide.Side && gripInput < 0.5f)
            {
                isGrounded = false;
                jumpVelocity += platformNormal * 1.0f;
            }

            Vector3 cameraForward = MathHelper.ProjectVectorToPlane(camera.forward, platformNormal);
            cameraForward.Normalize();
            Debug.DrawRay(transform.position, cameraForward, Color.magenta);

            Vector3 movementForward = ThumbstickToPlatform(cameraForward);

            float speed = movementInput.sqrMagnitude;
            rigidbody.velocity = movementForward * speed * maxSpeed + jumpVelocity;

            Vector3 colliderNormal;
            if (IsCollidingBelow(out colliderNormal))
            {
                platformSide = PlatformSide.Below;
                platformNormal = colliderNormal;
            }
            if (gripInput > 0.5f && IsCollidingOnSide(cameraForward, out colliderNormal))
            {
                platformSide = PlatformSide.Side;
                platformNormal = colliderNormal;
            }
        }

        if (platformSide != PlatformSide.Side || gripInput < 0.5f)
        {
            rigidbody.AddForce(new Vector3(0.0f, -gravity * rigidbody.mass, 0.0f));
        }

        isGrounded = false;
    }

    private Vector3 ThumbstickToPlatform(Vector3 cameraForward)
    {
        Vector3 movementForward = Vector3.zero;
        Vector3 movementRight = Vector3.zero;

        if (platformSide != PlatformSide.Side)
        {
            movementForward = cameraForward;
            movementRight = Vector3.Cross(cameraForward, platformNormal);
        }
        else
        {
            movementForward = MathHelper.ProjectVectorToPlane(Vector3.up, platformNormal);
            movementForward.Normalize();
            movementRight = Vector3.Cross(movementForward, platformNormal);
        }

        Vector3 movement = movementForward * movementInput.z - movementRight * movementInput.x;
        movement.Normalize();

        return movement;
    }

    private bool IsCollidingOnSide(Vector3 cameraForward, out Vector3 wallNormal)
    {
        Ray ray = new Ray(capsule.transform.position + capsule.transform.up * capsule.radius, cameraForward);
        
        RaycastHit hit;
        bool result = Physics.Raycast(ray, out hit, capsule.radius * 1.05f);
        
        wallNormal = hit.normal;
        return result;
    }

    private bool IsCollidingBelow(out Vector3 floorNormal)
    {
        Ray ray = new Ray(capsule.transform.position, -capsule.transform.up);
        
        RaycastHit hit;
        bool result = Physics.Raycast(ray, out hit, capsule.height * 0.01f);
        
        floorNormal = hit.normal;
        return result;
    }

    private bool IsCollidingAbove(out Vector3 ceilingNormal)
    {
        Ray ray = new Ray(capsule.transform.position, capsule.transform.up);
        
        RaycastHit hit;
        bool result = Physics.Raycast(ray, out hit, capsule.height * 1.01f);
        
        ceilingNormal = hit.normal;
        return result;
    }
    
    #endregion
}

public enum PlatformSide
{
    Below, Above, Side
}