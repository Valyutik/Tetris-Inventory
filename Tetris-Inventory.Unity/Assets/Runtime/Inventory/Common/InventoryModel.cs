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

        public bool CanPlaceItem(ItemModel itemModel, Vector2Int position)
        {
            return _grid.CanPlaceItem(itemModel, position);
        }

        public void SelectCell(Vector2Int position)
        {
            OnSelectCell?.Invoke(position, this);
        }

        public bool CanFitItems(IEnumerable<ItemModel> items)
        {
            var itemsToCheck = items.ToArray();

            var additionalCellsNeeded = 0;

            foreach (var newItem in itemsToCheck)
            {
                if (newItem.IsStackable)
                {
                    var existingStacks = _items.Where(existingItem => existingItem.Id == newItem.Id).ToList();
                    var remainingToPlace = newItem.CurrentStack;

                    foreach (var stack in existingStacks)
                    {
                        var spaceLeft = stack.MaxStack - stack.CurrentStack;

                        if (spaceLeft <= 0)
                        {
                            continue;
                        }

                        if (remainingToPlace <= spaceLeft)
                        {
                            remainingToPlace = 0;
                            break;
                        }

                        remainingToPlace -= spaceLeft;
                    }

                    if (remainingToPlace > 0)
                    {
                        additionalCellsNeeded += CountOccupiedCells(newItem);
                    }
                }
                else
                {
                    additionalCellsNeeded += CountOccupiedCells(newItem);
                }
            }
            
            var freeCells = 0;
            
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    if (_grid.GetCell(x, y).IsEmpty)
                    {
                        freeCells++;
                    }
                }
            }
            
            return freeCells >= additionalCellsNeeded;
        }

        public void DeselectCell(Vector2Int position)
        {
            OnDeselectCell?.Invoke();
        }

        public bool TryPlaceItem(ItemModel itemModel, Vector2Int position, bool allowStacking = true)
        {
            var existingItem = _grid.GetItem(position);

            if (existingItem != null && existingItem.Id == itemModel.Id)
            {
                if (existingItem.IsStackable && allowStacking)
                {
                    var success = existingItem.TryAddToStack(itemModel.CurrentStack);

                    if (success) OnItemStacked?.Invoke(existingItem);

                    return success;
                }
                
                return false;
            }

            if (_grid.TryAddItem(itemModel, position))
            {
                _items.Add(itemModel);

                OnAddItem?.Invoke(position, itemModel);

                return true;
            }

            return false;
        }

        public bool TryPlaceItem(ItemModel itemModel, bool allowStacking = true)
        {
            if (itemModel.IsStackable && allowStacking)
            {
                var existingStack = FindNonFullStack(itemModel.Id);
                if (existingStack != null)
                {
                    var success = existingStack.TryAddToStack(itemModel.CurrentStack);

                    if (success)
                    {
                        OnItemStacked?.Invoke(itemModel);
                        return true;
                    }
                }
            }

            if (_grid.TryAddItem(itemModel))
            {
                _items.Add(itemModel);

                OnAddItem?.Invoke(itemModel.AnchorPosition, itemModel);

                return true;
            }

            return false;
        }

        public bool TryRemoveItem(ItemModel itemModel)
        {
            if (_items.Contains(itemModel))
            {
                _grid.RemoveItem(itemModel);
                _items.Remove(itemModel);

                OnRemoveItem?.Invoke(itemModel.AnchorPosition, itemModel);

                return true;
            }

            return false;
        }

        public bool TryRemoveItem(Vector2Int position, out ItemModel itemModel)
        {
            itemModel = _grid.GetItem(position);
            
            _grid.RemoveItem(itemModel);
            _items.Remove(itemModel);
            
            OnRemoveItem?.Invoke(position, itemModel);
            
            return true;
        }
        
        public IReadOnlyCollection<ItemModel> GetAllItems()
        {
            return _items.AsReadOnly();
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

        private ItemModel FindNonFullStack(string id)
        {
            return _items.FirstOrDefault(existing =>
                existing.Id == id && existing.IsStackable && !existing.IsFullStack);
        }
        
        private static int CountOccupiedCells(ItemModel itemModel)
        {
            var count = 0;
            for (var x = 0; x < itemModel.Width; x++)
            {
                for (var y = 0; y < itemModel.Height; y++)
                {
                    if (itemModel.Shape[x, y])
                        count++;
                }
            }
            
            return count;
        }
    }
}