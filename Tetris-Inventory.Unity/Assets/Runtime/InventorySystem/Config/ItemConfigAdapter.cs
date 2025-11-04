using Runtime.InventorySystem.Model;

namespace Runtime.InventorySystem.Config
{
    public static class ItemConfigAdapter
    {
        public static Item ToModel(ItemConfig config)
        {
            return new Item(
                id: config.id,
                name: config.displayName,
                description: config.description,
                shape: config.GetShapeMatrix()
            );
        }
    }
}