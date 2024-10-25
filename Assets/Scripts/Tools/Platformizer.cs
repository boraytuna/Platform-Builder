using System;
using System.Collections.Generic;
using UnityEngine;
using InventorySystem;
using Managers;
using UnityEngine.InputSystem;

namespace Tools
{
    public class Platformizer : Tool
    {
        private int _currentPlatformIndex;
        private List<PlatformType> _availablePlatforms = new List<PlatformType>();

        private Inventory _inventory;

        // Mapping of PlatformType to Platform Prefab
        [Tooltip("Assign platform types and their corresponding prefabs here.")]
        public List<PlatformMapping> platformMappings = new List<PlatformMapping>();
        private readonly Dictionary<PlatformType, GameObject> _platformPrefabs = new Dictionary<PlatformType, GameObject>();

        // Placement variables
        [Tooltip("Radius around the player where platforms can be placed.")]
        public float placementRadius = 5f; // Radius around the player where platforms can be placed

        private GameObject _placementPreviewInstance;

        private Camera _mainCamera;

        // Visual indicator for placement radius
        [Tooltip("Assign a circular sprite to visualize the placement radius.")]
        public Sprite placementRadiusSprite; // Assign this in the Inspector
        private GameObject _placementRadiusIndicator;
        private SpriteRenderer _radiusSpriteRenderer;

        // Flag to determine if placement is valid
        private bool _isPlacementValid = true;
        
        private readonly Dictionary<SpriteRenderer, Color> _originalColors = new Dictionary<SpriteRenderer, Color>();

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

            // Subscribe to the OnUseTool event
            GamePlayEvents.instance.OnPlacePlatform += HandlePlacePlatform;
        }

        private void OnDestroy()
        {
            // Unsubscribe from the event
            if (GamePlayEvents.instance != null)
            {
                GamePlayEvents.instance.OnPlacePlatform -= HandlePlacePlatform;
            }
        }

        private void Update()
        {
            UpdatePlacementPreview();
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
                UpdateRadiusIndicatorVisibility();
            }
        }

        /// <summary>
        /// Switches to the platform at the given index.
        /// </summary>
        /// <param name="index">Index of the platform to switch to.</param>
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
                UpdateRadiusIndicatorVisibility();
            }
            // else
            // {
            //     Debug.LogWarning($"{toolName}: Invalid platform index {index}");
            // }
        }

        public override void Use()
        {
            // The placement logic is handled via the OnUseTool event
        }

        private void CreatePlacementPreview()
        {
            UpdatePlacementPreviewInstance();
        }

        /// <summary>
        /// Creates or updates the placement preview instance.
        /// </summary>
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
                // Instantiate the prefab as the placement preview without parenting to the player
                _placementPreviewInstance = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                _placementPreviewInstance.name = "PlacementPreview";
                
                // Disable any colliders and scripts on the preview instance
                foreach (var collider2D in _placementPreviewInstance.GetComponentsInChildren<Collider2D>())
                {
                    collider2D.enabled = false;
                }

                foreach (var component in _placementPreviewInstance.GetComponentsInChildren<MonoBehaviour>())
                {
                    component.enabled = false;
                }

                // Make the preview semi-transparent and store original colors
                foreach (var renderer in _placementPreviewInstance.GetComponentsInChildren<SpriteRenderer>())
                {
                    Color originalColor = renderer.color;
                    originalColor.a = 0.5f;
                    renderer.color = originalColor;

                    // Store the original color if not already stored
                    _originalColors.TryAdd(renderer, originalColor);
                }

                // Assign to Ignore Raycast layer to prevent interference
                _placementPreviewInstance.layer = LayerMask.NameToLayer("Ignore Raycast");
            }
            else
            {
                Debug.LogWarning($"No prefab found for platform type {currentPlatformType}.");
            }
        }

        /// <summary>
        /// Updates the position and color of the placement preview based on the mouse position.
        /// </summary>
        private void UpdatePlacementPreview()
        {
            if (_placementPreviewInstance == null)
            {
                return;
            }

            // Get mouse position in screen space
            Vector3 mouseScreenPosition = Mouse.current.position.ReadValue();

            // Set the z to the distance from the camera to the placement plane (assuming placement plane at z=0)
            mouseScreenPosition.z = Mathf.Abs(_mainCamera.transform.position.z);

            // Convert to world space
            Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(mouseScreenPosition);

            // Calculate direction and distance from the player to the mouse position
            Vector3 direction = worldPosition - transform.position;
            float distance = direction.magnitude;

            // Determine if placement is valid
            _isPlacementValid = distance <= placementRadius;

            // Clamp the direction within the placement radius
            if (distance > placementRadius)
            {
                direction = direction.normalized * placementRadius;
            }

            Vector3 placementPosition = transform.position + direction;

            // Optionally, snap to a grid (e.g., 1 unit)
            placementPosition.x = Mathf.Round(placementPosition.x);
            placementPosition.y = Mathf.Round(placementPosition.y);
            placementPosition.z = 0f; // Ensure z is 0

            // Update the preview's position
            _placementPreviewInstance.transform.position = placementPosition;

            // Update the preview's color based on placement validity
            UpdatePreviewColor();
        }
        
        /// <summary>
        /// Updates the color of the placement preview based on placement validity.
        /// </summary>
        private void UpdatePreviewColor()
        {
            SpriteRenderer[] renderers = _placementPreviewInstance.GetComponentsInChildren<SpriteRenderer>();
            foreach (var renderer in renderers)
            {
                if (_isPlacementValid)
                {
                    // Restore original color with alpha = 0.5f
                    if (_originalColors.TryGetValue(renderer, out Color originalColor))
                    {
                        renderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
                    }
                }
                else
                {
                    // Set color to red with alpha = 0.5f indicating invalid placement
                    renderer.color = new Color(1f, 0f, 0f, 0.5f);
                }
            }
        }

        /// <summary>
        /// Handles the UseTool event triggered by the UseTool input.
        /// </summary>
        private void HandlePlacePlatform()
        {
            if (!isActiveAndEnabled)
            {
                return; // Do not place if the tool is not active
            }

            PlacePlatform();
        }

        /// <summary>
        /// Places the platform at the current placement preview position.
        /// </summary>
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

            // Check if placement is valid
            if (!_isPlacementValid)
            {
                Debug.Log("Cannot place platform outside of placement radius.");
                return;
            }

            // Get the placement position from the preview instance
            Vector3 placementPosition = _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            // Ensure z-axis is set to 0
            placementPosition.z = 0f;

            // Instantiate the platform prefab at the placement position
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
            UpdateRadiusIndicatorVisibility();
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

        /// <summary>
        /// Creates the visual indicator for the placement radius.
        /// </summary>
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

                    // Calculate the scale factor based on the sprite's size
                    float spriteSize = _radiusSpriteRenderer.sprite.bounds.size.x; // Assuming the sprite is square
                    float diameter = placementRadius * 2f;
                    float scaleFactor = diameter / spriteSize;

                    _placementRadiusIndicator.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
                }
                else
                {
                    Debug.LogWarning("Placement radius sprite not assigned.");
                }
            }
        }

        /// <summary>
        /// Destroys the visual indicator for the placement radius.
        /// </summary>
        private void DestroyPlacementRadiusIndicator()
        {
            if (_placementRadiusIndicator != null)
            {
                Destroy(_placementRadiusIndicator);
                _placementRadiusIndicator = null;
                _radiusSpriteRenderer = null;
            }
        }

        /// <summary>
        /// Updates the visibility of the placement radius indicator based on inventory availability.
        /// </summary>
        private void UpdateRadiusIndicatorVisibility()
        {
            if (_radiusSpriteRenderer != null)
            {
                bool hasPlatforms = HasAvailablePlatforms();
                _placementRadiusIndicator.SetActive(hasPlatforms);
            }
        }

        /// <summary>
        /// Checks if the player has any platforms available in the inventory.
        /// </summary>
        /// <returns>True if at least one platform is available; otherwise, false.</returns>
        private bool HasAvailablePlatforms()
        {
            foreach (var platform in _availablePlatforms)
            {
                if (_inventory.GetPlatformQuantity(platform) > 0)
                    return true;
            }
            return false;
        }
    }

    [Serializable]
    public class PlatformMapping
    {
        public PlatformType platformType;
        public GameObject prefab;
    }
}