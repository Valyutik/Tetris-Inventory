using System;

namespace Runtime.InventorySystem.DeleteConfirmation
{
    public interface IDeleteConfirmation
    {
        event Action OnConfirmDelete;
        event Action OnCancelDelete;

        void Show();
        
        void Hide();
    }
}