using UnityEngine.UIElements;
using UnityEngine;
using System;

namespace Runtime.InventorySystem.ItemGeneration
{
    public sealed class ItemGenerationView : MonoBehaviour, IDisposable
    {
        private readonly VisualElement _root;
        private readonly Button _generateButton;

        public event Action OnGenerateClicked;
        
        public ItemGenerationView(VisualElement root)
        {
            _root = root;
            _generateButton = root.Q<Button>("ItemGenerateButton");

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