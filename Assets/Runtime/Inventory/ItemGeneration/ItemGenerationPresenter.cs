using System.Linq;
using Runtime.Core;

namespace Runtime.Inventory.ItemGeneration
{
    public sealed class ItemGenerationPresenter : IPresenter
    {
        private readonly ItemGenerationView _view;
        private readonly ItemGenerationModel _model;
        private readonly ItemGenerationRules _rules;
        
        public ItemGenerationPresenter(ItemGenerationView view,
            ItemGenerationModel model,
            ItemGenerationRules rules)
        {
            _view = view;
            _model = model;
            _rules = rules;
        }

        public void Enable()
        {
            _view.GenerateButton.clicked += HandleGenerateClicked;
        }

        public void Disable()
        {
            _view.GenerateButton.clicked -= HandleGenerateClicked;
        }

        private void HandleGenerateClicked()
        {
            var items = _model.GetRandomItems().ToList();
            
            if (_rules.CanGenerateItems(items))
            {
                _model.GenerateItem(items);
            }
        }
    }
}