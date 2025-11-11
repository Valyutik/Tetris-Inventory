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

        public void RepaintCell(VisualElement cell, Color newColor) => cell.style.backgroundColor = newColor;
        
        public void ClearGrid()
        {
            _grid.Clear();
        }
    }
}