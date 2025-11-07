using Codice.Client.GameUI.Checkin;
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

        private Vector2 _dragOffset;

        private Vector2 _cachedPointerPosition;
        
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

        public void Drag(Item item, Vector2 startPosition)
        {
            IsDragging = true;

            UpdateVisualByItem(item);

            _dragOffset = GetOffsetByItem(item);
            
            Move(startPosition);
        }

        public void Drag(Item item)
        {
            IsDragging = true;
            
            UpdateVisualByItem(item);
            
            _dragOffset = GetOffsetByItem(item);
            
            Move(_cachedPointerPosition);
        }

        public void Move(Vector2 screenPosition)
        {
            if (!IsDragging) return;

            _draggingElement.style.left = screenPosition.x - _dragOffset.x;
            _draggingElement.style.top = screenPosition.y - _dragOffset.y;

            _cachedPointerPosition = screenPosition;
        }

        public void Drop() => IsDragging = false;

        private void UpdateVisualByItem(Item item)
        {
            _draggingElement.style.width = item.Width * InventoryConstants.UI.CellSize;

            _draggingElement.style.height = item.Height * InventoryConstants.UI.CellSize;
        }

        private Vector2 GetOffsetByItem(Item item) 
            => new Vector2(item.Width * InventoryConstants.UI.CellSize, item.Height * InventoryConstants.UI.CellSize) / 2f;
    }
}