using UnityEngine;
using Zenject;
using TowerDefense.Core.Interfaces;
using TowerDefense.Core;
using TowerDefense.Core.Enemy;
using TowerDefense.Core.Tower;
using TowerDefense.Core.Data;
using TowerDefense.UI;

namespace TowerDefense.DI
{
    public class GameInstaller : MonoInstaller
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private GameObject towerPrefab;
        
        [Header("Data")]
        [SerializeField] private EnemyData enemyData;
        [SerializeField] private TowerData towerData;
        [SerializeField] private WaveData[] waveData;
        
        [Header("Game Settings")]
        [SerializeField] private int startingLives = 10;
        [SerializeField] private int startingMoney = 500;
        
        public override void InstallBindings()
        {
            // Bind data
            Container.BindInstance(enemyData).WhenInjectedInto<Enemy>();
            Container.BindInstance(towerData).WhenInjectedInto<Tower>();
            Container.BindInstance(waveData).WhenInjectedInto<EnemySpawner>();
            
            // Bind prefabs
            Container.BindFactory<EnemyData, Transform, Enemy, Enemy.Factory>()
                .FromComponentInNewPrefab(enemyPrefab);
            
            Container.BindFactory<TowerData, Tower, Tower.Factory>()
                .FromComponentInNewPrefab(towerPrefab);
            
            // Bind game manager
            Container.Bind<IGameManager>().To<GameManager>().FromComponentInHierarchy().AsSingle();
            
            // Bind enemy spawner
            Container.Bind<IEnemySpawner>().To<EnemySpawner>().FromComponentInHierarchy().AsSingle();
            
            // Bind game settings
            Container.BindInstance(startingLives).WithId("StartingLives");
            Container.BindInstance(startingMoney).WithId("StartingMoney");
            
            // Bind UI
            Container.Bind<GameUI>().FromComponentInHierarchy().AsSingle();
        }
    }
} 