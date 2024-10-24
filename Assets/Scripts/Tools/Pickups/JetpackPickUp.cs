using InventorySystem;
using UnityEngine;

namespace Tools.Pickups
{
    public class JetpackPickup : ToolPickup
    {
        public int quantity = 1; // Number of jetpacks to add
        
        private void Start()
        {
            toolName = "Jetpack";
        }

        public override void Interact(GameObject player)
        {
            Inventory inventory = player.GetComponent<Inventory>();
            if (inventory != null)
            {
                inventory.AddTool<Jetpack>(quantity);
                Debug.Log($"Picked up {quantity} Jetpack(s).");

                // Destroy the pickup object after interaction
                Destroy(gameObject);
            }
        }
    }
}