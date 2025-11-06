using Runtime.InventorySystem.Common;
using Runtime.InventorySystem.DeleteArea;
using Runtime.InventorySystem.DeleteConfirmation;
using Runtime.InventorySystem.Inventory;
using UnityEngine;

namespace Runtime.Core
{
    public class GameLoop
    {
        private readonly IInventoryPresenter _inventory;
        private readonly IInventoryPresenter _stash;

        private readonly IDeleteArea _deleteArea;

        private readonly IDeleteConfirmation _deleteConfirmation;
        
        private Item _cachedItem;
        
        private Vector2Int _cachedPosition;

        private IInventoryPresenter _cachedInventory;

        public GameLoop(IInventoryPresenter inventory, IInventoryPresenter stash, IDeleteArea deleteArea, IDeleteConfirmation deleteConfirmation)
        {
            _inventory = inventory;
            _stash = stash;

            _deleteArea = deleteArea;

            _deleteConfirmation = deleteConfirmation;
        }
        
        public void Run()
        {
            _inventory.OnPlaceItemInput += position => OnPlaceItem(position, _inventory);
            _inventory.OnTakeItemInput += position => OnTakeItem(position, _inventory);
            
            _stash.OnPlaceItemInput += position => OnPlaceItem(position, _stash);
            _stash.OnTakeItemInput += position => OnTakeItem(position, _stash);

            _deleteArea.OnEnterDeleteArea += OnEnterDeleteArea;
            _deleteArea.OnLeaveDeleteArea += OnLeaveDeleteArea;
            _deleteArea.OnDeleteAreaInput += OnDropItemToDelete;

            _deleteConfirmation.OnConfirmDelete += OnConfirmDelete;
            _deleteConfirmation.OnCancelDelete += OnCancelDelete;
        }

        private void OnPlaceItem(Vector2Int position, IInventoryPresenter inventory)
        {
            if (_cachedItem == null) return;
            
            _cachedPosition = position;

            if (inventory.PlaceItem(_cachedItem, position))
                _cachedItem = null;
            else
                _cachedInventory.PlaceItem(_cachedItem, _cachedPosition);
        }

        private void OnTakeItem(Vector2Int position, IInventoryPresenter inventory)
        {
            if (_cachedItem != null) return;

            if (!inventory.TakeItem(position, out _cachedItem)) return;
                        
            _cachedPosition = position;
            _cachedInventory = inventory;
        }

        private void OnEnterDeleteArea()
        {
            var hasItemInHand = _cachedItem != null;
            _deleteArea.DrawInteractReady(hasItemInHand);
        }
        
        private void OnLeaveDeleteArea() => _deleteArea.DrawInteractReady(false);

        private void OnDropItemToDelete()
        {
            if (_cachedItem == null) return;
            
            _deleteConfirmation.ShowPopup();
        }

        private void OnConfirmDelete()
        {
            _cachedItem = null;
            
            _deleteConfirmation.HidePopup();
            
            _deleteArea.DrawInteractReady(false);
        }

        private void OnCancelDelete()
        {
            _deleteConfirmation.HidePopup();
            
            if (_cachedItem != null && _cachedInventory != null)
            {
                _cachedInventory.PlaceItem(_cachedItem, _cachedPosition);
            }
            
            _cachedItem = null;
        }
    }
}