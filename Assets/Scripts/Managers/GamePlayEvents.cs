using System;
using UnityEngine;

namespace Managers
{
    public class GamePlayEvents : MonoBehaviour
    {
        public static GamePlayEvents instance { get; private set; }

        // Events for gameplay actions
        public event Action<Transform> OnPlayerSpawned;
        public event Action<Vector2> OnMove;
        public event Action OnJump;
        public event Action<float> OnClimb;
        public event Action OnUseTool;
        public event Action OnInteract;
        public event Action<Type> OnAbilityUnlocked;
        public event Action OnSwitchTool;
        public event Action<int> OnSwitchPlatform;
        public event Action OnPlacePlatform;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Methods to trigger the events (can be invoked by other scripts)
        public void PlayerSpawned(Transform playerTransform)
        {
            OnPlayerSpawned?.Invoke(playerTransform);
        }

        public void Move(Vector2 direction)
        {
            OnMove?.Invoke(direction);
        }

        public void Jump()
        {
            OnJump?.Invoke();
        }

        public void PlacePlatform()
        {
            OnPlacePlatform?.Invoke();
        }


        public void Climb(float value)
        {
            OnClimb?.Invoke(value);
        }

        public void UseTool()
        {
            OnUseTool?.Invoke();
        }

        public void Interact()
        {
            OnInteract?.Invoke();
        }
    
        // Method to invoke the ability unlocked event
        public void AbilityUnlocked(Type abilityType)
        {
            OnAbilityUnlocked?.Invoke(abilityType);
        }
        
        public void SwitchTool()
        {
            OnSwitchTool?.Invoke();
        }

        public void SwitchPlatform(int platformNumber)
        {
            OnSwitchPlatform?.Invoke(platformNumber);
        }
    }
}