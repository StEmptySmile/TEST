using UnityEngine;

namespace Core.Object.Character.Activity
{
    [System.Serializable]
    public struct ActivityComponent
    {
        public TypesActivity Type => _type;
        [SerializeField] private TypesActivity _type;
        /// <summary>
        /// Иногда возвращает null, так как возможна данная активность
        /// не изменяет параметры у анимации
        /// </summary>
        public int? IndexAnimatorControllerParameter
        {
            get
            {
                if(_indexAnimatorControllerParameter < 0)
                {
                    return null;
                }
                return _indexAnimatorControllerParameter;
            }
        }
        [SerializeField] private int _indexAnimatorControllerParameter;

        public ActivityComponent(TypesActivity type, int indexAnimatorControllerParameter = -1)
        {
            _type = type;
            _indexAnimatorControllerParameter = indexAnimatorControllerParameter;
        }
    }
    public enum TypesActivity
    {
        /// <summary>
        /// Используется только в редакторе
        /// </summary>
        None,
        Idle,
        Walk,
        Attack,
        Death
    }
}
