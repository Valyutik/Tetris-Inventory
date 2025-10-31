using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Services
{
    public class MouseInputHandler : IDeviceInputHandler
    {
        private const string TargetKey = "LeftClick";
        private const string PositionKey = "MousePosition";
        
        private const string TargetKeyPath = "<Mouse>/leftButton";
        private const string PositionKeyPath = "<Mouse>/position";
        
        public event Action OnPressed;
        
        public event Action OnReleased;
        
        public event Action<Vector2> OnChangePointerPosition;
        
        public Vector2 PointerPosition => _mouse?.position.ReadValue() ?? Vector2.zero;

        private readonly Mouse _mouse;
        
        private readonly InputAction _leftClickAction;
        
        private readonly InputAction _positionAction;

        public MouseInputHandler()
        {
            _mouse = Mouse.current;
            
            _leftClickAction = new InputAction(TargetKey, InputActionType.Button);
            
            _positionAction = new InputAction(PositionKey, InputActionType.Value);
            
            _leftClickAction.AddBinding(TargetKeyPath);
            
            _positionAction.AddBinding(PositionKeyPath);

            _leftClickAction.started += OnTargetKeyPressed;
            
            _leftClickAction.canceled += OnTargetKeyReleased;
            
            _positionAction.performed += OnMousePositionChanged;
            
            _leftClickAction.Enable();
            
            _positionAction.Enable();
        }

        private void OnTargetKeyPressed(InputAction.CallbackContext obj) => OnPressed?.Invoke();

        private void OnTargetKeyReleased(InputAction.CallbackContext obj) => OnReleased?.Invoke();

        private void OnMousePositionChanged(InputAction.CallbackContext obj) 
            => OnChangePointerPosition?.Invoke(_positionAction.ReadValue<Vector2>());

        public void Dispose()
        {
            _leftClickAction.started -= OnTargetKeyPressed;
                
            _leftClickAction.canceled -= OnTargetKeyReleased;
            
            _positionAction.performed -= OnMousePositionChanged;
            
            _leftClickAction?.Disable();
            
            _positionAction?.Disable();
            
            _leftClickAction?.Dispose();
            
            _positionAction?.Dispose();
        }
    }
}