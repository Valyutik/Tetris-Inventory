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
        
        protected InventoryPresenterBase(InventoryView view, InventoryModel model)
        {
            _model = model;
            
            _view = view;
        }

        public virtual void Enable()
        {
            CreateView();
            
            _model.OnAddItem += OnAddItem;

            _model.OnRemoveItem += OnRemoveItem;

            _model.OnItemStacked += OnItemStacked;
            
            _view.Grid.RegisterCallback<PointerEnterEvent>(_ => _model.PointerInGridArea(true));
            _view.Grid.RegisterCallback<PointerLeaveEvent>(_ => _model.PointerInGridArea(false));
        }

        public virtual void Dispose()
        {
            _model.OnAddItem -= OnAddItem;

            _model.OnRemoveItem -= OnRemoveItem;
            
            _model.OnItemStacked -= OnItemStacked;
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
            _items.Add(item, _view.CreateItem(item));
        }

        private void OnItemStacked(Item item)
        {
            Debug.Log("Item stacked");
            Debug.Log($"Succses item: {item.CurrentStack}");

            if (_items.TryGetValue(item, out var visualElement))
            {
                Debug.Log($"Succses item: {item.CurrentStack}");
                _view.DrawItem(visualElement, item);
            }
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
                _items.Add(item, _view.CreateItem(item));
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