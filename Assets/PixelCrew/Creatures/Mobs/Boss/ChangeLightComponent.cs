using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace PixelCrew.Creatures.Mobs.Boss
{
    public class ChangeLightComponent : MonoBehaviour
    {
        [SerializeField] private Light2D[] _lights;

        [ColorUsage(true, true)]
        [SerializeField]
        private Color _color;

        [ContextMenu("Setup")]
        public void SetColor()
        {
            foreach (var light2d in _lights)
            {
                light2d.color = _color;
            }
        }
    }
}