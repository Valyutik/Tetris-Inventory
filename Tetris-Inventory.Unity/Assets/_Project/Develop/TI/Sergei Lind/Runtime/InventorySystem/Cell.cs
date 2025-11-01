using UnityEngine;

namespace TI.Sergei_Lind.Runtime.InventorySystem
{
    public sealed class Cell
    {
        public Vector2Int Position { get; private set; }
        public Item Item { get; private set; }
        public bool IsEmpty => Item == null;
        
        public Cell(Vector2Int position)
        {
            Position = position;
        }

        public void SetItem(Item item)
        {
            Item = item;
        }
        
        public void Clear()
        {
            Item = null;
        }
    }
}