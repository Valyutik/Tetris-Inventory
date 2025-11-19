using Runtime.Input;
using Runtime.Inventory.Common;
using Runtime.Inventory.Stash;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Inventory
{
    public class InventoryController
    {
        private readonly InventoryPresenter _inventoryPresenter;
        private readonly InventoryModel _inventoryModel;
        
        private readonly StashPresenter _stashPresenter;
        private readonly InventoryModel _stashModel;
        
        private readonly PlayerControls _playerControls;


        public InventoryController(PlayerControls playerControls, IInventoryStorageModel inventoryStorageModel, InventoryPresenter inventoryPresenter, StashPresenter  stashPresenter)
        {
            _playerControls = playerControls;
            
            _inventoryPresenter = inventoryPresenter;
            
            _inventoryModel =  inventoryStorageModel.Get(InventoryType.Core);
            
            _stashPresenter = stashPresenter;
            _stashModel = inventoryStorageModel.Get(InventoryType.Stash);
        }
        
        public void Enable()
        {
            _playerControls.UI.ToggleInventory.performed += ToggleInventory;
            _playerControls.UI.ToggleStash.performed += ToggleStash;
            
            _stashPresenter.Enable();
            _inventoryPresenter.Enable();
        }

        public void Disable()
        {
            _playerControls.UI.ToggleInventory.performed -= ToggleInventory;
            _playerControls.UI.ToggleStash.performed -= ToggleStash;
            
            _stashPresenter.Disable();
            _inventoryPresenter.Disable();
        }

        private void ToggleInventory(InputAction.CallbackContext action)
        {
            if (_inventoryModel.Enabled)
            {
                _inventoryPresenter.Disable();
            }
            else
            {
                _inventoryPresenter.Enable();
            }
        }

        private void ToggleStash(InputAction.CallbackContext action)
        {
            if (_stashModel.Enabled)
            {
                _stashPresenter.Disable();
            }
            else
            {
                _stashPresenter.Enable();
            }
        }
    }
}