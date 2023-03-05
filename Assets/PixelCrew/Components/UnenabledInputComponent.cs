using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.Components
{
    public class UnenabledInputComponent : MonoBehaviour
    {
        public void SetInput(GameObject target)
        {
            var heroInput = target.GetComponent<PlayerInput>();

            if (heroInput != null)
            {
                SetLockInput(heroInput);
            }
        }

        private void SetLockInput(PlayerInput heroInput)
        {

            if (heroInput != null)
            {
                if (heroInput.enabled == false)
                {
                    heroInput.enabled = true;
                }
                else
                    heroInput.enabled = false;
            }
        }
    }
}