using System.Collections.Generic;
using Shared.Model;

namespace TI.Sergei_Lind.Runtime.InventorySystem
{
    public class Inventory
    {
        private readonly List<ItemInstance> _items;

        public TileMap TileMap { get; }

        public int Width => TileMap.Width;
        public int Height => TileMap.Height;
        
        public Inventory(int width, int height)
        { 
            TileMap = new TileMap(width, height);
            _items = new List<ItemInstance>();
        }

        public bool TryAddItem(ItemInstance instance, TilePosition position)
        {
            if (!TileMap.TryAddItem(instance, position))
                return false;

            _items.Add(instance);
            return true;
        }

        public bool RemoveItem(ItemInstance instance)
        {
            if (!_items.Contains(instance))
                return false;

            TileMap.RemoveItem(instance);
            _items.Remove(instance);
            return true;
        }
        
        public IEnumerable<ItemInstance> GetAllItems() => _items.AsReadOnly();

        public ItemInstance GetItem(TilePosition position)
        {
            return TileMap.GetItem(position);
        }

        public void Clear()
        {
            TileMap.Clear();
            _items.Clear();
        }
    }
}