using UnityEngine;

namespace TowerDefense.Core.Data
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Tower Defense/Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        [Header("Enemy Stats")]
        public float maxHealth = 100f;
        public float speed = 5f;
        public float damage = 10f;
        
        [Header("Visual")]
        public Color enemyColor = Color.red;
        public float scale = 1f;
        
        [Header("Reward")]
        public int moneyReward = 10;
    }
} 