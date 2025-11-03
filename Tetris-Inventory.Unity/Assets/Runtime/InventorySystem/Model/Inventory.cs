using System.Collections.Generic;
using UnityEngine;

namespace Runtime.InventorySystem.Model
{
    public class Inventory : IInventory
    {
        private readonly List<Item> _items;
        private readonly Grid _grid;

        public int Width => _grid.Width;
        public int Height => _grid.Height;
        
        public Inventory(int width, int height)
        { 
            _grid = new Grid(width, height);
            _items = new List<Item>();
        }

        public bool CanPlaceItem(Item item, Vector2Int position)
        {
            return _grid.CanPlaceItem(item, position);
        }

        public bool TryPlaceItem(Item item, Vector2Int position)
        {
            if (!_grid.TryAddItem(item, position))
                return false;

            _items.Add(item);
            return true;
        }

        public bool TryRemoveItem(Item instance)
        {
            if (!_items.Contains(instance))
                return false;

            _grid.RemoveItem(instance);
            _items.Remove(instance);
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

        public void Clear()
        {
            _grid.Clear();
            _items.Clear();
        }
    }
}