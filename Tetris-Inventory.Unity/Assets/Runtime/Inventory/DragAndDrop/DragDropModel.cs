using System;
using System.Collections.Generic;
using Runtime.Inventory.Common;
using UnityEngine;

namespace Runtime.Inventory.DragAndDrop
{
    public class DragDropModel
    {
        public event Action<InventoryModel> OnAddInventory;
        public event Action<InventoryModel> OnRemoveInventory;
        public event Action<Item> OnRotateItem;
        public event Action<Item> OnDraggingItem;
        public event Action OnDropItem;
        
        public bool CanProjectionPlacementInteract { get; set; }
        
        public Vector2Int CurrentPosition { get; set; }
        public Vector2Int StartPosition { get; set; }
        public Item CurrentItem { get; set; }
        public InventoryModel CurrentInventory { get; set; }
        public InventoryModel StartInventory { get; set; }
        public List<InventoryModel> Inventories { get; set; } = new();

        public void RegisterInventory(InventoryModel inventoryModel)
        {
            Inventories.Add(inventoryModel);    
            
            OnAddInventory?.Invoke(inventoryModel);
        }

        public void UnregisterInventory(InventoryModel inventoryModel)
        {
            Inventories.Remove(inventoryModel);
            
            OnRemoveInventory?.Invoke(inventoryModel);
        }

        public void RotateCurrentItem()
        {
            if (CurrentItem == null) return;
            
            CurrentItem.RotateShape();
            
            OnRotateItem?.Invoke(CurrentItem);
        }
    }
}