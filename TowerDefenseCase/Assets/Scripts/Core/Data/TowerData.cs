using UnityEngine;

namespace TowerDefense.Core.Data
{
    [CreateAssetMenu(fileName = "TowerData", menuName = "Tower Defense/Tower Data")]
    public class TowerData : ScriptableObject
    {
        [Header("Tower Stats")]
        public float attackRange = 5f;
        public float attackSpeed = 1f;
        public float damage = 25f;
        public int cost = 100;
        
        [Header("Visual")]
        public Color towerColor = Color.blue;
        public float scale = 1f;
        
        [Header("Projectile")]
        public bool useProjectile = true;
        public float projectileSpeed = 10f;
    }
} 