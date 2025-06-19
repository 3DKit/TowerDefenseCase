using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TowerDefense.Core.Interfaces;
using Zenject;

namespace TowerDefense.UI
{
    public class GameUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI livesText;
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI waveText;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private GameObject gameWonPanel;
        [SerializeField] private TextMeshProUGUI gameOverText;
        [SerializeField] private Button restartButton;
        
        private IGameManager gameManager;
        private IEnemySpawner enemySpawner;
        
        [Inject]
        public void Construct(IGameManager gameManager, IEnemySpawner enemySpawner)
        {
            this.gameManager = gameManager;
            this.enemySpawner = enemySpawner;
        }
        
        private void Start()
        {
            if (restartButton != null)
            {
                restartButton.onClick.AddListener(RestartGame);
            }
            
            UpdateUI();
        }
        
        private void Update()
        {
            UpdateUI();
            
            if (gameManager != null)
            {
                if (gameManager.IsGameOver && !gameOverPanel.activeSelf)
                {
                    ShowGameOver();
                }
            }
        }
        
        private void UpdateUI()
        {
            if (gameManager != null)
            {
                if (livesText != null)
                {
                    livesText.text = $"Lives: {gameManager.PlayerLives}";
                }
                
                if (moneyText != null)
                {
                    moneyText.text = $"Money: ${gameManager.PlayerMoney}";
                }
            }
            
            if (enemySpawner != null)
            {
                if (waveText != null)
                {
                    waveText.text = $"Wave: {enemySpawner.CurrentWave}/{enemySpawner.TotalWaves}";
                }
            }
        }
        
        private void ShowGameOver()
        {
            if (gameManager.IsGameWon)
            {
                gameWonPanel.SetActive(true);
                if (gameOverText != null)
                {
                    gameOverText.text = "Victory!";
                }
            }
            else
            {
                gameOverPanel.SetActive(true);
                if (gameOverText != null)
                {
                    gameOverText.text = "Game Over!";
                }
            }
        }
        
        private void RestartGame()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
            );
        }
    }
} 