using Runtime.InventorySystem.Common;
using System;

namespace Runtime.InventorySystem.ItemGeneration
{
    public sealed class ItemGenerationPresenter
    {
        private readonly ItemGenerationView _view;
        private readonly ItemGenerationModel _model;
        
        public event Action<Item> OnItemGenerated;

        public ItemGenerationPresenter(ItemGenerationView view, ItemGenerationModel model)
        {
            _view = view;
            _model = model;

            _view.OnGenerateClicked += HandleGenerateClicked;
        }

        private void HandleGenerateClicked()
        {
            var item = _model.GetRandomItem();
            OnItemGenerated?.Invoke(item);
        }
    }
}