using Runtime.InventorySystem.Common;
using UnityEngine.UIElements;
using UnityEngine;
using System;

namespace Runtime.InventorySystem.Inventory
{
    public abstract class InventoryPresenterBase : IInventoryPresenter
    {
        public event Action<Vector2Int, IInventoryPresenter> OnPointerEnterCell;
        public event Action OnPointerLeaveCell;

        public bool HasItems => model.HasItems;
        private int Width => model.Width;
        private int Height => model.Height;

        protected readonly InventoryModel model;

        private readonly InventoryView _view;
        private VisualElement[,] _cells;

        protected InventoryPresenterBase(InventoryView view, InventoryModel model, VisualElement menuRoot)
        {
            this.model = model;
            _view = view;

            menuRoot.Add(view.Root);

            CreateView();
            UpdateView();
        }

        public abstract bool TakeItem(Vector2Int position, out Item item);

        public Item GetItem(Vector2Int position) => model.GetItem(position);

        public abstract bool PlaceItem(Item item, Vector2Int position);

        private Color GetCellColor(Vector2Int position)
        {
            var item = model.GetItem(position);
            return model.IsCellOccupied(position)
                ? Color.gray
                : item?.Color ?? Color.grey;
        }

        private void CreateView()
        {
            _view.SetUpGrid(Width, Height);
            _cells = new VisualElement[Width, Height];

            for (var y = 0; y < Height; y++)
            for (var x = 0; x < Width; x++)
            {
                var cell = _view.CreateCell();
                var pos = new Vector2Int(x, y);

                cell.RegisterCallback<PointerEnterEvent>(_ => OnPointerEnterCell?.Invoke(pos, this));
                cell.RegisterCallback<PointerLeaveEvent>(_ => OnPointerLeaveCell?.Invoke());
                
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