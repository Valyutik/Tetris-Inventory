using Runtime.InventorySystem.Common;
using UnityEngine;

namespace Runtime.InventorySystem.Stash
{
    public sealed class StashModel
    {
        private Item _currentItem;
        
        public Item CurrentItem => _currentItem;
        public bool HasItem => _currentItem != null;

        public void SetItem(Item item)
        {
            _currentItem = item;
        }
        
        public void Clear()
        {
            _currentItem = null;
        }
        
        public bool IsCellOccupied(Vector2Int pos)
        {
            return CurrentItem.Shape[pos.x, pos.y];
        }
    }
}