using UnityEngine.UIElements;

namespace Runtime.InventorySystem.ItemGeneration
{
    public sealed class ItemGenerationView
    {
        public Button GenerateButton { get; }

        public ItemGenerationView(VisualElement menuRoot)
        {
            GenerateButton = menuRoot.Q<Button>(InventoryConstants.UI.CreateButton);
        }
    }
}