using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        // Dictionary to store tools and their quantities
        private readonly Dictionary<System.Type, int> _toolQuantities = new Dictionary<System.Type, int>();

        // Dictionary to store platform types and their quantities
        private readonly Dictionary<PlatformType, int> _platformQuantities = new Dictionary<PlatformType, int>();

        // Adds a tool to the inventory
        public void AddTool<T>(int quantity = 1) where T : Tool
        {
            var type = typeof(T);
            if (!_toolQuantities.TryAdd(type, quantity))
            {
                _toolQuantities[type] += quantity;
            }

            Debug.Log($"Added {quantity} {type.Name}(s) to inventory.");
        }

        // Checks if the inventory contains a tool
        public bool HasTool<T>() where T : Tool
        {
            var type = typeof(T);
            return _toolQuantities.ContainsKey(type) && _toolQuantities[type] > 0;
        }

        // Decreases the quantity of a tool by one
        public void ConsumeTool<T>() where T : Tool
        {
            var type = typeof(T);
            if (_toolQuantities.ContainsKey(type))
            {
                _toolQuantities[type]--;
                if (_toolQuantities[type] <= 0)
                {
                    _toolQuantities.Remove(type);
                    Debug.Log($"{type.Name} removed from inventory after use.");
                }
            }
        }

        // Adds a platform type to the inventory
        public void AddPlatform(PlatformType platformType, int quantity = 1)
        {
            if (!_platformQuantities.TryAdd(platformType, quantity))
            {
                _platformQuantities[platformType] += quantity;
            }

            Debug.Log($"Added {quantity} {platformType} platform(s) to inventory.");
        }

        // Checks if the inventory contains a platform
        public bool HasPlatform(PlatformType platformType)
        {
            return _platformQuantities.ContainsKey(platformType) && _platformQuantities[platformType] > 0;
        }

        // Decreases the quantity of a platform by one
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

        // Returns the list of tools in the inventory
        public List<Tool> GetTools()
        {
            var tools = new List<Tool>();
            foreach (var kvp in _toolQuantities)
            {
                if (kvp.Value > 0)
                {
                    // Instantiate tool components as needed
                    Tool tool = GetComponent(kvp.Key) as Tool;
                    if (tool == null)
                    {
                        tool = gameObject.AddComponent(kvp.Key) as Tool;
                    }
                    tools.Add(tool);
                }
            }
            return tools;
        }

        // Returns the list of available platform types
        public List<PlatformType> GetPlatforms()
        {
            return new List<PlatformType>(_platformQuantities.Keys);
        }

        // Gets the quantity of a platform
        public int GetPlatformQuantity(PlatformType platformType)
        {
            return _platformQuantities.ContainsKey(platformType) ? _platformQuantities[platformType] : 0;
        }

        // Gets the quantity of a tool
        public int GetToolQuantity<T>() where T : Tool
        {
            var type = typeof(T);
            return _toolQuantities.ContainsKey(type) ? _toolQuantities[type] : 0;
        }
    }

    // Enum for different platform types
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