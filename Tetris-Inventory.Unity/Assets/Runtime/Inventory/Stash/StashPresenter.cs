using Runtime.Inventory.ItemGeneration;
using System.Collections.Generic;
using Runtime.Inventory.Common;
using Runtime.Inventory.Core;
using Runtime.Inventory.Item;

namespace Runtime.Inventory.Stash
{
    public sealed class StashPresenter : InventoryPresenterBase
    {
        private readonly ItemGenerationModel _itemGenerationModel;
        
        public StashPresenter(InventoryView view, InventoryModel model, ModelStorage storage) : base(view, model)
        {
            _itemGenerationModel = storage.ItemGenerationModel;
        }

        public override void Enable()
        {
            base.Enable();
            
            _itemGenerationModel.OnItemGenerated += SetItems;
        }
        
        public override void Disable()
        {
            base.Disable();
            
            _itemGenerationModel.OnItemGenerated -= SetItems;
        }
        
        private void SetItems(IReadOnlyList<ItemModel> items)
        {
            Model.Clear();
            
            foreach (var item in items)
            {
                Model.TryPlaceItem(item, false);
            }
            
            RedrawView();
        }
    }
}