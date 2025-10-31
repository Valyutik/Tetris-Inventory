namespace _Project.Services
{
    public interface IInventoryView
    {
        IInventoryHandler InventoryHandler { get; }

        void DrawInventory(bool[,] array);
    }
}