using System;
using Runtime.Input;
using Runtime.Inventory.Common;
using UnityEngine.InputSystem;

namespace Runtime.Inventory.ItemRotation
{
    public sealed class ItemRotationHandler : IDisposable, IItemRotationHandler
    {
        public event Action OnItemRotated;
        
        private readonly PlayerControls _playerControls;
        private readonly Func<Item> _getCurrentItem;

        public ItemRotationHandler(PlayerControls playerControls, Func<Item> getCurrentItem)
        {
            _playerControls = playerControls;
            _getCurrentItem = getCurrentItem;

            _playerControls.UI.RotateItem.performed += RotateCurrentItem;
        }
        
        public void Dispose()
        {
            _playerControls.UI.RotateItem.performed -= RotateCurrentItem;
        }

        private void RotateCurrentItem(InputAction.CallbackContext callbackContext)
        {
            var item = _getCurrentItem?.Invoke();
            
            item?.RotateShape();
            
            OnItemRotated?.Invoke();
        }
    }
}