using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour
{
    #region Private Variables

    private InputManager InputManager;

    [SerializeField]
    private Vector2 positionOffset;
    [SerializeField]
    private float targetOffset;
    [SerializeField]
    private float smooth;
    [SerializeField]
    private Transform follow;

    private Vector3 targetPosition;
    private float verticalOffset;

    private CapsuleCollider playerCollider;

    private Vector3 forward = Vector3.forward;
    #endregion

    void Awake()
    {
        playerCollider = follow.transform.parent.collider as CapsuleCollider;
        InputManager = follow.parent.gameObject.GetComponent<InputManager>();
    }

	// Update is called once per frame
	void Update()
    {
        forward = Quaternion.Euler(0.0f, InputManager.CameraInput.y * 10.0f, 0.0f) * forward;
	}

    void LateUpdate()
    {
        Vector3 capsuleTopWS = playerCollider.height * playerCollider.transform.up + follow.position;
        Vector3 capsuleTopSS = camera.WorldToScreenPoint(capsuleTopWS);
        Vector3 screenUpperBoundWS = camera.ScreenToWorldPoint(new Vector3(capsuleTopSS.z, 4 * Screen.height / 5, capsuleTopSS.z));

        Vector3 capsuleBaseSS = camera.WorldToScreenPoint(follow.position);
        Vector3 screenLowerBoundWS = camera.ScreenToWorldPoint(new Vector3(capsuleBaseSS.z, 1 * Screen.height / 5, capsuleBaseSS.z));
      
        float upperOffset = Mathf.Max(0.0f, capsuleTopWS.y - screenUpperBoundWS.y);
        float lowerOffset = Mathf.Min(0.0f, follow.position.y - screenLowerBoundWS.y);
        verticalOffset += (upperOffset + lowerOffset);

        Vector3 followPosition = MathHelper.ProjectVectorToPlane(follow.position, Vector3.up) + verticalOffset * Vector3.up;

        targetPosition = followPosition + Vector3.up * targetOffset + follow.up * positionOffset.y - forward * positionOffset.x;

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);
        transform.LookAt(followPosition + Vector3.up * targetOffset);
    }
}
