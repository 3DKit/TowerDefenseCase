using UnityEngine;
using TowerDefense.Core.Interfaces;
using TowerDefense.Core.Data;
using Zenject;

namespace TowerDefense.Core.Enemy
{
    public class Enemy : MonoBehaviour, IEnemy
    {
        [Header("Components")]
        [SerializeField] private Renderer enemyRenderer;
        [SerializeField] private Transform target;
        
        [Header("Runtime Data")]
        [SerializeField] private float currentHealth;
        [SerializeField] private float currentSpeed;
        [SerializeField] private float currentDamage;
        
        private EnemyData enemyData;
        private bool isDead;
        private IGameManager gameManager;
        
        public float Health => currentHealth;
        public float MaxHealth => enemyData?.maxHealth ?? 100f;
        public float Speed => currentSpeed;
        public float Damage => currentDamage;
        public bool IsDead => isDead;
        public Transform Transform => transform;
        
        [Inject]
        public void Construct(IGameManager gameManager)
        {
            this.gameManager = gameManager;
        }
        
        public void Initialize(EnemyData data, Transform targetTransform)
        {
            enemyData = data;
            target = targetTransform;
            
            currentHealth = data.maxHealth;
            currentSpeed = data.speed;
            currentDamage = data.damage;
            
            // Visual setup
            if (enemyRenderer != null)
            {
                enemyRenderer.material.color = data.enemyColor;
            }
            
            transform.localScale = Vector3.one * data.scale;
        }
        
        private void Update()
        {
            if (isDead || target == null) return;
            
            Move((target.position - transform.position).normalized);
        }
        
        public void TakeDamage(float damage)
        {
            if (isDead) return;
            
            currentHealth -= damage;
            
            if (currentHealth <= 0)
            {
                Die();
            }
        }
        
        public void Move(Vector3 direction)
        {
            if (isDead) return;
            
            transform.position += direction * currentSpeed * Time.deltaTime;
        }
        
        public void Die()
        {
            if (isDead) return;
            
            isDead = true;
            
            // Notify game manager for money reward
            if (gameManager != null)
            {
                gameManager.AddMoney(enemyData.moneyReward);
            }
            
            // Destroy the enemy
            Destroy(gameObject);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("PlayerBase"))
            {
                // Damage the player base
                if (gameManager != null)
                {
                    gameManager.LoseLife();
                }
                
                Die();
            }
        }
        
        // Zenject Factory
        public class Factory : PlaceholderFactory<EnemyData, Transform, Enemy>
        {
        }
    }
} 