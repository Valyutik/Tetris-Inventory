using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Inventory.Common
{
    public abstract class InventoryPresenterBase : IDisposable
    {
        public bool HasItems => _model.HasItems;
        
        protected InventoryModel Model => _model;
        
        private int Width => _model.Width;
        
        private int Height => _model.Height;

        private readonly InventoryModel _model;

        private readonly InventoryView _view;
        
        private VisualElement[,] _cells;

        private readonly Dictionary<Item, VisualElement> _items  = new Dictionary<Item, VisualElement>();
        
        protected InventoryPresenterBase(InventoryView view, InventoryModel model, VisualElement menuRoot)
        {
            _model = model;
            
            _view = view;

            menuRoot.Add(view.Root);
        }

        public virtual void Enable()
        {
            CreateView();
            
            _model.OnAddItem += OnAddItem;

            _model.OnRemoveItem += OnRemoveItem;
        }

        public virtual void Dispose()
        {
            _model.OnAddItem -= OnAddItem;

            _model.OnRemoveItem -= OnRemoveItem;
        }

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
            {
                for (var x = 0; x < Width; x++)
                {
                    var cell = _view.CreateCell();
                    var pos = new Vector2Int(x, y);

                    var onSelect = new EventCallback<PointerEnterEvent>(_ => _model.SelectCell(pos));
                    var onDeselect = new EventCallback<PointerLeaveEvent>(_ => _model.DeselectCell(pos));
                    
                    cell.RegisterCallback<PointerEnterEvent>(onSelect);
                    
                    cell.RegisterCallback<PointerLeaveEvent>(onDeselect);
                
                    _cells[x, y] = cell;
                }
            }
        }
        
        private void OnAddItem(Vector2Int position, Item item)
        {
            _items.Add(item, _view.CreateItem(item.Visual, position, item.OriginalSize, item.Size, item.Rotation));
        }

        private void OnRemoveItem(Vector2Int position, Item item)
        {
            var visualElement = _items.GetValueOrDefault(item);

            if (visualElement == null) return;
            
            visualElement.RemoveFromHierarchy();
            
            _items.Remove(item);
        }
        
                        
        protected void RedrawView()
        {
            ClearView();
            CreateView();

            foreach (var item in _model.GetAllItems())
            {
                _items.Add(item, _view.CreateItem(item.Visual, item.AnchorPosition, item.OriginalSize, item.Size, item.Rotation));
            }
        }
        
        private void ClearView()
        {
            _items.Clear();
            _view.ClearGrid();
            _cells = null;
        }
    }
}