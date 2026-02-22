using Runtime.Inventory.Item;

namespace Runtime.Inventory.Common.Data
{
    public sealed class Cell
    {
        public ItemModel ItemModel { get; private set; }
        public bool IsEmpty => ItemModel == null;

        public void SetItem(ItemModel itemModel)
        {
            ItemModel = itemModel;
        }
        
        public void Clear()
        {
            ItemModel = null;
        }
    }
}