using System;
using Runtime.Inventory.Common;
using Runtime.Inventory.ItemRotation;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Inventory.DragAndDrop
{
    public class DragDropPresenter : IDisposable
    {
        private readonly ItemRotationHandler _rotationHandler;

        private readonly DragDropModel _model;

        private readonly DragDropView _view;

        public DragDropPresenter(DragDropView view, DragDropModel model, ItemRotationHandler rotationHandler)
        {
            _view = view;
            
            _model = model;
            
            _rotationHandler = rotationHandler;
            
            _rotationHandler.OnItemRotated += UpdateItem;
        }

        public void Enable()
        {
            _view.Root.RegisterCallback<PointerDownEvent>(OnPointerDown);
            _view.Root.RegisterCallback<PointerUpEvent>(OnPointerUp);
            _view.Root.RegisterCallback<PointerMoveEvent>(OnPointerMove);

            foreach (var inventory in _model.Inventories)
            {
                inventory.OnSelectCell +=  OnSelectCell;
            }
            
            _model.OnAddInventory += OnAddInventory;
            _model.OnRemoveInventory += OnRemoveInventory;
        }

        public void Dispose()
        {
            _rotationHandler.OnItemRotated -= UpdateItem;
            
            _view.Root.UnregisterCallback<PointerDownEvent>(OnPointerDown);
            _view.Root.UnregisterCallback<PointerUpEvent>(OnPointerUp);
            _view.Root.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
            
            _model.OnAddInventory -= OnAddInventory;
            _model.OnRemoveInventory -= OnRemoveInventory;
        }

        private void OnRemoveInventory(InventoryModel inventory)
        {
            inventory.OnSelectCell -= OnSelectCell;
        }

        private void OnAddInventory(InventoryModel inventory)
        {
            inventory.OnSelectCell -= OnSelectCell;
        }

        private void UpdateItem()
        {
            if (_model.CurrentItem == null) return;
            
            _view.Drag(_model.CurrentItem);
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            if (_model.CurrentInventory == null || _model.CurrentItem != null) return;
            
            var success = _model.CurrentInventory.TryRemoveItem(_model.CurrentPosition, out var item);
            
            if (!success) return;

            _model.CurrentItem = item;

            _model.CurrentItem.CacheShape();
            
            _model.StartPosition = item.AnchorPosition;
                        
            _model.StartInventory = _model.CurrentInventory;
            
            _view.Drag(_model.CurrentItem, evt.position);
        }

        private void OnPointerUp(PointerUpEvent evt)
        {
            if (_model.CurrentInventory == null || _model.CurrentItem == null) return;

            var success = _model.CurrentInventory.TryPlaceItem(_model.CurrentItem, _model.CurrentPosition);

            if (!success)
            {
                _model.CurrentItem.RestoreShape();
                _model.StartInventory.TryPlaceItem(_model.CurrentItem, _model.StartPosition);
            }
            
            _model.CurrentItem = null;
            
            _view.Drop();
        }

        private void OnPointerMove(PointerMoveEvent evt)
        {
            _view.Move(evt.position);

            if (_model.CurrentInventory == null || _model.CurrentItem == null)
            {
                return;
            }
            
            var canPlace = _model.CurrentInventory.CanPlaceItem(
                _model.CurrentItem, 
                _model.CurrentPosition
            );
            
            if (canPlace)
            {
                _view.SetCanPlace();
            }
            else
            {
                _view.SetCannotPlace();
            }
        }

        private void OnSelectCell(Vector2Int position, InventoryModel target)
        {
            _model.CurrentInventory = target;
            
            if (_model.CurrentItem != null)
            {
                var offsetX = _model.CurrentItem.Width / 2;
                var offsetY = _model.CurrentItem.Height / 2;

                _model.CurrentPosition = new Vector2Int(position.x - offsetX, position.y - offsetY);
            }
            else
            {
                _model.CurrentPosition = position;
            }
        }
    }
}