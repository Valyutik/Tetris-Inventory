using System;
using Runtime.Systems.ContentManager;

namespace Runtime.InventorySystem.DeleteConfirmation
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
            
            _view.ConfirmButton.clicked += OnConfirmDelete.Invoke;
            _view.CancelButton.clicked += OnCancelDelete.Invoke;
        }

        public void Hide()
        {
            _view.ConfirmButton.clicked -= OnConfirmDelete.Invoke;
            _view.CancelButton.clicked -= OnCancelDelete.Invoke;
            
            _view.Root.RemoveFromHierarchy();
        }
    }
}