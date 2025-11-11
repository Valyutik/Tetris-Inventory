using Runtime.InventorySystem;
using UnityEngine.UIElements;

namespace Runtime.Systems.ContentManager
{
    public class MenuContent
    {
        public VisualElement MenuRoot {get; private set;}
        
        public MenuContent(UIDocument document)
        {
            MenuRoot = document.rootVisualElement.Q<VisualElement>(InventoryConstants.UI.ContentRoot);
        }
    }
}