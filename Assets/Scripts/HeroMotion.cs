using UnityEngine;
using System.Collections;
using UnityFSM;
using HeroStates;

public class HeroMotion : FSM
{
    public Transform Camera;

    public float Stage1MaxSpeed;
    public float Stage2MaxSpeed;
    public float Stage3MaxSpeed;
    public float Stage4MaxSpeed;
    public float Stage5MaxSpeed;

    public float Stage2MinMomentum;
    public float Stage3MinMomentum;

    public float Gravity;

    public float MinWallRunSpeed;
    public Vector3 RespawnPosition;
    public float MaxJumpTime;

    public float MaxAirSpeed;

    public float LargeJumpBound;

    public float SmallJumpForce;
    public float LargeJumpForce;

    protected override void FSMAwake()
    {
        AddState<Idle>(true);
        AddState<MovementStage1>();
        AddState<MovementStage2>();
        AddState<MovementStage3>();
        AddState<Jump>();
        AddState<ControlledFall>();
        AddState<WallRun>();
        AddState<Respawn>();
    }
}