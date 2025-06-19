using System;
using UnityEngine;

namespace TowerDefense.Core.Data
{
    [CreateAssetMenu(fileName = "WaveData", menuName = "Tower Defense/Wave Data")]
    public class WaveData : ScriptableObject
    {
        [Serializable]
        public class EnemySpawnInfo
        {
            public EnemyData enemyData;
            public int count;
            public float spawnDelay = 1f;
        }
        
        [Header("Wave Info")]
        public int waveNumber;
        public float timeBetweenEnemies = 1f;
        public EnemySpawnInfo[] enemies;
    }
} 