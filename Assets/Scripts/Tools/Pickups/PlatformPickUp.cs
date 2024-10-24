using InventorySystem;
using UnityEngine;

namespace Tools.Pickups
{
    public class PlatformPickup : MonoBehaviour
    {
        public PlatformType platformType;
        public int quantity = 1; // Number of platforms to add

        public void Interact(GameObject player)
        {
            Inventory inventory = player.GetComponent<Inventory>();
            if (inventory != null)
            {
                inventory.AddPlatform(platformType, quantity);
                Debug.Log($"Picked up {quantity} {platformType} platform(s).");

                // Destroy the pickup object after interaction
                Destroy(gameObject);
            }
        }
    }
}