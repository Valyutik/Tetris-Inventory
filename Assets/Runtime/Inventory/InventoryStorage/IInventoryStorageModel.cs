using Runtime.Inventory.Common;

namespace Runtime.Inventory.InventoryStorage
{
    public interface IInventoryStorageModel
    {
        InventoryModel Get(InventoryType inventoryType);
    }
}