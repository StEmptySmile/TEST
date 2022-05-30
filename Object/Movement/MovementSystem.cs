using Core.Object.Character.Activity;
using Leopotam.EcsLite;
using UnityEngine;

namespace Core.Object.Movement
{
    public sealed class MovementSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsPool<DirectionMovementComponent> _directionMovementPool = null;
        private EcsPool<ActivityTrackingComponent> _activityTrackingPool = null;
        private EcsPool<MovableComponent> _movablePool = null;
        private EcsFilter _filter = null;
        private EcsWorld _world = null;

        public void Init(EcsSystems systems)
        {
            _world = systems.GetWorld();
            _directionMovementPool = _world.GetPool<DirectionMovementComponent>();
            _activityTrackingPool = _world.GetPool<ActivityTrackingComponent>();
            _movablePool = _world.GetPool<MovableComponent>();

            _filter = _world.Filter<MovableComponent>().End();

            foreach (int entity in _filter)
            {
                MovableComponent movable = _movablePool.Get(entity);
                movable.Body.updateRotation = false;
                movable.Body.updateUpAxis = false;
                movable.Body.speed = movable.Speed;
                movable.Body.isStopped = false;
                
                ref DirectionMovementComponent directionMovement = ref _directionMovementPool.Add(entity);
                directionMovement = new DirectionMovementComponent
                {
                    destination = Vector2.zero,
                    viewAngle = movable.Body.transform.eulerAngles.y
                };
            }
        }
        public void Run(EcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                ref ActivityTrackingComponent activityTracking = ref _activityTrackingPool.Get(entity);
                DirectionMovementComponent directionMovement = _directionMovementPool.Get(entity);

                if(activityTracking.CurrentValue.Type == TypesActivity.Walk)
                {
                    MovableComponent movable = _movablePool.Get(entity);
                    Moving(movable, directionMovement.destination);
                    Turning(movable, directionMovement.viewAngle);
                }
                Activity(ref activityTracking, directionMovement.destination);
            }
        }
        private void Turning(MovableComponent movable, float viewAngle)
        {
            movable.Body.transform.eulerAngles = new Vector3(0f, viewAngle, 0f);
        }
        private void Moving(MovableComponent movable, Vector2 destination)
        {
            movable.Body.destination = (Vector2)movable.Body.transform.position + destination;
        }
        private void Activity(ref ActivityTrackingComponent activityTracking, Vector2 destination)
        {
            if (destination != Vector2.zero && activityTracking.TryGetCharacterActivityList(TypesActivity.Walk, out ActivityComponent activityWalk))
            {
                activityTracking.CurrentValue = activityWalk;
            }
            else if (activityTracking.TryGetCharacterActivityList(TypesActivity.Idle, out ActivityComponent activityIdle))
            {
                activityTracking.CurrentValue = activityIdle;
            }
        }
    }
}
