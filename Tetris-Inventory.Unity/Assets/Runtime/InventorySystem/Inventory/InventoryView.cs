using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.InventorySystem.Inventory
{
    public class InventoryView
    {
        private const int CellSize = 100;
        
        private const string CellStyle = "cell";

        private readonly VisualElement _inventoryGrid;
        
        public InventoryView(UIDocument document)
        {
            var root = document.rootVisualElement;
            
            var inventory =root.Q<VisualElement>("Inventory");
            
            _inventoryGrid = inventory.Q<VisualElement>("Grid");
        }
        
        public void SetUpGrid(int width, int height)
        {
            _inventoryGrid.style.height = height * CellSize;
            
            _inventoryGrid.style.width = width * CellSize;
        }

        public VisualElement CreateCell()
        {
            var cell = new VisualElement();
            
            cell.AddToClassList(CellStyle);
            
            _inventoryGrid.Add(cell);

            return cell;
        }

        public void RepaintCell(VisualElement cell, Color newColor) => cell.style.backgroundColor = newColor;
    }
}