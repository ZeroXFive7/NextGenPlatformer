using UnityEngine;
using UnityFSM;

namespace HeroStates
{
    public class MovementStage3 : MovementStage
    {
        public MovementStage3(FSM fsm)
            : base(fsm)
        {
            // Add Transitions.
            AddTransition<MovementStage2>(() => { return Momentum < Hero.Stage3MinMomentum; });

            this.maxAnimationSpeed = 2.0f;
            this.maxSpeed = Hero.Stage3MaxSpeed;
            this.rotationRate = 2.0f;
        }

        protected override void UpdateMomentum(float directionDeltaDotProduct, float inputMagnitude)
        {
            if (directionDeltaDotProduct < -0.5f)
            {
                Momentum = 0.0f;
            }
        }
    }
}
