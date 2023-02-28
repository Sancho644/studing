using System.Collections;
using UnityEngine;
using PixelCrew.Components.ColliderBased;
using System;
using UnityEngine.Events;

namespace PixelCrew.Creatures.Mobs.Patrolling
{
    public class PlatformPatrol : Patrol
    {
        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] private LayerCheck _obstacleCheck;
        [SerializeField] private int _direction;
        [SerializeField] private OnChangeDirection _onChangeDirection;

        public LayerCheck ObstacleCheck => _obstacleCheck;
        public LayerCheck GroundCheck => _groundCheck;

        public override IEnumerator DoPatrol()
        {
            while (enabled)
            {
                if (_groundCheck.IsTochingLayer && !_obstacleCheck.IsTochingLayer)
                {
                    _onChangeDirection?.Invoke(new Vector2(_direction, 0));
                }
                else
                {
                    _direction = -_direction;
                    _onChangeDirection?.Invoke(new Vector2(_direction, 0));
                }

                yield return null;
            }
        }

        public void SetDirection(int direction)
        {
            _direction = direction;
        }
    }

    [Serializable]
    public class OnChangeDirection : UnityEvent<Vector2>
    {
    }
}