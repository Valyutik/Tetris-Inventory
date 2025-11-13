using System;

namespace Runtime.Inventory.DeleteConfirmation
{
    public interface IDeleteConfirmation
    {
        event Action OnConfirmDelete;
        event Action OnCancelDelete;

        void Show();
        
        void Hide();
    }
}