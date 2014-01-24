using UnityEngine;
using System.Collections;
using UnityFSM;
using HeroStates;

public class HeroMotion : FSM
{
    [SerializeField]
    public Transform Camera;
    [SerializeField]
    public float MaxSpeed;
    [SerializeField]
    public float JumpForce;
    [SerializeField]
    public float Gravity;
    [SerializeField]
    public float MaxAirSpeed;
    [SerializeField]
    public float AirSpeed;
    [SerializeField]
    public float MinWallRunSpeed;

    protected override void FSMAwake()
    {
        AddState<Idle>(true);
        AddState<Move>();
        AddState<Jump>();
        AddState<ControlledFall>();
        AddState<WallRun>();
    }
}