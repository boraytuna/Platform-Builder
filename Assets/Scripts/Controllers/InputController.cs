using System;
using UnityEngine;

namespace Controllers
{
    public class InputController : MonoBehaviour
    {
        // Events for different input actions
        public static event Action<Vector2> OnMove;
        public static event Action OnJump;
        public static event Action OnBuildPlatform;
        public static event Action<float> OnClimb; // Changed to pass climb direction
        public static event Action OnUseTool;
        public static event Action OnInteract;

        private GameInputAction _inputActions;

        private void Awake()
        {
            _inputActions = new GameInputAction();
        }

        private void OnEnable()
        {
            _inputActions.Enable();

            // Action bindings
            _inputActions.Player.Move.performed += context => HandleMove(context.ReadValue<Vector2>());
            _inputActions.Player.Move.canceled += _ => HandleMove(Vector2.zero);
            _inputActions.Player.Jump.performed += _ => HandleJump();
            _inputActions.Player.BuildPlatform.performed += _ => HandleBuildPlatform();
            _inputActions.Player.UseTool.performed += _ => HandleUseTool();
            _inputActions.Player.Interact.performed += _ => HandleInteract();

            // Climb action bindings
            _inputActions.Player.Climb.performed += context => HandleClimb(context.ReadValue<float>());
            _inputActions.Player.Climb.canceled += _ => HandleClimb(0f);
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }

        // Handling movement input
        private void HandleMove(Vector2 direction)
        {
            OnMove?.Invoke(direction);
        }

        // Handling jump input
        private void HandleJump()
        {
            OnJump?.Invoke();
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

        // Handling climb input
        private void HandleClimb(float value)
        {
            OnClimb?.Invoke(value);
        }
    }
}