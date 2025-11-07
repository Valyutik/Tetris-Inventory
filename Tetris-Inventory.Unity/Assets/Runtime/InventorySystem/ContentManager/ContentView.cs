using UnityEngine.UIElements;

namespace Runtime.InventorySystem.ContentManager
{
    public class ContentView
    {
        private readonly VisualElement _mainRoot;
        
        public ContentView(UIDocument document)
        {
            _mainRoot = document.rootVisualElement.Q<VisualElement>(InventoryConstants.UI.ContentRoot);
        }
        
        public void AddElement(VisualTreeAsset asset, string root)
        {
            var elementRoot = asset.CloneTree().Q<VisualElement>(root);
            _mainRoot.Add(elementRoot);
        }
        
        public void AddElement(VisualTreeAsset asset)
        {
            var elementRoot = asset.CloneTree();
            _mainRoot.Add(elementRoot);
        }

        public void AddElement(VisualElement element)
        {
            _mainRoot.Add(element);
        }

        public void RemoveElement(VisualElement asset)
        {
            asset.RemoveFromHierarchy();
        }

        public void RemoveElement(TemplateContainer container)
        {
            container.RemoveFromHierarchy();
        }
    }
}
