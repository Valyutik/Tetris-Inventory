using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Inventory.Common
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
        
        public override bool TakeItem(Vector2Int position, out Item.Item item)
        {
            item = _model.GetItem(position);
            _model.TryRemoveItem(item);
            UpdateView();
            return item != null;
        }

        public override bool PlaceItem(Item.Item item, Vector2Int position)
        {
            if (item == null) return false;

            var success = _model.TryPlaceItem(item, position);

            UpdateView();
            return success;
        }
    }
}