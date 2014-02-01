using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
    public float MinJumpTime;

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

    private float lastJump;
    public float LastJump
    {
        get
        {
            return lastJump;
        }
    }

    private float jumpTime = 0.0f;
    public float JumpTime
    {
        get
        {
            return jumpTime;
        }
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
        JumpReleased = false;

        MovementInput = new Vector3(Input.GetAxis("Horizontal Movement"), 0.0f, Input.GetAxis("Vertical Movement"));
        CameraInput = new Vector3(Input.GetAxis("Camera Pitch"), Input.GetAxis("Camera Yaw"), 0.0f);
        grip = Input.GetAxis("Grip");

        float newJump = Input.GetAxis("Jump");
        if (newJump > 0.0f)
        {
            if (jump == 0.0f)
            {
                jumpTime = 0.0f;
                lastJump = 0.0f;
            }
            else
            {
                jumpTime += Time.deltaTime;
                lastJump = Mathf.Max(lastJump, newJump);

                if (jumpTime > MinJumpTime)
                {
                    JumpReleased = true;
                }
            }            
        }

        jump = newJump;

        //if (newJump == 0.0f && jump > 0.0f)
        //{
        //    JumpReleased = true;
        //}
        //else if (newJump > 0.0f)
        //{
        //    if (jump == 0.0f)
        //    {
        //        jumpTime = 0.0f;
        //    }
        //    else
        //    {
        //        jumpTime += Time.deltaTime;
        //    }
        //}
        //jump = newJump;
    }
}
