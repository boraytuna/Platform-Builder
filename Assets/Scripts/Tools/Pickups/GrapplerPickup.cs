using System;
using InventorySystem;
using UnityEngine;

namespace Tools.Pickups
{
    public class GrapplerPickup : ToolPickup
    {
        public int quantity = 1; // Number of grapplers to add

        private void Start()
        {
            toolName = "Grappler";
        }

        public override void Interact(GameObject player)
        {
            Inventory inventory = player.GetComponent<Inventory>();
            if (inventory != null)
            {
                inventory.AddTool<Grappler>(quantity);
                Debug.Log($"Picked up {quantity} Grappler(s).");

                // Destroy the pickup object after interaction
                Destroy(gameObject);
            }
        }
    }
}