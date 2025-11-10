using Runtime.Systems.ContentManager;
using UnityEngine.UIElements;

namespace Runtime.InventorySystem.DeleteArea
{
    public class DeleteAreaView
    {
        public Button DeleteButton {get;} = MenuContent.MenuRoot.Q<Button>(InventoryConstants.UI.DeleteButton);

        public void DrawInteractReady(bool isReady)
        {
            if (isReady) DeleteButton.AddToClassList(InventoryConstants.UI.DeleteAreaStyle);
            else DeleteButton.RemoveFromClassList(InventoryConstants.UI.DeleteAreaStyle);
        }
    }
}
