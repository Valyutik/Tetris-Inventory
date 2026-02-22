namespace Runtime.Inventory.Item.Extensions
{
    public static class ItemExtensions
    {
        public static ItemViewData ToView(this ItemModel model)
        {
            return new ItemViewData(
                visual: model.Visual,
                name: model.Name,
                description: model.Description,
                currentStack: model.CurrentStack,
                maxStack: model.MaxStack,
                rotation: model.Rotation,
                originalWidth: model.OriginalWidth,
                originalHeight: model.OriginalHeight,
                width: model.Width,
                height: model.Height,
                anchorPosition: model.AnchorPosition
            );
        }
        
        public static ItemModel ToModel(this ItemConfig config)
        {
            return new ItemModel(
                id: config.Id,
                name: config.DisplayName,
                description: config.Description,
                isStackable: config.IsStackable,
                maxStack: config.MaxStack,
                currentStack: 1,
                sprite: config.Visual,
                shape: config.GetShapeMatrix()
            );
        }
    }
}