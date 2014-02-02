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

    private Vector3 mForward = Vector3.forward;
    #endregion

    void Awake()
    {
        InputManager = follow.parent.gameObject.GetComponent<InputManager>();
    }

	// Update is called once per frame
	void Update()
    {
        mForward = Quaternion.Euler(/*-InputManager.CameraInput.x * 3.0f*/0.0f, InputManager.CameraInput.y * 10.0f, 0.0f) * mForward;
	}

    void LateUpdate()
    {
        Vector3 xzPosition = MathHelper.ProjectVectorToPlane(follow.position, Vector3.up);
        targetPosition = xzPosition + Vector3.up * targetOffset + follow.up * positionOffset.y - mForward * positionOffset.x;

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);
        transform.LookAt(xzPosition + Vector3.up * targetOffset);
    }
}
