using System;
using UnityEngine.UIElements;

namespace Runtime.InventorySystem.DeleteArea
{
    public class DeleteAreaPresenter : IDeleteArea
    {
        public event Action OnDeleteAreaInput;
        public event Action OnEnterDeleteArea;
        public event Action OnLeaveDeleteArea;
        public bool InDeleteArea { get; private set; }
        public void DrawInteractReady(bool isReady) => _view.DrawInteractReady(isReady);

        private readonly DeleteAreaView _view;
        
        public DeleteAreaPresenter(DeleteAreaView view)
        {
            _view = view;

            SetUpListeners();
        }

        private void SetUpListeners()
        {
            _view.DeleteButton.RegisterCallback<PointerUpEvent>(OnPointerUp);
            _view.DeleteButton.RegisterCallback<PointerEnterEvent>(OnPointerEnter);
            _view.DeleteButton.RegisterCallback<PointerLeaveEvent>(OnPointerLeave);
        }
        
        private void OnPointerUp(PointerUpEvent evt)
        {
            OnDeleteAreaInput?.Invoke();
            
            DrawInteractReady(false);
        }

        private void OnPointerEnter(PointerEnterEvent evt)
        {
            InDeleteArea = true;
            
            OnEnterDeleteArea?.Invoke();   
        }

        private void OnPointerLeave(PointerLeaveEvent evt)
        {
            InDeleteArea = false;
            
            OnLeaveDeleteArea?.Invoke();
        }
    }
}