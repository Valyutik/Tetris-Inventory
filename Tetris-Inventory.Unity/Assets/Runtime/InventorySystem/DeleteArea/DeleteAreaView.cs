using System;
using UnityEngine.UIElements;

namespace Runtime.InventorySystem.DeleteArea
{
    public class DeleteAreaView : IDeleteArea
    {
        public event Action OnDeleteAreaInput;
        public event Action OnEnterDeleteArea;
        public event Action OnLeaveDeleteArea;
        
        public bool InDeleteArea { get; private set; }
        
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
            if (isReady) _root.AddToClassList(InventoryConstants.UI.DeleteAreaStyle);
            else _root.RemoveFromClassList(InventoryConstants.UI.DeleteAreaStyle);
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
