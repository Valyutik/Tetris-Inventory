using System;

namespace Runtime.InventorySystem.DeleteConfirmation
{
    public interface IDeleteConfirmation
    {
        event Action OnConfirmDelete;
        event Action OnCancelDelete;

        void ShowPopup();
        
        void HidePopup();
    }
}