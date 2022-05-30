using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
using System.Reflection;
using System.Linq;
using System;

namespace Core.Object.Character.Activity
{
    public sealed class DistributionSuitableByActivitySystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsPool<ActivityTrackingComponent> _activityTrakingPool = null;
        private EcsFilter _filter = null;
        private EcsWorld _world = null;

        private readonly Dictionary<TypesActivity, List<NeedsSuitableActiveComponent>> _sortedComponents = new();
        private readonly List<NeedsSuitableActiveComponent> _components = new();

        public void Init(EcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<ActivityTrackingComponent>().End();
            _activityTrakingPool = _world.GetPool<ActivityTrackingComponent>();

            _components.ForEach(x => x.OnInit?.Invoke(systems));

        }
        public void Run(EcsSystems systems)
        {
            foreach(int entity in _filter)
            {
                ActivityTrackingComponent activityTracking = _activityTrakingPool.Get(entity);
                if(_sortedComponents.ContainsKey(activityTracking.CurrentValue.Type))
                {
                    List<NeedsSuitableActiveComponent> components = _sortedComponents[activityTracking.CurrentValue.Type];
                    components.ForEach(x => x.OnRun?.Invoke(systems));
                }
            }
        }
        public DistributionSuitableByActivitySystem Add(NeedsSuitableActiveComponent component)
        {
            _components.Add(component);
            for(int index = 0; index < component.TypeActivities.Length; index++)
            {
                TypesActivity typeActivity = component.TypeActivities[index];
                if (_sortedComponents.ContainsKey(typeActivity) == false)
                    _sortedComponents.Add(typeActivity, new());
                _sortedComponents[typeActivity].Add(component);
            }
            return this;
        }
    }
}
