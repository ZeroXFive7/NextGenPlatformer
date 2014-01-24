using UnityEngine;
using System.Collections;

public static class InputManager
{
    static private int lastFrame = 0;

    static private Vector3 movementInput;
    static public Vector3 MovementInput
    {
        get
        {
            Update();
            return movementInput;
        }
    }

    static private Vector3 cameraInput;
    static public Vector3 CameraInput
    {
        get
        {
            Update();
            return cameraInput;
        }
    }

    static private float jump;
    static public float Jump
    {
        get
        {
            Update();
            return jump;
        }
    }

    static private float grip;
    static public float Grip
    {
        get
        {
            Update();
            return grip;
        }
    }

    static public void Update()
    {
        if (Time.frameCount <= lastFrame)
        {
            return;
        }

        lastFrame = Time.frameCount;
        
        movementInput = new Vector3(Input.GetAxis("Horizontal Movement"), 0.0f, Input.GetAxis("Vertical Movement"));
        cameraInput = new Vector3(Input.GetAxis("Camera Pitch"), Input.GetAxis("Camera Yaw"), 0.0f);
        jump = Input.GetAxis("Jump");
        grip = Input.GetAxis("Grip");
    }
}
