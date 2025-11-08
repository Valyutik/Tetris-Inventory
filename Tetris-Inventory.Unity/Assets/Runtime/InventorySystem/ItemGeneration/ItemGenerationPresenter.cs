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
        private readonly int _numberItemsGenerated;
        
        public event Action<IEnumerable<Item>> OnItemGenerated;

        public ItemGenerationPresenter(ItemGenerationView view,
            ItemGenerationModel model,
            ItemGenerationRules rules,
            int numberItemsGenerated)
        {
            _view = view;
            _model = model;
            _numberItemsGenerated = numberItemsGenerated;
            _rules = rules;

            _view.OnGenerateClicked += HandleGenerateClicked;
        }

        private void HandleGenerateClicked()
        {
            var items = _model.GetRandomItem(_numberItemsGenerated).ToList();
            if (_rules.CanGenerateItems(items))
            {
                OnItemGenerated?.Invoke(items);
            }
        }
    }
}