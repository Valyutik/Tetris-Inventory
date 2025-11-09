using Runtime.InventorySystem.Common;
using Runtime.InventorySystem.Inventory;
using UnityEngine;

namespace Runtime.InventorySystem.DragAndDrop
{
    public class DragDropModel
    {
        public Vector2Int CurrentPosition { get; set; }
        public Vector2Int StartPosition { get; set; }

        public Item CurrentItem { get; set; }
        
        public IInventoryPresenter CurrentInventory { get; set; }
        public IInventoryPresenter StartInventory { get; set; }
    }
}