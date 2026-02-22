namespace Runtime.Inventory.Item.Extensions
{
    public static class ItemExtensions
    {
        public static ItemViewData ToView(this ItemModel model)
        {
            return new ItemViewData(
                model.Visual,
                model.Name,
                model.Description,
                model.CurrentStack,
                model.MaxStack,
                model.Rotation,
                model.OriginalWidth,
                model.OriginalHeight,
                model.Width,
                model.Height,
                model.AnchorPosition
            );
        }
        
        public static ItemModel ToModel(this ItemConfig config)
        {
            return new ItemModel(
                config.Id,
                config.DisplayName,
                config.Description,
                config.IsStackable,
                config.MaxStack,
                1,
                config.Visual,
                config.GetShapeMatrix()
            );
        }
    }
}