// using Abilities;
// using Managers;
// using UnityEngine;
//
// namespace Player
// {
//     public class PlayerInteraction : MonoBehaviour
//     {
//         // Reference to the current ability unlocker the player is interacting with
//         private AbilityUnlocker _currentUnlocker;
//         private Abilities.Abilities _playerAbility;
//
//         private void OnEnable()
//         {
//             // Subscribe to the Interact event in GamePlayEvents
//             GamePlayEvents.instance.OnInteract += HandleInteract;
//         }
//
//         private void OnDisable()
//         {
//             // Unsubscribe from the Interact event to prevent memory leaks
//             GamePlayEvents.instance.OnInteract -= HandleInteract;
//         }
//
//         private void OnTriggerEnter2D(Collider2D other)
//         {
//             // Check if the player enters a trigger zone of an ability unlocker
//             AbilityUnlocker unlocker = other.GetComponent<AbilityUnlocker>();
//             if (unlocker != null)
//             {
//                 _currentUnlocker = unlocker;
//                 Debug.Log("Player is near an unlocker: " + _currentUnlocker.GetType().Name);
//             }
//         }
//
//         private void OnTriggerExit2D(Collider2D other)
//         {
//             // Check if the player exits the trigger zone of an ability unlocker
//             AbilityUnlocker unlocker = other.GetComponent<AbilityUnlocker>();
//             if (unlocker != null && _currentUnlocker == unlocker)
//             {
//                 _currentUnlocker = null;
//                 Debug.Log("Player left the unlocker.");
//             }
//         }
//
//         // Handle the Interact event from GamePlayEvents
//         private void HandleInteract()
//         {
//             if (_currentUnlocker != null)
//             {
//                 _currentUnlocker.TryUnlockAbility(_playerAbility);
//             }
//             else
//             {
//                 Debug.Log("No unlocker nearby to interact with.");
//             }
//         }
//     }
// }
using Abilities;
using Managers;
using Tools.Pickups;
using UnityEngine;

namespace Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        // Reference to the current interactable object the player is near
        private MonoBehaviour _currentInteractable;

        // Reference to the player's abilities
        private Abilities.Abilities _playerAbilities;

        private void Awake()
        {
            // Initialize _playerAbilities
            // Since Abilities is not a MonoBehaviour, you'll need to obtain it appropriately
            // For this example, let's assume it's a component or you can instantiate it
            _playerAbilities = new Abilities.Abilities();
            // Alternatively, if Abilities is managed elsewhere, get a reference to it
        }

        private void OnEnable()
        {
            // Subscribe to the Interact event in GamePlayEvents
            GamePlayEvents.instance.OnInteract += HandleInteract;
        }

        private void OnDisable()
        {
            // Unsubscribe from the Interact event to prevent memory leaks
            if (GamePlayEvents.instance != null)
            {
                GamePlayEvents.instance.OnInteract -= HandleInteract;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Check if the player enters a trigger zone of an interactable object
            if (other.TryGetComponent(out AbilityUnlocker abilityUnlocker))
            {
                _currentInteractable = abilityUnlocker;
                Debug.Log("Player is near an ability unlocker: " + abilityUnlocker.GetType().Name);
            }
            else if (other.TryGetComponent(out ToolPickup toolPickup))
            {
                _currentInteractable = toolPickup;
                Debug.Log("Player is near a tool pickup: " + toolPickup.toolName);
            }
            else if (other.TryGetComponent(out PlatformPickup platformPickup))
            {
                _currentInteractable = platformPickup;
                Debug.Log("Player is near a platform pickup: " + platformPickup.platformType.ToString());
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            // If the player exits the trigger zone of the current interactable
            if (_currentInteractable != null && other.GetComponent<MonoBehaviour>() == _currentInteractable)
            {
                Debug.Log("Player left the interactable object.");
                _currentInteractable = null;
            }
        }

        // Handle the Interact event from GamePlayEvents
        private void HandleInteract()
        {
            if (_currentInteractable != null)
            {
                if (_currentInteractable is AbilityUnlocker abilityUnlocker)
                {
                    // Use the existing functionality for ability unlocks
                    abilityUnlocker.TryUnlockAbility(_playerAbilities);
                }
                else if (_currentInteractable is ToolPickup toolPickup)
                {
                    // Interact with tool pickup
                    toolPickup.Interact(gameObject);
                }
                else if (_currentInteractable is PlatformPickup platformPickup)
                {
                    // Interact with platform pickup
                    platformPickup.Interact(gameObject);
                }
            }
            else
            {
                Debug.Log("No interactable object nearby.");
            }
        }
    }
}