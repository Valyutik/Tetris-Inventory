using UnityEngine.UIElements;
using System;

namespace Runtime.InventorySystem.ItemGeneration
{
    public sealed class ItemGenerationView : IDisposable
    {
        private readonly Button _generateButton;

        public event Action OnGenerateClicked;
        
        public ItemGenerationView(VisualElement root)
        {
            _generateButton = root.Q<Button>(InventoryConstants.UI.CreateButton);

            if (_generateButton != null)
            {
                _generateButton.clickable.clicked += HandleGenerateClick;
            }
        }

        private void HandleGenerateClick()  
        {
            OnGenerateClicked?.Invoke();
        }

        public void Dispose()
        {
            _generateButton.clickable.clicked -= HandleGenerateClick;
        }
    }
}