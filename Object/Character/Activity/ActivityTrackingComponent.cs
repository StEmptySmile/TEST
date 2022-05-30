using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Core.Object.Character.Activity
{
    [System.Serializable]
    public struct ActivityTrackingComponent
    {
        public delegate void HandlerChangeCurrentValue(ref ActivityTrackingComponent sender);
        public event HandlerChangeCurrentValue OnChangeCurrentValue;
        public event HandlerChangeCurrentValue OnGetReadyChangeÑurrentValue;

        public ActivityComponent CurrentValue
        {
            get => _currentValue;
            set
            {
                if(_currentValue.Type != value.Type)
                {
                    OnGetReadyChangeÑurrentValue?.Invoke(ref this);
                    _currentValue = value;
                    OnChangeCurrentValue?.Invoke(ref this);
                }
            }
        }
        public Animator Animator => _animator;
        
        [SerializeField] private Animator _animator;
        [SerializeField] private ActivityComponent _currentValue;
        [SerializeField] private List<ActivityComponent> _activities;

        private readonly static ActivityComponent DEFAULT_CURRENT = new (TypesActivity.Idle);

        public ActivityTrackingComponent(Animator animator, IReadOnlyList<ActivityComponent> activities)
        {
            _activities = new ();
            int number = System.Enum.GetValues(typeof(TypesActivity)).Length;
            _activities.Capacity = number;
            _activities.ForEach(x => x = new ActivityComponent(TypesActivity.None));
            for (int index = 0; index < activities.Count; index++)
            {
                ActivityComponent characterActivity = activities[index];
                _activities[(int)characterActivity.Type] = characterActivity;
            }


            OnGetReadyChangeÑurrentValue = null;
            OnChangeCurrentValue = null;
            _animator = animator;
            _currentValue = DEFAULT_CURRENT;
        }
        public ActivityComponent? GetCharacterActivityList(TypesActivity type)
        {
            ActivityComponent answer = _activities[(int)type];
            return answer.Type == TypesActivity.None ? null : answer;
        }
        public bool TryGetCharacterActivityList(TypesActivity type, out ActivityComponent activity)
        {
            activity = _activities[(int)type];
            return activity.Type != TypesActivity.None;
        }
    }
}
