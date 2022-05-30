using Leopotam.EcsLite;
using System;

namespace Core.Object.Character.Activity
{
    public struct NeedsSuitableActiveComponent
    {
        public readonly Action<EcsSystems> OnInit;
        public readonly Action<EcsSystems> OnRun;
        public readonly TypesActivity[] TypeActivities;

        public NeedsSuitableActiveComponent(IEcsSystem system, TypesActivity[] typeActivities)
        {
            OnInit = null;
            OnRun = null;

            if (system is IEcsInitSystem initSystem)
                OnInit = initSystem.Init;

            if (system is IEcsRunSystem runSystem)
                OnRun = runSystem.Run;
            
            TypeActivities = typeActivities;
        }
    }
}
