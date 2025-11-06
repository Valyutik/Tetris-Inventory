using System;
using UnityEngine.UIElements;

namespace Runtime.InventorySystem.DeleteConfirmation
{
    public class DeleteConfirmationView : IDeleteConfirmation
    {
        public event Action OnConfirmDelete;
        public event Action OnCancelDelete;
        
        private readonly VisualElement _root;
        private readonly VisualTreeAsset _panelAsset;
        private TemplateContainer _deletePopup;
        
        public DeleteConfirmationView(UIDocument document, VisualTreeAsset panel)
        {
            _root = document.rootVisualElement.Q<VisualElement>(DeleteConfirmationConst.popupRootTitle);
            _panelAsset =  panel;
        }

        public void ShowPopup()
        {
            _root.style.display = DisplayStyle.Flex;
            
            _deletePopup = _panelAsset.CloneTree();   
            
            var _confirmButton = _deletePopup.Q<Button>(DeleteConfirmationConst.confirmButtonTitle);
            var _cancelButton = _deletePopup.Q<Button>(DeleteConfirmationConst.cancelButtonTitle);
            
            _confirmButton.clicked += () => OnConfirmDelete?.Invoke();
            _cancelButton.clicked += () => OnCancelDelete?.Invoke();
            
            _root.Add(_deletePopup);
        }

        public void HidePopup()
        {
            _deletePopup?.RemoveFromHierarchy();
            
            _root.style.display = DisplayStyle.None;
        }
    }
}