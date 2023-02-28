using PixelCrew.Creatures.Mobs.Boss.Tentacles;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss
{
    public class BossTentaclesState : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var spawner = animator.GetComponent<TentaclesController>();
            spawner.StartTentacles();
        }
    }
}