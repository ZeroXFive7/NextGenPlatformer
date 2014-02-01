using UnityEngine;
using System.Collections;
using UnityFSM;
using HeroStates;

public class HeroMotion : FSM
{
    public Transform Camera;
    public float MaxSpeed;
    public float AccelerationTime;

    public float MaxMomentum;

    public float Gravity;
    public float MaxAirSpeed;
    public float AirSpeed;
    public float MinWallRunSpeed;
    public Vector3 RespawnPosition;
    public float MaxJumpTime;

    public float LargeJumpBound;

    public float SmallJumpForce;
    public float LargeJumpForce;

    protected override void FSMAwake()
    {
        AddState<Idle>(true);
        AddState<Move>();
        AddState<Jump>();
        AddState<ControlledFall>();
        AddState<WallRun>();
        AddState<Respawn>();
    }
}