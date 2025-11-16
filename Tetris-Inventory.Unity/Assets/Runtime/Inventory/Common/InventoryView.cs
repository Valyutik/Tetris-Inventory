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
            Grid.style.width = ToPx(width);
            Grid.style.height = ToPx(height);
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
            var el = new VisualElement();
            el.AddToClassList(InventoryConstants.UI.ItemStyle);

            el.Add(CreateItemImage(item));
            el.Add(CreateItemCountLabel());

            ApplyItemVisual(el, item);
            ApplyItemTransform(el, item);

            Grid.Add(el);
            return el;
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
                    width = ToPx(item.OriginalWidth),
                    height = ToPx(item.OriginalHeight),
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

        private void ApplyItemVisual(VisualElement el, ItemViewData item)
        {
            el.Q<VisualElement>(ImageName).style.rotate = new Rotate(item.Rotation);
            el.Q<TextElement>().text = $"x{item.CurrentStack}";
        }

        private void ApplyItemTransform(VisualElement el, ItemViewData item)
        {
            el.style.width = ToPx(item.Width);
            el.style.height = ToPx(item.Height);

            el.style.position = Position.Absolute;
            el.pickingMode = PickingMode.Ignore;

            el.style.left = ToPx(item.AnchorPosition.x);
            el.style.top = ToPx(item.AnchorPosition.y);
        }
        
        private static float ToPx(int cells) => cells * InventoryConstants.UI.CellSize;
    }
}