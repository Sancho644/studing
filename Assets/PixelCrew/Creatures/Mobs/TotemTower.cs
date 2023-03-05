using UnityEngine;
using PixelCrew.Utils;
using System.Collections.Generic;
using PixelCrew.Components.Health;

namespace PixelCrew.Creatures.Mobs
{
    public class TotemTower : MonoBehaviour
    {
        [SerializeField] private List<ShootingTrapAI> _traps;
        [SerializeField] private Cooldown _cooldown;

        private int _currentTrap;

        private void Start()
        {
            foreach (var shootingTrapAI in _traps)
            {
                shootingTrapAI.enabled = false;
                var hp = shootingTrapAI.GetComponent<HealthComponent>();
                hp._onDie.AddListener(() => OnTrapDead(shootingTrapAI));
            }
        }

        private void OnTrapDead(ShootingTrapAI shootingTrapAI)
        {
            var index = _traps.IndexOf(shootingTrapAI);
            _traps.Remove(shootingTrapAI);

            if (index < _currentTrap)
            {
                _currentTrap--;
            }
        }

        private void Update()
        {
            for (int i = 0; i < _traps.Count; i++)
            {
                if (_traps[i] == null) _traps.RemoveAt(i);
            }

            if (_traps.Count == 0)
            {
                enabled = false;
                Destroy(gameObject, 1f);
            }

            var hasAnyTarget = HasAnyTarget();
            if (hasAnyTarget)
            {
                if (_cooldown.IsReady)
                {
                    _traps[_currentTrap].Shoot();
                    _cooldown.Reset();
                    _currentTrap = (int)Mathf.Repeat(_currentTrap + 1, _traps.Count);
                }
            }
        }

        private bool HasAnyTarget()
        {
            foreach (var shootingTrapAi in _traps)
            {
                if (shootingTrapAi._vision.IsTochingLayer)
                    return true;
            }

            return false;
        }
    }
}