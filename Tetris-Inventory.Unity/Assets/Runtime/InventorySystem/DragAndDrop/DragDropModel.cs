using Runtime.InventorySystem.Common;
using Runtime.InventorySystem.Inventory;
using UnityEngine;

namespace Runtime.InventorySystem.DragAndDrop
{
    public class DragDropModel
    {
        public Vector2Int CachedPosition { get; set; }

        public Item CachedItem { get; set; }

        public IInventoryPresenter CachedInventory { get; set; }
    }
}