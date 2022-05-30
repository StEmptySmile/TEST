using System.Collections.Generic;
using Core.Utility;
using UnityEngine;

namespace Core.Object.Status
{
    [System.Serializable]
    public sealed class StatusComponent
    {
        public float CurrentValue => _currentValue;
        public float MaxValue => _currentMaxValue;
        public IReadOnlyList<NumericModifier> Modifiers => _modifiers;

        public event System.Action OnChangeMaxValue;
        public event System.Action OnChangeCurrent;

        [SerializeField, HideInInspector] private float _currentMaxValue = 0;
        [SerializeField, HideInInspector] private float _currentValue = 0;
        [SerializeField, Min(0f)] private float _maxValue = 0;

        private readonly List<NumericModifier> _modifiers = new();

        public StatusComponent(float maxValue, float value)
        {
            _maxValue = maxValue;
            _currentMaxValue = _maxValue;
            _currentValue = Mathf.Clamp(value, 0, maxValue);
        }
        /// <summary>
        /// Изменяет current
        /// </summary>
        /// <param name="modifier"></param>
        public void InteractWithCurrent(in NumericModifier modifier)
        {
            _currentValue += modifier;
            _currentValue = Mathf.Clamp(_currentValue, 0, _currentMaxValue);
            OnChangeCurrent?.Invoke();
        }
        /// <summary>
        /// Добавление модификатора увеличивает только максимальное значение
        /// </summary>
        /// <param name="modifier"></param>
        public void AddModifier(in NumericModifier modifier)
        {
            _modifiers.Add(modifier);

            float modifierValue = _currentMaxValue + modifier;
            _currentMaxValue = modifierValue <= 0 ? 0 : modifierValue;

            OnChangeMaxValue?.Invoke();
        }
        public bool RemoveModifier(in NumericModifier modifier)
        {
            if (_modifiers.Remove(modifier))
            {
                _currentMaxValue = _maxValue;
                for (int index = 0; index < _modifiers.Count; index++)
                {
                    _currentMaxValue += _modifiers[index];
                }
                _currentMaxValue = _currentMaxValue <= 0 ? 0 : _currentMaxValue;
                return true;
            }
            return false;
        }
    }
}
