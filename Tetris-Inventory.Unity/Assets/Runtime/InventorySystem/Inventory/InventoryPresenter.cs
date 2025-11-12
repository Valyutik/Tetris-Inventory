using Runtime.InventorySystem.Common;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;

namespace Runtime.InventorySystem.Inventory
{
    public class InventoryPresenter : InventoryPresenterBase
    {
        public InventoryPresenter(InventoryView view,
            InventoryModel model,
            VisualElement menuRoot) : base(view,
            model,
            menuRoot)
        {
        }
        
        public override bool TakeItem(Vector2Int position, out Item item)
        {
            item = model.GetItem(position);
            model.TryRemoveItem(item);
            UpdateView();
            return item != null;
        }

        public override bool PlaceItem(Item item, Vector2Int position)
        {
            if (item == null) return false;

            var success = model.TryPlaceItem(item, position);

            UpdateView();
            return success;
        }

        public bool CanFitItems(IEnumerable<Item> items)
        {
            return model.CanFitItems(items);
        }
    }
}