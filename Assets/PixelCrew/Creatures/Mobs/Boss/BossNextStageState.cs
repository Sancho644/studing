using PixelCrew.Components.GoBased;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss
{
    public class BossNextStageState : StateMachineBehaviour
    {
        [ColorUsage(true, true)] [SerializeField] private Color _stageColor;

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var spawner = animator.GetComponent<CircularProjectileSpawner>();
            spawner.Stage++;

            var changeLight = animator.GetComponent<ChangeLightComponent>();
            changeLight.SetColor(_stageColor);
        }
    }
}