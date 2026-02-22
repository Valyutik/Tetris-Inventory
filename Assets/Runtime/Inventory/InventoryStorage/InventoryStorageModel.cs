using System.Collections.Generic;
using Runtime.Inventory.Item;
using Runtime.Inventory.ItemGeneration;

namespace Runtime.Inventory.Common
{
    public class InventoryStorageModel :  IInventoryStorageModel
    {
        private readonly Dictionary<InventoryType, InventoryModel> _items = new();

        public InventoryModel Get(InventoryType inventoryType)
        {
            return _items[inventoryType];
        }

        public bool RegisterInventory(InventoryType key, InventoryModel inventory)
        {
            return _items.TryAdd(key, inventory);    
        } 

        public bool UnregisterInventory(InventoryType key)
        {
            var exists = _items.TryGetValue(key, out var model);
            
            _items.Remove(key);

            return exists;
        }
    }
}