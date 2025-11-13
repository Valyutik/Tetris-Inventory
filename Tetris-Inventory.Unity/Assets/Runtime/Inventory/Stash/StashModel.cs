namespace Runtime.Inventory.Stash
{
    public sealed class StashModel
    {
        public Item.Item CurrentItem { get; private set; }

        public bool HasItem => CurrentItem != null;

        public void SetItem(Item.Item item)
        {
            CurrentItem = item;
        }
        
        public void Clear()
        {
            CurrentItem = null;
        }
    }
}