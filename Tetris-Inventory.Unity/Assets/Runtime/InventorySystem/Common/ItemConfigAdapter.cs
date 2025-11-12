namespace Runtime.InventorySystem.Common
{
    public static class ItemConfigAdapter
    {
        public static Item ToModel(ItemConfig config)
        {
            return new Item(
                id: config.id,
                name: config.displayName,
                description: config.description,
                color: config.color,
                isStackable: config.isStackable,
                maxStack: config.MaxStack,
                shape: config.GetShapeMatrix()
            );
        }
    }
}