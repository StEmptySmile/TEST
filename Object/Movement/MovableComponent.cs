using UnityEngine.AI;
using UnityEngine;

namespace Core.Object.Movement
{
    [System.Serializable]
    public struct MovableComponent
    {
        public NavMeshAgent Body => _body;
        [SerializeField] private NavMeshAgent _body;
        public float Speed => _speed;
        [SerializeField] private float _speed;
    }
}
