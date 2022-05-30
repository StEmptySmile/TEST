using UnityEngine;

namespace Core.Object.Character.Activity
{
    [System.Serializable]
    public struct ActivityComponent
    {
        public TypesActivity Type => _type;
        [SerializeField] private TypesActivity _type;
        /// <summary>
        /// ������ ���������� null, ��� ��� �������� ������ ����������
        /// �� �������� ��������� � ��������
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
        /// ������������ ������ � ���������
        /// </summary>
        None,
        Idle,
        Walk,
        Attack,
        Death
    }
}
