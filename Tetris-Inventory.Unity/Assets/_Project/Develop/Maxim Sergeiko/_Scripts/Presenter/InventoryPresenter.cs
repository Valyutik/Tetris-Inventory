using System;
using _Project.Develop.Shared._Scripts.View;

namespace _Project.Services
{
    public class InventoryPresenter : IDisposable
    {
        private readonly IInventoryView _inventoryView;

        private readonly IDragDropView _dragDropView;
        
        private readonly IInputService _inputService;
        
        public InventoryPresenter(IInventoryView inventoryView, IDragDropView dragDropView, IInputService inputService)
        {
            _inventoryView = inventoryView;
            
            _inputService = inputService;
        }

        public void Init()
        {
            ApplySubscriptions();
        }

        public void Dispose()
        {
            RemoveSubscriptions();
        }

        private void OnCreateItem()
        {
            _inputService.InputHandler.OnChangePointerPosition += _dragDropView.Drag;
        }

        private void OnTakeItem()
        {

        }

        private void OnPlaceItem()
        {
            _inputService.InputHandler.OnChangePointerPosition -= _dragDropView.Drag;
            
            _dragDropView.Drop();
        }

        private void OnDeleteItem()
        {

        }

        private void ApplySubscriptions()
        {
            _inventoryView.InventoryHandler.OnRequestTakeItem += OnTakeItem;

            _inventoryView.InventoryHandler.OnRequestCreateItem += OnCreateItem;

            _inventoryView.InventoryHandler.OnRequestPlaceItem += OnPlaceItem;

            _inventoryView.InventoryHandler.OnRequestDeleteItem += OnDeleteItem;
        }

        private void RemoveSubscriptions()
        {
            _inventoryView.InventoryHandler.OnRequestTakeItem -= OnTakeItem;

            _inventoryView.InventoryHandler.OnRequestCreateItem -= OnCreateItem;

            _inventoryView.InventoryHandler.OnRequestPlaceItem -= OnPlaceItem;

            _inventoryView.InventoryHandler.OnRequestDeleteItem -= OnDeleteItem;
        }
    }
}