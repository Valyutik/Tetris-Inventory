using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Inventory.Common;

namespace Runtime.Inventory.ItemGeneration
{
    public sealed class ItemGenerationPresenter : IItemGenerationPresenter, IDisposable
    {
        private readonly ItemGenerationView _view;
        private readonly ItemGenerationModel _model;
        private readonly ItemGenerationRules _rules;
        
        public event Action<IEnumerable<Item>> OnItemGenerated;

        public ItemGenerationPresenter(ItemGenerationView view,
            ItemGenerationModel model,
            ItemGenerationRules rules)
        {
            _view = view;
            _model = model;
            _rules = rules;

            Enable();
        }

        public void Enable()
        {
            _view.GenerateButton.clicked += HandleGenerateClicked;
        }
        
        public void Dispose()
        {
            _view.GenerateButton.clicked -= HandleGenerateClicked;
        }

        private void HandleGenerateClicked()
        {
            var items = _model.GetRandomItems().ToList();
            
            if (_rules.CanGenerateItems(items))
            {
                OnItemGenerated?.Invoke(items);
            }
        }
    }
}