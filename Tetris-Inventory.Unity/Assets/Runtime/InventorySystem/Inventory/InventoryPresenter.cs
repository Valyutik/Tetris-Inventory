using Runtime.InventorySystem.Common;
using UnityEngine;

namespace Runtime.InventorySystem.Inventory
{
    public class InventoryPresenter
    {
        private IInventory _preview;
        
        private IInventory _inventory;

        private Item _cachedItem;

        private IInventory _cachedInventory;
        
        private Vector2 _cachedPosition;
        
        public InventoryPresenter(IInventory preview, IInventory inventory)
        {
            _preview = preview;
            
            _inventory = inventory;
        }

        private void OnTakeItem(IInventory inventory, Vector2Int position)
        {
            var item = inventory.GetItem(position);
            if (item == null) return;
            
            _cachedItem = item;
            _cachedInventory = inventory;
            _cachedPosition = position;
        }

        private void OnPlaceItem(IInventory inventory, Vector2Int position)
        {
            var success = inventory.CanPlaceItem(_cachedItem, position);

            if (!success) _cachedInventory.TryPlaceItem(_cachedItem, position);
            else ClearCache();
        }

        private void ClearCache()
        {
            _cachedItem = null;
            _cachedInventory = null;
            _cachedPosition = default;
        }
    }
}