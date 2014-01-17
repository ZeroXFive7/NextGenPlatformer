using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour
{
    #region Private Variables

    [SerializeField]
    private float distanceBehind;
    [SerializeField]
    private float distanceAbove;
    [SerializeField]
    private float smooth;
    [SerializeField]
    private Transform follow;
    private Vector3 targetPosition;

    private Vector3 mForward = Vector3.forward;
    #endregion

    // Use this for initialization
	void Start()
    {
        //follow = GameObject.FindWithTag("Follow").transform;
	}
	
	// Update is called once per frame
	void Update()
    {
        mForward = Quaternion.Euler(-Input.GetAxis("Camera Pitch") * 3.0f, Input.GetAxis("Camera Yaw") * 10.0f, 0.0f) * mForward;
	}

    void LateUpdate()
    {
        targetPosition = follow.position + follow.up * distanceAbove - mForward * distanceBehind;

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);
        transform.LookAt(follow);
    }
}
