using UnityEngine.UIElements;
using static Runtime.Inventory.InventoryConstants.UI;

namespace Runtime.Inventory.DeleteConfirmation
{
    public class DeleteConfirmationView
    {
        public VisualElement Root {get;}
        public Button ConfirmButton { get; }
        public Button CancelButton { get; }
        
        public DeleteConfirmationView(VisualTreeAsset asset)
        {
            Root = asset.CloneTree();

            ConfirmButton = Root.Q<Button>(DeleteConfirmationConst.ConfirmButtonTitle);
            CancelButton = Root.Q<Button>(DeleteConfirmationConst.CancelButtonTitle);
        }
    }
}