using Runtime.Inventory.ItemGeneration;
using System.Collections.Generic;
using Runtime.Inventory.Common;
using Runtime.Inventory.Item;
using Runtime.Core;
using UnityEngine;

namespace Runtime.Inventory.Stash
{
    public sealed class StashPresenter : InventoryPresenter
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
            
            var size = InventoryModel.CalculateGridSize(items);
            
            Model.RebuildGrid(size.x, size.y);
            
            var yOffset = 0;
            foreach (var item in items)
            {
                Model.TryPlaceItem(item, new Vector2Int(0, yOffset), allowStacking: false);
                yOffset += item.Height;
            }

            DrawView();
        }
    }
}