using Runtime.InventorySystem.Common;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Runtime.InventorySystem.ItemGeneration
{
    public sealed class ItemGenerationPresenter
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
            
            _view.GenerateButton.clicked += HandleGenerateClicked;
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