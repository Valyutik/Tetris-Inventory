using System.Collections.Generic;
using Runtime.Inventory.Common;
using Runtime.Popup;

namespace Runtime.Inventory.ItemGeneration
{
    public sealed class ItemGenerationRules
    {
        private readonly InventoryModel _inventory;
        private readonly InventoryModel _stash;
        private readonly PopupModel _popupModel;
        private readonly ItemGenerationErrorMessage _errorMessage;

        public ItemGenerationRules(InventoryModel inventory, InventoryModel stash, PopupModel popupModel, ItemGenerationErrorMessage errorMessage)
        {
            _inventory = inventory;
            _stash = stash;
            _popupModel = popupModel;
            _errorMessage = errorMessage;
        }

        public bool CanGenerateItems(IEnumerable<Item> items)
        {
            if (_stash.HasItems)
            {
                _popupModel.Open(new PopupData(_errorMessage.stashNotEmptyTitle, _errorMessage.stashNotEmptyMessage));
                return false;
            }

            if (!_inventory.CanFitItems(items))
            {
                _popupModel.Open(new PopupData(_errorMessage.inventoryHasNoSpaceTitle, _errorMessage.inventoryHasNoSpaceMessage));
                return false;
            }
            
            return true;
        }
    }
}