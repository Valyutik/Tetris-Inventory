using Runtime.Inventory.Common;

namespace Runtime.Inventory.Extensions
{
    public static class ItemModelExtensions
    {
        public static ItemView ToView(this ItemModel model)
        {
            return new ItemView(
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
    }
}