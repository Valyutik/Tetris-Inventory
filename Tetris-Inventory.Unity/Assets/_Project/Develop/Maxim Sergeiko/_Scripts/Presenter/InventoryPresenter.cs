using System;
using _Project.Develop.Shared._Scripts.View;

namespace _Project.Services
{
    public class InventoryPresenter : IDisposable
    {
        private readonly IInventoryView _view;

        public InventoryPresenter(IInventoryView view)
        {
            _view = view;
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

        }

        private void OnTakeItem()
        {

        }

        private void OnPlaceItem()
        {

        }

        private void OnDeleteItem()
        {

        }

        private void ApplySubscriptions()
        {
            _view.InventoryHandler.OnRequestTakeItem += OnTakeItem;

            _view.InventoryHandler.OnRequestCreateItem += OnCreateItem;

            _view.InventoryHandler.OnRequestPlaceItem += OnPlaceItem;

            _view.InventoryHandler.OnRequestDeleteItem += OnDeleteItem;
        }

        private void RemoveSubscriptions()
        {
            _view.InventoryHandler.OnRequestTakeItem -= OnTakeItem;

            _view.InventoryHandler.OnRequestCreateItem -= OnCreateItem;

            _view.InventoryHandler.OnRequestPlaceItem -= OnPlaceItem;

            _view.InventoryHandler.OnRequestDeleteItem -= OnDeleteItem;
        }
    }
}