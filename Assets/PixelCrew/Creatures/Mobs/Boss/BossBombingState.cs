using PixelCrew.Creatures.Mobs.Boss.Bombs;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss
{
    public class BossBombingState : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var spawner = animator.GetComponent<BombsController>();
            spawner.StartBombing();
        }
    }
}