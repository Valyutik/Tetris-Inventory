using Runtime.InventorySystem.DeleteConfirmation;
using Runtime.InventorySystem.ItemRotation;
using Runtime.InventorySystem.DeleteArea;
using Runtime.InventorySystem.Inventory;
using Runtime.InventorySystem.Common;
using UnityEngine.UIElements;
using UnityEngine;
using System;

namespace Runtime.InventorySystem.DragAndDrop
{
    public class DragDropPresenter : IDisposable
    {
        public Item CurrentItem => _model.CurrentItem;

        private readonly IDeleteArea _deleteArea;

        private readonly IDeleteConfirmation _deleteConfirmation;
        
        private readonly ItemRotationHandler _rotationHandler;

        private readonly DragDropModel _model;

        private readonly DragDropView _view;
        
        private readonly VisualElement _root;

        public DragDropPresenter(IDeleteArea deleteArea, IDeleteConfirmation deleteConfirmation, ItemRotationHandler rotationHandler, VisualElement root)
        {
            _deleteArea = deleteArea;

            _deleteConfirmation = deleteConfirmation;
            _rotationHandler = rotationHandler;

            _model = new DragDropModel();
            
            _view =  new DragDropView(root);
            
            _root =  root;
        }
        
        public void RegisterInventory(IInventoryPresenter inventory) => inventory.OnPointerEnterCell += OnPointerEnterCell;

        public void UnregisterInventory(IInventoryPresenter inventory) => inventory.OnPointerEnterCell -= OnPointerEnterCell;

        public void Init()
        {
            _root.RegisterCallback<PointerDownEvent>(OnPointerDown);
            
            _root.RegisterCallback<PointerUpEvent>(OnPointerUp);
            
            _root.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            
            _deleteArea.OnEnterDeleteArea += OnEnterDeleteArea;
            _deleteArea.OnLeaveDeleteArea += OnLeaveDeleteArea;
            _deleteArea.OnDeleteAreaInput += OnDropItemToDelete;

            _deleteConfirmation.OnConfirmDelete += OnConfirmDelete;
            _deleteConfirmation.OnCancelDelete += OnCancelDelete;
            
            _rotationHandler.OnItemRotated += UpdateItem;
        }

        public void Dispose()
        {
            _deleteArea.OnEnterDeleteArea -= OnEnterDeleteArea;
            _deleteArea.OnLeaveDeleteArea -= OnLeaveDeleteArea;
            _deleteArea.OnDeleteAreaInput -= OnDropItemToDelete;
            
            _deleteConfirmation.OnConfirmDelete -= OnConfirmDelete;
            _deleteConfirmation.OnCancelDelete -= OnCancelDelete;
            
            _rotationHandler.OnItemRotated -= UpdateItem;
        }

        private void UpdateItem()
        {
            if (_model.CurrentItem == null) return;
            
            _view.Drag(_model.CurrentItem);
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            if (_model.CurrentInventory == null || _model.CurrentItem != null) return;

            var success = _model.CurrentInventory.TakeItem(_model.CurrentPosition, out var item);

            if (!success) return;
            
            _model.CurrentItem = item;
            
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
            
            var success = _model.CurrentInventory.PlaceItem(_model.CurrentItem, _model.CurrentPosition);

            if (!success) _model.StartInventory.PlaceItem(_model.CurrentItem, _model.StartPosition);
            
            _model.CurrentItem = null;
            
            _view.Drop();
        }

        private void OnPointerMove(PointerMoveEvent evt) => _view.Move(evt.position);

        private void OnPointerEnterCell(Vector2Int position, IInventoryPresenter target)
        {
            _model.CurrentPosition = position;
            
            _model.CurrentInventory = target;
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
            
            if (_model.CurrentItem != null && _model.CurrentInventory != null)
            {
                _model.CurrentInventory.PlaceItem(_model.CurrentItem, _model.CurrentPosition);
            }
            
            _model.CurrentItem = null;
        }
    }
}