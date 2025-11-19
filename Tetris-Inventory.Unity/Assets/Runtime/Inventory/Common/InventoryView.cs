using Runtime.Inventory.Item;
using UnityEngine.UIElements;

namespace Runtime.Inventory.Common
{
    public class InventoryView
    {
        private const string ImageName = "icon";
        
        public VisualElement Root { get;}
        public VisualElement Grid { get; }

        public InventoryView(VisualTreeAsset asset, VisualElement menuRoot)
        {
            Root = asset.CloneTree();
            Grid = Root.Q<VisualElement>(InventoryConstants.UI.Inventory.Grid);
            
            menuRoot.Add(Root);
        }

        public void SetUpGrid(int width, int height)
        {
            Grid.style.width = ToPixel(width);
            Grid.style.height = ToPixel(height);
        }

        public VisualElement CreateCell()
        {
            var cell = new VisualElement();
            
            cell.AddToClassList(InventoryConstants.UI.CellStyle);
            
            Grid.Add(cell);

            return cell;
        }

        public VisualElement CreateItem(ItemViewData item)
        {
            var element = new VisualElement();
            element.AddToClassList(InventoryConstants.UI.ItemStyle);

            var image = CreateItemImage(item);
            var countLabel = CreateItemCountLabel();
            
            image.Add(countLabel);
            
            element.Add(image);

            ApplyItemVisual(element, item);
            ApplyItemTransform(element, item);

            Grid.Add(element);
            return element;
        }

        public void DrawItem(VisualElement element, ItemViewData item)
        {
            ApplyItemVisual(element, item);
            ApplyItemTransform(element, item);
        }

        public void ClearGrid()
        {
            Grid.Clear();
        }
        
        private VisualElement CreateItemImage(ItemViewData item)
        {
            return new VisualElement
            {
                name = ImageName,
                pickingMode = PickingMode.Ignore,
                style =
                {
                    width = ToPixel(item.OriginalWidth),
                    height = ToPixel(item.OriginalHeight),
                    backgroundImage = item.Visual.texture,
                    flexGrow = 0,
                    alignSelf = Align.Center
                }
            };
        }

        private TextElement CreateItemCountLabel()
        {
            var label = new TextElement
            {
                pickingMode = PickingMode.Ignore
            };

            label.AddToClassList(InventoryConstants.UI.Inventory.ItemCountLabel);
            return label;
        }

        private void ApplyItemVisual(VisualElement element, ItemViewData item)
        {
            element.Q<VisualElement>(ImageName).style.rotate = new Rotate(item.Rotation);
            
            var textElement = element.Q<TextElement>();
            
            textElement.style.rotate = new Rotate(-item.Rotation);
            textElement.text = $"x{item.CurrentStack}";
        }

        private void ApplyItemTransform(VisualElement element, ItemViewData item)
        {
            element.style.width = ToPixel(item.Width);
            element.style.height = ToPixel(item.Height);

            element.style.position = Position.Absolute;
            element.pickingMode = PickingMode.Ignore;

            element.style.left = ToPixel(item.AnchorPosition.x);
            element.style.top = ToPixel(item.AnchorPosition.y);
        }
        
        private static float ToPixel(int cells) => cells * InventoryConstants.UI.CellSize;
    }
}