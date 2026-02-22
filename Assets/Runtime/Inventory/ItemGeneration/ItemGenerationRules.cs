using System.Collections.Generic;
using Runtime.Core;
using Runtime.Inventory.Common;
using Runtime.Inventory.InventoryStorage;
using Runtime.Inventory.Item;
using Runtime.Popup;

namespace Runtime.Inventory.ItemGeneration
{
    public sealed class ItemGenerationRules
    {
        private readonly InventoryModel _inventory;
        private readonly InventoryModel _stash;
        private readonly PopupModel _popupModel;
        private readonly ItemGenerationErrorMessage _errorMessage;

        public ItemGenerationRules(InventoryModel inventory, ModelStorage modelStorage, ItemGenerationErrorMessage errorMessage)
        {
            _inventory = inventory;
            _stash = modelStorage.InventoryStorageModel.Get(InventoryType.Stash);
            _popupModel = modelStorage.PopupModel;
            _errorMessage = errorMessage;
        }

        public bool CanGenerateItems(List<ItemModel> items)
        {
            if (_stash.HasItems)
            {
                _popupModel.Open(new PopupData(_errorMessage.StashNotEmptyTitle, _errorMessage.StashNotEmptyMessage));
                return false;
            }

            if (!_inventory.CanFitItems(items))
            {
                _popupModel.Open(new PopupData(_errorMessage.InventoryHasNoSpaceTitle, _errorMessage.InventoryHasNoSpaceMessage));
                return false;
            }
            
            return true;
        }
    }
}