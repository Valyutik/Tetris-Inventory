using Runtime.InventorySystem.Inventory;
using Runtime.InventorySystem.Common;
using UnityEngine;

namespace Runtime.InventorySystem.Stash
{
    public sealed class StashPresenter : InventoryPresenterBase
    {
        public StashPresenter(InventoryView view, InventoryModel model) : base(view, model)
        {
            
        }
        
        public override bool TakeItem(Vector2Int position, out Item item)
        {
            item = Model.GetItem(position);
            Model.TryRemoveItem(item);
            UpdateView();
            return item != null;
        }

        public override bool PlaceItem(Item item, Vector2Int position) => false;

        public void SetItem(Item item)
        {
            Model.TryPlaceItem(item);
            RedrawView();
        }
    }
}