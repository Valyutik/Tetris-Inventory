using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Inventory.Common
{
    public class InventoryPresenter : InventoryPresenterBase
    {
        public InventoryPresenter(InventoryView view,
            InventoryModel model,
            VisualElement menuRoot) : base(view,
            model,
            menuRoot)
        {
        }

    }
}