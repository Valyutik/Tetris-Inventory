using System;
using System.Collections.Generic;
using Runtime.Inventory.Common;
using Runtime.Inventory.ItemGeneration;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Inventory.Stash
{
    public sealed class StashPresenter : InventoryPresenterBase
    {
        private readonly IItemGenerationPresenter _itemGenerationPresenter;

        public StashPresenter(InventoryView view, InventoryModel model, VisualElement menuRoot, IItemGenerationPresenter itemGenerationPresenter) : base(view, model, menuRoot)
        {
            _itemGenerationPresenter = itemGenerationPresenter;
        }

        public override void Enable()
        {
            base.Enable();
            
            _itemGenerationPresenter.OnItemGenerated += SetItems;
        }


        public override void Dispose()
        {
            base.Dispose();
            
            _itemGenerationPresenter.OnItemGenerated -= SetItems;
        }
        
        private void SetItems(IEnumerable<Item> items)
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