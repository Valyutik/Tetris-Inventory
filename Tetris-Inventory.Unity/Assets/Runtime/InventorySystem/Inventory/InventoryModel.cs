using Runtime.InventorySystem.Common;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Grid = Runtime.InventorySystem.Common.Grid;

namespace Runtime.InventorySystem.Inventory
{
    public sealed class InventoryModel
    {
        private readonly List<Item> _items;
        private readonly Grid _grid;

        public int Width => _grid.Width;
        public int Height => _grid.Height;
        
        public bool HasItems => _items.Count > 0;
        
        public InventoryModel(int width, int height)
        { 
            _grid = new Grid(width, height);
            _items = new List<Item>();
        }
        
        public InventoryModel(Grid grid)
        {
            _grid = grid;
            _items = new List<Item>();
        }

        public bool CanPlaceItem(Item item, Vector2Int position)
        {
            return _grid.CanPlaceItem(item, position);
        }

        public bool CanFitItems(IEnumerable<Item> items)
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
            }
            
            var freeCells = 0;
            for (var y = 0; y < Height; y++)
            for (var x = 0; x < Width; x++)
                if (_grid.GetCell(x, y).IsEmpty)
                    freeCells++;
            
            return freeCells >= additionalCellsNeeded;
        }

        public bool TryPlaceItem(Item item, Vector2Int position, bool allowStacking = true)
        {
            if (item == null)
            {
                return false;
            }
            
            var existingItem = _grid.GetItem(position);

            if (existingItem != null && existingItem.Id == item.Id)
            {
                if (existingItem.IsStackable && allowStacking)
                {
                    var success = existingItem.TryAddToStack(item.CurrentStack);
                    return success;
                }
                
                return false;
            }

            if (!_grid.TryAddItem(item, position))
            {
                return false;
            }

            _items.Add(item);
            return true;
        }

        public bool TryPlaceItem(Item item, bool allowStacking = true)
        {
            if (item == null)
            {
                return false;
            }

            if (item.IsStackable && allowStacking)
            {
                var existingStack = FindNonFullStack(item.Id);
                if (existingStack != null)
                {
                    var success = existingStack.TryAddToStack(item.CurrentStack);
                    if (success)
                        return true;
                }
            }
            
            if (!_grid.TryAddItem(item))
                return false;

            _items.Add(item);
            return true;
        }

        public bool TryRemoveItem(Item item)
        {
            if  (item == null) return false;
            
            if (!_items.Contains(item))
                return false;

            _grid.RemoveItem(item);
            _items.Remove(item);
            return true;
        }
        
        public bool TryRemoveItem(Vector2Int position)
        {
            var item = _grid.GetItem(position);
            if (item == null)
                return false;
            _grid.RemoveItem(item);
            _items.Remove(item);
            return true;
        }
        
        public IReadOnlyCollection<Item> GetAllItems() => _items.AsReadOnly();

        public Item GetItem(Vector2Int position)
        {
            return _grid.GetItem(position);
        }

        public bool IsCellOccupied(Vector2Int position)
        {
            return _grid.GetCell(position.x, position.y).IsEmpty;
        }

        public bool[,] GetOccupancyMap()
        {
            var occupancyMap = new bool[Width, Height];

            for (var y = 0; y < Height; y++)
            {
                for(var x = 0; x < Width; x++) 
                    occupancyMap[x, y] = _grid.GetCell(x, y).IsEmpty;
            }

            return occupancyMap;
        }

        public void Clear()
        {
            _grid.Clear();
            _items.Clear();
        }

        private Item FindNonFullStack(string id)
        {
            return _items.FirstOrDefault(existing =>
                existing.Id == id && existing.IsStackable && !existing.IsFullStack);
        }
        
        private static int CountOccupiedCells(Item item)
        {
            var count = 0;
            for (var x = 0; x < item.Width; x++)
            for (var y = 0; y < item.Height; y++)
                if (item.Shape[x, y])
                    count++;
            return count;
        }
    }
}