namespace Runtime.Inventory.Common
{
    public static class ItemConfigAdapter
    {
        public static ItemModel ToModel(ItemConfig config)
        {
            return new ItemModel(
                id: config.Id,
                name: config.DisplayName,
                description: config.Description,
                isStackable: config.IsStackable,
                maxStack: config.MaxStack,
                shape: config.GetShapeMatrix(),
                sprite: config.Visual
            );
        }
    }
}