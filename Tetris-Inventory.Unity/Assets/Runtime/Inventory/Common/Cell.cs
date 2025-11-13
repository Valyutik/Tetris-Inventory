using UnityEngine;

namespace Runtime.Inventory.Common
{
    public sealed class Cell
    {
        public Vector2Int Position { get; private set; }
        public Item.Item Item { get; private set; }
        public bool IsEmpty => Item == null;
        
        public Cell(Vector2Int position)
        {
            Position = position;
        }

        public void SetItem(Item.Item item)
        {
            Item = item;
        }
        
        public void Clear()
        {
            Item = null;
        }
    }
}