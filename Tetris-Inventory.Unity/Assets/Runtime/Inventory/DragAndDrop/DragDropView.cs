using Runtime.Inventory.Item;
using UnityEngine.UIElements;
using UnityEngine;

namespace Runtime.Inventory.DragAndDrop
{
    public class DragDropView
    {
        public VisualElement Root { get; }
        
        private bool IsDragging
        {
            get => DraggingElement.style.display == DisplayStyle.Flex;

            set => DraggingElement.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private VisualElement DraggingElement { get; }

        private readonly VisualElement _icon;

        private Vector2 _dragOffset;

        private Vector2 _cachedPointerPosition;
        
        public DragDropView(VisualElement root)
        {
            Root = root;
            
            DraggingElement = new VisualElement
            {
                style =
                {
                    position = Position.Absolute,
                    justifyContent = Justify.Center,
                },
                pickingMode = PickingMode.Ignore
            };

            _icon = new VisualElement()
            {
                pickingMode = PickingMode.Ignore,
                
                style =
                {
                    alignSelf = Align.Center,
                }
            };
            
            DraggingElement.AddToClassList(InventoryConstants.UI.Projection.ItemProjection);

            IsDragging = false;
            
            _icon.name = "item-icon";
            
            DraggingElement.Add(_icon);
            
            root.Add(DraggingElement);
        }

        public void Drag(ItemViewData item, Vector2 startPosition)
        {
            IsDragging = true;

            UpdateVisualByItem(item);

            _dragOffset = GetOffsetByItem(item);
            
            Move(startPosition);
            
            Root.Q<VisualElement>("item-icon").AddToClassList("item-drag");
        }

        public void DrawCanPlace()
        {
            DraggingElement.RemoveFromClassList(InventoryConstants.UI.Projection.ItemProjectionCannotPlace);
            DraggingElement.RemoveFromClassList(InventoryConstants.UI.Projection.ItemProjectionCanStack);
            DraggingElement.AddToClassList(InventoryConstants.UI.Projection.ItemProjectionCanPlace);
        }

        public void DrawCannotPlace()
        {
            DraggingElement.RemoveFromClassList(InventoryConstants.UI.Projection.ItemProjectionCanPlace);
            DraggingElement.RemoveFromClassList(InventoryConstants.UI.Projection.ItemProjectionCanStack);
            DraggingElement.AddToClassList(InventoryConstants.UI.Projection.ItemProjectionCannotPlace);
        }
        
        public void DrawCanStack()
        {
            DraggingElement.RemoveFromClassList(InventoryConstants.UI.Projection.ItemProjectionCannotPlace);
            DraggingElement.RemoveFromClassList(InventoryConstants.UI.Projection.ItemProjectionCanPlace);
            DraggingElement.AddToClassList(InventoryConstants.UI.Projection.ItemProjectionCanStack);
        }

        public void Drag(ItemViewData item)
        {
            IsDragging = true;
            
            UpdateVisualByItem(item);
            
            _dragOffset = GetOffsetByItem(item);
            
            Move(_cachedPointerPosition);

            Root.Q<VisualElement>("item-icon").AddToClassList("item-preview-show");
        }

        public void Move(Vector2 screenPosition)
        {
            if (IsDragging)
            {
                DraggingElement.style.left = screenPosition.x - _dragOffset.x * 0.1f;

                DraggingElement.style.top = screenPosition.y - _dragOffset.y * 0.1f;

                _cachedPointerPosition = screenPosition;
            }
        }

        public void Drop()
        {
            IsDragging = false;
            
            Root.Q<VisualElement>("item-icon").RemoveFromClassList("item-drag");
        }

        private void UpdateVisualByItem(ItemViewData item)
        {
            _icon.style.backgroundImage= item.Visual.texture;
            
            _icon.style.width = item.OriginalWidth * InventoryConstants.UI.CellSize;
            
            _icon.style.height = item.OriginalHeight * InventoryConstants.UI.CellSize;

            _icon.style.rotate = new Rotate(item.Rotation);
            
            DraggingElement.style.width = item.Width * InventoryConstants.UI.CellSize;

            DraggingElement.style.height = item.Height * InventoryConstants.UI.CellSize;
        }

        private Vector2 GetOffsetByItem(ItemViewData item)
        {
            return new Vector2(item.Width * InventoryConstants.UI.CellSize,
                item.Height * InventoryConstants.UI.CellSize) / 2f;
        }
    }
}