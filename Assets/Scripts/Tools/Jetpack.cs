using UnityEngine;
using InventorySystem;

namespace Tools
{
    public class Jetpack : Tool
    {
        private Inventory _inventory;
        
        private void Awake()
        {
            toolName = "Jetpack";
        }
        
        private void Start()
        {
            toolName = "Jetpack";
            _inventory = GetComponent<Inventory>();
        }

        public override void Use()
        {
            if (_inventory != null)
            {
                Debug.Log($"{toolName}: Activated jetpack.");
                // Implement jetpack functionality here

                // Consume the tool after use
                _inventory.ConsumeTool<Jetpack>();
                Destroy(this); // Remove the component from the player
            }
        }
    }
}