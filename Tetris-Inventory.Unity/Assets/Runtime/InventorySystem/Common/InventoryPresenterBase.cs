using Runtime.InventorySystem.Inventory;
using UnityEngine.UIElements;
using UnityEngine;
using System;

namespace Runtime.InventorySystem.Common
{
    public abstract class InventoryPresenterBase: IInventoryPresenter
    {
        public event Action<Vector2Int, IInventoryPresenter> OnPointerEnterCell;

        private readonly InventoryView _view;
        private VisualElement[,] _cells;

        protected InventoryPresenterBase(InventoryView view)
        {
            _view = view;
        }

        protected abstract int Width { get; }
        protected abstract int Height { get; }

        public abstract bool TakeItem(Vector2Int position, out Item item);
        public abstract bool PlaceItem(Item item, Vector2Int position);

        protected abstract Color GetCellColor(Vector2Int position);

        protected void CreateView()
        {
            _view.SetUpGrid(Width, Height);
            _cells = new VisualElement[Width, Height];

            for (var y = 0; y < Height; y++)
            for (var x = 0; x < Width; x++)
            {
                var cell = _view.CreateCell();
                var pos = new Vector2Int(x, y);

                cell.RegisterCallback<PointerEnterEvent>(_ => OnPointerEnterCell?.Invoke(pos, this));
                _cells[x, y] = cell;
            }
        }

        protected void UpdateView()
        {
            for (var y = 0; y < Height; y++)
            for (var x = 0; x < Width; x++)
            {
                var color = GetCellColor(new Vector2Int(x, y));
                _view.RepaintCell(_cells[x, y], color);
            }
        }

        protected void RedrawView()
        {
            ClearView();
            CreateView();
            UpdateView();
        }
        
        private void ClearView()
        {
            _view.ClearGrid();
            _cells = null;
        }
    }
}