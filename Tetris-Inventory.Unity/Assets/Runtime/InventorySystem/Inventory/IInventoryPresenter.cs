using System;
using Runtime.InventorySystem.Common;
using UnityEngine;

namespace Runtime.InventorySystem.Inventory
{
    public interface IInventoryPresenter
    {
        public event Action<Vector2Int> OnPlaceItemInput;
        event Action<Vector2Int> OnTakeItemInput;
        
        bool TakeItem(Vector2Int position, out Item item);
        
        bool PlaceItem(Item item, Vector2Int position);
    }
}