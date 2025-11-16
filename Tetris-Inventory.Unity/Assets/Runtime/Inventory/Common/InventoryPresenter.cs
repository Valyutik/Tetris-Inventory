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
        public bool HasItems => Model.HasItems;

        protected InventoryModel Model { get; }

        private int Width => Model.Width;
        
        private int Height => Model.Height;

        private readonly InventoryView _view;
        
        private VisualElement[,] _cells;

        private readonly Dictionary<ItemModel, VisualElement> _items  = new();

        public InventoryPresenter(InventoryView view, InventoryModel model)
        {
            Model = model;
            
            _view = view;
        }

        public virtual void Enable()
        {
            CreateView();
            
            Model.OnAddItem += HandleAddItem;

            Model.OnRemoveItem += HandleRemoveItem;

            Model.OnItemStacked += HandleItemStacked;

            _view.Grid.RegisterCallback<PointerEnterEvent>(HandleEnterInventoryArea);

            _view.Grid.RegisterCallback<PointerLeaveEvent>(HandleLeaveInventoryArea);
        }

        public virtual void Disable()
        {
            Model.OnAddItem -= HandleAddItem;

            Model.OnRemoveItem -= HandleRemoveItem;
            
            Model.OnItemStacked -= HandleItemStacked;
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

                    var onSelect = new EventCallback<PointerEnterEvent>(_ => Model.SelectCell(pos));
                    var onDeselect = new EventCallback<PointerLeaveEvent>(_ => Model.DeselectCell(pos));
                    
                    cell.RegisterCallback(onSelect);
                    
                    cell.RegisterCallback(onDeselect);
                
                    _cells[x, y] = cell;
                }
            }
        }

        protected void RedrawView()
        {
            ClearView();
            CreateView();

            foreach (var item in Model.GetAllItems())
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
    }
}