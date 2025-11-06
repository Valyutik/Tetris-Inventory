using System;
using UnityEngine.UIElements;

namespace Runtime.InventorySystem.DeleteArea
{
    public class DeleteAreaView : IDeleteArea
    {
        public event Action OnDeleteAreaInput;

        public event Action OnEnterDeleteArea;
        
        public event Action OnLeaveDeleteArea;
        
        private const string DeleteAreaStyle = "delete-button-ready";
        
        private readonly VisualElement _root;
        
        public DeleteAreaView(VisualElement root)
        {
            _root = root;
            
            _root.RegisterCallback<PointerUpEvent>(OnPointerUp);
            
            _root.RegisterCallback<PointerEnterEvent>(OnPointerEnter);
            
            _root.RegisterCallback<PointerLeaveEvent>(OnPointerLeave);
        }

        public void DrawInteractReady(bool isReady)
        {
            if (isReady) _root.AddToClassList(DeleteAreaStyle);
            else _root.RemoveFromClassList(DeleteAreaStyle);
        }

        private void OnPointerUp(PointerUpEvent evt)
        {
            OnDeleteAreaInput?.Invoke();
            
            DrawInteractReady(false);
        } 

        private void OnPointerEnter(PointerEnterEvent evt) => OnEnterDeleteArea?.Invoke();

        private void OnPointerLeave(PointerLeaveEvent evt) => OnLeaveDeleteArea?.Invoke();
    }
}
