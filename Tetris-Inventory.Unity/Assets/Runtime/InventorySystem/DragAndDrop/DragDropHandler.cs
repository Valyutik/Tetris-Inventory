using Runtime.InventorySystem.DeleteConfirmation;
using Runtime.InventorySystem.DeleteArea;
using Runtime.InventorySystem.Inventory;
using Runtime.InventorySystem.Common;
using UnityEngine.UIElements;
using Runtime.Core;
using UnityEngine;

namespace Runtime.InventorySystem.DragAndDrop
{
    public class DragDropHandler
    {
        private DragDropView _view;
        
        private readonly IInventoryPresenter _inventory;
        
        private readonly IInventoryPresenter _stash;

        private readonly IDeleteArea _deleteArea;

        private readonly IDeleteConfirmation _deleteConfirmation;
        
        private Item _cachedItem;

        private Vector2Int _cachedPosition;

        private IInventoryPresenter _cachedInventory;

        public DragDropHandler(IInventoryPresenter inventory, IInventoryPresenter stash, IDeleteArea deleteArea, IDeleteConfirmation deleteConfirmation)
        {
            _inventory = inventory;
            
            _stash = stash;

            _deleteArea = deleteArea;

            _deleteConfirmation = deleteConfirmation;
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
            if (_cachedInventory == null) return;

            var success = _cachedInventory.TakeItem(_cachedPosition, out var item);

            if (!success) return;
            
            _cachedItem = item;
            
            _view.Drag(_cachedItem);
        }

        private void OnPointerUp(PointerUpEvent evt)
        {
            if (_cachedInventory == null || _cachedItem == null) return;

            if (_deleteArea.InDeleteArea)
            {
                OnDropItemToDelete();
                
                return;
            }
            
            var success = _cachedInventory.PlaceItem(_cachedItem, _cachedPosition);

            if (!success) return;

            _cachedItem = null;
            
            _view.Drop();
        }

        private void OnPointerMove(PointerMoveEvent evt)
        {
            _view.Move(evt.position);
        }

        private void OnPointerEnterCell(Vector2Int position, IInventoryPresenter target)
        {
            _cachedPosition = position;
            
            _cachedInventory = target;
        }
        
        private void OnEnterDeleteArea() => _deleteArea.DrawInteractReady(_cachedItem != null);

        private void OnLeaveDeleteArea() => _deleteArea.DrawInteractReady(false);

        private void OnDropItemToDelete()
        {
            if (_cachedItem == null) return;
            
            _deleteConfirmation.ShowPopup();
        }

        private void OnConfirmDelete()
        {
            _cachedItem = null;
            
            _view.Drop();
            
            _cachedInventory.TakeItem(_cachedPosition, out var item);
            
            _deleteConfirmation.HidePopup();
            
            _deleteArea.DrawInteractReady(false);
        }

        private void OnCancelDelete()
        {
            _deleteConfirmation.HidePopup();
            
            if (_cachedItem != null && _cachedInventory != null)
            {
                _cachedInventory.PlaceItem(_cachedItem, _cachedPosition);
            }
            
            _cachedItem = null;
        }
    }
}