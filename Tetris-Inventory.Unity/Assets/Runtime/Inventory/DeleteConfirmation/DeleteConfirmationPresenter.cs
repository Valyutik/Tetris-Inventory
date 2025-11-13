using System;
using Runtime.Systems.ContentManager;

namespace Runtime.Inventory.DeleteConfirmation
{
    public class DeleteConfirmationPresenter : IDeleteConfirmation
    {
        public event Action OnConfirmDelete;
        public event Action OnCancelDelete;
        
        private readonly DeleteConfirmationView _view;
        private readonly PopupContent _popupContent;
        
        public DeleteConfirmationPresenter(DeleteConfirmationView view, PopupContent popupContent)
        {
            _view = view;
            _popupContent = popupContent;
        }

        public void Show()
        {
            _popupContent.PopupRoot.Add(_view.Root);
            
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