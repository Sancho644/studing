using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpspeed;
    [SerializeField] private float _sidespeed;

    private Vector2 _direction;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }
    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_direction.x * _sidespeed, _rigidbody.velocity.y);

        var isJumping = _direction.y > 0;
        if (isJumping)
        {
            _rigidbody.AddForce(Vector2.up * _jumpspeed, ForceMode.Impulse);
        }

        _rigidbody.AddForce(0, 0, _speed * Time.deltaTime);

    }
}
