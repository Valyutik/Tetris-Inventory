using System;

namespace Runtime.Inventory.DeleteArea
{
    public interface IDeleteArea
    {
        event Action OnDeleteAreaInput;
        event Action OnEnterDeleteArea;
        event Action OnLeaveDeleteArea;

        bool InDeleteArea { get; } 
        void DrawInteractReady(bool isReady);
    }
}