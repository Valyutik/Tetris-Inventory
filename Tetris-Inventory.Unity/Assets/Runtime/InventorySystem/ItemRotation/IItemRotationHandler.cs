using System;

namespace Runtime.InventorySystem.ItemRotation
{
    public interface IItemRotationHandler
    {
        event Action OnItemRotated;
    }
}