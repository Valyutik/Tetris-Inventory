using UnityEngine.UIElements;
using Runtime.Systems.ContentManager;

namespace Runtime.InventorySystem.ItemGeneration
{
    public sealed class ItemGenerationView
    {
        public Button GenerateButton { get; } = MenuContent.MenuRoot.Q<Button>(InventoryConstants.UI.CreateButton);
    }
}