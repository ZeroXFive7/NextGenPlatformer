using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
    public float MinTriggerValue;

    public Vector3 MovementInput
    {
        get;
        private set;
    }

    private Vector3 cameraInput;
    public Vector3 CameraInput
    {
        get;
        private set;
    }

    private float jump = 0.0f;
    public float RawJump
    {
        get
        {
            return jump;
        }
    }

    public bool JumpHeld
    {
        get;
        private set;
    }

    public bool JumpPressed
    {
        get;
        private set;
    }

    public bool JumpReleased
    {
        get;
        private set;
    }

    private float grip = 0.0f;
    public float Grip
    {
        get
        {
            return grip;
        }
    }

    void Update()
    {
        JumpPressed = false;

        MovementInput = new Vector3(Input.GetAxis("Horizontal Movement"), 0.0f, Input.GetAxis("Vertical Movement"));
        CameraInput = new Vector3(Input.GetAxis("Camera Pitch"), Input.GetAxis("Camera Yaw"), 0.0f);
        grip = Input.GetAxis("Grip");

        UpdateJump();
    }

    private float prevJump = 0.0f;
    private float prevJumpVel = 0.0f;

    private void UpdateJump()
    {
        float currentJump = Input.GetAxis("Jump");

        float jumpVel = currentJump - prevJump;
        float jumpAcc = jumpVel - prevJumpVel;

        JumpPressed = currentJump >= MinTriggerValue && prevJump < MinTriggerValue;
        JumpReleased = currentJump < MinTriggerValue && prevJump >= MinTriggerValue;
        JumpHeld = (JumpHeld && jumpVel >= -0.05f) || JumpPressed;

        prevJump = currentJump;
        prevJumpVel = jumpVel;
    }
}
