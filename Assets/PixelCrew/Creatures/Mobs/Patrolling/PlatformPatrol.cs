using System.Collections;
using UnityEngine;
using PixelCrew.Components.ColliderBased;

namespace PixelCrew.Creatures.Mobs.Patrolling
{
    public class PlatformPatrol : Patrol
    {
        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] private LayerCheck _obstacleCheck;
        [SerializeField] private Creature _creature;
        [SerializeField] private int _direction;

        public LayerCheck ObstacleCheck => _obstacleCheck;
        public LayerCheck GroundCheck => _groundCheck;

        public override IEnumerator DoPatrol()
        {
            while (enabled)
            {
                if (_groundCheck.IsTochingLayer && !_obstacleCheck.IsTochingLayer)
                {
                    _creature.SetDirection(new Vector2(_direction, 0));
                }
                else
                {
                    _direction = -_direction;
                    _creature.SetDirection(new Vector2(_direction, 0));
                }

                yield return null;
            }
        }
    }
}