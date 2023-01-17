using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.Creatures.Hero
{
    public class HeroInputReaver : MonoBehaviour
    {
        [SerializeField] private Hero _hero;

        public void OnHorizontalMovement(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            _hero.SetDirection(direction);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.Interact();
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.Attack();
            }
        }

        public void OnThrow(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _hero.StartThowing();
            }

            if (context.canceled)
            {
                _hero.UseInventory();
            }
        }

        public void OnNextItem(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.NextItem();
            }
        }

        public void OnDoDash(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.OnDoDash(true);
            }

            if (context.canceled)
            {
                _hero.OnDoDash(false);
            }
        }

        public void OnEnableCandle(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.EnableCandle();
            }
        }

        public void OnUsePerk(InputAction.CallbackContext context)
        {
            if (context.performed)
                _hero.UsePerk();
        }
    }
}