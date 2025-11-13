using System;

namespace Runtime.Inventory.ItemRotation
{
    public interface IItemRotationHandler
    {
        event Action OnItemRotated;
    }
}