using UnityEngine;

namespace TowerDefense.Core.Tower
{
    public class TowerPrefab : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Renderer towerRenderer;
        [SerializeField] private Transform attackPoint;
        
        private void Awake()
        {
            // Ensure we have required components
            if (towerRenderer == null)
                towerRenderer = GetComponent<Renderer>();
            
            // Create attack point if not present
            if (attackPoint == null)
            {
                GameObject attackPointObj = new GameObject("AttackPoint");
                attackPointObj.transform.SetParent(transform);
                attackPointObj.transform.localPosition = Vector3.up * 0.5f;
                attackPoint = attackPointObj.transform;
            }
            
            // Add Tower component if not present
            if (GetComponent<Tower>() == null)
            {
                gameObject.AddComponent<Tower>();
            }
        }
    }
} 