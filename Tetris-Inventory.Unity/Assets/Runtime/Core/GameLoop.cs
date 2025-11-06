using Runtime.InventorySystem.Common;
using Runtime.InventorySystem.DeleteArea;
using Runtime.InventorySystem.Inventory;
using UnityEngine;

namespace Runtime.Core
{
    public class GameLoop
    {
        private readonly IInventoryPresenter _inventory;
        
        private readonly IDeleteArea _deleteArea;
        
        private Item _cachedItem;
        
        private Vector2Int _cachedPosition;

        private IInventoryPresenter _cachedInventory;

        public GameLoop(IInventoryPresenter inventory, IDeleteArea deleteArea)
        {
            _inventory = inventory;
            
            _deleteArea = deleteArea;
        }
        
        public void Run()
        {
            _inventory.OnPlaceItemInput += position => OnPlaceItem(position, _inventory);
            
            _inventory.OnTakeItemInput += position => OnTakeItem(position, _inventory);

            _deleteArea.OnEnterDeleteArea += OnEnterDeleteArea;
            
            _deleteArea.OnLeaveDeleteArea += OnLeaveDeleteArea;
            
            _deleteArea.OnDeleteAreaInput += OnDeleteItem;
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

        private void OnDeleteItem() => _cachedItem = null;

        private void OnEnterDeleteArea() => _deleteArea.DrawInteractReady(_cachedInventory != null);

        private void OnLeaveDeleteArea() => _deleteArea.DrawInteractReady(false);
    }
}