using UnityEngine;
using UnityEngine.InputSystem;

namespace SimpleInputs.BaseInput
{
    public class PlayerInputBinding
    {
        public static void Bind(PlayerInput playerInput, PlayerInputs playerInputs)
        {
            BindInput(playerInput.actions.FindAction("Fire"), playerInputs.Fire);
            BindInput(playerInput.actions.FindAction("FireAlt"), playerInputs.FireAlt);
            BindInput(playerInput.actions.FindAction("Jump"), playerInputs.Jump);
            BindInput(playerInput.actions.FindAction("Run"), playerInputs.Run);
            BindInput(playerInput.actions.FindAction("Escape"), playerInputs.Escape);
            BindInput(playerInput.actions.FindAction("Move"), playerInputs.Move);
            BindInput(playerInput.actions.FindAction("Look"), playerInputs.Look);
        }

        private static void BindInput<TValue>(InputAction inputAction, InputValue<TValue> inputValue) where TValue : struct
        {
            inputAction.started += context => inputValue.SetValue(context.ReadValue<TValue>());
            inputAction.performed += context => inputValue.SetValue(context.ReadValue<TValue>());
            inputAction.canceled += context => inputValue.SetValue(default);
        }
    }
}
