using UnityEngine.UIElements;
using UnityEngine;

namespace Runtime.InventorySystem.Inventory
{
    public class InventoryView
    {
        private readonly VisualElement _inventoryGrid;
        
        public InventoryView(VisualElement inventoryGrid)
        {
            _inventoryGrid = inventoryGrid;
        }

        public void SetUpGrid(int width, int height)
        {
            _inventoryGrid.style.height = height * InventoryConstants.UI.CellSize;
            
            _inventoryGrid.style.width = width * InventoryConstants.UI.CellSize;
        }

        public VisualElement CreateCell()
        {
            var cell = new VisualElement();
            
            cell.AddToClassList(InventoryConstants.UI.CellStyle);
            
            _inventoryGrid.Add(cell);

            return cell;
        }

        public void RepaintCell(VisualElement cell, Color newColor) => cell.style.backgroundColor = newColor;
        
        public void ClearGrid()
        {
            _inventoryGrid.Clear();
        }
    }
}