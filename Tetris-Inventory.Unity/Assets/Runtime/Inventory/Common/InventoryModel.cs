using Runtime.Inventory.Item;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using Grid = Runtime.Inventory.Common.Data.Grid;

namespace Runtime.Inventory.Common
{
    public sealed class InventoryModel
    {
        public event Action<Vector2Int, InventoryModel> OnSelectCell;
        public event Action OnDeselectCell;
        public event Action<ItemModel> OnItemStacked;
        public event Action<Vector2Int, ItemModel> OnAddItem;
        public event Action<Vector2Int, ItemModel> OnRemoveItem;
        public event Action<bool> OnPointerInGridArea;
        
        public IReadOnlyList<ItemModel> Items => _items;
        
        private readonly List<ItemModel> _items;
        
        private Grid _grid;

        public int Width => _grid.Width;
        
        public int Height => _grid.Height;
        
        public bool HasItems => _items.Count > 0;
        
        
        public InventoryModel(int width, int height)
        { 
            _grid = new Grid(width, height);
            _items = new List<ItemModel>();
        }

        public void PointerInGridArea(bool isGridArea)
        {
            OnPointerInGridArea?.Invoke(isGridArea);
        }
        
        public void SelectCell(Vector2Int position)
        {
            OnSelectCell?.Invoke(position, this);
        }
        
        public void DeselectCell()
        {
            OnDeselectCell?.Invoke();
        }

        public bool CanPlaceItem(ItemModel itemModel, Vector2Int position)
        {
            return _grid.CanPlaceItem(itemModel, position);
        }

        public bool CanFitItems(IReadOnlyList<ItemModel> items)
        {
            var requiredCells = CalculateRequiredCellsForItems(items);
            var freeCells = CountFreeCells();

            return freeCells >= requiredCells;
        }

        public bool TryPlaceItem(ItemModel itemModel, Vector2Int position, bool allowStacking = true)
        {
            var existingItem = _grid.GetItem(position);
            
            return TryStack(existingItem, itemModel, allowStacking) || TryPlaceInGrid(itemModel, position);
        }

        public bool TryRemoveItem(ItemModel itemModel)
        {
            return _items.Contains(itemModel) && RemoveInternal(itemModel, itemModel.AnchorPosition);
        }

        public bool TryRemoveItem(Vector2Int position, out ItemModel itemModel)
        {
            itemModel = _grid.GetItem(position);
            return itemModel != null && RemoveInternal(itemModel, position);
        }

        public ItemModel GetItem(Vector2Int position)
        {
            return _grid.GetItem(position);
        }

        public void RebuildGrid(int width, int height)
        {
            _items.Clear();
            _grid.Clear();
            
            _grid = new Grid(width, height);
        }

        public static Vector2Int CalculateGridSize(IReadOnlyList<ItemModel> items)
        {
            var totalWidth = 0;
            var maxHeight = 0;

            foreach (var item in items)
            {
                totalWidth += item.Width;
                maxHeight = Mathf.Max(maxHeight, item.Height);
            }

            return new Vector2Int(totalWidth, maxHeight);
        }

        public void Clear()
        {
            _grid.Clear();
            _items.Clear();
        }
        
        private int CalculateRequiredCellsForItems(IReadOnlyList<ItemModel> items)
        {
            var total = 0;

            foreach (var item in items)
            {
                total += item.IsStackable
                    ? CalculateRequiredCellsForStackableItem(item)
                    : ItemModel.CountOccupiedCells(item);
            }

            return total;
        }
        
        private int CalculateRequiredCellsForStackableItem(ItemModel newItem)
        {
            var remaining = newItem.CurrentStack;
            var existingStacks = _items.Where(i => i.Id == newItem.Id);

            foreach (var existing in existingStacks)
            {
                var space = existing.MaxStack - existing.CurrentStack;
                if (space <= 0) continue;

                if (remaining <= space)
                    return 0; 

                remaining -= space;
            }

            return remaining > 0
                ? ItemModel.CountOccupiedCells(newItem)
                : 0;
        }
        
        private int CountFreeCells()
        {
            var free = 0;

            for (var y = 0; y < Height; y++)
            for (var x = 0; x < Width; x++)
                if (_grid.GetCell(x, y).IsEmpty)
                    free++;

            return free;
        }
        
        private bool TryStack(ItemModel existingItem, ItemModel newItem, bool allowStacking)
        {
            if (existingItem != null)
            {
                if (allowStacking && existingItem.IsStackable)
                {
                    if (existingItem.Id == newItem.Id)
                    {
                        if (existingItem.TryAddToStack(newItem.CurrentStack))
                        {
                            OnItemStacked?.Invoke(existingItem);
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        
        private bool TryPlaceInGrid(ItemModel item, Vector2Int position)
        {
            return _grid.TryAddItem(item, position) && CompletePlacement(position, item);
        }
        
        private bool CompletePlacement(Vector2Int position, ItemModel item)
        {
            _items.Add(item);
            OnAddItem?.Invoke(position, item);
            return true;
        }
        
        private bool RemoveInternal(ItemModel item, Vector2Int eventPosition)
        {
            _grid.RemoveItem(item);
            _items.Remove(item);

            OnRemoveItem?.Invoke(eventPosition, item);

            return true;
        }
    }
}