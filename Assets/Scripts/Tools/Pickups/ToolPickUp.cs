using UnityEngine;

namespace Tools.Pickups
{
    public abstract class ToolPickup : MonoBehaviour
    {
        public string toolName;
        // Called when the player interacts with the pickup
        public abstract void Interact(GameObject player);
    }
}