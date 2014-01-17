using UnityEngine;

namespace States
{
    public class IdleFloorStateHero : HeroState
    {
        public IdleFloorStateHero(HeroMotion hero) : base(hero) { }

        public override void Enter()
        {
            Debug.Log("Entering " + Name);
        }

        public override void Exit()
        {
            Debug.Log("Exiting " + Name);            
        }
    }
}
