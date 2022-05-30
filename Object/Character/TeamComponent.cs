using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Object.Character
{
    [CreateAssetMenu(fileName = "TeamComponent", menuName = "Tools/Create/Team")]
    public sealed class TeamComponent : ScriptableObject
    {
        public IReadOnlyList<TeamComponent> Enemies => _enemies;
        [SerializeField] private List<TeamComponent> _enemies = null;
    }
}
