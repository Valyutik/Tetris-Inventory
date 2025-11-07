using Runtime.InventorySystem.Common;

namespace Runtime.InventorySystem.Stash
{
    public sealed class StashModel
    {
        public Item CurrentItem { get; private set; }

        public bool HasItem => CurrentItem != null;

        public void SetItem(Item item)
        {
            CurrentItem = item;
        }
        
        public void Clear()
        {
            CurrentItem = null;
        }
    }
}