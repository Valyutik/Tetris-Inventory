using Runtime.Inventory.Common;
using UnityEngine;

namespace Runtime.Inventory.DragAndDrop
{
    public class DragDropModel
    {
        public Vector2Int CurrentPosition { get; set; }
        public Vector2Int StartPosition { get; set; }

        public Item CurrentItem { get; set; }
        
        public InventoryModel CurrentInventory { get; set; }
        public InventoryModel StartInventory { get; set; }
    }
}