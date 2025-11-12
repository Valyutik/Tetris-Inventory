using Runtime.InventorySystem.ItemGeneration;
using Runtime.InventorySystem.Inventory;
using Runtime.InventorySystem.Common;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;
using System;

namespace Runtime.InventorySystem.Stash
{
    public sealed class StashPresenter : InventoryPresenterBase, IDisposable
    {
        private readonly IItemGenerationPresenter _itemGenerationPresenter;

        public StashPresenter(InventoryView view,
            InventoryModel model,
            VisualElement menuRoot,
            IItemGenerationPresenter itemGenerationPresenter) : base(view,
            model,
            menuRoot)
        {
            _itemGenerationPresenter = itemGenerationPresenter;
            _itemGenerationPresenter.OnItemGenerated += SetItems;
        }
        
        public override bool TakeItem(Vector2Int position, out Item item)
        {
            item = model.GetItem(position);
            model.TryRemoveItem(item);
            UpdateView();
            return item != null;
        }

        public override bool PlaceItem(Item item, Vector2Int position) => false;

        private void SetItems(IEnumerable<Item> items)
        {
            model.Clear();
            foreach (var item in items)
            {
                model.TryPlaceItem(item);
            }
            
            RedrawView();
        }

        public void Dispose()
        {
            _itemGenerationPresenter.OnItemGenerated -= SetItems;
        }
    }
}