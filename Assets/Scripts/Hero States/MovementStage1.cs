﻿using UnityEngine;
using UnityFSM;

namespace HeroStates
{
    public class MovementStage1 : MovementStage
    {
        public MovementStage1(FSM fsm)
            : base(fsm)
        {
            // Add Transitions.
            AddTransition<MovementStage2>(() => { return Momentum > Hero.Stage2MinMomentum; });

            this.maxAnimationSpeed = 1.0f;
            this.maxSpeed = Hero.Stage1MaxSpeed;
            this.rotationRate = 12.0f;
        }
    }
}
