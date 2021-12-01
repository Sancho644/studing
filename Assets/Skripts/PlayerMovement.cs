using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Player _player;

        public void OnHorizontalMovement(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            _player.SetDirection(direction);
        }
    }
}