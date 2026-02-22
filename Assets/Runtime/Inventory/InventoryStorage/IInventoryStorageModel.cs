namespace Runtime.Inventory.Common
{
    public interface IInventoryStorageModel
    {
        InventoryModel Get(InventoryType inventoryType);
    }
}