using UnityEngine;
using PixelCrew.Utils;
using PixelCrew.Utils.ObjectPool;

namespace PixelCrew.Components.GoBased
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private bool _invertXScale;
        [SerializeField] private bool _usePool;

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            SpawnInstance();
        }

        public GameObject SpawnInstance()
        {
            var instance = _usePool
                ? Pool.Instance.Get(_prefab, _target.position)
                : SpawnUtils.Spawn(_prefab, _target.position);

            var scale = _target.lossyScale;
            scale.x *= _invertXScale ? -1 : 1;
            instance.transform.localScale = scale;
            instance.SetActive(true);
            return instance;
        }

        public void SetPrebaf(GameObject prebaf)
        {
            _prefab = prebaf;
        }
    }
}