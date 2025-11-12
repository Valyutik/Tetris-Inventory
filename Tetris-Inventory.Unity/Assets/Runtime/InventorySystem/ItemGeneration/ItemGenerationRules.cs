using Runtime.InventorySystem.Inventory;
using Runtime.InventorySystem.Common;
using System.Collections.Generic;

namespace Runtime.InventorySystem.ItemGeneration
{
    public sealed class ItemGenerationRules
    {
        private readonly InventoryModel _inventory;
        private readonly InventoryModel _stash;

        public ItemGenerationRules(InventoryModel inventory, InventoryModel stash)
        {
            _inventory = inventory;
            _stash = stash;
        }

        public bool CanGenerateItems(IEnumerable<Item> items)
        {
            return !_stash.HasItems && _inventory.CanFitItems(items);
        }
    }
}