using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefense.Core.Interfaces;
using TowerDefense.Core.Data;
using Zenject;

namespace TowerDefense.Core.Enemy
{
    public class EnemySpawner : MonoBehaviour, IEnemySpawner
    {
        [Header("Spawn Settings")]
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform targetPoint;
        
        [Header("Wave Data")]
        [SerializeField] private WaveData[] waves;
        
        [Header("Runtime Data")]
        [SerializeField] private int currentWaveIndex = 0;
        [SerializeField] private bool isSpawning = false;
        
        private List<IEnemy> activeEnemies = new List<IEnemy>();
        private Enemy.Factory enemyFactory;
        private IGameManager gameManager;
        
        public int CurrentWave => currentWaveIndex + 1;
        public int TotalWaves => waves.Length;
        public bool IsSpawning => isSpawning;
        
        [Inject]
        public void Construct(Enemy.Factory enemyFactory, IGameManager gameManager)
        {
            this.enemyFactory = enemyFactory;
            this.gameManager = gameManager;
        }
        
        public void StartSpawning()
        {
            if (isSpawning) return;
            
            isSpawning = true;
            currentWaveIndex = 0;
            StartCoroutine(SpawnWaves());
        }
        
        public void StopSpawning()
        {
            isSpawning = false;
            StopAllCoroutines();
        }
        
        public void SpawnWave(int waveNumber)
        {
            if (waveNumber < 0 || waveNumber >= waves.Length) return;
            
            StartCoroutine(SpawnWaveCoroutine(waves[waveNumber]));
        }
        
        public List<IEnemy> GetActiveEnemies()
        {
            // Clean up dead enemies
            activeEnemies.RemoveAll(enemy => enemy == null || enemy.IsDead);
            return activeEnemies;
        }
        
        private IEnumerator SpawnWaves()
        {
            while (currentWaveIndex < waves.Length && isSpawning)
            {
                yield return StartCoroutine(SpawnWaveCoroutine(waves[currentWaveIndex]));
                
                // Wait for all enemies to be defeated
                yield return StartCoroutine(WaitForWaveCompletion());
                
                currentWaveIndex++;
                
                // Check if game is won
                if (currentWaveIndex >= waves.Length)
                {
                    if (gameManager != null)
                    {
                        gameManager.WinGame();
                    }
                    break;
                }
            }
        }
        
        private IEnumerator SpawnWaveCoroutine(WaveData waveData)
        {
            foreach (var enemyInfo in waveData.enemies)
            {
                for (int i = 0; i < enemyInfo.count; i++)
                {
                    if (!isSpawning) yield break;
                    
                    SpawnEnemy(enemyInfo.enemyData);
                    yield return new WaitForSeconds(enemyInfo.spawnDelay);
                }
            }
        }
        
        private IEnumerator WaitForWaveCompletion()
        {
            while (GetActiveEnemies().Count > 0 && isSpawning)
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
        
        private void SpawnEnemy(EnemyData enemyData)
        {
            if (enemyFactory == null || spawnPoint == null || targetPoint == null) return;
            
            var enemy = enemyFactory.Create(enemyData, targetPoint);
            enemy.Initialize(enemyData, targetPoint);
            activeEnemies.Add(enemy);
        }
        
        private void OnDrawGizmos()
        {
            if (spawnPoint != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(spawnPoint.position, 1f);
            }
            
            if (targetPoint != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(targetPoint.position, 1f);
            }
        }
    }
} 