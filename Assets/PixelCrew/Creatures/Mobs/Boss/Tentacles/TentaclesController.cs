using PixelCrew.Components.GoBased;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss.Tentacles
{
    public class TentaclesController : MonoBehaviour
    {
        [SerializeField] private SpawnComponent[] _spawn;
        [SerializeField] private float _delay;

        private Coroutine _coroutine;

        [ContextMenu("Start tentacles")]
        public void StartTentacles()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(TentaclesSpawn());
        }

        private IEnumerator TentaclesSpawn()
        {
            foreach (var spawnComponent in _spawn)
            {
                spawnComponent.Spawn();

                yield return new WaitForSeconds(_delay);
            }

            _coroutine = null;
        }
    }
}