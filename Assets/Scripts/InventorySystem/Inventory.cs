using System.Collections.Generic;
using System;
using Tools;
using UnityEngine;

namespace InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        // Dictionary to store tools and their quantities
        private readonly Dictionary<Type, int> _toolQuantities = new Dictionary<Type, int>();

        // Dictionary to store platform types and their quantities
        private readonly Dictionary<PlatformType, int> _platformQuantities = new Dictionary<PlatformType, int>();

        /// <summary>
        /// Adds a tool to the inventory.
        /// </summary>
        public void AddTool<T>(int quantity = 1) where T : Tools.Tool
        {
            var type = typeof(T);
            if (!_toolQuantities.TryAdd(type, quantity))
            {
                _toolQuantities[type] += quantity;
            }
            else
            {
                // Ensure the Tool component exists on the GameObject
                if (GetComponent(type) == null)
                {
                    gameObject.AddComponent(type);
                }
            }

            Debug.Log($"Added {quantity} {type.Name}(s) to inventory.");
        }

        /// <summary>
        /// Checks if the inventory contains at least one of the specified tool.
        /// </summary>
        public bool HasTool<T>() where T : Tools.Tool
        {
            var type = typeof(T);
            return _toolQuantities.ContainsKey(type) && _toolQuantities[type] > 0;
        }

        /// <summary>
        /// Decreases the quantity of a tool by one.
        /// </summary>
        public void ConsumeTool<T>() where T : Tools.Tool
        {
            var type = typeof(T);
            if (_toolQuantities.ContainsKey(type))
            {
                _toolQuantities[type]--;

                if (_toolQuantities[type] <= 0)
                {
                    // Prevent Platformizer from being removed
                    if (type != typeof(Platformizer))
                    {
                        _toolQuantities.Remove(type);
                        Debug.Log($"{type.Name} removed from inventory after use.");
                    }
                    else
                    {
                        _toolQuantities[type] = 1; // Ensure Platformizer quantity stays at least 1
                        Debug.Log($"{type.Name} quantity remains at 1 (cannot be removed).");
                    }
                }
            }
        }

        /// <summary>
        /// Adds a platform type to the inventory.
        /// </summary>
        public void AddPlatform(PlatformType platformType, int quantity = 1)
        {
            if (!_platformQuantities.TryAdd(platformType, quantity))
            {
                _platformQuantities[platformType] += quantity;
            }

            Debug.Log($"Added {quantity} {platformType} platform(s) to inventory.");
        }

        /// <summary>
        /// Checks if the inventory contains at least one of the specified platform type.
        /// </summary>
        public bool HasPlatform(PlatformType platformType)
        {
            return _platformQuantities.ContainsKey(platformType) && _platformQuantities[platformType] > 0;
        }

        /// <summary>
        /// Decreases the quantity of a platform by one.
        /// </summary>
        public void ConsumePlatform(PlatformType platformType)
        {
            if (_platformQuantities.ContainsKey(platformType))
            {
                _platformQuantities[platformType]--;
                if (_platformQuantities[platformType] <= 0)
                {
                    _platformQuantities.Remove(platformType);
                    Debug.Log($"{platformType} platform depleted from inventory.");
                }
            }
        }

        /// <summary>
        /// Returns the list of tools in the inventory.
        /// </summary>
        public List<Tools.Tool> GetTools()
        {
            var tools = new List<Tools.Tool>();
            foreach (var kvp in _toolQuantities)
            {
                if (kvp.Value > 0)
                {
                    // Get the Tool component
                    Tools.Tool tool = GetComponent(kvp.Key) as Tools.Tool;
                    if (tool != null)
                    {
                        tools.Add(tool);
                    }
                    else
                    {
                        Debug.LogWarning($"Tool component of type {kvp.Key.Name} is missing.");
                    }
                }
            }
            return tools;
        }

        /// <summary>
        /// Returns the list of available platform types.
        /// </summary>
        public List<PlatformType> GetPlatforms()
        {
            return new List<PlatformType>(_platformQuantities.Keys);
        }

        /// <summary>
        /// Gets the quantity of a specific platform type.
        /// </summary>
        public int GetPlatformQuantity(PlatformType platformType)
        {
            return _platformQuantities.GetValueOrDefault(platformType, 0);
        }

        /// <summary>
        /// Gets the quantity of a specific tool type.
        /// </summary>
        public int GetToolQuantity<T>() where T : Tools.Tool
        {
            var type = typeof(T);
            return _toolQuantities.GetValueOrDefault(type, 0);
        }
    }

    /// <summary>
    /// Enum for different platform types.
    /// </summary>
    public enum PlatformType
    {
        BrickGround,
        BrickWall,
        SteelGround,
        SteelWall,
        GlassGround,
        GlassWall,
        WoodGround,
        WoodWall,
        ConcreteGround,
        ConcreteWall,
    }
}