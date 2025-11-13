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

        public VisualElement CreateItem(Sprite sprite, Vector2Int position, Vector2Int originalSize, Vector2Int size, int rotation)
        {
            var item = new VisualElement();

            var image = new VisualElement()
            {
                name = ImageName,
                pickingMode = PickingMode.Ignore,
                style =
                {
                    width = originalSize.x * InventoryConstants.UI.CellSize, 
                    height = originalSize.y * InventoryConstants.UI.CellSize,
                    backgroundImage =  sprite.texture,
                    alignSelf = Align.Center,
                    flexGrow = 0
                },
            };
            
            item.Add(image);
            
            item.AddToClassList(InventoryConstants.UI.ItemStyle);
            
            DrawItem(item, position, size, rotation);

            _grid.Add(item);
            
            return item;
        }

        public void DrawItem(VisualElement item, Vector2Int position, Vector2Int size, int rotation)
        {
            item.Q<VisualElement>(ImageName).style.rotate = new Rotate(rotation);
            
            item.style.width = size.x * InventoryConstants.UI.CellSize;
            item.style.height = size.y * InventoryConstants.UI.CellSize;

            item.style.position = Position.Absolute;
            item.pickingMode = PickingMode.Ignore;
            
            item.style.left = position.x * InventoryConstants.UI.CellSize;
            item.style.top = position.y * InventoryConstants.UI.CellSize;
        }
        
        public void RepaintCell(VisualElement cell, Color newColor) => cell.style.backgroundColor = newColor;
        
        
        public void ClearGrid()
        {
            _grid.Clear();
        }
    }
}