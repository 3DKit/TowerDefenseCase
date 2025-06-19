using System.Collections.Generic;

namespace TowerDefense.Core.Interfaces
{
    public interface IEnemySpawner
    {
        int CurrentWave { get; }
        int TotalWaves { get; }
        bool IsSpawning { get; }
        
        void StartSpawning();
        void StopSpawning();
        void SpawnWave(int waveNumber);
        List<IEnemy> GetActiveEnemies();
    }
} 