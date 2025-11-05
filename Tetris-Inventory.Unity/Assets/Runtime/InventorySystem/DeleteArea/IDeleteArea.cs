using System;

namespace Runtime.InventorySystem.DeleteArea
{
    public interface IDeleteArea
    {
        event Action OnDeleteAreaInput;
        event Action OnEnterDeleteArea;
        event Action OnLeaveDeleteArea;
        
        void DrawInteractReady(bool isReady);
        
    }
}