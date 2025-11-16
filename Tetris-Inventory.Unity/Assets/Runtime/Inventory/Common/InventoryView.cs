using Runtime.Inventory.Item;
using UnityEngine.UIElements;
using UnityEngine;

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
            Grid.style.height = height * InventoryConstants.UI.CellSize;
            Grid.style.width = width * InventoryConstants.UI.CellSize;
        }

        public VisualElement CreateCell()
        {
            var cell = new VisualElement();
            
            cell.AddToClassList(InventoryConstants.UI.CellStyle);
            
            Grid.Add(cell);

            return cell;
        }

        public VisualElement CreateItem(ItemView item)
        {
            var visualElement = new VisualElement();

            visualElement.AddToClassList(InventoryConstants.UI.ItemStyle);
            
            var image = new VisualElement()
            {
                name = ImageName,
                pickingMode = PickingMode.Ignore,
                style =
                {
                    width = item.OriginalWidth * InventoryConstants.UI.CellSize, 
                    height = item.OriginalHeight * InventoryConstants.UI.CellSize,
                    backgroundImage =  item.Visual.texture,
                    alignSelf = Align.Center,
                    flexGrow = 0
                },
            };

            var textElement = new TextElement()
            {
                pickingMode = PickingMode.Ignore
            };
            
            textElement.AddToClassList(InventoryConstants.UI.Inventory.ItemCountLabel);
            
            visualElement.Add(image);
            visualElement.Add(textElement);
            
            DrawItem(visualElement, item);

            Grid.Add(visualElement);
            
            return visualElement;
        }

        public void DrawItem(VisualElement visualElement, ItemView item)
        {
            visualElement.Q<VisualElement>(ImageName).style.rotate = new Rotate(item.Rotation);
            visualElement.Q<TextElement>().text = $"x{item.CurrentStack}";
            
            
            visualElement.style.width = item.Width * InventoryConstants.UI.CellSize;
            visualElement.style.height = item.Height * InventoryConstants.UI.CellSize;

            visualElement.style.position = Position.Absolute;
            visualElement.pickingMode = PickingMode.Ignore;
            
            visualElement.style.left = item.AnchorPosition.x * InventoryConstants.UI.CellSize;
            visualElement.style.top = item.AnchorPosition.y * InventoryConstants.UI.CellSize;
        }
        
        public void RepaintCell(VisualElement cell, Color newColor) => cell.style.backgroundColor = newColor;
        
        
        public void ClearGrid()
        {
            Grid.Clear();
        }
    }
}