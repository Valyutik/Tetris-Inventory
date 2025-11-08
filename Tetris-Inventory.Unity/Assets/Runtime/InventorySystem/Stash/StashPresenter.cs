using Runtime.InventorySystem.Inventory;
using Runtime.InventorySystem.Common;
using UnityEngine;

namespace Runtime.InventorySystem.Stash
{
    public sealed class StashPresenter : InventoryPresenterBase
    {
        private readonly StashModel _model;

        protected override int Width => _model.CurrentItem?.Width ?? 1;
        protected override int Height => _model.CurrentItem?.Height ?? 1;

        public StashPresenter(InventoryView view, StashModel model) : base(view)
        {
            _model = model;
            CreateView();
            UpdateView();
        }
        
        public override bool TakeItem(Vector2Int position, out Item item)
        {
            item = null;
            if (!_model.HasItem || !_model.CurrentItem.Shape[position.x, position.y])
                return false;

            item = _model.CurrentItem;
            _model.Clear();
            RedrawView();
            return true;
        }

        public override bool PlaceItem(Item item, Vector2Int position) => false;
        
        protected override Color GetCellColor(Vector2Int pos)
        {
            if (!_model.HasItem)
                return Color.grey;

            return _model.CurrentItem.Shape[pos.x, pos.y]
                ? _model.CurrentItem.Color
                : Color.grey;
        }

        public void SetItem(Item item)
        {
            Model.TryPlaceItem(item);
            RedrawView();
        }
    }
}