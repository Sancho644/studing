using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.CutScenes
{
    public class ShowTargetComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private CameraStateController _controller;
        [SerializeField] private float _delay = 0.5f;
        [SerializeField] private UnityEvent _onDelay;

        private void OnValidate()
        {
            if (_controller == null)
                _controller = FindObjectOfType<CameraStateController>();
        }

        [ContextMenu("Show")]
        public void ShowTarget()
        {
            _controller.SetPosition(_target.position);
            _controller.SetState(true);
            Invoke(nameof(MoveBack), _delay);
            _onDelay?.Invoke();
        }

        private void MoveBack()
        {
            _controller.SetState(false);
        }
    }
}