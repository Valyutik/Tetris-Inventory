using System.Collections.Generic;
using UnityEngine;

namespace Runtime.InventorySystem.Model
{
    public interface IInventory
    {
        int Width { get; }
        int Height { get; }
        
        bool CanPlaceItem(Item item, Vector2Int position);
        
        bool TryPlaceItem(Item item, Vector2Int position);
        
        bool TryRemoveItem(Item itemInstance);
        
        bool TryRemoveItem(Vector2Int position);
        
        Item GetItem(Vector2Int position);
        
        IReadOnlyCollection<Item> GetAllItems();
        
        bool IsCellOccupied(Vector2Int position);

        void Clear();
    }
}