using UnityEngine;
using UnityFSM;

namespace HeroStates
{
    public class UncontrolledFall : Fall
    {
        public UncontrolledFall(FSM fsm)
            : base(fsm)
        {
            // Add Transitions.
            this.gravity = Hero.Gravity;
            this.maxSpeed = 0.0f;
        }
    }
}
