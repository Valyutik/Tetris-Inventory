using Runtime.Inventory.Item.Extensions;
using System.Collections.Generic;
using Runtime.Inventory.Item;
using UnityEngine.UIElements;
using Runtime.Core;
using UnityEngine;

namespace Runtime.Inventory.Common
{
    public class InventoryPresenter : IPresenter
    {
        protected InventoryModel Model { get; }

        private int Width => Model.Width;
        
        private int Height => Model.Height;

        private readonly InventoryView _view;
        
        private CellView[,] _cells;

        private readonly Dictionary<ItemModel, VisualElement> _items  = new();

        public InventoryPresenter(InventoryView view, InventoryModel model)
        {
            Model = model;
            
            _view = view;
        }

        public virtual void Enable()
        {
            DrawView();
            
            Model.OnAddItem += HandleAddItem;

            Model.OnRemoveItem += HandleRemoveItem;

            Model.OnItemStacked += HandleItemStacked;

            _view.Grid.RegisterCallback<PointerEnterEvent>(HandleEnterInventoryArea);

            _view.Grid.RegisterCallback<PointerLeaveEvent>(HandleLeaveInventoryArea);
            
            Model.Enabled = true;
        }

        public virtual void Disable()
        {
            ClearView();
            
            Model.OnAddItem -= HandleAddItem;

            Model.OnRemoveItem -= HandleRemoveItem;
            
            Model.OnItemStacked -= HandleItemStacked;
            
            _view.Grid.UnregisterCallback<PointerEnterEvent>(HandleEnterInventoryArea);

            _view.Grid.UnregisterCallback<PointerLeaveEvent>(HandleLeaveInventoryArea);
            
            _view.HideGrid();
            
            Model.Enabled = false;
        }

        protected void DrawView()
        {
            ClearView();
            CreateView();
            DrawItems();
        }

        private void CreateView()
        {
            _view.ShowGrid(Width, Height);
            _cells = new CellView[Width, Height];

            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    var element = _view.CreateCell();
                    var cellView = new CellView(element, new Vector2Int(x, y));

                    element.RegisterCallback<PointerEnterEvent>(HandleCellPointerEnter);
                    element.RegisterCallback<PointerLeaveEvent>(HandleCellPointerLeave);
                
                    _cells[x, y] = cellView;
                }
            }
        }

        private void DrawItems()
        {
            foreach (var item in Model.Items)
            {
                _items.Add(item, _view.CreateItem(item.ToView()));
            }
        }

        private void ClearView()
        {
            if (_cells != null) UnsubscribeCells();
            
            _items.Clear();
            _view.ClearGrid();
            _cells = null;
        }

        private void UnsubscribeCells()
        {
            for (var y = 0; y < _cells.GetLength(1); y++)
            {
                for (var x = 0; x < _cells.GetLength(0); x++)
                {
                    var element = _cells[x, y];

                    element.Element.UnregisterCallback<PointerEnterEvent>(HandleCellPointerEnter);
                    
                    element.Element.UnregisterCallback<PointerLeaveEvent>(HandleCellPointerLeave);
                }
            }
        }

        private void HandleAddItem(Vector2Int position, ItemModel itemModel)
        {
            _items.Add(itemModel, _view.CreateItem(itemModel.ToView()));
        }

        private void HandleItemStacked(ItemModel itemModel)
        {
            if (_items.TryGetValue(itemModel, out var visualElement))
            {
                _view.DrawItem(visualElement, itemModel.ToView());
            }
        }

        private void HandleRemoveItem(Vector2Int position, ItemModel itemModel)
        {
            var visualElement = _items.GetValueOrDefault(itemModel);

            if (visualElement == null) return;
            
            visualElement.RemoveFromHierarchy();
            
            _items.Remove(itemModel);
        }
        
        private void HandleEnterInventoryArea(PointerEnterEvent evt)
        {
            Model.PointerInGridArea(true);
        }

        private void HandleLeaveInventoryArea(PointerLeaveEvent evt)
        {
            Model.PointerInGridArea(false);
        }
        
        private void HandleCellPointerEnter(PointerEnterEvent evt)
        {
            if (evt.currentTarget is VisualElement element)
            {
                foreach (var cell in _cells)
                {
                    if (cell.Element == element)
                    {
                        Model.SelectCell(cell.Position);
                        break;
                    }
                }
            }
        }

        private void HandleCellPointerLeave(PointerLeaveEvent evt)
        {
            Model.DeselectCell();
        }
    }
}