using Runtime.InventorySystem.Common;
using System.Collections.Generic;
using System;

namespace Runtime.InventorySystem.ItemGeneration
{
    public sealed class ItemGenerationPresenter
    {
        private readonly ItemGenerationView _view;
        private readonly ItemGenerationModel _model;
        private readonly int _numberItemsGenerated;
        
        public event Action<IEnumerable<Item>> OnItemGenerated;

        public ItemGenerationPresenter(ItemGenerationView view, ItemGenerationModel model, int numberItemsGenerated)
        {
            _view = view;
            _model = model;
            _numberItemsGenerated = numberItemsGenerated;

            _view.OnGenerateClicked += HandleGenerateClicked;
        }

        private void HandleGenerateClicked()
        {
            var item = _model.GetRandomItem(_numberItemsGenerated);
            OnItemGenerated?.Invoke(item);
        }
    }
}