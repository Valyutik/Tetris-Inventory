using static Runtime.InventorySystem.InventoryConstants.UI;
using UnityEngine.UIElements;

namespace Runtime.InventorySystem.DeleteConfirmation
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