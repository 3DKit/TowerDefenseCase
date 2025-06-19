using UnityEngine;

namespace TowerDefense.Core.Enemy
{
    public class EnemyPrefab : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Renderer enemyRenderer;
        [SerializeField] private Collider enemyCollider;
        
        private void Awake()
        {
            // Ensure we have required components
            if (enemyRenderer == null)
                enemyRenderer = GetComponent<Renderer>();
            
            if (enemyCollider == null)
                enemyCollider = GetComponent<Collider>();
            
            // Add Enemy component if not present
            if (GetComponent<Enemy>() == null)
            {
                gameObject.AddComponent<Enemy>();
            }
        }
    }
} 