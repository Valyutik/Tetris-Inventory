using System.Collections.Generic;
using Runtime.Inventory.Common;
using Runtime.Inventory.Core;
using Runtime.Inventory.ItemGeneration;
using UnityEngine.UIElements;

namespace Runtime.Inventory.Stash
{
    public sealed class StashPresenter : InventoryPresenterBase
    {
        private readonly ItemGenerationModel _itemGenerationModel;
        
        public StashPresenter(InventoryView view, InventoryModel model, InventoryModelStorage storage) : base(view, model)
        {
            _itemGenerationModel = storage.ItemGenerationModel;
        }

        public override void Enable()
        {
            base.Enable();
            
            _itemGenerationModel.OnItemGenerated += SetItems;
        }
        
        public override void Dispose()
        {
            base.Dispose();
            
            _itemGenerationModel.OnItemGenerated -= SetItems;
        }
        
        private void SetItems(IReadOnlyList<Item> items)
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