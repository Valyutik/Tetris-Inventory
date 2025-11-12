using Runtime.InventorySystem.Inventory;
using UnityEngine;

namespace Runtime.InventorySystem.ItemTooltip
{
    public class ItemTooltipPresenter
    {
        private readonly ItemTooltipView _view;
        private readonly IInventoryPresenter _inventory;
        private readonly IInventoryPresenter _stash;

        public ItemTooltipPresenter(ItemTooltipView view, IInventoryPresenter inventory,
            IInventoryPresenter stash)
        {
            _view = view;
            _inventory = inventory;
            _stash = stash;

            Enable();
        }

        public void Enable()
        {
            _stash.OnPointerEnterCell += ShowTooltip;
            _stash.OnPointerLeaveCell += HideTooltip;
            _inventory.OnPointerEnterCell += ShowTooltip;
            _inventory.OnPointerLeaveCell += HideTooltip;
        }
        
        public void Disable()
        {
            _stash.OnPointerEnterCell -= ShowTooltip;
            _stash.OnPointerLeaveCell -= HideTooltip;
            _inventory.OnPointerEnterCell -= ShowTooltip;
            _inventory.OnPointerLeaveCell -= HideTooltip;
        }

        private void ShowTooltip(Vector2Int position, IInventoryPresenter presenter)
        {
            var item = presenter.GetItem(position);
            
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