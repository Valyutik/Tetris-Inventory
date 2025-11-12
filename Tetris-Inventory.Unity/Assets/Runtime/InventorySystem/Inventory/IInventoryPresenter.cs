using Runtime.InventorySystem.Common;
using UnityEngine;
using System;

namespace Runtime.InventorySystem.Inventory
{
    public interface IInventoryPresenter
    {
        event Action<Vector2Int, IInventoryPresenter> OnPointerEnterCell;
        event Action OnPointerLeaveCell;
        
        bool TakeItem(Vector2Int position, out Item item);
        
        Item GetItem(Vector2Int position);
        
        bool PlaceItem(Item item, Vector2Int position);
    }
}