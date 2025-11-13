using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Inventory.Common
{
    public abstract class InventoryPresenterBase : IInventoryPresenter, IDisposable
    {
        public event Action<Vector2Int, IInventoryPresenter> OnPointerEnterCell;
        public event Action OnPointerLeaveCell;

        public bool HasItems => _model.HasItems;
        private int Width => _model.Width;
        private int Height => _model.Height;

        protected readonly InventoryModel _model;

        private readonly InventoryView _view;
        
        private VisualElement[,] _cells;

        private readonly Dictionary<Item.Item, VisualElement> _items  = new Dictionary<Item.Item, VisualElement>();

        protected InventoryPresenterBase(InventoryView view, InventoryModel model, VisualElement menuRoot)
        {
            this._model = model;
            _view = view;

            menuRoot.Add(view.Root);

            CreateView();
            UpdateView();
            
            // TODO: TEMP SHIT!
            Enable();
        }

        public void Enable()
        {
            _model.OnAddItem += OnAddItem;

            _model.OnRemoveItem += OnRemoveItem;
        }

        public void Dispose()
        {
            
        }

        public abstract bool TakeItem(Vector2Int position, out Item.Item item);

        public Item.Item GetItem(Vector2Int position) => _model.GetItem(position);
        public bool CanPlaceItem(Item.Item item, Vector2Int position) => _model.CanPlaceItem(item, position);

        public abstract bool PlaceItem(Item.Item item, Vector2Int position);

        private Color GetCellColor(Vector2Int position)
        {
            var item = _model.GetItem(position);
            return _model.IsCellOccupied(position)
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
        
        private void OnAddItem(Vector2Int position, Item.Item item)
        {
            Debug.Log($"AddItem");
            
            _items.Add(item, _view.CreateItem(item.Visual, position, item.Size));
        }

        private void OnRemoveItem(Vector2Int position, Item.Item item)
        {
            var visualElement = _items.GetValueOrDefault(item);

            if (visualElement == null) return;
            
            visualElement.RemoveFromHierarchy();
            
            _items.Remove(item);
        }


        protected void UpdateView()
        {

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