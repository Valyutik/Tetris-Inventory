using System;

namespace Runtime.InventorySystem.View
{
    public interface IInventoryHandler
    {
        event Action OnRequestCreateItem;
    }
}