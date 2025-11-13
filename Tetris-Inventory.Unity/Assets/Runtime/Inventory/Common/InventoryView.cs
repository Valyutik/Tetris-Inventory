using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Inventory.Common
{
    public class InventoryView
    {
        private const string ImageName = "icon";
        public VisualElement Root { get;}
        
        private readonly VisualElement _grid;

        public InventoryView(VisualTreeAsset asset)
        {
            Root = asset.CloneTree();
            _grid = Root.Q<VisualElement>(InventoryConstants.UI.Inventory.Grid);
        }

        public void SetUpGrid(int width, int height)
        {
            _grid.style.height = height * InventoryConstants.UI.CellSize;
            _grid.style.width = width * InventoryConstants.UI.CellSize;
        }

        public VisualElement CreateCell()
        {
            var cell = new VisualElement();
            
            cell.AddToClassList(InventoryConstants.UI.CellStyle);
            
            _grid.Add(cell);

            return cell;
        }

        public VisualElement CreateItem(Item item)
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

            var textElement = new TextElement();
            
            textElement.AddToClassList(InventoryConstants.UI.Inventory.ItemCountLabel);
            
            visualElement.Add(image);
            visualElement.Add(textElement);
            
            DrawItem(visualElement, item);

            _grid.Add(visualElement);
            
            return visualElement;
        }

        public void DrawItem(VisualElement visualElement, Item item)
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
            _grid.Clear();
        }
    }
}