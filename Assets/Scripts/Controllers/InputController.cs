using System;
using UnityEngine;

namespace Controllers
{
    public class InputController : MonoBehaviour
    {
        // Events for different input actions, to be subscribed by other controllers
        public static event Action<Vector2> OnMove;
        public static event Action OnJump;
        public static event Action OnFallFast;
        public static event Action OnBuildPlatform;
        public static event Action OnUseTool;
        public static event Action OnInteract;
        

        private GameInputAction _inputActions;

        private void Awake()
        {
            Debug.Log("InputController Awake");
            _inputActions = new GameInputAction();  // This class is generated from the Input System
        }

        private void OnEnable()
        {
            // Enable the input actions when the game starts
            _inputActions.Enable();
            
            // Set up action bindings
            _inputActions.Player.Move.performed += context => HandleMove(context.ReadValue<Vector2>());
            _inputActions.Player.Move.canceled += _ => HandleMove(Vector2.zero);
            _inputActions.Player.Jump.performed += _ => HandleJump();
            _inputActions.Player.FallFast.performed += _ => HandleFallFast(); 
            _inputActions.Player.BuildPlatform.performed += _ => HandleBuildPlatform();
            _inputActions.Player.UseTool.performed += _ => HandleUseTool();
            _inputActions.Player.Interact.performed += _ => HandleInteract();
        }

        private void OnDisable()
        {
            // Disable the input actions when the object is disabled
            _inputActions.Disable();
        }

        // Handling movement input (e.g., player movement)
        private void HandleMove(Vector2 direction)
        {
            OnMove?.Invoke(direction);
        }

        // Handling jump input
        private void HandleJump()
        {
            OnJump?.Invoke();
        }
        
        //Handling fall faster
        private void HandleFallFast()
        {
            OnFallFast?.Invoke();
        }

        // Handling platform building input
        private void HandleBuildPlatform()
        {
            OnBuildPlatform?.Invoke();
        }

        // Handling tool usage input
        private void HandleUseTool()
        {
            OnUseTool?.Invoke();
        }
        
        // Handling interaction
        private void HandleInteract()
        {
            OnInteract?.Invoke();
        }
    }
}