using System;
using Runtime.Systems.ContentManager;

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