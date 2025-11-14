using System;
using System.Collections.Generic;
using Runtime.Inventory.Core;
using Runtime.Inventory.Extensions;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Inventory.Common
{
    public abstract class InventoryPresenterBase : IPresenter
    {
        public bool HasItems => _model.HasItems;
        
        protected InventoryModel Model => _model;
        
        private int Width => _model.Width;
        
        private int Height => _model.Height;

        private readonly InventoryModel _model;

        private readonly InventoryView _view;
        
        private VisualElement[,] _cells;

        private readonly Dictionary<ItemModel, VisualElement> _items  = new Dictionary<ItemModel, VisualElement>();
        
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

            _view.Grid.RegisterCallback<PointerEnterEvent>(OnEnterInventoryArea);

            _view.Grid.RegisterCallback<PointerLeaveEvent>(OnLeaveInventoryArea);
        }

        public virtual void Disable()
        {
            _model.OnAddItem -= OnAddItem;

            _model.OnRemoveItem -= OnRemoveItem;
            
            _model.OnItemStacked -= OnItemStacked;
        }

        private void OnEnterInventoryArea(PointerEnterEvent evt)
        {
            _model.PointerInGridArea(true);
        }

        private void OnLeaveInventoryArea(PointerLeaveEvent evt)
        {
            _model.PointerInGridArea(false);
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

        private void OnAddItem(Vector2Int position, ItemModel itemModel)
        {
            _items.Add(itemModel, _view.CreateItem(itemModel.ToView()));
        }

        private void OnItemStacked(ItemModel itemModel)
        {
            Debug.Log("Item stacked");
            Debug.Log($"Succses item: {itemModel.CurrentStack}");

            if (_items.TryGetValue(itemModel, out var visualElement))
            {
                Debug.Log($"Succses item: {itemModel.CurrentStack}");
                _view.DrawItem(visualElement, itemModel.ToView());
            }
        }

        private void OnRemoveItem(Vector2Int position, ItemModel itemModel)
        {
            var visualElement = _items.GetValueOrDefault(itemModel);

            if (visualElement == null) return;
            
            visualElement.RemoveFromHierarchy();
            
            _items.Remove(itemModel);
        }


        protected void RedrawView()
        {
            ClearView();
            CreateView();

            foreach (var item in _model.GetAllItems())
            {
                _items.Add(item, _view.CreateItem(item.ToView()));
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