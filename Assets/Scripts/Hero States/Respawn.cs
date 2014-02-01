using UnityEngine;
using UnityFSM;

namespace HeroStates
{
    class Respawn : HeroState
    {
        public Respawn(FSM fsm)
            : base(fsm)
        {
            AddTransition<Idle>(() => { return true; });
        }

        public override void Enter()
        {
            base.Enter();
            Momentum = 0.0f;
            rigidbody.velocity = Vector3.zero;
            transform.position = Hero.RespawnPosition;
            transform.rotation = Quaternion.identity;
        }
    }
}
