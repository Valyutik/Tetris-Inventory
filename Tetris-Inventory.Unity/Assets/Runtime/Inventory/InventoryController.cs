using Runtime.Input;
using Runtime.Inventory.Common;
using Runtime.Inventory.Stash;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Inventory
{
    public class InventoryController
    {
        private InventoryPresenter InventoryPresenter { get; set; }
        private StashPresenter StashPresenter { get; set; }
        
        private readonly PlayerControls _playerControls;

        public InventoryController(PlayerControls playerControls, InventoryPresenter inventoryPresenter, StashPresenter  stashPresenter)
        {
            _playerControls = playerControls;
            
            InventoryPresenter = inventoryPresenter;
            
            StashPresenter = stashPresenter;
        }
        
        public void Enable()
        {
            _playerControls.UI.ToggleInventory.performed += ToggleInventory;
            _playerControls.UI.ToggleStash.performed += ToggleStash;
            
            StashPresenter.Enable();
            InventoryPresenter.Enable();
        }

        public void Disable()
        {
            _playerControls.UI.ToggleInventory.performed -= ToggleInventory;
            _playerControls.UI.ToggleStash.performed -= ToggleStash;
            
            StashPresenter.Disable();
            InventoryPresenter.Disable();
        }

        private void ToggleInventory(InputAction.CallbackContext action)
        {
            Debug.Log($"U");
            
            if (InventoryPresenter.Enabled)
            {
                InventoryPresenter.Disable();
            }
            else
            {
                InventoryPresenter.Enable();
            }
        }

        private void ToggleStash(InputAction.CallbackContext action)
        {
            Debug.Log($"I");
            
            if (StashPresenter.Enabled)
            {
                InventoryPresenter.Disable();
            }
            else
            {
                StashPresenter.Enable();
            }
        }
    }
}