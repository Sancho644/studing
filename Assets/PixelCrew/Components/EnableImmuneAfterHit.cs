using PixelCrew.Components.Health;
using UnityEngine;

namespace PixelCrew.Components
{
    public class EnableImmuneAfterHit : MonoBehaviour
    {
        [SerializeField] private float immuneTime = 1;
        public void SetImmuneAfterHit(GameObject target)
        {
            var setTime = target.GetComponent<ImmuneAfterHit>();

            if (setTime != null)
            {
                setTime.SetTime(immuneTime);
            }
        }
    }
}