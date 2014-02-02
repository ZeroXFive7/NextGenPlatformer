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
    public float Jump
    {
        get
        {
            return jump;
        }
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

        float thisJump = Input.GetAxis("Jump");
        JumpPressed = thisJump >= MinTriggerValue && jump < MinTriggerValue;
        JumpReleased = thisJump < MinTriggerValue && jump >= MinTriggerValue;
        jump = thisJump;
    }
}
