using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using InventorySystem;

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

        private Dictionary<Type, GameObject> _toolSprites = new Dictionary<Type, GameObject>();

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
            GamePlayEvents.instance.OnSwitchTool += HandleSwitchTool;
            GamePlayEvents.instance.OnUseTool += HandleUseTool;
            GamePlayEvents.instance.OnSwitchPlatform += HandleSwitchPlatform;
        }

        private void OnDisable()
        {
            if (GamePlayEvents.instance != null)
            {
                GamePlayEvents.instance.OnSwitchTool -= HandleSwitchTool;
                GamePlayEvents.instance.OnUseTool -= HandleUseTool;
                GamePlayEvents.instance.OnSwitchPlatform -= HandleSwitchPlatform;
            }
        }

        private void Update()
        {
            _currentTool?.UpdateTool();
        }

        // Initializes the dictionary mapping tool types to their sprites
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

        private void UpdateAvailableTools()
        {
            _availableTools = _inventory.GetTools();

            // Adjust current tool index if necessary
            if (_currentToolIndex >= _availableTools.Count)
            {
                _currentToolIndex = _availableTools.Count - 1;
            }
        }

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
                if (_toolSprites.ContainsKey(toolType))
                {
                    _toolSprites[toolType].SetActive(true);
                }
            }
            else
            {
                _currentTool = null;
                Debug.Log("No tool selected.");
            }
        }

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
