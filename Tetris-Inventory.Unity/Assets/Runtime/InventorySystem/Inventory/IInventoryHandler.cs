using System;

namespace Runtime.InventorySystem.Inventory
{
    public interface IInventoryHandler
    {
        event Action OnRequestCreateItem;
    }
}