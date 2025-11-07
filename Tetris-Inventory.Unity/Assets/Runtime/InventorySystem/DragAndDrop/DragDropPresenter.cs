using Runtime.InventorySystem.DeleteConfirmation;
using Runtime.InventorySystem.DeleteArea;
using Runtime.InventorySystem.Inventory;
using UnityEngine.UIElements;
using UnityEngine;

namespace Runtime.InventorySystem.DragAndDrop
{
    public class DragDropPresenter
    {
        private readonly IInventoryPresenter _inventory;
        
        private readonly IInventoryPresenter _stash;

        private readonly IDeleteArea _deleteArea;

        private readonly IDeleteConfirmation _deleteConfirmation;
        
        private readonly DragDropModel _model;

        private DragDropView _view; 

        public DragDropPresenter(IInventoryPresenter inventory, IInventoryPresenter stash, IDeleteArea deleteArea, IDeleteConfirmation deleteConfirmation)
        {
            _inventory = inventory;
            
            _stash = stash;

            _deleteArea = deleteArea;

            _deleteConfirmation = deleteConfirmation;
            
            _model = new DragDropModel();
        }

        public void Init(VisualElement root)
        {
            _view = new DragDropView(root);
            
            root.RegisterCallback<PointerDownEvent>(OnPointerDown);
            root.RegisterCallback<PointerUpEvent>(OnPointerUp);
            root.RegisterCallback<PointerMoveEvent>(OnPointerMove);

            _inventory.OnPointerEnterCell += OnPointerEnterCell;
            _stash.OnPointerEnterCell += OnPointerEnterCell;
            
            _deleteArea.OnEnterDeleteArea += OnEnterDeleteArea;
            _deleteArea.OnLeaveDeleteArea += OnLeaveDeleteArea;
            _deleteArea.OnDeleteAreaInput += OnDropItemToDelete;

            _deleteConfirmation.OnConfirmDelete += OnConfirmDelete;
            _deleteConfirmation.OnCancelDelete += OnCancelDelete;
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            if (_model.CachedInventory == null || _model.CachedItem != null) return;

            var success = _model.CachedInventory.TakeItem(_model.CachedPosition, out var item);

            if (!success) return;
            
            _model.CachedItem = item;
            
            _view.Drag(_model.CachedItem);
        }

        private void OnPointerUp(PointerUpEvent evt)
        {
            if (_model.CachedInventory == null || _model.CachedItem == null) return;

            if (_deleteArea.InDeleteArea)
            {
                OnDropItemToDelete();
                
                return;
            }
            
            var success = _model.CachedInventory.PlaceItem(_model.CachedItem, _model.CachedPosition);

            if (!success) return;

            _model.CachedItem = null;
            
            _view.Drop();
        }

        private void OnPointerMove(PointerMoveEvent evt)
        {
            _view.Move(evt.position);
        }

        private void OnPointerEnterCell(Vector2Int position, IInventoryPresenter target)
        {
            _model.CachedPosition = position;
            
            _model.CachedInventory = target;
        }
        
        private void OnEnterDeleteArea() => _deleteArea.DrawInteractReady(_model.CachedItem != null);

        private void OnLeaveDeleteArea() => _deleteArea.DrawInteractReady(false);

        private void OnDropItemToDelete()
        {
            if (_model.CachedItem == null) return;
            
            _deleteConfirmation.ShowPopup();
        }

        private void OnConfirmDelete()
        {
            _model.CachedItem = null;
            
            _view.Drop();
            
            _model.CachedInventory.TakeItem(_model.CachedPosition, out _);
            
            _deleteConfirmation.HidePopup();
            
            _deleteArea.DrawInteractReady(false);
        }

        private void OnCancelDelete()
        {
            _deleteConfirmation.HidePopup();
            
            if (_model.CachedItem != null && _model.CachedInventory != null)
            {
                _model.CachedInventory.PlaceItem(_model.CachedItem, _model.CachedPosition);
            }
            
            _model.CachedItem = null;
        }
    }
}