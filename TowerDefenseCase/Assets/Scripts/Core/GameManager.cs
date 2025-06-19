using UnityEngine;
using TowerDefense.Core.Interfaces;
using Zenject;

namespace TowerDefense.Core
{
    public class GameManager : MonoBehaviour, IGameManager
    {
        [Header("Game Settings")]
        [SerializeField] private int startingLives = 10;
        [SerializeField] private int startingMoney = 500;
        
        [Header("Runtime Data")]
        [SerializeField] private int playerLives;
        [SerializeField] private int playerMoney;
        [SerializeField] private bool isGameOver = false;
        [SerializeField] private bool isGameWon = false;
        
        private IEnemySpawner enemySpawner;
        
        public int PlayerLives => playerLives;
        public int PlayerMoney => playerMoney;
        public bool IsGameOver => isGameOver;
        public bool IsGameWon => isGameWon;
        
        [Inject]
        public void Construct(IEnemySpawner enemySpawner, [Inject(Id = "StartingLives")] int startingLives, [Inject(Id = "StartingMoney")] int startingMoney)
        {
            this.enemySpawner = enemySpawner;
            this.startingLives = startingLives;
            this.startingMoney = startingMoney;
        }
        
        private void Start()
        {
            StartGame();
        }
        
        public void StartGame()
        {
            playerLives = startingLives;
            playerMoney = startingMoney;
            isGameOver = false;
            isGameWon = false;
            
            // Start enemy spawning
            if (enemySpawner != null)
            {
                enemySpawner.StartSpawning();
            }
        }
        
        public void EndGame()
        {
            isGameOver = true;
            
            if (enemySpawner != null)
            {
                enemySpawner.StopSpawning();
            }
        }
        
        public void LoseLife()
        {
            if (isGameOver) return;
            
            playerLives--;
            
            if (playerLives <= 0)
            {
                EndGame();
            }
        }
        
        public void AddMoney(int amount)
        {
            if (isGameOver) return;
            
            playerMoney += amount;
        }
        
        public void SpendMoney(int amount)
        {
            if (isGameOver) return;
            
            if (playerMoney >= amount)
            {
                playerMoney -= amount;
            }
        }
        
        public void WinGame()
        {
            isGameWon = true;
            EndGame();
        }
        
        public bool CanAfford(int cost)
        {
            return playerMoney >= cost;
        }
    }
} 