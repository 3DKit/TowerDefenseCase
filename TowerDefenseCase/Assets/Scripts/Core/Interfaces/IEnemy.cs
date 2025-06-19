using UnityEngine;

namespace TowerDefense.Core.Interfaces
{
    public interface IEnemy
    {
        float Health { get; }
        float MaxHealth { get; }
        float Speed { get; }
        float Damage { get; }
        bool IsDead { get; }
        Transform Transform { get; }
        
        void TakeDamage(float damage);
        void Move(Vector3 direction);
        void Die();
    }
} 