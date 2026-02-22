using System;
using Runtime.Inventory.Common;
using Runtime.Inventory.Item;
using UnityEngine;

namespace Runtime.Inventory.DragAndDrop
{
    public class DragDropModel
    {
        public event Action<ItemModel> OnRotateItem;
        
        public bool CanProjectionPlacementInteract { get; set; }
        
        public Vector2Int CurrentPosition { get; set; }
        public Vector2Int StartPosition { get; set; }
        public ItemModel CurrentItemModel { get; set; }
        public InventoryModel CurrentInventory { get; set; }
        public InventoryModel StartInventory { get; set; }


        public void RotateCurrentItem()
        {
            if (CurrentItemModel != null)
            {
                CurrentItemModel.RotateShape();

                OnRotateItem?.Invoke(CurrentItemModel);
            }
        }
    }
}