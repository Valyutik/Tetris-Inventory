using Runtime.InventorySystem.Common;
using UnityEngine;

namespace Runtime.InventorySystem.Inventory
{
    public class InventoryPresenter : InventoryPresenterBase
    {
        private readonly InventoryModel _model;
        
        protected override int Width => _model.Width;
        protected override int Height => _model.Height;

        public InventoryPresenter(InventoryView view, InventoryModel model) : base(view)
        {
            _model = model;
            CreateView();
            UpdateView();
        }
        
        public override bool TakeItem(Vector2Int position, out Item item)
        {
            item = _model.GetItem(position);
            _model.TryRemoveItem(item);
            UpdateView();
            return item != null;
        }

        public override bool PlaceItem(Item item, Vector2Int position)
        {
            if (item == null) return false;

            var success = _model.CanPlaceItem(item, position);
            if (success)
                _model.TryPlaceItem(item, position);

            UpdateView();
            return success;
        }

        protected override Color GetCellColor(Vector2Int position)
        {
            var item = _model.GetItem(position);
            return _model.IsCellOccupied(position)
                ? Color.gray
                : item?.ItemColor ?? Color.grey;
        }
    }
}