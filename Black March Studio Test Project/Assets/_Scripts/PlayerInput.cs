//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/_Scripts/PlayerInput.inputactions
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

public partial class @PlayerInput : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""MouseInput"",
            ""id"": ""b0771918-d94e-4b39-be7d-f9de4c92b3a6"",
            ""actions"": [
                {
                    ""name"": ""RightMB"",
                    ""type"": ""Button"",
                    ""id"": ""dcd9683d-b787-4161-8773-36423c2f78e8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""1f12f18d-6379-4a1d-8456-8523bfa703c7"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightMB"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // MouseInput
        m_MouseInput = asset.FindActionMap("MouseInput", throwIfNotFound: true);
        m_MouseInput_RightMB = m_MouseInput.FindAction("RightMB", throwIfNotFound: true);
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

    // MouseInput
    private readonly InputActionMap m_MouseInput;
    private IMouseInputActions m_MouseInputActionsCallbackInterface;
    private readonly InputAction m_MouseInput_RightMB;
    public struct MouseInputActions
    {
        private @PlayerInput m_Wrapper;
        public MouseInputActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @RightMB => m_Wrapper.m_MouseInput_RightMB;
        public InputActionMap Get() { return m_Wrapper.m_MouseInput; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MouseInputActions set) { return set.Get(); }
        public void SetCallbacks(IMouseInputActions instance)
        {
            if (m_Wrapper.m_MouseInputActionsCallbackInterface != null)
            {
                @RightMB.started -= m_Wrapper.m_MouseInputActionsCallbackInterface.OnRightMB;
                @RightMB.performed -= m_Wrapper.m_MouseInputActionsCallbackInterface.OnRightMB;
                @RightMB.canceled -= m_Wrapper.m_MouseInputActionsCallbackInterface.OnRightMB;
            }
            m_Wrapper.m_MouseInputActionsCallbackInterface = instance;
            if (instance != null)
            {
                @RightMB.started += instance.OnRightMB;
                @RightMB.performed += instance.OnRightMB;
                @RightMB.canceled += instance.OnRightMB;
            }
        }
    }
    public MouseInputActions @MouseInput => new MouseInputActions(this);
    public interface IMouseInputActions
    {
        void OnRightMB(InputAction.CallbackContext context);
    }
}
