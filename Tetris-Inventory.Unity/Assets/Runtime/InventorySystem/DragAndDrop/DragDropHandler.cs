using Runtime.InventorySystem.Common;
using Runtime.InventorySystem.DeleteArea;
using Runtime.InventorySystem.DeleteConfirmation;
using Runtime.InventorySystem.Inventory;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Core
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

            _inventory.OnPointerEnterCell += OnSelectCell;
            _stash.OnPointerEnterCell += OnSelectCell;
            
            _deleteArea.OnEnterDeleteArea += OnEnterDeleteArea;
            _deleteArea.OnLeaveDeleteArea += OnLeaveDeleteArea;
            _deleteArea.OnDeleteAreaInput += OnDropItemToDelete;

            _deleteConfirmation.OnConfirmDelete += OnConfirmDelete;
            _deleteConfirmation.OnCancelDelete += OnCancelDelete;
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            Debug.Log($"Pointer Down!");

            if (_cachedInventory == null) return;

            var success = _cachedInventory.TakeItem(_cachedPosition, out var item);

            if (!success) return;
            
            _cachedItem = item;
            
            _view.Drag(_cachedItem);
        }

        private void OnPointerUp(PointerUpEvent evt)
        {
            Debug.Log($"Pointer Up!");

            if (_cachedInventory == null || _cachedItem == null) return;
            
            var success = _cachedInventory.PlaceItem(_cachedItem, _cachedPosition);

            if (!success) return;
            
            _cachedItem = null;
            
            _view.Drop();
        }

        private void OnPointerMove(PointerMoveEvent evt)
        {
            Debug.Log($"Pointer Move!");
            
            _view.Move(evt.position);
        }

        private void OnSelectCell(Vector2Int position, IInventoryPresenter target)
        {
            _cachedPosition = position;
            
            _cachedInventory = target;
        }
        
        private void OnPlaceItem(Vector2Int position, IInventoryPresenter inventory)
        {
            if (_cachedItem == null) return;
            
            _cachedPosition = position;

            if (inventory.PlaceItem(_cachedItem, position))
                _cachedItem = null;
            else
                _cachedInventory.PlaceItem(_cachedItem, _cachedPosition);
        }

        private void OnTakeItem(Vector2Int position, IInventoryPresenter inventory)
        {
            if (_cachedItem != null) return;

            if (!inventory.TakeItem(position, out _cachedItem)) return;
                        
            _cachedPosition = position;
            _cachedInventory = inventory;
        }

        private void OnEnterDeleteArea()
        {
            var hasItemInHand = _cachedItem != null;
            _deleteArea.DrawInteractReady(hasItemInHand);
        }
        
        private void OnLeaveDeleteArea() => _deleteArea.DrawInteractReady(false);

        private void OnDropItemToDelete()
        {
            if (_cachedItem == null) return;
            
            _deleteConfirmation.ShowPopup();
        }

        private void OnConfirmDelete()
        {
            _cachedItem = null;
            
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