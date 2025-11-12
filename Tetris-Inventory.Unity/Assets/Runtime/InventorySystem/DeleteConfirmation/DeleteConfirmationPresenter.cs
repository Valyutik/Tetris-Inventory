using Runtime.Systems.ContentManager;
using System;

namespace Runtime.InventorySystem.DeleteConfirmation
{
    public class DeleteConfirmationPresenter : IDeleteConfirmation
    {
        public event Action OnConfirmDelete;
        public event Action OnCancelDelete;
        
        private readonly DeleteConfirmationView _view;
        
        public DeleteConfirmationPresenter(DeleteConfirmationView view)
        {
            _view = view;
        }

        public void Show()
        {
            PopupContent.PopupRoot.Add(_view.Root);
            
            _view.ConfirmButton.clicked += HandleConfirmButtonClicked;
            _view.CancelButton.clicked += HandleCancelButtonClicked;
        }
        
        public void Hide()
        {
            _view.ConfirmButton.clicked -= HandleCancelButtonClicked;
            _view.CancelButton.clicked -= HandleConfirmButtonClicked;
            
            _view.Root.RemoveFromHierarchy();
        }

        private void HandleCancelButtonClicked()
        {
            OnCancelDelete?.Invoke();
        }

        private void HandleConfirmButtonClicked()
        {
            OnConfirmDelete?.Invoke();
        }
    }
}