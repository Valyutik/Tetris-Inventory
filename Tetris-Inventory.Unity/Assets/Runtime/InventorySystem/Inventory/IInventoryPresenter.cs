using System;
using Runtime.InventorySystem.Common;
using UnityEngine;

namespace Runtime.InventorySystem.Inventory
{
    public interface IInventoryPresenter
    {
        event Action<Vector2Int, IInventoryPresenter> OnPointerEnterCell;
        
        bool TakeItem(Vector2Int position, out Item item);
        
        bool PlaceItem(Item item, Vector2Int position);
    }
}