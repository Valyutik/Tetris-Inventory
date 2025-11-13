using System;
using System.Collections.Generic;
using Runtime.Inventory.Common;
using Runtime.Inventory.ItemGeneration;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Inventory.Stash
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
        
        public override bool TakeItem(Vector2Int position, out Item.Item item)
        {
            item = _model.GetItem(position);
            _model.TryRemoveItem(item);
            UpdateView();
            return item != null;
        }

        public override bool PlaceItem(Item.Item item, Vector2Int position) => false;

        private void SetItems(IEnumerable<Item.Item> items)
        {
            _model.Clear();
            foreach (var item in items)
            {
                _model.TryPlaceItem(item, false);
            }
            
            RedrawView();
        }

        public void Dispose()
        {
            _itemGenerationPresenter.OnItemGenerated -= SetItems;
        }
    }
}