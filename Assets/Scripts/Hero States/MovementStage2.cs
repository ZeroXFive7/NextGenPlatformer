using UnityEngine;
using UnityFSM;

namespace HeroStates
{
    public class MovementStage2 : MovementStage
    {
        public MovementStage2(FSM fsm)
            : base(fsm)
        {
            // Add Transitions.
            AddTransition<MovementStage1>(() => { return Momentum < Hero.Stage2MinMomentum; });

            this.maxAnimationSpeed = 1.5f;
            this.maxSpeed = Hero.Stage2MaxSpeed;
            this.rotationRate = 6.0f;
        }

        protected override void UpdateMomentum(float directionDeltaDotProduct, float inputMagnitude)
        {
            if (directionDeltaDotProduct < -0.5f)
            {
                Momentum = 0.0f;
            }
            if (inputMagnitude < 0.8f)
            {
                Momentum *= 0.6f;
            }
        }
    }
}
