using Leopotam.EcsLite;
using UnityEngine;

namespace Core.Object.Character.Activity
{
    public sealed class PlayingActivityAnimationSystem : IEcsInitSystem
    {
        private EcsPool<ActivityTrackingComponent> _activityTrackingPool = null;
        private EcsFilter _filter = null;
        private EcsWorld _world = null;

        public void Init(EcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<ActivityTrackingComponent>().End();
            _activityTrackingPool = _world.GetPool<ActivityTrackingComponent>();
            foreach (int entity in _filter)
            {
                ref ActivityTrackingComponent activityTracking = ref _activityTrackingPool.Get(entity);
                activityTracking.OnChangeCurrentValue += AnimationPlayback;
                activityTracking.CurrentValue = activityTracking.GetCharacterActivityList(TypesActivity.Idle).Value;
            }
        }
        private void AnimationPlayback(ref ActivityTrackingComponent activityTracking)
        {
            if (activityTracking.CurrentValue.IndexAnimatorControllerParameter != null)
            {
                int index = activityTracking.CurrentValue.IndexAnimatorControllerParameter.Value;
                AnimatorControllerParameter animatorControllerParameter = activityTracking.Animator.parameters[index];
                switch (animatorControllerParameter.type)
                {
                    case AnimatorControllerParameterType.Bool:
                        activityTracking.Animator.SetBool(animatorControllerParameter.name, true);
                        activityTracking.OnGetReadyChange—urrentValue += DisablingAnimation;
                        break;
                    case AnimatorControllerParameterType.Trigger:
                        activityTracking.Animator.SetTrigger(animatorControllerParameter.name);
                        break;
                }
            }
        }
        private void DisablingAnimation(ref ActivityTrackingComponent activityTracking)
        {
            int index = activityTracking.CurrentValue.IndexAnimatorControllerParameter.Value;
            AnimatorControllerParameter animatorControllerParameter = activityTracking.Animator.parameters[index];
            switch (animatorControllerParameter.type)
            {
                case AnimatorControllerParameterType.Bool:
                    activityTracking.Animator.SetBool(animatorControllerParameter.name, false);
                    break;
            }
            activityTracking.OnGetReadyChange—urrentValue -= DisablingAnimation;
        }
    }
}
