namespace TowerDefense.Core.Interfaces
{
    public interface IGameManager
    {
        int PlayerLives { get; }
        int PlayerMoney { get; }
        bool IsGameOver { get; }
        bool IsGameWon { get; }
        
        void StartGame();
        void EndGame();
        void LoseLife();
        void AddMoney(int amount);
        void SpendMoney(int amount);
        bool CanAfford(int cost);
        void WinGame();
    }
} 