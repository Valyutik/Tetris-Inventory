using Runtime.InventorySystem.Inventory;
using Runtime.InventorySystem.Common;
using System.Collections.Generic;
using Runtime.InventorySystem.Stash;

namespace Runtime.InventorySystem.ItemGeneration
{
    public sealed class ItemGenerationRules
    {
        private readonly InventoryPresenter _inventory;
        private readonly StashPresenter _stash;

        public ItemGenerationRules(InventoryPresenter inventory, StashPresenter stash)
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