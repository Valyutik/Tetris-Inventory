using Runtime.Inventory.Common;
using Runtime.Inventory.Core;
using UnityEngine;

namespace Runtime.Inventory.ItemTooltip
{
    public class ItemTooltipPresenter : IPresenter
    {
        private readonly ItemTooltipView _view;
        private readonly InventoryModel _inventory;
        private readonly InventoryModel _stash;

        public ItemTooltipPresenter(ItemTooltipView view, ModelStorage modelStorage)
        {
            _view = view;
            _inventory = modelStorage.CoreInventoryModel;
            _stash = modelStorage.StashInventoryModel;
        }

        public void Enable()
        {
            _stash.OnSelectCell += ShowTooltip;
            _stash.OnDeselectCell += HideTooltip;

            _inventory.OnSelectCell += ShowTooltip;
            _inventory.OnDeselectCell += HideTooltip;
        }
        
        public void Disable()
        {
            _stash.OnSelectCell -= ShowTooltip;
            _stash.OnDeselectCell -= HideTooltip;
            
            _inventory.OnSelectCell -= ShowTooltip;
            _inventory.OnDeselectCell -= HideTooltip;
        }

        private void ShowTooltip(Vector2Int position, InventoryModel inventory)
        {
            var item = inventory.GetItem(position);
            
            if (item == null)
            {
                return;
            }

            _view.TooltipTitle.text = item.Name;
            _view.TooltipDescription.text = item.Description;
            
            _view.Show();
        }

        private void HideTooltip()
        {
            _view.Hide();
        }
    }
}