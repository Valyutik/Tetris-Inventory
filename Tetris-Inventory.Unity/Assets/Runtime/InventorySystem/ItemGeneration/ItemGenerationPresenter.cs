using Runtime.InventorySystem.Common;
using System;

namespace Runtime.InventorySystem.ItemGeneration
{
    public sealed class ItemGenerationPresenter
    {
        private readonly ItemGenerationView _view;
        private readonly ItemGenerationModel _model;
        private readonly int _numberItemsGenerated;
        
        public event Action<Item> OnItemGenerated;

        public ItemGenerationPresenter(ItemGenerationView view, ItemGenerationModel model, int numberItemsGenerated)
        {
            _view = view;
            _model = model;
            _numberItemsGenerated = numberItemsGenerated;

            _view.OnGenerateClicked += HandleGenerateClicked;
        }

        private void HandleGenerateClicked()
        {
            for (var i = 0; i < _numberItemsGenerated; i++)
            {
                var item = _model.GetRandomItem();
                OnItemGenerated?.Invoke(item);
            }
        }
    }
}