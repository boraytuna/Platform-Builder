using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class InputController : MonoBehaviour
    {
        private GameInputAction _inputActions;

        private void Awake()
        {
            _inputActions = new GameInputAction();
        }

        private void OnEnable()
        {
            _inputActions.Enable();

            // Movement bindings
            _inputActions.Player.Move.performed += context => GamePlayEvents.instance.Move(context.ReadValue<Vector2>());
            _inputActions.Player.Move.canceled += _ => GamePlayEvents.instance.Move(Vector2.zero);
            _inputActions.Player.Jump.performed += _ => GamePlayEvents.instance.Jump();

            // Climb action bindings
            _inputActions.Player.Climb.performed += context => GamePlayEvents.instance.Climb(context.ReadValue<float>());
            _inputActions.Player.Climb.canceled += _ => GamePlayEvents.instance.Climb(0f);

            // Interaction bindings
            _inputActions.Player.Interact.performed += _ => GamePlayEvents.instance.Interact();

            // Tool usage binding
            _inputActions.Player.UseTool.performed += _ => GamePlayEvents.instance.UseTool();

            // Tool switching binding
            _inputActions.Player.SwitchTool.performed += _ => GamePlayEvents.instance.SwitchTool();
            
            // Platform placing binding
            _inputActions.Player.PlacePlatform.performed += _ => GamePlayEvents.instance.PlacePlatform();

            // Platform switching binding
            _inputActions.Player.SwitchPlatform.performed += HandleSwitchPlatform;
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }

        private void HandleSwitchPlatform(InputAction.CallbackContext context)
        {

            string keyPressed = context.control.name; // Get the key name (e.g., "1", "2", "3")

            if (keyPressed == "1" || keyPressed == "2" || keyPressed == "3" || keyPressed == "4" || keyPressed == "5")
            {
                int platformNumber = int.Parse(keyPressed);
                GamePlayEvents.instance.SwitchPlatform(platformNumber);
            }
        }
    }
}