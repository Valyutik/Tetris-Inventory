using Runtime.Inventory.Item;
using UnityEngine;

namespace Runtime.Inventory.Common.Data
{
    public sealed class Cell
    {
        public Vector2Int Position { get; private set; }
        public ItemModel ItemModel { get; private set; }
        public bool IsEmpty => ItemModel == null;
        
        public Cell(Vector2Int position)
        {
            Position = position;
        }

        public void SetItem(ItemModel itemModel)
        {
            ItemModel = itemModel;
        }
        
        public void Clear()
        {
            ItemModel = null;
        }
    }
}