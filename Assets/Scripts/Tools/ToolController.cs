using System;
using System.Collections.Generic;
using InventorySystem;
using Managers;
using UnityEngine;

namespace Tools
{ 
    public class ToolController : MonoBehaviour
    {
        private Tool _currentTool;
        private List<Tool> _availableTools = new List<Tool>();
        private int _currentToolIndex = -1;

        private Inventory _inventory;

        // Dictionary to map tool types to their corresponding sprites
        [Header("Tool Sprites")]
        public GameObject platformizerSprite;
        public GameObject grapplerSprite;
        public GameObject jetpackSprite;

        private readonly Dictionary<Type, GameObject> _toolSprites = new Dictionary<Type, GameObject>();

        private void Awake()
        {
            // Initialize the inventory
            _inventory = GetComponent<Inventory>();
            if (_inventory == null)
            {
                _inventory = gameObject.AddComponent<Inventory>();
            }

            // Initialize the tool sprites dictionary
            InitializeToolSprites();
        }

        private void Start()
        {
            // Add Platformizer to inventory and select it
            _inventory.AddTool<Platformizer>();
            UpdateAvailableTools();

            // Select the Platformizer as the default tool
            _currentToolIndex = 0;
            SelectTool(_currentToolIndex);
        }

        private void OnEnable()
        {
            // Ensure GamePlayEvents instance exists
            if (GamePlayEvents.instance == null)
            {
                Debug.LogError("GamePlayEvents instance not found in the scene.");
                return;
            }

            // Subscribe to gameplay events
            GamePlayEvents.instance.OnSwitchTool += HandleSwitchTool;
            GamePlayEvents.instance.OnUseTool += HandleUseTool;
            GamePlayEvents.instance.OnPlacePlatform += HandleUseTool;
            GamePlayEvents.instance.OnSwitchPlatform += HandleSwitchPlatform;
        }

        private void OnDisable()
        {
            if (GamePlayEvents.instance != null)
            {
                // Unsubscribe from gameplay events
                GamePlayEvents.instance.OnSwitchTool -= HandleSwitchTool;
                GamePlayEvents.instance.OnUseTool -= HandleUseTool;
                GamePlayEvents.instance.OnPlacePlatform -= HandleUseTool; 
                GamePlayEvents.instance.OnSwitchPlatform -= HandleSwitchPlatform;
            }
        }

        private void Update()
        {
            _currentTool?.UpdateTool();
        }

        /// <summary>
        /// Initializes the dictionary mapping tool types to their sprites.
        /// </summary>
        private void InitializeToolSprites()
        {
            _toolSprites.Add(typeof(Platformizer), platformizerSprite);
            _toolSprites.Add(typeof(Grappler), grapplerSprite);
            _toolSprites.Add(typeof(Jetpack), jetpackSprite);

            // Hide all tool sprites at the start
            foreach (var sprite in _toolSprites.Values)
            {
                sprite.SetActive(false);
            }
        }
        
        /// <summary>
        /// Checks if the currently selected tool is of type T.
        /// </summary>
        public bool IsSelectedTool<T>() where T : Tool
        {
            return _currentTool != null && _currentTool is T;
        }

        /// <summary>
        /// Handles the tool switching logic when the OnSwitchTool event is triggered.
        /// </summary>
        private void HandleSwitchTool()
        {
            UpdateAvailableTools();

            if (_availableTools.Count <= 1)
            {
                Debug.Log("No other tools available.");
                return;
            }

            // Deselect current tool
            _currentTool?.OnDeselect();

            // Cycle to the next tool
            _currentToolIndex = (_currentToolIndex + 1) % _availableTools.Count;
            SelectTool(_currentToolIndex);
        }

        /// <summary>
        /// Updates the list of available tools from the inventory.
        /// </summary>
        private void UpdateAvailableTools()
        {
            _availableTools = _inventory.GetTools();

            // Adjust current tool index if necessary
            if (_currentToolIndex >= _availableTools.Count)
            {
                _currentToolIndex = _availableTools.Count - 1;
            }
        }

        /// <summary>
        /// Selects a tool based on the provided index.
        /// </summary>
        private void SelectTool(int index)
        {
            // Deselect current tool if any
            _currentTool?.OnDeselect();

            // Hide all tool sprites
            foreach (var sprite in _toolSprites.Values)
            {
                sprite.SetActive(false);
            }

            // Select new tool
            if (index >= 0 && index < _availableTools.Count)
            {
                _currentTool = _availableTools[index];
                _currentTool.OnSelect();

                Debug.Log($"Selected tool: {_currentTool.toolName}");

                // Show the sprite corresponding to the selected tool
                Type toolType = _currentTool.GetType();
                if (_toolSprites.TryGetValue(toolType, out var sprite))
                {
                    sprite.SetActive(true);
                }
            }
            else
            {
                _currentTool = null;
                Debug.Log("No tool selected.");
            }
        }

        /// <summary>
        /// Handles the usage of the current tool when the OnUseTool or OnPlacePlatform event is triggered.
        /// </summary>
        private void HandleUseTool()
        {
            if (_currentTool != null)
            {
                _currentTool.Use();

                // After use, check if the tool still exists (consumable tools might remove themselves)
                UpdateAvailableTools();

                if (!_availableTools.Contains(_currentTool))
                {
                    Debug.Log("Current tool has been consumed.");

                    // Hide the sprite of the consumed tool
                    if (_toolSprites.ContainsKey(_currentTool.GetType()))
                    {
                        _toolSprites[_currentTool.GetType()].SetActive(false);
                    }

                    // Select the next available tool
                    if (_availableTools.Count > 0)
                    {
                        _currentToolIndex = 0; // Reset to first tool
                        SelectTool(_currentToolIndex);
                    }
                    else
                    {
                        _currentTool = null;
                        Debug.Log("No tools available.");
                    }
                }
            }
            else
            {
                Debug.Log("No tool selected.");
            }
        }
        
        /// <summary>
        /// Handles platform switching when the OnSwitchPlatform event is triggered.
        /// </summary>
        private void HandleSwitchPlatform(int platformNumber)
        {
            if (_currentTool is Platformizer platformizer)
            {
                platformizer.SwitchPlatform(platformNumber - 1);
            }
            else
            {
                Debug.Log("Current tool is not Platformizer.");
            }
        }
    }
}