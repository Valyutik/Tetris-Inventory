using UnityEngine.UIElements;
using UnityEngine;

namespace Runtime.InventorySystem.Inventory
{
    public class InventoryView
    {
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

        public VisualElement CreateItem(Sprite sprite, Vector2Int position, Vector2Int size)
        {
            var item = new VisualElement();
            
            item.AddToClassList(InventoryConstants.UI.CellStyle);
            
            DrawItem(item, sprite, position, size);

            _grid.Add(item);
            
            return item;
        }

        public void DrawItem(VisualElement item, Sprite sprite, Vector2Int position, Vector2Int size)
        {
            item.style.backgroundImage = sprite.texture;
            
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