using UnityEngine;
using UnityEngine.Events;
using System;
using PixelCrew.Utils;

namespace PixelCrew.Components.Health
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] public UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onHeal;
        [SerializeField] public UnityEvent _onDie;
        [SerializeField] public HealthChangeEvent _onChange;

        private readonly Lock _immune = new Lock();

        public int Health => _health;

        public Lock Immune => _immune;
       

        public void ModifyHealth(int healthDelta)
        {
            if (healthDelta < 0 && Immune.IsLocked) return;
            if (_health <= 0) return;

            _health += healthDelta;
            _onChange?.Invoke(_health);

            if (healthDelta < 0)
            {
                _onDamage?.Invoke();
            }

            if (healthDelta > 0)
            {
                _onHeal?.Invoke();
            }

            if (_health <= 0)
            {
                _onDie?.Invoke();
            }
        }

        public void SetHealth(int health)
        {
            _health = health;
        }

        private void OnDestroy()
        {
            _onDie.RemoveAllListeners();
        }

        [Serializable]
        public class HealthChangeEvent : UnityEvent<int>
        {
        }

        internal void SetHealth(float v)
        {
            throw new NotImplementedException();
        }
    }
}