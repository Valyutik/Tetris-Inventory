using Runtime.InventorySystem.Common;
using Runtime.InventorySystem.Inventory;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Core
{
    public class GameLoop
    {
        private readonly IInventoryPresenter _inventoryPresenter;
        
        private readonly PlayerControls _playerControls;

        private IInventoryPresenter _currentInventoryPresenter;
        
        private Item _cachedItem;
        
        private Vector2Int _cachedPosition;
        
        public GameLoop(IInventoryPresenter inventoryPresenter)
        {
            _playerControls = new PlayerControls();
            
            _inventoryPresenter = inventoryPresenter;
        }
        
        public void Run()
        {
            _playerControls.Enable();

            _playerControls.UI.Click.started += OnUIClickDown;
            
            _playerControls.UI.Click.canceled += OnUIClickUp;

            _inventoryPresenter.OnSelected += position => OnSelectInventoryCell(position, _inventoryPresenter);
        }

        public void Quit()
        {
            _playerControls.Disable();
        }

        private void OnUIClickDown(InputAction.CallbackContext context)
        {
            _currentInventoryPresenter?.TakeItem(_cachedPosition, out _cachedItem);
        }

        private void OnUIClickUp(InputAction.CallbackContext context)
        {
            _currentInventoryPresenter?.PlaceItem(_cachedItem, _cachedPosition);
        }

        private void OnSelectInventoryCell(Vector2Int position, IInventoryPresenter presenter)
        {
            _cachedPosition = position;
            
            _currentInventoryPresenter = presenter;
        }
    }
}