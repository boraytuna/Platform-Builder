using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using Tools;

namespace Controllers
{
    public class InputController : MonoBehaviour
    {
        private GameInputAction _inputActions;
        private ToolController _toolController; // Reference to ToolController

        private void Awake()
        {
            _inputActions = new GameInputAction();
        }

        private void Start()
        {
            // Attempt to get the ToolController component
            _toolController = FindObjectOfType<ToolController>();
            
            if (_toolController == null)
            {
                Debug.LogError("ToolController component is missing in the scene.");
            }
        }
        
        private void OnEnable()
        {
            _inputActions.Enable();

            // Check if GamePlayEvents.instance is available
            if (GamePlayEvents.instance == null)
            {
                Debug.LogError("GamePlayEvents instance is null. Make sure it is initialized in the scene.");
                return;
            }

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
            _inputActions.Player.UseTool.performed += _ => GamePlayEvents.instance.UseTool(); // Trigger general UseTool event

            // Tool switching binding
            _inputActions.Player.SwitchTool.performed += _ => GamePlayEvents.instance.SwitchTool();

            // Platform switching binding
            _inputActions.Player.SwitchPlatform.performed += HandleSwitchPlatform;
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }

        private void HandleSwitchPlatform(InputAction.CallbackContext context)
        {
            if (GamePlayEvents.instance == null)
            {
                Debug.LogError("GamePlayEvents instance is null.");
                return;
            }

            string keyPressed = context.control.name; // Get the key name (e.g., "1", "2", "3")

            if (int.TryParse(keyPressed, out int platformNumber) && platformNumber >= 1 && platformNumber <= 5)
            {
                GamePlayEvents.instance.SwitchPlatform(platformNumber);
            }
        }
    }
}