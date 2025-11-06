using System;
using Runtime.InventorySystem.Common;
using Runtime.InventorySystem.Inventory;
using UnityEngine;

namespace Runtime.InventorySystem.Stash
{
    public sealed class StashPresenter : IInventoryPresenter
    {
        private readonly StashView  _stashView;
        private readonly StashModel _stashModel;

        public event Action<Vector2Int, IInventoryPresenter> OnSelected;
        public event Action<Vector2Int> OnPlaceItemInput;
        public event Action<Vector2Int> OnTakeItemInput;

        public StashPresenter(StashView stashView, StashModel stashModel)
        {
            _stashModel = stashModel;
            _stashView = stashView;

            _stashView.OnCellClicked += HandleCellClicked;

        }

        private void HandleCellClicked(Vector2Int position)
        {
            if (_stashModel.CurrentItem != null)
                OnTakeItemInput?.Invoke(position);
            else
                OnPlaceItemInput?.Invoke(position);
        }

        public void Initialize()
        {
            _stashView.Clear();
            _stashView.BuildGrid(2, 2);
        }

        public void ShowItem(Item item)
        {
            if (_stashModel.CurrentItem != null) return;
            
            _stashModel.SetItem(item);
            _stashView.Clear();
            _stashView.BuildGrid(item.Width, item.Height);
            for (var y = 0; y < item.Height; y++)
            for (var x = 0; x < item.Width; x++)
            {
                if (item.Shape[x, y])
                {
                    _stashView.SetCellVisual(new Vector2Int(x, y), item.ItemColor);
                }
            }
        }

        public void Clear()
        {
            _stashModel.Clear();
            _stashView.Clear();
        }
        public bool TakeItem(Vector2Int position, out Item item)
        {
            item = null;

            if (_stashModel.CurrentItem == null)
                return false;

            if (!_stashModel.CurrentItem.Shape[position.x, position.y]) return false;
            item = _stashModel.CurrentItem;
            _stashModel.Clear();
            _stashView.Clear();
            return true;
        }

        public bool PlaceItem(Item item, Vector2Int position) => false;
    }
}