namespace Runtime.InventorySystem.Common
{
    public static class ItemConfigAdapter
    {
        public static Item ToModel(ItemConfig config)
        {
            return new Item(
                id: config.Id,
                name: config.DisplayName,
                description: config.Description,
                color: config.Color,
                isStackable: config.IsStackable,
                maxStack: config.MaxStack,
                shape: config.GetShapeMatrix(),
                sprite: config.Visual
            );
        }
    }
}