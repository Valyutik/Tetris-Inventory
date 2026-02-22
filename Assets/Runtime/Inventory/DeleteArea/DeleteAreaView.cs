using Runtime.Systems.ContentManager;
using UnityEngine.UIElements;

namespace Runtime.Inventory.DeleteArea
{
    public class DeleteAreaView
    {
        public Button DeleteArea { get; }
        
        public Button ConfirmDeleteButton { get;  }
        
        public Button CancelDeleteButton { get;  }
        
        private VisualElement Root { get; }
       
        private readonly PopupContent _popupContent;
        
        public DeleteAreaView(VisualElement menuRoot, VisualTreeAsset asset, PopupContent popupContent)
        {
            DeleteArea = menuRoot.Q<Button>(InventoryConstants.UI.DeleteButton);
            
            _popupContent = popupContent;
            
            Root = asset.CloneTree();

            ConfirmDeleteButton = Root.Q<Button>(InventoryConstants.UI.DeleteConfirmationConst.ConfirmButtonTitle);
            
            CancelDeleteButton = Root.Q<Button>(InventoryConstants.UI.DeleteConfirmationConst.CancelButtonTitle);
        }


        public void DrawInteractReady(bool isReady)
        {
            if (isReady)
                DeleteArea.AddToClassList(InventoryConstants.UI.DeleteAreaStyle);
            else
                DeleteArea.RemoveFromClassList(InventoryConstants.UI.DeleteAreaStyle);
        }

        public void ShowConfirmation()
        {
            _popupContent.PopupRoot.Add(Root);
        }

        public void HideConfirmation()
        {
            Root.RemoveFromHierarchy();
        }
    }
}
