using UnityEngine;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Utils;
using PixelCrew.Components.GoBased;
using PixelCrew.Components.Audio;

namespace PixelCrew.Creatures.Mobs
{
    public class SeashellTrapAI : MonoBehaviour
    {
        [SerializeField] private ColliderCheck _vision;

        [Header("Melee")]
        [SerializeField] private Cooldown _meleeCooldown;
        [SerializeField] private CheckCircleOverlap _meleeAttack;
        [SerializeField] private ColliderCheck _meleeCanAttack;

        [Header("Range")]
        [SerializeField] private Cooldown _rangeCooldown;
        [SerializeField] private SpawnComponent _rangeAttack;

        private static readonly int Melee = Animator.StringToHash("melee");
        private static readonly int Range = Animator.StringToHash("range");

        private PlaySoundsComponent _sounds;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _sounds = GetComponent<PlaySoundsComponent>();
        }

        private void Update()
        {
            if (_vision.IsTochingLayer)
            {
                if (_meleeCanAttack.IsTochingLayer)
                {
                    if (_meleeCooldown.IsReady)
                    {
                        MeleeAttack();
                        _sounds.Play("Melee");
                        return;
                    }
                }

                if (_rangeCooldown.IsReady)
                {
                    RangeAttack();
                    _sounds.Play("Range");
                }
            }
        }

        private void MeleeAttack()
        {
            _meleeCooldown.Reset();
            _animator.SetTrigger(Melee);
        }

        private void RangeAttack()
        {
            _rangeCooldown.Reset();
            _animator.SetTrigger(Range);
        }

        public void OnMeleeAttack()
        {
            _meleeAttack.Check();
        }

        public void OnRangeAttack()
        {
            _rangeAttack.Spawn();
        }
    }
}
