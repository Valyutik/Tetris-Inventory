using UnityEngine.UIElements;

namespace Runtime.InventorySystem.ItemGeneration
{
    public sealed class ItemGenerationView
    {
        public Button GenerateButton { get; }

        public ItemGenerationView(VisualElement root, VisualTreeAsset asset)
        {
            var templateContainer = asset.CloneTree();
            GenerateButton = templateContainer.Q<Button>(InventoryConstants.UI.CreateButton);
            root.Add(templateContainer);
        }
    }
}