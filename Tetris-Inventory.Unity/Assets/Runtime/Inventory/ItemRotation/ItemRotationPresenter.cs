using System;
using Runtime.Input;
using Runtime.Inventory.Core;
using Runtime.Inventory.DragAndDrop;
using UnityEngine.InputSystem;

namespace Runtime.Inventory.ItemRotation
{
    public sealed class ItemRotationPresenter : IDisposable
    {
        private readonly PlayerControls _playerControls;
        
        private readonly DragDropModel _dragDropModel;

        public ItemRotationPresenter(PlayerControls playerControls, InventoryModelStorage modelStorage)
        {
            _playerControls = playerControls;

            _dragDropModel = modelStorage.DragDropModel;
        }

        public void Enable()
        {
            _playerControls.UI.RotateItem.performed += RotateCurrentItem;
        }
        
        public void Dispose()
        {
            _playerControls.UI.RotateItem.performed -= RotateCurrentItem;
        }

        private void RotateCurrentItem(InputAction.CallbackContext callbackContext)
        {
            _dragDropModel.RotateCurrentItem();
        }
    }
}