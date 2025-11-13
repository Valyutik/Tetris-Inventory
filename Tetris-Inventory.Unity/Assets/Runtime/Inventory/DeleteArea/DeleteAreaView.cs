using UnityEngine.UIElements;

namespace Runtime.Inventory.DeleteArea
{
    public class DeleteAreaView
    {
        public Button DeleteButton {get;}
        
        public DeleteAreaView(VisualElement menuRoot)
        {
            DeleteButton = menuRoot.Q<Button>(InventoryConstants.UI.DeleteButton);
        }
        
        public void DrawInteractReady(bool isReady)
        {
            if (isReady) DeleteButton.AddToClassList(InventoryConstants.UI.DeleteAreaStyle);
            else DeleteButton.RemoveFromClassList(InventoryConstants.UI.DeleteAreaStyle);
        }
    }
}
