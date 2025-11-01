using System.Collections.Generic;
using UnityEngine;

namespace TI.Sergei_Lind.Runtime.InventorySystem
{
    public class Inventory
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

        public bool TryAddItem(Item instance, Vector2Int position)
        {
            if (!_grid.TryAddItem(instance, position))
                return false;

            _items.Add(instance);
            return true;
        }

        public bool RemoveItem(Item instance)
        {
            if (!_items.Contains(instance))
                return false;

            _grid.RemoveItem(instance);
            _items.Remove(instance);
            return true;
        }
        
        public IEnumerable<Item> GetAllItems() => _items.AsReadOnly();

        public Item GetItem(Vector2Int position)
        {
            return _grid.GetItem(position);
        }

        public Cell GetCell(Vector2Int  position)
        {
            return _grid.GetCell(position.x, position.y);
        }

        public void Clear()
        {
            _grid.Clear();
            _items.Clear();
        }
    }
}