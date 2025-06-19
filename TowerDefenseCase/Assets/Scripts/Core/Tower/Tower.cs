using UnityEngine;
using System.Collections;
using TowerDefense.Core.Interfaces;
using TowerDefense.Core.Data;
using Zenject;

namespace TowerDefense.Core.Tower
{
    public class Tower : MonoBehaviour, ITower
    {
        [Header("Components")]
        [SerializeField] private Renderer towerRenderer;
        [SerializeField] private Transform attackPoint;
        
        [Header("Runtime Data")]
        [SerializeField] private float currentAttackRange;
        [SerializeField] private float currentAttackSpeed;
        [SerializeField] private float currentDamage;
        
        private TowerData towerData;
        private IEnemy currentTarget;
        private bool canAttack = true;
        private float lastAttackTime;
        
        public float AttackRange => currentAttackRange;
        public float AttackSpeed => currentAttackSpeed;
        public float Damage => currentDamage;
        public bool CanAttack => canAttack && Time.time >= lastAttackTime + (1f / currentAttackSpeed);
        
        public void Initialize(TowerData data)
        {
            towerData = data;
            
            currentAttackRange = data.attackRange;
            currentAttackSpeed = data.attackSpeed;
            currentDamage = data.damage;
            
            // Visual setup
            if (towerRenderer != null)
            {
                towerRenderer.material.color = data.towerColor;
            }
            
            transform.localScale = Vector3.one * data.scale;
        }
        
        private void Update()
        {
            if (currentTarget == null || currentTarget.IsDead)
            {
                ClearTarget();
                FindNewTarget();
            }
            else if (CanAttack && IsInRange(GetEnemyPosition(currentTarget)))
            {
                Attack(currentTarget);
            }
        }
        
        private Vector3 GetEnemyPosition(IEnemy enemy)
        {
            return enemy.Transform.position;
        }
        
        public void Attack(IEnemy target)
        {
            if (!CanAttack) return;
            
            lastAttackTime = Time.time;
            
            if (towerData.useProjectile)
            {
                StartCoroutine(FireProjectile(target));
            }
            else
            {
                target.TakeDamage(currentDamage);
            }
        }
        
        private IEnumerator FireProjectile(IEnemy target)
        {
            // Create projectile
            GameObject projectile = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            projectile.transform.position = attackPoint ? attackPoint.position : transform.position;
            projectile.transform.localScale = Vector3.one * 0.3f;
            
            // Set projectile color
            var projectileRenderer = projectile.GetComponent<Renderer>();
            if (projectileRenderer != null)
            {
                projectileRenderer.material.color = Color.yellow;
            }
            
            // Move projectile towards target
            Vector3 targetPosition = GetEnemyPosition(target);
            float distance = Vector3.Distance(projectile.transform.position, targetPosition);
            float timeToTarget = distance / towerData.projectileSpeed;
            
            float elapsedTime = 0f;
            Vector3 startPosition = projectile.transform.position;
            
            while (elapsedTime < timeToTarget && target != null && !target.IsDead)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / timeToTarget;
                projectile.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                yield return null;
            }
            
            // Deal damage if target still exists
            if (target != null && !target.IsDead)
            {
                target.TakeDamage(currentDamage);
            }
            
            // Destroy projectile
            Destroy(projectile);
        }
        
        public void SetTarget(IEnemy target)
        {
            currentTarget = target;
        }
        
        public void ClearTarget()
        {
            currentTarget = null;
        }
        
        public bool IsInRange(Vector3 position)
        {
            return Vector3.Distance(transform.position, position) <= currentAttackRange;
        }
        
        private void FindNewTarget()
        {
            var enemies = FindObjectsOfType<TowerDefense.Core.Enemy.Enemy>();
            IEnemy closestEnemy = null;
            float closestDistance = float.MaxValue;
            
            foreach (var enemy in enemies)
            {
                if (enemy.IsDead) continue;
                
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance <= currentAttackRange && distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }
            
            if (closestEnemy != null)
            {
                SetTarget(closestEnemy);
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, currentAttackRange);
        }
        
        // Zenject Factory
        public class Factory : PlaceholderFactory<TowerData, Tower>
        {
        }
    }
} 