using System.Collections.Generic;
using Core.Utility;
using UnityEngine;

namespace Core.Object.Characteristic
{
    [System.Serializable]
    public sealed class CharacteristicComponent
    {
        public ValueTypes ValueType => _valueType;
        public float DefaultValue => _defaultValue;
        public float CurrentValue => _currentValue;
        public float MaxValue => _maxValue;
        public float MinValue => _minValue;
        public IReadOnlyList<NumericModifier> Modifiers => _modifiers;

        public event System.Action OnChangeCurrent;

        [SerializeField, HideInInspector] private float _currentValue;
        [SerializeField] private ValueTypes _valueType;
        [SerializeField] private float _defaultValue = 0;
        [SerializeField] private float _maxValue = 0;
        [SerializeField] private float _minValue = 0;

        private readonly List<NumericModifier> _modifiers = new();
        
        public CharacteristicComponent(float maxValue, float minValue, float value, ValueTypes valueType = ValueTypes.Meaning)
        {
            _defaultValue = value;
            _maxValue = maxValue;
            _minValue = minValue;
            _currentValue = _defaultValue;
            _valueType = valueType;
        }
        public void AddModifier(in NumericModifier modifier)
        {
            _modifiers.Add(modifier);

            _currentValue += Mathf.Clamp(_currentValue + modifier, _minValue, _maxValue);

            OnChangeCurrent?.Invoke();
        }
        public bool RemoveModifier(in NumericModifier modifier)
        {
            if(_modifiers.Remove(modifier))
            {
                _currentValue = _defaultValue;
                for(int index = 0; index < _modifiers.Count; index++)
                {
                    _currentValue += Mathf.Clamp(_currentValue + _modifiers[index], _minValue, _maxValue);
                }
                OnChangeCurrent?.Invoke();
                return true;
            }
            return false;
        }
        public enum ValueTypes
        {
            Meaning,
            Percent
        }
    }
}
