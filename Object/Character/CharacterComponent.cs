using Core.Object.Characteristic.Utility;
using Core.Battle.Damage;
using Core.Object.Status;
using UnityEngine;

namespace Core.Object.Character
{
    [System.Serializable]
    public struct CharacterComponent : IDoDamage, ICanBeDestroyed
    {
        public TeamComponent Team => _team;
        [SerializeField] private TeamComponent _team;
        public StatusComponent Health => _health;
        [SerializeField] private StatusComponent _health;
        public DamageByObjectComponent DamageByObject => _damageByObject;
        [SerializeField] private DamageByObjectComponent _damageByObject;
        public PackageWithCharacteristics Characteristics  => _characteristics;
        [SerializeField] private PackageWithCharacteristics _characteristics;
    }
}
