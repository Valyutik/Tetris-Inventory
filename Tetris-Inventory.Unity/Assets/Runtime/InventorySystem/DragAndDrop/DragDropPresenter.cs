using System.Collections.Generic;
using Runtime.InventorySystem.DeleteConfirmation;
using Runtime.InventorySystem.DeleteArea;
using Runtime.InventorySystem.Inventory;
using Runtime.InventorySystem.Common;
using Runtime.InventorySystem.ItemRotation;
using UnityEngine.UIElements;
using UnityEngine;

namespace Runtime.InventorySystem.DragAndDrop
{
    public class DragDropPresenter
    {
        public Item CurrentItem => _model.CurrentItem;
        
        private readonly List<IInventoryPresenter> _inventories;

        private readonly IDeleteArea _deleteArea;

        private readonly IDeleteConfirmation _deleteConfirmation;
        
        private readonly DragDropModel _model;

        private DragDropView _view;
        
        private ItemRotationHandler _rotationHandler;

        public DragDropPresenter(IDeleteArea deleteArea, IDeleteConfirmation deleteConfirmation)
        {
            _inventories = new List<IInventoryPresenter>();
            
            _deleteArea = deleteArea;

            _deleteConfirmation = deleteConfirmation;
            
            _model = new DragDropModel();
        }
        
        public void RegisterInventory(IInventoryPresenter inventory)
        {
            _inventories.Add(inventory);
            
            inventory.OnPointerEnterCell += OnPointerEnterCell;
        }

        public void Init(VisualElement root, ItemRotationHandler rotationHandler)
        {
            _view = new DragDropView(root);
            
            _rotationHandler = rotationHandler;

            _rotationHandler.OnItemRotated += RotateItem;
            
            root.RegisterCallback<PointerDownEvent>(OnPointerDown);
            root.RegisterCallback<PointerUpEvent>(OnPointerUp);
            root.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            
            _deleteArea.OnEnterDeleteArea += OnEnterDeleteArea;
            _deleteArea.OnLeaveDeleteArea += OnLeaveDeleteArea;
            _deleteArea.OnDeleteAreaInput += OnDropItemToDelete;

            _deleteConfirmation.OnConfirmDelete += OnConfirmDelete;
            _deleteConfirmation.OnCancelDelete += OnCancelDelete;
        }
        private void RotateItem()
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
            
            _deleteConfirmation.ShowPopup();
        }

        private void OnConfirmDelete()
        {
            _model.CurrentItem = null;
            
            _view.Drop();
            
            _deleteConfirmation.HidePopup();
            
            _deleteArea.DrawInteractReady(false);
        }

        private void OnCancelDelete()
        {
            _deleteConfirmation.HidePopup();
            
            if (_model.CurrentItem != null && _model.CurrentInventory != null)
            {
                _model.CurrentInventory.PlaceItem(_model.CurrentItem, _model.CurrentPosition);
            }
            
            _model.CurrentItem = null;
        }
    }
}