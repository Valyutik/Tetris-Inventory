using Runtime.Inventory;
using UnityEngine.UIElements;

namespace Runtime.Systems.ContentManager
{
    public class PopupContent
    {
        public VisualElement PopupRoot { get; private set; }

        public PopupContent(UIDocument document)
        {
            PopupRoot = document.rootVisualElement.Q<VisualElement>(InventoryConstants.UI.DeleteConfirmationConst
                .PopupRootTitle);
        }
    }
}