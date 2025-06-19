using UnityEngine;
using TowerDefense.Core.Data;
using TowerDefense.Core.Interfaces;
using Zenject;

namespace TowerDefense.Core.Tower
{
    public class TowerPlacer : MonoBehaviour
    {
        [Header("Tower Settings")]
        [SerializeField] private TowerData towerData;
        [SerializeField] private LayerMask placementLayer;
        
        [Header("Placement Preview")]
        [SerializeField] private Material validPlacementMaterial;
        [SerializeField] private Material invalidPlacementMaterial;
        
        private GameObject placementPreview;
        private bool isPlacing = false;
        private Camera mainCamera;
        private IGameManager gameManager;
        private Tower.Factory towerFactory;
        
        [Inject]
        public void Construct(IGameManager gameManager, Tower.Factory towerFactory)
        {
            this.gameManager = gameManager;
            this.towerFactory = towerFactory;
        }
        
        private void Start()
        {
            mainCamera = Camera.main;
            CreatePlacementPreview();
        }
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !isPlacing)
            {
                StartPlacement();
            }
            
            if (isPlacing)
            {
                UpdatePlacementPreview();
                
                if (Input.GetMouseButtonDown(0))
                {
                    TryPlaceTower();
                }
                
                if (Input.GetMouseButtonDown(1))
                {
                    CancelPlacement();
                }
            }
        }
        
        private void StartPlacement()
        {
            if (gameManager == null || !gameManager.CanAfford(towerData.cost)) return;
            
            isPlacing = true;
            placementPreview.SetActive(true);
        }
        
        private void UpdatePlacementPreview()
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, placementLayer))
            {
                placementPreview.transform.position = hit.point;
                
                // Check if placement is valid
                bool isValidPlacement = IsValidPlacement(hit.point);
                UpdatePreviewMaterial(isValidPlacement);
            }
        }
        
        private void TryPlaceTower()
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, placementLayer))
            {
                if (IsValidPlacement(hit.point) && gameManager.CanAfford(towerData.cost))
                {
                    PlaceTower(hit.point);
                    gameManager.SpendMoney(towerData.cost);
                }
            }
            
            CancelPlacement();
        }
        
        private void PlaceTower(Vector3 position)
        {
            var tower = towerFactory.Create(towerData);
            tower.transform.position = position;
            tower.Initialize(towerData);
        }
        
        private void CancelPlacement()
        {
            isPlacing = false;
            placementPreview.SetActive(false);
        }
        
        private bool IsValidPlacement(Vector3 position)
        {
            // Check if there's already a tower nearby
            Collider[] colliders = Physics.OverlapSphere(position, 1f);
            foreach (var collider in colliders)
            {
                if (collider.GetComponent<Tower>() != null)
                {
                    return false;
                }
            }
            
            return true;
        }
        
        private void CreatePlacementPreview()
        {
            placementPreview = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            placementPreview.transform.localScale = new Vector3(1f, 0.1f, 1f);
            placementPreview.SetActive(false);
            
            // Remove collider from preview
            Destroy(placementPreview.GetComponent<Collider>());
            
            // Set initial material
            var renderer = placementPreview.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = validPlacementMaterial;
            }
        }
        
        private void UpdatePreviewMaterial(bool isValid)
        {
            var renderer = placementPreview.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = isValid ? validPlacementMaterial : invalidPlacementMaterial;
            }
        }
    }
} 