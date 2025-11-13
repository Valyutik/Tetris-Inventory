using System;
using UnityEngine;

namespace Runtime.Inventory.Common
{
    public interface IInventoryPresenter
    {
        event Action<Vector2Int, IInventoryPresenter> OnPointerEnterCell;
        event Action OnPointerLeaveCell;
        
        bool TakeItem(Vector2Int position, out Item.Item item);
        
        Item.Item GetItem(Vector2Int position);

        bool CanPlaceItem(Item.Item item, Vector2Int position);
        
        bool PlaceItem(Item.Item item, Vector2Int position);
    }
}