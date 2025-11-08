using Runtime.InventorySystem.Common;
using UnityEngine;

namespace Runtime.InventorySystem.Inventory
{
    public class InventoryPresenter : InventoryPresenterBase
    { 
        public InventoryPresenter(InventoryView view, InventoryModel model) : base(view, model)
        {
            
        }
        
        public override bool TakeItem(Vector2Int position, out Item item)
        {
            item = Model.GetItem(position);
            Model.TryRemoveItem(item);
            UpdateView();
            return item != null;
        }

        public override bool PlaceItem(Item item, Vector2Int position)
        {
            if (item == null) return false;

            var success = Model.CanPlaceItem(item, position);
            if (success)
                Model.TryPlaceItem(item, position);

            UpdateView();
            return success;
        }
    }
}