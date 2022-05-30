using UnityEngine;

namespace Core.Object.Characteristic.Utility
{
    [System.Serializable]
    public struct PackageWithCharacteristics
    {
        public CharacteristicComponent Strength => _strength;
        [SerializeField] private CharacteristicComponent _strength;
        public CharacteristicComponent Agility => _agility;
        [SerializeField] private CharacteristicComponent _agility;
        public CharacteristicComponent Intelligence => _intelligence;
        [SerializeField] private CharacteristicComponent _intelligence;
        public CharacteristicComponent Luck => _luck;
        [SerializeField] private CharacteristicComponent _luck;
        public CharacteristicComponent Armor => _armor;
        [SerializeField] private CharacteristicComponent _armor;
        public float AttackRange => _attackRange;
        [SerializeField] private float _attackRange;
    }
}
