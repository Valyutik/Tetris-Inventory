using _Project.Services;

namespace _Project.Develop.Shared._Scripts.View
{
    public interface IInventoryView
    {
        IInventoryHandler InventoryHandler { get; }

        void DrawInventory(bool[,] array);
    }
}