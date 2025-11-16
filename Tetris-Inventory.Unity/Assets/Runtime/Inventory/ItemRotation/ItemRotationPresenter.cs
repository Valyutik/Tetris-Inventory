using Runtime.Inventory.DragAndDrop;
using UnityEngine.InputSystem;
using Runtime.Inventory.Core;
using Runtime.Core;
using Runtime.Input;

namespace Runtime.Inventory.ItemRotation
{
    public sealed class ItemRotationPresenter : IPresenter
    {
        private readonly PlayerControls _playerControls;
        
        private readonly DragDropModel _dragDropModel;

        public ItemRotationPresenter(PlayerControls playerControls, ModelStorage modelStorage)
        {
            _playerControls = playerControls;

            _dragDropModel = modelStorage.DragDropModel;
        }

        public void Enable()
        {
            _playerControls.UI.RotateItem.performed += RotateCurrentItem;
        }

        public void Disable()
        {
            _playerControls.UI.RotateItem.performed -= RotateCurrentItem;
        }

        private void RotateCurrentItem(InputAction.CallbackContext callbackContext)
        {
            _dragDropModel.RotateCurrentItem();
        }
    }
}