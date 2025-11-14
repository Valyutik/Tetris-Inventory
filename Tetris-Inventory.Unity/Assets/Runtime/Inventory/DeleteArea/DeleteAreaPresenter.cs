using System;
using Runtime.Inventory.Common;
using Runtime.Inventory.Core;
using Runtime.Inventory.DragAndDrop;
using UnityEngine.UIElements;

namespace Runtime.Inventory.DeleteArea
{
    public class DeleteAreaPresenter : IPresenter
    {
        private readonly DeleteAreaView _view;
        
        private readonly DragDropModel _dragDropModel;
        
        private Item _cachedItem;
        
        private InventoryModel _cachedInventory;

        public DeleteAreaPresenter(DeleteAreaView view, ModelStorage modelStorage)
        {
            _view = view;
            
            _dragDropModel = modelStorage.DragDropModel;
        }

        public void Enable()
        {
            _view.DeleteArea.RegisterCallback<PointerUpEvent>(OnPointerUp);
            
            _view.DeleteArea.RegisterCallback<PointerEnterEvent>(OnPointerEnter);
            
            _view.DeleteArea.RegisterCallback<PointerLeaveEvent>(OnPointerLeave);

            _view.ConfirmDeleteButton.clicked += DeleteItem;
            
            _view.CancelDeleteButton.clicked += Cancel;
        }

        public void Disable()
        {
            _view.DeleteArea.UnregisterCallback<PointerUpEvent>(OnPointerUp);
            
            _view.DeleteArea.UnregisterCallback<PointerEnterEvent>(OnPointerEnter);
            
            _view.DeleteArea.UnregisterCallback<PointerLeaveEvent>(OnPointerLeave);
            
            _view.ConfirmDeleteButton.clicked -= DeleteItem;
            
            _view.CancelDeleteButton.clicked -= Cancel;
        }

        private void OnPointerUp(PointerUpEvent evt)
        {
            if (_dragDropModel.CurrentItem == null) return;
            
            _view.DrawInteractReady(false);
            
            _view.ShowConfirmation();
        }

        private void OnPointerEnter(PointerEnterEvent evt)
        {
            _view.DrawInteractReady(_dragDropModel.CurrentItem != null);

            if (_dragDropModel.CurrentItem != null)
            {
                _cachedItem = _dragDropModel.CurrentItem;
                
                _dragDropModel.CanProjectionPlacementInteract = true;
                
                _cachedInventory = _dragDropModel.StartInventory;
            }
        }

        private void OnPointerLeave(PointerLeaveEvent evt)
        {
            _view.DrawInteractReady(false);
            
            _dragDropModel.CanProjectionPlacementInteract = false;
        }

        private void DeleteItem()
        {
            _cachedInventory.TryRemoveItem(_cachedItem);

            _cachedInventory = null;
            
            _cachedItem = null;
            
            _view.HideConfirmation();
            
            _view.DrawInteractReady(false);
        }

        private void Cancel()
        {
            _view.HideConfirmation();
        }
    }
}