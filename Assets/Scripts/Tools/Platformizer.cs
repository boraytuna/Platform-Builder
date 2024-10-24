using System;
using System.Collections.Generic;
using UnityEngine;
using InventorySystem;

namespace Tools
{
    public class Platformizer : Tool
    {
        private int _currentPlatformIndex;
        private List<PlatformType> _availablePlatforms = new List<PlatformType>();

        private Inventory _inventory;

        // Mapping of PlatformType to Platform Prefab
        public List<PlatformMapping> platformMappings = new List<PlatformMapping>();
        private readonly Dictionary<PlatformType, GameObject> _platformPrefabs = new Dictionary<PlatformType, GameObject>();

        // Placement variables
        public float placementRadius = 5f; // Radius around the player where platforms can be placed
        public LayerMask placementLayerMask; // Layers to consider when placing platforms

        private GameObject _placementPreviewInstance;

        private Camera _mainCamera;

        // Visual indicator for placement radius
        public Sprite placementRadiusSprite; // Assign this in the Inspector
        private GameObject _placementRadiusIndicator;
        private SpriteRenderer _radiusSpriteRenderer;

        private void Awake()
        {
            toolName = "Platformizer";
        }

        private void Start()
        {
            _inventory = GetComponent<Inventory>();
            _mainCamera = Camera.main;

            InitializePlatformPrefabs();
            UpdateAvailablePlatforms();
            CreatePlacementPreview();
        }

        private void Update()
        {
            UpdatePlacementPreview();
            HandlePlacementInput();
        }

        private void InitializePlatformPrefabs()
        {
            // Populate the dictionary with platform prefabs
            foreach (var mapping in platformMappings)
            {
                if (!_platformPrefabs.ContainsKey(mapping.platformType) && mapping.prefab != null)
                {
                    _platformPrefabs.Add(mapping.platformType, mapping.prefab);
                }
            }
        }

        private void UpdateAvailablePlatforms()
        {
            if (_inventory != null)
            {
                _availablePlatforms = _inventory.GetPlatforms();

                // Adjust current platform index if necessary
                if (_availablePlatforms.Count == 0)
                {
                    _currentPlatformIndex = -1;
                }
                else
                {
                    _currentPlatformIndex = Mathf.Clamp(_currentPlatformIndex, 0, _availablePlatforms.Count - 1);
                }

                UpdatePlacementPreviewInstance();
            }
        }

        public void SwitchPlatform(int index)
        {
            UpdateAvailablePlatforms();

            if (_availablePlatforms.Count == 0)
            {
                Debug.Log("No platforms available.");
                return;
            }

            if (index >= 0 && index < _availablePlatforms.Count)
            {
                _currentPlatformIndex = index;
                PlatformType platformType = _availablePlatforms[_currentPlatformIndex];
                Debug.Log($"{toolName}: Switched to platform {platformType} (Quantity: {_inventory.GetPlatformQuantity(platformType)})");

                UpdatePlacementPreviewInstance();
            }
            else
            {
                Debug.LogWarning($"{toolName}: Invalid platform index {index}");
            }
        }

        public override void Use()
        {
            // The placement logic is handled in Update(), so we don't need to implement Use() here.
        }

        private void CreatePlacementPreview()
        {
            UpdatePlacementPreviewInstance();
        }

        private void UpdatePlacementPreviewInstance()
        {
            // Destroy the old placement preview instance if it exists
            if (_placementPreviewInstance != null)
            {
                Destroy(_placementPreviewInstance);
                _placementPreviewInstance = null;
            }

            if (_availablePlatforms.Count == 0 || _currentPlatformIndex < 0 || _currentPlatformIndex >= _availablePlatforms.Count)
            {
                return;
            }

            PlatformType currentPlatformType = _availablePlatforms[_currentPlatformIndex];

            if (_platformPrefabs.TryGetValue(currentPlatformType, out GameObject prefab))
            {
                // Instantiate the prefab as the placement preview
                _placementPreviewInstance = Instantiate(prefab);
                _placementPreviewInstance.name = "PlacementPreview";
                _placementPreviewInstance.transform.SetParent(transform);

                // Disable any colliders and scripts on the preview instance
                foreach (var collider2D1 in _placementPreviewInstance.GetComponentsInChildren<Collider2D>())
                {
                    collider2D1.enabled = false;
                }

                foreach (var component in _placementPreviewInstance.GetComponentsInChildren<MonoBehaviour>())
                {
                    component.enabled = false;
                }

                // Make the preview semi-transparent
                foreach (var renderer in _placementPreviewInstance.GetComponentsInChildren<SpriteRenderer>())
                {
                    Color color = renderer.color;
                    color.a = 0.5f;
                    renderer.color = color;
                }
            }
            else
            {
                Debug.LogWarning($"No prefab found for platform type {currentPlatformType}.");
            }
        }

        private void UpdatePlacementPreview()
        {
            if (_placementPreviewInstance == null)
            {
                return;
            }

            // Get mouse position in world space
            Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            // Calculate the direction and distance from the player to the mouse position
            Vector3 direction = mousePosition - transform.position;
            float distance = direction.magnitude;

            // Clamp the distance to the placement radius
            if (distance > placementRadius)
            {
                direction = direction.normalized * placementRadius;
                distance = placementRadius;
            }

            // Set the position of the placement preview
            Vector3 placementPosition = transform.position + direction;

            // Optionally, snap to a grid
            placementPosition.x = Mathf.Round(placementPosition.x);
            placementPosition.y = Mathf.Round(placementPosition.y);

            _placementPreviewInstance.transform.position = placementPosition;
        }

        private void HandlePlacementInput()
        {
            if (Input.GetMouseButtonDown(0)) // Left mouse button
            {
                PlacePlatform();
            }
        }

        private void PlacePlatform()
        {
            UpdateAvailablePlatforms();

            if (_availablePlatforms.Count == 0 || _currentPlatformIndex < 0)
            {
                Debug.Log("No platforms available to build.");
                return;
            }

            PlatformType platformType = _availablePlatforms[_currentPlatformIndex];
            int quantity = _inventory.GetPlatformQuantity(platformType);

            if (quantity <= 0)
            {
                Debug.Log($"No more {platformType} platforms left.");
                UpdateAvailablePlatforms(); // Update the list after removal
                return;
            }

            if (!_platformPrefabs.TryGetValue(platformType, out GameObject platformPrefab))
            {
                Debug.LogWarning($"No prefab assigned for platform type {platformType}.");
                return;
            }

            // Get the placement position
            Vector3 placementPosition = _placementPreviewInstance.transform.position;

            // Instantiate the platform prefab
            Instantiate(platformPrefab, placementPosition, Quaternion.identity);

            Debug.Log($"{toolName}: Built platform {platformType} at {placementPosition}.");

            // Consume one platform
            _inventory.ConsumePlatform(platformType);

            // Update available platforms after consumption
            UpdateAvailablePlatforms();
        }

        public override void OnSelect()
        {
            base.OnSelect();

            if (_placementPreviewInstance != null)
            {
                _placementPreviewInstance.SetActive(true);
            }
            else
            {
                UpdatePlacementPreviewInstance();
            }

            CreatePlacementRadiusIndicator();
        }

        public override void OnDeselect()
        {
            base.OnDeselect();
            if (_placementPreviewInstance != null)
            {
                _placementPreviewInstance.SetActive(false);
            }
            DestroyPlacementRadiusIndicator();
        }

        private void CreatePlacementRadiusIndicator()
        {
            if (_placementRadiusIndicator == null)
            {
                _placementRadiusIndicator = new GameObject("PlacementRadiusIndicator");
                _placementRadiusIndicator.transform.SetParent(transform);
                _placementRadiusIndicator.transform.localPosition = Vector3.zero;

                _radiusSpriteRenderer = _placementRadiusIndicator.AddComponent<SpriteRenderer>();
                _radiusSpriteRenderer.color = new Color(1f, 1f, 1f, 0.2f); // Semi-transparent

                if (placementRadiusSprite != null)
                {
                    _radiusSpriteRenderer.sprite = placementRadiusSprite;
                }
                else
                {
                    Debug.LogWarning("Placement radius sprite not assigned.");
                }

                // Scale the sprite to match the placement radius
                float diameter = placementRadius * 2f;
                _placementRadiusIndicator.transform.localScale = new Vector3(diameter, diameter, 1f);
            }
        }

        private void DestroyPlacementRadiusIndicator()
        {
            if (_placementRadiusIndicator != null)
            {
                Destroy(_placementRadiusIndicator);
                _placementRadiusIndicator = null;
                _radiusSpriteRenderer = null;
            }
        }
    }

    [Serializable]
    public class PlatformMapping
    {
        public PlatformType platformType;
        public GameObject prefab;
    }
}