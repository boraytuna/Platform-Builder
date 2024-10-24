using UnityEngine;
using InventorySystem;

namespace Tools
{
    public class Grappler : Tool
    {
        private Inventory _inventory;

        private void Awake()
        {
            toolName = "Grappler";
        }
        private void Start()
        {
            _inventory = GetComponent<Inventory>();
        }

        public override void Use()
        {
            if (_inventory != null)
            {
                Debug.Log($"{toolName}: Used grappler.");
                // Implement grappler functionality here

                // Consume the tool after use
                _inventory.ConsumeTool<Grappler>();
                Destroy(this); // Remove the component from the player
            }
        }
    }
}