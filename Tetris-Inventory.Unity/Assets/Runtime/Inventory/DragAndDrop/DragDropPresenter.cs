using System;
using Runtime.Inventory.Common;
using Runtime.Inventory.DeleteArea;
using Runtime.Inventory.DeleteConfirmation;
using Runtime.Inventory.ItemRotation;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Inventory.DragAndDrop
{
    public class DragDropPresenter : IDisposable
    {
        private readonly IDeleteArea _deleteArea;

        private readonly IDeleteConfirmation _deleteConfirmation;
        
        private readonly ItemRotationHandler _rotationHandler;

        private readonly VisualElement _root;
        
        private readonly DragDropModel _model;

        private DragDropView _view;

        public DragDropPresenter(DragDropModel model, IDeleteArea deleteArea, IDeleteConfirmation deleteConfirmation, ItemRotationHandler rotationHandler, VisualElement root)
        {
            _model = model;
            
            _deleteArea = deleteArea;

            _deleteConfirmation = deleteConfirmation;
            _rotationHandler = rotationHandler;
            _rotationHandler.OnItemRotated += UpdateItem;

            _root = root;
        }

        public void Enable()
        {
            _view = new DragDropView(_root);
            
            _root.RegisterCallback<PointerDownEvent>(OnPointerDown);
            _root.RegisterCallback<PointerUpEvent>(OnPointerUp);
            _root.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            
            _deleteArea.OnEnterDeleteArea += OnEnterDeleteArea;
            _deleteArea.OnLeaveDeleteArea += OnLeaveDeleteArea;
            _deleteArea.OnDeleteAreaInput += OnDropItemToDelete;

            _deleteConfirmation.OnConfirmDelete += OnConfirmDelete;
            _deleteConfirmation.OnCancelDelete += OnCancelDelete;

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
            
            _root.UnregisterCallback<PointerDownEvent>(OnPointerDown);
            _root.UnregisterCallback<PointerUpEvent>(OnPointerUp);
            _root.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
            
            _deleteArea.OnEnterDeleteArea -= OnEnterDeleteArea;
            _deleteArea.OnLeaveDeleteArea -= OnLeaveDeleteArea;
            _deleteArea.OnDeleteAreaInput -= OnDropItemToDelete;
            
            _deleteConfirmation.OnConfirmDelete -= OnConfirmDelete;
            _deleteConfirmation.OnCancelDelete -= OnCancelDelete;
            
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

            if (_deleteArea.InDeleteArea)
            {
                OnDropItemToDelete();
                
                return;
            }
            
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

        private void OnEnterDeleteArea() => _deleteArea.DrawInteractReady(_model.CurrentItem != null);

        private void OnLeaveDeleteArea() => _deleteArea.DrawInteractReady(false);

        private void OnDropItemToDelete()
        {
            if (_model.CurrentItem == null) return;
            
            _deleteConfirmation.Show();
        }

        private void OnConfirmDelete()
        {
            _model.CurrentItem = null;
            
            _view.Drop();
            
            _deleteConfirmation.Hide();
            
            _deleteArea.DrawInteractReady(false);
        }

        private void OnCancelDelete()
        {
            _deleteConfirmation.Hide();
            
            _view.Drop();
            
            if (_model.CurrentItem != null && _model.CurrentInventory != null)
            {
                _model.CurrentInventory.TryPlaceItem(_model.CurrentItem, _model.CurrentItem.AnchorPosition);
            }
            
            _model.CurrentItem = null;
        }
    }
}