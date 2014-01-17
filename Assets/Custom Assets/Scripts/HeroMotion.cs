using UnityEngine;
using System.Collections;
using States;

public class HeroMotion : FSM
{
    [SerializeField]
    public Transform Camera;

    [SerializeField]
    public float MaxSpeed;

    protected override void FSMAwake()
    {
        HeroState idle = new IdleFloorStateHero(this);
        AddState(idle, true);

        HeroState walk = new MoveFloorStateHero(this);
        AddState(walk);

        AddTransition(new Transition(idle, walk, IdleMoveFloor));
        AddTransition(new Transition(walk, idle, MoveIdleFloor));
    }

    private bool IdleMoveFloor()
    {
        return Input.GetAxis("Horizontal Movement") != 0.0f || Input.GetAxis("Vertical Movement") != 0.0f;
    }

    private bool MoveIdleFloor()
    {
        return Input.GetAxis("Horizontal Movement") == 0.0f && Input.GetAxis("Vertical Movement") == 0.0f;
    }
}