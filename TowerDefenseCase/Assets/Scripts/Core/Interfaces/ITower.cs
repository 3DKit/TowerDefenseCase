using UnityEngine;

namespace TowerDefense.Core.Interfaces
{
    public interface ITower
    {
        float AttackRange { get; }
        float AttackSpeed { get; }
        float Damage { get; }
        bool CanAttack { get; }
        
        void Attack(IEnemy target);
        void SetTarget(IEnemy target);
        void ClearTarget();
        bool IsInRange(Vector3 position);
    }
} 