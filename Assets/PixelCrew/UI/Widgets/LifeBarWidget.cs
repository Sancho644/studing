using UnityEngine;
using PixelCrew.Components.Health;
using PixelCrew.Utils.Disposables;

namespace PixelCrew.UI.Widgets
{
    public class LifeBarWidget : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private RectTransform _lifeBarScale;
        [SerializeField] private ProgressBarWidget _lifeBar;
        [SerializeField] private HealthComponent _hp;

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private int _maxHp;

        private void Start()
        {
            if (_hp == null)
            {
                _hp = GetComponentInParent<HealthComponent>();
            }

            _maxHp = _hp.Health;

            _trash.Retain(_hp._onDie.Subscribe(OnDie));
            _trash.Retain(_hp._onChange.Subscribe(OnHpChanged));
        }

        private void Update()
        {
            var scale = _target.lossyScale;

            if (scale.x == -1) _lifeBarScale.localScale = new Vector3(-1f, 1f, 1f);
            else _lifeBarScale.localScale = new Vector3(1f, 1f, 1f);
        }

        private void OnDie()
        {
            Destroy(gameObject);
        }

        private void OnHpChanged(int hp)
        {
            var progress = (float)hp / _maxHp;
            _lifeBar.SetProgress(progress);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}