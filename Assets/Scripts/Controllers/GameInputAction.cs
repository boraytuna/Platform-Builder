//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.3
//     from Assets/Scripts/Controllers/GameInput/GameInputAction.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Controllers
{
    public partial class @GameInputAction: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @GameInputAction()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameInputAction"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""b2ede636-799a-46a5-bae8-8770a29b3e42"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""f517b419-2913-4b0a-bc6e-d79c5fced6d8"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""0201f306-0bfe-446b-bf9b-e8ec29d2e711"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Climb"",
                    ""type"": ""Value"",
                    ""id"": ""28af5433-997f-430a-ae88-f93fe3132c27"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""SwitchTool"",
                    ""type"": ""Button"",
                    ""id"": ""d18d1bf2-bb63-432b-8b84-a2acfda807e3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""UseTool"",
                    ""type"": ""Button"",
                    ""id"": ""9631481e-5c39-4dad-8bfc-28309e5d42fd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""42f95228-d6f8-4b41-830a-085719bdec42"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SwitchPlatform"",
                    ""type"": ""Button"",
                    ""id"": ""c9803a2f-bc1e-45eb-b00f-30fc2e51d20f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""fd57d592-8528-4653-aea3-ebb7e8966ddf"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""08e5e7ff-ebfe-4e33-91b9-4544869b1f9d"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""20276adb-c70c-4e0e-a33a-33bb5a0082ab"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""6ca6a828-a433-4372-bec6-b06a75435e2a"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""b37d7074-1631-4eee-ba93-d6a1c9a24ea6"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""4fc64a68-5fdb-454e-ac73-3643d8b2e580"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchTool"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7a0f0703-eae6-48ad-8a2b-7eac99b5305d"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UseTool"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""f36c147d-f058-4627-9bda-74e65aa2a1f5"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Climb"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""d4c25693-a840-4cb4-a5f7-00b37f008530"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Climb"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""27669446-7f7c-4295-be9c-c6c36e5685b8"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Climb"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""0eac6e02-8ebe-495f-8907-3cbcb922b969"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchPlatform"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7573fb19-e8f2-4114-b854-42bc008cabb0"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchPlatform"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a4a686cf-5a38-4443-8e2f-44fe1d822edb"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchPlatform"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e72d5e10-bdbf-46ea-82c6-c9a33b76877a"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchPlatform"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f0972806-e5e3-469e-8ebd-d8212c0f429b"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchPlatform"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Player
            m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
            m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
            m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
            m_Player_Climb = m_Player.FindAction("Climb", throwIfNotFound: true);
            m_Player_SwitchTool = m_Player.FindAction("SwitchTool", throwIfNotFound: true);
            m_Player_UseTool = m_Player.FindAction("UseTool", throwIfNotFound: true);
            m_Player_Interact = m_Player.FindAction("Interact", throwIfNotFound: true);
            m_Player_SwitchPlatform = m_Player.FindAction("SwitchPlatform", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        public IEnumerable<InputBinding> bindings => asset.bindings;

        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
        {
            return asset.FindAction(actionNameOrId, throwIfNotFound);
        }

        public int FindBinding(InputBinding bindingMask, out InputAction action)
        {
            return asset.FindBinding(bindingMask, out action);
        }

        // Player
        private readonly InputActionMap m_Player;
        private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
        private readonly InputAction m_Player_Move;
        private readonly InputAction m_Player_Jump;
        private readonly InputAction m_Player_Climb;
        private readonly InputAction m_Player_SwitchTool;
        private readonly InputAction m_Player_UseTool;
        private readonly InputAction m_Player_Interact;
        private readonly InputAction m_Player_SwitchPlatform;
        public struct PlayerActions
        {
            private @GameInputAction m_Wrapper;
            public PlayerActions(@GameInputAction wrapper) { m_Wrapper = wrapper; }
            public InputAction @Move => m_Wrapper.m_Player_Move;
            public InputAction @Jump => m_Wrapper.m_Player_Jump;
            public InputAction @Climb => m_Wrapper.m_Player_Climb;
            public InputAction @SwitchTool => m_Wrapper.m_Player_SwitchTool;
            public InputAction @UseTool => m_Wrapper.m_Player_UseTool;
            public InputAction @Interact => m_Wrapper.m_Player_Interact;
            public InputAction @SwitchPlatform => m_Wrapper.m_Player_SwitchPlatform;
            public InputActionMap Get() { return m_Wrapper.m_Player; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
            public void AddCallbacks(IPlayerActions instance)
            {
                if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Climb.started += instance.OnClimb;
                @Climb.performed += instance.OnClimb;
                @Climb.canceled += instance.OnClimb;
                @SwitchTool.started += instance.OnSwitchTool;
                @SwitchTool.performed += instance.OnSwitchTool;
                @SwitchTool.canceled += instance.OnSwitchTool;
                @UseTool.started += instance.OnUseTool;
                @UseTool.performed += instance.OnUseTool;
                @UseTool.canceled += instance.OnUseTool;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @SwitchPlatform.started += instance.OnSwitchPlatform;
                @SwitchPlatform.performed += instance.OnSwitchPlatform;
                @SwitchPlatform.canceled += instance.OnSwitchPlatform;
            }

            private void UnregisterCallbacks(IPlayerActions instance)
            {
                @Move.started -= instance.OnMove;
                @Move.performed -= instance.OnMove;
                @Move.canceled -= instance.OnMove;
                @Jump.started -= instance.OnJump;
                @Jump.performed -= instance.OnJump;
                @Jump.canceled -= instance.OnJump;
                @Climb.started -= instance.OnClimb;
                @Climb.performed -= instance.OnClimb;
                @Climb.canceled -= instance.OnClimb;
                @SwitchTool.started -= instance.OnSwitchTool;
                @SwitchTool.performed -= instance.OnSwitchTool;
                @SwitchTool.canceled -= instance.OnSwitchTool;
                @UseTool.started -= instance.OnUseTool;
                @UseTool.performed -= instance.OnUseTool;
                @UseTool.canceled -= instance.OnUseTool;
                @Interact.started -= instance.OnInteract;
                @Interact.performed -= instance.OnInteract;
                @Interact.canceled -= instance.OnInteract;
                @SwitchPlatform.started -= instance.OnSwitchPlatform;
                @SwitchPlatform.performed -= instance.OnSwitchPlatform;
                @SwitchPlatform.canceled -= instance.OnSwitchPlatform;
            }

            public void RemoveCallbacks(IPlayerActions instance)
            {
                if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IPlayerActions instance)
            {
                foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public PlayerActions @Player => new PlayerActions(this);
        public interface IPlayerActions
        {
            void OnMove(InputAction.CallbackContext context);
            void OnJump(InputAction.CallbackContext context);
            void OnClimb(InputAction.CallbackContext context);
            void OnSwitchTool(InputAction.CallbackContext context);
            void OnUseTool(InputAction.CallbackContext context);
            void OnInteract(InputAction.CallbackContext context);
            void OnSwitchPlatform(InputAction.CallbackContext context);
        }
    }
}
