using Runtime.InventorySystem.Common;
using Runtime.InventorySystem.Inventory;
using UnityEngine;

namespace Runtime.Core
{
    public class GameLoop
    {
        private readonly IInventoryPresenter _inventory;
        
        private Item _cachedItem;
        
        private Vector2Int _cachedPosition;

        private IInventoryPresenter _cachedInventory;
        
        public GameLoop(IInventoryPresenter inventory)
        {
            _inventory = inventory;
        }
        
        public void Run()
        {
            _inventory.OnPlaceItemInput += position => OnPlaceItem(position, _inventory);
            
            _inventory.OnTakeItemInput += position => OnTakeItem(position, _inventory);
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
    }
}