using Runtime.InventorySystem.Common;
using UnityEngine;

namespace Runtime.InventorySystem.Inventory
{
    public interface IInventoryPresenter
    {
        public Vector2Int SelectedPosition { get; }
        
        bool TakeItem(Vector2Int position, out Item item);
        
        bool PlaceItem(Item item, Vector2Int position);
    }
}