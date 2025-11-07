using Runtime.InventorySystem.Common;
using UnityEngine.UIElements;
using UnityEngine;

namespace Runtime.InventorySystem.DragAndDrop
{
    public class DragDropView
    {
        private bool IsDragging
        {
            get => _draggingElement.style.display == DisplayStyle.Flex;

            set => _draggingElement.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
        }
        
        private readonly VisualElement _draggingElement;

        public DragDropView(VisualElement root)
        {
            _draggingElement = new VisualElement
            {
                style =
                {
                    position = Position.Absolute
                },
                pickingMode = PickingMode.Ignore
            };
            
            _draggingElement.AddToClassList(InventoryConstants.UI.CellStyle);

            IsDragging = false;
            
            root.Add(_draggingElement);
        }

        public void Drag(Item item)
        {
            IsDragging = true;

            _draggingElement.style.width = item.Width * InventoryConstants.UI.CellSize;
            
            _draggingElement.style.height = item.Height * InventoryConstants.UI.CellSize;
        }

        public void Move(Vector2 screenPosition)
        {
            if (!IsDragging) return;
            
            var offsetX = _draggingElement.resolvedStyle.width / 2f;
            var offsetY = _draggingElement.resolvedStyle.height / 2f;

            _draggingElement.style.left = screenPosition.x - offsetX;
            _draggingElement.style.top = screenPosition.y - offsetY;
        }

        public void Drop() => IsDragging = false;
    }
}